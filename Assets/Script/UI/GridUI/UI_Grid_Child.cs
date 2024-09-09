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
    public UI_GridCell cell;
    /// <summary>
    /// 旧容器
    /// </summary>
    private ItemData oldContainerData;
    /// <summary>
    /// 新容器
    /// </summary>
    private ItemData newContainerData;
    private Func<ItemData, ItemData> tryToPutIn;
    public void OpenGrid(ItemData oldContainer, Func<ItemData,ItemData> cal)
    {
        gameObject.SetActive(true);
        oldContainerData = oldContainer;
        tryToPutIn = cal;
        DrawCell();
    }
    public void Close()
    {
        gameObject.SetActive(false);
        cell.ResetGridCell();
        tryToPutIn = null;
    }
    public override void PutIn(ItemData putInItem, out ItemData residueItem)
    {
        if (tryToPutIn != null)
        {
            residueItem = tryToPutIn(putInItem);
        }
        else
        {
            residueItem = putInItem;
        }
    }

    public void DrawCell()
    {
        ItemData itemInContainer = new ItemData(oldContainerData.Item_Content);
        if (itemInContainer.Item_ID != 0 && itemInContainer.Item_Count > 0)
        {
            cell.UpdateGridCell(itemInContainer);
            cell.BindDragAction(CellDragBegin, CellDragIn, CellDragEnd);
        }
        else
        {
            cell.ResetGridCell();
        }
    }
    public override void CellDragBegin(UI_GridCell gridCell, ItemData itemData, PointerEventData pointerEventData)
    {
        base.CellDragBegin(gridCell, itemData, pointerEventData);
    }
    public override void CellDragIn(UI_GridCell gridCell, ItemData itemData, PointerEventData pointerEventData)
    {
        gridCell.image_MainIcon.transform.position = Input.mousePosition;
        base.CellDragIn(gridCell, itemData, pointerEventData);
    }
    public override void CellDragEnd(UI_GridCell gridCell, ItemData itemData, PointerEventData pointerEventData)
    {
        newContainerData = oldContainerData;
        newContainerData.Item_Content = new ContentData(new ItemData());
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemInBag()
        {
            oldItem = oldContainerData,
            newItem = newContainerData,
        });
        pointerEventData.position = Input.mousePosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);
        if (raycastResults.Count > 0)
        {
            foreach (RaycastResult result in raycastResults)
            {
                if (result.gameObject.TryGetComponent(out UI_Grid grid))
                {
                    grid.ListenDragOn(this, gridCell, itemData);
                    return;
                }
            }
        }
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryDropItem()
        {
            item = itemData
        });
    }
    public override void ListenDragOn<T>(T grid, UI_GridCell cell, ItemData putInItem)
    {
        PutIn(putInItem, out ItemData residueItem);
        base.ListenDragOn<T>(grid, cell, putInItem);
    }
}
