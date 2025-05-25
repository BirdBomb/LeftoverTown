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
/// NPC����ű�
/// </summary>
public class ActorManager_NPC : ActorManager
{
    [Header("NPC����")]
    public ActorConfig_NPC config;
    #region//��ʼ��NPC
    public override void Client_Init()
    {
        AllClient_InitNPCData();
        base.Client_Init();
    }
    public override void State_Init()
    {
        actorNetManager.Object.AssignInputAuthority(actorNetManager.Object.StateAuthority);
        State_InitNPCData();
        base.State_Init();
    }
    /// <summary>
    /// ��ʼ��NPC����(�ͻ���)
    /// </summary>
    public virtual void AllClient_InitNPCData()
    {
        statusManager.statusType = config.status_Type;
    }
    /// <summary>
    /// ��ʼ��NPC����(������)
    /// </summary>
    public virtual void State_InitNPCData()
    {
        UnityEngine.Random.InitState(new System.Random().Next(0, 10000));
        short eyeID = EyeConfigData.eyeConfigs[UnityEngine.Random.Range(0, EyeConfigData.eyeConfigs.Count)].Eye_ID;
        short hairID = HairConfigData.hairConfigs[UnityEngine.Random.Range(0, HairConfigData.hairConfigs.Count)].Hair_ID;
        Color32 hairColor = UnityEngine.Random.ColorHSV();

        State_SetHeadAndBody(itemManager.CreateItemData(config.short_HatID), itemManager.CreateItemData(config.short_ClothesID));
        State_SetFace(config.str_Title, eyeID, hairID, hairColor);
        State_SetAbilityData(config.short_Hp, config.short_Armor, config.short_Resistance, config.short_Speed);
        State_ResetBag();
    }
    #endregion
    
    #region//����
    /// <summary>
    /// ����Ŀ��(����)
    /// </summary>
    /// <param name="followType">���ٷ�ʽ</param>
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
        //            /*Ŀ�����Լ��Ĺ�����Χ��,Զ��Ŀ��*/
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
        //            /*Ŀ�겻���Լ��Ĺ�����Χ��,����Ŀ��*/
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
    /// ��������(����)
    /// </summary>
    /// <returns></returns>
    public virtual ItemData State_FindItemInBag(Func<ItemConfig, bool> function)
    {
        ItemData item = new ItemData(0);
        List<ItemData> items = actorNetManager.Local_GetBagItem();
        for (int i = 0; i < items.Count; i++)
        {
            if (function.Invoke(ItemConfigData.GetItemConfig(items[i].Item_ID)))
            {
                item = items[i];
            }
        }
        return item;
    }
    /// <summary>
    /// �ó�����(����)
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
            List<ItemData> items = actorNetManager.Local_GetBagItem();
            for (int i = 0; i < items.Count; i++)
            {
                if (function.Invoke(ItemConfigData.GetItemConfig(items[i].Item_ID)))
                {
                    int index = i;
                    ItemData itemData_Old = items[i];
                    ItemData itemData_New = new ItemData(); 
                    actorNetManager.OnlyState_AddItemOnHand(itemData_Old);
                    actorNetManager.Local_ChangeItemInBag(index, itemData_New);
                    return;
                }
            }
        }
    }
    /// <summary>
    /// ��������(����)
    /// </summary>
    public virtual void State_PutDownHand()
    {
        ItemData item = actorNetManager.Net_ItemInHand;
        actorNetManager.OnlyState_SubItemOnHand(item);
        actorNetManager.Local_AddItemInBag(0, item);
    }
    /// <summary>
    /// ������Ϣ(������)
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
    /// ������Ϣ(����)
    /// </summary>
    /// <param name="text"></param>
    public void Local_TryToSendText(string text, Emoji emoji)
    {
        actorNetManager.RPC_LocalInput_SendText(text, (int)emoji);
    }
    /// <summary>
    /// ����Emoji(������)
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
    /// ����Emoji(����)
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
    #region//����
    /// <summary>
    /// ˢ�±���
    /// </summary>
    public virtual void State_ResetBag()
    {
        List<ItemData> itemDatas = new List<ItemData>();
        for (int i = 0; i < config.lootInfos_Base.Count; i++)
        {
            int count = new System.Random().Next(config.lootInfos_Base[i].CountMin, config.lootInfos_Base[i].CountMax + 1);
            ItemData item = itemManager.CreateItemData(config.lootInfos_Base[i].ID, (short)count);
            itemDatas.Add(item);
        }
        for (int i = 0; i < config.lootInfos_Extra.Count; i++)
        {
            int temp = new System.Random().Next(0, 1000);
            if (temp > config.lootInfos_Extra[i].Weight)
            {
                ItemData item = itemManager.CreateItemData(config.lootInfos_Extra[i].ID, config.lootInfos_Extra[i].Count);
                itemDatas.Add(item);
            }
        }
        actorNetManager.Local_SetBagItem(itemDatas);
    }
    #endregion
}
[Serializable]
public struct ActorConfig_NPC
{
    [Header("��ʼ����")]
    public string str_Title;
    [Header("��ʼ���")]
    public StatusType status_Type;
    [Header("��ʼ����")]
    public short short_Hp;
    [Header("��ʼ����")]
    public short short_Armor;
    [Header("��ʼ����")]
    public short short_Resistance;
    [Header("��ʼ��Ұ")]
    public short short_View;
    [Header("��ʼ�ٶ�(����ÿ��)")]
    public short short_Speed;
    [Header("��ʼñ��")]
    public short short_HatID;
    [Header("��ʼ�·�")]
    public short short_ClothesID;
    [Header("��ʼ������Ʒ")]
    public List<BaseLootInfo> lootInfos_Base; 
    [Header("���ر�����Ʒ")]
    public List<ExtraLootInfo> lootInfos_Extra;
}
/// <summary>
/// ���ٷ�ʽ
/// </summary>
public enum FollowType
{
    /// <summary>
    /// ����
    /// </summary>
    RunAway,
    /// <summary>
    /// ����
    /// </summary>
    RunTo,
    /// <summary>
    /// ����
    /// </summary>
    Attack
}
