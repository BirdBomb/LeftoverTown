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
    public void Start()
    {
        gridCell.BindAction(PutIn, PutOut, null, null);
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
        gridCell.CleanData();
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
            gridCell.CleanData();
        }
    }
    public void PutIn(ItemData addItem)
    {
        Type type = Type.GetType("Item_" + itemData_oldContainer.Item_ID.ToString());
        ((ItemBase)Activator.CreateInstance(type)).StaticAction_FillUp(itemData_oldContainer, addItem, out itemData_newContainer, out ItemData resItem);
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemInBag()
        {
            oldItem = itemData_oldContainer,
            newItem = itemData_newContainer,
        });
        if (resItem.Item_ID > 0 && resItem.Item_Count > 0)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
            {
                item = resItem,
            });
        }
        itemData_oldContainer = itemData_newContainer;
        DrawCell(itemData_oldContainer);
    }
    public ItemData PutOut(ItemData data)
    {
        if (itemData_oldContainer.Item_Content.Item_ID == data.Item_ID)
        {
            ItemData res = GameToolManager.Instance.PutOutItemSingle(new ItemData(itemData_oldContainer.Item_Content), data);
            itemData_newContainer = itemData_oldContainer;
            itemData_newContainer.Item_Content = new ContentData(res);
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemInBag()
            {
                oldItem = itemData_oldContainer,
                newItem = itemData_newContainer,
            });
            itemData_oldContainer = itemData_newContainer;
            DrawCell(itemData_oldContainer);
        }
        return data;
    }
}
