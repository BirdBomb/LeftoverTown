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

public class ItemNetObj : NetworkBehaviour
{
    [Header("物品图标")]
    public SpriteRenderer icon;
    [Header("物品根部")]
    public SortingGroup root;
    [Networked, OnChangedRender(nameof(UpdateItem))]
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
            if (_bindItem.data.Item_ID != data.Item_ID)
            {
                Type type = Type.GetType("Item_" + data.Item_ID.ToString());
                _bindItem = (ItemBase)Activator.CreateInstance(type);
            }
        }
        _bindItem.UpdateData(data);
        _bindItem.DrawItemObj(this);
        _bindItem.PlayDropAnim(this);
    }
    public virtual void PickUp(out ItemData itemData)
    {
        itemData = data;
    }
}
