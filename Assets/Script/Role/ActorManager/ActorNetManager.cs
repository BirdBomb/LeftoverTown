using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UniRx;
using UnityEngine.Windows;
using Fusion.Addons.Physics;
using System;
/// <summary>
/// 角色网络管理器
/// </summary>
public class ActorNetManager : NetworkBehaviour
{
    [Header("位置同步组件")]
    public NetworkTransform NetTransform;
    [Header("物理同步组件")]
    public NetworkRigidbody2D networkRigidbody;
    [Header("本地角色组件")]
    public ActorManager LocalManager;
    public override void Spawned()
    {
        if (LocalManager.isPlayer)
        {
           
        }
        else
        {
            if (Object.HasStateAuthority)
            {
                Object.AssignInputAuthority(Runner.LocalPlayer);
            }
        }
        LocalManager.InitByNetManager(Object.HasStateAuthority, Object.HasInputAuthority);
        base.Spawned();
        InitNetworkData();
    }
    public override void FixedUpdateNetwork()
    {
        LocalManager.FixedUpdateNetwork(Runner.DeltaTime);
        base.FixedUpdateNetwork();
    }
    /// <summary>
    /// 更新网络位置
    /// </summary>
    /// <param name="pos"></param>
    public void UpdateNetworkTransform(Vector3 pos,float speed)
    {
        if (networkRigidbody /*&& Object.HasStateAuthority*/)
        {
            if (networkRigidbody.Rigidbody.velocity.magnitude <= speed)
            {
                networkRigidbody.Rigidbody.velocity = Vector2.zero;
                networkRigidbody.Rigidbody.position = (pos);
            }
            //networkRigidbody.Teleport(pos);
            //transform.position = pos;
        }
    }
    /// <summary>
    /// 更新随机数
    /// </summary>
    public void UpdateSeed()
    {
        Data_Seed += 1;
    }
    #region//周期
    /// <summary>
    /// 饥饿计时器
    /// </summary>
    private int foodTimer;
    public void StartLoop()
    {
        InvokeRepeating("AddOneSecond", 1, 1);
    }
    /// <summary>
    /// (秒)周期
    /// </summary>
    private void AddOneSecond()
    {
        UpdateFoodTime();
        UpdateItemTime();
    }
    /// <summary>
    /// (小时)周期
    /// </summary>
    public void AddOneHour(int hour,int date, GlobalTime globalTime)
    {
        
    }
    private void UpdateFoodTime()
    {
        foodTimer++;
        if (foodTimer >= Data_Water + 5)
        {
            foodTimer = 0;
            if (LocalManager.SubFood(-1) <= 0)
            {
                Data_CurHp -= (int)(Data_MaxHp * 0.1f);
            }
        }

    }
    private void UpdateItemTime()
    {
        if (LocalManager.holdingByHand != null)
        {
            LocalManager.holdingByHand.UpdateTime(1);
        }
        if (LocalManager.wearingOnHead != null)
        {
            LocalManager.wearingOnHead.UpdateTime(1);
        }
        if (LocalManager.wearingOnBody != null)
        {
            LocalManager.wearingOnBody.UpdateTime(1);
        }
    }
    #endregion
    #region//Networked

    public void InitNetworkData()
    {
        OnSeedChange();
        OnNameChange();
        OnHairIDChange();
        OnHairColorChange();
        OnEyeIDChange();

        OnItemInBagChange();
        OnItemInHandChange();
        OnItemOnHeadChange();
        OnItemOnBodyChange();

        OnBuffsChange();
        OnSkillKnowChange();
        OnSkillUseChange();
    }

    [Networked, OnChangedRender(nameof(OnSeedChange)),HideInInspector]
    public int Data_Seed { get; set; }
    public int RandomInRange { get; set; }
    public void OnSeedChange()
    {
        UnityEngine.Random.InitState(Data_Seed);
        RandomInRange = UnityEngine.Random.Range(0, 101);
    }

    #region//外貌和名字
    /// <summary>
    /// 名称
    /// </summary>
    [Networked, OnChangedRender(nameof(OnNameChange)), HideInInspector]
    public string Data_Name { get; set; }
    /// <summary>
    /// 头发ID
    /// </summary>
    [Networked, OnChangedRender(nameof(OnHairIDChange))]
    public int Data_HairID { get; set; }
    /// <summary>
    /// 头发颜色
    /// </summary>
    [Networked, OnChangedRender(nameof(OnHairColorChange)), HideInInspector]
    public Color32 Data_HairColor { get;set; }
    /// <summary>
    /// 眼镜ID
    /// </summary>
    [Networked, OnChangedRender(nameof(OnEyeIDChange)), HideInInspector]
    public int Data_EyeID { get; set; }

    public void OnNameChange()
    {

    }
    public void OnHairIDChange()
    {
        LocalManager.BodyController.InitFace(Data_HairID, Data_EyeID, Data_HairColor);
    }
    public void OnHairColorChange()
    {
        LocalManager.BodyController.InitFace(Data_HairID, Data_EyeID, Data_HairColor);
    }
    public void OnEyeIDChange()
    {
        LocalManager.BodyController.InitFace(Data_HairID, Data_EyeID, Data_HairColor);
    }
    #endregion

    #region//基础属性
    /// <summary>
    /// 普通速度(分米/秒)
    /// </summary>
    [Networked, HideInInspector]
    public short Data_CommonSpeed { get; set; }
    /// <summary>
    /// 最大速度(分米/秒)
    /// </summary>
    [Networked, HideInInspector]
    public short Data_MaxSpeed { get; set; }
    /// <summary>
    /// 当前生命值
    /// </summary>
    [Networked, OnChangedRender(nameof(OnCurHpChange)), HideInInspector]
    public int Data_CurHp { get; set; }
    /// <summary>
    /// 最大生命值
    /// </summary>
    [Networked, OnChangedRender(nameof(OnMaxHpChange)), HideInInspector]
    public int Data_MaxHp { get; set; }
    /// <summary>
    /// 护甲
    /// </summary>
    [Networked, OnChangedRender(nameof(OnArmorChange)), HideInInspector]
    public short Data_Armor { get; set; }
    /// <summary>
    /// 当前食物值
    /// </summary>
    [Networked, OnChangedRender(nameof(OnCurFoodChange)), HideInInspector]
    public short Data_CurFood { get; set; }
    /// <summary>
    /// 最大食物值
    /// </summary>
    [Networked, OnChangedRender(nameof(OnMaxFoodChange)), HideInInspector]
    public short Data_MaxFood { get; set; }
    /// <summary>
    /// 缺水值
    /// </summary>
    [Networked, OnChangedRender(nameof(OnWaterChange)), HideInInspector]
    public short Data_Water { get; set; }
    /// <summary>
    /// 当前精神值
    /// </summary>
    [Networked, OnChangedRender(nameof(OnCurSanChange)), HideInInspector]
    public short Data_CurSan { get; set; }
    /// <summary>
    /// 最大精神值
    /// </summary>
    [Networked, OnChangedRender(nameof(OnMaxSanChange)), HideInInspector]
    public short Data_MaxSan { get; set; }
    /// <summary>
    /// 心情值
    /// </summary>
    [Networked, OnChangedRender(nameof(OnHappyChange)), HideInInspector]
    public short Data_Happy { get; set; }
    /// <summary>
    /// 金币
    /// </summary>
    [Networked, OnChangedRender(nameof(OnCoinChange)), HideInInspector]
    public int Data_Coin { get; set; }
    /// <summary>
    /// 悬赏
    /// </summary>
    [Networked, OnChangedRender(nameof(OnFineChange)), HideInInspector]
    public short Data_Fine { get; set; }
    /// <summary>
    /// 最大耐力值
    /// </summary>
    [Networked, OnChangedRender(nameof(OnEnChange)), HideInInspector]
    public int Data_MaxEn { get; set; }/*单位毫秒*/
    /// <summary>
    /// 当前耐力值
    /// </summary>
    [Networked, OnChangedRender(nameof(OnEnChange)), HideInInspector]
    public int Data_CurEn { get; set; }/*单位毫秒*/
    /// <summary>
    /// 耐力恢复速度
    /// </summary>
    [Networked, HideInInspector]
    public int Data_EnRelease { get; set; }/*常态为1000*/

    public void OnCurHpChange()
    {
        if (Data_CurHp <= 0)
        {
            LocalManager.ActorUI.UpdateHPBar(0 / (float)Data_MaxHp);
            LocalManager.TryToDead();
        }
        else
        {
            LocalManager.ActorUI.UpdateHPBar((float)Data_CurHp / (float)Data_MaxHp);
        }
        if (LocalManager.isPlayer && Object.HasInputAuthority)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateHPData()
            {
                HP = Data_CurHp,
                MaxHP = Data_MaxHp,
            });
        }
    }
    public void OnMaxHpChange()
    {

    }
    public void OnArmorChange()
    {
        if (LocalManager.isPlayer && Object.HasInputAuthority)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateArmorData()
            {
                Armor = Data_Armor,
            });
        }
    }
    public void OnCurFoodChange()
    {
        if (LocalManager.isPlayer && Object.HasInputAuthority)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateFoodData()
            {
                Food = Data_CurFood,
                MaxFood = Data_MaxFood,
            });
        }
    }
    public void OnMaxFoodChange()
    {

    }
    public void OnWaterChange()
    {
        if (LocalManager.isPlayer && Object.HasInputAuthority)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateWaterData()
            {
                Water = Data_Water,
            });
        }
    }
    public void OnCurSanChange()
    {
        if (LocalManager.isPlayer && Object.HasInputAuthority)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateSanData()
            {
                San = Data_CurSan,
                MaxSan = Data_MaxSan,
            });
        }
    }
    public void OnMaxSanChange()
    {

    }
    public void OnHappyChange()
    {
        if (LocalManager.isPlayer && Object.HasInputAuthority)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateHappyData()
            {
                Happy = Data_Happy,
            });
        }
    }
    public void OnCoinChange()
    {
        if (LocalManager.isPlayer && Object.HasInputAuthority)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateCoinData()
            {
                Coin = Data_Coin,
            });
        }
    }
    public void OnFineChange()
    {
        if (LocalManager.isPlayer && Object.HasInputAuthority)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateFineData()
            {
                Fine = Data_Fine,
            });
        }
    }

    public void OnEnChange()
    {
        LocalManager.ActorUI.UpdateENBar((float)Data_CurEn / (float)Data_MaxEn);
    }

    #endregion

    #region//能力属性
    [Networked, HideInInspector]
    public short Data_Point_Strength { get; set; }
    [Networked, HideInInspector]
    public short Data_Point_Intelligence { get; set; }
    [Networked, HideInInspector]
    public short Data_Point_SPower { get; set; }
    [Networked, HideInInspector]
    public short Data_Point_Focus { get; set; }
    [Networked, HideInInspector]
    public short Data_Point_Agility { get; set; }
    [Networked, HideInInspector]
    public short Data_Point_Make { get; set; }
    [Networked, HideInInspector]
    public short Data_Point_Build { get; set; }
    [Networked, HideInInspector]
    public short Data_Point_Cook { get; set; }
    #endregion

    #region//持有物品
    /// <summary>
    /// 背包物体
    /// </summary>
    [Networked, Capacity(20), OnChangedRender(nameof(OnItemInBagChange)), HideInInspector]
    public NetworkLinkedList<ItemData> Data_ItemInBag { get; }
    /// <summary>
    /// 背包容量
    /// </summary>
    [Networked, OnChangedRender(nameof(OnItemInBagChange)), HideInInspector]
    public int Data_BagCapacity { get; set; }
    /// <summary>
    /// 手部物体
    /// </summary>
    [Networked, OnChangedRender(nameof(OnItemInHandChange)), HideInInspector]
    public ItemData Data_ItemInHand { get; set; }
    public ItemData Last_ItemInHand;
    /// <summary>
    /// 头部物体
    /// </summary>
    [Networked, OnChangedRender(nameof(OnItemOnHeadChange)), HideInInspector]
    public ItemData Data_ItemOnHead { get; set; }
    public ItemData Last_ItemOnHead;
    /// <summary>
    /// 身体物体
    /// </summary>
    [Networked, OnChangedRender(nameof(OnItemOnBodyChange)), HideInInspector]
    public ItemData Data_ItemOnBody { get; set; }
    public ItemData Last_ItemOnBody;
    public short LocalData_Status;
    public void OnItemInHandChange()
    {
        LocalManager.AddItem_Hand(Data_ItemInHand);
    }
    public void OnItemOnHeadChange()
    {
        LocalManager.CheckStatus(Data_ItemOnBody.Item_ID, Data_ItemOnHead.Item_ID, Data_Fine, out LocalData_Status);
        LocalManager.WearItem_Head(Data_ItemOnHead);
    }
    public void OnItemOnBodyChange()
    {
        LocalManager.CheckStatus(Data_ItemOnBody.Item_ID, Data_ItemOnHead.Item_ID, Data_Fine, out LocalData_Status);
        LocalManager.WearItem_Body(Data_ItemOnBody);
    }
    public void OnItemInBagChange()
    {
        if (LocalManager.isPlayer && Object.HasInputAuthority)
        {
            List<ItemData> temp = new List<ItemData>();
            for (int i = 0; i < Data_ItemInBag.Count; i++)
            {
                temp.Add(Data_ItemInBag[i]);
            }
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateItemInBag()
            {
                bagCapacity = Data_BagCapacity,
                itemDatas = temp
            });
        }
    }
    #endregion

    #region//特性与技能
    /// <summary>
    /// Buff
    /// </summary>
    [Networked, Capacity(20), OnChangedRender(nameof(OnBuffsChange)), HideInInspector]
    public NetworkLinkedList<short> Data_Buffs { get; }
    [Networked, Capacity(20), OnChangedRender(nameof(OnSkillKnowChange)), HideInInspector]
    public NetworkLinkedList<short> Data_SkillKnow { get; }
    [Networked, Capacity(20), OnChangedRender(nameof(OnSkillUseChange)), HideInInspector]
    public NetworkLinkedList<short> Data_SkillUse { get; }
    public void OnBuffsChange()
    {

    }
    public void OnSkillKnowChange()
    {

    }
    public void OnSkillUseChange()
    {

    }
    #endregion
    #endregion
    #region//RPC
    #region//初始化
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_LocalInput_InitPlayerCommonData(PlayerNetData netData)
    {
        Data_HairID = netData.HairID;
        Data_HairColor = netData.HairColor;
        Data_EyeID = netData.EyeID;

        Data_CurHp = netData.CurHp;
        Data_MaxHp = netData.MaxHp;
        Data_Armor = netData.Armor;
        Data_CurFood = netData.CurFood;
        Data_MaxFood = netData.MaxFood;
        Data_Water = netData.Water;
        Data_CurSan = netData.CurSan;
        Data_MaxSan = netData.MaxSan;
        Data_Happy = netData.Happy;
        Data_Coin = netData.Coin;

        Data_CommonSpeed = netData.CommonSpeed;
        Data_MaxSpeed = netData.MaxSpeed;

        Data_MaxEn = netData.En;
        Data_CurEn = netData.En;
        Data_EnRelease = 1000;

        Data_Point_Strength = netData.Point_Strength;
        Data_Point_Intelligence = netData.Point_Intelligence;
        Data_Point_SPower = netData.Point_SPower;
        Data_Point_Focus = netData.Point_Focus;
        Data_Point_Agility = netData.Point_Agility;
        Data_Point_Make = netData.Point_Make;
        Data_Point_Build = netData.Point_Build;
        Data_Point_Cook = netData.Point_Cook;
    }
    #endregion
    #region//背包操作
    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_ChangeBagCapacity(int capacity)
    {
        OnlyState_ChangeBagCapacity(capacity);
    }
    public void OnlyState_ChangeBagCapacity(int capacity)
    {
        Data_BagCapacity = capacity;
    }
    /// <summary>
    /// RPC:本地端输入添加一个物体
    /// </summary>
    /// <param name="itemData"></param>
    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_AddItemInBag(ItemData itemData)
    {
        OnlyState_AddItemInBag(itemData);
    }
    public void OnlyState_AddItemInBag(ItemData itemData)
    {
        ItemConfig config = ItemConfigData.GetItemConfig(itemData.Item_ID);
        if (config.Item_Size == ItemSize.AsGroup)
        {
            ItemData resData = itemData;

            for (int i = 0; i < Data_ItemInBag.Count; i++)
            {
                if (Data_ItemInBag[i].Item_ID == resData.Item_ID)
                {
                    Type type = Type.GetType("Item_" + Data_ItemInBag[i].Item_ID.ToString());
                    ((ItemBase)Activator.CreateInstance(type)).StaticAction_PileUp(Data_ItemInBag[i], resData, config.Item_MaxCount, out ItemData newData, out resData);
                    Data_ItemInBag.Set(i, newData);
                }
            }
            if (resData.Item_Count > 0)
            {
                if (Data_ItemInBag.Count < Data_BagCapacity)
                {
                    Data_ItemInBag.Add(resData);
                }
                else
                {
                    MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnItem()
                    {
                        pos = transform.position,
                        itemData = resData
                    });
                }
            }
        }
        else
        {
            /*不可堆叠*/
            if (Data_ItemInBag.Count < Data_BagCapacity)
            {
                Data_ItemInBag.Add(itemData);
            }
            else
            {
                MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnItem()
                {
                    pos = transform.position,
                    itemData = itemData
                });
            }
        }

    }
    /// <summary>
    /// RPC:本地端输入失去一个物品
    /// </summary>
    /// <param name="itemData"></param>
    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_LoseItem(ItemData itemData)
    {
        OnlyState_LoseItem(itemData);
    }
    public void OnlyState_LoseItem(ItemData itemData)
    {
        if (Data_ItemInBag.Contains(itemData))
        {
            Data_ItemInBag.Remove(itemData);
        }
    }
    /// <summary>
    /// RPC:本地端输入修改一个物品
    /// </summary>
    /// <param name="itemData"></param>
    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_ChangeItemInBag(ItemData oldItemData, ItemData newItemData)
    {
        if (Object.HasStateAuthority)
        {
            OnlyState_ChangeItemInBag(oldItemData, newItemData);
        }
    }
    public void OnlyState_ChangeItemInBag(ItemData oldItemData, ItemData newItemData)
    {
        /*查找到需要修改的物品*/
        if (Data_ItemInBag.Contains(oldItemData))
        {
            int index = Data_ItemInBag.IndexOf(oldItemData);
            Data_ItemInBag.Set(index, newItemData);
            Debug.Log("物品修改成功");
        }
        else
        {
            Debug.Log("物品修改失败,未找到目标物品:" + oldItemData);
        }
    }
    #endregion
    #region//持握操作(手部)
    /// <summary>
    /// RPC:本地端输入持握一个物品
    /// </summary>
    /// <param name="itemData"></param>
    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_AddItemOnHand(ItemData itemData)
    {
        OnlyState_AddItemOnHand(itemData);
    }
    public void OnlyState_AddItemOnHand(ItemData itemData)
    {
        if (Data_ItemInHand.Item_ID == 0)
        {
            Data_ItemInHand = itemData;
        }
        else
        {
            ItemConfig config = ItemConfigData.GetItemConfig(itemData.Item_ID);
            ItemData resData = itemData;
            if (Data_ItemInHand.Item_ID == resData.Item_ID && config.Item_Size == ItemSize.AsGroup)
            {
                Type type = Type.GetType("Item_" + resData.Item_ID.ToString());
                ((ItemBase)Activator.CreateInstance(type)).StaticAction_PileUp(Data_ItemInHand, resData, config.Item_MaxCount, out ItemData newData, out resData);
                Data_ItemInHand = newData;
                if (resData.Item_Count > 0)
                {
                    OnlyState_AddItemInBag(resData);
                }
            }
            else
            {
                OnlyState_AddItemInBag(Data_ItemInHand);
                Data_ItemInHand = resData;
            }
        }
    }
    /// <summary>
    /// RPC:本地端结束持握一个物品
    /// </summary>
    /// <param name="itemData"></param>
    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_RemoveItemOnHand()
    {
        OnlyState_RemoveItemOnHand();
    }
    public void OnlyState_RemoveItemOnHand()
    {
        Data_ItemInHand = new ItemData();
    }
    /// <summary>
    /// RPC:本地端输入修改一个物品
    /// </summary>
    /// <param name="itemData"></param>
    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_ChangeItemOnHand(ItemData newItemData)
    {
        OnlyState_ChangeItemOnHand(newItemData);
    }
    public void OnlyState_ChangeItemOnHand(ItemData newItemData)
    {
        Data_ItemInHand = newItemData;
    }
    #endregion
    #region//穿戴操作(头部)
    /// <summary>
    /// 本地端把一个物体戴在头上
    /// </summary>
    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_AddItemOnHead(ItemData itemData)
    {
        OnlyState_AddItemOnHead(itemData);
    }
    public void OnlyState_AddItemOnHead(ItemData itemData)
    {
        if (Data_ItemOnHead.Item_ID == 0)
        {
            Data_ItemOnHead = itemData;
        }
        else
        {
            ItemConfig config = ItemConfigData.GetItemConfig(itemData.Item_ID);
            ItemData resData = itemData;
            if (Data_ItemOnHead.Item_ID == resData.Item_ID && config.Item_Size == ItemSize.AsGroup)
            {
                Type type = Type.GetType("Item_" + resData.Item_ID.ToString());
                ((ItemBase)Activator.CreateInstance(type)).StaticAction_PileUp(Data_ItemOnHead, resData, config.Item_MaxCount, out ItemData newData, out resData);
                Data_ItemOnHead = newData;
                if (resData.Item_Count > 0)
                {
                    OnlyState_AddItemInBag(resData);
                }
            }
            else
            {
                OnlyState_AddItemInBag(Data_ItemOnHead);
                Data_ItemOnHead = resData;
            }
        }
    }
    /// <summary>
    /// 本地端把一个物体从头上摘下来
    /// </summary>
    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_RemoveItemOnHead()
    {
        OnlyState_RemoveItemOnHead();
    }
    public void OnlyState_RemoveItemOnHead()
    {
        Data_ItemOnHead = new ItemData();
    }
    /// <summary>
    /// RPC:本地端输入修改一个物品
    /// </summary>
    /// <param name="itemData"></param>
    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_ChangeItemOnHead(ItemData newItemData)
    {
        OnlyState_ChangeItemOnHead(newItemData);
    }
    public void OnlyState_ChangeItemOnHead(ItemData newItemData)
    {
        Data_ItemInHand = newItemData;
    }
    #endregion
    #region//穿戴操作(身体)
    /// <summary>
    /// 本地端把一个物体戴在身上
    /// </summary>
    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_AddItemOnBody(ItemData itemData)
    {
        OnlyState_AddItemOnBody(itemData);
    }
    public void OnlyState_AddItemOnBody(ItemData itemData)
    {
        if (Data_ItemOnBody.Item_ID == 0)
        {
            Data_ItemOnBody = itemData;
        }
        else
        {
            ItemConfig config = ItemConfigData.GetItemConfig(itemData.Item_ID);
            ItemData resData = itemData;
            if (Data_ItemOnBody.Item_ID == resData.Item_ID && config.Item_Size == ItemSize.AsGroup)
            {
                Type type = Type.GetType("Item_" + resData.Item_ID.ToString());
                ((ItemBase)Activator.CreateInstance(type)).StaticAction_PileUp(Data_ItemOnBody, resData, config.Item_MaxCount, out ItemData newData, out resData);
                Data_ItemOnBody = newData;
                if (resData.Item_Count > 0)
                {
                    OnlyState_AddItemInBag(resData);
                }
            }
            else
            {
                OnlyState_AddItemInBag(Data_ItemOnBody);
                Data_ItemOnBody = resData;
            }

        }
    }
    /// <summary>
    /// 本地端把一个物体从身上摘下来
    /// </summary>
    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_RemoveItemOnBody()
    {
        OnlyState_RemoveItemOnBody();
    }
    public void OnlyState_RemoveItemOnBody()
    {
        Data_ItemOnBody = new ItemData();
    }
    /// <summary>
    /// RPC:本地端输入修改一个物品
    /// </summary>
    /// <param name="itemData"></param>
    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_ChangeItemOnBody(ItemData newItemData)
    {
        OnlyState_ChangeItemOnBody(newItemData);
    }
    public void OnlyState_ChangeItemOnBody(ItemData newItemData)
    {
        Data_ItemOnBody = newItemData;
    }
    #endregion
    #region//Buff操作
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_LocalInput_AddBuff(short buffID)
    {
        Data_Buffs.Add(buffID);
    }

    #endregion
    #region//玩家技能操作
    /// <summary>
    /// RPC:本地端添加一个已知技能
    /// </summary>
    /// <param name="skillID"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_LocalInput_AddSkillKnow(short skillID)
    {
        Data_SkillKnow.Add(skillID);
    }
    /// <summary>
    /// RPC:本地端添加一个可用技能
    /// </summary>
    /// <param name="skillID"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_LocalInput_AddSkillUse(short skillID)
    {
        Data_SkillUse.Add(skillID);
    }
    /// <summary>
    /// RPC:本地端绑定一个技能
    /// </summary>
    /// <param name="itemData"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_LocalInput_BindSkill(int skillID)
    {
        LocalManager.FromRPC_BindSkill(skillID);
    }
    /// <summary>
    /// RPC:本地端使用一个技能
    /// </summary>
    /// <param name="itemData"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_LocalInput_InvokeSkill(int skillID, NetworkId networkId)
    {
        LocalManager.FromRPC_NpcUseSkill(skillID, networkId);
    }

    #endregion
    #region//角色技能操作
    /// <summary>
    /// 更改攻击状态
    /// </summary>
    /// <param name="parameter"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPC_State_NpcChangeAttackState(bool parameter)
    {
        LocalManager.FromRPC_ChangeAttackState(parameter);
    }
    /// <summary>
    /// 更改攻击目标
    /// </summary>
    /// <param name="id"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPC_State_NpcChangeAttackTarget(NetworkId id)
    {
        LocalManager.FromRPC_ChangeAttackTarget(id);
    }
    /// <summary>
    /// 使用技能
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="networkId"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPC_State_NpcUseSkill(int parameter, NetworkId networkId)
    {
        LocalManager.FromRPC_NpcUseSkill(parameter, networkId);
    }

    #endregion
    #region//其他操作
    /// <summary>
    /// 本地端发送表情
    /// </summary>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_LocalInput_SendEmoji(int id)
    {
        LocalManager.FromRPC_SendEmoji(id);
    }
    /// <summary>
    /// RPC:本地端拾起物品
    /// </summary>
    /// <param name="id"></param>
    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.All)]
    public void RPC_LocalInput_PickItem(NetworkId id)
    {
        if (Object.HasStateAuthority)
        {
            LocalManager.PlayPickUp(1, (string name) =>
            {
                if (name == "PickUp")
                {
                    NetworkObject networkPlayerObject = Runner.FindObject(id);
                    networkPlayerObject.GetComponent<ItemNetObj>().PickUp(out ItemData itemData);
                    Runner.Despawn(networkPlayerObject);

                    OnlyState_AddItemInBag(itemData);
                }
            });
        }
        else
        {
            LocalManager.PlayPickUp(1, null);
        }
    }
    /// <summary>
    /// RPC:生命值改变
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="networkId"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_HpChange(int parameter, NetworkId networkId)
    {
        LocalManager.Listen_HpChange(parameter, networkId);
        if (Object.HasStateAuthority)
        {
            Data_CurHp += parameter;
        }
    }
    /// <summary>
    /// RPC:饥饿值改变
    /// </summary>
    /// <param name="val"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_FoodChange(short parameter)
    {
        Data_CurFood = parameter;
    }
    /// <summary>
    /// 本地端支付
    /// </summary>
    /// <param name="val"></param>
    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_PayCoin(int val)
    {
        OnlyState_PayCoin(val);
    }
    private void OnlyState_PayCoin(int val)
    {
        Data_Coin -= val;
    }
    /// <summary>
    /// 本地端赚钱
    /// </summary>
    /// <param name="val"></param>
    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_EarnCoin(int val)
    {
        OnlyState_EarnCoin(val);
    }
    private void OnlyState_EarnCoin(int val)
    {
        Data_Coin += val;
    }
    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_ChangeFine(short val)
    {
        OnlyState_ChangeFine(val);
    }
    private void OnlyState_ChangeFine(short val)
    {
        Data_Fine += val;
    }
    #endregion

    #endregion

}
public struct ActorNetData : INetworkStruct
{
    public int Speed;
    public int MaxSpeed;
    public int Endurance;
    public int Point_Strength;
    public int Point_Intelligence;
    public int Point_Focus;
    public int Point_Agility;
}
