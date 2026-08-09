using Fusion;
using Fusion.Addons.Physics;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
/// <summary>
/// 角色网络管理器
/// </summary>
public class ActorNetManager : NetworkBehaviour
{
    [Header("同步物理组件")]
    public NetworkRigidbody2D networkRigidbody;
    [Header("本地物理组件")]
    public Rigidbody2D localRigidbody;
    [Header("刚体碰撞器")]
    public CircleCollider2D circleCollider2D;
    [Header("触发碰撞器")]
    public BoxCollider2D boxCollider2D;
    /// <summary>
    /// 开启本地权威
    /// </summary>
    [Header("开启本地权威")]
    public bool local_Simulation = false;
    private float float_LockTimer = 0;

    private float float_SimulationTimer = 0;
    private const float float_SyncCD = 0.2f; 
    private const float float_SyncPositionTolerance = 0.25f;
    [Header("本地角色组件")]
    public ActorManager actorManager_Local;
    public override void Spawned()
    {
        actorManager_Local.AllClient_BindConfig();
        if (Object.HasStateAuthority) actorManager_Local.State_Init();
        actorManager_Local.AllClient_Init();
        AllClient_InitNetData();
        WorldActorManager.Instance.AddActor(actorManager_Local);
        base.Spawned();
    }
    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        WorldActorManager.Instance.SubActor(actorManager_Local);
        base.Despawned(runner, hasState);
    }
    public override void FixedUpdateNetwork()
    {
        if (actorManager_Local.actorAuthority.isState) actorManager_Local.State_FixedUpdateNetwork(Runner.DeltaTime);
        base.FixedUpdateNetwork();
    }
    /// <summary>
    /// 本地端移动预测
    /// </summary>
    public void Local_PlaySimulation(bool on)
    {
        local_Simulation = on;
        if (on)
        {
            if (networkRigidbody != null) networkRigidbody.enabled = false;
            if (localRigidbody != null)
            {
                localRigidbody.isKinematic = false;
                localRigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            }
        }
        else
        {
            if (networkRigidbody != null) networkRigidbody.enabled = true;
        }
    }
    public void Local_ChangeCollider(bool on)
    {
        circleCollider2D.enabled = on;
    }
    #region//(非本地)初始化>>>请求数据
    /// <summary>
    /// 客户端初始化数据
    /// </summary>
    public void AllClient_InitNetData()
    {
        AllClient_RequestInfo();
        AllClient_ItemHand_Broadcast();
        AllClient_ItemHead_Broadcast();
        AllClient_ItemBody_Broadcast();
        AllClient_ItemAccessory_Broadcast();
        AllClient_ItemConsumables_Broadcast();
    }
    /// <summary>
    /// 客户端向服务器请求数据
    /// </summary>
    public void AllClient_RequestInfo()
    {
        PlayerRef playerRef = Runner.LocalPlayer;
        RPC_Client_RequestInfo(playerRef);
    }
    /// <summary>
    /// 客户端向服务器请求数据
    /// </summary>
    /// <param name="from"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    private void RPC_Client_RequestInfo(PlayerRef from)
    {
        State_SendInfo(from);
    }
    /// <summary>
    /// [向特定客户端]服务器向客户端发送数据
    /// </summary>
    /// <param name="to"></param>
    public void State_SendInfo(PlayerRef to)
    {
        RPC_State_SendFaceInfo(to, Local_Name, Local_Seed, Local_HairID, Local_EyeID, Local_HairColor);
        RPC_State_SendAbilityInfo(to, Local_HpMax, Local_FoodMax, Local_SanMax, Local_Coin, Local_Fine);
    }
    /// <summary>
    /// [向特定客户端]发送本地外貌数据
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
    }
    /// <summary>
    /// [向特定客户端]发送本地属性数据
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
    #region//(本地玩家)初始化>>>发送数据
    /// <summary>
    /// 向服务器发送本地数据
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
    /// 服务器向客户端发送数据
    /// </summary>
    public void State_SendInfoToAll()
    {
        RPC_State_SendFaceInfoToAll(Local_Name, Local_Seed, Local_HairID, Local_EyeID, Local_HairColor);
        RPC_State_SendAbilityInfoToAll(Local_HpMax, Local_FoodMax, Local_SanMax, Local_Coin, Local_Fine);
    }
    /// <summary>
    /// 发送本地外貌数据
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
    }
    /// <summary>
    /// 发送本地属性数据
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
        float barVal = Net_HpCur <= 0 ? 0 : (float)Net_HpCur / Local_HpMax;
        actorManager_Local.AllClient_UpdateHpBar(barVal); 
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
    public void RPC_AllClient_HpChange(int parameter, int reason, NetworkId networkId)
    {
        actorManager_Local.ForAll_Listen_MyselfHpChange(parameter, (HpChangeReason)reason, networkId);
        if (actorManager_Local.actorAuthority.isState)
        {
            OnlyState_HpChange(parameter, reason, networkId);
        }
    }
    private void OnlyState_HpChange(int parameter, int reason, NetworkId networkId)
    {
        int hp = Net_HpCur + parameter;

        if (hp > 0)
        {
            Net_HpCur = hp > Local_HpMax ? Local_HpMax : (short)(Net_HpCur + parameter);
            if (parameter < 0) actorManager_Local.ForState_Listen_MyselfInjured(parameter, (HpChangeReason)reason, networkId);
        }
        else
        {
            Net_HpCur = 0; Net_SpeedCommon = 0;
            actorManager_Local.ForState_Listen_MyselfDead(parameter, (HpChangeReason)reason, networkId);
            RPC_AllClient_Dead();
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
        if (actorManager_Local.actorAuthority.isLocal)
        {
            if (actorManager_Local.actorAuthority.isPlayer)
            {
                MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateFoodData()
                {
                    Food_Cur = Net_FoodCur,
                    Food_Max = Local_FoodMax,
                });
            }
        }
        actorManager_Local.buffManager.Listen_UpdateHungry();
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
        actorManager_Local.buffManager.Listen_UpdateSan();
    }
    /// <summary>
    /// RPC:精神值改变
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
        Net_Armor = val;
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
        Net_Resistance = val;
    }
    #endregion
    #region//载具(所有客户端)
    [Networked, OnChangedRender(nameof(OnVehicleChange)), HideInInspector]
    public NetworkId Net_Vehicle { get; set; }
    public void OnVehicleChange()
    {
        if (Runner.FindObject(Net_Vehicle)&& Runner.FindObject(Net_Vehicle).TryGetComponent(out ActorManager actor))
        {
            actorManager_Local.vehicleManager.AllClient_SetVehicle(actor);
        }
        else
        {
            actorManager_Local.vehicleManager.AllClient_CleanVehicle();
        }
    }

    #endregion

    #region//物体持握(所有客户端)
    private ItemData local_ItemHand;
    public ItemData Local_ItemHand 
    {
        get { return local_ItemHand; }
        set 
        {
            if (!local_ItemHand.FullyEqual(value))
            {
                local_ItemHand = value;
                actorManager_Local.itemManager.UpdateItemHand(local_ItemHand);
                if (actorManager_Local.actorAuthority.isLocal)  { RPC_LocalInput_ItemHand_Upload(local_ItemHand); }
            }
        }
    }
    public void Local_ItemHand_Add(ItemData itemData)
    {
        Local_ItemHand = GameToolManager.Instance.CombineItem(Local_ItemHand, itemData, out ItemData itemData_Res);
        if (itemData_Res.I > 0 && itemData_Res.C > 0)
        {
            RPC_State_ItemInBag_Add(itemData_Res, (short)ItemFrom.Hand);
        }
    }
    public void Local_ItemHand_Sub(ItemData itemData)
    {
        Local_ItemHand = GameToolManager.Instance.SplitItem(Local_ItemHand, itemData);
    }
    public void Local_ItemHand_Change(ItemData itemData_Old, ItemData itemData_New)
    {
        if (Local_ItemHand.Equals(itemData_Old)) Local_ItemHand = itemData_New;
    }
    /// <summary>
    /// 上传本地手持物体
    /// </summary>
    /// <param name="itemData"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_ItemHand_Upload(ItemData itemData) 
    {
        Net_ItemHand = itemData;
    }
    /// <summary>
    /// 手部物体
    /// </summary>
    [Networked, OnChangedRender(nameof(AllClient_ItemHand_Broadcast)), HideInInspector]
    public ItemData Net_ItemHand { get; set; }
    /// <summary>
    /// 将服务器信息广播给除了本地端之外的客户端
    /// </summary>
    public void AllClient_ItemHand_Broadcast()
    {
        if (!actorManager_Local.actorAuthority.isLocal) { Local_ItemHand = Net_ItemHand; }
    }
    #endregion
    #region//物体帽子(所有客户端)
    private ItemData loacl_ItemHead;
    public ItemData Local_ItemHead
    {
        get { return loacl_ItemHead; }
        set
        {
            if (!loacl_ItemHead.FullyEqual(value))
            {
                loacl_ItemHead = value;
                actorManager_Local.itemManager.UpdateItemHead(loacl_ItemHead);
                if (actorManager_Local.actorAuthority.isLocal) { RPC_LocalInput_ItemHead_Upload(loacl_ItemHead); }
            }
        }
    }
    public void Local_ItemHead_Add(ItemData itemData)
    {
        Local_ItemHead = GameToolManager.Instance.CombineItem(Local_ItemHead, itemData, out ItemData itemData_Res);
        if (itemData_Res.I > 0 && itemData_Res.C > 0)
        {
            RPC_State_ItemInBag_Add(itemData_Res, (short)ItemFrom.Head);
        }
    }
    public void Local_ItemHead_Sub(ItemData itemData)
    {
        Local_ItemHead = GameToolManager.Instance.SplitItem(Local_ItemHead, itemData);
    }
    public void Local_ItemHead_Change(ItemData itemData_Old, ItemData itemData_New)
    {
        if (Local_ItemHead.Equals(itemData_Old)) Local_ItemHead = itemData_New; 
    }
    /// <summary>
    /// 上传本地手持物体
    /// </summary>
    /// <param name="itemData"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_ItemHead_Upload(ItemData itemData)
    {
        Net_ItemHead = itemData;
    }
    /// <summary>
    /// 头部物体
    /// </summary>
    [Networked, OnChangedRender(nameof(AllClient_ItemHead_Broadcast)), HideInInspector]
    public ItemData Net_ItemHead { get; set; }
    public void AllClient_ItemHead_Broadcast()
    {
        if (!actorManager_Local.actorAuthority.isLocal) { Local_ItemHead = Net_ItemHead; }
    }

    #endregion
    #region//物体衣物(所有客户端)
    private ItemData loacl_ItemBody;
    public ItemData Local_ItemBody
    {
        get { return loacl_ItemBody; }
        set
        {
            if (!loacl_ItemBody.FullyEqual(value))
            {
                loacl_ItemBody = value;
                actorManager_Local.itemManager.UpdateItemBody(loacl_ItemBody);
                if (actorManager_Local.actorAuthority.isLocal) { RPC_LocalInput_ItemBody_Upload(loacl_ItemBody); }
            }
        }
    }
    public void Local_ItemBody_Add(ItemData itemData)
    {
        Local_ItemBody = GameToolManager.Instance.CombineItem(Local_ItemBody, itemData, out ItemData itemData_Res);
        if (itemData_Res.I > 0 && itemData_Res.C > 0)
        {
            RPC_State_ItemInBag_Add(itemData_Res, (short)ItemFrom.Body);
        }
    }
    public void Local_ItemBody_Sub(ItemData itemData)
    {
        Local_ItemBody = GameToolManager.Instance.SplitItem(Local_ItemBody, itemData);
    }
    public void Local_ItemBody_Change(ItemData itemData_Old, ItemData itemData_New)
    {
        if (Local_ItemBody.Equals(itemData_Old)) Local_ItemBody = itemData_New;
    }
    /// <summary>
    /// 上传本地手持物体
    /// </summary>
    /// <param name="itemData"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_ItemBody_Upload(ItemData itemData)
    {
        Net_ItemBody = itemData;
    }


    [Networked, OnChangedRender(nameof(AllClient_ItemBody_Broadcast)), HideInInspector]
    public ItemData Net_ItemBody { get; set; }
    public void AllClient_ItemBody_Broadcast()
    {
        if (!actorManager_Local.actorAuthority.isLocal) { Local_ItemBody = Net_ItemBody; }
    }
    #endregion
    #region//物体饰品(所有客户端)
    private ItemData loacl_ItemAccessory;
    public ItemData Local_ItemAccessory
    {
        get { return loacl_ItemAccessory; }
        set
        {
            if (!loacl_ItemAccessory.FullyEqual(value))
            {
                loacl_ItemAccessory = value;
                actorManager_Local.itemManager.UpdateItemAccessory(loacl_ItemAccessory);
                if (actorManager_Local.actorAuthority.isLocal) { RPC_LocalInput_ItemAccessory_Upload(loacl_ItemAccessory); }
            }
        }
    }
    public void Local_ItemAccessory_Add(ItemData itemData)
    {
        Local_ItemAccessory = GameToolManager.Instance.CombineItem(Local_ItemAccessory, itemData, out ItemData itemData_Res);
        if (itemData_Res.I > 0 && itemData_Res.C > 0)
        {
            RPC_State_ItemInBag_Add(itemData_Res, (short)ItemFrom.Accessory);
        }
    }
    public void Local_ItemAccessory_Sub(ItemData itemData)
    {
        Local_ItemAccessory = GameToolManager.Instance.SplitItem(Local_ItemAccessory, itemData);
    }
    public void Local_ItemAccessory_Change(ItemData itemData_Old, ItemData itemData_New)
    {
        if (Local_ItemAccessory.Equals(itemData_Old)) Local_ItemAccessory = itemData_New;
    }
    /// <summary>
    /// 上传本地手持物体
    /// </summary>
    /// <param name="itemData"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_ItemAccessory_Upload(ItemData itemData)
    {
        Net_ItemAccessory = itemData;
    }
    [Networked, OnChangedRender(nameof(AllClient_ItemAccessory_Broadcast)), HideInInspector]
    public ItemData Net_ItemAccessory { get; set; }
    public void AllClient_ItemAccessory_Broadcast()
    {
        if (!actorManager_Local.actorAuthority.isLocal) { Local_ItemAccessory = Net_ItemAccessory; }
    }
    #endregion
    #region//物体耗材(所有客户端)
    private ItemData loacl_ItemConsumables;
    public ItemData Local_ItemConsumables
    {
        get { return loacl_ItemConsumables; }
        set
        {
            if (!loacl_ItemConsumables.FullyEqual(value))
            {
                loacl_ItemConsumables = value;
                actorManager_Local.itemManager.UpdateItemConsumables(loacl_ItemConsumables);
                if (actorManager_Local.actorAuthority.isLocal) { RPC_LocalInput_ItemConsumables_Upload(loacl_ItemConsumables); }
            }
        }
    }
    public void Local_ItemConsumables_Add(ItemData itemData)
    {
        Local_ItemConsumables = GameToolManager.Instance.CombineItem(Local_ItemConsumables, itemData, out ItemData itemData_Res);
        if (itemData_Res.I > 0 && itemData_Res.C > 0)
        {
            RPC_State_ItemInBag_Add(itemData_Res, (short)ItemFrom.Consumables);
        }
    }
    public void Local_ItemConsumables_Sub(ItemData itemData)
    {
        Local_ItemConsumables = GameToolManager.Instance.SplitItem(Local_ItemConsumables, itemData);
    }
    public void Local_ItemConsumables_Change(ItemData itemData_Old, ItemData itemData_New)
    {
        if (Local_ItemConsumables.Equals(itemData_Old)) Local_ItemConsumables = itemData_New;
    }
    /// <summary>
    /// 上传本地手持物体
    /// </summary>
    /// <param name="itemData"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_ItemConsumables_Upload(ItemData itemData)
    {
        Net_ItemConsumables = itemData;
    }
    [Networked, OnChangedRender(nameof(AllClient_ItemConsumables_Broadcast)), HideInInspector]
    public ItemData Net_ItemConsumables { get; set; }
    public void AllClient_ItemConsumables_Broadcast()
    {
        if (!actorManager_Local.actorAuthority.isLocal) { Local_ItemConsumables = Net_ItemConsumables; }
    }
    #endregion

    #region//物体背包(只在本地端计算)
    public int Local_BagCapacity = 40;
    public int Local_BagItemCount = 0;
    private List<ItemData> Local_ItemBag { get; } = new List<ItemData>(40);
    /// <summary>
    /// 获得背包物体
    /// </summary>
    /// <returns></returns>
    public List<ItemData> Local_ItemBag_Get()
    {
        return new List<ItemData>(Local_ItemBag);
    }
    /// <summary>
    /// 设置背包物体
    /// </summary>
    /// <param name="itemDatas"></param>
    public void Local_ItemBag_Set(List<ItemData> itemDatas)
    {
        Local_ItemBag.Clear();
        Local_BagItemCount = 0;
        for (int i = 0; i < itemDatas.Count; i++)
        {
            if (itemDatas[i].I != 0) { Local_BagItemCount++; }
            Local_ItemBag.Add(itemDatas[i]);
        }
        if (actorManager_Local.actorAuthority.isPlayer && actorManager_Local.actorAuthority.isLocal)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateItemInBag()
            {
                itemDatas = Local_ItemBag_Get(),
                itemCount = Local_BagItemCount
            });
        }
    }
    /// <summary>
    /// RPC:添加背包物体
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
        if (addData.I > 0)
        {
            List<ItemData> itemDatas = Local_ItemBag_Get();
            GameToolManager.Instance.PutInItemList(itemDatas, addData, index, Local_BagCapacity, out ItemData resData);
            Local_ItemBag_Set(itemDatas);
            if (resData.C > 0)
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
    /// RPC:消耗背包物品
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
    /// RPC:修改背包物体
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
                if (itemDatas[index].I == itemData.I)
                {
                    if (itemDatas[index].C < itemData.C)
                    {
                        ItemData temp = itemData;
                        temp.C = (short)(itemData.C - itemDatas[index].C);
                        //MessageBroker.Default.Publish(new UIEvent.UIEvent_PutItemInBag()
                        //{
                        //    item = temp
                        //});
                    }
                    else if (itemDatas[index].C > itemData.C)
                    {
                        ItemData temp = itemData;
                        temp.C = (short)(itemDatas[index].C - itemData.C);
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

    #region//金币(只在本地端计算)

    private int _localCoin;
    /// <summary>
    /// 金币
    /// </summary>
    public int Local_Coin
    {
        get => _localCoin;
        set
        {
            _localCoin = value;
            if (actorManager_Local.actorAuthority.isPlayer && actorManager_Local.actorAuthority.isLocal)
            {
                MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateCoinData()
                {
                    Coin = _localCoin,
                });
            }
        }
    }
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
    }

    #endregion
    #region//赏金(只在本地端计算)
    /// <summary>
    /// 悬赏
    /// </summary>
    public int Local_Fine { get; set; }
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
    #region//Buff(只在本地端计算)
    public List<BuffData> Local_GetBuffList()
    {
        actorManager_Local.buffManager.Local_GetBuffList(out List<BuffData> buffdata);
        return buffdata;
    }
    public void Local_SetBuffList(List<BuffData> buffDatas)
    {
        actorManager_Local.buffManager.Local_InitBuffs(buffDatas);
    }
    public void Local_AddBuff(short buffData, short buffVal,Vector3Int buffPos)
    {
        actorManager_Local.buffManager.Local_AddBuff(new BuffData(buffData, buffVal, buffPos));
    }
    public void Local_RemoveBuff(short buffID)
    {
        actorManager_Local.buffManager.Local_RemoveBuff(buffID);
    }
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_Local_PlayBuffEffect(short ID,short index)
    {
        actorManager_Local.buffManager.All_PlayBuffEffect(ID,index);
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
        actorManager_Local.ForAll_Listen_ChangeAttackState(attacking);
    }
    /// <summary>
    /// 更改攻击目标
    /// </summary>
    /// <param name="id"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPC_State_NpcChangeAttackTarget(NetworkId id)
    {
        actorManager_Local.ForAll_Listen_ChangeAttackTarget(id);
    }

    /// <summary>
    /// 更改威胁目标
    /// </summary>
    /// <param name="id"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPC_State_NpcChangeThreatenedTarget(NetworkId id)
    {
        actorManager_Local.ForAll_Listen_ChangeThreatenedTarget(id);
    }

    /// <summary>
    /// 使用技能
    /// </summary>
    /// <param name="id"></param>
    /// <param name="vector3"></param>
    /// <param name="networkId"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPC_State_NpcUseSkill(int id, Vector3Int vector3, NetworkId networkId)
    {
        actorManager_Local.ForAll_Listen_NpcAction(id, vector3, networkId);
    }
    /// <summary>
    /// 使用技能
    /// </summary>
    /// <param name="id"></param>
    /// <param name="vector3"></param>
    /// <param name="networkId"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_Local_NpcUseSkill(int id, Vector3Int vector3, NetworkId networkId)
    {
        actorManager_Local.ForAll_Listen_NpcAction(id, vector3, networkId);
    }
    #endregion
    #region//角色位移
    /// <summary>
    /// 本地端更改玩家位置
    /// </summary>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_Local_SetNetworkTransform(Vector3 pos)
    {
        State_SetNetworkRigidbody(pos);
    }
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_Local_SyncNetworkTransform(Vector3 pos)
    {
        if (Vector3.Distance(pos, transform.position) > 0.5f)
        {
            State_SetNetworkRigidbody(pos);
        }
    }
    /// <summary>
    /// 网络权威设置位置
    /// </summary>
    /// <param name="pos"></param>
    private void State_SetNetworkRigidbody(Vector3 pos)
    {
        float_LockTimer = 0.05f;
        if (!local_Simulation)
        {
            if (networkRigidbody != null)
            {
                networkRigidbody.Rigidbody.velocity = Vector2.zero;
                networkRigidbody.Rigidbody.position = (pos);
                actorManager_Local.bodyController.Local_ResetPos(pos);
            }
        }
        else
        {
            localRigidbody.velocity = Vector2.zero;
            localRigidbody.MovePosition(pos);
        }
    }
    private Dictionary<ExternalForceType, Vector2> dic_ExternalForce = new Dictionary<ExternalForceType, Vector2>();
    public Vector3 dic_ResultantForce { get; set; }
    public enum ExternalForceType { Pull, Wind, MySleft }
    public void State_SetExternalForce(Vector2 forceDir, ExternalForceType forceType)
    {
        dic_ExternalForce[forceType] = forceDir;
        dic_ResultantForce = Vector2.zero;
        foreach (var kvp in dic_ExternalForce)
        {
            dic_ResultantForce += (Vector3)kvp.Value;
        }
        
    }
    public void State_CleanExternalForce(ExternalForceType forceType)
    {
        dic_ExternalForce.Remove(forceType);
        dic_ResultantForce = Vector3.zero;
    }
    /// <summary>
    /// 网络权威移动位置
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="dt"></param>
    public Vector3 State_MoveNetworkRigidbody(Vector2 dir, float dt)
    {
        dir = dir.normalized;
        float speed = actorManager_Local.actionManager.Client_GetSpeed();
        Vector3 velocity = new Vector3(dir.x * speed, dir.y * speed, 0);
        Vector3 pos = transform.position + (velocity + dic_ResultantForce) * dt;
        State_UpdateNetworkRigidbody(pos, speed, dt);
        return pos;
    }
    /// <summary>
    /// 本地模拟移动位置
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="dt"></param>
    /// <returns></returns>
    public Vector3 Local_MoveNetworkRigidbody(Vector2 dir, float dt) 
    {
        dir = dir.normalized;
        float speed = actorManager_Local.actionManager.Client_GetSpeed();
        Vector3 velocity = new Vector2(dir.x * speed, dir.y * speed);
        Vector3 pos = transform.position + (velocity + dic_ResultantForce) * dt;
        Local_UpdateSimulationRigidbody(pos, velocity.magnitude, dt);
        return pos;
    }

    /// <summary>
    /// 网络权威更新位置
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="speed"></param>
    /// <param name="dt"></param>
    public void State_UpdateNetworkRigidbody(Vector3 pos, float speed, float dt)
    {
        if (!local_Simulation && Object.HasStateAuthority)
        {
            if (float_LockTimer > 0) { float_LockTimer -= dt; return; }
            if (networkRigidbody && networkRigidbody.Rigidbody.velocity.magnitude <= speed)
            {
                networkRigidbody.Rigidbody.velocity = Vector2.zero;
                networkRigidbody.Rigidbody.position = (pos);
            }
        }
    }
    /// <summary>
    /// 本地模拟更新位置
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="speed"></param>
    /// <param name="dt"></param>
    public void Local_UpdateSimulationRigidbody(Vector3 pos, float speed, float dt)
    {
        if (local_Simulation)
        {
            if (float_LockTimer > 0) { float_LockTimer -= dt; return; }
            if (localRigidbody && localRigidbody.velocity.magnitude <= speed)
            {
                localRigidbody.velocity = Vector2.zero;
                localRigidbody.MovePosition(pos);
            }
            Local_TryToSync(pos, dt);
        }
    }
    public void Local_TryToSync(Vector3 pos, float dt)
    {
        if (float_SimulationTimer > float_SyncCD)
        {
            float_SimulationTimer = 0;
            RPC_Local_SyncNetworkTransform(pos);
        }
        else
        {
            float_SimulationTimer += dt;
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
        if (networkRigidbody)
        {
            if (networkRigidbody.Rigidbody.velocity == Vector2.zero)
            {
                networkRigidbody.Rigidbody.velocity = dir * force;
            }
            else
            {
                if (Vector2.Dot(networkRigidbody.Rigidbody.velocity, dir) > 0.2)
                {
                    /*同向--取大*/
                    if ((dir * force).magnitude > networkRigidbody.Rigidbody.velocity.magnitude)
                    {
                        networkRigidbody.Rigidbody.velocity = dir * force;
                    }
                }
                else
                {
                    /*异向--抵消*/
                    networkRigidbody.Rigidbody.velocity += dir * force;
                }
            }
        }
    }
    #endregion
    #region//角色动作
    /// <summary>
    /// 身体动作
    /// </summary>
    /// <param name="id">动作ID</param>
    /// <param name="speed">动作速度</param>
    /// <param name="pos">动作绑定位置</param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_Local_SetBodyAction(short id, float speed, Vector2 pos)
    {
        actorManager_Local.bodyController.PlayBodyAction((BodyActionType)id, speed, pos);
    }
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_Local_SetHeadAction(short id, float speed)
    {
        actorManager_Local.bodyController.PlayHeadAction((HeadActionType)id, speed);
    }
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_Local_SetHandAction(short id, float speed)
    {
        actorManager_Local.bodyController.PlayHandAction((HandActionType)id, speed);
    }

    #endregion
    #region//其他操作
    /// <summary>
    /// 本地端发送表情
    /// </summary>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_LocalInput_SendEmoji(short id, float duration, bool loop, short distance)
    {
        actorManager_Local.actorUI.SendEmoji((Emoji)id, duration, loop, distance);
    }
    /// <summary>
    /// 本地端发送语言
    /// </summary>
    /// <param name="text"></param>
    /// <param name="id"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_LocalInput_SendText(string text, int id, float duration, bool loop, short distance)
    {
        actorManager_Local.actorUI.SendText(text, (Emoji)id, duration, loop, distance);
    }
    /// <summary>
    /// 本地端犯罪
    /// </summary>
    /// <param name="text"></param>
    /// <param name="id"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_LocalInput_Commit(short commit,short fine)
    {
        actorManager_Local.actionManager.AllClient_Commit((CommitState)commit, fine);
    }
    /// <summary>
    /// RPC:本地端主动拾起物品
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
    /// RPC:本地端自动拾起物品
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
            GameObject itemObj = PoolManager.Instance.GetEffectObj("Effect/Effect_ItemObj");
            itemObj.transform.position = itemNetObj.transform.position;
            itemObj.GetComponent<Effect_ItemObj>().DrawSpriter(itemNetObj.spriteRenderer_Icon.sprite);
            itemObj.GetComponent<Effect_ItemObj>().BindFollow(transform);
        }
    }
    /// <summary>
    /// RPC:本地端停下
    /// </summary>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_LocalInput_StandDown(short time)
    {
        if (actorManager_Local.actorAuthority.isState)
        {
            actorManager_Local.pathManager.State_Stop(time);
        }
    }
    /// <summary>
    /// RPC:本地端转向
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
