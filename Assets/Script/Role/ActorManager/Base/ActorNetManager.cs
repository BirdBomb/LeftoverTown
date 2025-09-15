using Fusion;
using Fusion.Addons.Physics;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
/// <summary>
/// ��ɫ���������
/// </summary>
public class ActorNetManager : NetworkBehaviour
{
    [Header("����ͬ�����")]
    public NetworkRigidbody2D networkRigidbody;
    [Header("���ؽ�ɫ���")]
    public ActorManager actorManager_Local;
    public override void Spawned()
    {
        if (Object.HasStateAuthority)
        {
            actorManager_Local.State_Init();
        }
        actorManager_Local.AllClient_Init();
        AllClient_InitNetData();
        WorldManager.Instance.AddActor(actorManager_Local);
        base.Spawned();
    }
    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        WorldManager.Instance.SubActor(actorManager_Local);
        base.Despawned(runner, hasState);
    }
    public override void FixedUpdateNetwork()
    {
        if (actorManager_Local.actorAuthority.isState) actorManager_Local.State_FixedUpdateNetwork(Runner.DeltaTime);
        base.FixedUpdateNetwork();
    }
    #region//(�Ǳ���)��ʼ��>>>��������
    /// <summary>
    /// �ͻ��˳�ʼ������
    /// </summary>
    public void AllClient_InitNetData()
    {
        AllClient_RequestInfo();
        OnItemHandChange();
        OnItemHeadChange();
        OnItemBodyChange();
    }
    /// <summary>
    /// �ͻ������������������
    /// </summary>
    public void AllClient_RequestInfo()
    {
        PlayerRef playerRef = Runner.LocalPlayer;
        RPC_Client_RequestInfo(playerRef);
    }
    /// <summary>
    /// �ͻ������������������
    /// </summary>
    /// <param name="from"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    private void RPC_Client_RequestInfo(PlayerRef from)
    {
        State_SendInfo(from);
    }
    /// <summary>
    /// [���ض��ͻ���]��������ͻ��˷�������
    /// </summary>
    /// <param name="to"></param>
    public void State_SendInfo(PlayerRef to)
    {
        RPC_State_SendFaceInfo(to, Local_Name, Local_Seed, Local_HairID, Local_EyeID, Local_HairColor);
        RPC_State_SendAbilityInfo(to, Local_HpMax, Local_FoodMax, Local_SanMax, Local_Coin, Local_Fine);
    }
    /// <summary>
    /// [���ض��ͻ���]���ͱ�����ò����
    /// </summary>
    /// <param name="target"></param>
    /// <param name="name"></param>
    /// <param name="seed"></param>
    /// <param name="hairID"></param>
    /// <param name="eyeID"></param>
    /// <param name="hairColor"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_State_SendFaceInfo([RpcTarget] PlayerRef target, string name, short seed, short hairID, short eyeID, Color32 hairColor)
    {
        Local_Name = name;
        Local_Seed = seed;
        Local_HairID = hairID;
        Local_EyeID = eyeID;
        Local_HairColor = hairColor;
        actorManager_Local.bodyController.InitFace(Local_HairID, Local_EyeID, Local_HairColor);
        actorManager_Local.actorUI.ShowName(Local_Name);
    }
    /// <summary>
    /// [���ض��ͻ���]���ͱ�����������
    /// </summary>
    /// <param name="target"></param>
    /// <param name="maxHp"></param>
    /// <param name="maxFood"></param>
    /// <param name="maxSan"></param>
    /// <param name="coin"></param>
    /// <param name="fine"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_State_SendAbilityInfo([RpcTarget] PlayerRef target, short maxHp, short maxFood, short maxSan, int coin, int fine)
    {
        Local_HpMax = maxHp;
        Local_FoodMax = maxFood;
        Local_SanMax = maxSan;
        Local_Coin = coin;
        Local_Fine = fine;
    }
    #endregion
    #region//(�������)��ʼ��>>>��������
    /// <summary>
    /// ����������ͱ�������
    /// </summary>
    /// <param name="netData"></param>
    /// <param name="playerName"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_InitPlayerCommonData(PlayerNetData netData, string playerName)
    {
        Local_Name = playerName;
        Local_EyeID = netData.Eye_ID;
        Local_HairID = netData.Hair_ID;
        Local_HairColor = netData.Hair_Color;
        if (netData.Hp_Cur <= 0)
        {
            Net_HpCur = netData.Hp_Max;
        }
        else
        {
            Net_HpCur = netData.Hp_Cur;
        }
        Local_HpMax = netData.Hp_Max;
        Net_Armor = netData.Armor_Cur;
        Net_Resistance = netData.Resistance_Cur;
        Net_FoodCur = netData.Food_Cur;
        Local_FoodMax = netData.Food_Max;
        Net_SanCur = netData.San_Cur;
        Local_SanMax = netData.San_Max;
        Local_Coin = netData.Coin_Cur;
        Local_Fine = netData.Fine_Cur;

        Net_SpeedCommon = netData.Speed_Common;

        State_SendInfoToAll();
    }
    /// <summary>
    /// ��������ͻ��˷�������
    /// </summary>
    public void State_SendInfoToAll()
    {
        RPC_State_SendFaceInfoToAll(Local_Name, Local_Seed, Local_HairID, Local_EyeID, Local_HairColor);
        RPC_State_SendAbilityInfoToAll(Local_HpMax, Local_FoodMax, Local_SanMax, Local_Coin, Local_Fine);
    }
    /// <summary>
    /// ���ͱ�����ò����
    /// </summary>
    /// <param name="name"></param>
    /// <param name="seed"></param>
    /// <param name="hairID"></param>
    /// <param name="eyeID"></param>
    /// <param name="hairColor"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_State_SendFaceInfoToAll(string name, short seed, short hairID, short eyeID, Color32 hairColor)
    {
        Local_Name = name;
        Local_Seed = seed;
        Local_HairID = hairID;
        Local_EyeID = eyeID;
        Local_HairColor = hairColor;
        actorManager_Local.bodyController.InitFace(Local_HairID, Local_EyeID, Local_HairColor);
        actorManager_Local.actorUI.ShowName(Local_Name);
    }
    /// <summary>
    /// ���ͱ�����������
    /// </summary>
    /// <param name="maxHp"></param>
    /// <param name="maxFood"></param>
    /// <param name="maxSan"></param>
    /// <param name="coin"></param>
    /// <param name="fine"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_State_SendAbilityInfoToAll(short maxHp, short maxFood, short maxSan, int coin, int fine)
    {
        Local_HpMax = maxHp;
        Local_FoodMax = maxFood;
        Local_SanMax = maxSan;
        Local_Coin = coin;
        Local_Fine = fine;
    }
    #endregion
    #region//��ò������(ֻ�ڱ��ض˼���)
    public string Local_Name { get; set; } = "";
    public short Local_Seed { get; set; } = 0;
    public short Local_EyeID { get; set; } = 0;
    public short Local_HairID { get; set; } = 0;
    public Color32 Local_HairColor { get; set; } = Color.white;
    #endregion
    #region//����ֵ(ֻ�ڱ��ض˼���)
    private short Local_Level { get; set; }
    private short Local_ExpCur { get; set; }
    private short Local_ExpMax { get; set; }
    public short Local_GetExp()
    {
        return Local_ExpCur;
    }
    public void Local_SetExp(short exp)
    {
        if (Local_Level < 1) { return; }
        Local_ExpMax = (short)(Local_Level * 5 + 45);
        if (exp > Local_ExpMax)
        {
            Local_ExpCur = (short)(exp - Local_ExpMax);
            Local_SetLevel((short)(Local_Level + 1));
        }
        if (actorManager_Local.actorAuthority.isPlayer && actorManager_Local.actorAuthority.isLocal)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateExpData()
            {
                Level = Local_Level,
                Exp_Cur = Local_ExpCur,
                Exp_Max = Local_ExpMax,
            });
        }
    }
    public short Local_GetLevel()
    {
        return Local_Level;
    }
    public void Local_SetLevel(short level)
    {
        Local_Level = level;
        Local_ExpMax = (short)(Local_Level * 5 + 45);
        if (actorManager_Local.actorAuthority.isPlayer && actorManager_Local.actorAuthority.isLocal)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateExpData()
            {
                Level = Local_Level,
                Exp_Cur = Local_ExpCur,
                Exp_Max = Local_ExpMax,
            });
        }
    }
    #endregion
    #region//����ֵ(���пͻ���)
    /// <summary>
    /// ��ǰ����ֵ
    /// </summary>
    [Networked, OnChangedRender(nameof(OnCurHpChange)), HideInInspector]
    public short Net_HpCur { get; set; }
    /// <summary>
    /// �������ֵ
    /// </summary>
    public short Local_HpMax { get; set; }
    public void OnCurHpChange()
    {
        if (Net_HpCur <= 0)
        {
            actorManager_Local.AllClient_UpdateHpBar(0);
        }
        else
        {
            actorManager_Local.AllClient_UpdateHpBar((float)Net_HpCur / (float)Local_HpMax);
        }
        if (actorManager_Local.actorAuthority.isPlayer && actorManager_Local.actorAuthority.isLocal)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateHPData()
            {
                HP_Cur = Net_HpCur,
                HP_Max = Local_HpMax,
            });
        }
    }
    /// <summary>
    /// RPC:����ֵ�ı�
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="networkId"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_AllClient_HpChange(int parameter, int reason, NetworkId networkId)
    {
        actorManager_Local.AllClient_Listen_MyselfHpChange(parameter, (HpChangeReason)reason, networkId);
        if (actorManager_Local.actorAuthority.isState)
        {
            OnlyState_HpChange(parameter, reason, networkId);
        }
    }
    private void OnlyState_HpChange(int parameter, int reason, NetworkId networkId)
    {
        if (Net_HpCur + parameter <= 0)
        {
            Net_HpCur = 0;
            Net_SpeedCommon = 0;
            RPC_AllClient_Dead();
        }
        else if (Net_HpCur + parameter > Local_HpMax)
        {
            Net_HpCur = Local_HpMax;
        }
        else
        {
            Net_HpCur = (short)(Net_HpCur + parameter);
            actorManager_Local.State_Listen_MyselfHpChange(parameter, (HpChangeReason)reason, networkId);
        }
    }
    /// <summary>
    /// RPC:�������ֵ�ı�
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="networkId"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_AllClient_MaxHpChange(short parameter, NetworkId networkId)
    {
        Local_HpMax += parameter;
        if (actorManager_Local.actorAuthority.isPlayer && actorManager_Local.actorAuthority.isLocal)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateHPData()
            {
                HP_Cur = Net_HpCur,
                HP_Max = Local_HpMax,
            });
        }
    }
    #endregion
    #region//����ֵ(���пͻ���)
    /// <summary>
    /// ��ǰʳ��ֵ
    /// </summary>
    [Networked, OnChangedRender(nameof(OnCurFoodChange)), HideInInspector]
    public short Net_FoodCur { get; set; }
    /// <summary>
    /// ���ʳ��ֵ
    /// </summary>
    public short Local_FoodMax { get; set; }
    public void OnCurFoodChange()
    {
        if (actorManager_Local.actorAuthority.isPlayer && actorManager_Local.actorAuthority.isLocal)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateFoodData()
            {
                Food_Cur = Net_FoodCur,
                Food_Max = Local_FoodMax,
            });
        }
    }
    /// <summary>
    /// RPC:����ֵ�ı�
    /// </summary>
    /// <param name="val"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_FoodChange(short parameter)
    {
        OnlyState_FoodChange(parameter);
    }
    private void OnlyState_FoodChange(short parameter)
    {
        if (Net_FoodCur + parameter <= 0)
        {
            Net_FoodCur = 0;
        }
        else if (Net_FoodCur + parameter > Local_FoodMax)
        {
            Net_FoodCur = Local_FoodMax;
        }
        else
        {
            Net_FoodCur = (short)(Net_FoodCur + parameter);
        }
    }
    #endregion
    #region//����ֵ(���пͻ���)
    /// <summary>
    /// ��ǰ����ֵ
    /// </summary>
    [Networked, OnChangedRender(nameof(OnCurSanChange)), HideInInspector]
    public short Net_SanCur { get; set; }
    /// <summary>
    /// �����ֵ
    /// </summary>
    public short Local_SanMax { get; set; }
    public void OnCurSanChange()
    {
        if (actorManager_Local.actorAuthority.isPlayer && actorManager_Local.actorAuthority.isLocal)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateSanData()
            {
                San_Cur = Net_SanCur,
                San_Max = Local_SanMax,
            });
        }
    }
    /// <summary>
    /// RPC:����ֵ�ı�
    /// </summary>
    /// <param name="val"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_SanChange(short parameter)
    {
        OnlyState_SanChange(parameter);
    }
    private void OnlyState_SanChange(short parameter)
    {
        if (Net_SanCur + parameter <= 0)
        {
            Net_SanCur = 0;
        }
        else if (Net_SanCur + parameter > Local_SanMax)
        {
            Net_SanCur = Local_FoodMax;
        }
        else
        {
            Net_SanCur = (short)(Net_SanCur + parameter);
        }
    }
    #endregion
    #region//�ٶ�(���пͻ���)
    /// <summary>
    /// ��ͨ�ٶ�(����/��)
    /// </summary>
    [Networked, HideInInspector]
    public short Net_SpeedCommon { get; set; }
    #endregion
    #region//����(���пͻ���)
    /// <summary>
    /// ����
    /// </summary>
    [Networked, OnChangedRender(nameof(OnArmorChange)), HideInInspector]
    public short Net_Armor { get; set; }
    public void OnArmorChange()
    {
        if (actorManager_Local.actorAuthority.isPlayer && actorManager_Local.actorAuthority.isLocal)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateArmorData()
            {
                Armor = Net_Armor,
            });
        }
    }
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_ChangeArmor(short val)
    {
        OnlyState_ChangeArmor(val);
    }
    private void OnlyState_ChangeArmor(short val)
    {
        Net_Armor += val;
    }
    #endregion
    #region//ħ��(���пͻ���)
    /// <summary>
    /// ħ��
    /// </summary>
    [Networked, OnChangedRender(nameof(OnResistanceChange)), HideInInspector]
    public short Net_Resistance { get; set; }
    public void OnResistanceChange()
    {
        if (actorManager_Local.actorAuthority.isPlayer && actorManager_Local.actorAuthority.isLocal)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateResistanceData()
            {
                Resistance = Net_Resistance,
            });
        }
    }
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_ChangeResistance(short val)
    {
        OnlyState_ChangeResistance(val);
    }
    private void OnlyState_ChangeResistance(short val)
    {
        Net_Resistance += val;
    }
    #endregion
    #region//�������(���пͻ���)
    /// <summary>
    /// �ֲ�����
    /// </summary>
    [Networked, OnChangedRender(nameof(OnItemHandChange)), HideInInspector]
    public ItemData Net_ItemHand { get; set; }
    public void OnItemHandChange()
    {
        actorManager_Local.itemManager.UpdateItemHand(Net_ItemHand);
    }
    /// <summary>
    /// RPC:��ӳ�������
    /// </summary>
    /// <param name="itemData_Add"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_ItemHand_Add(ItemData itemData_Add)
    {
        OnlyState_ItemHand_Add(itemData_Add);
    }
    public void OnlyState_ItemHand_Add(ItemData itemData_Add)
    {
        Net_ItemHand = GameToolManager.Instance.CombineItem(Net_ItemHand, itemData_Add, out ItemData itemData_Res);
        if (itemData_Res.Item_ID > 0 && itemData_Res.Item_Count > 0)
        {
            RPC_State_ItemInBag_Add(itemData_Res, (short)ItemFrom.Hand);
        }
    }
    /// <summary>
    /// RPC:���ٳ�������
    /// </summary>
    /// <param name="itemData_Sub"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_ItemHand_Sub(ItemData itemData_Sub)
    {
        OnlyState_ItemHand_Sub(itemData_Sub);
    }
    public void OnlyState_ItemHand_Sub(ItemData itemData_Sub)
    {
        Net_ItemHand = GameToolManager.Instance.SplitItem(Net_ItemHand, itemData_Sub);
    }
    /// <summary>
    /// RPC:�޸ĳ�����Ʒ
    /// </summary>
    /// <param name="itemData"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_ItemHand_Change(ItemData itemData_Old, ItemData itemData_New)
    {
        OnlyState_ItemHand_Change(itemData_Old, itemData_New);
    }
    public void OnlyState_ItemHand_Change(ItemData itemData_Old, ItemData itemData_New)
    {
        if (Net_ItemHand.Equals(itemData_Old))
        {
            Net_ItemHand = itemData_New;
        }
    }

    #endregion
    #region//����ñ��(���пͻ���)
    /// <summary>
    /// ͷ������
    /// </summary>
    [Networked, OnChangedRender(nameof(OnItemHeadChange)), HideInInspector]
    public ItemData Net_ItemHead { get; set; }
    public void OnItemHeadChange()
    {
        actorManager_Local.itemManager.UpdateItemHead(Net_ItemHead);
    }
    /// <summary>
    /// RPC:���ͷ������
    /// </summary>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_ItemHead_Add(ItemData itemData_Add)
    {
        Debug.Log(itemData_Add.Item_ID);
        OnlyState_ItemHead_Add(itemData_Add);
    }
    public void OnlyState_ItemHead_Add(ItemData itemData_Add)
    {
        Net_ItemHead = GameToolManager.Instance.CombineItem(Net_ItemHead, itemData_Add, out ItemData itemData_Res);
        if (itemData_Res.Item_ID > 0 && itemData_Res.Item_Count > 0)
        {
            RPC_State_ItemInBag_Add(itemData_Res, (short)ItemFrom.Head);
        }
    }
    /// <summary>
    /// RPC:����ͷ������
    /// </summary>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_ItemHead_Sub(ItemData itemData_Sub)
    {
        OnlyState_ItemHead_Sub(itemData_Sub);
    }
    public void OnlyState_ItemHead_Sub(ItemData itemData_Sub)
    {
        Net_ItemHead = GameToolManager.Instance.SplitItem(Net_ItemHead, itemData_Sub);
    }
    /// <summary>
    /// RPC:����ͷ������
    /// </summary>
    /// <param name="itemData"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_ItemHead_Change(ItemData itemData_Old, ItemData itemData_New)
    {
        OnlyState_ItemHead_Change(itemData_Old, itemData_New);
    }
    public void OnlyState_ItemHead_Change(ItemData oldItemData, ItemData newItemData)
    {
        if (Net_ItemHead.Equals(oldItemData))
        {
            Net_ItemHead = newItemData;
        }
    }

    #endregion
    #region//��������(���пͻ���)
    [Networked, OnChangedRender(nameof(OnItemBodyChange)), HideInInspector]
    public ItemData Net_ItemBody { get; set; }
    public void OnItemBodyChange()
    {
        actorManager_Local.itemManager.UpdateItemBody(Net_ItemBody);
    }
    /// <summary>
    /// RPC:��Ӵ�������
    /// </summary>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_ItemBody_Add(ItemData itemData_Add)
    {
        OnlyState_ItemBody_Add(itemData_Add);
    }
    public void OnlyState_ItemBody_Add(ItemData itemData_Add)
    {
        Net_ItemBody = GameToolManager.Instance.CombineItem(Net_ItemBody, itemData_Add, out ItemData itemData_Res);
        if (itemData_Res.Item_ID > 0 && itemData_Res.Item_Count > 0)
        {
            RPC_State_ItemInBag_Add(itemData_Res, (short)ItemFrom.Body);
        }
    }
    /// <summary>
    /// RPC:���ٴ�������
    /// </summary>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_ItemBody_Sub(ItemData itemData_Sub)
    {
        OnlyState_ItemBody_Sub(itemData_Sub);
    }
    public void OnlyState_ItemBody_Sub(ItemData itemData_Sub)
    {
        Net_ItemBody = GameToolManager.Instance.SplitItem(Net_ItemBody, itemData_Sub);
    }
    /// <summary>
    /// RPC:�޸Ĵ�������
    /// </summary>
    /// <param name="itemData"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_ItemBody_Change(ItemData itemData_Old, ItemData itemData_New)
    {
        OnlyState_ItemBody_Change(itemData_Old, itemData_New);
    }
    public void OnlyState_ItemBody_Change(ItemData itemData_Old, ItemData itemData_New)
    {
        if (Net_ItemBody.Equals(itemData_Old))
        {
            Net_ItemBody = itemData_New;
        }
    }

    #endregion
    #region//������Ʒ(���пͻ���)
    [Networked, OnChangedRender(nameof(OnItemAccessoryChange)), HideInInspector]
    public ItemData Net_ItemAccessory { get; set; }
    public void OnItemAccessoryChange()
    {
        actorManager_Local.itemManager.UpdateItemAccessory(Net_ItemAccessory);
    }
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_ItemAccessory_Add(ItemData itemData_Add)
    {
        OnlyState_ItemAccessory_Add(itemData_Add);
    }
    public void OnlyState_ItemAccessory_Add(ItemData itemData_Add)
    {
        Net_ItemAccessory = GameToolManager.Instance.CombineItem(Net_ItemAccessory, itemData_Add, out ItemData itemData_Res);
        if (itemData_Res.Item_ID > 0 && itemData_Res.Item_Count > 0)
        {
            RPC_State_ItemInBag_Add(itemData_Res, (short)ItemFrom.Accessory);
        }
    }
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_ItemAccessory_Sub(ItemData itemData_Sub)
    {
        OnlyState_ItemAccessory_Sub(itemData_Sub);
    }
    public void OnlyState_ItemAccessory_Sub(ItemData itemData_Sub)
    {
        Net_ItemAccessory = GameToolManager.Instance.SplitItem(Net_ItemAccessory, itemData_Sub);
    }
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_ItemAccessory_Change(ItemData itemData_Old, ItemData itemData_New)
    {
        OnlyState_ItemAccessory_Change(itemData_Old, itemData_New);
    }
    public void OnlyState_ItemAccessory_Change(ItemData itemData_Old, ItemData itemData_New)
    {
        if (Net_ItemAccessory.Equals(itemData_Old))
        {
            Net_ItemAccessory = itemData_New;
        }
    }

    #endregion
    #region//����Ĳ�(���пͻ���)
    [Networked, OnChangedRender(nameof(OnItemConsumablesChange)), HideInInspector]
    public ItemData Net_ItemConsumables { get; set; }
    public void OnItemConsumablesChange()
    {
        actorManager_Local.itemManager.UpdateItemConsumables(Net_ItemConsumables);
    }
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_ItemConsumables_Add(ItemData itemData_Add)
    {
        OnlyState_ItemConsumables_Add(itemData_Add);
    }
    public void OnlyState_ItemConsumables_Add(ItemData itemData_Add)
    {
        Net_ItemConsumables = GameToolManager.Instance.CombineItem(Net_ItemConsumables, itemData_Add, out ItemData itemData_Res);
        if (itemData_Res.Item_ID > 0 && itemData_Res.Item_Count > 0)
        {
            RPC_State_ItemInBag_Add(itemData_Res, (short)ItemFrom.Consumables);
        }
    }
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_ItemConsumables_Sub(ItemData itemData_Sub)
    {
        OnlyState_ItemConsumables_Sub(itemData_Sub);
    }
    public void OnlyState_ItemConsumables_Sub(ItemData itemData_Sub)
    {
        Net_ItemConsumables = GameToolManager.Instance.SplitItem(Net_ItemConsumables, itemData_Sub);
    }
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_ItemConsumables_Change(ItemData itemData_Old, ItemData itemData_New)
    {
        OnlyState_ItemConsumables_Change(itemData_Old, itemData_New);
    }
    public void OnlyState_ItemConsumables_Change(ItemData itemData_Old, ItemData itemData_New)
    {
        if (Net_ItemConsumables.Equals(itemData_Old))
        {
            Net_ItemConsumables = itemData_New;
        }
    }
    #endregion
    #region//���屳��(ֻ�ڱ��ض˼���)
    public int Local_BagCapacity = 40;
    public int Local_BagItemCount = 0;
    private List<ItemData> Local_ItemBag { get; } = new List<ItemData>(40);
    /// <summary>
    /// ��ñ�������
    /// </summary>
    /// <returns></returns>
    public List<ItemData> Local_ItemBag_Get()
    {
        return new List<ItemData>(Local_ItemBag);
    }
    /// <summary>
    /// ���ñ�������
    /// </summary>
    /// <param name="itemDatas"></param>
    public void Local_ItemBag_Set(List<ItemData> itemDatas)
    {
        Local_ItemBag.Clear();
        Local_BagItemCount = 0;
        for (int i = 0; i < itemDatas.Count; i++)
        {
            if (itemDatas[i].Item_ID != 0) { Local_BagItemCount++; }
            Local_ItemBag.Add(itemDatas[i]);
        }
        if (actorManager_Local.actorAuthority.isPlayer && actorManager_Local.actorAuthority.isLocal)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateItemInBag()
            {
                itemDatas = Local_ItemBag_Get()
            });
        }
    }
    /// <summary>
    /// RPC:��ӱ�������
    /// </summary>
    /// <param name="itemData"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPC_State_ItemInBag_Add(ItemData addData,short from)
    {
        if (actorManager_Local.actorAuthority.isLocal)
        {
            if (actorManager_Local.actorAuthority.isPlayer)
            {
                MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Add()
                {
                    index = 0,
                    itemData = addData,
                    itemFrom = (ItemFrom)(from),
                });
            }
            else
            {
                Local_ItemBag_Add(0, addData, (ItemFrom)(from));
            }
        }
    }
    public void Local_ItemBag_Add(int index, ItemData addData,ItemFrom from)
    {
        if (addData.Item_ID > 0)
        {
            List<ItemData> itemDatas = Local_ItemBag_Get();
            GameToolManager.Instance.PutInItemList(itemDatas, addData, index, Local_BagCapacity, out ItemData resData);
            Local_ItemBag_Set(itemDatas);
            if (resData.Item_Count > 0)
            {
                MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnItem()
                {
                    itemData = resData,
                    itemOwner = Object.Id,
                    pos = transform.position,
                });
            }
            if (actorManager_Local.actorAuthority.isPlayer && from == ItemFrom.OutSide)
            {
                MessageBroker.Default.Publish(new UIEvent.UIEvent_PutItemInBag()
                {
                    item = addData
                });
            }
        }
    }
    /// <summary>
    /// RPC:���ı�����Ʒ
    /// </summary>
    /// <param name="itemData"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPC_State_ItemBag_Expend(int id, int count)
    {
        if (actorManager_Local.actorAuthority.isLocal)
        {
            if (actorManager_Local.actorAuthority.isPlayer)
            {
                MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Expend()
                {
                    itemID = id,
                    itemCount = count
                });
            }
            else
            {
                Local_ItemBag_Expend(id, count);
            }
        }
    }
    public void Local_ItemBag_Expend(int id, int count)
    {
        if (id > 0 && count > 0)
        {
            List<ItemData> itemDatas = Local_ItemBag_Get();
            GameToolManager.Instance.ExpendItemList(itemDatas, id, count);
            Local_ItemBag_Set(itemDatas);
        }
    }
    /// <summary>
    /// RPC:�޸ı�������
    /// </summary>
    /// <param name="itemData"></param>
    /// <param name="index"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPC_State_ItemBag_Change(int index, ItemData itemData)
    {
        if (actorManager_Local.actorAuthority.isLocal)
        {
            if (actorManager_Local.actorAuthority.isPlayer)
            {
                MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Change()
                {
                    index = index,
                    itemData = itemData
                });
            }
            else
            {
                Local_ItemBag_Change(index, itemData);
            }
        }
    }
    public void Local_ItemBag_Change(int index, ItemData itemData)
    {
        List<ItemData> itemDatas = Local_ItemBag_Get();
        if (actorManager_Local.actorAuthority.isPlayer)
        {
            if (index < itemDatas.Count)
            {
                if (itemDatas[index].Item_ID == itemData.Item_ID)
                {
                    if (itemDatas[index].Item_Count < itemData.Item_Count)
                    {
                        ItemData temp = itemData;
                        temp.Item_Count = (short)(itemData.Item_Count - itemDatas[index].Item_Count);
                        //MessageBroker.Default.Publish(new UIEvent.UIEvent_PutItemInBag()
                        //{
                        //    item = temp
                        //});
                    }
                    else if (itemDatas[index].Item_Count > itemData.Item_Count)
                    {
                        ItemData temp = itemData;
                        temp.Item_Count = (short)(itemDatas[index].Item_Count - itemData.Item_Count);
                        //MessageBroker.Default.Publish(new UIEvent.UIEvent_PutItemOutBag()
                        //{
                        //    item = temp
                        //});
                    }
                }
                else
                {
                    //MessageBroker.Default.Publish(new UIEvent.UIEvent_PutItemOutBag()
                    //{
                    //    item = itemDatas[index]
                    //});
                    //MessageBroker.Default.Publish(new UIEvent.UIEvent_PutItemInBag()
                    //{
                    //    item = itemData
                    //});
                }
            }
        }

        GameToolManager.Instance.ChangeItemList(itemDatas, itemData, index);
        Local_ItemBag_Set(itemDatas);
    }
    #endregion

    #region//���(ֻ�ڱ��ض˼���)
    /// <summary>
    /// ���
    /// </summary>
    public int Local_Coin { get; set; }
    /// <summary>
    /// ���ض�֧��
    /// </summary>
    /// <param name="val"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_Local_PayCoin(int val)
    {
        RPC_State_UpdateCoin(Local_Coin - val);
    }
    /// <summary>
    /// ���ض�׬Ǯ
    /// </summary>
    /// <param name="val"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_Local_EarnCoin(int val)
    {
        RPC_State_UpdateCoin(Local_Coin + val);
    }
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_State_UpdateCoin(int newVal)
    {
        Local_Coin = newVal;
        if (actorManager_Local.actorAuthority.isPlayer && actorManager_Local.actorAuthority.isLocal)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateCoinData()
            {
                Coin = Local_Coin,
            });
        }
    }

    #endregion
    #region//�ͽ�(ֻ�ڱ��ض˼���)
    /// <summary>
    /// ����
    /// </summary>
    public int Local_Fine { get; set; }
    /// <summary>
    /// ���ض������ͽ�
    /// </summary>
    /// <param name="val"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_Local_ChangeFine(short val)
    {
        RPC_State_UpdateFine(val);
    }
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_State_UpdateFine(short val)
    {
        Local_Fine = val;
        if (actorManager_Local.actorAuthority.isPlayer && actorManager_Local.actorAuthority.isLocal)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateFineData()
            {
                Fine = Local_Fine,
            });
        }
    }

    #endregion
    #region//Buff(ֻ�ڱ��ض˼���)
    private List<BuffData> Local_BuffList { get; } = new List<BuffData>();
    public List<BuffData> Local_GetBuffList()
    {
        return new List<BuffData>(Local_BuffList);
    }
    public void Local_SetBuffList(List<BuffData> buffDatas)
    {
        Local_BuffList.Clear();
        for (int i = 0; i < buffDatas.Count; i++)
        {
            Local_BuffList.Add(buffDatas[i]);
        }
        actorManager_Local.buffManager.Local_InitBuffs(Local_BuffList);
    }
    public void Local_AddBuff(BuffData buffData)
    {
        if (!Local_BuffList.Contains(buffData))
        {
            Local_BuffList.Add(buffData);
            actorManager_Local.buffManager.Local_AddBuff(buffData);
        }
    }
    public void Local_SubBuff(short buffID)
    {
        BuffData buffData = Local_BuffList.Find((x) => { return x.BuffID == buffID; });
        if (buffData.BuffID != 0)
        {
            Local_BuffList.Remove(buffData);
            actorManager_Local.buffManager.Local_RemoveBuff(buffID);
        }
    }
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_Local_PlayBuffEffect(short ID,short index)
    {
        actorManager_Local.buffManager.All_PlayBuffEffect(ID,index);
    }
    #endregion
    #region//������Ʒ(ֻ�ڱ��ض˼���)
    private List<ItemData> Local_LootItems { get; } = new List<ItemData>(15);
    /// <summary>
    /// ��õ�������
    /// </summary>
    /// <returns></returns>
    public List<ItemData> Local_GetLootItems()
    {
        return new List<ItemData>(Local_LootItems);
    }
    /// <summary>
    /// ���õ�������
    /// </summary>
    /// <param name="itemDatas"></param>
    public void Local_SetLootItems(List<ItemData> itemDatas)
    {
        Local_LootItems.Clear();
        for (int i = 0; i < itemDatas.Count; i++)
        {
            Local_LootItems.Add(itemDatas[i]);
        }
    }
    #endregion
    #region//��ɫ���ܲ���
    /// <summary>
    /// ���Ĺ���״̬
    /// </summary>
    /// <param name="attacking"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPC_State_NpcChangeAttackState(bool attacking)
    {
        actorManager_Local.AllClient_Listen_ChangeAttackState(attacking);
        if (actorManager_Local.actorAuthority.isState)
        {
            actorManager_Local.State_Listen_ChangeAttackState(attacking);
        }
    }
    /// <summary>
    /// ���Ĺ���Ŀ��
    /// </summary>
    /// <param name="id"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPC_State_NpcChangeAttackTarget(NetworkId id)
    {
        actorManager_Local.AllClient_Listen_ChangeAttackTarget(id);
        if (actorManager_Local.actorAuthority.isState)
        {
            actorManager_Local.State_Listen_ChangeAttackTarget(id);
        }
    }
    /// <summary>
    /// ʹ�ü���
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="networkId"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPC_State_NpcUseSkill(int id, Vector3Int vector3, NetworkId networkId)
    {
        actorManager_Local.AllClient_Listen_NpcAction(id, vector3, networkId);
    }
    /// <summary>
    /// ʹ�ü���
    /// </summary>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_Local_NpcUseSkill(int id, Vector3Int vector3, NetworkId networkId)
    {
        actorManager_Local.AllClient_Listen_NpcAction(id, vector3, networkId);
    }
    #endregion
    #region//��ɫλ��
    /// <summary>
    /// ���ض˸������λ��
    /// </summary>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_Local_SetNetworkTransform(Vector3 pos)
    {
        State_SetNetworkRigidbody(pos);
    }
    private void State_SetNetworkRigidbody(Vector3 pos)
    {
        networkRigidbody.Rigidbody.velocity = Vector2.zero;
        networkRigidbody.Rigidbody.position = (pos);
    }
    public void State_UpdateNetworkRigidbody(Vector3 pos, float speed)
    {
        if (networkRigidbody/* && actorManager_Local.actorAuthority.isState*/)
        {
            if (networkRigidbody.Rigidbody.velocity.magnitude <= speed)
            {
                networkRigidbody.Rigidbody.velocity = Vector2.zero;
                networkRigidbody.Rigidbody.position = (pos);
            }
        }
    }
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_AllClient_AddForce(Vector2 dir, short force)
    {
        if (actorManager_Local.actorAuthority.isState)
        {
            State_AddForce(dir, force);
        }
    }
    private void State_AddForce(Vector2 dir, short force)
    {
        if (networkRigidbody.Rigidbody.velocity == Vector2.zero)
        {
            networkRigidbody.Rigidbody.velocity = dir * force;
        }
        else
        {
            if (Vector2.Dot(networkRigidbody.Rigidbody.velocity, dir) > 0.2)
            {
                /*ͬ��--ȡ��*/
                if ((dir * force).magnitude > networkRigidbody.Rigidbody.velocity.magnitude)
                {
                    networkRigidbody.Rigidbody.velocity = dir * force;
                }
            }
            else
            {
                /*����--����*/
                networkRigidbody.Rigidbody.velocity += dir * force;
            }
        }
    }
    #endregion
    #region//��������
    /// <summary>
    /// ���ض˷��ͱ���
    /// </summary>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_LocalInput_SendEmoji(int id)
    {
        actorManager_Local.actionManager.SendEmoji(id);
    }
    /// <summary>
    /// ���ض˷�������
    /// </summary>
    /// <param name="text"></param>
    /// <param name="id"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_LocalInput_SendText(string text, int id)
    {
        actorManager_Local.actionManager.SendText(text, id);
    }
    /// <summary>
    /// ���ض˷���
    /// </summary>
    /// <param name="text"></param>
    /// <param name="id"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_LocalInput_Commit(short fine)
    {
        actorManager_Local.actionManager.Commit(fine);
    }
    /// <summary>
    /// RPC:���ض�����ʰ����Ʒ
    /// </summary>
    /// <param name="id"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_LocalInput_PickItemManual(NetworkId id)
    {
        bool OnlyState_PickItem(string val)
        {
            if (val.Equals("Pick"))
            {
                if (actorManager_Local.actorAuthority.isPlayer && actorManager_Local.actorAuthority.isLocal)
                {
                    AudioManager.Instance.Play2DEffect(1003);
                }
                if (Object.HasStateAuthority)
                {
                    NetworkObject networkPlayerObject = Runner.FindObject(id);
                    if (networkPlayerObject != null && networkPlayerObject.transform.TryGetComponent(out ItemNetObj itemNetObj))
                    {
                        itemNetObj.State_PickUp(Object.Id, out ItemData itemData_Pick);
                        RPC_State_ItemInBag_Add(itemData_Pick, (short)ItemFrom.OutSide);
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        actorManager_Local.actionManager.PlayPickUp(1, OnlyState_PickItem);
    }
    /// <summary>
    /// RPC:���ض��Զ�ʰ����Ʒ
    /// </summary>
    /// <param name="id"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_LocalInput_PickItemAuto(NetworkId id)
    {
        NetworkObject networkPlayerObject = Runner.FindObject(id);
        if (networkPlayerObject != null && networkPlayerObject.transform.TryGetComponent(out ItemNetObj itemNetObj))
        {
            if (Object.HasStateAuthority)
            {
                itemNetObj.State_PickUp(Object.Id, out ItemData itemData_Pick);
                RPC_State_ItemInBag_Add(itemData_Pick, (short)ItemFrom.OutSide);
            }
            GameObject itemObj = PoolManager.Instance.GetObject("Effect/Effect_ItemObj");
            itemObj.transform.position = itemNetObj.transform.position;
            itemObj.GetComponent<Effect_ItemObj>().DrawSpriter(itemNetObj.spriteRenderer_Icon.sprite);
            itemObj.GetComponent<Effect_ItemObj>().BindFollow(transform);
        }
    }
    /// <summary>
    /// RPC:���ض�ͣ��
    /// </summary>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_LocalInput_StandDown(short time)
    {
        if (actorManager_Local.actorAuthority.isState)
        {
            actorManager_Local.pathManager.State_StandDown(time);
        }
    }
    /// <summary>
    /// RPC:���ض�ת��
    /// </summary>
    /// <param name="right"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_LocalInput_TurnTo(bool right)
    {
        if (right) 
        {
            actorManager_Local.actionManager.TurnTo(Vector2.right);
        }
        else
        {
            actorManager_Local.actionManager.TurnTo(Vector2.left);
        }
    }
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPC_AllClient_Dead()
    {
        actorManager_Local.bodyController.Dead();
        actorManager_Local.actionManager.Dead();
    }
    #endregion
}
public enum HpChangeReason
{
    MagicDamage,
    AttackDamage,
    RealDamage,
    Healing,
}
