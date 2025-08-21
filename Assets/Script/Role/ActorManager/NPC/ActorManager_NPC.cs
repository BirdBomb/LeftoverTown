using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UniRx;
using Unity.Collections.LowLevel.Unsafe;
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
    /// ��ʼ��NPC����(�ͻ���)
    /// </summary>
    public virtual void AllClient_InitNPCData()
    {
        statusManager.statusType = config.status_Type;
        Local_ResetGood();
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
        State_ResetDrop();
    }
    public override void AllClient_AddListener()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_AllClient_SomeoneSendEmoji>().Subscribe(_ =>
        {
            AllClient_Listen_RoleSendEmoji(_.actor, _.emoji, _.distance);
            if (actorAuthority.isState) State_Listen_RoleSendEmoji(_.actor, _.emoji, _.distance);

        }).AddTo(this);
        MessageBroker.Default.Receive<GameEvent.GameEvent_AllClient_SomeoneCommit>().Subscribe(_ =>
        {
            AllClient_Listen_RoleCommit(_.actor, _.fine);
            if (actorAuthority.isState) State_Listen_RoleCommit(_.actor, _.fine);
        }).AddTo(this);
        MessageBroker.Default.Receive<GameEvent.GameEvent_All_UpdateHour>().Subscribe(_ =>
        {
            AllClient_Listen_UpdateTime(_.hour, _.day, _.now);
        }).AddTo(this);
        base.AllClient_AddListener();
    }

    #endregion
    #region//����
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
        ItemData itemNew = new ItemData(0);
        actorNetManager.OnlyState_AddItemOnHand(itemNew);
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
    public void State_TryToSendEmoji(float wait, Emoji emoji)
    {
        if (actorAuthority.isState)
        {
            StartCoroutine(State_SendEmoji(wait, emoji));
        }
    }
    /// <summary>
    /// ����Emoji(������)
    /// </summary>
    /// <param name="wait"></param>
    /// <returns></returns>
    private IEnumerator State_SendEmoji(float wait, Emoji emoji)
    {
        yield return new WaitForSeconds(wait);
        actorNetManager.RPC_LocalInput_SendEmoji((int)emoji);
    }
    #endregion
    #region//����ʱ��
    public override void AllClient_Listen_UpdateTime(int hour, int date, GlobalTime globalTime)
    {
        if(hour == 0) Local_ResetGood();
        base.AllClient_Listen_UpdateTime(hour, date, globalTime);
    }
    public override void State_Listen_UpdateTime(int hour, int date, GlobalTime globalTime)
    {
        if (hour == 0) State_ResetBag();
        base.State_Listen_UpdateTime(hour, date, globalTime);
    }
    #endregion
    #region//���������ˢ��
    /// <summary>
    /// ˢ�±���
    /// </summary>
    public virtual void State_ResetBag()
    {
        List<ItemData> itemDatas = new List<ItemData>();
        for (int i = 0; i < config.bagInfos_Base.Count; i++)
        {
            int count = new System.Random().Next(config.bagInfos_Base[i].CountMin, config.bagInfos_Base[i].CountMax + 1);
            ItemData item = itemManager.CreateItemData(config.bagInfos_Base[i].ID, (short)count);
            itemDatas.Add(item);
        }
        actorNetManager.Local_SetBagItem(itemDatas);
    }
    /// <summary>
    /// ˢ�µ���
    /// </summary>
    public virtual void State_ResetDrop()
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
        actorNetManager.Local_SetLootItems(itemDatas);
    }
    #endregion
    #region//����
    public override bool Local_CanDialog()
    {
        return true;
    }
    public override void Local_PlayerClose(ActorManager player)
    {
        if (!brainManager.allClient_actorManager_AttackTarget)
        {
            actorUI.ShowSingalR();
        }
        base.Local_PlayerClose(player);
    }
    public override void Local_PlayerFaraway(ActorManager player)
    {
        actorUI.HideSingal();
        Local_OverDialog(player);
        Local_OverDeal(player);
        base.Local_PlayerFaraway(player);
    }
    public override void Local_GetPlayerInput_R(ActorManager player)
    {
        if (!brainManager.allClient_actorManager_AttackTarget)
        {
            actorUI.ShowSingalTalk();
            if (!bool_Dialog)
            {
                Local_StartDialog(player);
            }
        }
        base.Local_GetPlayerInput_R(player);
    }
    #endregion
    #region//�Ի�
    protected TileUI_Dialog tileUI_Dialog = null;
    protected bool bool_Dialog = false;
    /// <summary>
    /// ��ʼ̸��
    /// </summary>
    /// <param name="actor"></param>
    /// <param name="index"></param>
    public virtual void Local_StartDialog(ActorManager player)
    {
        bool_Dialog = true;
        if (player != null && player.actorNetManager.Object != null)
        {
            actorNetManager.RPC_LocalInput_StandDown(66);
            UIManager.Instance.ShowTileUI(Resources.Load<GameObject>("UI/TileUI/TileUI_Dialog"), out TileUI tileUI);
            tileUI_Dialog = tileUI.GetComponent<TileUI_Dialog>();
            actorNetManager.RPC_LocalInput_TurnTo((player.transform.position.x > transform.position.x));
            Local_SetDialog(player, 0);
        }
    }
    /// <summary>
    /// ����̸��
    /// </summary>
    public virtual void Local_SetDialog(ActorManager player, int index)
    {

    }
    /// <summary>
    /// ����̸��
    /// </summary>
    /// <param name="actor"></param>
    public virtual void Local_OverDialog(ActorManager player)
    {
        bool_Dialog = false;
        if (player != null && player.actorNetManager.Object != null)
        {
            actorNetManager.RPC_LocalInput_StandDown(1);
            UIManager.Instance.HideTileUI(tileUI_Dialog);
            tileUI_Dialog = null;
        }
    }

    #endregion
    #region//����
    protected List<ItemData> goodList = new List<ItemData>();
    protected TileUI_Deal tileUI_Deal = null;
    protected bool bool_Deal = false;
    /// <summary>
    /// ��ʼ����
    /// </summary>
    /// <param name="actor"></param>
    public virtual void Local_StartDeal(ActorManager actor)
    {
        bool_Deal = true;
        if (actor != null && actor.actorNetManager.Object != null)
        {
            UIManager.Instance.ShowTileUI(Resources.Load<GameObject>("UI/TileUI/TileUI_DealUI"), out TileUI tileUI);
            tileUI_Deal = tileUI.GetComponent<TileUI_Deal>();
            Local_SetDeal(tileUI_Deal);
        }
    }
    /// <summary>
    /// ���ý���
    /// </summary>
    /// <param name="tileUI_Deal"></param>
    public virtual void Local_SetDeal(TileUI_Deal tileUI)
    {
        tileUI.Init(this);
    }
    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="actor"></param>
    public virtual void Local_OverDeal(ActorManager actor)
    {
        bool_Deal = false;
        if (actor != null && actor.actorNetManager.Object != null)
        {
            UIManager.Instance.HideTileUI(tileUI_Deal);
        }
    }
    /// <summary>
    /// ����
    /// </summary>
    /// <param name="itemData"></param>
    /// <returns></returns>
    public virtual int Local_Offer(ItemData itemData)
    {
        return 0;
    }
    /// <summary>
    /// ˢ����Ʒ
    /// </summary>
    public virtual void Local_ResetGood()
    {
        actorNetManager.Local_Coin = config.short_Coin;
        goodList.Clear();
        List<ItemData> itemDatas = new List<ItemData>();
        for (int i = 0; i < config.goodInfos_Base.Count; i++)
        {
            int count = new System.Random().Next(config.goodInfos_Base[i].CountMin, config.goodInfos_Base[i].CountMax + 1);
            ItemData item = itemManager.CreateItemData(config.goodInfos_Base[i].ID, (short)count);
            itemDatas.Add(item);
        }
        for (int i = 0; i < config.goodInfos_Extra.Count; i++)
        {
            int temp = new System.Random().Next(0, 1000);
            if (temp > config.goodInfos_Extra[i].Weight)
            {
                ItemData item = itemManager.CreateItemData(config.goodInfos_Extra[i].ID, config.goodInfos_Extra[i].Count);
                itemDatas.Add(item);
            }
        }
        for (int i = 0; i < itemDatas.Count; i++)
        {
            if (itemDatas[i].Item_Count >= 0)
            {
                for (int j = 0; j < itemDatas[i].Item_Count; j++)
                {
                    ItemData itemData = itemDatas[i];
                    itemData.Item_Count = 1;
                    goodList.Add(itemData);
                }
            }
        }
    }
    /// <summary>
    /// ��ȡ��Ʒ
    /// </summary>
    /// <returns></returns>
    public List<ItemData> Local_GetGood()
    {
        return goodList;
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
    [Header("��ʼ���")]
    public short short_Coin;
    [Header("��ʼ����")]
    public short short_Armor;
    [Header("��ʼħ��")]
    public short short_Resistance;
    [Header("��ʼ��Ұ")]
    public short short_View;
    [Header("��ʼ�ٶ�(����ÿ��)")]
    public short short_Speed;
    [Header("��ʼñ��")]
    public short short_HatID;
    [Header("��ʼ�·�")]
    public short short_ClothesID;
    [Header("�̶�����")]
    public List<BaseLootInfo> bagInfos_Base;
    [Header("�̶�����")]
    public List<BaseLootInfo> lootInfos_Base; 
    [Header("�������")]
    public List<ExtraLootInfo> lootInfos_Extra;
    [Header("�̶���Ʒ")]
    public List<BaseLootInfo> goodInfos_Base;
    [Header("������Ʒ")]
    public List<ExtraLootInfo> goodInfos_Extra;
}