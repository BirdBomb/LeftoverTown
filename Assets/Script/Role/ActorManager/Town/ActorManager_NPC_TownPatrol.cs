using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 小镇势力夜巡队
/// </summary>
public class ActorManager_NPC_TownPatrol : ActorManager_NPC
{
    public override void FixedUpdate()
    {
        if (attackTarget)
        {
            float dt = Time.deltaTime;
            AllLocal_Update_LookingTarget(dt);
            AllLocal_Update_AttackingTarget(dt);
            if (isState)
            {
                if (OnlyState_Update_CheckingAttackingTimer(dt))
                {
                    if (OnlyState_Update_CheckingAttackingDistance(dt))
                    {
                        NetManager.RPC_State_NpcChangeAttackState(true);
                    }
                }
            }
        }
        base.FixedUpdate();
    }
    #region//夜训队专有方法
    public override void ListenRoleMove_Other(ActorManager actor, MyTile where)
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
        base.ListenRoleMove_Other(actor, where);
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

    #endregion
}
