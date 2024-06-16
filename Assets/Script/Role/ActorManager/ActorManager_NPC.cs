using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class ActorManager_NPC : ActorManager
{
    protected float view = 20;
    protected float courage = 2;
    protected float attackCD = 2;
    private float onlyState_attackCDTimer;
    
    public void FixedUpdate()
    {
        if (isState)
        {
            if (attackTarget)
            {
                if (onlyState_attackCDTimer > 0)
                {
                    onlyState_attackCDTimer -= Time.fixedDeltaTime;
                }
                else
                {
                    OnlyState_CheckingAttackDistance();
                }
            }
        }
        AllLocal_Looking();
        AllLocal_Attacking();
    }

    public virtual void InitNPC(ItemData head,ItemData body,ItemData hand,List<ItemData> bag)
    {

    }
    public override void ListenRoleMove(ActorManager who, MyTile where)
    {
        if (isState)
        {
            OnlyState_LookAtSomeone(who);
            OnlyState_CheckOutTarget();
        }
        base.ListenRoleMove(who, where);
    }
    public override void ListenWorldGlobalTimeChange(GlobalTime globalTime)
    {
        if (isState)
        {
            if (globalTime == GlobalTime.Morning)
            {

            }
            if (globalTime == GlobalTime.Evening)
            {

            }
        }
        base.ListenWorldGlobalTimeChange(globalTime);
    }
    /// <summary>
    /// 看向某人
    /// </summary>
    /// <param name="who"></param>
    public virtual void OnlyState_LookAtSomeone(ActorManager who)
    {
        if (isState)
        {
            if (who != this )//这个人不是我
            {
                if (Vector3.Distance(transform.position, who.transform.position) >= view)
                {
                    Debug.Log("这个人在我视野外");
                    if(who == attackTarget)
                    {
                        NetManager.RPC_State_ChangeAttackTarget(new NetworkId());
                    }
                    return;
                }
                if (Physics2D.LinecastAll(who.transform.position, transform.position, tileLayer).Length > 0)
                {
                    Debug.Log("这个人有障碍物阻挡");
                    if (who == attackTarget) 
                    {
                        NetManager.RPC_State_ChangeAttackTarget(new NetworkId());
                    }
                    return;
                }
                OnlyState_CheckOutSomeone(who);
            }
        }
    }
    /// <summary>
    /// 审视某人
    /// </summary>
    /// <param name="who"></param>
    public virtual void OnlyState_CheckOutSomeone(ActorManager who)
    {
        int id = who.NetManager.Data_ItemInHand.Item_ID;
        if (ItemConfigData.GetItemConfig(id).Item_Type == ItemType.Weapon)
        {
            /*这个人拿着武器!*/
            NetManager.RPC_State_ChangeAttackTarget(who.NetManager.Object.Id);
        }

    }
    /// <summary>
    /// 审视目标
    /// </summary>
    public virtual void OnlyState_CheckOutTarget()
    {
        if (attackTarget)
        {
            float targetDistance = attackTarget == null ? 0 : Vector3.Distance(attackTarget.transform.position, transform.position);
            float attackDistance = holdingByHand == null ? 0 : holdingByHand.config.Attack_Distance;
            if (targetDistance < attackDistance && attackDistance > 2)
            {
                Vector3 attackTargetDir = attackTarget.transform.position - transform.position;
                int toX = 0;
                int toY = 0;
                if (attackTargetDir.x > 0.5)
                {
                    toX = -1;
                }
                else if (attackTargetDir.x < -0.5)
                {
                    toX = 1;
                }
                if (attackTargetDir.y > 0.5)
                {
                    toY = -1;
                }
                else if (attackTargetDir.y < -0.5)
                {
                    toY = 1;
                }
                FindWayToTarget(GetMyTileWithOffset(new Vector3Int(toX, toY, 0)).posInWorld);
            }
            else
            {
                FindWayToTarget(attackTarget.GetMyTile().posInWorld);
            }
        }
    }
    public override void FromRPC_ChangeAttackTarget(NetworkId id)
    {
        base.FromRPC_ChangeAttackTarget(id);
        if (attackTarget)
        {
            if (holdingByHand != null && holdingByHand.config.Item_Type == ItemType.Weapon)
            {
                return;
            }
            else
            {
                for (int i = 0; i < NetManager.Data_ItemInBag.Count; i++)
                {
                    if (ItemConfigData.GetItemConfig(NetManager.Data_ItemInBag[i].Item_ID).Item_Type == ItemType.Weapon)
                    {
                        ItemData item = NetManager.Data_ItemInBag[i];
                        NetManager.RPC_LocalInput_AddItemOnHand(item);
                        NetManager.RPC_LocalInput_LoseItem(item);
                        return;
                    }
                }
            }
        }
        else
        {
            if (NetManager.Data_ItemInHand.Item_ID != 0)
            {
                NetManager.RPC_LocalInput_AddItemInBag(NetManager.Data_ItemInHand);
                NetManager.RPC_LocalInput_RemoveItemOnHand();
            }
        }
    }
    public virtual void OnlyState_CheckingAttackDistance()
    {
        if (Vector3.Distance(attackTarget.transform.position, transform.position) < holdingByHand.config.Attack_Distance + 0.5f)
        {
            onlyState_attackCDTimer = attackCD;
            NetManager.RPC_State_Attack(true);
        }
    }
    public virtual void AllLocal_Attacking()
    {
        if (attackState)
        {
            if (holdingByHand != null)
            {
                if (holdingByHand.PressRightClick(Time.fixedDeltaTime, isState, isInput, false))
                {
                    if (holdingByHand.PressLeftClick(Time.fixedDeltaTime, isState, isInput, false))
                    {
                        holdingByHand.ClickLeftClick(Time.fixedDeltaTime, isState, isInput, false);
                        if (isState)
                        {
                            holdingByHand.ReleaseLeftClick(Time.fixedDeltaTime, isState, isInput, false);
                            holdingByHand.ReleaseRightClick(Time.fixedDeltaTime, isState, isInput, false);
                            NetManager.RPC_State_Attack(false);
                        }
                    }
                }
            }
        }
    }
    public virtual void AllLocal_Looking()
    {
        if (attackTarget != null)
        {
            FaceTo(attackTarget.transform.position - transform.position);
            if (holdingByHand != null)
            {
                holdingByHand.FaceTo(attackTarget.transform.position - transform.position, Time.fixedDeltaTime);
            }
        }
    }

}
