using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class ActorManager_Monster : ActorManager
{
    protected float float_StateThinkCD = 1;
    protected float float_StateThinkTimer;
    protected float float_StateAttackCD = 1;
    protected float float_StateAttackTimer;
    private System.Random random = new System.Random();

    #region//初始化
    public override void AllClient_Init()
    {
        AllClient_InitNPCData();
        base.AllClient_Init();
    }
    public override void State_Init()
    {
        actorNetManager.Object.AssignInputAuthority(actorNetManager.Object.StateAuthority);
        float_StateThinkCD += random.Next(0, 100) * 0.01f;
        float_StateAttackCD += random.Next(0, 100) * 0.01f;
        State_ResetThinkTime(float_StateThinkCD);
        State_RsetAttackTime(float_StateAttackCD);
        State_InitNPCData();
        base.State_Init();
    }
    /// <summary>
    /// 初始化NPC数据(客户端)
    /// </summary>
    public virtual void AllClient_InitNPCData()
    {
        statusManager.statusType = actorConfig.Status;
    }
    /// <summary>
    /// 初始化NPC数据(服务器)
    /// </summary>
    public virtual void State_InitNPCData()
    {
        State_InitAbilityData(actorConfig.Hp, actorConfig.Armor, actorConfig.Resistance, actorConfig.Speed);
    }
    public override void ForAll_AddListener()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_AllClient_SomeoneSendEmoji>().Subscribe(ForAll_Listen_RoleSendEmoji).AddTo(this);
        MessageBroker.Default.Receive<GameEvent.GameEvent_All_UpdateHour>().Subscribe(ForAll_Listen_UpdateTime).AddTo(this);
        base.ForAll_AddListener();
    }
    #endregion
    #region//生命周期
    public override void State_FixedUpdateNetwork(float dt)
    {
        if (brainManager.ForAll_GetAttackTarget(out _))
        {
            /*当前有攻击目标*/
            float_StateAttackTimer -= dt;
            if (float_StateAttackTimer < 0)
            {
                State_RsetAttackTime(float_StateAttackCD);
                State_AttackLoop();
            }
        }
        else if (!brainManager.ForAll_GetThreatenedTarget(out _))
        {
            /*当前没有攻击目标和威胁目标*/
            float_StateThinkTimer -= dt;
            if (float_StateThinkTimer < 0)
            {
                State_ResetThinkTime(float_StateThinkCD);
                State_ThinkLoop();
            }
        }
        base.State_FixedUpdateNetwork(dt);
    }
    public override void ForState_CustomUpdate()
    {
        if (brainManager.ForAll_GetAttackTarget(out _) && brainManager.allClient_AttackingRunning)
        {
            brainManager.allClient_AttackingRunning = !State_Attack();
        }
        State_CheckSurroundings();
        base.ForState_CustomUpdate();
    }
    #endregion
    #region//监听
    public override void ForAll_Listen_UpdateTime(GameEvent.GameEvent_All_UpdateHour eventData)
    {
        brainManager.SetTime(eventData.day, eventData.hour, eventData.now);
        base.ForAll_Listen_UpdateTime(eventData);
    }
    public override void ForState_Listen_UpdateTime(GameEvent.GameEvent_All_UpdateHour eventData)
    {
        State_ThinkByTimeChange(eventData.hour, eventData.day, eventData.now);
        base.ForState_Listen_UpdateTime(eventData);
    }
    public override void ForState_Listen_MyselfInjured(int parameter, HpChangeReason reason, NetworkId id)
    {
        NetworkObject networkObject = actorNetManager.Runner.FindObject(id);
        if (networkObject != null && networkObject.TryGetComponent(out ActorManager who) && who.actorAuthority.isPlayer)
        {
            State_InThreatened(who);
        }
        base.ForState_Listen_MyselfInjured(parameter, reason, id);
    }
    #endregion
    #region//检查
    private const float float_SearchTime = 5;
    /// <summary>
    /// 检查附近
    /// </summary>
    public virtual void State_CheckSurroundings()
    {
        if (State_CheckSurroundings_AttackState()) return;
        if (State_CheckSurroundings_ThreatenedState()) return;
        State_CheckNearbyActor();
    }
    /// <summary>
    /// 检查附近(攻击状态检测)
    /// </summary>
    /// <returns></returns>
    public virtual bool State_CheckSurroundings_AttackState()
    {
        bool attackTargetinView;
        if (brainManager.ForAll_GetAttackTarget(out ActorManager attackTarget))
        {
            attackTargetinView = actionManager.LookAt(attackTarget, State_CalculateView());
            brainManager.ForAll_UpdateAttackTargetInView(attackTargetinView);
            if (attackTargetinView)
            {
                brainManager.ForState_SetSearchPos(attackTarget.pathManager.vector3Int_CurPos);
                brainManager.ForAll_SetAttackDesire(5);
                return true;
            }
        }
        if (brainManager.ForAll_GetAttackDesire(out float desire))
        {
            //视野里不存在攻击目标但是攻击欲望高涨
            float newDesire = desire - const_customUpdateTime;
            brainManager.ForAll_SetAttackDesire(newDesire);

            if (newDesire <= 0)
            {
                //攻击欲望消退
                State_Search(float_SearchTime);
                State_OutAttack();
            }
        }
        return false;
    }
    /// <summary>
    /// 检查附近(威胁状态检测)
    /// </summary>
    /// <returns></returns>
    public virtual bool State_CheckSurroundings_ThreatenedState()
    {
        bool threatenedTargetinView;
        if (brainManager.ForAll_GetThreatenedTarget(out ActorManager threatenedTarget))
        {
            threatenedTargetinView = actionManager.LookAt(threatenedTarget, State_CalculateView());
            brainManager.ForAll_UpdateThreatenedTargetInView(threatenedTargetinView);
            State_StartRunAway(threatenedTarget.transform);
            if (threatenedTargetinView)
            {
                brainManager.ForAll_SetThreatenedDesire(2);
                return true;
            }
        }
        if (brainManager.ForAll_GetThreatenedDesire(out float desire))
        {
            //不存在威胁目标但是心有余悸
            float newDesire = desire - const_customUpdateTime;
            brainManager.ForAll_SetThreatenedDesire(newDesire);
            if (newDesire <= 0)
            {
                //应该没事了
                State_OutThreatened();
            }
        }
        return false;
    }
    /// <summary>
    /// 检查附近角色
    /// </summary>
    /// <returns>终止思考</returns>
    public virtual bool State_CheckNearbyActor()
    {
        List<ActorManager> temp = brainManager.State_GetNearbyActors();
        foreach (ActorManager actor in temp)
        {
            if (actionManager.LookAt(actor, State_CalculateView()))
            {
                if (brainManager.ForAll_CheckLastThreatenedTarget(actor) || actor.statusManager.statusType == StatusType.Monster_Common)
                {
                    State_InThreatened(actor);
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
        return 10;
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
    public void State_ResetThinkTime(float time)
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
        pathManager.State_Continue();
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
        if (brainManager.ForAll_GetAttackTarget(out ActorManager actorManager))
        {
            if (actorManager.actorState != ActorState.Dead)
            {
                actorNetManager.RPC_State_NpcChangeAttackState(true);
            }
            else
            {
                State_OutAttack();
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
        pathManager.State_MoveShort(to, null);
    }
    /// <summary>
    /// 撤退
    /// </summary>
    public virtual void State_Retreat(Vector3 targetPos)
    {
        Vector3Int dirX = (targetPos.x > transform.position.x) ? Vector3Int.left : Vector3Int.right;
        Vector3Int dirY = (targetPos.y > transform.position.y) ? Vector3Int.down : Vector3Int.up;
        //短距离对角逃窜
        if (pathManager.State_MoveShort(pathManager.vector3Int_CurPos + dirX + dirY, State_EndRunAway)) return;
        //短距离水平逃窜
        if (pathManager.State_MoveShort(pathManager.vector3Int_CurPos + dirX, State_EndRunAway)) return;
        //短距离垂直逃窜
        if (pathManager.State_MoveShort(pathManager.vector3Int_CurPos + dirY, State_EndRunAway)) return;
    }
    /// <summary>
    /// 走位
    /// </summary>
    public virtual void State_Elude(Vector3 targetPos)
    {
        float distance_x = Math.Abs(targetPos.x - transform.position.x);
        float distance_y = Math.Abs(targetPos.y - transform.position.y);
        Vector3Int offset = (distance_x < distance_y) ? Vector3Int.left : Vector3Int.down;
        if (random.Next(0, 2) == 0) { offset *= -1; }
        if (pathManager.State_MoveShort(pathManager.vector3Int_CurPos + offset, null)) return;
        if (pathManager.State_MoveShort(pathManager.vector3Int_CurPos - offset, null)) return;
    }

    /// <summary>
    /// 搜寻
    /// </summary>
    public virtual void State_Search(float searchTime)
    {
        if (brainManager.state_searchPostion.isValue)
        {
            State_ResetThinkTime(searchTime);
            pathManager.State_MoveLong(brainManager.state_searchPostion.position, 2);
            brainManager.ForState_ResetSearchPos();
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
        pathManager.State_Continue();
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
        Vector3Int dirX = (from.position.x > transform.position.x) ? Vector3Int.left : Vector3Int.right;
        Vector3Int dirY = (from.position.y > transform.position.y) ? Vector3Int.down : Vector3Int.up;
        //短距离对角逃窜
        if (pathManager.State_MoveShort(pathManager.vector3Int_CurPos + dirX + dirY, State_EndRunAway)) return;
        //短距离水平逃窜
        if (pathManager.State_MoveShort(pathManager.vector3Int_CurPos + dirX, State_EndRunAway)) return;
        //短距离垂直逃窜
        if (pathManager.State_MoveShort(pathManager.vector3Int_CurPos + dirY, State_EndRunAway)) return;
        //长距离对角逃窜
        if (pathManager.State_MoveLong(pathManager.vector3Int_CurPos + dirX + dirY, 5, State_EndRunAway)) return;
        if (pathManager.State_MoveLong(pathManager.vector3Int_CurPos - dirX - dirY, 5, State_EndRunAway)) return;
    }
    /// <summary>
    /// 逃走结束
    /// </summary>
    public void State_EndRunAway()
    {
        //继续观察四周
        State_CheckSurroundings();
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
        if (pathManager.State_CheckRemainingPathCount() > 0) return;
        int dir_x = random.Next(-1, 2);
        int dir_y = random.Next(-1, 2);
        Vector3Int offset = Vector3Int.zero;
        if (brainManager.state_ActivityPostion.isValue)
        {
            if ((brainManager.state_ActivityPostion.position - pathManager.vector3Int_CurPos).sqrMagnitude > 225)
            {
                //太远了 我要回家
                if (pathManager.State_MoveLong(brainManager.state_ActivityPostion.position, 5, State_Think_BetweenStroll)) return;
            }
            else
            {
                //还行不是很远 微调目标区域位置
                offset = (brainManager.state_ActivityPostion.position.x > pathManager.vector3Int_CurPos.x) ? Vector3Int.right : Vector3Int.left;
                offset = (brainManager.state_ActivityPostion.position.y > pathManager.vector3Int_CurPos.y) ? Vector3Int.up : Vector3Int.down;
            }
        }
        Vector3Int centerPos = pathManager.vector3Int_CurPos + offset + new Vector3Int(dir_x * areaDistance, dir_y * areaDistance, 0);
        if (pathManager.State_MoveLong(centerPos, 5, State_Think_BetweenStroll)) return;
    }
    /// <summary>
    /// 闲逛间隔
    /// </summary>
    public virtual void State_Think_BetweenStroll()
    {
        float freezeTime = new System.Random().Next(10, 40) * 0.1f;
        pathManager.State_Stop(freezeTime);
    }
    /// <summary>
    /// 回家
    /// </summary>
    /// <returns></returns>
    public virtual bool State_Think_GoToHome()
    {
        if (brainManager.state_homePostion.isValue)
        {
            pathManager.State_MoveLong(brainManager.state_homePostion.position, 5);
            return true;
        }
        return false;
    }
    #endregion
}
