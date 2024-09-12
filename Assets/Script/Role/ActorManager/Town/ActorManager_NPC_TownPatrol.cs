using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// С������ҹѲ��
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
    #region//ҹѵ��ר�з���
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
    /// ��Ӧ�ù�����
    /// </summary>
    /// <returns>����</returns>
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
