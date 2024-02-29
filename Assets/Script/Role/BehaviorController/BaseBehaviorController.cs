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
/// 行为控制器
/// </summary>
public class BaseBehaviorController : NetworkBehaviour
{
    [Header("身体控制器")]
    public BaseBodyController bodyController;
    [Header("技能指示器")]
    public SI_Sector skillSector;
    [Header("手部持有物")]
    public ItemBase holdingByHand = new ItemBase();
    private Vector2 faceTo = Vector2.zero;
    [SerializeField]
    public RoleData Data = new RoleData();
    [HideInInspector]
    public bool isPlayer = false;
    private void Start()
    {
        Init();
    }
    public virtual void FixedUpdate()
    {
        if (tempPath != null)
        {
            MoveByPath(Time.fixedDeltaTime);
        }
        if (!Object.HasStateAuthority)
        {
            ClientMove();
        }
    }
    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
    }
    /*初始*/
    #region
    /// <summary>
    /// 初始化
    /// </summary>
    public virtual void Init()
    {
        navManager = GameObject.Find("Furniture").GetComponent<NavManager>();
        MessageBroker.Default.Receive<GameEvent.GameEvent_SomeoneMove>().Subscribe(_ =>
        {
            ListenRoleMove(_.moveRole, _.moveTile);
        }).AddTo(this);
    }
    #endregion
    /*移动*/
    #region
    [Header("身体根节点")]
    public Transform Root;
    [Header("位置同步组件")]
    public NetworkTransform NetTransform;

    [HideInInspector, Header("移动方向")]
    public Vector2 tempVector;
    [HideInInspector, Header("当前位置")]
    public MyTile curTile;
    [HideInInspector, Header("之前位置")]
    public MyTile lastTile;
    [HideInInspector, Header("加速状态")]
    public bool speedUp_State;
    [HideInInspector, Header("加速增幅")]
    public float speedUp_Val = 2f;
    [HideInInspector, Header("临时路径")]
    public List<MyTile> tempPathList = new List<MyTile>();
    [HideInInspector, Header("下一个路径点")]
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
    /// 加速
    /// </summary>
    /// <param name="up"></param>
    public virtual void SpeedUp(bool up)
    {
        speedUp_State = up;
    }
    /// <summary>
    /// 更新路径
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
    /// 按照方向移动
    /// </summary>
    /// <param name="dt"></param>
    public virtual void MoveByVector(float dt)
    {
        if (Object.HasStateAuthority)
        {
            StateMove(tempVector, Data.Data_Speed, dt);
        }
        if (tempVector != Vector2.zero)
        {
            PlayMove(Data.Data_Speed);
            PlayTurn(tempVector);
        }
        else
        {
            PlayStop(Data.Data_Speed);
        }
        CheckMyPos();        
        tempVector = Vector3.zero;
    }
    /// <summary>
    /// 按照路径移动
    /// </summary>
    public virtual void MoveByPath(float dt)
    {
        /*到达路径点*/
        if (Vector2.Distance(tempPath.pos,transform.position) <= 0.1f)
        {
            PlayStop(Data.Data_Speed);
            tempPath = null;
            if (tempPathList.Count > 0)
            {
                tempPathList.RemoveAt(0);
                if(tempPathList.Count > 0)
                {
                    tempPath = tempPathList[0];
                }
            }
            CheckMyPos();
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
            if (Object.HasStateAuthority)
            {
                StateMove(temp, Data.Data_Speed, dt);
            }
            PlayTurn(temp);
            PlayMove(Data.Data_Speed);
            if (Vector2.Distance(tempPath.pos, transform.position) <= 0.1f)
            {
                PlayStop(Data.Data_Speed);
                CheckMyPos();
            }
        }
    }
    private Vector3 newPositon;
    /// <summary>
    /// 移动
    /// </summary>
    private void StateMove(Vector2 dir, float speed, float dt)
    {
        dir = dir.normalized;
        if (speedUp_State) { speed *= speedUp_Val; }
        statePos = transform.position;
        transform.position =
            transform.position + new UnityEngine.Vector3(dir.x * speed * dt, dir.y * speed * dt, 0);
    }
    private void ClientMove()
    {
        Debug.Log(statePos);
        transform.position = statePos;
    }
    /// <summary>
    /// 移动动画
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="speed"></param>
    /// <param name="dt"></param>
    public void PlayMove(float speed)
    {
        bodyController.PlayBodyAction(BodyAction.Move, speed, null);
        bodyController.PlayHeadAction(HeadAction.Move, speed, null);
        bodyController.PlayLegAction(LegAction.Step, speed);
        bodyController.PlayHandAction(HandAction.Step, speed, null);
    }
    /// <summary>
    /// 转向动画
    /// </summary>
    /// <param name="dir"></param>
    public void PlayTurn(Vector2 dir)
    {
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
    /// 停止动画
    /// </summary>
    public void PlayStop(float speed)
    {
        bodyController.PlayBodyAction(BodyAction.Idle, speed, null);
        bodyController.PlayHeadAction(HeadAction.Idle, speed, null);
        bodyController.PlayLegAction(LegAction.Idle, speed);
        bodyController.PlayHandAction(HandAction.Idle, speed, null);
    }
    /// <summary>
    /// 检查我的位置
    /// </summary>
    private void CheckMyPos()
    {
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
    #endregion
    /*寻路*/
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
    /*监听*/
    #region
    /// <summary>
    /// 时间更新
    /// </summary>
    public virtual void ListenTimeUpdate()
    {

    }
    /// <summary>
    /// 位置更新
    /// </summary>
    /// <param name="who"></param>
    /// <param name="where"></param>
    public virtual void ListenRoleMove(BaseBehaviorController who,MyTile where)
    {
        
    }
    #endregion
    /*行为*/
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
        /*更新UI*/
        if (showInUI)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_AddItemInHand()
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
        /*传入物体数量不能为零*/
        if(config.Item_CurCount <= 0) { config.Item_CurCount = 1; }
        /*遍历背包*/
        for(int i = 0; i < Data.Holding_BagList.Count; i++)
        {
            /*检查背包里有同种物体*/
            if (Data.Holding_BagList[i].Item_ID == config.Item_ID)
            {
                /*检查背包里的同种物体是否达到堆叠最大值*/
                if (Data.Holding_BagList[i].Item_CurCount < Data.Holding_BagList[i].Item_MaxCount)
                {
                    ItemConfig tempItem = Data.Holding_BagList[i];
                    /*剩余最大值不够这个物体的叠加*/
                    if (Data.Holding_BagList[i].Item_MaxCount - Data.Holding_BagList[i].Item_CurCount < config.Item_CurCount)
                    {
                        tempItem.Item_CurCount = tempItem.Item_MaxCount;
                        config.Item_CurCount -= (Data.Holding_BagList[i].Item_MaxCount - Data.Holding_BagList[i].Item_CurCount);
                        Data.Holding_BagList[i] = tempItem;
                    }
                    /*剩余最大值够这个物体的叠加*/
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
        /*遍历一遍之后还有剩余*/
        if(config.Item_CurCount > 0)
        {
            Data.Holding_BagList.Add(config);
        }
        /*更新UI*/
        if (showInUI)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateItemInBag()
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
    /*网络同步行为*/
    #region
    [Networked]
    public Vector2 statePos { get; set; } 

    public void TryToFindPathByRPC(Vector2 to, Vector2 from)
    {
        if (Object.HasStateAuthority)
        {
            RPC_FindPath(to, from);
        }
    }
    /// <summary>
    /// 寻找路径
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
    public void TryToTakeDamage(int damage)
    {
        RPC_TakeDamage(damage);
    }
    /// <summary>
    /// 受到伤害
    /// </summary>
    /// <param name="damage"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_TakeDamage(int damage)
    {
        GameObject obj_num = UIManager.Instance.ShowUI("UI/UI_DamageNum", (Vector2)transform.position+new Vector2(0,1));
        obj_num.GetComponent<UI_DamageNum>().Play(damage, new Color32(255, 50, 50, 255));

        if (Data.Data_Hp >= damage)
        {
            Data.Data_Hp -= damage;
            bodyController.PlayHeadAction(HeadAction.TakeDamage,1,null);
        }
        else
        {
            Data.Data_Hp -= damage;
            bodyController.PlayHeadAction(HeadAction.TakeDamage, 1, null);
        }
    }
    #endregion
}
