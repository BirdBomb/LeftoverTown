using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UniRx;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
/// <summary>
/// NPC基类脚本
/// </summary>
public class ActorManager_NPC : ActorManager
{
    [Header("NPC配置")]
    public ActorConfig_NPC config;
    #region//初始化NPC
    public override void AllClient_Init()
    {
        AllClient_InitNPCData();
        base.AllClient_Init();
    }
    public override void State_Init()
    {
        actorNetManager.Object.AssignInputAuthority(actorNetManager.Object.StateAuthority);
        State_InitNPCData();
        base.State_Init();
    }
    /// <summary>
    /// 初始化NPC数据(客户端)
    /// </summary>
    public virtual void AllClient_InitNPCData()
    {
        statusManager.statusType = config.status_Type;
    }
    /// <summary>
    /// 初始化NPC数据(服务器)
    /// </summary>
    public virtual void State_InitNPCData()
    {
        UnityEngine.Random.InitState((int)(transform.position.x + transform.position.y));
        actorNetManager.Net_EyeID = EyeConfigData.eyeConfigs[UnityEngine.Random.Range(0, EyeConfigData.eyeConfigs.Count)].Eye_ID;
        actorNetManager.Net_HairID = HairConfigData.hairConfigs[UnityEngine.Random.Range(0, HairConfigData.hairConfigs.Count)].Hair_ID;
        actorNetManager.Net_HairColor = UnityEngine.Random.ColorHSV();

        actorNetManager.Net_HpCur = config.short_Hp;
        actorNetManager.Net_HpMax = config.short_Hp;
        actorNetManager.Net_Armor = config.short_Armor;
        actorNetManager.Net_SpeedCommon = config.short_Speed;
        actorNetManager.Net_SpeedMax = config.short_Speed;
        actorNetManager.Net_EnCur = config.short_Endurance;
        actorNetManager.Net_EnMax = config.short_Endurance;

        actorNetManager.Net_ItemOnHead = itemManager.CreateItemData(config.short_HatID);
        actorNetManager.Net_ItemOnBody = itemManager.CreateItemData(config.short_ClothesID);
        for (int i = 0; i < config.lootInfos_Base.Count; i++)
        {
            int count = new System.Random().Next(config.lootInfos_Base[i].CountMin, config.lootInfos_Base[i].CountMax + 1);
            ItemData item = itemManager.CreateItemData(config.lootInfos_Base[i].ID, (short)count);
            actorNetManager.Net_ItemsInBag.Add(item);
        }
        actorNetManager.Net_ItemsInBagCapacity = (short)actorNetManager.Net_ItemsInBag.Capacity;
    }
    #endregion
    
    #region//方法
    /// <summary>
    /// 跟踪目标(主机)
    /// </summary>
    /// <param name="followType">跟踪方式</param>
    public virtual void State_Follow(FollowType followType)
    {
        //if (brainManager.allClient_actorManager_AttackTarget)
        //{
        //    Vector2 moveTo;
        //    if (followType == FollowType.RunAway)
        //    {
        //        Vector3 attackTargetDir = brainManager.allClient_actorManager_AttackTarget.transform.position - transform.position;
        //        int toX = 0;
        //        int toY = 0;
        //        if (attackTargetDir.x > 0.5)
        //        {
        //            toX = -1;
        //        }
        //        else if (attackTargetDir.x < -0.5)
        //        {
        //            toX = 1;
        //        }
        //        if (attackTargetDir.y > 0.5)
        //        {
        //            toY = -1;
        //        }
        //        else if (attackTargetDir.y < -0.5)
        //        {
        //            toY = 1;
        //        }
        //        moveTo = pathManager.GetBuildingTile()._posInWorld + new Vector2(toX, toY);
        //    }
        //    else if (followType == FollowType.RunTo)
        //    {
        //        moveTo = brainManager.allClient_actorManager_AttackTarget.pathManager.GetBuildingTile()._posInWorld;
        //    }
        //    else if (followType == FollowType.Attack)
        //    {
        //        float targetDistance = brainManager.allClient_actorManager_AttackTarget == null ? 0 : Vector3.Distance(brainManager.allClient_actorManager_AttackTarget.transform.position, transform.position);
        //        float attackDistance = itemManager.itemBase_OnHand.itemData.Item_ID == 0 ? 1 : itemManager.itemBase_OnHand.itemConfig.Attack_Distance;
        //        if (targetDistance < attackDistance && attackDistance > 2)
        //        {
        //            /*目标在自己的攻击范围内,远离目标*/
        //            Vector3 attackTargetDir = brainManager.allClient_actorManager_AttackTarget.transform.position - transform.position;
        //            int toX = 0;
        //            int toY = 0;
        //            if (attackTargetDir.x > 0.5)
        //            {
        //                toX = -1;
        //            }
        //            else if (attackTargetDir.x < -0.5)
        //            {
        //                toX = 1;
        //            }
        //            if (attackTargetDir.y > 0.5)
        //            {
        //                toY = -1;
        //            }
        //            else if (attackTargetDir.y < -0.5)
        //            {
        //                toY = 1;
        //            }
        //            moveTo = pathManager.GetBuildingTile()._posInWorld + new Vector2(toX, toY);
        //        }
        //        else
        //        {
        //            /*目标不在自己的攻击范围内,走向目标*/
        //            moveTo = brainManager.allClient_actorManager_AttackTarget.pathManager.GetBuildingTile()._posInWorld;
        //        }
        //    }
        //    else
        //    {
        //        moveTo = brainManager.allClient_actorManager_AttackTarget.pathManager.GetBuildingTile()._posInWorld;
        //    }
        //    pathManager.State_MoveTo(moveTo);
        //}
    }
    /// <summary>
    /// 查找物体(主机)
    /// </summary>
    /// <returns></returns>
    public virtual ItemData State_FindItemInBag(Func<ItemConfig, bool> function)
    {
        ItemData item = new ItemData(0);
        for (int i = 0; i < actorNetManager.Net_ItemsInBag.Count; i++)
        {
            if (function.Invoke(ItemConfigData.GetItemConfig(actorNetManager.Net_ItemsInBag[i].Item_ID)))
            {
                item = actorNetManager.Net_ItemsInBag[i];
            }
        }
        return item;
    }
    /// <summary>
    /// 拿出物体(主机)
    /// </summary>
    /// <param name="function"></param>
    public virtual void State_PutOnHand(Func<ItemConfig, bool> function)
    {
        if(itemManager.itemBase_OnHand != null && function.Invoke(itemManager.itemBase_OnHand.itemConfig))
        {
            return;
        }
        else
        {
            for (int i = 0; i < actorNetManager.Net_ItemsInBag.Count; i++)
            {
                if (function.Invoke(ItemConfigData.GetItemConfig(actorNetManager.Net_ItemsInBag[i].Item_ID)))
                {
                    ItemData item = actorNetManager.Net_ItemsInBag[i];
                    actorNetManager.OnlyState_AddItemOnHand(item);
                    actorNetManager.OnlyState_SubItemInBag(item);
                    return;
                }
            }
        }
    }
    /// <summary>
    /// 收起物体(主机)
    /// </summary>
    public virtual void State_PutDownHand()
    {
        ItemData item = actorNetManager.Net_ItemInHand;
        actorNetManager.OnlyState_SubItemOnHand(item);
        actorNetManager.OnlyState_AddItemInBag(item);
    }
    /// <summary>
    /// 发送消息(服务器)
    /// </summary>
    /// <param name="text"></param>
    public void State_TryToSendText(string text,Emoji emoji)
    {
        if (actorAuthority.isLocal)
        {
            actorNetManager.RPC_LocalInput_SendText(text, (int)emoji);
        }
    }
    /// <summary>
    /// 发送Emoji(服务器)
    /// </summary>
    /// <param name="wait"></param>
    /// <param name="id"></param>
    public void State_TryToSendEmoji(float wait, Emoji emoji)
    {
        if (actorAuthority.isState)
        {
            StartCoroutine(State_SendEmoji(wait, emoji));
        }
    }
    /// <summary>
    /// 发送Emoji(本地)
    /// </summary>
    /// <param name="wait"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    private IEnumerator State_SendEmoji(float wait, Emoji emoji)
    {
        yield return new WaitForSeconds(wait);
        actorNetManager.RPC_LocalInput_SendEmoji((int)emoji);
    }
    #endregion


}
[Serializable]
public struct ActorConfig_NPC
{
    [Header("初始身份")]
    public StatusType status_Type;
    [Header("初始生命")]
    public short short_Hp;
    [Header("初始护甲")]
    public short short_Armor;
    [Header("初始视野")]
    public short short_View;
    [Header("初始速度(分米每秒)")]
    public short short_Speed;
    [Header("初始体力")]
    public int short_Endurance;
    [Header("帽子ID")]
    public short short_HatID;
    [Header("衣服ID")]
    public short short_ClothesID;
    [Header("基本掉落列表")]
    public List<BaseLootInfo> lootInfos_Base; 
    [Header("额外掉落列表")]
    public List<ExtraLootInfo> lootInfos_Extra;
}
/// <summary>
/// 跟踪方式
/// </summary>
public enum FollowType
{
    /// <summary>
    /// 逃离
    /// </summary>
    RunAway,
    /// <summary>
    /// 靠近
    /// </summary>
    RunTo,
    /// <summary>
    /// 攻击
    /// </summary>
    Attack
}
