using DG.Tweening;
using Fusion;
using Fusion.Addons.Physics;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;
/// <summary>
/// 角色管理器
/// </summary>
public class ActorManager : MonoBehaviour
{
    [Header("物理")]
    public Rigidbody2D myRigidbody;
    [SerializeField, Header("网络控制器")]
    private ActorNetManager netManager;
    public ActorNetManager NetManager
    {
        get { return netManager; }
        set { netManager = value; }
    }
    [SerializeField, Header("身体控制器")]
    private BaseBodyController bodyController;
    public BaseBodyController BodyController
    {
        get { return bodyController; }
        set { bodyController = value; }
    }
    [SerializeField, Header("范围指示器")]
    private SI_Sector skillSector;
    public SI_Sector SkillSector
    {
        get { return skillSector; }
        set { skillSector = value; }
    }
    [SerializeField, Header("UI管理器")]
    private ActorUI actorUI;
    [HideInInspector]
    public ActorState actorState;
    public ActorUI ActorUI
    {
        get { return actorUI; }
        set { actorUI = value; }
    }
    [HideInInspector]
    public ItemBase holdingByHand = new ItemBase();
    [HideInInspector]
    public ItemBase wearingOnHead = new ItemBase();
    [HideInInspector]
    public ItemBase wearingOnBody = new ItemBase();
    [HideInInspector]
    public List<ItemBase> itemInBag = new List<ItemBase>();
    [HideInInspector]
    public SkillBase bindSkill = new SkillBase();
    [HideInInspector]
    public NavManager navManager;
    [HideInInspector]
    public bool isPlayer = false;
    [HideInInspector]
    public bool isState = false;
    [HideInInspector]
    public bool isInput = false;
    public const float customUpdateTime = 0.1f;
    protected LayerMask tileLayer;
    private void Awake()
    {
        tileLayer = LayerMask.GetMask("TileObj_Wall");
        myRigidbody.gravityScale = 0;
    }
    public virtual void FixedUpdateNetwork(float dt)
    {
        UpdatePath(dt);
    }
    public virtual void CustomUpdate()
    {
        UpdateAnima(customUpdateTime);
        CheckMyTile();
    }
    public virtual void InitByNetManager(bool hasStateAuthority,bool hasInputAuthority)
    {
        isState = hasStateAuthority;
        isInput = hasInputAuthority;
        InvokeRepeating("CustomUpdate", 1f, customUpdateTime);
        navManager = GameObject.Find("Map").GetComponent<NavManager>();
        MessageBroker.Default.Receive<GameEvent.GameEvent_Local_SomeoneMove>().Subscribe(_ =>
        {
            Listen_RoleMove(_.moveActor, _.moveTile);
        }).AddTo(this);
        MessageBroker.Default.Receive<GameEvent.GameEvent_Local_SomeoneSendEmoji>().Subscribe(_ =>
        {
            Listen_RoleSendEmoji(_.actor, _.id, _.distance);
        }).AddTo(this);
        MessageBroker.Default.Receive<GameEvent.GameEvent_Local_SomeoneDoSomething>().Subscribe(_ =>
        {
            ListenRoleDoSamething(_.actor, _.action);
        }).AddTo(this);
        MessageBroker.Default.Receive<GameEvent.GameEvent_Local_SomeoneCommit>().Subscribe(_ =>
        {
            ListenRoleCommit(_.actor, _.fine);
        }).AddTo(this);
        MessageBroker.Default.Receive<GameEvent.GameEvent_Local_TimeChange>().Subscribe(_ =>
        {
            Listen_WorldGlobalTimeChange(_.hour, _.date, _.now);
        }).AddTo(this);
    }
    public void InitByPlayer()
    {
        isPlayer = true;
    }
    /*监听*/
    #region
    /// <summary>
    /// 监听时间改变
    /// </summary>
    /// <param name="globalTime"></param>
    public virtual void Listen_WorldGlobalTimeChange(int hour,int date,GlobalTime globalTime)
    {
        netManager.AddOneHour(hour, date, globalTime);
    }
    /// <summary>
    /// 监听某人移动
    /// </summary>
    /// <param name="who"></param>
    /// <param name="where"></param>
    public virtual void Listen_RoleMove(ActorManager who, MyTile where)
    {

    }
    /// <summary>
    /// 监听某人做了什么事情
    /// </summary>
    /// <param name="who"></param>
    /// <param name="actorAction"></param>
    public virtual void ListenRoleDoSamething(ActorManager who, GameEvent.ActorAction actorAction)
    {

    }
    /// <summary>
    /// 监听某人犯法
    /// </summary>
    /// <param name="who"></param>
    /// <param name="actorAction"></param>
    public virtual void ListenRoleCommit(ActorManager who, short val)
    {

    }
    /// <summary>
    /// 监听某人发送emoji
    /// </summary>
    /// <param name="actor">谁</param>
    /// <param name="id">什么</param>
    /// <param name="distance">距离</param>
    public virtual void Listen_RoleSendEmoji(ActorManager actor, int id, float distance)
    {

    }
    #endregion
    /*动画*/
    #region
    private float curPos_X;
    private float curPos_Y;
    private float rigSpeed;
    private float lastPos_X;
    private float lastPos_Y;
    private bool turnRight = false;
    private bool turnLeft = false;
    [HideInInspector]
    public Vector2 face;
    /*检查动画播放状态*/
    public virtual void UpdateAnima(float dt)
    {
        curPos_X = transform.position.x;
        curPos_Y = transform.position.y;
        rigSpeed = netManager.networkRigidbody.Rigidbody.velocity.magnitude;

        float distance = Vector2.Distance(new Vector2(curPos_X, curPos_Y), new Vector2(lastPos_X, lastPos_Y));
        float speed = distance / dt;
        if (rigSpeed <= 0.1f)
        {
            if (speed > 0.1f)
            {
                PlayMove(speed);
            }
            else
            {
                PlayStop(1);
            }
        }
        else
        {

        }

        if (curPos_X > lastPos_X)
        {
            if (!turnRight)
            {
                if (!isPlayer)
                {
                    FaceRight();
                }
                TurnRight();
            }
        }
        else
        {
            if (!turnLeft)
            {
                if (!isPlayer)
                {
                    FaceLeft();
                }
                TurnLeft();
            }
        }

        lastPos_X = curPos_X;
        lastPos_Y = curPos_Y;
    }
    /// <summary>
    /// 面向某处
    /// </summary>
    public virtual void FaceTo(Vector3 face)
    {
        this.face = new Vector2(face.x, face.y).normalized;
        if (face.x > 0)
        {
            FaceRight();
        }
        else
        {
            FaceLeft();
        }
    }
    /// <summary>
    /// 面向左边
    /// </summary>
    public virtual void FaceLeft()
    {
        BodyController.Head.localScale = new Vector3(-1, 1, 1);
        BodyController.Body.localScale = new Vector3(-1, 1, 1);
        BodyController.Hand.localScale = new Vector3(-1, 1, 1);
    }
    /// <summary>
    /// 面向右边
    /// </summary>
    public virtual void FaceRight()
    {
        BodyController.Head.localScale = new Vector3(1, 1, 1);
        BodyController.Body.localScale = new Vector3(1, 1, 1);
        BodyController.Hand.localScale = new Vector3(1, 1, 1);
    }
    /// <summary>
    /// 转向某处
    /// </summary>
    /// <param name="turn"></param>
    public virtual void TurnTo(Vector3 turn)
    {
        if (turn.x > 0)
        {
            TurnRight();
        }
        if (turn.x < 0)
        {
            TurnLeft();
        }
    }
    /// <summary>
    /// 转向左边
    /// </summary>
    public virtual void TurnLeft()
    {
        turnLeft = true;
        turnRight = false;
        BodyController.Leg.localScale = new Vector3(-1, 1, 1);
    }
    /// <summary>
    /// 转向右边
    /// </summary>
    public virtual void TurnRight()
    {
        turnRight = true;
        turnLeft = false;
        BodyController.Leg.localScale = new Vector3(1, 1, 1);
    }

    public virtual void PlayMove(float speed)
    {
        bodyController.SetHeadFloat("MoveSpeed", speed);
        bodyController.SetHeadBool("Step", true, 1, null);
        bodyController.SetHeadBool("Idle", false, 1, null);

        bodyController.SetBodyFloat("MoveSpeed", speed);
        bodyController.SetBodyBool("Step", true, 1, null);
        bodyController.SetBodyBool("Idle", false, 1, null);

        bodyController.SetHandFloat("MoveSpeed", speed);
        bodyController.SetHandBool("Step", true, 1, null);
        bodyController.SetHandBool("Idle", false, 1, null);

        bodyController.SetLegFloat("MoveSpeed", speed);
        bodyController.SetLegBool("Step", true, 1, null);
        bodyController.SetLegBool("Idle", false, 1, null);
    }
    public virtual void PlayStop(float speed)
    {
        bodyController.SetHeadFloat("MoveSpeed", speed);
        bodyController.SetHeadBool("Step", false, 1, null);
        bodyController.SetHeadBool("Idle", true, 1, null);

        bodyController.SetBodyFloat("MoveSpeed", speed);
        bodyController.SetBodyBool("Step", false, 1, null);
        bodyController.SetBodyBool("Idle", true, 1, null);

        bodyController.SetHandFloat("MoveSpeed", speed);
        bodyController.SetHandBool("Step", false, 1, null);
        bodyController.SetHandBool("Idle", true, 1, null);

        bodyController.SetLegFloat("MoveSpeed", speed);
        bodyController.SetLegBool("Step", false, 1, null);
        bodyController.SetLegBool("Idle", true, 1, null);
    }
    public virtual void PlayDead(float speed)
    {
        bodyController.SetHeadTrigger("Dead", 1, (str) =>
        {
            if (str == "Dead")
            {
                Dead();
            }
        });
        bodyController.SetBodyTrigger("Dead", 1, null);
        bodyController.SetHandTrigger("Dead", 1, null);
        bodyController.SetLegTrigger("Dead", 1, null);
        bodyController.locking = true;
    }
    public virtual void PlayTakeDamage(float speed)
    {
        bodyController.SetHeadTrigger("TakeDamage", 1, null);
    }
    public virtual void PlayPickUp(float speed, Action<string> action)
    {
        bodyController.SetHeadTrigger("LowerHead", 1, null);
        bodyController.SetHandTrigger("PickUp", 1, action);
    }
    #endregion
    /*位置*/
    #region
    private List<MyTile> tempTiles = new List<MyTile>();
    private MyTile curTile;
    private MyTile lastTile;
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
    public MyTile GetMyTileWithOffset(Vector3Int offset)
    {
        if (navManager != null)
        {
            Vector3Int vector3Int = navManager.grid.WorldToCell(transform.position);
            return (MyTile)navManager.tilemap.GetTile(vector3Int + offset);
        }
        else
        {
            return null;
        }

    }
    public List<MyTile> GetNearbyTiles()
    {
        tempTiles.Clear();
        MyTile tile = GetMyTile();
        tempTiles.Add((MyTile)navManager.tilemap.GetTile(tile._posInCell));
        tempTiles.Add((MyTile)navManager.tilemap.GetTile(tile._posInCell + Vector3Int.up));
        tempTiles.Add((MyTile)navManager.tilemap.GetTile(tile._posInCell + Vector3Int.right));
        tempTiles.Add((MyTile)navManager.tilemap.GetTile(tile._posInCell + Vector3Int.left));
        tempTiles.Add((MyTile)navManager.tilemap.GetTile(tile._posInCell + Vector3Int.down));
        return tempTiles;
    }
    public void CheckMyTile()
    {
        curTile = GetMyTile();
        if (!curTile) { return; }
        if (lastTile == null) { lastTile = curTile; }
        if (lastTile._posInWorld != curTile._posInWorld)
        {
            ListenMyself_InNewTile();
        }
    }
    /// <summary>
    /// 到达新地块
    /// </summary>
    public virtual void ListenMyself_InNewTile()
    {
        lastTile = curTile;
        MessageBroker.Default.Publish(new GameEvent.GameEvent_Local_SomeoneMove
        {
            moveActor = this,
            moveTile = curTile
        });
    }
    /// <summary>
    /// 到达终点
    /// </summary>
    public virtual void ListenMyself_InDestinationTile()
    {

    }
    #endregion
    /*物品*/
    #region
    /// <summary>
    /// 拿到手上
    /// </summary>
    /// <param name="data"></param>
    /// <param name="showInUI"></param>
    public void AddItem_Hand(ItemData data)
    {
        /*更新UI*/
        if (isPlayer && netManager.Object.HasInputAuthority)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateItemInHand()
            {
                itemData = data
            });
        }
        if (data.Item_ID != 0)
        {
            if (netManager.Last_ItemInHand.Item_ID != data.Item_ID)
            {
                ResetItem_Hand();
                Type type = Type.GetType("Item_" + data.Item_ID.ToString());
                holdingByHand = (ItemBase)Activator.CreateInstance(type);
                holdingByHand.UpdateData(data);
                holdingByHand.Holding_Start(this, BodyController);
                holdingByHand.Holding_UpdateLook();
            }
            else
            {
                if (holdingByHand != null)
                {
                    holdingByHand.UpdateData(data);
                    holdingByHand.Holding_UpdateLook();
                }
            }
        }
        else
        {
            ResetItem_Hand();
        }
        netManager.Last_ItemInHand = data;
    }
    public void ResetItem_Hand()
    {
        if (holdingByHand != null) { holdingByHand.Holding_Over(this); }
        holdingByHand = new ItemBase();
        if (bodyController.Hand_LeftItem.childCount > 0)
        {
            for (int i = 0; i < bodyController.Hand_LeftItem.childCount; i++)
            {
                Destroy(bodyController.Hand_LeftItem.GetChild(i).gameObject);
            }
        }
        if (bodyController.Hand_RightItem.childCount > 0)
        {
            for (int i = 0; i < bodyController.Hand_RightItem.childCount; i++)
            {
                Destroy(bodyController.Hand_RightItem.GetChild(i).gameObject);
            }
        }

        bodyController.Hand_Left.GetComponent<SpriteRenderer>().enabled = true;
        bodyController.Hand_Right.GetComponent<SpriteRenderer>().enabled = true;

        bodyController.SetBodyBool("Idle", true, 1, null);
        bodyController.SetHeadBool("Idle", true, 1, null);
        bodyController.SetHandBool("Idle", true, 1, null);
        bodyController.SetHandBool("Idle", true, 1, null);

        bodyController.Hand_RightItem.localScale = Vector3.one;
        bodyController.Hand_LeftItem.localScale = Vector3.one;
        bodyController.Hand_RightItem.localScale = Vector3.one;
        bodyController.Hand_LeftItem.localPosition = Vector3.zero;
        bodyController.Hand_LeftItem.localRotation = Quaternion.identity;
        bodyController.Hand_RightItem.localPosition = Vector3.zero;
        bodyController.Hand_RightItem.localRotation = Quaternion.identity;
        bodyController.Hand_LeftItem.GetComponent<SpriteRenderer>().sprite
            = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_Default");
        bodyController.Hand_LeftItem.GetComponent<SpriteRenderer>().sortingOrder = 1;

        bodyController.Hand_RightItem.GetComponent<SpriteRenderer>().sprite
            = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_Default");
        bodyController.Hand_RightItem.GetComponent<SpriteRenderer>().sortingOrder = 4;
    }
    /// <summary>
    /// 戴到头上
    /// </summary>
    /// <param name="data"></param>
    public void WearItem_Head(ItemData data)
    {
        ResetItem_Head();
        /*更新UI*/
        if (isPlayer && netManager.Object.HasInputAuthority)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateStatus()
            {
                statusId = NetManager.LocalData_Status
            });
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateItemOnHead()
            {
                itemData = data
            });
        }
        if (data.Item_ID != 0)
        {
            Type type = Type.GetType("Item_" + data.Item_ID.ToString());
            wearingOnHead = (ItemBase)Activator.CreateInstance(type);
            wearingOnHead.UpdateData(data);
            wearingOnHead.BeWearingOnHead(this, BodyController);
        }
    }
    public void ResetItem_Head()
    {
        wearingOnHead = new ItemBase();
        if (bodyController.Head_Item.childCount > 0)
        {
            for (int i = 0; i < bodyController.Head_Item.childCount; i++)
            {
                Destroy(bodyController.Head_Item.GetChild(i).gameObject);
            }
        }
        bodyController.Head_Item.localScale = Vector3.one;
        bodyController.Head_Item.localPosition = new Vector3(0, 0.25f, 0);
        bodyController.Head_Item.localRotation = Quaternion.identity;
        bodyController.Head_Item.GetComponent<SpriteRenderer>().sprite
            = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_Default");
        bodyController.Head_Item.GetComponent<SpriteRenderer>().sortingOrder = 3;
    }
    /// <summary>
    /// 穿到身上
    /// </summary>
    /// <param name="data"></param>
    public void WearItem_Body(ItemData data)
    {
        ResetItem_Body();
        /*更新UI*/
        if (isPlayer && netManager.Object.HasInputAuthority)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateStatus()
            {
                statusId = NetManager.LocalData_Status
            });
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateItemOnBody()
            {
                itemData = data
            });
        }
        if (data.Item_ID != 0)
        {
            Type type = Type.GetType("Item_" + data.Item_ID.ToString());
            wearingOnBody = (ItemBase)Activator.CreateInstance(type);
            wearingOnBody.UpdateData(data);
            wearingOnBody.BeWearingOnBody(this, BodyController);
        }
    }
    public void ResetItem_Body()
    {
        wearingOnHead = new ItemBase();
        if (bodyController.Body_Item.childCount > 0)
        {
            for (int i = 0; i < bodyController.Body_Item.childCount; i++)
            {
                Destroy(bodyController.Body_Item.GetChild(i).gameObject);
            }
        }
        bodyController.Body_Item.localScale = Vector3.one;
        bodyController.Body_Item.localPosition = Vector3.zero;
        bodyController.Body_Item.localRotation = Quaternion.identity;
        bodyController.Body_Item.GetComponent<SpriteRenderer>().sprite
            = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_Default");
        bodyController.Body_Item.GetComponent<SpriteRenderer>().sortingOrder = 2;
    }
    #endregion
    /*身份*/
    #region
    /// <summary>
    /// 检查身份
    /// </summary>
    /// <param name="clothes"></param>
    /// <param name="hat"></param>
    /// <param name="id"></param>
    /// <param name="fine"></param>
    public void CheckStatus(short clothes,short hat,short fine,out short id)
    {
        id = 0;
        StatusConfig status;
        status = StatusConfigData.statusConfigs.Find((x) => { return x.Status_Clothes == clothes; });
        if (status.Status_ID != 0)
        {
            id = status.Status_ID;
            return;
        }
        status = StatusConfigData.statusConfigs.Find((x) => { return x.Status_Hat == hat; });
        if (status.Status_ID != 0)
        {
            id = status.Status_ID;
            return;
        }
        if (clothes / 1000 != 5 && hat / 1000 != 5)
        {
            id = 1002;
        }
        else
        {
            id = 1003;
        }
    }
    #endregion
    /*寻路*/
    #region
    [HideInInspector]
    public List<MyTile> tempPath = new List<MyTile>();
    [HideInInspector]
    public MyTile targetTile = null;
    public void UpdatePath(float dt)
    {
        if (targetTile)
        {
            if (Vector2.Distance(targetTile._posInWorld, transform.position) <= 0.1f)
            {
                /*到达路径点，检查*/
                targetTile = null;
                if (tempPath.Count > 0)
                {
                    /*路径还未结束*/
                    targetTile = tempPath[0];
                    tempPath.RemoveAt(0);

                    //tempPath.RemoveAt(0);
                    //if (tempPath.Count > 0)
                    //{
                    //    targetTile = tempPath[0];
                    //}
                }
                else
                {
                    ListenMyself_InDestinationTile();
                    /*路径已经结束*/
                }
                CheckMyTile();
                return;
            }
            else
            {
                /*还未到达路径点，前往路径点*/
                Vector2 temp = Vector2.zero;
                if (transform.position.x > targetTile._posInWorld.x)
                {
                    if ((transform.position.x - targetTile._posInWorld.x) > 0.05f)
                    {
                        temp += new Vector2(-1, 0);
                    }
                }
                else
                {
                    if ((transform.position.x - targetTile._posInWorld.x) < -0.05f)
                    {
                        temp += new Vector2(1, 0);
                    }
                }
                if (transform.position.y > targetTile._posInWorld.y)
                {
                    if ((transform.position.y - targetTile._posInWorld.y) > 0.05f)
                    {
                        temp += new Vector2(0, -1);
                    }
                }
                else
                {
                    if ((transform.position.y - targetTile._posInWorld.y) < -0.05f)
                    {
                        temp += new Vector2(0, 1);
                    }
                }
                temp = temp.normalized;

                float commonSpeed = NetManager.Data_CommonSpeed / 10f;
                Vector2 velocity = new Vector2(temp.x * commonSpeed, temp.y * commonSpeed);
                Vector3 newPos = transform.position + new UnityEngine.Vector3(velocity.x * dt, velocity.y * dt, 0);
                netManager.UpdateNetworkTransform(newPos, velocity.magnitude);
            }
        }
    }
    /// <summary>
    /// 找到一条路径
    /// </summary>
    /// <param name="targetPos"></param>
    public virtual void FindWayToTarget(Vector2 targetPos)
    {
        tempPath.Clear();
        tempPath = navManager.FindPath(navManager.FindTileByPos(targetPos), GetMyTile());
        if (tempPath.Count > 1)
        {
            targetTile = tempPath[0];
            tempPath.RemoveAt(0);
        }
    }
    #endregion
    /*基本属性变化*/
    #region
    /// <summary>
    /// 受伤
    /// </summary>
    /// <param name="val"></param>
    /// <param name="id"></param>
    public void TakeDamage(int val,ActorNetManager from)
    {
        if (actorState != ActorState.Dead && from.HasInputAuthority)
        {
            val -= netManager.Data_Armor;
            if (val > 0)
            {
                netManager.RPC_HpChange(-val, from.Object.Id);
            }
            else
            {
                netManager.RPC_HpChange(0, from.Object.Id);
            }
        }
    }
    public void TryToDead()
    {
        if (actorState != ActorState.Dead)
        {
            actorState = ActorState.Dead;
            PlayDead(1);
        }
    }
    /// <summary>
    /// 死亡
    /// </summary>
    public virtual void Dead()
    {
        if (isState)
        {
            if (!isPlayer)
            {
                netManager.Runner.Despawn(netManager.Object);
            }
        }
    }
    /// <summary>
    /// 减少食物值
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public int SubFood(int val)
    {
        if (actorState != ActorState.Dead && isState)
        {
            if (netManager.Data_CurFood + val > 0)
            {
                netManager.RPC_FoodChange((short)(netManager.Data_CurFood + val));
            }
            else
            {
                netManager.RPC_FoodChange(0);
            }
        }
        return netManager.Data_CurFood;
    }
    /// <summary>
    /// 增加食物值
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public int AddFood(int val)
    {
        if (actorState != ActorState.Dead && isState)
        {
            if (netManager.Data_CurFood + val <= netManager.Data_MaxFood)
            {
                netManager.RPC_FoodChange((short)(netManager.Data_CurFood + val));
            }
            else
            {
                netManager.RPC_FoodChange(netManager.Data_MaxFood);
            }
        }
        return netManager.Data_CurFood;
    }
    /// <summary>
    /// 支付
    /// </summary>
    /// <returns></returns>
    public bool PayCoin(int coin)
    {
        if (netManager.Data_Coin >= coin)
        {
            netManager.RPC_LocalInput_PayCoin(coin);
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// 赚钱
    /// </summary>
    /// <returns></returns>
    public int EarnCoin(int coin)
    {
        netManager.RPC_LocalInput_EarnCoin(coin);
        return netManager.Data_Coin;
    }
    /// <summary>
    /// 设置罚金
    /// </summary>
    public void SetFine(short val)
    {
        if (NetManager.Data_Fine < val)
        {
            NetManager.RPC_LocalInput_ChangeFine(val);
        }
    }
    /// <summary>
    /// 清空罚金
    /// </summary>
    public void ClearFine()
    {
        NetManager.RPC_LocalInput_ChangeFine(0);
    }
    #endregion
    /*RPC*/
    #region
    /// <summary>
    /// 攻击状态
    /// </summary>
    protected bool attackState = false;
    /// <summary>
    /// 攻击目标
    /// </summary>
    protected ActorManager attackTarget;
    /// <summary>
    /// 注视目标
    /// </summary>
    protected List<ActorManager> lookTarget = new List<ActorManager>();
    /// <summary>
    /// 记忆目标
    /// </summary>
    protected List<ActorManager> rememberTarget = new List<ActorManager>();
    /// <summary>
    /// 更改攻击状态
    /// </summary>
    /// <param name="state"></param>
    public virtual void FromRPC_ChangeAttackState(bool state)
    {
        attackState = state;
    }
    /// <summary>
    /// 更改攻击目标
    /// </summary>
    /// <param name="id"></param>
    public virtual void FromRPC_ChangeAttackTarget(Fusion.NetworkId id)
    {
        if (id != new NetworkId())
        {
            attackTarget = netManager.Runner.FindObject(id).GetComponent<ActorManager>();
        }
        else
        {
            attackTarget = null;
        }
    }
    public virtual void FromRPC_BindSkill(int skillID)
    {
        Type type = Type.GetType("Skill_" + skillID.ToString());
        bindSkill = (SkillBase)Activator.CreateInstance(type);
        bindSkill.Init(this);
    }
    public virtual void FromRPC_NpcUseSkill(int parameter, Fusion.NetworkId id)
    {
        bindSkill.ClickSpace();
    }
    public virtual void FromRPC_SendEmoji(int emojiID)
    {
        MessageBroker.Default.Publish(new GameEvent.GameEvent_Local_SomeoneSendEmoji
        {
            actor = this,
            id = emojiID,
            distance = EmojiConfigData.GetEmojiConfig(emojiID).Emoji_Distance
        });
        ActorUI.SendEmoji(EmojiConfigData.GetEmojiConfig(emojiID));
    }
    public virtual void Listen_HpChange(int parameter, Fusion.NetworkId id)
    {
        if (parameter <= 0)
        {
            GameObject obj_num = UIManager.Instance.ShowUI("UI/UI_DamageNum", (Vector2)transform.position + new Vector2(0, 1));
            obj_num.GetComponent<UI_DamageNum>().Play(parameter, new Color32(255, 50, 50, 255));
            PlayTakeDamage(1);
        }
    }
    #endregion
}
public enum ActorState
{
    Default,
    Dead
}