using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UniRx;
using UnityEngine.Windows;
using Fusion.Addons.Physics;
/// <summary>
/// 角色网络管理器
/// </summary>
public class ActorNetManager : NetworkBehaviour
{
    [Header("位置同步组件")]
    public NetworkTransform NetTransform;
    [Header("位置同步组件")]
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
    /// 在物体添加到背包之前,模拟添加结果
    /// </summary>
    /// <param name="before">添加前</param>
    /// <param name="after">添加后</param>
    public void CalculateBag(ItemData before,out ItemData after)
    {
        int index = -1;
        int add = 0;
        for (int i = 0; i < Data_ItemInBag.Count; i++)
        {
            if (Data_ItemInBag[i].Item_ID == before.Item_ID)
            {
                /*背包里面有同名物体*/
                ItemConfig item = ItemConfigData.GetItemConfig(before.Item_ID);
                int maxCount = item.Item_MaxCount;
                if (before.Item_Count + Data_ItemInBag[i].Item_Count <= maxCount)
                {
                    /*背包里面有同名物体,且可以叠加*/
                    index = i;
                    add = before.Item_Count;
                }
                else
                {
                    /*背包里面有同名物体,不可叠加*/
                    index = i;
                    add = maxCount - Data_ItemInBag[i].Item_Count;
                }
            }
        }

        if (index == -1)
        {
            if (Data_ItemInBag.Count < 5)
            {
                /*背包里面没有同名物体且背包未达上限*/
                before.Item_Count = 0;
            }
        }
        else
        {
            if (Data_ItemInBag.Count < 5)/*塞一个新的*/
            {
                before.Item_Count = 0;
            }
            else
            {
                before.Item_Count -= add;
            }
        }
        after = before;
    }
    /// <summary>
    /// 服务器尝试添加物体到背包
    /// </summary>
    /// <param name="itemData"></param>
    /// <returns>是否成功</returns>
    private void AddItemInBag(ItemData itemData)
    {
        int index = -1;
        int add = 0;
        for (int i = 0; i < Data_ItemInBag.Count; i++)
        {
            if (Data_ItemInBag[i].Item_ID == itemData.Item_ID)
            {
                /*背包里面有同名物体*/
                ItemConfig item = ItemConfigData.GetItemConfig(itemData.Item_ID);
                int maxCount = item.Item_MaxCount;
                if (itemData.Item_Count + Data_ItemInBag[i].Item_Count <= maxCount)
                {
                    /*背包里面有同名物体,且可以叠加*/
                    index = i;
                    add = itemData.Item_Count;
                }
                else
                {
                    /*背包里面有同名物体,不可叠加*/
                    index = i;
                    add = maxCount - Data_ItemInBag[i].Item_Count;
                }
            }
        }

        if (index == -1)
        {
            /*背包里面无同名物体*/
            if (Data_ItemInBag.Count < 5)
            {
                Data_ItemInBag.Add(itemData);
            }
        }
        else
        {
            ItemData targetItem = Data_ItemInBag[index];
            targetItem.Item_Count += add;
            Data_ItemInBag.Set(index, targetItem);
            /*如果还有剩余*/
            itemData.Item_Count -= add;
            if (itemData.Item_Count > 0 && Data_ItemInBag.Count < 5)
            {
                Data_ItemInBag.Add(itemData);
            }
        }
    }
    /// <summary>
    /// 更新随机数
    /// </summary>
    public void UpdateSeed()
    {
        Data_Seed += 1;
    }
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
        Random.InitState(Data_Seed);
        RandomInRange = Random.Range(0, 101);
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
    /// 普通速度
    /// </summary>
    [Networked, HideInInspector]
    public int Data_CommonSpeed { get; set; }
    /// <summary>
    /// 最大速度
    /// </summary>
    [Networked, HideInInspector]
    public int Data_MaxSpeed { get; set; }
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
    public int Data_Armor { get; set; }
    /// <summary>
    /// 当前食物值
    /// </summary>
    [Networked, OnChangedRender(nameof(OnCurFoodChange)), HideInInspector]
    public int Data_CurFood { get; set; }
    /// <summary>
    /// 最大食物值
    /// </summary>
    [Networked, OnChangedRender(nameof(OnMaxFoodChange)), HideInInspector]
    public int Data_MaxFood { get; set; }
    /// <summary>
    /// 缺水值
    /// </summary>
    [Networked, OnChangedRender(nameof(OnWaterChange)), HideInInspector]
    public int Data_Water { get; set; }
    /// <summary>
    /// 当前精神值
    /// </summary>
    [Networked, OnChangedRender(nameof(OnCurSanChange)), HideInInspector]
    public int Data_CurSan { get; set; }
    /// <summary>
    /// 最大精神值
    /// </summary>
    [Networked, OnChangedRender(nameof(OnMaxSanChange)), HideInInspector]
    public int Data_MaxSan { get; set; }
    /// <summary>
    /// 心情值
    /// </summary>
    [Networked, OnChangedRender(nameof(OnHappyChange)), HideInInspector]
    public int Data_Happy { get; set; }
    /// <summary>
    /// 耐力值
    /// </summary>
    [Networked, OnChangedRender(nameof(OnEnChange)), HideInInspector]
    public int Data_En { get; set; }/*1000倍*/
    /// <summary>
    /// 耐力恢复
    /// </summary>
    [Networked, OnChangedRender(nameof(OnEnReleaseChange)), HideInInspector]
    public int Data_EnRelease { get; set; }/*1000倍*/

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
            Debug.Log("UpdateHP");
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateData()
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

    }
    public void OnCurFoodChange()
    {
    }
    public void OnMaxFoodChange()
    {

    }
    public void OnWaterChange()
    {

    }
    public void OnCurSanChange()
    {

    }
    public void OnMaxSanChange()
    {

    }
    public void OnHappyChange()
    {

    }
    public void OnEnChange()
    {
        LocalManager.ActorUI.UpdateENBar(1 - Data_En * 0.001f);
    }
    public void OnEnReleaseChange()
    {
        LocalManager.ActorUI.UpdateENReleaseBar(Data_EnRelease * 0.001f);
    }

    #endregion

    #region//能力属性
    [Networked, HideInInspector]
    public int Data_Point_Strength { get; set; }
    [Networked, HideInInspector]
    public int Data_Point_Intelligence { get; set; }
    [Networked, HideInInspector]
    public int Data_Point_SPower { get; set; }
    [Networked, HideInInspector]
    public int Data_Point_Focus { get; set; }
    [Networked, HideInInspector]
    public int Data_Point_Agility { get; set; }
    [Networked, HideInInspector]
    public int Data_Point_Make { get; set; }
    [Networked, HideInInspector]
    public int Data_Point_Build { get; set; }
    [Networked, HideInInspector]
    public int Data_Point_Cook { get; set; }
    #endregion

    #region//持有物品
    /// <summary>
    /// 背包物体
    /// </summary>
    [Networked, Capacity(20), OnChangedRender(nameof(OnItemInBagChange)), HideInInspector]
    public NetworkLinkedList<ItemData> Data_ItemInBag { get; }
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

    public void OnItemInHandChange()
    {
        LocalManager.AddItem_Hand(Data_ItemInHand);
    }
    public void OnItemOnHeadChange()
    {
        LocalManager.WearItem_Head(Data_ItemOnHead);
    }
    public void OnItemOnBodyChange()
    {
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
    public NetworkLinkedList<int> Data_Buffs { get; }
    [Networked, Capacity(20), OnChangedRender(nameof(OnSkillKnowChange)), HideInInspector]
    public NetworkLinkedList<int> Data_SkillKnow { get; }
    [Networked, Capacity(20), OnChangedRender(nameof(OnSkillUseChange)), HideInInspector]
    public NetworkLinkedList<int> Data_SkillUse { get; }
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

        Data_CommonSpeed = netData.CommonSpeed;
        Data_MaxSpeed = netData.MaxSpeed;

        Data_En = netData.En;

        Data_Point_Strength = netData.Point_Strength;
        Data_Point_Intelligence = netData.Point_Intelligence;
        Data_Point_SPower = netData.Point_SPower;
        Data_Point_Focus = netData.Point_Focus;
        Data_Point_Agility = netData.Point_Agility;
        Data_Point_Make = netData.Point_Make;
        Data_Point_Build = netData.Point_Build;
        Data_Point_Cook = netData.Point_Cook;
    }
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_LocalInput_AddBuff(int buffID)
    {
        Data_Buffs.Add(buffID);
    }
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_LocalInput_AddSkillKnow(int skillID)
    {
        Data_SkillKnow.Add(skillID);
    }
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_LocalInput_AddSkillUse(int skillID)
    {
        Data_SkillUse.Add(skillID);
    }

    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPC_State_Attack(bool parameter)
    {
        LocalManager.FromRPC_ChangeAttackState(parameter);
    }
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPC_State_ChangeAttackTarget(NetworkId id)
    {
        LocalManager.FromRPC_ChangeAttackTarget(id);
    }
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPC_State_Skill(int parameter, NetworkId networkId)
    {
        LocalManager.FromRPC_InvokeSkill(parameter, networkId);
    }
    /// <summary>
    /// 生命值改变
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
        LocalManager.FromRPC_InvokeSkill(skillID, networkId);
    }

    /// <summary>
    /// RPC:本地端输入添加一个物体
    /// </summary>
    /// <param name="itemData"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_LocalInput_AddItemInBag(ItemData itemData)
    {
        if (Object.HasStateAuthority)
        {
            AddItemInBag(itemData);
        }
    }
    /// <summary>
    /// RPC:本地端输入拾起一个物品
    /// </summary>
    /// <param name="id"></param>
    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.All)]
    public void RPC_LocalInput_PickItem(NetworkId id)
    {
        if (Object.HasStateAuthority)
        {
            LocalManager.PlayPickUp(1, (string name) =>
            {
                if(name == "PickUp")
                {
                    NetworkObject networkPlayerObject = Runner.FindObject(id);
                    networkPlayerObject.GetComponent<ItemNetObj>().PickUp(out ItemData itemData);

                    CalculateBag(itemData, out ItemData back);

                    if (back.Item_Count == 0)//可以全部放入
                    {
                        AddItemInBag(itemData);
                        Runner.Despawn(networkPlayerObject);
                    }
                    else if (back.Item_Count < itemData.Item_Count)//可以放入部分
                    {
                        AddItemInBag(itemData);
                        ItemData residue = new ItemData();
                        residue.Item_ID = itemData.Item_ID;
                        residue.Item_Seed = itemData.Item_Seed;
                        residue.Item_Count = back.Item_Count;

                        MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnItem()
                        {
                            itemData = residue,
                            pos = Runner.FindObject(id).transform.position
                        });
                        Runner.Despawn(networkPlayerObject);
                    }
                    else if (back.Item_Count == itemData.Item_Count)//一个都没放入
                    {

                    }
                }
            });
        }
        else
        {
            LocalManager.PlayPickUp(1,null);
        }
    }
    /// <summary>
    /// RPC:本地端输入失去一个物品
    /// </summary>
    /// <param name="itemData"></param>
    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.All)]
    public void RPC_LocalInput_LoseItem(ItemData itemData)
    {
        if (Object.HasStateAuthority)
        {
            Data_ItemInBag.Remove(itemData);
            //if (Data_ItemInHand.Item_ID == itemData.Item_ID)
            //{
            //    UnityEngine.Debug.Log("失去的物体是手上的物体");
            //    UnityEngine.Debug.Log(itemData.Item_ID + "/" + itemData.Item_Seed + "/" + itemData.Item_Val + "/" + itemData.Item_Count);
            //    UnityEngine.Debug.Log(Data_ItemInHand.Item_ID + "/" + Data_ItemInHand.Item_Seed + "/" + Data_ItemInHand.Item_Val + "/" + Data_ItemInHand.Item_Count);
            //    if (Data_ItemInHand.Item_Seed == itemData.Item_Seed)
            //    {
            //        if (Data_ItemInHand.Item_Val == itemData.Item_Val)
            //        {
            //            if (Data_ItemInHand.Item_Count == itemData.Item_Count)
            //            {
            //                Data_ItemInHand = new ItemData();
            //            }
            //        }
            //    }
            //}
        }
    }
    /// <summary>
    /// RPC:本地端输入修改一个物品
    /// </summary>
    /// <param name="itemData"></param>
    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.All)]
    public void RPC_LocalInput_ChangeItemInBag(ItemData oldItemData, ItemData newItemData)
    {
        if (Object.HasStateAuthority)
        {
            /*查找到需要修改的物品*/
            if (Data_ItemInBag.Contains(oldItemData))
            {
                int index = Data_ItemInBag.IndexOf(oldItemData);
                Data_ItemInBag.Set(index, newItemData);
                Debug.Log("物品修改成功");
                Debug.Log("旧" + oldItemData.Item_ID + "/" + oldItemData.Item_Val + "/" + oldItemData.Item_Count);
                Debug.Log("新" + newItemData.Item_ID + "/" + newItemData.Item_Val + "/" + newItemData.Item_Count);

                //if (Data_ItemInHand.Item_ID == oldItemData.Item_ID)
                //{
                //    if(Data_ItemInHand.Item_Seed == oldItemData.Item_Seed)
                //    {
                //        Data_ItemInHand = newItemData;
                //        Debug.Log("物品修改成功");
                //        Debug.Log("旧" + oldItemData.Item_ID + "/" + oldItemData.Item_Val + "/" + oldItemData.Item_Count);
                //        Debug.Log("新" + newItemData.Item_ID + "/" + newItemData.Item_Val + "/" + newItemData.Item_Count);
                //    }
                //}
            }
            else
            {
                Debug.Log("物品修改失败,未找到目标物品:" + oldItemData);
            }
        }
    }
    /// <summary>
    /// RPC:本地端输入修改一个物品
    /// </summary>
    /// <param name="itemData"></param>
    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.All)]
    public void RPC_LocalInput_ChangeItemInHand(ItemData oldItemData, ItemData newItemData)
    {
        if (Object.HasStateAuthority)
        {
            /*查找到需要修改的物品*/
            if (oldItemData.Item_ID == Data_ItemInHand.Item_ID && oldItemData.Item_Seed == Data_ItemInHand.Item_Seed)
            {
                Data_ItemInHand = newItemData;
                Debug.Log("物品修改成功");
                Debug.Log("旧" + oldItemData.Item_ID + "/" + oldItemData.Item_Val + "/" + oldItemData.Item_Count);
                Debug.Log("新" + newItemData.Item_ID + "/" + newItemData.Item_Val + "/" + newItemData.Item_Count);
            }
            else
            {
                Debug.Log("物品修改失败,未找到目标物品:" + oldItemData);
            }
        }
    }

    /// <summary>
    /// RPC:本地端输入持握一个物品
    /// </summary>
    /// <param name="itemData"></param>
    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.All)]
    public void RPC_LocalInput_AddItemOnHand(ItemData itemData)
    {
        UnityEngine.Debug.Log("Player" + "HoldItem" + itemData.Item_ID);

        if (Object.HasStateAuthority)
        {
            Data_ItemInHand = itemData;
        }
    }
    /// <summary>
    /// RPC:本地端输入持握一个物品
    /// </summary>
    /// <param name="itemData"></param>
    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.All)]
    public void RPC_LocalInput_RemoveItemOnHand()
    {
        if (Object.HasStateAuthority)
        {
            Data_ItemInHand = new ItemData();
        }
    }
    /// <summary>
    /// 本地端把一个物体戴在头上
    /// </summary>
    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.All)]
    public void RPC_LocalInput_AddItemOnHead(ItemData itemData)
    {
        UnityEngine.Debug.Log("Player" + "HeadOn" + itemData.Item_ID);

        if (Object.HasStateAuthority)
        {
            Data_ItemOnHead = itemData;
        }
    }
    /// <summary>
    /// 本地端把一个物体从头上摘下来
    /// </summary>
    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.All)]
    public void RPC_LocalInput_RemoveItemOnHead()
    {
        if (Object.HasStateAuthority)
        {
            Data_ItemOnHead = new ItemData();
        }
    }
    /// <summary>
    /// 本地端把一个物体戴在身上
    /// </summary>
    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.All)]
    public void RPC_LocalInput_AddItemOnBody(ItemData itemData)
    {
        UnityEngine.Debug.Log("Player" + "BodyOn" + itemData.Item_ID);

        if (Object.HasStateAuthority)
        {
            Data_ItemOnBody = itemData;
        }
    }
    /// <summary>
    /// 本地端把一个物体从身上摘下来
    /// </summary>
    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.All)]
    public void RPC_LocalInput_RemoveItemOnBody()
    {
        if (Object.HasStateAuthority)
        {
            Data_ItemOnBody = new ItemData();
        }
    }
    /// <summary>
    /// 本地端发送表情
    /// </summary>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_LocalInput_SendEmoji(int id)
    {
        LocalManager.FromRPC_SendEmoji(id);
    }

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
