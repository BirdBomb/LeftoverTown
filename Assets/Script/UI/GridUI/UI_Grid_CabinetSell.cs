using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Grid_CabinetSell : UI_Grid
{
    [SerializeField, Header("售卖格子")]
    private UI_GridCell cell_Sell = new UI_GridCell();
    [SerializeField, Header("售卖价格")]
    private TextMeshProUGUI text_Price = new TextMeshProUGUI();
    [SerializeField, Header("售卖按钮")]
    private Button btn_Sell;
    private ItemData itemData_InSell = new ItemData();
    private int itemPrice_InSell;
    private TileObj bindTileObj;
    private void Start()
    {
        btn_Sell.onClick.AddListener(Sell);
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
        itemData_InSell = new ItemData();
        string[] strings = info.Split("/*I*/");
        for (int i = 0; i < strings.Length; i++)
        {
            if (strings[i] != "")
            {
                ItemData data = JsonUtility.FromJson<ItemData>(strings[i]);
                UpdateSellItem(data);
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
        builder.Append(JsonUtility.ToJson(itemData_InSell));
        bindTileObj.TryToChangeInfo(builder.ToString());
    }
    #endregion
    #region//UI绘制
    /// <summary>
    /// 绘制所有格子
    /// </summary>
    private void DrawEveryCell()
    {
        if (itemData_InSell.Item_ID != 0)
        {
            ShowSellBtn();
            DrawCell(itemData_InSell, cell_Sell);
        }
        else
        {
            HideSellBtn();
            ResetCell(cell_Sell);
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
    /// <summary>
    /// 显示售卖按钮
    /// </summary>
    private void ShowSellBtn()
    {
        btn_Sell.gameObject.SetActive(true);
        btn_Sell.transform.DOKill();
        btn_Sell.transform.localScale = Vector3.one;
        btn_Sell.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.2f);
    }
    /// <summary>
    /// 隐藏售卖按钮
    /// </summary>
    private void HideSellBtn()
    {
        btn_Sell.gameObject.SetActive(false);
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
    #region//放入取出
    /*取出*/
    public override void PutOut(ItemData before, out ItemData after)
    {
        itemData_InSell = new ItemData();
        after = before;
        ChangeInfoToTile();
    }
    /*放入*/
    public override void PutIn(ItemData before, out ItemData after)
    {
        ItemConfig config = ItemConfigData.GetItemConfig(before.Item_ID);
        ItemData resData = before;
        if (config.Item_Size == ItemSize.AsGroup)
        {
            if (itemData_InSell.Item_ID == 0)
            {
                ItemData emptyData = resData;
                emptyData.Item_Count = 0;
                Type type = Type.GetType("Item_" + resData.Item_ID.ToString());
                ((ItemBase)Activator.CreateInstance(type)).StaticAction_PileUp(emptyData, resData, config.Item_MaxCount, out ItemData newData, out resData);
                itemData_InSell = newData;
            }
            else if (itemData_InSell.Item_ID == resData.Item_ID)
            {
                Type type = Type.GetType("Item_" + resData.Item_ID.ToString());
                ((ItemBase)Activator.CreateInstance(type)).StaticAction_PileUp(itemData_InSell, resData, config.Item_MaxCount, out ItemData newData, out resData);
                itemData_InSell = newData;
            }
        }
        else
        {
            if (itemData_InSell.Item_ID == 0)
            {
                ItemData emptyData = resData;
                emptyData.Item_Count = 0;
                Type type = Type.GetType("Item_" + resData.Item_ID.ToString());
                ((ItemBase)Activator.CreateInstance(type)).StaticAction_PileUp(emptyData, resData, config.Item_MaxCount, out ItemData newData, out resData);
                itemData_InSell = newData;
            }
        }
        after = resData;
        ChangeInfoToTile();
    }

    #endregion
    #region//出售
    /// <summary>
    /// 更新售卖物体
    /// </summary>
    /// <param name="data"></param>
    private void UpdateSellItem(ItemData data)
    {
        itemData_InSell = data;
        if (data.Item_ID != 0)
        {
            int val = (int)ItemConfigData.GetItemConfig(data.Item_ID).Average_Value * data.Item_Count;
            itemPrice_InSell = val;
            text_Price.text = itemPrice_InSell.ToString();
        }
        else
        {
            itemPrice_InSell = 0;
            text_Price.text = "";
        }
    }
    /// <summary>
    /// 售卖
    /// </summary>
    private void Sell()
    {
        if (itemData_InSell.Item_ID != 0)
        {
            GameLocalManager.Instance.localPlayer.actorManager.AllClient_EarnCoin(itemPrice_InSell);
            itemData_InSell = new ItemData();
            AudioManager.Instance.PlayEffect(1006, bindTileObj.transform.position);
            ChangeInfoToTile();
        }
    }
    #endregion
}
