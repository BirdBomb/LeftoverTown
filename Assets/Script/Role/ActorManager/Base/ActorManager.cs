using DG.Tweening;
using Fusion;
using UnityEngine;
using static GameEvent;
/// <summary>
/// ��ɫ������
/// </summary>
public class ActorManager : MonoBehaviour
{
    [Header("���ض��ƶ�Ԥ��")]
    public PlayerSimulation playerSimulation;
    [HideInInspector]
    public Rigidbody2D rigidbody2;
    [HideInInspector]
    public ActorUI actorUI;
    [HideInInspector]
    public ActorNetManager actorNetManager;
    [HideInInspector]
    public BodyController_Base bodyController;
    [HideInInspector]
    public ActorState actorState;
    [HideInInspector]
    public ActorAuthority actorAuthority;
    [HideInInspector]
    public PlayerRef actorPlayerRef;

    /// <summary>
    /// ����
    /// </summary>
    public ActorHungryManager hungryManager = new ActorHungryManager();
    /// <summary>
    /// ����
    /// </summary>
    public ActorSanManager sanManager = new ActorSanManager(); 
    /// <summary>
    /// Buff
    /// </summary>
    public ActorBuffManager buffManager = new ActorBuffManager();
    /// <summary>
    /// ��Ʒ
    /// </summary>
    public ActorItemManager itemManager = new ActorItemManager();
    /// <summary>
    /// ���
    /// </summary>
    public ActorStatusManager statusManager = new ActorStatusManager();
    /// <summary>
    /// ����
    /// </summary>
    public ActorInputManager inputManager = new ActorInputManager();
    /// <summary>
    /// ��Ϊ
    /// </summary>
    public ActorActionManager actionManager = new ActorActionManager();
    /// <summary>
    /// ·��
    /// </summary>
    public ActorPathManager pathManager = new ActorPathManager();
    /// <summary>
    /// �ؾ�
    /// </summary>
    public ActorVehicleManager vehicleManager = new ActorVehicleManager();
    /// <summary>
    /// ����
    /// </summary>
    public ActorBrainManager brainManager = new ActorBrainManager();
    [HideInInspector]
    public ActorViewManager viewManager;
    public virtual void Awake()
    {
        viewManager = transform.GetComponentInChildren<ActorViewManager>();
        if (viewManager) { viewManager.Bind(this); }
        else { Debug.Log("δ�ҵ���Ұ������"); }

        rigidbody2 = transform.GetComponent<Rigidbody2D>();
        if (rigidbody2) {  }
        else { Debug.Log("δ�ҵ���Ұ������"); }

        actorUI = transform.GetComponentInChildren<ActorUI>();
        if (actorUI) { }
        else { Debug.Log("δ�ҵ���ɫUI"); }

        actorNetManager = transform.GetComponent<ActorNetManager>();
        if (actorNetManager) { }
        else { Debug.Log("δ�ҵ����������"); }

        bodyController = transform.GetComponentInChildren<BodyController_Base>();
        if (bodyController) { }
        else { Debug.Log("δ�ҵ����������"); }

        rigidbody2.gravityScale = 0;
        hungryManager.Bind(this);
        sanManager.Bind(this);
        buffManager.Bind(this);
        itemManager.Bind(this);
        statusManager.Bind(this);
        inputManager.Bind(this);
        actionManager.Bind(this);
        pathManager.Bind(this);
        vehicleManager.Bind(this);
        brainManager.Bind(this);
    }

    public virtual void FixedUpdate()
    {

    }
    /*��ʼ��*/
    #region
    /// <summary>
    /// �ͻ��˳�ʼ�� 
    /// </summary>
    public virtual void AllClient_Init()
    {
        transform.localScale = Vector3.one;
        transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
        AllClient_StatrLoop();
        AllClient_AddListener();

        actorAuthority.isLocal = actorNetManager.Object.HasStateAuthority;
        actorAuthority.isState = actorNetManager.Object.HasStateAuthority;
    }
    /// <summary>
    /// ��������ʼ��
    /// </summary>
    public virtual void State_Init()
    {
        actorNetManager.State_SendInfoToAll();
    }
    public void State_SetHeadAndBody(ItemData headItem, ItemData bodyItem)
    {
        actorNetManager.Net_ItemHead = headItem;
        actorNetManager.Net_ItemBody = bodyItem;
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
    /*�����*/
    #region
    /// <summary>
    /// �����
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
    /*����*/
    #region
    /// <summary>
    /// ��ʼ����
    /// </summary>
    public virtual void AllClient_AddListener()
    {
    }
    /// <summary>
    /// ����ĳ�������Ұ��Χ(������)
    /// </summary>
    /// <param name="obj"></param>
    public virtual void State_Listen_ItemInView(ItemNetObj obj)
    {

    }
    /// <summary>
    /// ����ĳ�������Ұ��Χ(�ͻ���)
    /// </summary>
    /// <param name="obj"></param>
    public virtual void AllClient_Listen_ItemInView(ItemNetObj obj)
    {

    }
    /// <summary>
    /// ����ĳ���뿪��Ұ��Χ(������)
    /// </summary>
    /// <param name="obj"></param>
    public virtual void State_Listen_ItemOutView(ItemNetObj obj)
    {

    }
    /// <summary>
    /// ����ĳ���뿪��Ұ��Χ(�ͻ���)
    /// </summary>
    /// <param name="obj"></param>
    public virtual void AllClient_Listen_ItemOutView(ItemNetObj obj)
    {

    }
    /// <summary>
    /// ����ĳ�˽�����Ұ��Χ(�ͻ���)
    /// </summary>
    public virtual void AllClient_Listen_RoleInView(ActorManager actor)
    {

    }
    /// <summary>
    /// ����ĳ�˽�����Ұ��Χ(������)
    /// </summary>
    public virtual void State_Listen_RoleInView(ActorManager actor)
    {

    }
    /// <summary>
    /// ����ĳ���뿪��Ұ��Χ(�ͻ���)
    /// </summary>
    public virtual void AllClient_Listen_RoleOutView(ActorManager actor)
    {

    }
    /// <summary>
    /// ����ĳ���뿪��Ұ��Χ(������)
    /// </summary>
    public virtual void State_Listen_RoleOutView(ActorManager actor)
    {

    }
    /// <summary>
    /// ����ʱ��ı�(�ͻ���)
    /// </summary>
    /// <param name="globalTime"></param>
    public virtual void AllClient_Listen_UpdateTime(int hour, int date, GlobalTime globalTime)
    {
        if (actorAuthority.isState) State_Listen_UpdateTime(hour, date, globalTime);
    }
    /// <summary>
    /// ����ʱ��ı�(������)
    /// </summary>
    /// <param name="hour"></param>
    /// <param name="date"></param>
    /// <param name="globalTime"></param>
    public virtual void State_Listen_UpdateTime(int hour, int date, GlobalTime globalTime)
    {

    }
    /// <summary>
    /// �������Լ��ƶ�(�ͻ���)
    /// </summary>
    /// <param name="who"></param>
    /// <param name="where"></param>
    public virtual void AllClient_Listen_MoveMyself(Vector3Int pos)
    {
        buffManager.Listen_Move(pos);
        pathManager.UpdateNearbyBuilding();
    }
    /// <summary>
    /// �������Լ��ƶ�(����)
    /// </summary>
    /// <param name="who"></param>
    /// <param name="where"></param>
    public virtual void State_Listen_MoveMyself(Vector3Int pos)
    {
        pathManager.CheckDistance();
    }
    /// <summary>
    /// �����������ƶ�(�ͻ���)
    /// </summary>
    /// <param name="who"></param>
    /// <param name="where"></param>
    public virtual void AllClient_Listen_MoveOther(ActorManager who, Vector3Int where)
    {

    }
    /// <summary>
    /// �����������ƶ�(����)
    /// </summary>
    /// <param name="who"></param>
    /// <param name="where"></param>
    public virtual void State_Listen_MoveOther(ActorManager who, Vector3Int where)
    {

    }
    /// <summary>
    /// ����ĳ������ʲô����(�ͻ���)
    /// </summary>
    /// <param name="who"></param>
    /// <param name="actorAction"></param>
    public virtual void AllClient_Listen_RoleDoSomething(ActorManager who, ActorAction actorAction)
    {

    }
    /// <summary>
    /// ����ĳ������ʲô����(����)
    /// </summary>
    /// <param name="who"></param>
    /// <param name="actorAction"></param>
    public virtual void State_Listen_RoleDoSomething(ActorManager who, ActorAction actorAction)
    {

    }
    /// <summary>
    /// ����ĳ�˷���(�ͻ���)
    /// </summary>
    /// <param name="who"></param>
    /// <param name="actorAction"></param>
    public virtual void AllClient_Listen_RoleCommit(ActorManager who, short val)
    {
    }
    /// <summary>
    /// ����ĳ�˷���(����)
    /// </summary>
    /// <param name="who"></param>
    /// <param name="val"></param>
    public virtual void State_Listen_RoleCommit(ActorManager who, short val)
    {

    }
    /// <summary>
    /// ����ĳ�˷���emoji(�ͻ���)
    /// </summary>
    /// <param name="actor">˭</param>
    /// <param name="id">ʲô</param>
    /// <param name="distance">����</param>
    public virtual void AllClient_Listen_RoleSendEmoji(ActorManager actor, Emoji emoji, float distance)
    {

    }
    /// <summary>
    /// ����ĳ�˷���emoji(����)
    /// </summary>
    /// <param name="actor"></param>
    /// <param name="id"></param>
    /// <param name="distance"></param>
    public virtual void State_Listen_RoleSendEmoji(ActorManager actor, Emoji emoji, float distance)
    {

    }
    /// <summary>
    /// ��������ֵ�ı�(�ͻ���)
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="id"></param>
    public virtual void AllClient_Listen_MyselfHpChange(int parameter, HpChangeReason reason, Fusion.NetworkId id)
    {
        if (parameter <= 0)
        {
            if (reason == HpChangeReason.AttackDamage)
            {
                AllClient_ShowText(parameter.ToString(), new Color32(255, 100, 0, 255));
            }
            if (reason == HpChangeReason.MagicDamage)
            {
                AllClient_ShowText(parameter.ToString(), new Color32(200, 0, 255, 255));
            }
            if (reason == HpChangeReason.RealDamage)
            {
                AllClient_ShowText(parameter.ToString(), new Color32(255, 255, 255, 255));
            }
            actionManager.PlayTakeDamage(1);
        }
        else
        {
            AllClient_ShowText(parameter.ToString(), new Color32(0, 255, 0, 255));
        }
    }
    /// <summary>
    ///  ��������ֵ�ı�(����)
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="id"></param>
    public virtual void State_Listen_MyselfHpChange(int parameter, HpChangeReason reason, Fusion.NetworkId id)
    {

    }
    /// <summary>
    /// ��������״̬�仯(�ͻ���)
    /// </summary>
    /// <param name="attacking"></param>
    public virtual void AllClient_Listen_ChangeAttackState(bool attacking)
    {
        brainManager.bool_AttackState = attacking;
    }
    /// <summary>
    /// ��������״̬�仯(����)
    /// </summary>
    /// <param name="attacking"></param>
    public virtual void State_Listen_ChangeAttackState(bool attacking)
    {
       
    }
    /// <summary>
    /// ��������Ŀ��仯(�ͻ���)
    /// </summary>
    /// <param name="id"></param>
    public virtual void AllClient_Listen_ChangeAttackTarget(NetworkId id)
    {
        brainManager.allClient_actorManager_AttackTarget = (id == new NetworkId()) ? null : actorNetManager.Runner.FindObject(id).GetComponent<ActorManager>();
        brainManager.allClient_actorManager_AttackTargetID = id;
    }
    /// <summary>
    /// ��������Ŀ��仯(����)
    /// </summary>
    /// <param name="id"></param>
    public virtual void State_Listen_ChangeAttackTarget(NetworkId id)
    {

    }
    /// <summary>
    /// ��������(�ͻ���)
    /// </summary>
    /// <param name="val">��ֵ</param>
    /// <param name="damageState">����</param>
    /// <param name="from">��Դ</param>
    public virtual void AllClient_Listen_TakeDamage(int val, DamageState damageState,ActorNetManager from)
    {
        actionManager.TakeDamage(val, damageState, from);
    }
    /// <summary>
    /// ��������(�ͻ���)
    /// </summary>
    public virtual void AllClient_Listen_Heal(int val)
    {
        actionManager.Client_HealHP(val);
    }
    /// <summary>
    /// ����NPC��Ϊ(�ͻ���)
    /// </summary>
    /// <param name="id"></param>
    /// <param name="vector3"></param>
    /// <param name="networkId"></param>
    public virtual void AllClient_Listen_NpcAction(int id, Vector3Int vector3, Fusion.NetworkId networkId)
    {

    }
    #endregion
    /*��ʱ*/
    #region
    /// <summary>
    /// �Զ�����¼��
    /// </summary>
    protected const float const_customUpdateTime = 0.1f;
    /// <summary>
    /// ��ʼ�Զ���ѭ��(�ͻ���)
    /// </summary>
    private void AllClient_StatrLoop()
    {
        InvokeRepeating("CustomUpdate", 1f, const_customUpdateTime);
        InvokeRepeating("SecondUpdate", 1f, 1f);
    }
    /// <summary>
    /// �������(����)
    /// </summary>
    /// <param name="dt"></param>
    public virtual void State_FixedUpdateNetwork(float dt)
    {
        if (!actorAuthority.isPlayer) pathManager.State_RunningPath(dt);
    }
    /// <summary>
    /// �������(�ͻ���)
    /// </summary>
    /// <param name="dt"></param>
    public virtual void AllClient_Render(float dt)
    {

    }
    /// <summary>
    /// �������(����)
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
    /// �ͻ����Զ������
    /// </summary>
    public virtual void Local_CustomUpdate()
    {
        bodyController.Local_CheckPos(const_customUpdateTime);
        pathManager.Local_CheckTile();
    }
    /// <summary>
    /// �������Զ������
    /// </summary>
    public virtual void State_CustomUpdate()
    {

    }
    public virtual void Local_SecondUpdate()
    {
        hungryManager.Listen_UpdateSecond();
        sanManager.Listen_UpdateSecond();
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
        Effect_NumUI damageUI = PoolManager.Instance.GetObject("Effect/Effect_NumUI").GetComponent<Effect_NumUI>();
        damageUI.transform.position = (Vector2)transform.position + Vector2.up;
        damageUI.PlayShow(val, color, offset);
    }
    public virtual void AllClient_UpdateHpBar(float val)
    {
        actorUI.UpdateHPBar(val);
    }
    #endregion
    /*����*/
    #region
    /// <summary>
    /// �Ƿ���ԶԻ�
    /// </summary>
    /// <returns></returns>
    public virtual bool Local_CanDialog()
    {
        return false;
    }
    /// <summary>
    /// �������
    /// </summary>
    public virtual void Local_PlayerClose(ActorManager player)
    {
        
    }
    /// <summary>
    /// Զ�����
    /// </summary>
    public virtual void Local_PlayerFaraway(ActorManager player)
    {

    }
    /// <summary>
    /// ��ȡ�������R
    /// </summary>
    /// <param name="actor"></param>
    public virtual void Local_GetPlayerInput_R(ActorManager player)
    {

    }
    /// <summary>
    /// ���������Ұ
    /// </summary>
    public virtual void Local_InPlayerView(ActorManager actor)
    {

    }
    /// <summary>
    /// �뿪�����Ұ
    /// </summary>
    public virtual void Local_OutPlayerView(ActorManager actor)
    {

    }
    /// <summary>
    /// ��ȡ�������
    /// </summary>
    public virtual void Local_GetPlayerInput(KeyCode keyCode, ActorManager actor)
    {

    }
    #endregion
}
/// <summary>
/// ��ɫ״̬
/// </summary>
public enum ActorState
{
    Default,
    Dead
}
/// <summary>
/// ��ɫȨ��
/// </summary>
public struct ActorAuthority
{
    /// <summary>
    /// �Ƿ������
    /// </summary>
    public bool isPlayer;
    /// <summary>
    /// �Ƿ�������
    /// </summary>
    public bool isState;
    /// <summary>
    /// �Ƿ��Ǳ���
    /// </summary>
    public bool isLocal;
}
/// <summary>
/// �˺�����
/// </summary>
public enum DamageState
{
    /// <summary>
    /// ħ���˺�
    /// </summary>
    MagicDamage,
    /// <summary>
    /// �������˺�
    /// </summary>
    AttackPiercingDamage,
    /// <summary>
    /// ����ۻ��˺�
    /// </summary>
    AttackBludgeoningDamage,
    /// <summary>
    /// ���������˺�
    /// </summary>
    AttackSlashingDamage,
    /// <summary>
    /// �����ж�˺�
    /// </summary>
    AttackStructureDamage,
    /// <summary>
    /// �����ո��˺�
    /// </summary>
    AttackReapDamage,
    /// <summary>
    /// ��ʵ�˺�
    /// </summary>
    RealDamage,
}