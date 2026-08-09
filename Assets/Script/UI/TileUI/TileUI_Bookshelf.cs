using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class TileUI_Bookshelf : TileUI_BoxBase
{
    public override void PutIn(ItemData itemData_Add, ItemPath path)
    {
        ItemConfig itemConfig = ItemConfigData.GetItemConfig(itemData_Add.I);
        if(itemConfig.Item_Type == ItemType.Book)
        {
            base.PutIn(itemData_Add, path);
        }
        else
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Add()
            {
                itemData = itemData_Add,
                itemFrom = ItemFrom.OutSide
            });
        }
    }
}
