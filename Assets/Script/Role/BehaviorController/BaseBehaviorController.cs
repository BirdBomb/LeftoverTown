using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Unity.VisualScripting;
using Vector3 = UnityEngine.Vector3;
/// <summary>
/// ��Ϊ������
/// </summary>
public class BaseBehaviorController : MonoBehaviour
{
    [Header("���������")]
    public BaseBodyController bodyController;
    [Header("�ֲ�������")]
    public ItemBase holdingByHand = new ItemBase();

    [SerializeField]
    public RoleData Data = new RoleData();
    private void Start()
    {
        navManager = GameObject.Find("Grid").GetComponent<NavManager>();
        MessageBroker.Default.Receive<GameEvent.GameEvent_SomeoneMove>().Subscribe(_ =>
        {
            ListenRoleMove(_.moveRole,_.moveTile);
        }).AddTo(this);
    }
    /*�ƶ�*/
    #region
    [Header("������ڵ�")]
    public Transform Root;
    private MyTile curTile;
    private MyTile lastTile;
    public virtual void TurnLeft()
    {
        bodyController.Leg.localScale = new Vector3(-1, 1, 1);
    }
    public virtual void FaceLeft()
    {
        bodyController.Head.localScale = new Vector3(-1, 1, 1);
        bodyController.Body.localScale = new Vector3(-1, 1, 1);
        bodyController.Hand.localScale = new Vector3(-1, 1, 1);
    }
    public virtual void TurnRight()
    {
        bodyController.Leg.localScale = new Vector3(1, 1, 1);
    }
    public virtual void FaceRight()
    {
        bodyController.Head.localScale = new Vector3(1, 1, 1);
        bodyController.Body.localScale = new Vector3(1, 1, 1);
        bodyController.Hand.localScale = new Vector3(1, 1, 1);
    }
    public virtual void SpeedUp(bool up)
    {
        speedUp_State = up;
    }
    public virtual void InputMoveVector(UnityEngine.Vector2 vector2,float dt)
    {
        tempVector += vector2;
    }
    public virtual void InputFaceVector(Vector3 mousePostion)
    {
        if (Camera.main.ScreenToWorldPoint(mousePostion).x > transform.position.x)
        {
            FaceRight();
        }
        else
        {
            FaceLeft();
        }
    }
    /// <summary>
    /// ���շ����ƶ�
    /// </summary>
    /// <param name="vector2"></param>
    /// <param name="dt"></param>
    private Vector2 tempVector;
    private bool speedUp_State;
    private float speedUp_Val = 2f;
    public virtual void MoveByVector(float dt)
    {
        tempVector = tempVector.normalized;
        if (speedUp_State) { tempVector *= speedUp_Val; }
        transform.position =
        transform.position + new UnityEngine.Vector3(tempVector.x * Data.Data_Speed * dt, tempVector.y * Data.Data_Speed * dt, 0);

        if (Vector2.Distance(Vector2.zero,tempVector) > 0.2f)
        {
            float val = Data.Data_Speed;
            if (speedUp_State) { val *= speedUp_Val; }
            bodyController.PlayBodyAction(BodyAction.Move, val, null);
            bodyController.PlayHeadAction(HeadAction.Move, val, null);
            bodyController.PlayLegAction(LegAction.Step, val);
            bodyController.PlayHandAction(HandAction.Step, val, null);
        }
        else
        {
            float val = Data.Data_Speed;
            if (speedUp_State) { val *= speedUp_Val; }
            bodyController.PlayBodyAction(BodyAction.Idle, val, null);
            bodyController.PlayHeadAction(HeadAction.Idle, val, null);
            bodyController.PlayLegAction(LegAction.Idle, val);
            bodyController.PlayHandAction(HandAction.Idle, val, null);
        }

        curTile = GetMyTile();
        if (lastTile == null) { lastTile = curTile; }
        if (lastTile.pos != curTile.pos)
        {
            lastTile = curTile;
            MessageBroker.Default.Publish(new GameEvent.GameEvent_SomeoneMove
            {
                moveRole = this,
                moveTile = curTile
            });
        }
        tempVector = Vector3.zero;
    }
    /// <summary>
    /// ʣ�ಽ��
    /// </summary>
    private int tempStep = 0;
    /// <summary>
    /// ����·���ƶ�
    /// </summary>
    /// <param name="path"></param>
    /// <param name="callBack"></param>
    public virtual void MoveByPath(List<MyTile> path)
    {
        if ( path != null && path.Count != 0)
        {
            transform.DOKill();
            MyTile next = path[0];
            path.Remove(next);
            transform.DOMove(new(next.pos.x, next.pos.y, 0),1f / Data.Data_Speed).SetEase(Ease.Linear).OnComplete(() =>
            {
                if (tempStep > 0)
                {
                    tempStep--;
                    if (tempStep == 0) { return; }
                }
                curTile = GetMyTile();
                if (lastTile != curTile)
                {
                    lastTile = curTile;
                }

                MessageBroker.Default.Publish(new GameEvent.GameEvent_SomeoneMove
                {
                    moveRole = this,
                    moveTile = curTile
                });
                MoveByPath(path);
            });
        }
    }
    #endregion
    /*Ѱ·*/
    #region
    public List<MyTile> myLoad = new List<MyTile>();

    public NavManager navManager;
    public MyTile GetMyTile()
    {
        Vector3Int vector3Int = navManager.grid.WorldToCell(transform.position);
        return (MyTile)navManager.tilemap.GetTile(vector3Int);
    }
    List<MyTile> tempTiles = new List<MyTile>();
    public List<MyTile> GetNearbyTile()
    {
        tempTiles.Clear();
        MyTile tile = GetMyTile();

        tempTiles.Add((MyTile)navManager.tilemap.GetTile(new Vector3Int(tile.x, tile.y, 0)));
        tempTiles.Add((MyTile)navManager.tilemap.GetTile(new Vector3Int(tile.x + 1, tile.y, 0)));
        tempTiles.Add((MyTile)navManager.tilemap.GetTile(new Vector3Int(tile.x - 1, tile.y, 0)));
        tempTiles.Add((MyTile)navManager.tilemap.GetTile(new Vector3Int(tile.x, tile.y + 1, 0)));
        tempTiles.Add((MyTile)navManager.tilemap.GetTile(new Vector3Int(tile.x, tile.y - 1, 0)));
        return tempTiles;
    }

    public Vector2 GetMyPos()
    {
        int X = (int)(transform.position.x);
        int Y = (int)(transform.position.y);
        return new Vector2(X,Y);
    }
    public void FindPathByPos(Vector2 to,Vector2 from,int Step = -1)
    {
        if (myLoad != null)
        {
            myLoad.Clear();
        }
        myLoad = navManager.FindPath(navManager.FindTileByPos(to), navManager.FindTileByPos(from));
        UpdatePath(Step);
    }
    public void UpdatePath(int Step)
    {
        tempStep = Step;
        MoveByPath(myLoad);
    }
    #endregion
    /*����*/
    #region
    /// <summary>
    /// ʱ�����
    /// </summary>
    public virtual void ListenTimeUpdate()
    {

    }
    /// <summary>
    /// λ�ø���
    /// </summary>
    /// <param name="who"></param>
    /// <param name="where"></param>
    public virtual void ListenRoleMove(BaseBehaviorController who,MyTile where)
    {
        
    }
    #endregion
    /*��Ϊ*/
    #region
    /// <summary>
    /// ��������������
    /// </summary>
    public void HoldItem(ItemConfig config)
    {
        Type type = Type.GetType("Item_" + config.Item_ID.ToString());
        holdingByHand = (ItemBase)Activator.CreateInstance(type);
        holdingByHand.Init(config);
        holdingByHand.BeHolding(this, bodyController.Hand_Right);
    }
    /// <summary>
    /// ʹ�����ϵ�����
    /// </summary>
    public void UseItem(ItemConfig config)
    {
        
    }
    /// <summary>
    /// �������
    /// </summary>
    public void AddItem(ItemConfig config)
    {
        Data.Holding_BagList.Add(config);
    }
    /// <summary>
    /// ʧȥ����
    /// </summary>
    public void SubItem(ItemConfig config)
    {
        Data.Holding_BagList.Remove(config);
    }
    #endregion
}
