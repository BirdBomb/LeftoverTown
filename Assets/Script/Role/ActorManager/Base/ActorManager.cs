using DG.Tweening;
using Fusion;
using Fusion.Addons.Physics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UniRx;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;
using static Fusion.Allocator;
using static GameEvent;
/// <summary>
/// 角色管理器
/// </summary>
public class ActorManager : MonoBehaviour
{
    [SerializeField, Header("物理刚体")]
    private Rigidbody2D rigidbody2;
    [Header("角色UI")]
    public ActorUI actorUI;
    [Header("网络控制器")]
    public ActorNetManager actorNetManager;
    [Header("身体控制器")]
    public BodyController_Base bodyController;
    [Header("预测输入")]
    public PlayerSimulation playerSimulation;
    [HideInInspector]
    public ActorState actorState;
    [HideInInspector]
    public ActorAuthority actorAuthority;
    [HideInInspector]
    public PlayerRef actorPlayerRef;

    /// <summary>
    /// 饥饿
    /// </summary>
    public ActorHungryManager hungryManager = new ActorHungryManager();
    /// <summary>
    /// Buff
    /// </summary>
    public ActorBuffManager buffManager = new ActorBuffManager();
    /// <summary>
    /// 物品
    /// </summary>
    public ActorItemManager itemManager = new ActorItemManager();
    /// <summary>
    /// 身份
    /// </summary>
    public ActorStatusManager statusManager = new ActorStatusManager();
    /// <summary>
    /// 输入
    /// </summary>
    public ActorInputManager inputManager = new ActorInputManager();
    /// <summary>
    /// 行为
    /// </summary>
    public ActorActionManager actionManager = new ActorActionManager();
    /// <summary>
    /// 路径
    /// </summary>
    public ActorPathManager pathManager = new ActorPathManager();
    /// <summary>
    /// 载具
    /// </summary>
    public ActorVehicleManager vehicleManager = new ActorVehicleManager();
    /// <summary>
    /// 大脑
    /// </summary>
    public ActorBrainManager brainManager = new ActorBrainManager();
    public ActorViewManager viewManager;
    public virtual void Awake()
    {
        rigidbody2.gravityScale = 0;
        hungryManager.Bind(this);
        buffManager.Bind(this);
        itemManager.Bind(this);
        statusManager.Bind(this);
        inputManager.Bind(this);
        actionManager.Bind(this);
        pathManager.Bind(this);
        vehicleManager.Bind(this);
        brainManager.Bind(this);
        viewManager = transform.GetComponentInChildren<ActorViewManager>();
        if (viewManager) viewManager.Bind(this);
    }
    public virtual void FixedUpdate()
    {

    }
    /*初始化*/
    #region
    /// <summary>
    /// 客户端初始化
    /// </summary>
    public virtual void Client_Init()
    {
        transform.localScale = Vector3.one;
        transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
        AllClient_StatrLoop();
        AllClient_AddListener();
        
        actorAuthority.isLocal = actorNetManager.Object.HasStateAuthority;
        actorAuthority.isState = actorNetManager.Object.HasStateAuthority;
        actorNetManager.Client_RequestInfo();
    }
    /// <summary>
    /// 服务器初始化
    /// </summary>
    public virtual void State_Init()
    {
        actorNetManager.State_SendInfoToAll();
    }
    public void State_SetHeadAndBody(ItemData headItem, ItemData bodyItem)
    {
        actorNetManager.Net_ItemOnHead = headItem;
        actorNetManager.Net_ItemOnBody = bodyItem;
    }
    public void State_SetFace(string name,short eyeID,short hairID,Color32 hairColor)
    {
        actorNetManager.Local_Name = name;
        actorNetManager.Local_EyeID = eyeID;
        actorNetManager.Local_HairID = hairID;
        actorNetManager.Local_HairColor = hairColor;
    }
    public void State_SetAbilityData(short Hp, short armor, short resistance, short speed)
    {
        actorNetManager.Net_HpCur = Hp;
        actorNetManager.Local_HpMax = Hp;
        actorNetManager.Net_Armor = armor;
        actorNetManager.Net_Resistance = resistance;
        actorNetManager.Net_SpeedCommon = speed;
    }
    #endregion
    /*绑定玩家*/
    #region
    /// <summary>
    /// 绑定玩家
    /// </summary>
    /// <param name="state"></param>
    /// <param name="local"></param>
    public void AllClient_BindPlayer(bool state, bool local, PlayerRef playerRef)
    {
        actorAuthority.isPlayer = true;
        actorAuthority.isState = state;
        actorAuthority.isLocal = local;
        actorPlayerRef = playerRef;
    }
    #endregion
    /*监听*/
    #region
    /// <summary>
    /// 开始监听
    /// </summary>
    public virtual void AllClient_AddListener()
    {

    }
    /// <summary>
    /// 监听某物进入视野范围(服务器)
    /// </summary>
    /// <param name="obj"></param>
    public virtual void State_Listen_ItemInView(ItemNetObj obj)
    {

    }
    /// <summary>
    /// 监听某物离开视野范围(服务器)
    /// </summary>
    /// <param name="obj"></param>
    public virtual void State_Listen_ItemOutView(ItemNetObj obj)
    {

    }
    /// <summary>
    /// 监听某人进入视野范围(客户端)
    /// </summary>
    public virtual void AllClient_Listen_RoleInView(ActorManager actor)
    {

    }
    /// <summary>
    /// 监听某人进入视野范围(服务器)
    /// </summary>
    public virtual void State_Listen_RoleInView(ActorManager actor)
    {

    }
    /// <summary>
    /// 监听某人离开视野范围(客户端)
    /// </summary>
    public virtual void AllClient_Listen_RoleOutView(ActorManager actor)
    {

    }
    /// <summary>
    /// 监听某人离开视野范围(服务器)
    /// </summary>
    public virtual void State_Listen_RoleOutView(ActorManager actor)
    {

    }
    /// <summary>
    /// 监听时间改变(客户端)
    /// </summary>
    /// <param name="globalTime"></param>
    public virtual void AllClient_Listen_UpdateTime(int hour, int date, GlobalTime globalTime)
    {
        if (actorAuthority.isState) State_Listen_WorldGlobalTimeChange(hour, date, globalTime);
    }
    /// <summary>
    /// 监听时间改变(服务器)
    /// </summary>
    /// <param name="hour"></param>
    /// <param name="date"></param>
    /// <param name="globalTime"></param>
    public virtual void State_Listen_WorldGlobalTimeChange(int hour, int date, GlobalTime globalTime)
    {

    }
    /// <summary>
    /// 监听我自己移动(客户端)
    /// </summary>
    /// <param name="who"></param>
    /// <param name="where"></param>
    public virtual void AllClient_Listen_MoveMyself(Vector3Int pos)
    {
        buffManager.Listen_Move(pos);
        pathManager.UpdateNearbyBuilding();
    }
    /// <summary>
    /// 监听我自己移动(主机)
    /// </summary>
    /// <param name="who"></param>
    /// <param name="where"></param>
    public virtual void State_Listen_MoveMyself(Vector3Int pos)
    {
        pathManager.CheckDistance();
    }
    /// <summary>
    /// 监听其他人移动(客户端)
    /// </summary>
    /// <param name="who"></param>
    /// <param name="where"></param>
    public virtual void AllClient_Listen_MoveOther(ActorManager who, Vector3Int where)
    {

    }
    /// <summary>
    /// 监听其他人移动(主机)
    /// </summary>
    /// <param name="who"></param>
    /// <param name="where"></param>
    public virtual void State_Listen_MoveOther(ActorManager who, Vector3Int where)
    {

    }
    /// <summary>
    /// 监听某人做了什么事情(客户端)
    /// </summary>
    /// <param name="who"></param>
    /// <param name="actorAction"></param>
    public virtual void AllClient_Listen_RoleDoSomething(ActorManager who, ActorAction actorAction)
    {

    }
    /// <summary>
    /// 监听某人做了什么事情(主机)
    /// </summary>
    /// <param name="who"></param>
    /// <param name="actorAction"></param>
    public virtual void State_Listen_RoleDoSomething(ActorManager who, ActorAction actorAction)
    {

    }
    /// <summary>
    /// 监听某人犯法(客户端)
    /// </summary>
    /// <param name="who"></param>
    /// <param name="actorAction"></param>
    public virtual void AllClient_Listen_RoleCommit(ActorManager who, short val)
    {
    }
    /// <summary>
    /// 监听某人犯法(主机)
    /// </summary>
    /// <param name="who"></param>
    /// <param name="val"></param>
    public virtual void State_Listen_RoleCommit(ActorManager who, short val)
    {

    }
    /// <summary>
    /// 监听某人发送emoji(客户端)
    /// </summary>
    /// <param name="actor">谁</param>
    /// <param name="id">什么</param>
    /// <param name="distance">距离</param>
    public virtual void AllClient_Listen_RoleSendEmoji(ActorManager actor, Emoji emoji, float distance)
    {

    }
    /// <summary>
    /// 监听某人发送emoji(主机)
    /// </summary>
    /// <param name="actor"></param>
    /// <param name="id"></param>
    /// <param name="distance"></param>
    public virtual void State_Listen_RoleSendEmoji(ActorManager actor, Emoji emoji, float distance)
    {

    }
    /// <summary>
    /// 监听生命值改变(客户端)
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="id"></param>
    public virtual void AllClient_Listen_MyselfHpChange(int parameter, Fusion.NetworkId id)
    {
        if (parameter <= 0)
        {
            if(actorAuthority.isPlayer && actorAuthority.isLocal)
            {
                AllClient_ShowText(parameter.ToString(), new Color32(255, 0, 0, 255));
            }
            else
            {
                AllClient_ShowText(parameter.ToString(), new Color32(255, 100, 0, 255));
            }
            actionManager.PlayTakeDamage(1);
        }
    }
    /// <summary>
    ///  监听生命值改变(主机)
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="id"></param>
    public virtual void State_Listen_MyselfHpChange(int parameter, Fusion.NetworkId id)
    {

    }
    /// <summary>
    /// 监听攻击状态变化(客户端)
    /// </summary>
    /// <param name="attacking"></param>
    public virtual void AllClient_Listen_ChangeAttackState(bool attacking)
    {
        brainManager.bool_AttackState = attacking;
    }
    /// <summary>
    /// 监听攻击状态变化(主机)
    /// </summary>
    /// <param name="attacking"></param>
    public virtual void State_Listen_ChangeAttackState(bool attacking)
    {
       
    }
    /// <summary>
    /// 监听攻击目标变化(客户端)
    /// </summary>
    /// <param name="id"></param>
    public virtual void AllClient_Listen_ChangeAttackTarget(NetworkId id)
    {
        brainManager.allClient_actorManager_AttackTarget = (id == new NetworkId()) ? null : actorNetManager.Runner.FindObject(id).GetComponent<ActorManager>();
        brainManager.allClient_actorManager_AttackTargetID = id;
    }
    /// <summary>
    /// 监听攻击目标变化(主机)
    /// </summary>
    /// <param name="id"></param>
    public virtual void State_Listen_ChangeAttackTarget(NetworkId id)
    {

    }
    /// <summary>
    /// 监听受伤(客户端)
    /// </summary>
    /// <param name="val"></param>
    /// <param name="from"></param>
    public virtual void AllClient_Listen_TakeAttackDamage(int val, ActorNetManager from)
    {
        actionManager.TakeAttackDamage(val, from);
    }
    public virtual void AllClient_Listen_TakeMagicDamage(int val, ActorNetManager from)
    {

    }
    /// <summary>
    /// 监听治疗(客户端)
    /// </summary>
    public virtual void AllClient_Listen_Heal(int val)
    {
        actionManager.Client_HealHP(val);
    }
    /// <summary>
    /// 监听NPC行为(客户端)
    /// </summary>
    /// <param name="id"></param>
    /// <param name="vector3"></param>
    /// <param name="networkId"></param>
    public virtual void AllClient_Listen_NpcAction(int id, Vector3Int vector3, Fusion.NetworkId networkId)
    {

    }
    #endregion
    /*计时*/
    #region
    /// <summary>
    /// 自定义更新间隔
    /// </summary>
    protected const float const_customUpdateTime = 0.1f;
    /// <summary>
    /// 开始自定义循环(客户端)
    /// </summary>
    private void AllClient_StatrLoop()
    {
        InvokeRepeating("CustomUpdate", 1f, const_customUpdateTime);
        InvokeRepeating("SecondUpdate", 1f, 1f);
    }
    /// <summary>
    /// 网络更新(客户端)
    /// </summary>
    /// <param name="dt"></param>
    public virtual void AllClient_FixedUpdateNetwork(float dt)
    {

    }
    /// <summary>
    /// 网络更新(主机)
    /// </summary>
    /// <param name="dt"></param>
    public virtual void State_FixedUpdateNetwork(float dt)
    {
        if (!actorAuthority.isPlayer) pathManager.State_RunningPath(dt);
    }
    /// <summary>
    /// 网络更新(客户端)
    /// </summary>
    /// <param name="dt"></param>
    public virtual void AllClient_Render(float dt)
    {

    }
    /// <summary>
    /// 网络更新(主机)
    /// </summary>
    /// <param name="dt"></param>
    public virtual void State_Render(float dt)
    {

    }
    public void CustomUpdate()
    {
        Local_CustomUpdate();
        if (actorAuthority.isState) { State_CustomUpdate(); }
    }
    public void SecondUpdate()
    {
        Local_SecondUpdate();
        if (actorAuthority.isState) { State_SecondUpdate(); }
    }
    /// <summary>
    /// 客户端自定义更新
    /// </summary>
    public virtual void Local_CustomUpdate()
    {
        bodyController.Local_CheckPos(const_customUpdateTime);
        pathManager.Local_CheckTile();
    }
    /// <summary>
    /// 服务器自定义更新
    /// </summary>
    public virtual void State_CustomUpdate()
    {

    }
    public virtual void Local_SecondUpdate()
    {
        hungryManager.Listen_UpdateSecond();
        buffManager.Listen_UpdateSecond();
        itemManager.Listen_UpdateSecond(1);
    }
    public virtual void State_SecondUpdate()
    {

    }
    #endregion
    /*UI*/
    #region
    public virtual void AllClient_ShowText(string val,Color32 color)
    {
        Vector2 offset = 0.025f * new Vector2(new System.Random().Next(-10, 10), new System.Random().Next(-5, 5));
        Effect_DamageUI damageUI = PoolManager.Instance.GetObject("Effect/Effect_DamageUI").GetComponent<Effect_DamageUI>();
        damageUI.transform.position = (Vector2)transform.position + Vector2.up;
        damageUI.PlayShow(val, color, offset);
    }
    public virtual void AllClient_UpdateHpBar(float val)
    {
        actorUI.UpdateHPBar(val);
    }
    #endregion
    /*交互*/
    #region
    /// <summary>
    /// 进入玩家视野
    /// </summary>
    public virtual void Local_InPlayerView(ActorManager actor)
    {

    }
    /// <summary>
    /// 离开玩家视野
    /// </summary>
    public virtual void Local_OutPlayerView(ActorManager actor)
    {

    }
    /// <summary>
    /// 获取玩家输入
    /// </summary>
    public virtual void Local_GetPlayerInput(KeyCode keyCode, ActorManager actor)
    {

    }
    #endregion
}
/// <summary>
/// 角色状态
/// </summary>
public enum ActorState
{
    Default,
    Dead
}
/// <summary>
/// 角色权限
/// </summary>
public struct ActorAuthority
{
    /// <summary>
    /// 是否是玩家
    /// </summary>
    public bool isPlayer;
    /// <summary>
    /// 是否是主机
    /// </summary>
    public bool isState;
    /// <summary>
    /// 是否是本地
    /// </summary>
    public bool isLocal;
}