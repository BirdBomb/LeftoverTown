using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UniRx;
using UnityEngine.Windows;
using Fusion.Addons.Physics;
using System;
using UnityEngine.Accessibility;
/// <summary>
/// 角色网络管理器
/// </summary>
public class ActorNetManager : NetworkBehaviour
{
    [Header("物理同步组件")]
    public NetworkRigidbody2D networkRigidbody;
    [Header("本地角色组件")]
    public ActorManager actorManager_Local;
    public override void Spawned()
    {
        if (Object.HasStateAuthority) actorManager_Local.State_Init();
        actorManager_Local.Client_Init();
        Client_InitNetData();
        base.Spawned();
    }
    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        base.Despawned(runner, hasState);
    }
    public override void FixedUpdateNetwork()
    {
        actorManager_Local.AllClient_FixedUpdateNetwork(Runner.DeltaTime);
        if (actorManager_Local.actorAuthority.isState) actorManager_Local.State_FixedUpdateNetwork(Runner.DeltaTime);
        base.FixedUpdateNetwork();
    }
    #region//初始化
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

        Net_SpeedCommon = netData.Speed_Common;

        State_SendInfoToAll();
    }
    public void Client_InitNetData()
    {
        OnItemInHandChange();
        OnItemOnHeadChange();
        OnItemOnBodyChange();
    }
    public void Client_RequestInfo()
    {
        PlayerRef playerRef = Runner.LocalPlayer;
        RPC_Client_RequestInfo(playerRef);
    }
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    private void RPC_Client_RequestInfo(PlayerRef from)
    {
        SendInfo(from);
    }
    public void SendInfo(PlayerRef to)
    {
        RPC_State_SendFaceInfo(to, Local_Name, Local_Seed, Local_HairID, Local_EyeID, Local_HairColor);
        RPC_State_SendAbilityInfo(to, Local_HpMax, Local_FoodMax, Local_SanMax, Local_Coin, Local_Fine);
    }
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
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_State_SendAbilityInfo([RpcTarget] PlayerRef target, short maxHp, short maxFood, short maxSan, int coin, short fine)
    {
        Local_HpMax = maxHp;
        Local_FoodMax = maxFood;
        Local_SanMax = maxSan;
        Local_Coin = coin;
        Local_Fine = fine;
    }
    public void State_SendInfoToAll()
    {
        RPC_State_SendFaceInfoToAll(Local_Name, Local_Seed, Local_HairID, Local_EyeID, Local_HairColor);
        RPC_State_SendAbilityInfoToAll(Local_HpMax, Local_FoodMax, Local_SanMax, Local_Coin, Local_Fine);
    }
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
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_State_SendAbilityInfoToAll(short maxHp, short maxFood, short maxSan, int coin, short fine)
    {
        Local_HpMax = maxHp;
        Local_FoodMax = maxFood;
        Local_SanMax = maxSan;
        Local_Coin = coin;
        Local_Fine = fine;
    }
    #endregion
    #region//外貌和名字(只在本地端计算)
    public string Local_Name { get; set; } = "";
    public short Local_Seed { get; set; } = 0;
    public short Local_EyeID { get; set; } = 0;
    public short Local_HairID { get; set; } = 0;
    public Color32 Local_HairColor { get; set; } = Color.white;
    #endregion
    #region//生命值(所有客户端)
    /// <summary>
    /// 当前生命值
    /// </summary>
    [Networked, OnChangedRender(nameof(OnCurHpChange)), HideInInspector]
    public short Net_HpCur { get; set; }
    /// <summary>
    /// 最大生命值
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
    /// RPC:生命值改变
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="networkId"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_AllClient_HpChange(int parameter, NetworkId networkId)
    {
        actorManager_Local.AllClient_Listen_MyselfHpChange(parameter, networkId);
        if (actorManager_Local.actorAuthority.isState)
        {
            OnlyState_HpChange(parameter, networkId);
        }
    }
    private void OnlyState_HpChange(int parameter, NetworkId networkId)
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
            actorManager_Local.State_Listen_MyselfHpChange(parameter, networkId);
        }
    }
    /// <summary>
    /// RPC:最大生命值改变
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
    #region//饥饿值(所有客户端)
    /// <summary>
    /// 当前食物值
    /// </summary>
    [Networked, OnChangedRender(nameof(OnCurFoodChange)), HideInInspector]
    public short Net_FoodCur { get; set; }
    /// <summary>
    /// 最大食物值
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
    /// RPC:饥饿值改变
    /// </summary>
    /// <param name="val"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_FoodChange(short parameter)
    {
        OnlyState_FoodChange(parameter);
    }
    private void OnlyState_FoodChange(short val)
    {
        Net_FoodCur = val;
    }
    #endregion
    #region//精神值(所有客户端)
    /// <summary>
    /// 当前精神值
    /// </summary>
    [Networked, OnChangedRender(nameof(OnCurSanChange)), HideInInspector]
    public short Net_SanCur { get; set; }
    /// <summary>
    /// 最大精神值
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
    #endregion
    #region//速度(所有客户端)
    /// <summary>
    /// 普通速度(分米/秒)
    /// </summary>
    [Networked, HideInInspector]
    public short Net_SpeedCommon { get; set; }
    #endregion
    #region//护甲(所有客户端)
    /// <summary>
    /// 护甲
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
    #region//魔抗(所有客户端)
    /// <summary>
    /// 魔抗
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
    #region//手部物体(所有客户端)
    /// <summary>
    /// 手部物体
    /// </summary>
    [Networked, OnChangedRender(nameof(OnItemInHandChange)), HideInInspector]
    public ItemData Net_ItemInHand { get; set; }
    /// <summary>
    /// 身体物体
    /// </summary>
    public void OnItemInHandChange()
    {
        actorManager_Local.itemManager.UpdateItemInHand(Net_ItemInHand);
    }
    /// <summary>
    /// RPC:添加持握物体
    /// </summary>
    /// <param name="itemData_Add"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_AddItemOnHand(ItemData itemData_Add)
    {
        OnlyState_AddItemOnHand(itemData_Add);
    }
    public void OnlyState_AddItemOnHand(ItemData itemData_Add)
    {
        Net_ItemInHand = GameToolManager.Instance.CombineItem(Net_ItemInHand, itemData_Add, out ItemData itemData_Res);
        if (itemData_Res.Item_ID > 0 && itemData_Res.Item_Count > 0)
        {
            RPC_State_AddItemInBag(itemData_Res);
        }
    }
    /// <summary>
    /// RPC:减少持握物体
    /// </summary>
    /// <param name="itemData_Sub"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_SubItemOnHand(ItemData itemData_Sub)
    {
        OnlyState_SubItemOnHand(itemData_Sub);
    }
    public void OnlyState_SubItemOnHand(ItemData itemData_Sub)
    {
        Net_ItemInHand = GameToolManager.Instance.SplitItem(Net_ItemInHand, itemData_Sub);
    }
    /// <summary>
    /// RPC:修改持握物品
    /// </summary>
    /// <param name="itemData"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_ChangeItemOnHand(ItemData itemData_Old, ItemData itemData_New)
    {
        OnlyState_ChangeItemOnHand(itemData_Old, itemData_New);
    }
    public void OnlyState_ChangeItemOnHand(ItemData itemData_Old, ItemData itemData_New)
    {
        if (Net_ItemInHand.Equals(itemData_Old))
        {
            Net_ItemInHand = itemData_New;
        }
    }

    #endregion
    #region//头部物体(所有客户端)
    /// <summary>
    /// 头部物体
    /// </summary>
    [Networked, OnChangedRender(nameof(OnItemOnHeadChange)), HideInInspector]
    public ItemData Net_ItemOnHead { get; set; }
    public void OnItemOnHeadChange()
    {
        actorManager_Local.itemManager.UpdateItemOnHead(Net_ItemOnHead);
    }
    /// <summary>
    /// RPC:添加头戴物体
    /// </summary>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_AddItemOnHead(ItemData itemData_Add)
    {
        OnlyState_AddItemOnHead(itemData_Add);
    }
    public void OnlyState_AddItemOnHead(ItemData itemData_Add)
    {
        Net_ItemOnHead = GameToolManager.Instance.CombineItem(Net_ItemOnHead, itemData_Add, out ItemData itemData_Res);
        if (itemData_Res.Item_ID > 0 && itemData_Res.Item_Count > 0)
        {
            RPC_State_AddItemInBag(itemData_Res);
        }
    }
    /// <summary>
    /// RPC:减少头戴物体
    /// </summary>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_SubItemOnHead(ItemData itemData_Sub)
    {
        OnlyState_SubItemOnHead(itemData_Sub);
    }
    public void OnlyState_SubItemOnHead(ItemData itemData_Sub)
    {
        Net_ItemOnHead = GameToolManager.Instance.SplitItem(Net_ItemOnHead, itemData_Sub);
    }
    /// <summary>
    /// RPC:更改头戴物体
    /// </summary>
    /// <param name="itemData"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_ChangeItemOnHead(ItemData itemData_Old, ItemData itemData_New)
    {
        OnlyState_ChangeItemOnHead(itemData_Old, itemData_New);
    }
    public void OnlyState_ChangeItemOnHead(ItemData oldItemData, ItemData newItemData)
    {
        if (Net_ItemOnHead.Equals(oldItemData))
        {
            Net_ItemOnHead = newItemData;
        }
    }

    #endregion
    #region//身体物体(所有客户端)
    [Networked, OnChangedRender(nameof(OnItemOnBodyChange)), HideInInspector]
    public ItemData Net_ItemOnBody { get; set; }
    public void OnItemOnBodyChange()
    {
        actorManager_Local.itemManager.UpdateItemOnBody(Net_ItemOnBody);
    }
    /// <summary>
    /// RPC:添加穿着物体
    /// </summary>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_AddItemOnBody(ItemData itemData_Add)
    {
        OnlyState_AddItemOnBody(itemData_Add);
    }
    public void OnlyState_AddItemOnBody(ItemData itemData_Add)
    {
        Net_ItemOnBody = GameToolManager.Instance.CombineItem(Net_ItemOnBody, itemData_Add, out ItemData itemData_Res);
        if (itemData_Res.Item_ID > 0 && itemData_Res.Item_Count > 0)
        {
            RPC_State_AddItemInBag(itemData_Res);
        }
    }
    /// <summary>
    /// RPC:减少穿着物体
    /// </summary>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_SubItemOnBody(ItemData itemData_Sub)
    {
        OnlyState_SubItemOnBody(itemData_Sub);
    }
    public void OnlyState_SubItemOnBody(ItemData itemData_Sub)
    {
        Net_ItemOnBody = GameToolManager.Instance.SplitItem(Net_ItemOnBody, itemData_Sub);
    }
    /// <summary>
    /// RPC:修改穿着物体
    /// </summary>
    /// <param name="itemData"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_ChangeItemOnBody(ItemData itemData_Old, ItemData itemData_New)
    {
        OnlyState_ChangeItemOnBody(itemData_Old, itemData_New);
    }
    public void OnlyState_ChangeItemOnBody(ItemData itemData_Old, ItemData itemData_New)
    {
        if (Net_ItemOnBody.Equals(itemData_Old))
        {
            Net_ItemOnBody = itemData_New;
        }
    }

    #endregion
    #region//金币(所有客户端)
    /// <summary>
    /// 金币
    /// </summary>
    public int Local_Coin { get; set; }
    /// <summary>
    /// 本地端支付
    /// </summary>
    /// <param name="val"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_Local_PayCoin(int val)
    {
        RPC_State_UpdateCoin(Local_Coin - val);
    }
    /// <summary>
    /// 本地端赚钱
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
    #region//赏金(所有客户端)
    /// <summary>
    /// 悬赏
    /// </summary>
    public short Local_Fine { get; set; }
    /// <summary>
    /// 本地端设置赏金
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
    #region//背包物体(只在本地端计算)
    private List<ItemData> Local_BagItems { get; } = new List<ItemData>(15);
    /// <summary>
    /// 获得背包物体
    /// </summary>
    /// <returns></returns>
    public List<ItemData> Local_GetBagItem()
    {
        return new List<ItemData>(Local_BagItems);
    }
    /// <summary>
    /// 设置背包物体
    /// </summary>
    /// <param name="itemDatas"></param>
    public void Local_SetBagItem(List<ItemData> itemDatas)
    {
        Local_BagItems.Clear();
        for (int i = 0; i < itemDatas.Count; i++)
        {
            Local_BagItems.Add(itemDatas[i]);
        }
        if (actorManager_Local.actorAuthority.isPlayer && actorManager_Local.actorAuthority.isLocal)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateItemInBag()
            {
                bagCapacity = 15,
                itemDatas = Local_GetBagItem()
            });
        }
    }
    /// <summary>
    /// RPC:添加背包物体
    /// </summary>
    /// <param name="itemData"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPC_State_AddItemInBag(ItemData addData)
    {
        if (actorManager_Local.actorAuthority.isLocal)
        {
            Local_AddItemInBag(0, addData);
        }
    }
    public void Local_AddItemInBag(int index, ItemData addData)
    {
        if (addData.Item_ID > 0)
        {
            List<ItemData> itemDatas = Local_GetBagItem();
            GameToolManager.Instance.PutInItemList(itemDatas, addData, index, 15, out ItemData resData);
            Local_SetBagItem(itemDatas);
            if (resData.Item_Count > 0)
            {
                MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnItem()
                {
                    pos = transform.position,
                    itemData = resData
                });
            }
            if (actorManager_Local.actorAuthority.isPlayer)
            {
                MessageBroker.Default.Publish(new UIEvent.UIEvent_PutItemInBag()
                {
                    item = addData
                });
            }
        }
    }
    /// <summary>
    /// RPC:消耗背包物品
    /// </summary>
    /// <param name="itemData"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPC_State_ExpendItemInBag(int id, int count)
    {
        if (actorManager_Local.actorAuthority.isLocal)
        {
            Local_ExpendItemInBag(id, count);
        }
    }
    public void Local_ExpendItemInBag(int id, int count)
    {
        if (id > 0 && count > 0)
        {
            List<ItemData> itemDatas = Local_GetBagItem();
            GameToolManager.Instance.ExpendItemList(itemDatas, id, count);
            Local_SetBagItem(itemDatas);
        }
    }
    /// <summary>
    /// RPC:修改背包物体
    /// </summary>
    /// <param name="itemData"></param>
    /// <param name="index"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPC_State_ChangeItemInBag(int index, ItemData itemData)
    {
        if (actorManager_Local.actorAuthority.isLocal)
        {
            Local_ChangeItemInBag(index, itemData);
        }
    }
    public void Local_ChangeItemInBag(int index, ItemData itemData)
    {
        List<ItemData> itemDatas = Local_GetBagItem();
        if (actorManager_Local.actorAuthority.isPlayer)
        {
            if (index < itemDatas.Count)
            {
                if (itemDatas[index].Item_ID == itemData.Item_ID)
                {
                    if(itemDatas[index].Item_Count < itemData.Item_Count)
                    {
                        ItemData temp = itemData;
                        temp.Item_Count = (short)(itemData.Item_Count - itemDatas[index].Item_Count);
                        MessageBroker.Default.Publish(new UIEvent.UIEvent_PutItemInBag()
                        {
                            item = temp
                        });
                    }
                    else if(itemDatas[index].Item_Count > itemData.Item_Count)
                    {
                        ItemData temp = itemData;
                        temp.Item_Count = (short)(itemDatas[index].Item_Count - itemData.Item_Count);
                        MessageBroker.Default.Publish(new UIEvent.UIEvent_PutItemOutBag()
                        {
                            item = temp
                        });
                    }
                }
                else
                {
                    MessageBroker.Default.Publish(new UIEvent.UIEvent_PutItemOutBag()
                    {
                        item = itemDatas[index]
                    });
                    MessageBroker.Default.Publish(new UIEvent.UIEvent_PutItemInBag()
                    {
                        item = itemData
                    });
                }
            }
        }

        GameToolManager.Instance.ChangeItemList(itemDatas, itemData, index);
        Local_SetBagItem(itemDatas);
    }
    #endregion
    #region//角色技能操作
    /// <summary>
    /// 更改攻击状态
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
    /// 更改攻击目标
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
    /// 使用技能
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="networkId"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPC_State_NpcUseSkill(int id, Vector3Int vector3, NetworkId networkId)
    {
        actorManager_Local.AllClient_Listen_NpcAction(id, vector3, networkId);
    }

    #endregion
    #region//角色位移
    /// <summary>
    /// 本地端更改玩家位置
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
    #endregion
    #region//其他操作
    /// <summary>
    /// 本地端发送表情
    /// </summary>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_LocalInput_SendEmoji(int id)
    {
        actorManager_Local.actionManager.SendEmoji(id);
    }
    /// <summary>
    /// 本地端发送语言
    /// </summary>
    /// <param name="text"></param>
    /// <param name="id"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_LocalInput_SendText(string text, int id)
    {
        actorManager_Local.actionManager.SendText(text, id);
    }
    /// <summary>
    /// RPC:本地端拾起物品
    /// </summary>
    /// <param name="id"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_LocalInput_PickItem(NetworkId id)
    {
        void OnlyState_PickItem(string val)
        {
            if (actorManager_Local.actorAuthority.isPlayer && actorManager_Local.actorAuthority.isLocal)
            {
                AudioManager.Instance.Play2DEffect(1003);
            }
            if (Object.HasStateAuthority && val.Equals("Pick"))
            {
                NetworkObject networkPlayerObject = Runner.FindObject(id);
                networkPlayerObject.GetComponent<ItemNetObj>().PickUp(out ItemData itemData_Pick);
                Runner.Despawn(networkPlayerObject);
                RPC_State_AddItemInBag(itemData_Pick);
            }
        }
        actorManager_Local.actionManager.PlayPickUp(1, OnlyState_PickItem);
    }
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPC_AllClient_Dead()
    {
        actorManager_Local.bodyController.Dead();
        actorManager_Local.actionManager.Dead();
    }
    #endregion

}
