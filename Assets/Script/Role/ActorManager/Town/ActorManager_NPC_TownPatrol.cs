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
    public override void OnlyState_FixedUpdateNetwork(float dt)
    {
        if (allClient_AttackTarget)
        {
            if (OnlyState_Update_CheckingAttackingTimer(dt))
            {
                if (OnlyState_Update_CheckingAttackingDistance(dt))
                {
                    if (allClient_AttackTarget.actorState != ActorState.Dead)
                    {
                        NetManager.RPC_State_NpcChangeAttackState(true);
                    }
                    else
                    {
                        NetManager.RPC_State_NpcChangeAttackTarget(new NetworkId());
                    }
                }
            }
        }

        base.OnlyState_FixedUpdateNetwork(dt);
    }
    #region//夜训队专有方法
    public override void OnlyStateListen_MoveOther(ActorManager actor, MyTile where)
    {
        if (Tool_TryLookAt(actor))
        {
            /*我看的见*/
            if (allClient_AttackTarget == actor)
            {
                OnlyState_Follow(FollowType.Attack);
            }
            else
            {
                if (OnlyState_CanIAttack(actor))
                {
                    AllClient_TryToSendEmoji(0.1f, 9);
                    NetManager.RPC_State_NpcChangeAttackTarget(actor.NetManager.Object.Id);
                }
            }
        }
        else
        {
            /*我看不见*/
            if (allClient_AttackTarget == actor)
            {
                /*丢失攻击目标*/
                OnlyState_Follow(FollowType.RunTo);
                AllClient_TryToSendEmoji(0.1f, 1);
                NetManager.RPC_State_NpcChangeAttackTarget(new NetworkId());
            }
        }
        base.OnlyStateListen_MoveOther(actor, where);
    }
    public override void AllClientListen_MyselfHpChange(int parameter, NetworkId id)
    {
        ActorManager who = NetManager.Runner.FindObject(id).GetComponent<ActorManager>();
        if (who.isPlayer && Tool_TryLookAt(who))
        {
            who.AllClient_SetFine(500);
            AllClient_TryToSendEmoji(0.1f, 16);
        }
        base.AllClientListen_MyselfHpChange(parameter, id);
    }
    public override void OnlyStateListen_RoleCommit(ActorManager who, short val)
    {
        if (Tool_TryLookAt(who))
        {
            who.AllClient_SetFine(val);
        }
        AllClient_TryToSendEmoji(0.2f, 0);
        base.OnlyStateListen_RoleCommit(who, val);
    }
    public override void OnlyStateListen_WorldGlobalTimeChange(int hour, int date, GlobalTime globalTime)
    {
        if (!allClient_AttackTarget)
        {
            if (globalTime == GlobalTime.Dusk || globalTime == GlobalTime.Evening)
            {
                OnlyState_PutOut(2004);
            }
            else
            {
                OnlyState_PutDown();
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
        base.OnlyStateListen_WorldGlobalTimeChange(hour, date, globalTime);
    }
    public override void OnlyStateListen_ChangeAttackTarget(NetworkId id)
    {
        if (allClient_AttackTarget)
        {
            OnlyState_PutOut(ItemType.Weapon);
            OnlyState_FindWayToTarget(allClient_AttackTarget.Tool_GetMyTileWithOffset(Vector3Int.zero)._posInWorld);
        }
        else
        {
            OnlyState_PutDown();
        }
        base.OnlyStateListen_ChangeAttackTarget(id);
    }
    public override void OnlyState_TryUnderstand(ActorManager who, int id, bool look, bool hear)
    {
        int emoji = -1;
        switch (id)
        {
            case 16:
                emoji = 0;
                OnlyState_FindWayToTarget(who.Tool_GetMyTileWithOffset(Vector3Int.zero)._posInWorld);
                break;
        }
        AllClient_TryToSendEmoji(0.5f, emoji);
        base.OnlyState_TryUnderstand(who, id, look, hear);
    }
    /// <summary>
    /// 攻击循环
    /// </summary>
    public void AllClient_AttackLoop(float dt)
    {
        if (allClient_AttackTarget)
        {
            AllClient_Simulate_InputMousePos(allClient_AttackTarget.transform.position);
            if (allClient_AttackState)
            {
                allClient_AttackState = !AllClient_Simulate_InputMousePress(dt, MouseInputType.PressRightThenPressLeft);
            }
        }
    }
    /// <summary>
    /// 我应该攻击吗
    /// </summary>
    /// <returns>攻击</returns>
    private bool OnlyState_CanIAttack(ActorManager actor)
    {
        if (!allClient_AttackTarget && actor.NetManager.Data_Fine > 0)
        {
            if (actor.NetManager.LocalData_Status != 1004)
            {
                return true;
            }
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
            action = OnlyState_AddSentryStation
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
        Vector2 temp = Tool_GetMyTileWithOffset(Vector3Int.zero)._posInWorld;
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
    /// 添加岗哨
    /// </summary>
    /// <param name="tile"></param>
    private void OnlyState_AddSentryStation(TileObj tile)
    {
        onlyState_sentryStationList.Add(tile.bindTile._posInWorld);
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
        MyTile tile = Tool_GetMyTileWithOffset(Vector3Int.zero);
        if (tile)
        {
            if (Vector2.Distance(target, tile._posInWorld) > 5)
            {
                OnlyState_FindWayToTarget(target);
            }
        }
    }
    #endregion
}
