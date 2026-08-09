using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using static GameEvent;
/// <summary>
/// NPC基类脚本
/// </summary>
public class ActorManager_NPC : ActorManager
{
    protected float float_StateThinkCD = 1;
    protected float float_StateThinkTimer;
    protected float float_StateAttackCD = 1;
    protected float float_StateAttackTimer;
    private System.Random random = new System.Random();
    #region//初始化NPC
    public override void AllClient_Init()
    {
        AllClient_InitNPCData();
        ForAll_InitDialog();
        base.AllClient_Init();
    }
    public override void State_Init()
    {
        actorNetManager.Object.AssignInputAuthority(actorNetManager.Object.StateAuthority);
        float_StateThinkCD += random.Next(0, 100) * 0.01f;
        State_ResetThinkTime(float_StateThinkCD);
        State_InitNPCData();
        base.State_Init();
    }
    /// <summary>
    /// 初始化NPC数据(客户端)
    /// </summary>
    public virtual void AllClient_InitNPCData()
    {
        statusManager.statusType = actorConfig.Status;
        Local_ResetGood();
    }
    /// <summary>
    /// 初始化NPC数据(服务器)
    /// </summary>
    public virtual void State_InitNPCData()
    {
        UnityEngine.Random.InitState(new System.Random().Next(0, 10000));
        short eyeID = EyeConfigData.eyeConfigs[UnityEngine.Random.Range(0, EyeConfigData.eyeConfigs.Count)].Eye_ID;
        short hairID = HairConfigData.hairConfigs[UnityEngine.Random.Range(0, HairConfigData.hairConfigs.Count)].Hair_ID;
        Color32 hairColor = UnityEngine.Random.ColorHSV();

        ItemData itemData_Hat = new ItemData();
        ItemData itemData_Clothes = new ItemData();
        if (actorConfig.Hat_List != null)
        {
            int weightCount = 0;
            int temp = 0;
            foreach (LootRandomInfo info in actorConfig.Hat_List)
            {
                weightCount += info.Weight;
            }
            for (int i = 0; i < actorConfig.Hat_List.Length; i++)
            {
                temp += actorConfig.Hat_List[i].Weight;
                if (random.Next(0, weightCount) < temp)
                {
                    itemData_Hat = itemManager.CreateItemData(actorConfig.Hat_List[i].ID);
                    break;
                }
            }
        }
        if (actorConfig.Clothes_List != null)
        {
            int weightCount = 0;
            int temp = 0;
            foreach (LootRandomInfo info in actorConfig.Clothes_List)
            {
                weightCount += info.Weight;
            }
            for (int i = 0; i < actorConfig.Clothes_List.Length; i++)
            {
                temp += actorConfig.Clothes_List[i].Weight;
                if (random.Next(0, weightCount) < temp)
                {
                    itemData_Clothes = itemManager.CreateItemData(actorConfig.Clothes_List[i].ID);
                    break;
                }
            }
        }
        State_InitHeadAndBody(itemData_Hat, itemData_Clothes);
        State_InitFace("", eyeID, hairID, hairColor);
        State_InitAbilityData(actorConfig.Hp, actorConfig.Armor, actorConfig.Resistance, actorConfig.Speed);
        State_ResetBag();
    }
    public override void ForAll_AddListener()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_AllClient_SomeoneSendEmoji>().Subscribe(ForAll_Listen_RoleSendEmoji).AddTo(this);
        MessageBroker.Default.Receive<GameEvent.GameEvent_AllClient_SomeoneCommit>().Subscribe(ForAll_Listen_RoleCommit).AddTo(this);
        MessageBroker.Default.Receive<GameEvent.GameEvent_All_UpdateHour>().Subscribe(ForAll_Listen_UpdateTime).AddTo(this);
        base.ForAll_AddListener();
    }
    #endregion
    #region//生命周期
    public override void FixedUpdate()
    {
        AllClient_UpdateAttack(Time.fixedDeltaTime);
        base.FixedUpdate();
    }
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
    public override void State_SecondUpdate()
    {
        base.State_SecondUpdate();
    }
    public override void ForState_CustomUpdate()
    {
        State_CheckSurroundings();
        base.ForState_CustomUpdate();
    }
    #endregion
    #region//监听
    public override void ForAll_Listen_UpdateTime(GameEvent.GameEvent_All_UpdateHour eventData)
    {
        if(eventData.hour == 0) Local_ResetGood();
        brainManager.SetTime(eventData.day,eventData.hour,eventData.now);
        base.ForAll_Listen_UpdateTime(eventData);
    }
    public override void ForState_Listen_UpdateTime(GameEvent.GameEvent_All_UpdateHour eventData)
    {
        if (eventData.hour == 0) State_ResetBag();
        State_ThinkByTimeChange(eventData.day, eventData.hour, eventData.now);
        base.ForState_Listen_UpdateTime(eventData);
    }
    public override void ForState_Listen_MyselfInjured(int parameter, HpChangeReason reason, NetworkId id)
    {
        NetworkObject networkObject = actorNetManager.Runner.FindObject(id);
        if (networkObject != null && networkObject.TryGetComponent(out ActorManager who) && who.actorAuthority.isPlayer)
        {
            who.actionManager.AllClient_Commit(CommitState.Attacking, 10);
            State_InThreatened(who);
        }
        else
        {
            State_TryToSendEmoji(0, Emoji.Panic, 1f, false);
        }
        base.ForState_Listen_MyselfInjured(parameter, reason, id);
    }
    public override void ForState_Listen_MyselfDead(int parameter, HpChangeReason reason, NetworkId id)
    {
        NetworkObject networkObject = actorNetManager.Runner.FindObject(id);
        if (networkObject != null && parameter < 0)
        {
            ActorManager who = networkObject.GetComponent<ActorManager>();
            if (who.actorAuthority.isPlayer)
            {
                who.actionManager.AllClient_Commit(CommitState.Murder, 500);
            }
        }
        base.ForState_Listen_MyselfDead(parameter, reason, id);
    }
    public override void ForState_Listen_RoleSendEmoji(GameEvent.GameEvent_AllClient_SomeoneSendEmoji eventData)
    {
        if (brainManager.ForAll_GetAttackTarget(out _) || brainManager.ForAll_GetThreatenedTarget(out _))
        {
            return;
        }
        if (actionManager.HearTo(eventData.actor, eventData.distance))
        {
            State_Think_Hear(eventData);
        }
        base.ForState_Listen_RoleSendEmoji(eventData);
    }
    public override void ForState_Listen_RoleCommit(GameEvent.GameEvent_AllClient_SomeoneCommit eventData)
    {
        if (actionManager.LookAt(eventData.actor, State_CalculateView()))
        {
            State_TryToSendEmoji(0, Emoji.Yell, 1, false);
        }
        base.ForState_Listen_RoleCommit(eventData);
    }
    public override void ForAll_Listen_ChangeAttackTarget(NetworkId id)
    {
        if (id != new NetworkId())
        {
            actorUI.HideAllSingal();
        }
        base.ForAll_Listen_ChangeAttackTarget(id);
    }
    public override void ForAll_Listen_ChangeThreatenedTarget(NetworkId id)
    {
        if (id != new NetworkId())
        {
            actorUI.HideAllSingal();
        }
        base.ForAll_Listen_ChangeThreatenedTarget(id);
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
            threatenedTargetinView = actionManager.LookAt(threatenedTarget, 10);
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
        foreach(ActorManager actor in temp)
        {
            if (actionManager.LookAt(actor, State_CalculateView()))
            {
                if (brainManager.ForAll_CheckLastThreatenedTarget(actor) || actor.actorNetManager.Local_Fine > 0 || actor.statusManager.statusType == StatusType.Monster_Common)
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
    public virtual int State_CalculateView()
    {
        if (bodyController.bodyAction_Cur.bodyActionType == BodyActionType.LayToRight ||
            bodyController.bodyAction_Cur.bodyActionType == BodyActionType.LayToLeft ||
            bodyController.bodyAction_Cur.bodyActionType == BodyActionType.LayToUp)
        {
            return 1;
        }
        return 10;
    }
    #endregion
    #region//思考
    /// <summary>
    /// 思考循环
    /// </summary>
    public void State_ThinkLoop()
    {
        if (brainManager.ForAll_FocusOnNearbyItem() && State_Think_GoToPickUp()) return;
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
        State_Think_GoToStroll_Long(5, 5);
    }
    /// <summary>
    /// 根据时间变化决定动作(关键时间触发)
    /// </summary>
    public virtual void State_ThinkByTimeChange(int date, int hour, GlobalTime globalTime)
    {
        
    }
    #endregion

    #region//攻击逻辑
    public virtual void State_InAttack(ActorManager actor)
    {
        State_TryToSendEmoji(0, Emoji.Attack, 1f, true);
        pathManager.State_Continue();
        actorNetManager.RPC_State_NpcChangeAttackTarget(actor.actorNetManager.Object.Id);
        State_PutOnHand(State_ChooseWeapon);
    }
    public virtual void State_OutAttack()
    {
        actorNetManager.RPC_State_NpcChangeAttackTarget(new NetworkId());
    }
    public virtual void State_AttackLoop()
    {
        if (brainManager.ForAll_GetAttackTarget(out ActorManager actorManager))
        {
            float val = State_CheckingAttackingDistance();
            if (val > 1 || !brainManager.ForAll_CheckAttackTargetInView())
            {
                State_Follow(actorManager.pathManager.vector3Int_CurPos);
            }
            else
            {
                if (actorManager.actorState != ActorState.Dead)
                {
                    actorNetManager.RPC_State_NpcChangeAttackState(true);
                }
                else
                {
                    State_OutAttack();
                }
                if (val < 0.5f)
                {
                    State_Retreat(actorManager.transform.position); //撤退
                }
                else
                {
                    State_Elude(actorManager.transform.position);//走位
                }
            }
        }
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
    /// <param name="vector2"></param>
    public virtual void State_Search(float searchTime)
    {
        if (brainManager.state_searchPostion.isValue)
        {
            State_ResetThinkTime(searchTime);
            pathManager.State_MoveLong(brainManager.state_searchPostion.position, 2);
            brainManager.ForState_ResetSearchPos();
        }
    }
    /// <summary>
    /// 更新攻击状态
    /// </summary>
    public virtual void AllClient_UpdateAttack(float dt)
    {
        if (brainManager.ForAll_GetAttackTarget(out ActorManager attackTarget)&& brainManager.ForAll_CheckAttackTargetInView())
        {
            inputManager.Simulate_InputMousePos(attackTarget.transform.position);
            if (brainManager.allClient_AttackingRunning)
            {
                brainManager.allClient_AttackingRunning = !inputManager.Simulate_InputMousePress(dt, ActorInputManager.MouseInputType.PressRightThenPressLeft);
            }
        }
        else
        {
            inputManager.Simulate_SmoothlyResetMouseDir(bodyController.turnDir);
        }
    }
    /// <summary>
    /// 检查攻击距离
    /// </summary>
    /// <returns>实际距离/射程</returns>
    public float State_CheckingAttackingDistance()
    {
        float attackDistance = WeaponConfigData.GetWeaponConfig(itemManager.itemBase_OnHand.itemData.I).Distance;
        float realDistanceSqr = 0;
        if (brainManager.ForAll_GetAttackTarget(out ActorManager actorManager))
        {
            realDistanceSqr = (actorManager.transform.position - transform.position).sqrMagnitude;
        }
        return realDistanceSqr / (attackDistance * attackDistance);
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
        State_TryToSendEmoji(0, Emoji.Panic, 1f, true);
        State_StartRunAway(actor.transform);
        actorNetManager.RPC_State_NpcChangeThreatenedTarget(actor.actorNetManager.Object.Id);
    }
    /// <summary>
    /// 离开威胁状态
    /// </summary>
    public virtual void State_OutThreatened()
    {
        State_TryToSendEmoji(0, Emoji.Search, 1f, false);
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
        if (pathManager.State_MoveLong(pathManager.vector3Int_CurPos + dirX + dirY, 2, State_EndRunAway)) return;
        if (pathManager.State_MoveLong(pathManager.vector3Int_CurPos - dirX - dirY, 2, State_EndRunAway)) return;
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
    /// 前往拾取
    /// </summary>
    /// <returns>是否可以去拾取</returns>
    public bool State_Think_GoToPickUp()
    {
        if (brainManager.ForAll_GetTargetItem(out ItemNetObj itemNetObj))
        {
            Vector3Int targetPos = MapManager.Instance.grid_Ground.WorldToCell(itemNetObj.transform.position);
            if (pathManager.State_MoveLong(targetPos, 2))
            {
                State_TryToSendEmoji(0, Emoji.Happy, 1, false);
            }
            actionManager.State_PickUp(1f);
            return true;
        }
        return false;
    }
    #region//睡觉
    public virtual bool State_Think_GoToSleep()
    {
        if (bodyController.bodyAction_Cur.bodyActionType == BodyActionType.LayToLeft || 
            bodyController.bodyAction_Cur.bodyActionType == BodyActionType.LayToRight || 
            bodyController.bodyAction_Cur.bodyActionType == BodyActionType.LayToUp)
        {
            //已经睡觉
            return true;
        }
        if (brainManager.state_sleepPostion.isValue)
        {
            //有床位
            GameObject obj = MapManager.Instance.GetBuildingObj(brainManager.state_sleepPostion.position);
            if (obj.TryGetComponent(out BuildingObj_Bed bed) && bed.GetBedState() == BedState.Empty)
            {
                if (pathManager.vector3Int_CurPos == brainManager.state_sleepPostion.position)//我已经在床位这里,睡觉
                {
                    bed.Local_StartingSleep(this); return true;
                }
                else
                {
                    if (pathManager.State_MoveLong(brainManager.state_sleepPostion.position, 3)) return true;
                    else { brainManager.ForState_ResetSleepPos(); Debug.Log("ResrtSleep") ; return false; }
                }
            }
            //我要找一个新床铺
            return State_Think_FindSleepPosFast();
        }
        else
        {
            return false;
        }
    }
    public virtual bool State_Think_FindSleepPosFast(int maxDistance = 15)
    {
        if (brainManager.state_sleepPostion.isValue && MapManager.Instance.GetBuilding(brainManager.state_sleepPostion.position, out BuildingTile tile))
        {
            if (State_Think_CheckSleepPlace(tile)) return true;
        }
        brainManager.ForState_ResetSleepPos();
        List<Vector3Int> searchOrder = new List<Vector3Int>();
        // 生成螺旋搜索顺序（从近到远）
        for (int radius = 0; radius <= maxDistance; radius++)
        {
            for (int i = -radius; i <= radius; i++)
            {
                for (int j = -radius; j <= radius; j++)
                {
                    if (Mathf.Abs(i) == radius || Mathf.Abs(j) == radius)
                    {
                        searchOrder.Add(new Vector3Int(i, j, 0));
                    }
                }
            }
        }
        foreach (Vector3Int offset in searchOrder)
        {
            Vector3Int checkPos = brainManager.state_homePostion.position + offset;

            if (MapManager.Instance.GetBuilding(checkPos, out BuildingTile buildingTile))
            {
                if (State_Think_CheckSleepPlace(buildingTile))
                {
                    brainManager.ForState_SetSleepPos(checkPos);
                    return true;
                }
            }
        }
        return false;
    }
    public IEnumerator State_Think_FindSleepPos(int maxDistance = 15)
    {
        if (brainManager.state_sleepPostion.isValue && MapManager.Instance.GetBuilding(brainManager.state_sleepPostion.position, out BuildingTile tile))
        {
            if (State_Think_CheckSleepPlace(tile)) yield break;
        }
        brainManager.ForState_ResetSleepPos();
        List<Vector3Int> searchOrder = new List<Vector3Int>();
        // 生成螺旋搜索顺序（从近到远）
        for (int radius = 0; radius <= maxDistance; radius++)
        {
            for (int i = -radius; i <= radius; i++)
            {
                for (int j = -radius; j <= radius; j++)
                {
                    if (Mathf.Abs(i) == radius || Mathf.Abs(j) == radius)
                    {
                        searchOrder.Add(new Vector3Int(i, j, 0));
                    }
                }
            }
        }
        foreach (Vector3Int offset in searchOrder)
        {
            Vector3Int checkPos = brainManager.state_homePostion.position + offset;

            if (MapManager.Instance.GetBuilding(checkPos, out BuildingTile buildingTile))
            {
                if (State_Think_CheckSleepPlace(buildingTile))
                {
                    brainManager.ForState_SetSleepPos(checkPos);
                    yield return true;
                    yield break;
                }
            }
            yield return null; // 每帧检查一个位置
        }
    }
    public virtual bool State_Think_CheckSleepPlace(BuildingTile buildingTile)
    {
        bool temp = buildingTile.tileObj.transform.TryGetComponent(out BuildingObj_Bed bed) && (bed.GetBedState() == BedState.Empty);
        return temp;
    }
    #endregion
    #region//工作
    public virtual bool State_Think_GoToWork()
    {
        if (brainManager.state_workPostion.isValue)
        {
            if (pathManager.vector3Int_CurPos == brainManager.state_workPostion.position)
            {
                return true;
            }
            else
            {
                if (pathManager.State_MoveLong(brainManager.state_workPostion.position, 2)) return true;
                else { brainManager.ForState_ResetWorkPos(); return false; }
            }
        }
        return brainManager.state_workPostion.isValue;
    }
    public IEnumerator State_Think_FindWorkPos(int maxDistance = 15)
    {
        if (brainManager.state_workPostion.isValue && MapManager.Instance.GetBuilding(brainManager.state_workPostion.position, out BuildingTile tile))
        {
            if (State_Think_CheckWorkPlace(tile)) yield break;
        }
        brainManager.ForState_ResetWorkPos();
        List<Vector3Int> searchOrder = new List<Vector3Int>();
        // 生成螺旋搜索顺序（从近到远）
        for (int radius = 0; radius <= maxDistance; radius++)
        {
            for (int i = -radius; i <= radius; i++)
            {
                for (int j = -radius; j <= radius; j++)
                {
                    if (Mathf.Abs(i) == radius || Mathf.Abs(j) == radius)
                    {
                        searchOrder.Add(new Vector3Int(i, j, 0));
                    }
                }
            }
        }
        foreach (Vector3Int offset in searchOrder)
        {
            Vector3Int checkPos = brainManager.state_homePostion.position + offset;

            if (MapManager.Instance.GetBuilding(checkPos, out BuildingTile buildingTile))
            {
                if (State_Think_CheckWorkPlace(buildingTile))
                {
                    brainManager.ForState_SetWorkPos(checkPos);
                    yield break;
                }
            }
            yield return null; // 每帧检查一个位置
        }
    }
    public virtual bool State_Think_CheckWorkPlace(BuildingTile buildingTile)
    {
        return true;
    }
    #endregion
    #region//吃饭
    public virtual bool State_Think_GoForFood()
    {
        if (brainManager.state_foodPositon.isValue && brainManager.state_foodPositon.foodCount > 0)
        {
            if (pathManager.vector3Int_CurPos == brainManager.state_foodPositon.position)
            {
                actorNetManager.RPC_Local_SetHeadAction((short)HeadActionType.Eat, 1);
                actorNetManager.RPC_Local_SetHandAction((short)HandActionType.Eat, 1);
                brainManager.ForState_SetFoodPos(brainManager.state_foodPositon.position, brainManager.state_foodPositon.foodCount - 1);
                return true;
            }
            else
            {
                if (pathManager.State_MoveLong(brainManager.state_foodPositon.position, 2)) return true;
                else { brainManager.ForState_ResetFoodPos(); return false; }
            }
        }
        return false;
    }
    public IEnumerator State_Think_FindFoodPos(int maxDistance = 15)
    {
        if (brainManager.state_foodPositon.isValue && MapManager.Instance.GetBuilding(brainManager.state_foodPositon.position, out BuildingTile tile))
        {
            if (State_Think_CheckFoodPlace(tile)) yield break;
        }
        brainManager.ForState_ResetFoodPos();
        List<Vector3Int> searchOrder = new List<Vector3Int>();
        // 生成螺旋搜索顺序（从近到远）
        for (int radius = 0; radius <= maxDistance; radius++)
        {
            for (int i = -radius; i <= radius; i++)
            {
                for (int j = -radius; j <= radius; j++)
                {
                    if (Mathf.Abs(i) == radius || Mathf.Abs(j) == radius)
                    {
                        searchOrder.Add(new Vector3Int(i, j, 0));
                    }
                }
            }
        }
        foreach (Vector3Int offset in searchOrder)
        {
            Vector3Int checkPos = brainManager.state_homePostion.position + offset;

            if (MapManager.Instance.GetBuilding(checkPos, out BuildingTile buildingTile))
            {
                if (State_Think_CheckFoodPlace(buildingTile))
                {
                    brainManager.ForState_SetFoodPos(checkPos);
                    yield break;
                }
            }
            yield return null; // 每帧检查一个位置
        }
    }
    public virtual bool State_Think_CheckFoodPlace(BuildingTile buildingTile)
    {
        bool temp = buildingTile.tileObj.transform.TryGetComponent<BuildingObj_Machine_Cook>(out _);
        return temp;
    }
    #endregion
    #region//闲逛
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
        Vector3Int centerPos;
        if (brainManager.state_ActivityPostion.isValue)
        {
            if ((brainManager.state_ActivityPostion.position - pathManager.vector3Int_CurPos).sqrMagnitude > 225)
            {
                //太远了 我要回家
                if (pathManager.State_MoveLong(brainManager.state_ActivityPostion.position, 2, State_Think_BetweenStroll)) return;
            }
            else
            {
                //还行不是很远 微调目标区域位置
                offset = (brainManager.state_ActivityPostion.position.x > pathManager.vector3Int_CurPos.x) ? Vector3Int.right : Vector3Int.left;
                offset = (brainManager.state_ActivityPostion.position.y > pathManager.vector3Int_CurPos.y) ? Vector3Int.up : Vector3Int.down;
            }
        }
        centerPos = pathManager.vector3Int_CurPos + offset + new Vector3Int(dir_x * areaDistance, dir_y * areaDistance, 0);
        if (pathManager.State_MoveLong(centerPos, 2, State_Think_BetweenStroll)) return;
        centerPos = pathManager.vector3Int_CurPos + offset - new Vector3Int(dir_x * areaDistance, dir_y * areaDistance, 0);
        if (pathManager.State_MoveLong(centerPos, 2, State_Think_BetweenStroll)) return;
        centerPos = pathManager.vector3Int_CurPos + new Vector3Int(dir_x, dir_y, 0);
        if (pathManager.State_MoveShort(centerPos, State_Think_BetweenStroll)) return ;
        /*闲逛失败*/
        State_TryToSendEmoji(0.1f, Emoji.Search, 1, false);
    }
    /// <summary>
    /// 闲逛间隔
    /// </summary>
    public virtual void State_Think_BetweenStroll()
    {
        if (State_Think_TryToTalk()) return;
        State_Wait(random.Next(20, 40) * 0.1f);
    }
    #endregion
    #region//闲聊

    private const short const_TalkProbability = 75;
    private const float const_TalkWaitTime = 1.5f;
    private const float const_TalkDistanceSqr = 4;
    private Coroutine coroutine_Talking;
    public virtual void State_Think_Hear(GameEvent_AllClient_SomeoneSendEmoji eventData)
    {
        if (eventData.actor.Equals(this)) return;
        switch (eventData.emoji)
        {
            case Emoji.Greeting:
                {
                    if (actionManager.LookAt(eventData.actor, State_CalculateView()) && coroutine_Talking == null)
                    {
                        StartCoroutine(State_Think_TalkStart(eventData.actor));
                    }
                    break;
                }
            case Emoji.Talking:
                {
                    if (actionManager.LookAt(eventData.actor, State_CalculateView()) && coroutine_Talking == null)
                    {
                        coroutine_Talking = StartCoroutine(State_Think_Talking(eventData.actor));
                    }
                    break;
                }
            case Emoji.TalkEnd:
                {
                    if (actionManager.LookAt(eventData.actor, State_CalculateView())) 
                    { 
                        State_Think_TalkEnd(eventData.actor); 
                    }
                    break;
                }
            case Emoji.Yell:
                {
                    State_TryToSendEmoji(0.5f, Emoji.Puzzled, 1, false);
                    State_Follow(eventData.actor.pathManager.vector3Int_CurPos);
                    break;
                }
        }
    }
    public virtual bool State_Think_TryToTalk()
    {
        if (random.Next(0, 100) > const_TalkProbability || coroutine_Talking != null) return false;
        foreach (ActorManager actor in brainManager.State_GetNearbyActors())
        {
            if (actionManager.LookAt(actor, 5)&& actor.statusManager.statusType == StatusType.Human_Common)
            {
                State_Wait(const_TalkWaitTime);
                actorNetManager.RPC_State_NpcUseSkill((int)ActorSkill_NPC.Greeting, pathManager.vector3Int_CurPos, actor.actorNetManager.Object.Id);
                actorNetManager.RPC_LocalInput_TurnTo((actor.transform.position.x > transform.position.x));
                State_TryToSendEmoji(0, Emoji.Greeting, const_TalkWaitTime, false);
                return true;
            }
        }
        return false;
    }
    public IEnumerator State_Think_TalkStart(ActorManager who)
    {
        State_Wait(const_TalkWaitTime * 2 + 1);
        if ((who.transform.position - transform.position).sqrMagnitude > const_TalkDistanceSqr)
        {
            pathManager.State_Continue();
            pathManager.State_MoveLong(who.pathManager.vector3Int_CurPos, 2);
        }
        yield return new WaitForSeconds(const_TalkWaitTime);
        actorNetManager.RPC_State_NpcUseSkill((int)ActorSkill_NPC.Talk, pathManager.vector3Int_CurPos, who.actorNetManager.Object.Id);
        actorNetManager.RPC_LocalInput_TurnTo((who.transform.position.x > transform.position.x));
        State_TryToSendEmoji(0, Emoji.Talking, const_TalkWaitTime, false);
    }
    public IEnumerator State_Think_Talking(ActorManager who)
    {
        State_Wait(const_TalkWaitTime * 2 + 1);
        yield return new WaitForSeconds(const_TalkWaitTime + random.Next(0, 10) * 0.1f);
        actorNetManager.RPC_State_NpcUseSkill((int)ActorSkill_NPC.Talk, pathManager.vector3Int_CurPos, who.actorNetManager.Object.Id);
        actorNetManager.RPC_LocalInput_TurnTo((who.transform.position.x > transform.position.x));
        if (random.Next(0, 100) > const_TalkProbability || brainManager.globalTime_Now == GlobalTime.Evening)
        {
            State_TryToSendEmoji(0, Emoji.TalkEnd, const_TalkWaitTime, false);
        }
        else
        {
            State_TryToSendEmoji(0, Emoji.Talking, const_TalkWaitTime, false);
        }
        coroutine_Talking = null;
    }
    public virtual void State_Think_TalkEnd(ActorManager who)
    {
        State_Wait(const_TalkWaitTime);
    }

    #endregion
    #region//其他行为
    /// <summary>
    /// 等待
    /// </summary>
    /// <param name="time"></param>
    public virtual void State_Wait(float time)
    {
        pathManager.State_Stop(time);
        State_ResetThinkTime(time);
    }
    #endregion
    #endregion
    #region//背包逻辑
    /// <summary>
    /// 查找物体(主机)
    /// </summary>
    /// <returns></returns>
    public virtual ItemData State_FindItemInBag(Func<ItemConfig, bool> function)
    {
        ItemData item = new ItemData(0);
        List<ItemData> items = actorNetManager.Local_ItemBag_Get();
        for (int i = 0; i < items.Count; i++)
        {
            if (function.Invoke(ItemConfigData.GetItemConfig(items[i].I)))
            {
                item = items[i];
            }
        }
        return item;
    }
    /// <summary>
    /// 拿出物体(主机)
    /// </summary>
    /// <param name="function"></param>
    public virtual void State_PutOnHand(Func<ItemConfig, bool> function)
    {
        if (itemManager.itemBase_OnHand != null && function.Invoke(itemManager.itemBase_OnHand.itemConfig)) return;
        List<ItemData> items = actorNetManager.Local_ItemBag_Get();
        for (int i = 0; i < items.Count; i++)
        {
            if (function.Invoke(ItemConfigData.GetItemConfig(items[i].I)))
            {
                int index = i;
                ItemData itemData_Old = items[i];
                ItemData itemData_New = new ItemData();
                actorNetManager.Local_ItemHand_Add(itemData_Old);
                actorNetManager.Local_ItemBag_Change(index, itemData_New);
                return;
            }
        }
    }
    /// <summary>
    /// 收起物体(主机)
    /// </summary>
    public virtual void State_PutDownHand()
    {
        ItemData itemData_Old = actorNetManager.Local_ItemHand;
        ItemData itemData_New = new ItemData(0);

        actorNetManager.Local_ItemHand_Change(itemData_Old, itemData_New);
        actorNetManager.Local_ItemBag_Add(0, itemData_Old, ItemFrom.Hand);
    }
    /// <summary>
    /// 刷新背包
    /// </summary>
    public virtual void State_ResetBag()
    {
        List<ItemData> itemDatas = new List<ItemData>();
        if (actorConfig.Bag_List != null)
        {
            foreach (LootFixedInfo lootFixedInfo in actorConfig.Bag_List)
            {
                ItemData item = itemManager.CreateItemData(lootFixedInfo.ID, (short)lootFixedInfo.CountMin);
                itemDatas.Add(item);
            }
        }
        actorNetManager.Local_ItemBag_Set(itemDatas);
    }
    public bool State_ChooseWeapon(ItemConfig itemConfig)
    {
        return itemConfig.Item_Type == ItemType.Weapon;
    }
    #endregion

    #region//交互
    public override void Local_PlayerClose(ActorManager player)
    {
        if (Local_IsInteractable())
        {
            actorUI.ShowSingal_R(true);
        }
        base.Local_PlayerClose(player);
    }
    public override void Local_PlayerFaraway(ActorManager player)
    {
        actorUI.HideAllSingal();
        Local_OverDialog();
        Local_OverDeal();
        base.Local_PlayerFaraway(player);
    }
    public override void Local_GetPlayerInput(ActorManager actor, KeyCode keyCode)
    {
        
            switch (keyCode)
        {
            case KeyCode.R:
                {
                    if (!brainManager.ForAll_GetAttackTarget(out _) && Local_IsInteractable())
                    {
                        actorUI.ShowSingal_Talk(true);
                        if (!bool_Dialog) Local_StartDialog();
                    }
                }
                break;
            case KeyCode.Escape:
                {
                    if (bool_Dialog) Local_OverDialog();
                    if (bool_Deal) Local_OverDeal();
                }
                break;
        }
        base.Local_GetPlayerInput(actor, keyCode);
    }
    #endregion
    #region//对话
    protected Dictionary<int, Action> dialogMap;
    protected TileUI_Dialog tileUI_Dialog = null;
    protected bool bool_Dialog = false;
    public override bool Local_IsInteractable()
    {
        return (!brainManager.ForAll_GetAttackTarget(out _) && !brainManager.ForAll_GetThreatenedTarget(out _));
    }
    public virtual void Local_StartDialog()
    {
        bool_Dialog = true;
        actorNetManager.RPC_LocalInput_StandDown(66);
        UIManager.Instance.ShowTileUI(Resources.Load<GameObject>("UI/TileUI/TileUI_Dialog"), out TileUI tileUI);
        tileUI_Dialog = tileUI.GetComponent<TileUI_Dialog>();
    }
    public virtual void Local_OverDialog()
    {
        bool_Dialog = false;
        actorNetManager.RPC_LocalInput_StandDown(0);
        UIManager.Instance.HideTileUI(tileUI_Dialog);
        tileUI_Dialog = null;
    }
    public virtual void ForAll_InitDialog()
    {

    }
    public void ChooseDialog(int index)
    {
        if (dialogMap.ContainsKey(index))
        {
            dialogMap[index]();
        }
    }
    public void ShowDialog(string textKey, List<DialogOption> options)
    {
        // 统一UI逻辑
        tileUI_Dialog.InitDialog("Role_String", $"{actorConfig.ID}", "Role_String", textKey);
        tileUI_Dialog.ResetDialog();
        StartCoroutine(tileUI_Dialog.InitOption(options));
    }
    #endregion
    #region//交易
    protected List<ItemData> list_goodItems = new List<ItemData>();
    protected TileUI_Deal tileUI_Deal = null;
    protected bool bool_Deal = false;
    public virtual void Local_StartDeal()
    {
        bool_Deal = true;
        UIManager.Instance.ShowTileUI(Resources.Load<GameObject>("UI/TileUI/TileUI_DealUI"), out TileUI tileUI);
        tileUI_Deal = tileUI.GetComponent<TileUI_Deal>();
        tileUI_Deal.Init(this);
    }
    /// <summary>
    /// 结束交易
    /// </summary>
    /// <param name="actor"></param>
    public virtual void Local_OverDeal()
    {
        bool_Deal = false;
        UIManager.Instance.HideTileUI(tileUI_Deal);
    }
    /// <summary>
    /// 报价
    /// </summary>
    /// <param name="itemData"></param>
    /// <returns></returns>
    public virtual int Local_Offer(ItemData itemData)
    {
        return 0;
    }
    /// <summary>
    /// 刷新商品
    /// </summary>
    public virtual void Local_ResetGood()
    {
        list_goodItems.Clear();
        if (actorConfig.Goods_List != null)
        {
            int weightCount = 0;
            foreach (LootRandomInfo info in actorConfig.Goods_List)
            {
                weightCount += info.Weight;
            }
            for (int i = 0; i < actorConfig.Goods_Count; i++)
            {
                int temp = 0;
                for (int j = 0; j < actorConfig.Goods_List.Length; j++)
                {
                    temp += actorConfig.Goods_List[j].Weight;
                    if (random.Next(0, weightCount) < temp)
                    {
                        list_goodItems.Add(itemManager.CreateItemData(actorConfig.Goods_List[j].ID));
                        break;
                    }
                }
            }
        }
        actorNetManager.Local_Coin = actorConfig.Coin;
    }
    /// <summary>
    /// 获取商品
    /// </summary>
    /// <returns></returns>
    public List<ItemData> Local_GetGood()
    {
        return list_goodItems;
    }
    #endregion
    #region//Skill
    public override void ForAll_Listen_NpcAction(int id, Vector3Int vector3, NetworkId networkId)
    {
        switch ((ActorSkill_NPC)id)
        {
            case ActorSkill_NPC.Pick:
                {
                    AllClient_Pick(); break;
                }
            case ActorSkill_NPC.Talk:
                {
                    AllClient_Talk(); break;
                }
            case ActorSkill_NPC.Greeting:
                {
                    AllClient_Greeting(); break;
                }
        }
        base.ForAll_Listen_NpcAction(id, vector3, networkId);
    }
    private void AllClient_Pick()
    {
        bodyController.SetAnimatorTrigger(BodyPart.Hand, "Pick");
        bodyController.SetAnimatorTrigger(BodyPart.Head, "Pick");
    }
    private void AllClient_Talk()
    {
        int headAnimator = random.Next(0, 3);
        int handAnimator = random.Next(0, 2);

        switch (headAnimator)
        {
            case 0: bodyController.SetAnimatorTrigger(BodyPart.Head, "Talk"); break;
            case 1: bodyController.SetAnimatorTrigger(BodyPart.Head, "Nod"); break;
            case 2: bodyController.SetAnimatorTrigger(BodyPart.Head, "Shake"); break;
        }
        switch (handAnimator)
        {
            case 0: bodyController.SetAnimatorTrigger(BodyPart.Hand, "Gesture_0"); break;
            case 1: bodyController.SetAnimatorTrigger(BodyPart.Hand, "Gesture_1"); break;
        }
    }
    private void AllClient_Greeting()
    {
        bodyController.SetAnimatorTrigger(BodyPart.Hand, "Greeting");
    }
    #endregion
    #region//Emoji
    private Coroutine coroutine_SendEmoji;
    /// <summary>
    /// 发送Emoji(服务器)
    /// </summary>
    /// <param name="wait">等待</param>
    /// <param name="emoji">表情类别</param>
    public void State_TryToSendEmoji(float wait, Emoji emoji, float duration, bool loop, short distance = 10)
    {
        if (actorAuthority.isState)
        {
            if (coroutine_SendEmoji != null) StopCoroutine(coroutine_SendEmoji);
            coroutine_SendEmoji = StartCoroutine(State_SendEmoji(wait, emoji, duration, loop, distance));
        }
    }
    /// <summary>
    /// 发送Emoji(服务器)
    /// </summary>
    /// <param name="wait"></param>
    /// <returns></returns>
    private IEnumerator State_SendEmoji(float wait, Emoji emoji, float duration, bool loop, short distance)
    {
        yield return new WaitForSeconds(wait);
        actionManager.AllClient_SendEmoji((short)emoji, duration, loop, distance);
    }
    #endregion
}
public enum ActorSkill_NPC
{
    Pick,
    Talk,
    Greeting,
}