using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;
using static Unity.Collections.Unicode;
/// <summary>
/// 角色管理器
/// </summary>
public class ActorManager : MonoBehaviour
{
    [SerializeField, Header("角色配置属性")]
    public ActorConfig actorConfig;
    [SerializeField, Header("网络控制器")]
    private ActorNetManager netController;
    public ActorNetManager NetController
    {
        get { return netController; }
        set { netController = value; }
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
    [Header("你现在什么情况?")]
    public ActorState actorState;
    public ActorUI ActorUI
    {
        get { return actorUI; }
        set { actorUI = value; }
    }
    [HideInInspector]
    public ItemBase holdingByHand = new ItemBase();
    [HideInInspector]
    public NavManager navManager;

    protected bool isPlayer = false;
    protected bool isState = false;
    public const float customUpdateTime = 0.1f;

    public virtual void FixedUpdateNetwork(float dt)
    {
        UpdatePath(dt);
    }
    public virtual void CustomUpdate()
    {
        UpdateAnima(customUpdateTime);
        CheckMyTile();
    }
    public void InitByNetManager(bool hasStateAuthority)
    {
        isState = hasStateAuthority;
        InvokeRepeating("CustomUpdate", 1f, customUpdateTime);
        navManager = GameObject.Find("Furniture").GetComponent<NavManager>();
        MessageBroker.Default.Receive<GameEvent.GameEvent_Local_SomeoneMove>().Subscribe(_ =>
        {
            ListenRoleMove(_.moveActor, _.moveTile);
        }).AddTo(this);
    }
    public void InitByPlayer()
    {
        isPlayer = true;
    }
    /*监听*/
    #region
    public virtual void ListenRoleMove(ActorManager who, MyTile where)
    {

    }
    #endregion
    /*动画*/
    #region
    private float curPos_X;
    private float curPos_Y;
    private float lastPos_X;
    private float lastPos_Y;
    private bool turnRight = false;
    private bool turnLeft = false;
    /*检查动画播放状态*/
    public virtual void UpdateAnima(float dt)
    {
        curPos_X = transform.position.x;
        curPos_Y = transform.position.y;
        float distance = Vector2.Distance(new Vector2(curPos_X, curPos_Y), new Vector2(lastPos_X, lastPos_Y));
        float speed = distance / dt;
        if (speed > 0.1f)
        {
            PlayMove(speed);
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
        }
        else
        {
            PlayStop(1);
        }
        lastPos_X = curPos_X;
        lastPos_Y = curPos_Y;
    }
    /// <summary>
    /// 面向某处
    /// </summary>
    public virtual void FaceTo(Vector3 face)
    {
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
        bodyController.SetHeadBool("Step", true, 1, null);
        bodyController.SetHeadBool("Idle", false, 1, null);

        bodyController.SetBodyBool("Step", true, 1, null);
        bodyController.SetBodyBool("Idle", false, 1, null);

        bodyController.SetHandBool("Step", true, 1, null);
        bodyController.SetHandBool("Idle", false, 1, null);

        bodyController.SetLegBool("Step", true, 1, null);
        bodyController.SetLegBool("Idle", false, 1, null);
    }
    public virtual void PlayStop(float speed)
    {
        bodyController.SetHeadBool("Step", false, 1, null);
        bodyController.SetHeadBool("Idle", true, 1, null);

        bodyController.SetBodyBool("Step", false, 1, null);
        bodyController.SetBodyBool("Idle", true, 1, null);

        bodyController.SetHandBool("Step", false, 1, null);
        bodyController.SetHandBool("Idle", true, 1, null);

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
    public List<MyTile> GetNearbyTile()
    {
        tempTiles.Clear();
        MyTile tile = GetMyTile();
        tempTiles.Add((MyTile)navManager.tilemap.GetTile(tile.posInCell));
        tempTiles.Add((MyTile)navManager.tilemap.GetTile(tile.posInCell + Vector3Int.right));
        tempTiles.Add((MyTile)navManager.tilemap.GetTile(tile.posInCell + Vector3Int.left));
        tempTiles.Add((MyTile)navManager.tilemap.GetTile(tile.posInCell + Vector3Int.up));
        tempTiles.Add((MyTile)navManager.tilemap.GetTile(tile.posInCell + Vector3Int.down));
        return tempTiles;
    }
    public void CheckMyTile()
    {
        curTile = GetMyTile();
        if (!curTile) { return; }
        if (lastTile == null) { lastTile = curTile; }
        if (lastTile.posInWorld != curTile.posInWorld)
        {
            InNewTile();
        }
    }
    /// <summary>
    /// 到达新地块
    /// </summary>
    public virtual void InNewTile()
    {
        lastTile = curTile;
        MessageBroker.Default.Publish(new GameEvent.GameEvent_Local_SomeoneMove
        {
            moveActor = this,
            moveTile = curTile
        });
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
        Debug.Log("拿到了手上" + data.Item_ID + "/" + data.Item_Val + "/" + data.Item_Count);
        ResetItem_Hand();
        /*更新UI*/
        if (isPlayer && netController.Object.HasInputAuthority)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateItemInHand()
            {
                itemData = data
            });
        }
        if (data.Item_ID != 0) 
        {
            Type type = Type.GetType("Item_" + data.Item_ID.ToString());
            holdingByHand = (ItemBase)Activator.CreateInstance(type);
            holdingByHand.UpdateData(data);
            holdingByHand.BeHolding(this, BodyController);
        }
    }
    public void ResetItem_Hand()
    {
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
    /// 掉落
    /// </summary>
    public virtual void Loot()
    {
        for (int i = 0; i < actorConfig.Config_LootCount; i++)
        {
            int id = GetLootID();
            if (id != 0)
            {
                ItemData item = new ItemData();
                item.Item_ID = id;
                item.Item_Count = 1;
                item.Item_Seed = System.DateTime.Now.Second + i + id;

                MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnItem()
                {
                    itemData = item,
                    pos = transform.position - new Vector3(0, 0.1f, 0)
                });
            }
        }
        for (int i = 0; i < netController.Data_ItemInBag.Count; i++)
        {
            ItemData item = netController.Data_ItemInBag[i];
            MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnItem()
            {
                itemData = item,
                pos = transform.position - new Vector3(0, 0.1f, 0)
            });
        }
    }
    /// <summary>
    /// 根据权重获得一个掉落物id
    /// </summary>
    /// <returns></returns>
    private int GetLootID()
    {
        int weight_Main = 0;
        int weight_temp = 0;
        int random = 0;
        for (int j = 0; j < actorConfig.Config_LootList.Count; j++)
        {
            weight_Main += actorConfig.Config_LootList[j].Weight;
        }
        random = UnityEngine.Random.Range(0, weight_Main);
        for (int j = 0; j < actorConfig.Config_LootList.Count; j++)
        {
            weight_temp += actorConfig.Config_LootList[j].Weight;
            if (weight_temp > random)
            {
                return actorConfig.Config_LootList[j].ID;
            }
        }
        return 0;
    }

    #endregion
    /*寻路*/
    #region
    public List<MyTile> tempPath = new List<MyTile>();
    public MyTile targetTile = null;
    public void UpdatePath(float dt)
    {
        if (targetTile)
        {
            /*到达路径点，检查*/
            if (Vector2.Distance(targetTile.posInWorld, transform.position) <= 0.1f)
            {
                targetTile = null;
                /*如果路径还未结束*/
                if (tempPath.Count > 0)
                {
                    tempPath.RemoveAt(0);
                    if (tempPath.Count > 0)
                    {
                        targetTile = tempPath[0];
                    }
                }
                CheckMyTile();
                return;
            }
            /*到达路径点，前往路径点*/
            else
            {
                Vector2 temp = Vector2.zero;
                if (transform.position.x > targetTile.posInWorld.x)
                {
                    if ((transform.position.x - targetTile.posInWorld.x) > 0.05f)
                    {
                        temp += new Vector2(-1, 0);
                    }
                }
                else
                {
                    if ((transform.position.x - targetTile.posInWorld.x) < -0.05f)
                    {
                        temp += new Vector2(1, 0);
                    }
                }
                if (transform.position.y > targetTile.posInWorld.y)
                {
                    if ((transform.position.y - targetTile.posInWorld.y) > 0.05f)
                    {
                        temp += new Vector2(0, -1);
                    }
                }
                else
                {
                    if ((transform.position.y - targetTile.posInWorld.y) < -0.05f)
                    {
                        temp += new Vector2(0, 1);
                    }
                }
                temp = temp.normalized;
                netController.UpdateNetworkTransform(transform.position + new UnityEngine.Vector3(temp.x * dt * actorConfig.Config_Speed, temp.y * dt * actorConfig.Config_Speed, 0));
            }
        }
    }
    /// <summary>
    /// 找到一条路径
    /// </summary>
    /// <param name="targetPos"></param>
    public virtual void FindWayToTarget(Vector2 targetPos)
    {
        tempPath = navManager.FindPath(navManager.FindTileByPos(targetPos), GetMyTile());
        if (tempPath != null)
        {
            /*按照路径移动时会直接跳过第一个路径点，防止横跳*/
            if (tempPath.Count > 1)
            {
                tempPath.RemoveAt(0);
            }
            targetTile = tempPath[0];
        }
    }
    public virtual void CalculatePath(Vector2 from, Vector2 to)
    {
        if (tempPath != null)
        {
            tempPath.Clear();
        }
        tempPath = navManager.FindPath(navManager.FindTileByPos(to), navManager.FindTileByPos(from));
    }
    #endregion
    /*基本属性变化*/
    #region
    /// <summary>
    /// 受伤
    /// </summary>
    /// <param name="val"></param>
    /// <param name="id"></param>
    public void TakeDamage(int val,Fusion.NetworkId id)
    {
        if (actorState != ActorState.Dead)
        {
            netController.RPC_HpChange(-val, id);
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
    public void Dead()
    {
        if (isState)
        {
            Loot();
            if (!isPlayer)
            {
                netController.Runner.Despawn(netController.Object);
            }
        }
    }

    #endregion
    /*RPC*/
    public virtual void FromRPC_Skill(int parameter, Fusion.NetworkId id)
    {

    }
    public virtual void Listen_HpChange(int parameter, Fusion.NetworkId id)
    {
        if (parameter < 0)
        {
            GameObject obj_num = UIManager.Instance.ShowUI("UI/UI_DamageNum", (Vector2)transform.position + new Vector2(0, 1));
            obj_num.GetComponent<UI_DamageNum>().Play(parameter, new Color32(255, 50, 50, 255));
            PlayTakeDamage(1);
        }
    }
}
[Serializable]
public struct ActorConfig
{
    public float Config_Speed;
    public int Config_LootCount;
    public List<LootInfo> Config_LootList;
}
public enum ActorState
{
    Default,
    Dead
}