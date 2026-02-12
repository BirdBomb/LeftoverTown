using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
/// <summary>
/// NPC基类脚本
/// </summary>
public class ActorManager_NPC : ActorManager
{
    [Header("NPC配置")]
    public ActorConfig_NPC config;
    [HideInInspector]
    public float float_StateThinkTimer;
    [Header("思考间隔")]
    public float float_StateThinkCD = 1;
    [HideInInspector]
    public float float_StateAttackTimer;
    [Header("攻击间隔")]
    public float float_StateAttackCD = 1;

    #region//初始化NPC
    public override void AllClient_Init()
    {
        AllClient_InitNPCData();
        base.AllClient_Init();
    }
    public override void State_Init()
    {
        actorNetManager.Object.AssignInputAuthority(actorNetManager.Object.StateAuthority);
        float_StateThinkCD += new System.Random().Next(0, 100) * 0.01f;
        State_RsetThinkTime(float_StateThinkCD);
        State_InitNPCData();
        base.State_Init();
    }
    /// <summary>
    /// 初始化NPC数据(客户端)
    /// </summary>
    public virtual void AllClient_InitNPCData()
    {
        statusManager.statusType = config.status_Type;
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

        State_InitHeadAndBody(itemManager.CreateItemData(config.short_HatID), itemManager.CreateItemData(config.short_ClothesID));
        State_InitFace(config.str_Title, eyeID, hairID, hairColor);
        State_InitAbilityData(config.short_Hp, config.short_Armor, config.short_Resistance, config.short_Speed);
        State_InitFine(config.short_Fine);
        State_ResetBag();
        State_ResetDrop();
    }
    public override void AllClient_AddListener()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_AllClient_SomeoneSendEmoji>().Subscribe(_ =>
        {
            AllClient_Listen_RoleSendEmoji(_.actor, _.emoji, _.distance);
            if (actorAuthority.isState) State_Listen_RoleSendEmoji(_.actor, _.emoji, _.distance);

        }).AddTo(this);
        MessageBroker.Default.Receive<GameEvent.GameEvent_AllClient_SomeoneCommit>().Subscribe(_ =>
        {
            AllClient_Listen_RoleCommit(_.actor, _.commit, _.fine);
            if (actorAuthority.isState) State_Listen_RoleCommit(_.actor, _.commit, _.fine);
        }).AddTo(this);
        MessageBroker.Default.Receive<GameEvent.GameEvent_All_UpdateHour>().Subscribe(_ =>
        {
            AllClient_Listen_UpdateTime(_.hour, _.day, _.now);
        }).AddTo(this);
        base.AllClient_AddListener();
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
                State_RsetAttackTime(float_StateAttackCD);
                State_AttackLoop();
            }

        }
        base.State_FixedUpdateNetwork(dt);
    }
    public override void State_CustomUpdate()
    {
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
        if(hour == 0) Local_ResetGood();
        brainManager.SetTime(date, hour, globalTime);
        base.AllClient_Listen_UpdateTime(hour, date, globalTime);
    }
    public override void State_Listen_UpdateTime(int hour, int date, GlobalTime globalTime)
    {
        if (hour == 0) State_ResetBag();
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
                who.actionManager.AllClient_Commit(CommitState.Attacking, 10); 
                State_InThreatened(who);
            }
            State_TryToSendEmoji(0, Emoji.Panic);
        }
        base.State_Listen_MyselfHpChange(parameter, reason, id);
    }
    public override void State_Listen_MyselfDead(int parameter, HpChangeReason reason, NetworkId id)
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
        base.State_Listen_MyselfDead(parameter, reason, id);
    }
    public override void State_Listen_RoleSendEmoji(ActorManager actor, Emoji emoji, float distance)
    {
        if (actionManager.HearTo(actor, distance))
        {
            if (emoji == Emoji.Yell) { State_TryToSendEmoji(0, Emoji.Puzzled); }
        }
        base.State_Listen_RoleSendEmoji(actor, emoji, distance);
    }
    public override void State_Listen_RoleCommit(ActorManager who, CommitState commit, short val)
    {
        if (actionManager.LookAt(who, State_CalculateView()))
        {
            State_TryToSendEmoji(0, Emoji.Yell);
        }
        base.State_Listen_RoleCommit(who, commit, val);
    }
    public override void AllClient_Listen_ChangeAttackTarget(NetworkId id)
    {
        if (id != new NetworkId())
        {
            actorUI.HideSingal();
        }
        else
        {

        }
        base.AllClient_Listen_ChangeAttackTarget(id);
    }
    public override void AllClient_Listen_ChangeThreatenedTarget(NetworkId id)
    {
        if (id != new NetworkId())
        {
            actorUI.HideSingal();
        }
        else
        {

        }
        base.AllClient_Listen_ChangeThreatenedTarget(id);
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
            if (actionManager.LookAt(brainManager.allClient_actorManager_AttackTarget, config.short_View))
            {
                brainManager.State_SetSearchPos(brainManager.allClient_actorManager_AttackTarget.pathManager.vector3Int_CurPos);
            }
            else
            {
                State_TryToSendEmoji(0, Emoji.Search);
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
                if (brainManager.actorManagers_Nearby[i].actorNetManager.Local_Fine > 0)
                {
                    State_InThreatened(brainManager.actorManagers_Nearby[i]);
                    return true;
                }
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
    public virtual int State_CalculateView()
    {
        if (bodyController.bodyAction_Cur == BodyAction.LayToRight) return 1;
        if (bodyController.bodyAction_Cur == BodyAction.LayToLeft) return 1;
        if (bodyController.bodyAction_Cur == BodyAction.LayToUp) return 1;
        return config.short_View;
    }
    #endregion
    #region//思考
    /// <summary>
    /// 思考循环
    /// </summary>
    public void State_ThinkLoop()
    {
        if (brainManager.State_FocusOnNearbyItem())
        {
            State_Think_GoToPickUp();
            return;
        }
        else
        {
            State_ThinkByTimeUpdate(brainManager.day_Now, brainManager.hour_Now, brainManager.globalTime_Now);
        }
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
        if (time == GlobalTime.Forenoon)
        {
            if (!State_Think_GoToWork())
            {
                State_Think_GoToStroll_Long(10, 5);
            }
            return;
        }
        if (time == GlobalTime.Afternoon)
        {
            if (!State_Think_GoToWork())
            {
                State_Think_GoToStroll_Long(10, 5);
            }
            return;
        }
        if (time == GlobalTime.Evening)
        {
            if (!State_Think_GoToSleep())
            {
                State_Think_GoToStroll_Long(10, 5);
            }
            return;
        }
        State_Think_GoToStroll_Long(10, 5);
    }
    /// <summary>
    /// 根据时间变化决定动作(关键时间触发)
    /// </summary>
    public virtual void State_ThinkByTimeChange(int date, int hour, GlobalTime globalTime)
    {
        //if (globalTime == GlobalTime.Forenoon) { State_Think_FindWork(); }
        //if (globalTime == GlobalTime.Afternoon) { State_Think_FindWork(); }
        if (globalTime == GlobalTime.Evening) { State_Think_FindBed(); }
    }
    #endregion

    #region//攻击逻辑
    /// <summary>
    /// 进入攻击状态
    /// </summary>
    /// <param name="actor"></param>
    public virtual void State_InAttack(ActorManager actor)
    {
        State_TryToSendEmoji(0, Emoji.Attack);
        pathManager.State_SetFrezzeTime(0);
        pathManager.State_ClearPath();
        actorNetManager.RPC_State_NpcChangeAttackTarget(actor.actorNetManager.Object.Id);
        State_PutOnHand((itemConfig) =>
        {
            if (itemConfig.Item_Type == ItemType.Weapon) return true;
            return false;
        });
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
            float val = State_CheckingAttackingDistance();
            if (val > 1)
            {
                //攻击距离外
                State_Follow(brainManager.allClient_actorManager_AttackTarget.pathManager.vector3Int_CurPos);
            }
            else
            {
                //攻击距离内
                if (brainManager.allClient_actorManager_AttackTarget.actorState != ActorState.Dead)
                {
                    actorNetManager.RPC_State_NpcChangeAttackState(true);
                }
                else
                {
                    actorNetManager.RPC_State_NpcChangeAttackTarget(new NetworkId());
                }
                if (val < 0.5f)
                {
                    //撤退
                    State_Retreat(brainManager.allClient_actorManager_AttackTarget.transform.position);
                }
                else
                {
                    //走位
                    State_Elude(brainManager.allClient_actorManager_AttackTarget.transform.position);
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
    /// <param name="vector2"></param>
    public virtual void State_Search(float searchTime)
    {
        if (brainManager.state_searchPostion.isValue)
        {
            State_RsetThinkTime(searchTime);
            pathManager.State_MovePostion(brainManager.state_searchPostion.position);
            brainManager.State_ResetSearchPos();
        }
    }
    /// <summary>
    /// 更新攻击状态
    /// </summary>
    public virtual void AllClient_UpdateAttack(float dt)
    {
        if (brainManager.allClient_actorManager_AttackTarget)
        {
            inputManager.Simulate_InputMousePos(brainManager.allClient_actorManager_AttackTarget.transform.position);
            if (brainManager.allClient_AttackState)
            {
                brainManager.allClient_AttackState = !inputManager.Simulate_InputMousePress(dt, ActorInputManager.MouseInputType.PressRightThenPressLeft);
            }
        }
        else
        {
            actionManager.FaceTo(bodyController.turnDir);
        }

    }
    /// <summary>
    /// 检查攻击距离
    /// </summary>
    /// <returns>实际距离/射程</returns>
    public float State_CheckingAttackingDistance()
    {
        float attackDistance = WeaponConfigData.GetWeaponConfig(itemManager.itemBase_OnHand.itemData.I).Distance;
        float realDistance = Vector3.Distance(brainManager.allClient_actorManager_AttackTarget.transform.position, transform.position);
        return realDistance / attackDistance;
    }

    #endregion
    #region//威胁逻辑
    /// <summary>
    /// 进入威胁状态
    /// </summary>
    /// <param name="actor"></param>
    public virtual void State_InThreatened(ActorManager actor)
    {
        State_TryToSendEmoji(0, Emoji.Panic);
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
    /// 前往拾取
    /// </summary>
    /// <returns>是否可以去拾取</returns>
    public void State_Think_GoToPickUp()
    {
        if (brainManager.allClient_ItemNetObj_Target != null)
        {
            Vector3Int targetPos = MapManager.Instance.grid_Ground.WorldToCell(brainManager.allClient_ItemNetObj_Target.transform.position);
            if (pathManager.State_MovePostion(targetPos))
            {
                State_TryToSendEmoji(0, Emoji.Happy);
            }
            actionManager.State_PickUp(1f);
        }
    }
    /// <summary>
    /// 去睡觉
    /// </summary>
    /// <returns>可以睡觉</returns>
    public virtual bool State_Think_GoToSleep()
    {
        if (bodyController.bodyAction_Cur == BodyAction.LayToLeft || bodyController.bodyAction_Cur == BodyAction.LayToRight || bodyController.bodyAction_Cur == BodyAction.LayToUp)
        {
            //已经睡觉
            return true;
        }
        else
        {
            //还没睡觉
            if (brainManager.state_sleepPostion.isValue)
            {
                //有床位
                GameObject obj = MapManager.Instance.GetBuildingObj(brainManager.state_sleepPostion.position);
                BuildingObj_Bed bed = obj.GetComponent<BuildingObj_Bed>();
                if (bed != null && !bed.bool_SleeperOn)
                {
                    if (pathManager.vector3Int_CurPos == brainManager.state_sleepPostion.position)
                    {
                        //我已经在床位这里
                        bed.Local_StartingSleep(this);
                    }
                    else
                    {
                        //我还没到床位那里
                        pathManager.State_MovePostion(brainManager.state_sleepPostion.position);
                    }
                    return true;
                }
                return State_Think_FindBed();
            }
            else
            {
                //没床位
                return false;
            }
        }
    }
    /// <summary>
    /// 去工作
    /// </summary>
    /// <returns>可以工作</returns>
    public virtual bool State_Think_GoToWork()
    {
        if (brainManager.state_workPostion.isValue)
        {
            pathManager.State_MovePostion(brainManager.state_workPostion.position);
            return true;
        }
        return false;
    }
    /// <summary>
    /// 去吃东西
    /// </summary>
    /// <returns></returns>
    public virtual bool State_Think_GoForFood()
    {
        if (brainManager.state_foodPositon.isValue)
        {
            if (brainManager.state_foodPositon.foodCount > 0)
            {
                //确认我是否还需要吃东西
                if (Vector3.Distance(brainManager.state_foodPositon.position, pathManager.vector3Int_CurPos) < 2)
                {
                    actorNetManager.RPC_Local_SetHeadAction((short)HeadAction.Eat);
                    actorNetManager.RPC_Local_SetHandAction((short)HandAction.Eat);
                    brainManager.State_SetFoodPos(brainManager.state_foodPositon.position, brainManager.state_foodPositon.foodCount - 1);
                }
                pathManager.State_MovePostion(brainManager.state_foodPositon.position);
                return true;
            }
            //我不需要吃东西,离开
            return false;
        }
        //我没找到吃东西的位置,离开
        return false;
    }
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
                    State_TryToSendEmoji(0.1f, Emoji.Search);
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
    /// 寻找床位
    /// </summary>
    /// <param name="func"></param>
    public virtual bool State_Think_FindBed()
    {
        brainManager.ResetSleepPos();
        for (int i = -10; i < 10; i++)
        {
            for (int j = -10; j < 10; j++)
            {
                if (MapManager.Instance.GetBuilding(brainManager.state_homePostion.position + new Vector3Int(i, j, 0), out BuildingTile buildingTile))
                {
                    if (buildingTile.tileObj.transform.TryGetComponent(out BuildingObj_Bed bed))
                    {
                        if (!bed.bool_SleeperOn || bed.networkId_SleeperNew == actorNetManager.Object.Id)
                        {
                            brainManager.State_SetSleepPos(brainManager.state_homePostion.position + new Vector3Int(i, j, 0));
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }
    /// <summary>
    /// 寻找食物
    /// </summary>
    /// <returns></returns>
    public virtual bool State_Think_FindFood()
    {
        brainManager.State_ResetFoodPos();
        for (int i = -10; i < 10; i++)
        {
            for (int j = -10; j < 10; j++)
            {
                if (MapManager.Instance.GetBuilding(brainManager.state_homePostion.position + new Vector3Int(i, j, 0), out BuildingTile buildingTile))
                {
                    if (buildingTile.tileObj.transform.TryGetComponent(out BuildingObj_Machine_Cook cook))
                    {
                        brainManager.State_SetFoodPos(brainManager.state_homePostion.position + new Vector3Int(i, j, 0), 3);
                    }
                }
            }
        }
        return false;
    }
    /// <summary>
    /// 寻找工作地点
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual bool State_Think_FindWork()
    {
        brainManager.State_ResetWorkPos();
        for (int i = -5; i < 5; i++)
        {
            for (int j = -5; j < 5; j++)
            {
                if (MapManager.Instance.GetBuilding(brainManager.state_homePostion.position + new Vector3Int(i, j, 0), out BuildingTile buildingTile))
                {
                    if (buildingTile.tileID == 0)
                    {
                        brainManager.State_SetWorkPos(brainManager.state_homePostion.position + new Vector3Int(i, j, 0));
                        return true;
                    }
                }
            }
        }
        return false;
    }
    /// <summary>
    /// 寻找目标地块
    /// </summary>
    /// <param name="func"></param>
    /// <returns></returns>
    public bool State_Think_FindTile(Func<Vector3Int, bool> func, out Vector3Int pos)
    {
        for (int i = -10; i < 10; i++)
        {
            for (int j = -10; j < 10; j++)
            {
                if (func.Invoke(brainManager.state_homePostion.position + new Vector3Int(i, j, 0)))
                {
                    pos = brainManager.state_homePostion.position + new Vector3Int(i, j, 0);
                    return true;
                }
            }
        }
        pos = Vector3Int.zero;
        return false;
    }
    /// <summary>
    /// 开始谈话
    /// </summary>
    /// <param name="who"></param>
    public virtual void State_Think_TalkStart(ActorManager who)
    {
        State_RsetThinkTime(5);
        State_TryToSendEmoji(0.2f, Emoji.Talking);
        Vector3Int offset;
        if (who.transform.position.x >= pathManager.vector3Int_CurPos.x) offset = Vector3Int.left;
        else offset = Vector3Int.right;
        if (pathManager.State_MovePostion(who.pathManager.vector3Int_CurPos + offset)) return;
        offset *= -1;
        if (pathManager.State_MovePostion(who.pathManager.vector3Int_CurPos + offset)) return;
        offset *= 0;
        pathManager.State_MovePostion(who.pathManager.vector3Int_CurPos);
    }
    /// <summary>
    /// 和某人谈话
    /// </summary>
    public virtual void State_Think_Talking(ActorManager who)
    {
        State_RsetThinkTime(5);
        if (new System.Random().Next(0, 10) < 3)
        {
            State_TryToSendEmoji(0.5f, Emoji.TalkEnd, 5);
        }
        else
        {
            State_TryToSendEmoji(2, Emoji.Talking, 5);
            actorNetManager.RPC_LocalInput_TurnTo((who.transform.position.x > transform.position.x));
        }
    }
    /// <summary>
    /// 结束谈话
    /// </summary>
    public virtual void State_Think_TalkEnd(ActorManager who)
    {
        State_RsetThinkTime(1);
        State_StartRunAway(who.transform);
    }
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
        if (itemManager.itemBase_OnHand != null && function.Invoke(itemManager.itemBase_OnHand.itemConfig))
        {
            return;
        }
        else
        {
            List<ItemData> items = actorNetManager.Local_ItemBag_Get();
            for (int i = 0; i < items.Count; i++)
            {
                if (function.Invoke(ItemConfigData.GetItemConfig(items[i].I)))
                {
                    int index = i;
                    ItemData itemData_Old = items[i];
                    ItemData itemData_New = new ItemData();
                    actorNetManager.OnlyState_ItemHand_Add(itemData_Old);
                    actorNetManager.Local_ItemBag_Change(index, itemData_New);
                    return;
                }
            }
        }
    }
    /// <summary>
    /// 收起物体(主机)
    /// </summary>
    public virtual void State_PutDownHand()
    {
        ItemData itemNew = new ItemData(0);
        actorNetManager.OnlyState_ItemHand_Add(itemNew);
    }
    /// <summary>
    /// 刷新背包
    /// </summary>
    public virtual void State_ResetBag()
    {
        List<ItemData> itemDatas = new List<ItemData>();
        for (int i = 0; i < config.bagInfos_Base.Count; i++)
        {
            int count = new System.Random().Next(config.bagInfos_Base[i].CountMin, config.bagInfos_Base[i].CountMax + 1);
            ItemData item = itemManager.CreateItemData(config.bagInfos_Base[i].ID, (short)count);
            itemDatas.Add(item);
        }
        actorNetManager.Local_ItemBag_Set(itemDatas);
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

    #region//交互
    public override void Local_PlayerClose(ActorManager player)
    {
        if (Local_CanDialog())
        {
            actorUI.ShowSingalR();
        }
        base.Local_PlayerClose(player);
    }
    public override void Local_PlayerFaraway(ActorManager player)
    {
        actorUI.HideSingal();
        Local_OverDialog(player);
        Local_OverDeal(player);
        base.Local_PlayerFaraway(player);
    }
    public override void Local_GetPlayerInput_R(ActorManager player)
    {
        if (!brainManager.allClient_actorManager_AttackTarget)
        {
            actorUI.ShowSingalTalk();
            if (!bool_Dialog)
            {
                Local_StartDialog(player);
            }
        }
        base.Local_GetPlayerInput_R(player);
    }
    #endregion
    #region//对话
    protected TileUI_Dialog tileUI_Dialog = null;
    protected bool bool_Dialog = false;
    public override bool Local_CanDialog()
    {
        if (brainManager.allClient_actorManager_AttackTarget != null || brainManager.allClient_actorManager_ThreatenedTarget != null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// 开始谈话
    /// </summary>
    /// <param name="actor"></param>
    /// <param name="index"></param>
    public virtual void Local_StartDialog(ActorManager player)
    {
        if (!bool_Dialog && player != null && player.actorNetManager.Object != null)
        {
            bool_Dialog = true;
            actorNetManager.RPC_LocalInput_StandDown(66);
            UIManager.Instance.ShowTileUI(Resources.Load<GameObject>("UI/TileUI/TileUI_Dialog"), out TileUI tileUI);
            tileUI_Dialog = tileUI.GetComponent<TileUI_Dialog>();
            actorNetManager.RPC_LocalInput_TurnTo((player.transform.position.x > transform.position.x));
            Local_SetDialog(player, 0);
        }
    }
    /// <summary>
    /// 设置谈话
    /// </summary>
    public virtual void Local_SetDialog(ActorManager player, int index)
    {

    }
    /// <summary>
    /// 结束谈话
    /// </summary>
    /// <param name="actor"></param>
    public virtual void Local_OverDialog(ActorManager player)
    {
        if (bool_Dialog && player != null && player.actorNetManager.Object != null)
        {
            bool_Dialog = false;
            actorNetManager.RPC_LocalInput_StandDown(0);
            UIManager.Instance.HideTileUI(tileUI_Dialog);
            tileUI_Dialog = null;
        }
    }

    #endregion
    #region//交易
    protected List<ItemData> goodList = new List<ItemData>();
    protected TileUI_Deal tileUI_Deal = null;
    protected bool bool_Deal = false;
    /// <summary>
    /// 开始交易
    /// </summary>
    /// <param name="actor"></param>
    public virtual void Local_StartDeal(ActorManager actor)
    {
        bool_Deal = true;
        if (actor != null && actor.actorNetManager.Object != null)
        {
            UIManager.Instance.ShowTileUI(Resources.Load<GameObject>("UI/TileUI/TileUI_DealUI"), out TileUI tileUI);
            tileUI_Deal = tileUI.GetComponent<TileUI_Deal>();
            Local_SetDeal(tileUI_Deal);
        }
    }
    /// <summary>
    /// 设置交易
    /// </summary>
    /// <param name="tileUI_Deal"></param>
    public virtual void Local_SetDeal(TileUI_Deal tileUI)
    {
        tileUI.Init(this);
    }
    /// <summary>
    /// 结束交易
    /// </summary>
    /// <param name="actor"></param>
    public virtual void Local_OverDeal(ActorManager actor)
    {
        bool_Deal = false;
        if (actor != null && actor.actorNetManager.Object != null)
        {
            UIManager.Instance.HideTileUI(tileUI_Deal);
        }
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
        actorNetManager.Local_Coin = config.short_Coin;
        goodList.Clear();
        List<ItemData> itemDatas = new List<ItemData>();
        for (int i = 0; i < config.goodInfos_Base.Count; i++)
        {
            int count = new System.Random().Next(config.goodInfos_Base[i].CountMin, config.goodInfos_Base[i].CountMax + 1);
            ItemData item = itemManager.CreateItemData(config.goodInfos_Base[i].ID, (short)count);
            itemDatas.Add(item);
        }
        for (int i = 0; i < config.goodInfos_Extra.Count; i++)
        {
            int temp = new System.Random().Next(0, 1000);
            if (temp > config.goodInfos_Extra[i].Weight)
            {
                ItemData item = itemManager.CreateItemData(config.goodInfos_Extra[i].ID, config.goodInfos_Extra[i].Count);
                itemDatas.Add(item);
            }
        }
        for (int i = 0; i < itemDatas.Count; i++)
        {
            if (itemDatas[i].C >= 0)
            {
                for (int j = 0; j < itemDatas[i].C; j++)
                {
                    ItemData itemData = itemDatas[i];
                    itemData.C = 1;
                    goodList.Add(itemData);
                }
            }
        }
    }
    /// <summary>
    /// 获取商品
    /// </summary>
    /// <returns></returns>
    public List<ItemData> Local_GetGood()
    {
        return goodList;
    }
    #endregion
    #region//Emoji
    /// <summary>
    /// 发送消息(服务器)
    /// </summary>
    /// <param name="text"></param>
    public void State_TryToSendText(string text, Emoji emoji)
    {
        if (actorAuthority.isLocal)
        {
            actionManager.AllClient_SendText(text, (int)emoji);
        }
    }
    /// <summary>
    /// 发送消息(本地)
    /// </summary>
    /// <param name="text"></param>
    public void Local_TryToSendText(string text, Emoji emoji)
    {
        actionManager.AllClient_SendText(text, (int)emoji);
    }
    private Coroutine coroutine_SendEmoji;
    /// <summary>
    /// 发送Emoji(服务器)
    /// </summary>
    /// <param name="wait">等待</param>
    /// <param name="emoji">表情类别</param>
    public void State_TryToSendEmoji(float wait, Emoji emoji, short distance = 10)
    {
        if (actorAuthority.isState)
        {
            if(coroutine_SendEmoji != null)
            {
                StopCoroutine(coroutine_SendEmoji);
                coroutine_SendEmoji = null;
            }
            coroutine_SendEmoji = StartCoroutine(State_SendEmoji(wait, emoji, distance));
        }
    }
    /// <summary>
    /// 发送Emoji(服务器)
    /// </summary>
    /// <param name="wait"></param>
    /// <returns></returns>
    private IEnumerator State_SendEmoji(float wait, Emoji emoji,short distance)
    {
        yield return new WaitForSeconds(wait);
        actionManager.AllClient_SendEmoji((short)emoji, distance);
    }
    #endregion

}
[Serializable]
public struct ActorConfig_NPC
{
    [Header("初始名称")]
    public string str_Title;
    [Header("初始身份")]
    public StatusType status_Type;
    [Header("初始生命")]
    public short short_Hp;
    [Header("初始金币")]
    public short short_Coin;
    [Header("初始护甲")]
    public short short_Armor;
    [Header("初始魔抗")]
    public short short_Resistance;
    [Header("悬赏")]
    public short short_Fine;
    [Header("初始视野")]
    public short short_View;
    [Header("初始速度(分米每秒)")]
    public short short_Speed;
    [Header("初始帽子")]
    public short short_HatID;
    [Header("初始衣服")]
    public short short_ClothesID;
    [Header("固定背包")]
    public List<BaseLootInfo> bagInfos_Base;
    [Header("固定掉落")]
    public List<BaseLootInfo> lootInfos_Base; 
    [Header("额外掉落")]
    public List<ExtraLootInfo> lootInfos_Extra;
    [Header("固定商品")]
    public List<BaseLootInfo> goodInfos_Base;
    [Header("额外商品")]
    public List<ExtraLootInfo> goodInfos_Extra;
}