using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using UnityEngine.U2D;
using UnityEngine.XR;
using static Unity.Collections.Unicode;
using UnityEngine.EventSystems;
using System;

public class UI_GameSenceUI : UI_Grid
{
    [Header("手部槽位")]
    public UI_GridCell _handCellList;
    [Header("生命值")]
    public Text Text_Hp;
    [Header("饥饿值")]
    public Text Text_Food;
    [Header("缺水值")]
    public Text Text_Water;
    private void Start()
    {
        MessageBroker.Default.Receive<UIEvent.UIEvent_UpdateItemInHand>().Subscribe(_ =>
        {
            UpdateHandSlot(_.itemData);
        }).AddTo(this);
        MessageBroker.Default.Receive<UIEvent.UIEvent_UpdateItemInBag>().Subscribe(_ =>
        {
            _bagItemDataList.Clear();
            for (int i = 0; i < _.itemDatas.Count; i++)
            {
                _bagItemDataList.Add(_.itemDatas[i]);
            }
            BagUpdateItem();

        }).AddTo(this);
        MessageBroker.Default.Receive<UIEvent.UIEvent_UpdateData>().Subscribe(_ =>
        {
            Text_Hp.text = _.HP.ToString();
            Text_Food.text = _.Food.ToString();
            Text_Water.text = _.Water.ToString();
        }).AddTo(this);
    }
    private void UpdateHandSlot(ItemData item)
    {
        _handCellList.UpdateGridCell(item);
    }
    #region//背包
    [SerializeField,Header("背包槽位")]
    private List<UI_GridCell> _bagCellList = new List<UI_GridCell>();
    private List<ItemData> _bagItemDataList = new List<ItemData>();
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
            if (i < _bagItemDataList.Count)
            {
                DrawCell(_bagCellList[i], _bagItemDataList[i]);
            }
            else
            {
                ResetCell(_bagCellList[i]);
            }
        }
    }
    private void ResetCell(UI_GridCell cell)
    {
        cell.ClearGridCell();
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
        gridCell._bindItem.LeftClickGridCell(gridCell, gridCell._bindItem.data);
    }
    public void ClickCellRight(UI_GridCell gridCell)
    {
        gridCell._bindItem.RightClickGridCell(gridCell, gridCell._bindItem.data);
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
        pointerEventData.position = Input.mousePosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);
        if (raycastResults.Count > 0)
        {
            foreach (RaycastResult result in raycastResults)
            {
                if(result.gameObject.TryGetComponent(out UI_Grid grid))
                {
                    grid.ListenDragOn(this, gridCell, itemData);
                    return;
                }
            }
        }
        else
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryDropItem()
            {
                item = itemData
            });
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryRemoveItemFromBag()
            {
                item = itemData
            });
        }
    }
    public override void ListenDragOn<T> (T grid,UI_GridCell cell, ItemData itemData)
    {
        if (grid.GetType() == typeof(UI_Grid_Cabinet))
        {
            UI_Grid_Cabinet cabinet = grid as UI_Grid_Cabinet;
            cabinet.PutOut(itemData);
        }
        if (grid.GetType() == typeof(UI_Build))
        {
            UI_Build cabinet = grid as UI_Build;
            cabinet.PutOut(itemData);
        }
    }
    #endregion
}
