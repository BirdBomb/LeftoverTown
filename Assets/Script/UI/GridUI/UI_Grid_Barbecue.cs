using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Grid_Barbecue : UI_Grid
{
    [SerializeField, Header("烧烤格子")]
    private UI_GridCell cell_Barbecue;
    [SerializeField, Header("烧烤进度条")]
    private Image image_Bar;
    private ItemData itemData_Barbecue;
    private TileObj bindTileObj;
    private short barbecueVal;
    private Action<ItemData, short> action_AddBarbecue;
    public void BindAction(Action<ItemData, short> action)
    {
        action_AddBarbecue = action;
    }
    #region//打开关闭
    public override void Open(TileObj tileObj)
    {
        bindTileObj = tileObj;
        base.Open(tileObj);
    }
    public override void Close(TileObj tileObj)
    {
        bindTileObj = tileObj;
        base.Close(tileObj);
    }
    #endregion
    #region//UI交互
    public override void CellDragBegin(UI_GridCell gridCell, ItemData itemData, PointerEventData pointerEventData)
    {

    }
    public override void CellDragIn(UI_GridCell gridCell, ItemData itemData, PointerEventData pointerEventData)
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(gridCell.image_MainIcon.rectTransform, Input.mousePosition, Camera.main, out Vector3 pos);
        gridCell.image_MainIcon.transform.position = pos;
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
                if (result.gameObject.TryGetComponent(out UI_Grid grid))
                {
                    PutOut(itemData, out ItemData afterData);
                    grid.ListenDragOn(this, gridCell, afterData);
                    return;
                }
            }
        }
        else
        {
            PutOut(itemData, out ItemData afterData);
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryDropItem()
            {
                item = afterData
            });
        }

    }
    public override void ListenDragOn<T>(T grid, UI_GridCell cell, ItemData itemData)
    {
        PutIn(itemData, out ItemData back);
        if (back.Item_Count != 0)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
            {
                item = back,
            });
        }
        base.ListenDragOn<T>(grid, cell, itemData);
    }
    #endregion
    #region//UI管理
    /// <summary>
    /// 重置一个格子
    /// </summary>
    /// <param name="cell"></param>
    private void ResetCell(UI_GridCell cell)
    {
        cell.ResetGridCell();
    }
    /// <summary>
    /// 绘制一个格子
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="config"></param>
    private void DrawCell(ItemData data, UI_GridCell cell)
    {
        cell.UpdateGridCell(data);
        cell.BindDragAction(CellDragBegin, CellDragIn, CellDragEnd);
    }

    #endregion
    #region//放入取出
    public override void PutIn(ItemData before, out ItemData after)
    {
        ItemData resData = before;
        BarbecueConfig barbecue = BarbecueConfigData.GetBarbecueConfig(before.Item_ID);
        if (barbecue.BarbecueID != 0)
        {
            ItemData itemData = before;
            itemData.Item_Count = 0;
            Type type = Type.GetType("Item_" + before.Item_ID.ToString());
            ((ItemBase)Activator.CreateInstance(type)).StaticAction_PileUp(itemData, resData, 1, out ItemData newData, out resData);
            after = resData;
            itemData_Barbecue = newData;
            barbecueVal = barbecue.BarbecueVal;
            ChangeInfoToTile();
        }
        else
        {
            base.PutIn(before, out after);
        }
    }
    public override void PutOut(ItemData before, out ItemData after)
    {
        itemData_Barbecue = new ItemData();
        barbecueVal = 0;
        ChangeInfoToTile();
        base.PutOut(before, out after);
    }
    #endregion
    #region//上传更新
    public void UpdateInfoFromTile(short barbecueVal, short barbecueMax, ItemData barbecue)
    {
        itemData_Barbecue = barbecue;
        DrawCell(itemData_Barbecue, cell_Barbecue);
        if (barbecueMax != 0)
        {
            image_Bar.transform.DOKill();
            image_Bar.transform.DOScaleX(((float)barbecueVal / barbecueMax), 0.1f);
            if (barbecueVal == barbecueMax)
            {
                cell_Barbecue.DOKill();
                cell_Barbecue.transform.localScale = Vector3.one;
                cell_Barbecue.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
            }
        }
        else
        {
            image_Bar.transform.localScale = new Vector3(0, 1f, 1f);
        }
    }
    public void ChangeInfoToTile()
    {
        if (action_AddBarbecue != null)
        {
            action_AddBarbecue.Invoke(itemData_Barbecue, barbecueVal);
        };
        bindTileObj.TryToChangeInfo("");
    }
    #endregion
}
