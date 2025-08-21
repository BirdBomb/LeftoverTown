using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Fusion;
using System;

public class ActorManager_NPC_Guard : ActorManager_NPC
{
    [Header("攻击间隔"), Range(1, 10)]
    public float float_AttackCD;
    private float float_ThinkCD = 1;
    private float float_SearchTime = 4;
    private float float_AttackTimer;
    private float float_ThinkTimer;
    [HideInInspector]
    public GlobalTime globalTime_Work = GlobalTime.Evening;
    #region//初始化
    public override void State_Init()
    {
        float_ThinkCD += new System.Random().Next(0, 100) * 0.01f;
        float_ThinkTimer = float_ThinkCD;
        base.State_Init();
    }
    #endregion
    #region//时间周期
    public override void FixedUpdate()
    {
        AllClient_AttackLoop(Time.fixedDeltaTime);
        base.FixedUpdate();
    }
    public override void State_FixedUpdateNetwork(float dt)
    {
        if (brainManager.allClient_actorManager_AttackTarget)
        {
            float_AttackTimer -= dt;
            if (float_AttackTimer < 0)
            {
                float_AttackTimer = float_AttackCD;
                float val = State_CheckingAttackingDistance();
                if (val > 1)
                {
                    State_Follow(brainManager.allClient_actorManager_AttackTarget.pathManager.vector3Int_CurPos);
                }
                else
                {
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
                        State_RunAway(brainManager.allClient_actorManager_AttackTarget);
                    }
                    else
                    {
                        State_Elude();
                    }

                }
            }
        }
        else
        {
            float_ThinkTimer -= dt;
            if (float_ThinkTimer < 0)
            {
                float_ThinkTimer = float_ThinkCD;
                if (State_CheckToPickUp())
                {
                    State_MoveToPickUp();
                }
                else
                {
                    State_Think();
                }
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
    public override void AllClient_Listen_UpdateTime(int hour, int date, GlobalTime globalTime)
    {
        brainManager.SetTime(globalTime);
        base.AllClient_Listen_UpdateTime(hour, date, globalTime);
    }
    public override void State_Listen_UpdateTime(int hour, int date, GlobalTime globalTime)
    {
        if (globalTime == globalTime_Work)
        {
            State_Think_FindSentry();
        }
        base.State_Listen_UpdateTime(hour, date, globalTime);
    }
    public override void State_Listen_MyselfHpChange(int parameter, HpChangeReason reason, NetworkId id)
    {
        NetworkObject networkObject = actorNetManager.Runner.FindObject(id);
        if (networkObject != null)
        {
            ActorManager who = networkObject.GetComponent<ActorManager>();
            if (who.actorAuthority.isPlayer && actionManager.LookAt(who, config.short_View))
            {
                who.actionManager.SetFine(500);
                if (brainManager.allClient_actorManager_AttackTarget == null)
                {
                    State_TryToSendEmoji(0, Emoji.Menace);
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
    public override void State_Listen_RoleCommit(ActorManager who, short val)
    {
        if (who.actorAuthority.isPlayer && actionManager.LookAt(who, config.short_View))
        {
            who.actionManager.SetFine(val);
            if (brainManager.allClient_actorManager_AttackTarget == null)
            {
                State_TryToSendEmoji(0, Emoji.Menace);
                State_InAttack(who);
            }
        }
        base.State_Listen_RoleCommit(who, val);
    }
    public override void State_Listen_RoleSendEmoji(ActorManager actor, Emoji emoji, float distance)
    {
        if (actionManager.HearTo(actor, distance))
        {
            switch (emoji)
            {
                case Emoji.Yell:
                    if (brainManager.allClient_actorManager_AttackTarget == null)
                    {
                        brainManager.SetSearch(actor.pathManager.vector3Int_CurPos);
                        State_TryToSendEmoji(0, Emoji.Puzzled);
                        State_Search();
                        pathManager.State_MovePostion(actor.pathManager.vector3Int_CurPos);
                    }
                    break;
            }
        }
        base.State_Listen_RoleSendEmoji(actor, emoji, distance);
    }
    public override void State_Listen_RoleInView(ActorManager actor)
    {
        brainManager.actorManagers_Nearby.Add(actor);
        base.State_Listen_RoleInView(actor);
    }
    public override void State_Listen_RoleOutView(ActorManager actor)
    {
        brainManager.actorManagers_Nearby.Remove(actor);
        base.State_Listen_RoleInView(actor);
    }
    public override void State_Listen_ItemInView(ItemNetObj obj)
    {
        brainManager.ItemNetObj_Nearby.Add(obj);
        base.State_Listen_ItemInView(obj);
    }
    public override void State_Listen_ItemOutView(ItemNetObj obj)
    {
        brainManager.ItemNetObj_Nearby.Remove(obj);
        base.State_Listen_ItemOutView(obj);
    }
    /// <summary>
    /// 检查附近
    /// </summary>
    public void State_CheckNearby()
    {
        if (brainManager.allClient_actorManager_AttackTarget != null)
        {
            if (!actionManager.LookAt(brainManager.allClient_actorManager_AttackTarget, config.short_View))
            {
                State_TryToSendEmoji(0, Emoji.Search);
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
        for (int i = 0; i < brainManager.actorManagers_Nearby.Count; i++)
        {
            if (actionManager.LookAt(brainManager.actorManagers_Nearby[i], config.short_View))
            {
                if (brainManager.actorManagers_Nearby[i].statusManager.statusType == StatusType.Monster_Common)
                {
                    State_TryToSendEmoji(0, Emoji.Attack);
                    State_InAttack(brainManager.actorManagers_Nearby[i]);
                    return true;
                }
                if (brainManager.actorManagers_Nearby[i].statusManager.statusType == StatusType.Human_Offender)
                {
                    State_TryToSendEmoji(0, Emoji.Attack);
                    State_InAttack(brainManager.actorManagers_Nearby[i]);
                    return true;
                }
                if (brainManager.actorManagers_Nearby[i].actorNetManager.Local_Fine > 0)
                {
                    State_TryToSendEmoji(0, Emoji.Attack);
                    State_InAttack(brainManager.actorManagers_Nearby[i]);
                    return true;
                }
            }
        }
        return false;
    }
    #endregion
    #region//拾取状态
    /// <summary>
    /// 检查拾取
    /// </summary>
    /// <returns>发现目标</returns>
    private bool State_CheckToPickUp()
    {
        ItemNetObj target = null;
        float distance = float.MaxValue;
        for (int i = 0; i < brainManager.ItemNetObj_Nearby.Count; i++)
        {
            float temp = Vector2.Distance(transform.position, brainManager.ItemNetObj_Nearby[i].transform.position);
            if (temp < distance)
            {
                distance = temp;
                target = brainManager.ItemNetObj_Nearby[i];
            }
        }
        if (target != null)
        {
            /*前往目标*/
            brainManager.ItemNetObj_Target = target;
            return true;
        }
        else
        {
            /*没有目标*/
            brainManager.ItemNetObj_Target = null;
            return false;
        }
    }
    /// <summary>
    /// 前往拾取
    /// </summary>
    /// <returns>是否可以去拾取</returns>
    private void State_MoveToPickUp()
    {
        if (brainManager.ItemNetObj_Target != null)
        {
            Vector3Int targetPos = MapManager.Instance.grid_Ground.WorldToCell(brainManager.ItemNetObj_Target.transform.position);
            if (pathManager.State_MovePostion(targetPos))
            {
                State_TryToSendEmoji(0, Emoji.Happy);
            }
            actionManager.PickUp(1f);
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
        State_PutOnHand((itemConfig) =>
        {
            if (itemConfig.Item_Type == ItemType.Weapon) return true;
            return false;
        });
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
    /// <returns>实际距离/射程</returns>
    public float State_CheckingAttackingDistance()
    {
        float attackDistance = WeaponConfigData.GetWeaponConfig(itemManager.itemBase_OnHand.itemData.Item_ID).Distance;
        float realDistance = Vector3.Distance(brainManager.allClient_actorManager_AttackTarget.transform.position, transform.position);
        return realDistance / attackDistance;
    }
    /// <summary>
    /// 躲避
    /// </summary>
    private void State_Elude()
    {
        Vector3Int vector3 = Vector3Int.zero;
        int random = new System.Random().Next(1, 5);
        if (random == 1) vector3 = Vector3Int.down;
        if (random == 2) vector3 = Vector3Int.up;
        if (random == 3) vector3 = Vector3Int.right;
        if (random == 4) vector3 = Vector3Int.left;
        Vector3Int pos_R = pathManager.vector3Int_CurPos + vector3;
        pathManager.State_MovePostion(pos_R);
    }
    /// <summary>
    /// 追击
    /// </summary>
    /// <param name="vector2"></param>
    private void State_Follow(Vector3Int to)
    {
        pathManager.State_MovePostion(to);
    }
    /// <summary>
    /// 逃离
    /// </summary>
    /// <param name="from"></param>
    private void State_RunAway(ActorManager from)
    {
        int randomX = new System.Random().Next(0, 2);
        int randomY = new System.Random().Next(0, 2);
        Vector3Int dirX = Vector3Int.zero;
        Vector3Int dirY = Vector3Int.zero;

        #region//首先尝试对角逃跑
        if (from.pathManager.vector3Int_CurPos.x > pathManager.vector3Int_CurPos.x)
        {
            dirX += Vector3Int.left;
        }
        else if (from.pathManager.vector3Int_CurPos.x < pathManager.vector3Int_CurPos.x)
        {
            dirX += Vector3Int.right;
        }
        if (from.pathManager.vector3Int_CurPos.y > pathManager.vector3Int_CurPos.y)
        {
            dirY += Vector3Int.down;
        }
        else if (from.pathManager.vector3Int_CurPos.y < pathManager.vector3Int_CurPos.y)
        {
            dirY += Vector3Int.up;
        }
        if (pathManager.State_MovePostion(pathManager.vector3Int_CurPos + dirX + dirY)) return;
        #endregion
        #region//然后尝试四向逃跑
        int distanceX = Math.Abs(from.pathManager.vector3Int_CurPos.x - pathManager.vector3Int_CurPos.x);
        int distanceY = Math.Abs(from.pathManager.vector3Int_CurPos.y - pathManager.vector3Int_CurPos.y);
        if (distanceX >= distanceY)
        {
            if (pathManager.State_MovePostion(pathManager.vector3Int_CurPos + dirX)) return;
            if (randomY == 0)
            {
                dirY = Vector3Int.up;
            }
            else
            {
                dirY = Vector3Int.down;
            }
            if (pathManager.State_MovePostion(pathManager.vector3Int_CurPos + dirY)) return;
            dirY = -dirY;
            if (pathManager.State_MovePostion(pathManager.vector3Int_CurPos + dirY)) return;
        }
        else
        {
            if (pathManager.State_MovePostion(pathManager.vector3Int_CurPos + dirY)) return;
            if (randomX == 0)
            {
                dirX = Vector3Int.right;
            }
            else
            {
                dirX = Vector3Int.left;
            }
            if (pathManager.State_MovePostion(pathManager.vector3Int_CurPos + dirX)) return;
            dirX = -dirX;
            if (pathManager.State_MovePostion(pathManager.vector3Int_CurPos + dirX)) return;
        }
        #endregion
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
    /// <summary>
    /// 攻击循环
    /// </summary>
    public void AllClient_AttackLoop(float dt)
    {
        if (brainManager.allClient_actorManager_AttackTarget)
        {
            inputManager.Simulate_InputMousePos(brainManager.allClient_actorManager_AttackTarget.transform.position);
            if (brainManager.bool_AttackState)
            {
                brainManager.bool_AttackState = !inputManager.Simulate_InputMousePress(dt, ActorInputManager.MouseInputType.PressRightThenPressLeft);
            }
        }
        else
        {
            actionManager.FaceTo(bodyController.turnDir);
        }
    }
    #endregion
    #region//闲置状态
    /// <summary>
    /// 思考该做什么
    /// </summary>
    private void State_Think()
    {
        if (brainManager.globalTime_Now == globalTime_Work && brainManager.workPostion.isValue)
        {
            if (brainManager.workPostion.postion != pathManager.vector3Int_CurPos)
            {
                State_Think_GoToSentry();
            }
        }
        else if (brainManager.globalTime_Now == globalTime_Work + 1 && brainManager.homePostion.isValue)
        {
            if (brainManager.homePostion.postion != pathManager.vector3Int_CurPos)
            {
                State_Think_GoToHome();
            }
        }
        else
        {
            State_Think_Stroll(3);
        }
    }
    /// <summary>
    /// 寻找岗哨
    /// </summary>
    private void State_Think_FindSentry()
    {
        for (int i = -10; i < 10; i++)
        {
            for (int j = -10; j < 10; j++)
            {
                if (MapManager.Instance.GetBuilding(brainManager.homePostion.postion + new Vector3Int(i, j, 0), out BuildingTile buildingTile))
                {
                    if (buildingTile.tileID == 2101)
                    {
                        brainManager.SetWork((brainManager.homePostion.postion + new Vector3Int(i, j, 0)));
                        return;
                    }
                }
            }
        }
    }
    /// <summary>
    /// 去站岗
    /// </summary>
    private void State_Think_GoToSentry()
    {
        pathManager.State_MovePostion(brainManager.workPostion.postion);
        State_PutOnHand((itemConfig) =>
        {
            if (itemConfig.Item_Type == ItemType.Weapon) return true;
            return false;
        });
    }
    /// <summary>
    /// 回家
    /// </summary>
    private void State_Think_GoToHome()
    {
        pathManager.State_MovePostion(brainManager.homePostion.postion + Vector3Int.down);
        State_PutDownHand();
    }
    /// <summary>
    /// 闲逛
    /// </summary>
    private void State_Think_Stroll(int distance, int tryTime = 0)
    {
        if (distance < 1 || tryTime > 3)
        {
            /*闲逛失败*/
            State_TryToSendEmoji(0.1f, Emoji.Search);
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

    #endregion
    #region//交互
    public override bool Local_CanDialog()
    {
        return false;
    }
    #endregion
}
