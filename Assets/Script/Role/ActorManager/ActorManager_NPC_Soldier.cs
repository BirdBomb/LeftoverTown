using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class ActorManager_NPC_Soldier : ActorManager_NPC
{
    public override void FixedUpdate()
    {
        if (isState)
        {
            if (attackTarget)
            {
                if (OnlyState_Update_CheckingAttackingTimer(Time.fixedDeltaTime))
                {
                    if (OnlyState_Update_CheckingAttackingDistance(Time.fixedDeltaTime))
                    {
                        NetManager.RPC_State_NpcChangeAttackState(true);
                    }
                }
            }
        }
        if (attackTarget)
        {
            AllLocal_Update_LookingTarget(Time.fixedDeltaTime);
            AllLocal_Update_AttackingTarget(Time.fixedDeltaTime);
        }
        base.FixedUpdate();
    }
    #region//����
    public override void ListenRoleMove_Other(ActorManager actor, MyTile where)
    {
        if (isState)
        {
            if (OnlyState_TryLookAt(actor))
            {
                /*���ܿ�����*/
                CheckOutSomeone(actor, out short handItemID, out short headItemID, out short bodyItemID, out short fine);
                if (attackTarget)
                {
                    /*���Ѿ�����Ŀ��*/
                    if (actor != attackTarget)
                    {
                        /*����˲����ҵĹ���Ŀ��*/
                        if (CanIAttack(actor, handItemID, headItemID, bodyItemID, fine))
                        {
                            if (Vector3.Distance(transform.position, actor.transform.position) < Vector3.Distance(transform.position, attackTarget.transform.position) - 1)
                            {
                                NetManager.RPC_LocalInput_SendEmoji(9);
                                NetManager.RPC_State_NpcChangeAttackTarget(actor.NetManager.Object.Id);
                            }
                        }
                    }
                    else
                    {
                        /*��������ҵĹ���Ŀ��*/
                        OnlyState_Follow(FollowType.Attack);
                    }
                }
                else
                {
                    /*�һ�û��Ŀ��*/
                    FaceTo(actor.transform.position - transform.position);
                    if (CanIAttack(actor, handItemID, headItemID, bodyItemID, fine))
                    {
                        NetManager.RPC_LocalInput_SendEmoji(9);
                        NetManager.RPC_State_NpcChangeAttackTarget(actor.NetManager.Object.Id);
                    }
                }
            }
            else
            {
                if (actor == attackTarget)
                {
                    NetManager.RPC_LocalInput_SendEmoji(1);
                    NetManager.RPC_State_NpcChangeAttackTarget(new NetworkId());
                }
            }
        }
        base.ListenRoleMove_Other(actor, where);
    }
    #endregion
    #region//���෽��
    public override void FromRPC_ChangeAttackTarget(NetworkId id)
    {
        base.FromRPC_ChangeAttackTarget(id);
        if (attackTarget)
        {
            OnlyState_PutOut(ItemType.Weapon);
        }
        else
        {

            OnlyState_PutDown();
        }
    }
    public override void OnlyState_TryUnderstand(ActorManager who, int id, bool look, bool hear)
    {
        int emoji = -1;
        if (id == 2 || id == 3)
        {
            emoji = 1;
        }
        else if (id == 4)
        {
            emoji = 4;
            if (attackTarget == who)
            {
                NetManager.RPC_State_NpcChangeAttackTarget(new NetworkId());
            }
        }
        else if (id == 5)
        {
            emoji = 9;
            NetManager.RPC_State_NpcChangeAttackTarget(who.NetManager.Object.Id);
        }
        else if (id == 12)
        {
            emoji = 3;
        }
        else if (id == 16)
        {
            emoji = 0;
            FindWayToTarget(who.GetMyTile()._posInWorld);
        }
        TryToSendEmoji(0.5f, emoji);
    }
    #endregion
    #region//ʿ��ר�з���
    /// <summary>
    /// ��Ӧ�ù�����
    /// </summary>
    /// <returns>����</returns>
    private bool CanIAttack(ActorManager actor, short inHand, short onHead, short onBody,short fine)
    {
        if (actor.NetManager.Data_Fine > 0)
        {
            CheckStatus(onBody, onHead, fine, out short id);
            if (id != 1004)
            {
                return true;
            }
        }
        return false;
    }

    #endregion
}
