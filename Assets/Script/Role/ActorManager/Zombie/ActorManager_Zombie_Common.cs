using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class ActorManager_Zombie_Common : ActorManager
{
    [Header("僵尸配置")]
    public ActorConfig_ZombieCommon config;
    [Header("攻击间隔"), Range(1, 10)]
    public float float_AttackCD;
    private float float_AttackTimer;
    private float float_FollowCD = 1f;
    private float float_FollowTimer;
    private float float_ThinkCD = 1f;
    private float float_ThinkTimer;
    private float float_SearchTime = 4f;

    #region//初始化
    public override void AllClient_Init()
    {
        statusManager.statusType = config.status_Type;
        base.AllClient_Init();
    }
    public override void State_Init()
    {
        float_ThinkCD += new System.Random().Next(0, 100) * 0.01f;
        float_ThinkTimer = float_ThinkCD;
        float_FollowCD += new System.Random().Next(0, 100) * 0.01f;
        float_FollowTimer = float_FollowCD;
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
        actorNetManager.Local_SetLootItems(itemDatas);
    }
    public override void AllClient_AddListener()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_All_UpdateHour>().Subscribe(_ =>
        {
            AllClient_Listen_UpdateTime(_.hour, _.day, _.now);
        }).AddTo(this);
        base.AllClient_AddListener();
    }
    #endregion
    #region//时间周期
    public override void State_FixedUpdateNetwork(float dt)
    {
        if (brainManager.allClient_actorManager_AttackTarget)
        {
            float_AttackTimer -= dt;
            float_FollowTimer -= dt;
            if (float_AttackTimer < 0)
            {
                if (State_CheckingBiteDistance())
                {
                    pathManager.State_StandDown(3);
                    float_AttackTimer = float_AttackCD;
                    actorNetManager.RPC_State_NpcUseSkill((int)Skill.Bite, brainManager.allClient_actorManager_AttackTarget.pathManager.vector3Int_CurPos, brainManager.allClient_actorManager_AttackTarget.actorNetManager.Object.Id);
                }
            }
            if (float_FollowTimer < 0)
            {
                float_FollowTimer = float_FollowCD;
                if (!State_CheckingBiteDistance())
                {
                    pathManager.State_MovePostion(brainManager.allClient_actorManager_AttackTarget.pathManager.vector3Int_CurPos);
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
    public override void State_CustomUpdate()
    {
        State_CheckNearby();
        base.State_CustomUpdate();
    }
    #endregion
    #region//对外界的反应
    public override void State_Listen_MyselfHpChange(int parameter, HpChangeReason reason, NetworkId id)
    {
        NetworkObject networkObject = actorNetManager.Runner.FindObject(id);
        if (networkObject != null)
        {
            ActorManager who = networkObject.GetComponent<ActorManager>();
            if (who.actorAuthority.isPlayer && actionManager.LookAt(who, config.float_ViewDistance))
            {
                if (brainManager.allClient_actorManager_AttackTarget == null)
                {
                    State_InAttack(who);
                }
                else
                {
                    if (!brainManager.allClient_actorManager_AttackTarget.actorAuthority.isPlayer)
                    {
                        State_InAttack(who);
                    }
                }
            }
        }
        base.State_Listen_MyselfHpChange(parameter, reason, id);
    }
    public override void AllClient_Listen_UpdateTime(int hour, int date, GlobalTime globalTime)
    {
        brainManager.SetTime(globalTime);
        base.AllClient_Listen_UpdateTime(hour, date, globalTime);
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
                brainManager.SetSearch(brainManager.allClient_actorManager_AttackTarget.pathManager.vector3Int_CurPos);
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
        if (brainManager.globalTime_Now == GlobalTime.Evening)
        {
            State_Think_Stroll(config.short_MoveStep);
        }
        else
        {
            if (brainManager.homePostion.isValue)
            {
                State_Think_GoHome();
            }
            else
            {
                State_Think_Stroll(config.short_MoveStep);
            }
        }
    }
    /// <summary>
    /// 闲逛
    /// </summary>
    private void State_Think_Stroll(int distance, int tryTime = 0)
    {
        if (distance < 1 || tryTime > 3)
        {
            /*闲逛失败*/
        }
        else
        {
            Vector3Int random = new Vector3Int(new System.Random().Next(-distance, distance + 1), new System.Random().Next(-distance, distance + 1));
            Vector3Int offset = Vector3Int.zero;
            if (brainManager.activityPostion.isValue)
            {
                if (brainManager.activityPostion.postion.x - pathManager.vector3Int_CurPos.x > 5)
                {
                    offset += Vector3Int.right;
                }
                if (brainManager.activityPostion.postion.x - pathManager.vector3Int_CurPos.x < -5)
                {
                    offset += Vector3Int.left;
                }
                if (brainManager.activityPostion.postion.y - pathManager.vector3Int_CurPos.y > 5)
                {
                    offset += Vector3Int.up;
                }
                if (brainManager.activityPostion.postion.y - pathManager.vector3Int_CurPos.y < -5)
                {
                    offset += Vector3Int.down;
                }
            }
            if (!pathManager.State_MovePostion(pathManager.vector3Int_CurPos + random + offset))
            {
                /*无法抵达*/
                State_Think_Stroll(distance - 1, tryTime + 1);
            }
        }
    }
    /// <summary>
    /// 回家
    /// </summary>
    public void State_Think_GoHome()
    {
        if (Vector3.Distance(pathManager.vector3Int_CurPos, brainManager.homePostion.postion) < 0.5f)
        {
            actionManager.Despawn();
        }
        else
        {
            pathManager.State_MovePostion(brainManager.homePostion.postion);
        }
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
    public bool State_CheckingBiteDistance()
    {
        if (Vector3.Distance(brainManager.allClient_actorManager_AttackTarget.transform.position, transform.position) < config.float_BiteDistance)
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
        if (brainManager.searchPostion.isValue)
        {
            float_ThinkTimer = float_SearchTime;
            pathManager.State_MovePostion(brainManager.searchPostion.postion);
            brainManager.ResetSearch();
        }
    }

    #endregion
    #region//技能
    private enum Skill
    {
        Bite
    }
    public override void AllClient_Listen_NpcAction(int id, Vector3Int vector3, NetworkId networkId)
    {
        if (id == (int)Skill.Bite)
        {
            AllClient_Bite(vector3, networkId);
        }
        base.AllClient_Listen_NpcAction(id, vector3, networkId);
    }
    private void AllClient_Bite(Vector3Int vector3, NetworkId networkId)
    {
        actionManager.TurnTo(((Vector3)(vector3 - pathManager.vector3Int_CurPos)));
        bodyController.SetAnimatorTrigger(BodyPart.Body, "Bite");
        if (actorAuthority.isState)
        {
            bodyController.SetAnimatorFunc(BodyPart.Body, (str) =>
            {
                if (str.Equals("Bite"))
                {
                    RaycastHit2D[] raycastHit2Ds = Physics2D.CircleCastAll(transform.position, 1, Vector2.zero);
                    foreach (RaycastHit2D hit2D in raycastHit2Ds)
                    {
                        if (hit2D.collider.isTrigger && hit2D.collider.gameObject.TryGetComponent(out ActorManager actorManager))
                        {
                            if (actorManager.statusManager.statusType != StatusType.Monster_Common)
                            {
                                actorManager.AllClient_Listen_TakeDamage(config.int_BiteDamage, DamageState.AttackSlashingDamage, actorNetManager);
                            }
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
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
    public float float_BiteDistance;
    [Header("视野距离"), Range(1, 99)]
    public float float_ViewDistance;
    [Header("啃咬伤害")]
    public int int_BiteDamage;
    [Header("基本掉落列表")]
    public List<BaseLootInfo> lootInfos_Base;
    [Header("额外掉落列表")]
    public List<ExtraLootInfo> lootInfos_Extra;
}
