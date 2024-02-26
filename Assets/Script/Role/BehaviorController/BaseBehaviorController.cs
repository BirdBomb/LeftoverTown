using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Unity.VisualScripting;
using Vector3 = UnityEngine.Vector3;
using static UnityEditor.Progress;
using Fusion;
using static PlayerNetController;
/// <summary>
/// ��Ϊ������
/// </summary>
public class BaseBehaviorController : NetworkBehaviour
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
    [HideInInspector]
    public bool isPlayer = false;
    private void Start()
    {
        navManager = GameObject.Find("Furniture").GetComponent<NavManager>();
        MessageBroker.Default.Receive<GameEvent.GameEvent_SomeoneMove>().Subscribe(_ =>
        {
            ListenRoleMove(_.moveRole,_.moveTile);
        }).AddTo(this);
    }
    public virtual void FixedUpdate()
    {
        if (tempPath != null)
        {
            MoveByPath(Time.fixedDeltaTime);
        }
    }
    /*�ƶ�*/
    #region
    [Header("������ڵ�")]
    public Transform Root;
    /// <summary>
    /// �ƶ�����
    /// </summary>
    private Vector2 tempVector;
    /// <summary>
    /// ��ǰλ��
    /// </summary>
    private MyTile curTile;
    /// <summary>
    /// ֮ǰλ��
    /// </summary>
    private MyTile lastTile;
    /// <summary>
    /// ����״̬
    /// </summary>
    private bool speedUp_State;
    /// <summary>
    /// ��������
    /// </summary>
    private float speedUp_Val = 2f;
    [HideInInspector, Header("��ʱ·��")]
    public List<MyTile> tempPathList = new List<MyTile>();
    [HideInInspector, Header("��һ��·����")]
    public MyTile tempPath = null;

    public virtual void InputMoveVector(UnityEngine.Vector2 vector2, float dt)
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
    /// ����
    /// </summary>
    /// <param name="up"></param>
    public virtual void SpeedUp(bool up)
    {
        speedUp_State = up;
    }
    /// <summary>
    /// ����·��
    /// </summary>
    public virtual void UpdatePath(List<MyTile> path)
    {
        tempPathList.Clear();
        for(int i = 1; i < path.Count; i++)
        {
            tempPathList.Add(path[i]);
            if (i == 1) { tempPath = tempPathList[0]; }
        }
    }
    /// <summary>
    /// ���շ����ƶ�
    /// </summary>
    /// <param name="dt"></param>
    public virtual void MoveByVector(float dt)
    {
        Move(tempVector, Data.Data_Speed, dt);
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
    /// ����·���ƶ�
    /// </summary>
    public virtual void MoveByPath(float dt)
    {
        /*����Ƿ�ﵽ��·����*/
        if (Vector2.Distance(tempPath.pos,transform.position) <= 0.1f)
        {
            Move(Vector2.zero, Data.Data_Speed, dt);
            tempPath = null;
            if (tempPathList.Count > 0)
            {
                tempPathList.RemoveAt(0);
                if(tempPathList.Count > 0)
                {
                    tempPath = tempPathList[0];
                }
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
            return;
        }
        else
        {
            Vector2 temp = Vector2.zero;
            if (transform.position.x > tempPath.x)
            {
                if((transform.position.x - tempPath.x) > 0.05f)
                {
                    temp += new Vector2(-1, 0);
                }
            }
            else
            {
                if ((transform.position.x - tempPath.x) < -0.05f)
                {
                    temp += new Vector2(1, 0);
                }
            }
            if(transform.position.y > tempPath.y)
            {
                if ((transform.position.y - tempPath.y) > 0.05f)
                {
                    temp += new Vector2(0, -1);
                }
            }
            else
            {
                if ((transform.position.y - tempPath.y) < -0.05f)
                {
                    temp += new Vector2(0, 1);
                }
            }
            Move(temp, Data.Data_Speed, dt);
            if (Vector2.Distance(tempPath.pos, transform.position) <= 0.1f)
            {
                Move(Vector2.zero, Data.Data_Speed, dt);
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
            }
        }
    }
    /// <summary>
    /// �ƶ�
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="speed"></param>
    /// <param name="dt"></param>
    private void Move(Vector2 dir,float speed,float dt)
    {
        dir = dir.normalized;
        if (speedUp_State) { speed *= speedUp_Val; }

        transform.position =
            transform.position + new UnityEngine.Vector3(dir.x * speed * dt, dir.y * speed * dt, 0);

        if (Vector2.Distance(Vector2.zero, dir) > 0.2f)
        {
            bodyController.PlayBodyAction(BodyAction.Move, speed, null);
            bodyController.PlayHeadAction(HeadAction.Move, speed, null);
            bodyController.PlayLegAction(LegAction.Step, speed);
            bodyController.PlayHandAction(HandAction.Step, speed, null);
        }
        else
        {
            bodyController.PlayBodyAction(BodyAction.Idle, speed, null);
            bodyController.PlayHeadAction(HeadAction.Idle, speed, null);
            bodyController.PlayLegAction(LegAction.Idle, speed);
            bodyController.PlayHandAction(HandAction.Idle, speed, null);
        }
        if (dir.x > 0)
        {
            TurnRight();
        }
        if (dir.x < 0)
        {
            TurnLeft();
        }
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
        bodyController.PlayHeadAction( HeadAction.LowerHead, 1, null);
        bodyController.PlayHandAction( HandAction.PickUp,1,
            (string x) => 
            {
                Debug.Log("OK"+x);
                if (x == "PickUp")
                {
                    itemObj.PickUp(this);
                }
            });
    }
    public void AddItem_Hand(ItemConfig config,bool showInUI)
    {
        Type type = Type.GetType("Item_" + config.Item_ID.ToString());
        holdingByHand = (ItemBase)Activator.CreateInstance(type);
        holdingByHand.Init(config);
        holdingByHand.BeHolding(this, bodyController.Hand_Right);
        /*����UI*/
        if (showInUI)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_UI_AddItemInHand()
            {
                itemConfig = config
            });
        }
    }
    public void UseItem_Hand(ItemConfig config)
    {
        
    }
    public void SubItem_Hand(ItemConfig config)
    {

    }
    public void AddItem_Bag(ItemConfig config,bool showInUI)
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
        /*����UI*/
        if (showInUI)
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
    public virtual void RunAway()
    {

    }
    public virtual void RunTo()
    {

    }
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

    #endregion
    /*����ͬ����Ϊ*/
    #region
    public void TryToFindPathByRPC(Vector2 to, Vector2 from)
    {
        if (Object.HasStateAuthority)
        {
            RPC_FindPath(to, from);
        }
    }
    /// <summary>
    /// Ѱ��·��
    /// </summary>
    /// <param name="to"></param>
    /// <param name="from"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPC_FindPath(Vector2 to, Vector2 from)
    {
        if (myLoad != null)
        {
            myLoad.Clear();
        }
        myLoad = navManager.FindPath(navManager.FindTileByPos(to), navManager.FindTileByPos(from));
        UpdatePath(myLoad);
    }
    #endregion
}
