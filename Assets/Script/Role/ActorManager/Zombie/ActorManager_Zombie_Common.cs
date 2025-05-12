using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class ActorManager_Zombie_Common : ActorManager
{
    [SerializeField, Header("攻击间隔"), Range(1, 10)]
    public float float_AttackCD;
    private float float_AttackTimer;
    [SerializeField, Header("追击间隔"), Range(1, 10)]
    public float float_FollowCD;
    private float float_FollowTimer;
    [SerializeField, Header("思考间隔"), Range(1, 10)]
    public float float_ThinkCD;
    private float float_ThinkTimer;
    [SerializeField, Header("搜查时长"), Range(1, 10)]
    private float float_SearchTime;

    [Header("僵尸配置")]
    public ActorConfig_ZombieCommon config;
    /// <summary>
    /// 出生点
    /// </summary>
    private Vector3Int vector3_HomePos;
    /// <summary>
    /// 搜查点
    /// </summary>
    private Vector3Int vector3_SearchPos;

    private int int_ThinkTimer;
    private GlobalTime globalTime_Cur;
    public override void AllClient_AddListener()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_All_UpdateHour>().Subscribe(_ =>
        {
            AllClient_Listen_UpdateTime(_.hour, _.day, _.now);
        }).AddTo(this);
        base.AllClient_AddListener();
    }
    public override void State_FixedUpdateNetwork(float dt)
    {
        if (brainManager.allClient_actorManager_AttackTarget)
        {
            float_AttackTimer -= dt;
            float_FollowTimer -= dt;
            if (float_AttackTimer < 0)
            {
                if (State_CheckingAttackingDistance())
                {
                    pathManager.State_StandDown(3);
                    float_AttackTimer = float_AttackCD;
                    actorNetManager.RPC_State_NpcUseSkill(0, brainManager.allClient_actorManager_AttackTarget.pathManager.vector3Int_CurPos, brainManager.allClient_actorManager_AttackTarget.actorNetManager.Object.Id);
                }
            }
            if (float_FollowTimer < 0)
            {
                float_FollowTimer = float_FollowCD;
                if (!State_CheckingAttackingDistance())
                {
                    pathManager.State_MoveTo(brainManager.allClient_actorManager_AttackTarget.pathManager.vector3Int_CurPos);
                }
            }

        }
        else
        {
            float_ThinkTimer -= dt;
            if (float_ThinkTimer < 0)
            {
                float_ThinkTimer = float_ThinkCD;
                State_Think();
            }
        }

        base.State_FixedUpdateNetwork(dt);
    }
    public override void AllClient_Listen_UpdateTime(int hour, int date, GlobalTime globalTime)
    {
        globalTime_Cur = globalTime;
        base.AllClient_Listen_UpdateTime(hour, date, globalTime);
    }
    #region//初始化
    public override void Client_Init()
    {
        statusManager.statusType = config.status_Type;
        base.Client_Init();
    }
    public override void State_Init()
    {
        State_InitNPCData();
        base.State_Init();
    }
    /// <summary>
    /// 初始化数据
    /// </summary>
    private void State_InitNPCData()
    {
        List<ItemData> itemDatas = new List<ItemData>();
        for (int i = 0; i < config.lootInfos_Base.Count; i++)
        {
            int count = new System.Random().Next(config.lootInfos_Base[i].CountMin, config.lootInfos_Base[i].CountMax + 1);
            ItemData item = itemManager.CreateItemData(config.lootInfos_Base[i].ID, (short)count);
            itemDatas.Add(item);
        }
        for (int i = 0; i < config.lootInfos_Extra.Count; i++)
        {
            int random = new System.Random().Next(0, 1000);
            if (random <= config.lootInfos_Extra[i].Weight)
            {
                ItemData item = itemManager.CreateItemData(config.lootInfos_Extra[i].ID, (short)config.lootInfos_Extra[i].Count);
                itemDatas.Add(item);
            }
        }
        State_SetAbilityData(config.short_Hp, config.short_Armor, config.short_Resistance, config.short_MoveSpeed);
        actorNetManager.Local_SetBagItem(itemDatas);
    }
    /// <summary>
    /// 绑定出生点
    /// </summary>
    /// <param name="pos"></param>
    public void State_BindHome(Vector3Int pos)
    {
        vector3_HomePos = pos;
    }
    #endregion
    #region//对外界的反应
    public override void State_Listen_MyselfHpChange(int parameter, NetworkId id)
    {
        NetworkObject networkObject = actorNetManager.Runner.FindObject(id);
        if (networkObject != null)
        {
            ActorManager who = networkObject.GetComponent<ActorManager>();
            if (actionManager.LookAt(who, config.float_ViewDistance))
            {
                if (!brainManager.allClient_actorManager_AttackTarget) { State_InAttack(who); }
            }
        }
        base.State_Listen_MyselfHpChange(parameter, id);
    }
    public override void State_Listen_RoleInView(ActorManager actor)
    {
        if (actor.statusManager.statusType != StatusType.Monster_Common)
        {
            brainManager.actorManagers_Nearby.Add(actor);
        }
        base.State_Listen_RoleInView(actor);
    }
    public override void State_Listen_RoleOutView(ActorManager actor)
    {
        if (actor.statusManager.statusType != StatusType.Monster_Common)
        {
            brainManager.actorManagers_Nearby.Remove(actor);
        }
        base.State_Listen_RoleInView(actor);
    }
    public override void State_CustomUpdate()
    {
        State_CheckNearby();
        base.State_CustomUpdate();
    }
    /// <summary>
    /// 检查附近
    /// </summary>
    public void State_CheckNearby()
    {
        if (brainManager.allClient_actorManager_AttackTarget != null)
        {
            if (!actionManager.LookAt(brainManager.allClient_actorManager_AttackTarget, config.float_ViewDistance))
            {
                State_Search();
                State_OutAttack();
            }
            else
            {
                vector3_SearchPos = brainManager.allClient_actorManager_AttackTarget.pathManager.vector3Int_CurPos;
            }
        }
        else
        {
            if (brainManager.allClient_actorManager_AttackTargetID != new Fusion.NetworkId())
            {
                State_Search();
                State_OutAttack();
            }
            State_CheckNearbyActor();
        }
    }
    /// <summary>
    /// 检查附近角色
    /// </summary>
    /// <returns>终止思考</returns>
    private bool State_CheckNearbyActor()
    {
        float distance_Min = float.MaxValue;
        float distance_New = 0;
        ActorManager actorManager_Attack = null;
        brainManager.actorManagers_Nearby.RemoveAll((x) => { return x == null; });
        for (int i = 0; i < brainManager.actorManagers_Nearby.Count; i++)
        {
            if (actionManager.LookAt(brainManager.actorManagers_Nearby[i], config.float_ViewDistance))
            {
                distance_New = Vector3.Distance(transform.position, (brainManager.actorManagers_Nearby[i].transform.position));
                if (distance_Min > distance_New)
                {
                    distance_Min = distance_New;
                    actorManager_Attack = brainManager.actorManagers_Nearby[i];
                }
            }
        }
        if (actorManager_Attack != null)
        {
            State_InAttack(actorManager_Attack);
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion
    #region//闲置状态
    /// <summary>
    /// 思考
    /// </summary>
    public void State_Think()
    {
        if (globalTime_Cur != GlobalTime.Evening)
        {
            if (brainManager.actorManagers_ThreatenedTarget.Count == 0)
            {
                State_Think_Stroll();
            }
        }
        else
        {
            if (brainManager.actorManagers_ThreatenedTarget.Count == 0)
            {
                State_Think_GoHome();
            }
        }
    }
    /// <summary>
    /// 闲逛
    /// </summary>
    public void State_Think_Stroll()
    {
        if (vector3_HomePos.x - pathManager.vector3Int_CurPos.x > 10)
        {
            State_MoveSameStep(Vector3Int.right);
        }
        else if (vector3_HomePos.x - pathManager.vector3Int_CurPos.x < -10)
        {
            State_MoveSameStep(Vector3Int.left);
        }
        else if (vector3_HomePos.y - pathManager.vector3Int_CurPos.y > 10)
        {
            State_MoveSameStep(Vector3Int.up);
        }
        else if (vector3_HomePos.y - pathManager.vector3Int_CurPos.y < -10)
        {
            State_MoveSameStep(Vector3Int.down);
        }
        else
        {
            int random = new System.Random().Next(1, 4);
            if (random == 1) State_MoveSameStep(Vector3Int.down, config.short_MoveStep);
            if (random == 2) State_MoveSameStep(Vector3Int.up, config.short_MoveStep);
            if (random == 3) State_MoveSameStep(Vector3Int.right, config.short_MoveStep);
            if (random == 4) State_MoveSameStep(Vector3Int.left, config.short_MoveStep);
        }
    }
    /// <summary>
    /// 回家
    /// </summary>
    public void State_Think_GoHome()
    {
        if (Vector3.Distance(pathManager.vector3Int_CurPos, vector3_HomePos) < 0.5f)
        {
            actionManager.Despawn();
        }
        else
        {
            pathManager.State_MoveTo(vector3_HomePos);
        }
    }
    /// <summary>
    /// 朝目标方向移动
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    public void State_MoveSameStep(Vector3Int dir, short step = 1)
    {
        Vector3Int pos_F = pathManager.vector3Int_CurPos + dir * step;
        Vector3Int pos_R = pathManager.vector3Int_CurPos - dir * step;
        if (!pathManager.State_MoveTo(pos_F)) pathManager.State_MoveTo(pos_R);
    }
    #endregion
    #region//战斗状态
    /// <summary>
    /// 进入攻击状态
    /// </summary>
    /// <param name="actor"></param>
    private void State_InAttack(ActorManager actor)
    {
        actorNetManager.RPC_State_NpcChangeAttackTarget(actor.actorNetManager.Object.Id);
    }
    /// <summary>
    /// 离开攻击状态
    /// </summary>
    private void State_OutAttack()
    {
        actorNetManager.RPC_State_NpcChangeAttackTarget(new NetworkId());
    }
    /// <summary>
    /// 检查攻击距离
    /// </summary>
    /// <returns></returns>
    public bool State_CheckingAttackingDistance()
    {
        if (Vector3.Distance(brainManager.allClient_actorManager_AttackTarget.transform.position, transform.position) < config.float_AttackDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// 搜寻
    /// </summary>
    /// <param name="vector2"></param>
    private void State_Search()
    {
        if (vector3_SearchPos != Vector3Int.zero)
        {
            float_ThinkTimer = float_SearchTime;
            pathManager.State_MoveTo(vector3_SearchPos);
            vector3_SearchPos = Vector3Int.zero;
        }
    }

    #endregion
    #region//技能
    public override void AllClient_Listen_NpcAction(int id, Vector3Int vector3, NetworkId networkId)
    {
        if (id == 0)
        {
            AllClient_Bite(vector3, networkId);
        }
        base.AllClient_Listen_NpcAction(id, vector3, networkId);
    }
    private void AllClient_Bite(Vector3Int vector3, NetworkId networkId)
    {
        if (vector3.x > pathManager.vector3Int_CurPos.x)
        {
            bodyController.TurnRight();
        }
        else
        {
            bodyController.TurnLeft();
        }
        bodyController.SetAnimatorTrigger(BodyPart.Body, "Attack");
        if (actorAuthority.isState)
        {
            bodyController.SetAnimatorAction(BodyPart.Body, (str) =>
            {
                if (str.Equals("Attack"))
                {
                    RaycastHit2D[] raycastHit2Ds = Physics2D.CircleCastAll(transform.position, 1, Vector2.zero);
                    foreach (RaycastHit2D hit2D in raycastHit2Ds)
                    {
                        if (hit2D.collider.isTrigger && hit2D.collider.gameObject.TryGetComponent(out ActorManager actorManager))
                        {
                            if (actorManager.statusManager.statusType != StatusType.Monster_Common)
                            {
                                actorManager.AllClient_Listen_TakeAttackDamage(5, actorNetManager);
                            }
                        }
                    }
                }
            });
        }
    }
    #endregion
}
[Serializable]
public struct ActorConfig_ZombieCommon
{
    [Header("初始身份")]
    public StatusType status_Type;
    [Header("生命")]
    public short short_Hp;
    [Header("护甲")]
    public short short_Armor;
    [Header("魔抗")]
    public short short_Resistance;
    [Header("移动速度")]
    public short short_MoveSpeed;
    [Header("移动距离"), Range(1, 10)]
    public short short_MoveStep;
    [Header("攻击距离"), Range(1, 5)]
    public float float_AttackDistance;
    [Header("视野距离"), Range(1, 99)]
    public float float_ViewDistance;
    [Header("基本掉落列表")]
    public List<BaseLootInfo> lootInfos_Base;
    [Header("额外掉落列表")]
    public List<ExtraLootInfo> lootInfos_Extra;
}
