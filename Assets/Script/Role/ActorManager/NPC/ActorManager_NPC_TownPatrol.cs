using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
/// <summary>
/// 小镇势力夜巡队
/// </summary>
public class ActorManager_NPC_TownPatrol : ActorManager_NPC
{
    /// <summary>
    /// 已知岗哨
    /// </summary>
    private List<UnityEngine.Vector2> onlyState_sentryStationList = new List<UnityEngine.Vector2>();
    /// <summary>
    /// 上个岗哨
    /// </summary>
    private UnityEngine.Vector2 onlyState_sentryStationLast = UnityEngine.Vector2.zero;
    public override void FixedUpdate()
    {
        AllClient_AttackLoop(Time.fixedDeltaTime);
        base.FixedUpdate();
    }
    public override void State_FixedUpdateNetwork(float dt)
    {
        if (brainManager.allClient_actorManager_AttackTarget)
        {
            //if (brainManager.UpdateAttackTimer(dt))
            //{
            //    if (brainManager.CheckingAttackingDistance(dt))
            //    {
            //        if (brainManager.allClient_actorManager_AttackTarget.actorState != ActorState.Dead)
            //        {
            //            actorNetManager.RPC_State_NpcChangeAttackState(true);
            //        }
            //        else
            //        {
            //            actorNetManager.RPC_State_NpcChangeAttackTarget(new NetworkId());
            //        }
            //    }
            //}
        }

        base.State_FixedUpdateNetwork(dt);
    }
    #region//夜训队专有方法
    public override void State_Listen_MoveOther(ActorManager actor, Vector3Int where)
    {
        if (actionManager.LookAt(actor, config.short_View))
        {
            /*我看的见*/
            if (brainManager.allClient_actorManager_AttackTarget == actor)
            {
                //State_Follow(FollowType.Attack);
            }
            else
            {
                if (OnlyState_CanIAttack(actor))
                {
                    //State_TryToSendEmoji(0.1f, 9);
                    actorNetManager.RPC_State_NpcChangeAttackTarget(actor.actorNetManager.Object.Id);
                }
            }
        }
        else
        {
            /*我看不见*/
            if (brainManager.allClient_actorManager_AttackTarget == actor)
            {
                /*丢失攻击目标*/
                //State_Follow(FollowType.RunTo);
                //State_TryToSendEmoji(0.1f, 1);
                actorNetManager.RPC_State_NpcChangeAttackTarget(new NetworkId());
            }
        }
        base.State_Listen_MoveOther(actor, where);
    }
    public override void AllClient_Listen_MyselfHpChange(int parameter, NetworkId id)
    {
        ActorManager who = actorNetManager.Runner.FindObject(id).GetComponent<ActorManager>();
        if (who.actorAuthority.isPlayer && actionManager.LookAt(who, config.short_View))
        {
            who.actionManager.SetFine(500);
            //State_TryToSendEmoji(0.1f, 16);
        }
        base.AllClient_Listen_MyselfHpChange(parameter, id);
    }
    public override void State_Listen_RoleCommit(ActorManager who, short val)
    {
        if (actionManager.LookAt(who, config.short_View))
        {
            who.actionManager.SetFine(val);
        }
        //State_TryToSendEmoji(0.2f, 0);
        base.State_Listen_RoleCommit(who, val);
    }
    public override void State_Listen_WorldGlobalTimeChange(int hour, int date, GlobalTime globalTime)
    {
        if (!brainManager.allClient_actorManager_AttackTarget)
        {
            if (globalTime == GlobalTime.Dusk || globalTime == GlobalTime.Evening)
            {
                State_PutOnHand((itemConig) =>
                {
                    if (itemConig.Item_ID == 2000) return true;
                    return false;
                });
            }
            else
            {
                State_PutDownHand();
            }
        }
        if (onlyState_sentryStationList.Count <= 0)
        {
            OnlyState_FindSentryStation();
        }
        if (hour % 6 == 0)
        {
            OnlyState_TurnToNextSentryStation();
        }
        base.State_Listen_WorldGlobalTimeChange(hour, date, globalTime);
    }
    public override void State_Listen_ChangeAttackTarget(NetworkId id)
    {
        if (brainManager.allClient_actorManager_AttackTarget)
        {
            State_PutOnHand((itemConfig) =>
            {
                if (itemConfig.Item_Type == ItemType.Weapon) return true;
                return false;
            });
            pathManager.State_MoveTo(brainManager.allClient_actorManager_AttackTarget.pathManager.vector3Int_CurPos);
        }
        else
        {
            State_PutDownHand();
        }
        base.State_Listen_ChangeAttackTarget(id);
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
    }
    /// <summary>
    /// 我应该攻击吗
    /// </summary>
    /// <returns>攻击</returns>
    private bool OnlyState_CanIAttack(ActorManager actor)
    {
        if (actor.statusManager.statusType == StatusType.Monster_Common) return true;
        if (!brainManager.allClient_actorManager_AttackTarget && actor.actorNetManager.Local_Fine > 0)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// 查找岗哨
    /// </summary>
    private void OnlyState_FindSentryStation()
    {
        MessageBroker.Default.Publish(new GameEvent.GameEvent_Local_SomeoneFindTargetTile
        {
            actor = this,
            id = 1023,
            pos = transform.position,
            distance = 60,
        });
        onlyState_sentryStationLast = OnlyState_FindClosestSentryStation(onlyState_sentryStationList);
    }
    /// <summary>
    /// 查找最近岗哨
    /// </summary>
    /// <param name="vectors"></param>
    /// <returns></returns>
    private Vector2 OnlyState_FindClosestSentryStation(List<Vector2> vectors)
    {
        Vector2 temp = transform.position;
        Vector2 closestVector = vectors[0];
        float closestDistanceSquared = Vector2.Distance(temp, closestVector);

        foreach (var vector in vectors)
        {
            float distanceSquared = Vector2.Distance(temp, vector);

            if (distanceSquared < closestDistanceSquared)
            {
                closestVector = vector;
                closestDistanceSquared = distanceSquared;
            }
        }

        return closestVector;
    }
    /// <summary>
    /// 轮换岗哨
    /// </summary>
    private void OnlyState_TurnToNextSentryStation()
    {
        int index = onlyState_sentryStationList.IndexOf(onlyState_sentryStationLast) + 1;
        if (index >= onlyState_sentryStationList.Count)
        {
            index = 0;
        }
        onlyState_sentryStationLast = onlyState_sentryStationList[index];
        OnlyState_MoveToTargetSentryStation(onlyState_sentryStationLast);
    }
    /// <summary>
    /// 前往目的岗哨
    /// </summary>
    /// <param name="target"></param>
    private void OnlyState_MoveToTargetSentryStation(Vector2 target)
    {
        //MyTile tile = pathManager.GetBuildingTile();
        //if (tile)
        //{
        //    if (Vector2.Distance(target, tile._posInWorld) > 5)
        //    {
        //        pathManager.State_MoveTo(target);
        //    }
        //}
    }
    #endregion
}
