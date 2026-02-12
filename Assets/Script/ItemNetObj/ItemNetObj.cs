using Fusion;
using System;
using UnityEngine;
using System.Collections;
public class ItemNetObj : NetworkBehaviour
{
    [Header("物品节点")]
    public Transform transform_Root;
    [Header("物品图标")]
    public SpriteRenderer spriteRenderer_Icon;
    [Header("物品数量")]
    public TextMesh textMesh_Count;
    public override void Spawned()
    {
        base.Spawned();
        if (Net_data.I != new ItemData().I)
        {
            UpdateItem();
        }
    }
    #region//物品信息
    [Networked, OnChangedRender(nameof(UpdateItem)), HideInInspector]
    public ItemData Net_data { get; set; } = new ItemData();
    private ItemBase _bindItem;

    public virtual void UpdateItem()
    {
        if (_bindItem == null || _bindItem.itemData.I != Net_data.I)
        {
            Type type = Type.GetType("Item_" + Net_data.I.ToString());
            _bindItem = (ItemBase)Activator.CreateInstance(type);
        }
        _bindItem.UpdateDataFromNet(Net_data);
        _bindItem.NetObj_Draw(this, Net_data);
        _bindItem.NetObj_PlayDrop(this);
    }

    #endregion
    #region//物品所有者
    [Networked, OnChangedRender(nameof(UpdateOwner)), HideInInspector]
    public NetworkId Net_Owner { get; set; } = new NetworkId();
    public NetworkId Client_Owner = new NetworkId();
    public void UpdateOwner()
    {
        Client_Owner = Net_Owner;
    }
    /// <summary>
    /// 服务器_绑定所有者
    /// </summary>
    public virtual void State_BindOwner(NetworkId networkId)
    {
        Net_Owner = networkId;
    }
    /// <summary>
    /// 服务器_解除所有者
    /// </summary>
    public virtual void State_RsetOwner()
    {
        StartCoroutine(State_RmoveOwner());
    }
    private IEnumerator State_RmoveOwner()
    {
        yield return new WaitForSeconds(1f);
        Net_Owner = new NetworkId();
    }
    #endregion
    #region//物品操作
    /// <summary>
    /// 合并距离
    /// </summary>
    private float radiu_Combine = 2;
    /// <summary>
    /// 服务器_合并
    /// </summary>
    public virtual void State_CombineItem()
    {
        if (Object.HasStateAuthority)
        {
            var items = Physics2D.OverlapCircleAll(transform.position, radiu_Combine, LayerMask.GetMask("ItemObj"));
            foreach (Collider2D item in items)
            {
                if (item.gameObject.transform.parent.TryGetComponent(out ItemNetObj obj))
                {
                    if (obj.Equals(this)) { continue; }
                    if (obj.Net_data.I == Net_data.I)
                    {
                        Debug.Log(obj);
                        obj.Net_data = GameToolManager.Instance.CombineItem(obj.Net_data, Net_data, out ItemData itemData_Res);
                        if (itemData_Res.C == 0 || itemData_Res.I == 0)
                        {
                            Runner.Despawn(Object);
                        }
                        break;
                    }
                    else
                    {
                        Debug.Log(obj.Net_data.I + "/" + Net_data.I);
                    }
                }
            }
        }
    }
    /// <summary>
    /// 服务器_捡起
    /// </summary>
    /// <param name="itemData"></param>
    public virtual void State_PickUp(NetworkId networkId, out ItemData itemData)
    {
        itemData = Net_data;
        Runner.Despawn(Object);
    }
    /// <summary>
    /// 服务器_初始化
    /// </summary>
    public virtual void State_Init(ItemData itemData)
    {
        Net_data = itemData;
    }
    #endregion
}
