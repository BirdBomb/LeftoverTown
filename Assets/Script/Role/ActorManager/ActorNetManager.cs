using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using static UnityEditor.PlayerSettings;
using UniRx;
using static Unity.Collections.Unicode;
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
        Debug.Log("成功了吗" + Runner.SetIsSimulated(Object, true));
        base.Spawned();
    }
    public override void FixedUpdateNetwork()
    {
        Debug.Log("正在模拟");
        if (Object.HasStateAuthority || Object.HasInputAuthority)
        {
            LocalManager.FixedUpdateNetwork(Runner.DeltaTime);
        }
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
            NetTransform.Teleport(pos);
        }
    }
    #region//Networked

    [Networked, OnChangedRender(nameof(OnHpChange))]
    public int Data_Hp { get; set; }
    [Networked, OnChangedRender(nameof(OnMaxHpChange))]
    public int Data_MaxHp{ get; set; }
    [Networked, OnChangedRender(nameof(OnItemInHandChange))]
    public NetworkItemConfig Data_ItemInHand { get; set; }
    [Networked, OnChangedRender(nameof(OnItemInBagChange))]
    public NetworkLinkedList<NetworkItemConfig> Data_ItemInBag { get; }
    [Networked, OnChangedRender(nameof(OnBuffsChange))]
    public NetworkLinkedList<int> Data_Buffs { get; }

    public void OnHpChange()
    {

    }
    public void OnMaxHpChange()
    {

    }
    public void OnItemInHandChange()
    {

    }
    public void OnItemInBagChange()
    {

    }
    public void OnBuffsChange()
    {

    }

    #endregion
    #region//RPC
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPC_Skill(int parameter)
    {

    }
    #endregion
    private ItemConfig ItemConfigNetToLocal(NetworkItemConfig config)
    {
        ItemConfig itemConfig = new ItemConfig();
        itemConfig.Item_ID = config.Item_ID;
        itemConfig.Item_Name = config.Item_Name.Value;
        itemConfig.Item_Desc = config.Item_Desc.Value;
        itemConfig.Item_CurCount = config.Item_CurCount;
        itemConfig.Item_MaxCount = config.Item_MaxCount;
        itemConfig.Item_Type = config.Item_Type;
        itemConfig.Average_Weight = config.Average_Weight;
        itemConfig.Average_Value = config.Average_Value;
        itemConfig.Item_Info = config.Item_Info.Value;
        return itemConfig;
    }
    private NetworkItemConfig ItemConfigLocalToNet(ItemConfig config)
    {
        NetworkItemConfig itemConfig = new NetworkItemConfig();
        itemConfig.Item_ID = config.Item_ID;
        itemConfig.Item_Name = config.Item_Name;
        itemConfig.Item_Desc = config.Item_Desc;
        itemConfig.Item_CurCount = config.Item_CurCount;
        itemConfig.Item_MaxCount = config.Item_MaxCount;
        itemConfig.Item_Type = config.Item_Type;
        itemConfig.Average_Weight = config.Average_Weight;
        itemConfig.Average_Value = config.Average_Value;
        itemConfig.Item_Info = config.Item_Info;
        return itemConfig;
    }

}
