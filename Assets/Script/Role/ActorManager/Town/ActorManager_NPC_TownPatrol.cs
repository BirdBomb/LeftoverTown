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
    private List<UnityEngine.Vector2> sentryStationList = new List<UnityEngine.Vector2>();
    private UnityEngine.Vector2 sentryStationLast = UnityEngine.Vector2.zero;
    public override void FixedUpdate()
    {
        if (attackTarget)
        {
            float dt = Time.fixedDeltaTime;
            AllLocal_Update_LookingTarget(dt);
            AllLocal_Update_AttackingTarget(dt);
            if (isState)
            {
                if (OnlyState_Update_CheckingAttackingTimer(dt))
                {
                    if (OnlyState_Update_CheckingAttackingDistance(dt))
                    {
                        if (attackTarget.actorState != ActorState.Dead)
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
        }
        base.FixedUpdate();
    }
    #region//夜训队专有方法
    public override void Listen_OtherMove(ActorManager actor, MyTile where)
    {
        if (isState)
        {
            if (OnlyState_TryLookAt(actor))
            {
                if (!attackTarget)
                {
                    FaceTo(actor.transform.position - transform.position);
                    if (CanIAttack(actor))
                    {
                        TryToSendEmoji(0.1f, 9);
                        NetManager.RPC_State_NpcChangeAttackTarget(actor.NetManager.Object.Id);
                    }
                }
                else
                {
                    OnlyState_Follow(FollowType.Attack);
                }
            }
            else
            {
                if (attackTarget == actor)
                {
                    TryToSendEmoji(0.1f, 1);
                    NetManager.RPC_State_NpcChangeAttackTarget(new NetworkId());
                }
            }
        }
        base.Listen_OtherMove(actor, where);
    }
    public override void Listen_HpChange(int parameter, NetworkId id)
    {
        ActorManager who = NetManager.Runner.FindObject(id).GetComponent<ActorManager>();
        if (who.isPlayer && OnlyState_TryLookAt(who))
        {
            who.SetFine(500);
            TryToSendEmoji(0.1f, 16);
        }
        base.Listen_HpChange(parameter, id);
    }
    public override void ListenRoleCommit(ActorManager who, short val)
    {
        if (OnlyState_TryLookAt(who))
        {
            who.SetFine(val);
        }
        TryToSendEmoji(0.2f, 0);
        base.ListenRoleCommit(who, val);
    }
    public override void ListenMyself_InDestinationTile()
    {
        if (!attackTarget)
        {
            TryToSendEmoji(0.1f, 1);
            if (sentryStationLast != Vector2.zero)
            {
                MoveToTargetSentryStation(sentryStationLast);
            }
        }
        base.ListenMyself_InDestinationTile();
    }
    public override void Listen_WorldGlobalTimeChange(int hour, int date, GlobalTime globalTime)
    {
        if (!attackTarget)
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
        if (sentryStationList.Count <= 0)
        {
            FindSentryStation();
        }
        if (hour % 6 == 0)
        {
            TurnToNextSentryStation();
        }
        base.Listen_WorldGlobalTimeChange(hour, date, globalTime);
    }
    public override void FromRPC_ChangeAttackTarget(NetworkId id)
    {
        base.FromRPC_ChangeAttackTarget(id);
        if (isState)
        {
            if (attackTarget)
            {
                OnlyState_PutOut(ItemType.Weapon);
                FindWayToTarget(attackTarget.GetMyTile()._posInWorld);
            }
            else
            {
                OnlyState_PutDown();
            }
        }
    }
    public override void OnlyState_TryUnderstand(ActorManager who, int id, bool look, bool hear)
    {
        int emoji = -1;
        switch (id)
        {
            case 16:
                emoji = 0;
                FindWayToTarget(who.GetMyTile()._posInWorld);
                break;
        }
        TryToSendEmoji(0.5f, emoji);
        base.OnlyState_TryUnderstand(who, id, look, hear);
    }
    /// <summary>
    /// 我应该攻击吗
    /// </summary>
    /// <returns>攻击</returns>
    private bool CanIAttack(ActorManager actor)
    {
        if (actor.NetManager.Data_Fine > 0)
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
    private void FindSentryStation()
    {
        MessageBroker.Default.Publish(new GameEvent.GameEvent_Local_SomeoneFindTargetTile
        {
            actor = this,
            id = 1023,
            pos = transform.position,
            distance = 60,
            action = AddSentryStation
        });
        sentryStationLast = FindClosestSentryStation(sentryStationList);
    }
    /// <summary>
    /// 查找最近岗哨
    /// </summary>
    /// <param name="vectors"></param>
    /// <returns></returns>
    private Vector2 FindClosestSentryStation(List<Vector2> vectors)
    {
        Vector2 temp = GetMyTile()._posInWorld;
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
    private void AddSentryStation(TileObj tile)
    {
        sentryStationList.Add(tile.bindTile._posInWorld);
    }
    /// <summary>
    /// 轮换岗哨
    /// </summary>
    private void TurnToNextSentryStation()
    {
        int index = sentryStationList.IndexOf(sentryStationLast) + 1;
        if (index >= sentryStationList.Count)
        {
            index = 0;
        }
        sentryStationLast = sentryStationList[index];
        MoveToTargetSentryStation(sentryStationLast);
    }
    /// <summary>
    /// 前往目的岗哨
    /// </summary>
    /// <param name="target"></param>
    private void MoveToTargetSentryStation(Vector2 target)
    {
        MyTile tile = GetMyTile();
        if (tile)
        {
            if (Vector2.Distance(target, tile._posInWorld) > 5)
            {
                FindWayToTarget(target);
            }
        }
    }
    #endregion
}
