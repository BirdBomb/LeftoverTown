using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using static UnityEditor.PlayerSettings;
using UniRx;
using static Unity.Collections.Unicode;
using static UnityEditor.Progress;
using System;
/// <summary>
/// 角色网络管理器
/// </summary>
public class ActorNetManager : NetworkBehaviour
{
    [Header("位置同步组件")]
    public NetworkTransform NetTransform;
    [Header("本地角色组件")]
    public ActorManager LocalManager;

    public override void Spawned()
    {
        LocalManager.InitByNetManager(Object.HasStateAuthority);
        OnItemInBagChange();
        OnItemInHandChange();
        base.Spawned();
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
    #region//Networked

    [Networked, OnChangedRender(nameof(OnHpChange))]
    public int Data_Hp { get; set; }
    [Networked, OnChangedRender(nameof(OnMaxHpChange))]
    public int Data_MaxHp{ get; set; }
    [Networked, OnChangedRender(nameof(OnItemInHandChange))]
    public ItemData Data_ItemInHand { get; set; }
    [Networked,Capacity(20), OnChangedRender(nameof(OnItemInBagChange))]
    public NetworkLinkedList<ItemData> Data_ItemInBag { get; }

    [Networked, OnChangedRender(nameof(OnBuffsChange))]
    public NetworkLinkedList<int> Data_Buffs { get; }

    public void OnHpChange()
    {
        if (LocalManager.actorState != ActorState.Dead)
        {
            LocalManager.ActorUI.UpdateHPBar((float)Data_Hp / (float)Data_MaxHp);
            if (Data_Hp <= 0)
            {
                LocalManager.PlayDead(1);
            }
        }
    }
    public void OnMaxHpChange()
    {

    }
    public void OnItemInHandChange()
    {
        LocalManager.AddItem_Hand(Data_ItemInHand);
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
    /// RPC:添加物体到背包
    /// </summary>
    /// <param name="itemData"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_AddItemInBag(ItemData itemData)
    {
        if (Object.HasStateAuthority)
        {
            Data_ItemInBag.Add(itemData);
        }
    }
    /// <summary>
    /// RPC:添加物体到背包
    /// </summary>
    /// <param name="itemData"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_RemoveItemFromBag(ItemData itemData)
    {
        if (Object.HasStateAuthority)
        {
            Data_ItemInBag.Remove(itemData);
        }
    }


    /// <summary>
    /// RPC:本地端拾起一个物品
    /// </summary>
    /// <param name="id"></param>
    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.All)]
    public void RPC_Local_PickItem(NetworkId id)
    {
        if (Object.HasStateAuthority)
        {
            LocalManager.PlayPickUp(1, (string name) =>
            {
                if(name == "PickUp")
                {
                    NetworkObject networkPlayerObject = Runner.FindObject(id);
                    networkPlayerObject.GetComponent<ItemNetObj>().PickUp(out ItemData data);
                    Runner.Despawn(networkPlayerObject);
                    RPC_AddItemInBag(data);
                }
            });
        }
        else
        {
            LocalManager.PlayPickUp(1,null);
        }
    }
    /// <summary>
    /// RPC:本地端持握一个物品
    /// </summary>
    /// <param name="itemData"></param>
    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.All)]
    public void RPC_Local_HoldItem(ItemData itemData)
    {
        if (Object.HasStateAuthority)
        {
            Data_ItemInHand = itemData;
        }
    }
    /// <summary>
    /// RPC:本地端失去一个物品
    /// </summary>
    /// <param name="itemData"></param>
    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.All)]
    public void RPC_Local_LoseItem(ItemData itemData)
    {
        if (Object.HasStateAuthority)
        {
            RPC_RemoveItemFromBag(itemData);
        }
    }


    #endregion

}
