using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Grid_Refrigerator : UI_Grid
{
    [SerializeField, Header("格子列表")]
    private List<UI_GridCell> cellList = new List<UI_GridCell>();
    private List<ItemData> itemDataList = new List<ItemData>();

    private TileObj bindTileObj;
    public void Start()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_Local_TimeChange>().Subscribe(_ =>
        {
            DrawEveryCell();
        }).AddTo(this);
    }

    #region//打开关闭
    public override void Open(TileObj tileObj)
    {
        bindTileObj = tileObj;
        base.Open(tileObj);
    }
    public override void Close(TileObj tileObj)
    {
        base.Open(tileObj);
    }

    #endregion
    #region//信息更新与上传
    /// <summary>
    /// 从地块获取更新
    /// </summary>
    /// <param name="info"></param>
    public void UpdateInfoFromTile(string info)
    {
        itemDataList.Clear();
        string[] strings = info.Split("/*I*/");
        for (int i = 0; i < strings.Length; i++)
        {
            if (strings[i] != "")
            {
                ItemData data = JsonUtility.FromJson<ItemData>(strings[i]);
                itemDataList.Add(data);
            }
        }
        DrawEveryCell();
    }
    /// <summary>
    /// 改变更新给地块
    /// </summary>
    public void ChangeInfoToTile()
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < itemDataList.Count; i++)
        {
            if (i == 0)
            {
                builder.Append(JsonUtility.ToJson(itemDataList[i]));
            }
            else
            {
                builder.Append("/*I*/" + JsonUtility.ToJson(itemDataList[i]));
            }
        }
        bindTileObj.TryToChangeInfo(builder.ToString());
    }
    #endregion
    #region//UI绘制
    /// <summary>
    /// 绘制所有格子
    /// </summary>
    private void DrawEveryCell()
    {
        for (int i = 0; i < cellList.Count; i++)
        {
            if (i < itemDataList.Count)
            {
                DrawCell(itemDataList[i], cellList[i]);
            }
            else
            {
                ResetCell(cellList[i]);
            }
        }
    }
    /// <summary>
    /// 绘制一个格子
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="config"></param>
    private void DrawCell(ItemData data, UI_GridCell cell)
    {
        cell.UpdateGridCell(data);
        cell.BindClickAction(ClickCellLeft, ClickCellRight);
        cell.BindDragAction(CellDragBegin, CellDragIn, CellDragEnd);
    }
    /// <summary>
    /// 重置一个格子
    /// </summary>
    /// <param name="cell"></param>
    private void ResetCell(UI_GridCell cell)
    {
        cell.ResetGridCell();
    }

    #endregion
    #region//UI交互
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
    #region//取出放入
    /*取出*/
    public override void PutOut(ItemData before, out ItemData after)
    {
        itemDataList.Remove(before);
        ItemConfig config = ItemConfigData.GetItemConfig(before.Item_ID);
        if (config.Item_Type == ItemType.Ingredient || config.Item_Type == ItemType.Food)
        {
            before.Item_Info = 100;
        }
        after = before;
        ChangeInfoToTile();
    }
    /*放入*/
    public override void PutIn(ItemData before, out ItemData after)
    {
        ItemConfig config = ItemConfigData.GetItemConfig(before.Item_ID);
        if (config.Item_Type == ItemType.Ingredient || config.Item_Type == ItemType.Food)
        {
            before.Item_Info = 0;
        }
        ItemData resData = before;
        if (config.Item_Size == ItemSize.AsGroup)
        {
            for (int i = 0; i < cellList.Count; i++)
            {
                if (itemDataList.Count > i)
                {
                    if (itemDataList[i].Item_ID == resData.Item_ID)
                    {
                        Type type = Type.GetType("Item_" + itemDataList[i].Item_ID.ToString());
                        ((ItemBase)Activator.CreateInstance(type)).StaticAction_PileUp(itemDataList[i], resData, config.Item_MaxCount, out ItemData newData, out resData);
                        itemDataList[i] = newData;
                    }
                }
                else
                {
                    if (resData.Item_Count > 0)
                    {
                        ItemData emptyData = resData;
                        emptyData.Item_Count = 0;
                        Type type = Type.GetType("Item_" + resData.Item_ID.ToString());
                        ((ItemBase)Activator.CreateInstance(type)).StaticAction_PileUp(emptyData, resData, config.Item_MaxCount, out ItemData newData, out resData);
                        itemDataList.Add(newData);
                    }
                }
            }
        }
        else
        {
            if (itemDataList.Count < cellList.Count)
            {
                ItemData emptyData = resData;
                emptyData.Item_Count = 0;
                Type type = Type.GetType("Item_" + resData.Item_ID.ToString());
                ((ItemBase)Activator.CreateInstance(type)).StaticAction_PileUp(emptyData, resData, config.Item_MaxCount, out ItemData newData, out resData);
                itemDataList.Add(newData);
            }
        }
        after = resData;
        ChangeInfoToTile();
    }


    #endregion
}
