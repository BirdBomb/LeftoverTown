using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class TileUI_BoxFreeze : TileUI_BoxBase
{
    public override void PutIn(ItemData itemData_Add, ItemPath path)
    {
        itemData_Add = Freeze(itemData_Add);
        base.PutIn(itemData_Add, path);
    }
    public override ItemData PutOut(ItemData itemData_From, ItemData itemData_Out, ItemPath itemPath)
    {
        return Thaw(base.PutOut(itemData_From, itemData_Out, itemPath));
    }
    private ItemData Freeze(ItemData itemData)
    {
        ItemConfig itemConfig = ItemConfigData.GetItemConfig(itemData.I);
        if (itemConfig.Item_Type == ItemType.Food || itemConfig.Item_Type == ItemType.Dishes)
        {
            itemData.V = 0;
        }
        return itemData;
    }
    private ItemData Thaw(ItemData itemData)
    {
        ItemConfig itemConfig = ItemConfigData.GetItemConfig(itemData.I);
        if (itemConfig.Item_Type == ItemType.Food || itemConfig.Item_Type == ItemType.Dishes)
        {
            itemData.V = 100;
        }
        return itemData;
    }
}
