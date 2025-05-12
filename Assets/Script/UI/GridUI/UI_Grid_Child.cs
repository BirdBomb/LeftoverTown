using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// 子集格子，用于物品嵌套
/// </summary>
public class UI_Grid_Child : UI_Grid
{
    public UI_GridCell gridCell;
    public bool _open;
    private ItemData itemData_oldContainer;
    private ItemData itemData_newContainer;
    public void BindCell(ItemPath itemPath)
    {
        gridCell.BindGrid(itemPath, PutIn, PutOut, null, null);
    }
    public void OpenOrCloseGrid(ItemData oldContainer)
    {
        if (_open)
        {
            CloseGrid();
        }
        else
        {
            OpenGrid(oldContainer);
        }
    }
    public void OpenGrid(ItemData oldContainer)
    {
        itemData_oldContainer = oldContainer;
        _open = true;
        gameObject.SetActive(true);
        DrawCell(itemData_oldContainer);
    }
    public void CloseGrid()
    {
        _open = false;
        gameObject.SetActive(false);
        gridCell.CleanItemBase();
    }
    public void UpdateGrid(ItemData oldContainer)
    {
        itemData_oldContainer = oldContainer;
    }

    public void DrawCell(ItemData itemData)
    {
        ItemData itemInContainer = new ItemData(itemData.Item_Content);
        if (itemInContainer.Item_ID != 0 && itemInContainer.Item_Count > 0)
        {
            gridCell.UpdateData(itemInContainer);
        }
        else
        {
            gridCell.CleanItemBase();
        }
    }
    public void PutIn(ItemData addItem, ItemPath itemPath)
    {
        Type type = Type.GetType("Item_" + itemData_oldContainer.Item_ID.ToString());
        ((ItemBase)Activator.CreateInstance(type)).StaticAction_FillUp(itemData_oldContainer, addItem, out itemData_newContainer, out ItemData resItem);
        switch (itemPath.itemFrom)
        {
            case ItemFrom.Bag:
                MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemInBag()
                {
                    itemData = itemData_newContainer,
                    index = itemPath.itemIndex
                });
                break;
            case ItemFrom.Hand:
                MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemOnHand()
                {
                    oldItem = itemData_oldContainer,
                    newItem = itemData_newContainer,
                });
                break;
            case ItemFrom.Head:
                MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemOnHead()
                {
                    oldItem = itemData_oldContainer,
                    newItem = itemData_newContainer,
                });
                break;
            case ItemFrom.Body:
                MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemOnBody()
                {
                    oldItem = itemData_oldContainer,
                    newItem = itemData_newContainer,
                });
                break;
        }

        if (resItem.Item_ID > 0 && resItem.Item_Count > 0)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
            {
                itemData = resItem,
            });
        }
        itemData_oldContainer = itemData_newContainer;
        DrawCell(itemData_oldContainer);
    }
    public ItemData PutOut(ItemData itemData_From, ItemData itemData_Out, ItemPath itemPath)
    {
        if (itemData_oldContainer.Item_Content.Item_ID == itemData_Out.Item_ID)
        {
            ItemData res = GameToolManager.Instance.SplitItem(itemData_From, itemData_Out);
            itemData_newContainer = itemData_oldContainer;
            itemData_newContainer.Item_Content = new ContentData(res);
            switch (itemPath.itemFrom)
            {
                case ItemFrom.Bag:
                    MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemInBag()
                    {
                        itemData = itemData_newContainer,
                        index = itemPath.itemIndex
                    });
                    break;
                case ItemFrom.Hand:
                    MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemOnHand()
                    {
                        oldItem = itemData_oldContainer,
                        newItem = itemData_newContainer,
                    });
                    break;
                case ItemFrom.Head:
                    MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemOnHead()
                    {
                        oldItem = itemData_oldContainer,
                        newItem = itemData_newContainer,
                    });
                    break;
                case ItemFrom.Body:
                    MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemOnBody()
                    {
                        oldItem = itemData_oldContainer,
                        newItem = itemData_newContainer,
                    });
                    break;
            }

            itemData_oldContainer = itemData_newContainer;
            DrawCell(itemData_oldContainer);
        }
        return itemData_Out;
    }
}
