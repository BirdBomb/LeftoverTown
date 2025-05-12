using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.U2D;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Rendering;
using System.Globalization;
using System;
using Unity.VisualScripting;
using static UnityEngine.GraphicsBuffer;

public class ItemNetObj : NetworkBehaviour
{
    [Header("物品节点")]
    public Transform transform_Root;
    [Header("物品图标")]
    public SpriteRenderer spriteRenderer_Icon;
    [Header("物品数量")]
    public TextMesh textMesh_Count;
    [Networked, OnChangedRender(nameof(UpdateItem)), HideInInspector]
    public ItemData data { get; set; } = new ItemData();
    private ItemBase _bindItem;
    public override void Spawned()
    {
        base.Spawned();
        if (data.Item_ID != new ItemData().Item_ID)
        {
            UpdateItem();
        }
    }
    public virtual void UpdateItem()
    {
        if (_bindItem == null)
        {
            Type type = Type.GetType("Item_" + data.Item_ID.ToString());
            _bindItem = (ItemBase)Activator.CreateInstance(type);
        }
        else
        {
            if (_bindItem.itemData.Item_ID != data.Item_ID)
            {
                Type type = Type.GetType("Item_" + data.Item_ID.ToString());
                _bindItem = (ItemBase)Activator.CreateInstance(type);
            }
        }
        _bindItem.UpdateDataFromNet(data);
        _bindItem.NetObj_Draw(this, data);
        _bindItem.NetObj_PlayDrop(this);
    }
    public virtual void CombineItem()
    {
        if (Object.HasStateAuthority)
        {
            var items = Physics2D.OverlapCircleAll(transform.position, 1, LayerMask.GetMask("ItemObj"));
            foreach (Collider2D item in items)
            {
                if (item.gameObject.transform.parent.TryGetComponent(out ItemNetObj obj))
                {
                    if (obj.Equals(this)) { continue; }
                    if (obj.data.Item_ID == data.Item_ID)
                    {
                        Debug.Log(obj);
                        obj.data = GameToolManager.Instance.CombineItem(obj.data, data, out ItemData itemData_Res);
                        if(itemData_Res.Item_Count == 0 || itemData_Res.Item_ID == 0)
                        {
                            Runner.Despawn(Object);
                        }
                        break;
                    }
                    else
                    {
                        Debug.Log(obj.data.Item_ID + "/" + data.Item_ID);
                    }
                }
            }

        }
    }
    public virtual void PickUp(out ItemData itemData)
    {
        itemData = data;
    }
}
