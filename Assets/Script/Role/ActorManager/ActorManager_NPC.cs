using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UniRx;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using static ActorConfigData;
using static UnityEditor.Progress;
using static UnityEngine.InputManagerEntry;

public class ActorManager_NPC : ActorManager
{
    public ActorManager_NPC_Config config;
    protected float onlyState_attackCDTimer;
    protected GlobalTime globalTime;
    public override void InitByNetManager(bool hasStateAuthority, bool hasInputAuthority)
    {
        base.InitByNetManager(hasStateAuthority, hasInputAuthority);
        InitData();
    }
    private void InitData()
    {

    }
    public virtual void FixedUpdate()
    {

    }
    #region//监听
    public override void ListenRoleMove(ActorManager actor, MyTile where)
    {
        if (actor == this)
        {
            ListenRoleMove_Me(actor, where);
        }
        else
        {
            ListenRoleMove_Other(actor, where);
        }
        base.ListenRoleMove(actor, where);
    }
    public virtual void ListenRoleMove_Other(ActorManager actor, MyTile where)
    {

    }
    public virtual void ListenRoleMove_Me(ActorManager actor, MyTile where)
    {

    }
    public override void ListenWorldGlobalTimeChange(GlobalTime globalTime)
    {
        this.globalTime = globalTime;
        if (globalTime == GlobalTime.Morning)
        {
            ListenWorldGlobalTimeChange_Morning();
        }
        if (globalTime == GlobalTime.HighNoon)
        {
            ListenWorldGlobalTimeChange_HighNoon();
        }
        if (globalTime == GlobalTime.Dusk)
        {
            ListenWorldGlobalTimeChange_Dusk();
        }
        if (globalTime == GlobalTime.Evening)
        {
            ListenWorldGlobalTimeChange_Evening();
        }
        base.ListenWorldGlobalTimeChange(globalTime);
    }
    public virtual void ListenWorldGlobalTimeChange_Morning()
    {

    }
    public virtual void ListenWorldGlobalTimeChange_HighNoon()
    {

    }
    public virtual void ListenWorldGlobalTimeChange_Dusk()
    {

    }
    public virtual void ListenWorldGlobalTimeChange_Evening()
    {

    }
    public override void ListenRoleSendEmoji(ActorManager actor, int id)
    {
        if (actor != this && actor.isPlayer)
        {
            ListenRoleSendEmoji_Player(actor, id);
        }
        base.ListenRoleSendEmoji(actor, id);
    }
    public virtual void ListenRoleSendEmoji_Player(ActorManager actor, int id)
    {
        if (isState)
        {
            if (OnlyState_TryLookAt(actor))
            {
                OnlyState_TryUnderstand(actor, id);
            }
        }
    }
    public override void Listen_HpChange(int parameter, NetworkId id)
    {
        if (parameter < 0 && attackTarget == null)
        {
            StartCoroutine(SendEmoji(0.5f, 0));
        }
        base.Listen_HpChange(parameter, id);
    }

    #endregion
    #region//帧更新
    public virtual void AllLocal_Update_AttackingTarget(float dt)
    {
        if (attackState && attackTarget)
        {
            if (holdingByHand != null)
            {
                if (holdingByHand.PressRightClick(dt, isState, isInput, false))
                {
                    if (holdingByHand.PressLeftClick(dt, isState, isInput, false))
                    {
                        holdingByHand.ClickLeftClick(dt, isState, isInput, false);
                        if (isState)
                        {
                            holdingByHand.ReleaseLeftClick(dt, isState, isInput, false);
                            holdingByHand.ReleaseRightClick(dt, isState, isInput, false);
                            NetManager.RPC_State_Attack(false);
                        }
                        else
                        {
                            attackState = false;
                        }
                    }
                }
            }
        }
    }
    public virtual void AllLocal_Update_LookingTarget(float dt)
    {
        if (attackTarget)
        {
            FaceTo(attackTarget.transform.position - transform.position);
            if (holdingByHand != null)
            {
                holdingByHand.FaceTo(attackTarget.transform.position - transform.position, dt);
            }
        }
    }
    public virtual bool OnlyState_Update_AttackingTimer(float dt)
    {
        if (onlyState_attackCDTimer > 0)
        {
            onlyState_attackCDTimer -= dt;
            return false;
        }
        else
        {
            return true;
        }

    }
    public virtual bool OnlyState_Update_CheckingAttackingDistance(float dt)
    {
        if (Vector3.Distance(attackTarget.transform.position, transform.position) < holdingByHand.config.Attack_Distance + 0.5f)
        {
            NetManager.RPC_State_Attack(true);
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion
    #region//方法
    /// <summary>
    /// 更改目标
    /// </summary>
    /// <param name="id">目标的netID</param>
    public override void FromRPC_ChangeAttackTarget(NetworkId id)
    {
        base.FromRPC_ChangeAttackTarget(id);
    }
    /// <summary>
    /// 审视目标
    /// </summary>
    /// <param name="who"></param>
    /// <param name="handItemID"></param>
    /// <param name="headItemID"></param>
    /// <param name="bodyItemID"></param>
    public virtual void OnlyState_CheckOutTarget(ActorManager who, out int handItemID, out int headItemID, out int bodyItemID)
    {
        handItemID = who.NetManager.Data_ItemInHand.Item_ID;
        headItemID = who.NetManager.Data_ItemOnHead.Item_ID;
        bodyItemID = who.NetManager.Data_ItemOnBody.Item_ID;
    }
    /// <summary>
    /// 跟踪目标
    /// </summary>
    /// <param name="followType">跟踪方式</param>
    public virtual void OnlyState_Follow(FollowType followType)
    {
        if (attackTarget)
        {
            Vector2 moveTo = Vector2.one;
            if (followType == FollowType.RunAway)
            {

            }
            else if (followType == FollowType.RunTo)
            {

            }
            else if (followType == FollowType.Attack)
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
                    moveTo = GetMyTileWithOffset(new Vector3Int(toX, toY, 0))._posInWorld;
                }
                else
                {
                    moveTo = attackTarget.GetMyTile()._posInWorld;
                }

            }
            else
            {
                moveTo = GetMyTile()._posInWorld;
            }
            FindWayToTarget(moveTo);
        }
    }
    /// <summary>
    /// 拿出
    /// </summary>
    /// <param name="itemType">拿出物体类别</param>
    public virtual void OnlyState_PutOut(ItemType itemType)
    {
        if (holdingByHand != null && holdingByHand.config.Item_Type == itemType)
        {
            return;
        }
        else
        {
            for (int i = 0; i < NetManager.Data_ItemInBag.Count; i++)
            {
                if (ItemConfigData.GetItemConfig(NetManager.Data_ItemInBag[i].Item_ID).Item_Type == itemType)
                {
                    ItemData item = NetManager.Data_ItemInBag[i];
                    NetManager.RPC_LocalInput_AddItemOnHand(item);
                    NetManager.RPC_LocalInput_LoseItem(item);
                    return;
                }
            }
        }
    }
    /// <summary>
    /// 收起
    /// </summary>
    public virtual void OnlyState_PutDown()
    {
        if (holdingByHand != null)
        {
            NetManager.RPC_LocalInput_AddItemInBag(NetManager.Data_ItemInHand);
            NetManager.RPC_LocalInput_RemoveItemOnHand();
        }
    }
    /// <summary>
    /// 尝试看向
    /// </summary>
    /// <param name="who"></param>
    /// <returns></returns>
    public virtual bool OnlyState_TryLookAt(ActorManager who)
    {
        if (isState)
        {
            if (who != this)//这个人不是我
            {
                if (Vector3.Distance(transform.position, who.transform.position) >= config.Config_View)
                {
                    return false;
                }
                if (Physics2D.LinecastAll(who.transform.position, transform.position, tileLayer).Length > 0)
                {
                    return false;
                }
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// 尝试理解
    /// </summary>
    /// <param name="who"></param>
    /// <param name="id"></param>
    public virtual void OnlyState_TryUnderstand(ActorManager who, int id)
    {
    }

    /// <summary>
    /// 延迟发送emoji
    /// </summary>
    /// <param name="wait"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public IEnumerator SendEmoji(float wait,int id)
    {
        yield return new WaitForSeconds(wait);
        NetManager.RPC_LocalInput_SendEmoji(id);
    }
    #endregion
    public enum FollowType
    {
        RunAway,
        RunTo,
        Attack
    }
    //public virtual void Loot()
    //{
    //    for (int i = 0; i < actorConfig.Config_LootCount; i++)
    //    {
    //        ItemData item = GetLootItem();
    //        if (item.Item_ID != 0)
    //        {
    //            item.Item_Seed = System.DateTime.Now.Second + item.Item_Seed;
    //            MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnItem()
    //            {
    //                itemData = item,
    //                pos = transform.position - new Vector3(0, 0.1f, 0)
    //            });
    //        }
    //    }
    //    for (int i = 0; i < netManager.Data_ItemInBag.Count; i++)
    //    {
    //        ItemData item = netManager.Data_ItemInBag[i];
    //        MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnItem()
    //        {
    //            itemData = item,
    //            pos = transform.position - new Vector3(0, 0.1f, 0)
    //        });
    //    }
    //    if (netManager.Data_ItemInHand.Item_ID != 0)
    //    {
    //        MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnItem()
    //        {
    //            itemData = netManager.Data_ItemInHand,
    //            pos = transform.position - new Vector3(0, 0.1f, 0)
    //        });
    //    }
    //}
    //private ItemData GetLootItem()
    //{
    //    int weight_Main = 0;
    //    int weight_temp = 0;
    //    int random;
    //    for (int j = 0; j < actorConfig.Config_LootList.Count; j++)
    //    {
    //        weight_Main += actorConfig.Config_LootList[j].Weight;
    //    }
    //    random = UnityEngine.Random.Range(0, weight_Main);
    //    for (int j = 0; j < actorConfig.Config_LootList.Count; j++)
    //    {
    //        weight_temp += actorConfig.Config_LootList[j].Weight;
    //        if (weight_temp > random)
    //        {
    //            return actorConfig.Config_LootList[j].Item;
    //        }
    //    }
    //    return new ItemData();
    //}
    //private ItemData GetBagItem()
    //{
    //    int weight_Main = 0;
    //    int weight_temp = 0;
    //    int random;
    //    for (int j = 0; j < actorConfig.Config_BagList.Count; j++)
    //    {
    //        weight_Main += actorConfig.Config_BagList[j].Weight;
    //    }
    //    random = UnityEngine.Random.Range(0, weight_Main);
    //    for (int j = 0; j < actorConfig.Config_BagList.Count; j++)
    //    {
    //        weight_temp += actorConfig.Config_BagList[j].Weight;
    //        if (weight_temp > random)
    //        {
    //            return actorConfig.Config_BagList[j].Item;
    //        }
    //    }
    //    return new ItemData();
    //}
}
public struct ActorConfig
{
}
[Serializable]
public struct ActorManager_NPC_Config
{
    [Header("生命")]
    public float Config_Hp;
    [Header("最大生命")]
    public float Config_MaxHp;
    [Header("速度")]
    public float Config_Speed;
    [Header("最大速度")]
    public float Config_MaxSpeed;
    [Header("体力")]
    public float Config_Endurance;
    [Header("掉落数量")]
    public int Config_LootCount;
    [Header("掉落列表")]
    public List<LootInfo> Config_LootList;
    [Header("背包数量")]
    public int Config_BagCount;
    [Header("背包列表")]
    public List<LootInfo> Config_BagList;
    [Header("攻击间隔")]
    public float Config_AttackCD;
    [Header("视野")]
    public float Config_View;
    [Header("勇气")]
    public float Config_Courage;
}