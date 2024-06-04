using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Grid_OnHead : UI_Grid
{
    [SerializeField, Header("头部格子")]
    private UI_GridCell cell;
    private ItemData itemData;

    private void Start()
    {
        MessageBroker.Default.Receive<UIEvent.UIEvent_UpdateItemOnHead>().Subscribe(_ =>
        {
            itemData = _.itemData;
            if (itemData.Item_ID == 0)
            {
                ResetCell();
            }
            else
            {
                DrawCell();
            }
        }).AddTo(this);
    }
    private void DrawCell()
    {
        cell.UpdateGridCell(itemData);
        cell.BindClickAction(ClickCellLeft, ClickCellRight);
        cell.BindDragAction(CellDragBegin, CellDragIn, CellDragEnd);
    }
    private void ResetCell()
    {
        cell.ClearGridCell();
    }
    public void ClickCellLeft(UI_GridCell gridCell)
    {
       
    }
    public void ClickCellRight(UI_GridCell gridCell)
    {
        
    }
    public override void CellDragBegin(UI_GridCell gridCell, ItemData itemData, PointerEventData pointerEventData)
    {

    }
    public override void CellDragIn(UI_GridCell gridCell, ItemData itemData, PointerEventData pointerEventData)
    {
        gridCell.image_Icon.transform.position = Input.mousePosition;
    }
    public override void CellDragEnd(UI_GridCell gridCell, ItemData itemData, PointerEventData pointerEventData)
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryRemoveItemOnHead()
        {
            item = itemData
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
    public override void ListenDragOn<T>(T grid, UI_GridCell cell, ItemData itemData)
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemOnHead()
        {
            item = itemData
        });
    }
}
