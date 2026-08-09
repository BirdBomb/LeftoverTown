using DG.Tweening;
using Fusion;
using System;
using System.Collections;
using UnityEngine;
using static GameEvent;
/// <summary>
/// 角色管理器
/// </summary>
public class ActorManager : MonoBehaviour
{
    [Header("角色ID")]
    public int actorID;
    [Header("角色UI")]
    public ActorUI actorUI;
    [HideInInspector]
    public ActorConfig actorConfig;
    public ActorNetManager actorNetManager;
    public BodyController_Base bodyController = new BodyController_Base();
    public ActorHpManager actorHpManager = new ActorHpManager(); 
    public ActorHungryManager hungryManager = new ActorHungryManager();
    public ActorSanManager sanManager = new ActorSanManager(); 
    public ActorBuffManager buffManager = new ActorBuffManager();
    public ActorItemManager itemManager = new ActorItemManager();
    public ActorStatusManager statusManager = new ActorStatusManager();
    public ActorInputManager inputManager = new ActorInputManager();
    public ActorActionManager actionManager = new ActorActionManager();
    public ActorPathManager pathManager = new ActorPathManager();
    public ActorBrainManager brainManager = new ActorBrainManager();
    public ActorViewManager viewManager = new ActorViewManager();
    public ActorVehicleManager vehicleManager = new ActorVehicleManager();

    [HideInInspector]
    public ActorState actorState;
    [HideInInspector]
    public ActorAuthority actorAuthority;
    [HideInInspector]
    public PlayerRef actorPlayerRef;

    public virtual void Awake()
    {
        actorUI.Bind(this);
        viewManager.Bind(this);
        actorHpManager.Bind(this);
        hungryManager.Bind(this);
        sanManager.Bind(this);
        buffManager.Bind(this);
        itemManager.Bind(this);
        statusManager.Bind(this);
        inputManager.Bind(this);
        actionManager.Bind(this);
        pathManager.Bind(this);
        brainManager.Bind(this);
        vehicleManager.Bind(this);
    }

    /*初始化*/
    #region
    public void AllClient_BindConfig()
    {
        actorConfig = ActorConfigData.GetActorConfig(actorID);
    }
    /// <summary>
    /// 客户端初始化 
    /// </summary>
    public virtual void AllClient_Init()
    {
        transform.localScale = Vector3.one;
        transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
        ForAll_AddListener();

        actorAuthority.isLocal = actorNetManager.Object.HasStateAuthority;
        actorAuthority.isState = actorNetManager.Object.HasStateAuthority;
    }
    /// <summary>
    /// 服务器初始化
    /// </summary>
    public virtual void State_Init()
    {
        actorNetManager.State_SendInfoToAll();
    }
    public void State_InitHeadAndBody(ItemData headItem, ItemData bodyItem)
    {
        actorNetManager.Local_ItemHead = headItem;
        actorNetManager.Local_ItemBody = bodyItem;
    }
    public void State_InitFace(string name,short eyeID,short hairID,Color32 hairColor)
    {
        actorNetManager.Local_Name = name;
        actorNetManager.Local_EyeID = eyeID;
        actorNetManager.Local_HairID = hairID;
        actorNetManager.Local_HairColor = hairColor;
    }
    public void State_InitAbilityData(short Hp, short armor, short resistance, short speed)
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
        actorNetManager.Local_PlaySimulation(local && !state);
    }
    #endregion
    /*监听*/
    #region
    /// <summary>
    /// 开始监听
    /// </summary>
    public virtual void ForAll_AddListener()
    {

    }
    /// <summary>
    /// 监听某物进入视野范围(客户端)
    /// </summary>
    /// <param name="obj"></param>
    public virtual void ForAll_Listen_ItemInView(ItemNetObj obj)
    {
        brainManager.ForAll_AddNearbyItem(obj);
        if (actorAuthority.isState) ForState_Listen_ItemInView(obj);
    }
    /// <summary>
    /// 监听某物进入视野范围(服务器)
    /// </summary>
    /// <param name="obj"></param>
    public virtual void ForState_Listen_ItemInView(ItemNetObj obj)
    {

    }
    /// <summary>
    /// 监听某物离开视野范围(客户端)
    /// </summary>
    /// <param name="obj"></param>
    public virtual void ForAll_Listen_ItemOutView(ItemNetObj obj)
    {
        brainManager.ForAll_RemoveNearbyItem(obj);
        if (actorAuthority.isState) ForState_Listen_ItemOutView(obj);
    }
    /// <summary>
    /// 监听某物离开视野范围(服务器)
    /// </summary>
    /// <param name="obj"></param>
    public virtual void ForState_Listen_ItemOutView(ItemNetObj obj)
    {

    }
    /// <summary>
    /// 监听某人进入视野范围(服务器)
    /// </summary>
    public virtual void State_Listen_RoleInView(ActorManager actor)
    {
        brainManager.State_AddNearbyActors(actor);
    }
    /// <summary>
    /// 监听某人进入视野范围(本地)
    /// </summary>
    public virtual void AllClient_Listen_RoleInView(ActorManager actor)
    {
        
    }
    /// <summary>
    /// 监听某人离开视野范围(服务器)
    /// </summary>
    public virtual void State_Listen_RoleOutView(ActorManager actor)
    {
        brainManager.State_RemoveNearbyActors(actor);
    }
    /// <summary>
    /// 监听某人离开视野范围(本地)
    /// </summary>
    public virtual void AllClient_Listen_RoleOutView(ActorManager actor)
    {
        
    }
    public virtual void ForAll_Listen_UpdateTime(GameEvent_All_UpdateHour eventData)
    {
        if (actorAuthority.isState) ForState_Listen_UpdateTime(eventData);
    }
    public virtual void ForState_Listen_UpdateTime(GameEvent_All_UpdateHour eventData)
    {

    }
    public virtual void ForAll_Listen_MoveMyself(Vector3Int pos)
    {
        buffManager.Listen_Move(pos);
        if (actorAuthority.isState) ForState_Listen_MoveMyself(pos);
    }
    public virtual void ForState_Listen_MoveMyself(Vector3Int pos)
    {
        
    }
    public virtual void ForAll_Listen_RoleCommit(GameEvent_AllClient_SomeoneCommit eventData)
    {
        if (actorAuthority.isState) ForState_Listen_RoleCommit(eventData);
    }
    public virtual void ForState_Listen_RoleCommit(GameEvent_AllClient_SomeoneCommit eventData)
    {

    }
    public virtual void ForAll_Listen_RoleSendEmoji(GameEvent_AllClient_SomeoneSendEmoji eventData)
    {
        if (actorAuthority.isState) ForState_Listen_RoleSendEmoji(eventData);
    }
    public virtual void ForState_Listen_RoleSendEmoji(GameEvent_AllClient_SomeoneSendEmoji eventData)
    {

    }
    public virtual void ForAll_Listen_MyselfHpChange(int parameter, HpChangeReason reason, Fusion.NetworkId id)
    {
        Color32 color32 = new Color32();
        NumPlayType numPlayType = new NumPlayType();
        string showText = "";
        switch (reason)
        {
            case HpChangeReason.AttackDamage:
                color32 = new Color32(255, 100, 0, 255);
                numPlayType = NumPlayType.Jump;
                showText = Math.Round(parameter * 0.1f, 1).ToString();
                break;
            case HpChangeReason.MagicDamage:
                color32 = new Color32(200, 0, 255, 255);
                numPlayType = NumPlayType.Jump;
                showText = Math.Round(parameter * 0.1f, 1).ToString();
                break;
            case HpChangeReason.RealDamage:
                color32 = new Color32(255, 255, 255, 255);
                numPlayType = NumPlayType.Jump;
                showText = Math.Round(parameter * 0.1f, 1).ToString();
                break;
            case HpChangeReason.Healing:
                color32 = new Color32(0, 255, 0, 255);
                numPlayType = NumPlayType.Float;
                showText = $"+{Math.Round(parameter * 0.1f, 1)}";
                break;
        }
        AllClient_ShowNumUI(showText, color32, Vector2.up, numPlayType);
    }
    public virtual void ForState_Listen_MyselfDead(int parameter, HpChangeReason reason, Fusion.NetworkId id)
    {

    }
    public virtual void ForState_Listen_MyselfInjured(int parameter, HpChangeReason reason, Fusion.NetworkId id)
    {

    }
    public virtual void ForAll_Listen_ChangeAttackState(bool attacking)
    {
        brainManager.allClient_AttackingRunning = attacking;
        if (actorAuthority.isState) ForState_Listen_ChangeAttackState(attacking);
    }
    public virtual void ForState_Listen_ChangeAttackState(bool attacking)
    {
       
    }
    public virtual void ForAll_Listen_ChangeAttackTarget(NetworkId id)
    {
        brainManager.ForAll_SetAttackTarget((id == new NetworkId()) ? null : actorNetManager.Runner.FindObject(id).GetComponent<ActorManager>());
        brainManager.ForAll_SetAttackID(id);
        if (actorAuthority.isState) ForState_Listen_ChangeAttackTarget(id);
    }
    public virtual void ForState_Listen_ChangeAttackTarget(NetworkId id)
    {

    }
    public virtual void ForAll_Listen_ChangeThreatenedTarget(NetworkId id)
    {
        brainManager.ForAll_SetThreatenedTarget((id == new NetworkId()) ? null : actorNetManager.Runner.FindObject(id).GetComponent<ActorManager>());
        brainManager.ForAll_SetThreatenedID(id);
        if (actorAuthority.isState) ForState_Listen_ChangeThreatenedTarget(id);
    }
    public virtual void ForState_Listen_ChangeThreatenedTarget(NetworkId id)
    {

    }
    /// <summary>
    /// 监听NPC行为(客户端)
    /// </summary>
    /// <param name="id"></param>
    /// <param name="vector3"></param>
    /// <param name="networkId"></param>
    public virtual void ForAll_Listen_NpcAction(int id, Vector3Int vector3, Fusion.NetworkId networkId)
    {

    }
    #endregion
    /*计时*/
    #region
    protected const float const_customUpdateTime = 0.05f;
    protected const float const_customUpdateTimeRec = 20f;
    private float customUpdateTimer;
    private float secondUpdateTimer;
    public virtual void FixedUpdate()
    {
        customUpdateTimer += Time.fixedDeltaTime;
        if (customUpdateTimer > const_customUpdateTime) { customUpdateTimer = 0; CustomUpdate(const_customUpdateTime); }
        secondUpdateTimer += Time.fixedDeltaTime;
        if (secondUpdateTimer > 1) { secondUpdateTimer = 0; SecondUpdate(); }
    }
    /// <summary>
    /// 网络更新(主机)
    /// </summary>
    /// <param name="dt"></param>
    public virtual void State_FixedUpdateNetwork(float dt)
    {
        if (!actorAuthority.isPlayer) pathManager.State_RunningPath(dt);
    }
    public void CustomUpdate(float dt)
    {
        ForAll_CustomUpdate(dt);
    }
    public void SecondUpdate()
    {
        ForAll_SecondUpdate();
    }
    public virtual void ForAll_CustomUpdate(float dt)
    {
        bodyController.Local_CheckPos(const_customUpdateTime, const_customUpdateTimeRec);
        pathManager.ForAll_CheckTile(dt);
        if (actorAuthority.isState) { ForState_CustomUpdate(); }
    }
    public virtual void ForState_CustomUpdate()
    {
        pathManager.ForState_CheckTile();
    }
    public virtual void ForAll_SecondUpdate()
    {
        if (actorAuthority.isPlayer)
        {
            hungryManager.Listen_UpdateSecond();
            sanManager.Listen_UpdateSecond();
        }
        buffManager.Listen_UpdateSecond();
        itemManager.Listen_UpdateSecond(1);
        if (actorAuthority.isState) { State_SecondUpdate(); }
    }
    public virtual void State_SecondUpdate()
    {

    }
    #endregion
    /*UI*/
    #region
    public virtual void AllClient_ShowNumUI(string val, Color32 color, Vector2 pos, NumPlayType playType)
    {
        if (PoolManager.Instance.GetEffectObj("Effect/Effect_NumUI").TryGetComponent(out Effect_NumUI damageUI))
        {
            damageUI.transform.position = (Vector2)transform.position + pos;
            damageUI.Init(val, color, playType);
        }
    }
    public virtual void AllClient_UpdateHpBar(float val)
    {
        
    }
    #endregion
    /*交互*/
    #region
    /// <summary>
    /// 是否可以交互
    /// </summary>
    /// <returns></returns>
    public virtual bool Local_IsInteractable()
    {
        return false;
    }
    /// <summary>
    /// 靠近玩家
    /// </summary>
    public virtual void Local_PlayerClose(ActorManager player)
    {
        
    }
    /// <summary>
    /// 远离玩家
    /// </summary>
    public virtual void Local_PlayerFaraway(ActorManager player)
    {

    }
    /// <summary>
    /// 获取玩家输入
    /// </summary>
    public virtual void Local_GetPlayerInput(ActorManager actor, KeyCode keyCode)
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
/// <summary>
/// 伤害类型
/// </summary>
public enum DamageState
{
    /// <summary>
    /// 魔法伤害
    /// </summary>
    MagicDamage,
    /// <summary>
    /// 物理穿刺伤害
    /// </summary>
    AttackPiercingDamage,
    /// <summary>
    /// 物理钝击伤害
    /// </summary>
    AttackBludgeoningDamage,
    /// <summary>
    /// 物理劈砍伤害
    /// </summary>
    AttackSlashingDamage,
    /// <summary>
    /// 物理拆卸伤害
    /// </summary>
    AttackStructureDamage,
    /// <summary>
    /// 物理收割伤害
    /// </summary>
    AttackReapDamage,
    /// <summary>
    /// 真实伤害
    /// </summary>
    RealDamage,
}
public enum DamageTarget
{
    All,
    WithoutMe,

}
/// <summary>
/// 刑法
/// </summary>
public enum CommitState
{
    /// <summary>
    /// 偷窃罪
    /// </summary>
    Steal,
    /// <summary>
    /// 攻击罪
    /// </summary>
    Attacking,
    /// <summary>
    /// 谋杀罪
    /// </summary>
    Murder,
}
