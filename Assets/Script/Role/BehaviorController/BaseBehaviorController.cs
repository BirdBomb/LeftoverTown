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
    [Header("����ָʾ��")]
    public SI_Sector skillSector;
    [Header("�ֲ�������")]
    public ItemBase holdingByHand = new ItemBase();
    private Vector2 faceTo = Vector2.zero;
    [SerializeField]
    public RoleData Data = new RoleData();
    public bool isPlayer = false; 
    private void Start()
    {
        navManager = GameObject.Find("Furniture").GetComponent<NavManager>();
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
    public virtual void InputFaceVector(Vector3 face)
    {
        faceTo = face;
        if (faceTo.x > 0)
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
        if (!curTile) { return; }
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
        if (navManager != null)
        {
            Vector3Int vector3Int = navManager.grid.WorldToCell(transform.position);
            return (MyTile)navManager.tilemap.GetTile(vector3Int);
        }
        else
        {
            return null;
        }
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
    public void FindPath(Vector2 to,Vector2 from,int Step = -1)
    {
        if (myLoad != null)
        {
            myLoad.Clear();
        }
        myLoad = navManager.FindPath(navManager.FindTileByPos(to), navManager.FindTileByPos(from));
        UpdatePath(Step);
        void UpdatePath(int Step)
        {
            tempStep = Step;
            MoveByPath(myLoad);
        }
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
    public void PickUpItem_Bag(ItemObj itemObj)
    {
        Debug.Log("PickUp");
        bodyController.PlayHeadAction( HeadAction.LowerHead, 1, null);
        bodyController.PlayHandAction( HandAction.PickUp,1,
            (string x) => 
            {
                Debug.Log("OK"+x);
                if (x == "PickUp")
                {
                    Debug.Log("OKthan");
                    itemObj.PickUp(this);
                }
            });
    }
    public void AddItem_Hand(ItemConfig config)
    {
        Type type = Type.GetType("Item_" + config.Item_ID.ToString());
        holdingByHand = (ItemBase)Activator.CreateInstance(type);
        holdingByHand.Init(config);
        holdingByHand.BeHolding(this, bodyController.Hand_Right);
    }
    public void UseItem_Hand(ItemConfig config)
    {
        
    }
    public void SubItem_Hand(ItemConfig config)
    {

    }
    public void AddItem_Bag(ItemConfig config)
    {
        /*����������������Ϊ��*/
        if(config.Item_CurCount <= 0) { config.Item_CurCount = 1; }
        /*��������*/
        for(int i = 0; i < Data.Holding_BagList.Count; i++)
        {
            /*��鱳������ͬ������*/
            if (Data.Holding_BagList[i].Item_ID == config.Item_ID)
            {
                /*��鱳�����ͬ�������Ƿ�ﵽ�ѵ����ֵ*/
                if (Data.Holding_BagList[i].Item_CurCount < Data.Holding_BagList[i].Item_MaxCount)
                {
                    ItemConfig tempItem = Data.Holding_BagList[i];
                    /*ʣ�����ֵ�����������ĵ���*/
                    if (Data.Holding_BagList[i].Item_MaxCount - Data.Holding_BagList[i].Item_CurCount < config.Item_CurCount)
                    {
                        tempItem.Item_CurCount = tempItem.Item_MaxCount;
                        config.Item_CurCount -= (Data.Holding_BagList[i].Item_MaxCount - Data.Holding_BagList[i].Item_CurCount);
                        Data.Holding_BagList[i] = tempItem;
                    }
                    /*ʣ�����ֵ���������ĵ���*/
                    else
                    {
                        tempItem.Item_CurCount += config.Item_CurCount;
                        config.Item_CurCount = 0; 
                        Data.Holding_BagList[i] = tempItem;
                    }
                }
                else
                {
                    continue;
                }
            }
        }
        /*����һ��֮����ʣ��*/
        if(config.Item_CurCount > 0)
        {
            Data.Holding_BagList.Add(config);
        }
        if (isPlayer)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_UI_UpdateItemInBag()
            {
                itemConfigs = Data.Holding_BagList
            });
        }

    }
    public void SubItem_Bag(ItemConfig config)
    {
        Data.Holding_BagList.Remove(config);
    }
    #endregion
}
