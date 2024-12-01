using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Grid_Bag : UI_Grid
{
    [SerializeField, Header("背包槽位")]
    private List<UI_GridCell> _bagCellList = new List<UI_GridCell>();
    [SerializeField, Header("背包锁")]
    private List<Image> images_bagLockList = new List<Image>();
    private List<ItemData> itemDatas_bagItemDataList = new List<ItemData>();
    private int _bagCapacity;
    private void Start()
    {
        MessageBroker.Default.Receive<UIEvent.UIEvent_UpdateItemInBag>().Subscribe(_ =>
        {
            _bagCapacity = _.bagCapacity;
            itemDatas_bagItemDataList.Clear();
            for (int i = 0; i < _.itemDatas.Count; i++)
            {
                itemDatas_bagItemDataList.Add(_.itemDatas[i]);
            }
            BagUpdateItem();
            BagDrawEveryLock();

        }).AddTo(this);
        MessageBroker.Default.Receive<GameEvent.GameEvent_Local_TimeChange>().Subscribe(_ =>
        {
            BagUpdateItem();
        }).AddTo(this);
    }
    /// <summary>
    /// 更新背包物体
    /// </summary>
    private void BagUpdateItem()
    {
        BagDrawEveryCell();
    }
    private void BagDrawEveryCell()
    {
        for (int i = 0; i < _bagCellList.Count; i++)
        {
            if (i < itemDatas_bagItemDataList.Count)
            {
                DrawCell(_bagCellList[i], itemDatas_bagItemDataList[i]);
            }
            else
            {
                ResetCell(_bagCellList[i]);
            }
        }
    }
    private void BagDrawEveryLock()
    {
        for (int i = 0; i < images_bagLockList.Count; i++)
        {
            if (i < _bagCapacity)
            {
                images_bagLockList[i].enabled = false;
            }
            else
            {
                images_bagLockList[i].enabled = true;
            }
        }

    }
    private void ResetCell(UI_GridCell cell)
    {
        cell.ResetGridCell();
    }
    /// <summary>
    /// 绘制一个格子
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="data"></param>
    private void DrawCell(UI_GridCell cell, ItemData data)
    {
        cell.UpdateGridCell(data);
        cell.BindClickAction(ClickCellLeft, ClickCellRight);
        cell.BindDragAction(CellDragBegin, CellDragIn, CellDragEnd);
    }
    public void ClickCellLeft(UI_GridCell gridCell)
    {
        gridCell._bindItem.LeftClickGridCell(gridCell, gridCell._bindItem.itemData);
    }
    public void ClickCellRight(UI_GridCell gridCell)
    {
        gridCell._bindItem.RightClickGridCell(gridCell, gridCell._bindItem.itemData);
    }
    public override void CellDragBegin(UI_GridCell gridCell, ItemData itemData, PointerEventData pointerEventData)
    {

    }
    public override void CellDragIn(UI_GridCell gridCell, ItemData itemData, PointerEventData pointerEventData)
    {
        gridCell.image_MainIcon.transform.position = Input.mousePosition;
    }
    public override void CellDragEnd(UI_GridCell gridCell, ItemData itemData, PointerEventData pointerEventData)
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryRemoveItemFromBag()
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
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
        {
            item = itemData,
        });
    }
}
