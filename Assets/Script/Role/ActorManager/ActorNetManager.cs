using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UniRx;
/// <summary>
/// 角色网络管理器
/// </summary>
public class ActorNetManager : NetworkBehaviour
{
    [Header("位置同步组件")]
    public NetworkTransform NetTransform;
    [Header("本地角色组件")]
    public ActorManager LocalManager;
    public ActorNetData NetData;
    public override void Spawned()
    {
        LocalManager.InitByNetManager(Object.HasStateAuthority);
        InitNetData();
        OnItemInBagChange();
        OnItemInHandChange();
        base.Spawned();
    }
    private void InitNetData()
    {
        NetData.Speed = (int)(LocalManager.actorConfig.Config_Speed * 1000);
        NetData.MaxSpeed = (int)(LocalManager.actorConfig.Config_Speed * 1000);
        NetData.Endurance = (int)(LocalManager.actorConfig.Config_Endurance * 1000);
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
    public void UpdateNetworkTransform(Vector3 pos)
    {
        if (NetTransform && Object.HasStateAuthority)
        {
            transform.position = pos;
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

    [Networked, OnChangedRender(nameof(OnSeedChange))]
    public int Data_Seed { get; set; }
    public int RandomInRange { get; set; }
    [Networked, OnChangedRender(nameof(OnHpChange))]
    public int Data_Hp { get; set; }
    [Networked, OnChangedRender(nameof(OnEnChange))]
    public int Data_En { get; set; }/*1000倍*/
    [Networked, OnChangedRender(nameof(OnEnReleaseChange))]
    public int Data_EnRelease { get; set; }/*1000倍*/
    [Networked, OnChangedRender(nameof(OnMaxHpChange))]
    public int Data_MaxHp{ get; set; }
    [Networked, OnChangedRender(nameof(OnItemInHandChange))]
    public ItemData Data_ItemInHand { get; set; }
    [Networked, OnChangedRender(nameof(OnItemOnHeadChange))]
    public ItemData Data_ItemOnHead { get; set; }
    [Networked, OnChangedRender(nameof(OnItemOnBodyChange))]
    public ItemData Data_ItemOnBody { get; set; }

    [Networked,Capacity(20), OnChangedRender(nameof(OnItemInBagChange))]
    public NetworkLinkedList<ItemData> Data_ItemInBag { get; }

    [Networked, OnChangedRender(nameof(OnBuffsChange))]
    public NetworkLinkedList<int> Data_Buffs { get; }

    public void OnHpChange()
    {
        if (Data_Hp <= 0)
        {
            LocalManager.ActorUI.UpdateHPBar(0 / (float)Data_MaxHp);
            LocalManager.TryToDead();
        }
        else
        {
            LocalManager.ActorUI.UpdateHPBar((float)Data_Hp / (float)Data_MaxHp);
        }
    }
    public void OnEnChange()
    {
        LocalManager.ActorUI.UpdateENBar(1 - Data_En * 0.001f);
    }
    public void OnEnReleaseChange()
    {
        LocalManager.ActorUI.UpdateENReleaseBar(Data_EnRelease * 0.001f);
    }
    public void OnSeedChange()
    {
        Random.InitState(Data_Seed);
        RandomInRange = Random.Range(0, 101);
    }
    public void OnMaxHpChange()
    {

    }
    public void OnItemInHandChange()
    {
        LocalManager.AddItem_Hand(Data_ItemInHand);
    }
    public void OnItemOnHeadChange()
    {
        
    }
    public void OnItemOnBodyChange()
    {
        
    }
    public void OnItemInBagChange()
    {
        if (Object.HasInputAuthority)
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
    public void OnBuffsChange()
    {

    }

    #endregion
    #region//RPC
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPC_Skill(int parameter, NetworkId networkId)
    {
        LocalManager.FromRPC_Skill(parameter, networkId);
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
            Data_Hp += parameter;
        }
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
    /// RPC:本地端输入持握一个物品
    /// </summary>
    /// <param name="itemData"></param>
    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.All)]
    public void RPC_LocalInput_HoldItem(ItemData itemData)
    {
        UnityEngine.Debug.Log("Player" + "HoldItem" + itemData.Item_ID);

        if (Object.HasStateAuthority)
        {
            Data_ItemInHand = itemData;
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
            if (Data_ItemInHand.Item_ID == itemData.Item_ID)
            {
                UnityEngine.Debug.Log("失去的物体是手上的物体");
                UnityEngine.Debug.Log(itemData.Item_ID + "/" + itemData.Item_Seed + "/" + itemData.Item_Val + "/" + itemData.Item_Count);
                UnityEngine.Debug.Log(Data_ItemInHand.Item_ID + "/" + Data_ItemInHand.Item_Seed + "/" + Data_ItemInHand.Item_Val + "/" + Data_ItemInHand.Item_Count);
                if (Data_ItemInHand.Item_Seed == itemData.Item_Seed)
                {
                    if (Data_ItemInHand.Item_Val == itemData.Item_Val)
                    {
                        if (Data_ItemInHand.Item_Count == itemData.Item_Count)
                        {
                            Data_ItemInHand = new ItemData();
                        }
                    }
                }
            }
        }
    }
    /// <summary>
    /// RPC:本地端输入修改一个物品
    /// </summary>
    /// <param name="itemData"></param>
    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.All)]
    public void RPC_LocalInput_ChangeItem(ItemData oldItemData, ItemData newItemData)
    {
        if (Object.HasStateAuthority)
        {
            /*查找到需要修改的物品*/
            if (Data_ItemInBag.Contains(oldItemData))
            {
                int index = Data_ItemInBag.IndexOf(oldItemData);
                Data_ItemInBag.Set(index, newItemData);
                if (Data_ItemInHand.Item_ID == oldItemData.Item_ID)
                {
                    if(Data_ItemInHand.Item_Seed == oldItemData.Item_Seed)
                    {
                        Data_ItemInHand = newItemData;
                        Debug.Log("物品修改成功");
                        Debug.Log("旧" + oldItemData.Item_ID + "/" + oldItemData.Item_Val + "/" + oldItemData.Item_Count);
                        Debug.Log("新" + newItemData.Item_ID + "/" + newItemData.Item_Val + "/" + newItemData.Item_Count);
                    }
                }
            }
            else
            {
                Debug.Log("物品修改失败,未找到目标物品:" + oldItemData);
            }
        }
    }

    #endregion

}
public struct ActorNetData : INetworkStruct
{
    public int Speed;
    public int MaxSpeed;
    public int Endurance;
}
