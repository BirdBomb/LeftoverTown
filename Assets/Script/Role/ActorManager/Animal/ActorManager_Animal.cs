using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class ActorManager_Animal : ActorManager
{
    [Header("动物配置")]
    public ActorConfig_Animal config;
    [HideInInspector]
    public float float_StateThinkTimer;
    [Header("思考间隔")]
    public float float_StateThinkCD = 1;
    [HideInInspector]
    public float float_StateAttackTimer;
    [Header("攻击间隔")]
    public float float_StateAttackCD = 1;

    #region//初始化
    public override void State_Init()
    {
        actorNetManager.Object.AssignInputAuthority(actorNetManager.Object.StateAuthority);
        float_StateThinkCD += new System.Random().Next(0, 100) * 0.01f;
        State_RsetThinkTime(float_StateThinkCD);
        float_StateAttackCD += new System.Random().Next(0, 100) * 0.01f;
        State_RsetAttackTime(float_StateAttackCD);
        State_InitNPCData();
        base.State_Init();
    }
    public override void AllClient_Init()
    {
        AllClient_InitNPCData();
        base.AllClient_Init();
    }
    /// <summary>
    /// 初始化NPC数据(客户端)
    /// </summary>
    public virtual void AllClient_InitNPCData()
    {
        statusManager.statusType = config.status_Type;
    }
    /// <summary>
    /// 初始化NPC数据(服务器)
    /// </summary>
    public virtual void State_InitNPCData()
    {
        State_InitAbilityData(config.short_Hp, config.short_Armor, config.short_Resistance, config.short_Speed);
        State_ResetDrop();
    }
    public override void AllClient_AddListener()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_AllClient_SomeoneSendEmoji>().Subscribe(_ =>
        {
            AllClient_Listen_RoleSendEmoji(_.actor, _.emoji, _.distance);
            if (actorAuthority.isState) State_Listen_RoleSendEmoji(_.actor, _.emoji, _.distance);

        }).AddTo(this);
        MessageBroker.Default.Receive<GameEvent.GameEvent_All_UpdateHour>().Subscribe(_ =>
        {
            AllClient_Listen_UpdateTime(_.hour, _.day, _.now);
        }).AddTo(this);
        base.AllClient_AddListener();
    }
    /// <summary>
    /// 刷新掉落
    /// </summary>
    public virtual void State_ResetDrop()
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
            int temp = new System.Random().Next(0, 1000);
            if (temp > config.lootInfos_Extra[i].Weight)
            {
                ItemData item = itemManager.CreateItemData(config.lootInfos_Extra[i].ID, config.lootInfos_Extra[i].Count);
                itemDatas.Add(item);
            }
        }
        actorNetManager.Local_SetLootItems(itemDatas);
    }
    #endregion
    #region//生命周期
    public override void State_FixedUpdateNetwork(float dt)
    {
        if (!brainManager.allClient_actorManager_AttackTarget)
        {
            if (!brainManager.allClient_actorManager_ThreatenedTarget)
            {
                /*当前没有攻击目标和威胁目标*/
                float_StateThinkTimer -= dt;
                if (float_StateThinkTimer < 0)
                {
                    State_RsetThinkTime(float_StateThinkCD);
                    State_ThinkLoop();
                }
            }
            else
            {
                /*当前有威胁目标*/
            }
        }
        else
        {
            /*当前有攻击目标*/
            float_StateAttackTimer -= dt;
            if (float_StateAttackTimer < 0)
            {
                State_AttackLoop();
                State_RsetAttackTime(float_StateAttackCD);
            }
        }
        base.State_FixedUpdateNetwork(dt);
    }
    public override void State_CustomUpdate()
    {
        if (brainManager.allClient_actorManager_AttackTarget)
        {
            if (brainManager.allClient_AttackState)
            {
                brainManager.allClient_AttackState = !State_Attack();
            }
        }
        State_CheckNearby();
        base.State_CustomUpdate();
    }
    public override void Local_SecondUpdate()
    {
        base.Local_SecondUpdate();
    }
    public override void Local_CustomUpdate()
    {
        //actionManager.FaceTo(bodyController.turnDir);
        base.Local_CustomUpdate();
    }

    #endregion
    #region//监听
    public override void AllClient_Listen_UpdateTime(int hour, int date, GlobalTime globalTime)
    {
        brainManager.SetTime(date, hour, globalTime);
        base.AllClient_Listen_UpdateTime(hour, date, globalTime);
    }
    public override void State_Listen_UpdateTime(int hour, int date, GlobalTime globalTime)
    {
        State_ThinkByTimeChange(hour, date, globalTime);
        base.State_Listen_UpdateTime(hour, date, globalTime);
    }
    public override void State_Listen_MyselfHpChange(int parameter, HpChangeReason reason, NetworkId id)
    {
        NetworkObject networkObject = actorNetManager.Runner.FindObject(id);
        if (networkObject != null && parameter < 0)
        {
            ActorManager who = networkObject.GetComponent<ActorManager>();
            if (who.actorAuthority.isPlayer)
            {
                State_InThreatened(who);
            }
        }
        base.State_Listen_MyselfHpChange(parameter, reason, id);
    }
    #endregion
    #region//检查
    /// <summary>
    /// 检查附近
    /// </summary>
    public virtual void State_CheckNearby()
    {
        if (brainManager.allClient_actorManager_AttackTarget != null)
        {
            //存在攻击目标
            if (actionManager.LookAt(brainManager.allClient_actorManager_AttackTarget, config.float_ViewDistance))
            {
                brainManager.State_SetSearchPos(brainManager.allClient_actorManager_AttackTarget.pathManager.vector3Int_CurPos);
            }
            else
            {
                State_Search(5);
                State_OutAttack();
            }
            return;
        }
        else
        {
            if (brainManager.allClient_actorManager_AttackTargetID != new Fusion.NetworkId())
            {
                State_Search(5);
                State_OutAttack();
            }
            State_CheckNearbyActor();
        }
        if (brainManager.allClient_actorManager_ThreatenedTarget != null)
        {
            //存在威胁目标
            if (actionManager.LookAt(brainManager.allClient_actorManager_ThreatenedTarget, State_CalculateView()))
            {
                State_StartRunAway(brainManager.allClient_actorManager_ThreatenedTarget.transform);
            }
            else
            {
                State_OutThreatened();
            }
        }
        else
        {
            if (brainManager.allClient_actorManager_ThreatenedTargetID != new Fusion.NetworkId())
            {
                State_OutThreatened();
            }
            State_CheckNearbyActor();
        }
    }
    /// <summary>
    /// 检查附近角色
    /// </summary>
    /// <returns>终止思考</returns>
    public virtual bool State_CheckNearbyActor()
    {
        for (int i = 0; i < brainManager.actorManagers_Nearby.Count; i++)
        {
            if (actionManager.LookAt(brainManager.actorManagers_Nearby[i], State_CalculateView()))
            {
                if (brainManager.actorManagers_Nearby[i].statusManager.statusType == StatusType.Monster_Common)
                {
                    State_InThreatened(brainManager.actorManagers_Nearby[i]);
                    return true;
                }
            }
        }
        return false;
    }
    /// <summary>
    /// 获得检查视野
    /// </summary>
    /// <returns></returns>
    public virtual float State_CalculateView()
    {
        return config.float_ViewDistance;
    }
    #endregion
    #region//思考
    /// <summary>
    /// 思考循环
    /// </summary>
    public void State_ThinkLoop()
    {
        State_ThinkByTimeUpdate(brainManager.day_Now, brainManager.hour_Now, brainManager.globalTime_Now);
    }
    /// <summary>
    /// 重设思考间隔
    /// </summary>
    /// <param name="time"></param>
    public void State_RsetThinkTime(float time)
    {
        float_StateThinkTimer = time;
    }

    /// <summary>
    /// 根据时间决定动作(经常触发)
    /// </summary>
    public virtual void State_ThinkByTimeUpdate(int date, int hour, GlobalTime time)
    {
        State_Think_GoToStroll_Long(10, 5);
    }
    /// <summary>
    /// 根据时间变化决定动作(关键时间触发)
    /// </summary>
    public virtual void State_ThinkByTimeChange(int date, int hour, GlobalTime globalTime)
    {

    }
    #endregion
    #region//攻击逻辑
    /// <summary>
    /// 进入攻击状态
    /// </summary>
    /// <param name="actor"></param>
    public virtual void State_InAttack(ActorManager actor)
    {
        pathManager.State_SetFrezzeTime(0);
        pathManager.State_ClearPath();
        actorNetManager.RPC_State_NpcChangeAttackTarget(actor.actorNetManager.Object.Id);
    }
    /// <summary>
    /// 离开攻击状态
    /// </summary>
    public virtual void State_OutAttack()
    {
        actorNetManager.RPC_State_NpcChangeAttackTarget(new NetworkId());
    }
    /// <summary>
    /// 攻击循环
    /// </summary>
    public virtual void State_AttackLoop()
    {
        if (brainManager.allClient_actorManager_AttackTarget != null)
        {
            if (brainManager.allClient_actorManager_AttackTarget.actorState != ActorState.Dead)
            {
                actorNetManager.RPC_State_NpcChangeAttackState(true);
            }
            else
            {
                actorNetManager.RPC_State_NpcChangeAttackTarget(new NetworkId());
            }
        }
    }
    /// <summary>
    /// 攻击
    /// </summary>
    /// <returns>攻击成功</returns>
    public virtual bool State_Attack()
    {
        return true;
    }
    /// <summary>
    /// 重设攻击间隔
    /// </summary>
    /// <param name="time"></param>
    public void State_RsetAttackTime(float time)
    {
        float_StateAttackTimer = time;
    }
    /// <summary>
    /// 追击
    /// </summary>
    /// <param name="vector2"></param>
    public virtual void State_Follow(Vector3Int to)
    {
        pathManager.State_MovePostion(to, null);
    }
    /// <summary>
    /// 撤退
    /// </summary>
    public virtual void State_Retreat(Vector3 targetPos)
    {
        Vector3Int dirX = Vector3Int.zero;
        Vector3Int dirY = Vector3Int.zero;
        if (targetPos.x > transform.position.x) dirX += Vector3Int.left;
        else dirX += Vector3Int.right;
        if (targetPos.y > transform.position.y) dirY += Vector3Int.down;
        else dirY += Vector3Int.up;
        //短距离对角逃窜
        if (pathManager.State_MovePostion(pathManager.vector3Int_CurPos + dirX + dirY, State_EndRunAway))
        {
            return;
        }
        //短距离水平逃窜
        if (pathManager.State_MovePostion(pathManager.vector3Int_CurPos + dirX, State_EndRunAway))
        {
            return;
        }
        //短距离垂直逃窜
        if (pathManager.State_MovePostion(pathManager.vector3Int_CurPos + dirY, State_EndRunAway))
        {
            return;
        }
    }
    /// <summary>
    /// 走位
    /// </summary>
    public virtual void State_Elude(Vector3 targetPos)
    {
        float distance_x = Math.Abs(targetPos.x - transform.position.x);
        float distance_y = Math.Abs(targetPos.y - transform.position.y);
        Vector3Int offset = Vector3Int.zero;
        int random = new System.Random().Next(0, 2);
        if (distance_x < distance_y)
        {
            //垂直方向距离远,水平走位
            offset = Vector3Int.left;
        }
        else
        {
            //水平方向距离远,垂直走位
            offset = Vector3Int.down;
        }
        if (random == 0) { offset *= -1; }
        if (!pathManager.State_MovePostion(pathManager.vector3Int_CurPos + offset, null))
        {
            offset *= -1;
            pathManager.State_MovePostion(pathManager.vector3Int_CurPos + offset, null);
        }
    }

    /// <summary>
    /// 搜寻
    /// </summary>
    public virtual void State_Search(float searchTime)
    {
        if (brainManager.state_searchPostion.isValue)
        {
            State_RsetThinkTime(searchTime);
            pathManager.State_MovePostion(brainManager.state_searchPostion.position);
            brainManager.State_ResetSearchPos();
        }
    }
    #endregion
    #region//威胁逻辑
    /// <summary>
    /// 进入威胁状态
    /// </summary>
    /// <param name="actor"></param>
    public virtual void State_InThreatened(ActorManager actor)
    {
        pathManager.State_SetFrezzeTime(0);
        pathManager.State_ClearPath();
        State_StartRunAway(actor.transform);
        actorNetManager.RPC_State_NpcChangeThreatenedTarget(actor.actorNetManager.Object.Id);
    }
    /// <summary>
    /// 离开威胁状态
    /// </summary>
    public virtual void State_OutThreatened()
    {
        actorNetManager.RPC_State_NpcChangeThreatenedTarget(new NetworkId());
    }
    /// <summary>
    /// 开始逃走
    /// </summary>
    /// <param name="from"></param>
    public void State_StartRunAway(Transform from)
    {
        if (pathManager.State_CheckRemainingPathCount() > 0)
        {
            //路径未完成
            return;
        }
        Vector3Int dirX = Vector3Int.zero;
        Vector3Int dirY = Vector3Int.zero;
        if (from.position.x > transform.position.x) dirX += Vector3Int.left;
        else dirX += Vector3Int.right;
        if (from.position.y > transform.position.y) dirY += Vector3Int.down;
        else dirY += Vector3Int.up;
        //短距离对角逃窜
        if (pathManager.State_MovePostion(pathManager.vector3Int_CurPos + dirX + dirY, State_EndRunAway))
        {
            return;
        }
        //短距离水平逃窜
        if (pathManager.State_MovePostion(pathManager.vector3Int_CurPos + dirX, State_EndRunAway))
        {
            return;
        }
        //短距离垂直逃窜
        if (pathManager.State_MovePostion(pathManager.vector3Int_CurPos + dirY, State_EndRunAway))
        {
            return;
        }
        //长距离对角逃窜
        if (pathManager.State_MoveArea(pathManager.vector3Int_CurPos + (dirX + dirY), 4, dirX.x, dirY.y, State_EndRunAway)) return;
        //长距离随机逃窜
        if (pathManager.State_MoveArea(pathManager.vector3Int_CurPos + (dirX + dirY) * 4, 8, -dirX.x, -dirY.y, State_EndRunAway)) return;
    }
    /// <summary>
    /// 逃走结束
    /// </summary>
    public void State_EndRunAway()
    {
        //继续观察四周
        State_CheckNearby();
    }
    #endregion
    #region//行为逻辑
    /// <summary>
    /// 长距离闲逛
    /// </summary>
    /// <param name="areaDistance">目标区域距离</param>
    /// <param name="areaSize">目标区域尺寸</param>
    public void State_Think_GoToStroll_Long(int areaDistance, int areaSize)
    {
        if (pathManager.State_CheckRemainingPathCount() > 0)
        {
            //之前路程未完成
            return;
        }
        else
        {
            int dir_x = new System.Random().Next(-1, 2);
            int dir_y = new System.Random().Next(-1, 2);
            Vector3Int offset = Vector3Int.zero;
            if (brainManager.state_ActivityPostion.isValue)
            {
                if (Vector3.Distance(brainManager.state_ActivityPostion.position, pathManager.vector3Int_CurPos) > 15)
                {
                    //太远了 我要回家
                    if (brainManager.state_ActivityPostion.position.x >= pathManager.vector3Int_CurPos.x) dir_x = 1;
                    else dir_x = -1;
                    if (brainManager.state_ActivityPostion.position.y >= pathManager.vector3Int_CurPos.y) dir_y = 1;
                    else dir_y = -1;
                }
                else
                {
                    //还行不是很远 微调目标区域位置
                    if (brainManager.state_ActivityPostion.position.x > pathManager.vector3Int_CurPos.x) offset += Vector3Int.right;
                    else offset += Vector3Int.left;
                    if (brainManager.state_ActivityPostion.position.y > pathManager.vector3Int_CurPos.y) offset += Vector3Int.up;
                    else offset += Vector3Int.down;
                }
            }
            Vector3Int centerPos = pathManager.vector3Int_CurPos + offset + new Vector3Int(dir_x * areaDistance, dir_y * areaDistance, 0);
            if (pathManager.State_MoveArea(centerPos, areaSize, dir_x, dir_y, State_Think_BetweenStroll))
            {
                return;
            }
            else
            {
                centerPos = pathManager.vector3Int_CurPos + new Vector3Int(dir_x * 5, dir_y * 5, 0);
                if (pathManager.State_MoveArea(centerPos, 10, -dir_x, -dir_y, State_Think_BetweenStroll))
                {
                    return;
                }
                else
                {
                    /*闲逛失败*/
                }
            }
        }
    }
    /// <summary>
    /// 闲逛间隔
    /// </summary>
    public virtual void State_Think_BetweenStroll()
    {
        float freezeTime = new System.Random().Next(10, 40) * 0.1f;
        pathManager.State_SetFrezzeTime(freezeTime);
    }
    /// <summary>
    /// 回家
    /// </summary>
    /// <returns></returns>
    public virtual bool State_Think_GoToHome()
    {
        if (brainManager.state_homePostion.isValue)
        {
            pathManager.State_MovePostion(brainManager.state_homePostion.position);
            return true;
        }
        return false;
    }
    #endregion

}
[Serializable]
public struct ActorConfig_Animal
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
    public short short_Speed;
    [Header("移动距离"), Range(1, 10)]
    public short short_MoveStep;
    [Header("视野距离"), Range(1, 99)]
    public float float_ViewDistance;
    [Header("基本掉落列表")]
    public List<BaseLootInfo> lootInfos_Base;
    [Header("额外掉落列表")]
    public List<ExtraLootInfo> lootInfos_Extra;
}
