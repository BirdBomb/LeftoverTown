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
                if (OnlyState_Update_AttackingTimer(Time.fixedDeltaTime))
                {
                    if (OnlyState_Update_CheckingAttackingDistance(Time.fixedDeltaTime))
                    {
                        NetManager.RPC_State_Attack(true);
                        onlyState_attackCDTimer = config.Config_AttackCD;
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
    #region//监听
    public override void ListenWorldGlobalTimeChange_HighNoon()
    {
        base.ListenWorldGlobalTimeChange_HighNoon();
    }
    public override void ListenWorldGlobalTimeChange_Dusk()
    {
        if (onlyState_myBed)
        {
            FindWayToTarget(onlyState_myBed._posInWorld);
        }
        base.ListenWorldGlobalTimeChange_Dusk();
    }
    public override void ListenRoleMove_Me(ActorManager actor, MyTile where)
    {
        if (isState)
        {

        }
        base.ListenRoleMove_Me(actor, where);
    }
    public override void ListenRoleMove_Other(ActorManager actor, MyTile where)
    {
        if (isState)
        {
            if (OnlyState_TryLookAt(actor))
            {
                /*我能看见他*/
                OnlyState_CheckOutTarget(actor, out int handItemID, out int headItemID, out int bodyItemID);
                if (attackTarget)
                {
                    /*我已经有了目标*/
                    if (actor != attackTarget)
                    {
                        /*这个人不是我的攻击目标*/
                        if (ItemConfigData.GetItemConfig(handItemID).Item_Type == ItemType.Weapon)
                        {
                            if (Vector3.Distance(transform.position, actor.transform.position) < Vector3.Distance(transform.position, attackTarget.transform.position) - 1)
                            {
                                NetManager.RPC_LocalInput_SendEmoji(9);
                                NetManager.RPC_State_ChangeAttackTarget(actor.NetManager.Object.Id);
                            }
                        }
                    }
                    else
                    {
                        OnlyState_Follow(FollowType.Attack);
                    }
                }
                else
                {
                    /*我还没有目标*/
                    FaceTo(actor.transform.position - transform.position);
                    if (ItemConfigData.GetItemConfig(handItemID).Item_Type == ItemType.Weapon)
                    {
                        NetManager.RPC_LocalInput_SendEmoji(9);
                        NetManager.RPC_State_ChangeAttackTarget(actor.NetManager.Object.Id);
                    }
                }
            }
            else
            {
                if (actor == attackTarget)
                {
                    NetManager.RPC_LocalInput_SendEmoji(1);
                    NetManager.RPC_State_ChangeAttackTarget(new NetworkId());
                }
            }
        }
        base.ListenRoleMove_Other(actor, where);
    }
    #endregion
    #region//条件更新
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
    public override void OnlyState_TryUnderstand(ActorManager who, int id)
    {
        if (id == 2 || id == 3)
        {
            StartCoroutine(SendEmoji(0.5f, 1));
        }
        else if (id == 4)
        {
            StartCoroutine(SendEmoji(0.5f, 4));
            if (attackTarget == who)
            {
                NetManager.RPC_State_ChangeAttackTarget(new NetworkId());
            }
        }
        else if (id == 5)
        {
            StartCoroutine(SendEmoji(0.5f, 9));
            NetManager.RPC_State_ChangeAttackTarget(who.NetManager.Object.Id);
        }
        else if (id == 12)
        {
            StartCoroutine(SendEmoji(0.5f, 3));
        }
        else if (id == 16)
        {
            StartCoroutine(SendEmoji(0.5f, 0));
        }
    }
    #endregion

}
