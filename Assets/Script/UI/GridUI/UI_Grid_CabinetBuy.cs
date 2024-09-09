using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Grid_CabinetBuy : UI_Grid
{
    [SerializeField, Header("格子列表")]
    private List<UI_GridCell> cellList = new List<UI_GridCell>();
    [SerializeField, Header("价格列表")]
    private List<TextMeshProUGUI> priceList = new List<TextMeshProUGUI>();
    [HideInInspector]
    public List<ItemData> itemDataList = new List<ItemData>();

    private TileObj cabinet;

    public override void Open(TileObj tileObj)
    {
        cabinet = tileObj;
        base.Open(tileObj);
    }
    public override void Close(TileObj tileObj)
    {
        base.Open(tileObj);
    }
    public void Start()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_Local_TimeChange>().Subscribe(_ =>
        {
            DrawEveryCell();
        }).AddTo(this);
    }

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
        cabinet.TryToChangeInfo(builder.ToString());
    }
    /// <summary>
    /// 绘制所有格子
    /// </summary>
    private void DrawEveryCell()
    {
        for (int i = 0; i < cellList.Count; i++)
        {
            if (i < itemDataList.Count)
            {
                DrawCell(itemDataList[i], cellList[i], priceList[i]);
            }
            else
            {
                ResetCell(cellList[i], priceList[i]);
            }
        }
    }
    /// <summary>
    /// 重置一个格子
    /// </summary>
    /// <param name="cell"></param>
    private void ResetCell(UI_GridCell cell, TextMeshProUGUI price)
    {
        cell.ResetGridCell();
        price.transform.parent.gameObject.SetActive(false);
        price.text = "";
    }
    /// <summary>
    /// 绘制一个格子
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="config"></param>
    private void DrawCell(ItemData data, UI_GridCell cell,TextMeshProUGUI price)
    {
        cell.UpdateGridCell(data);
        cell.BindClickAction(ClickCellLeft, ClickCellRight);
        cell.BindDragAction(CellDragBegin, CellDragIn, CellDragEnd);
        if(data.Item_ID == 0)
        {
            price.transform.parent.gameObject.SetActive(false);
            price.text = "";
        }
        else
        {
            int val = (int)ItemConfigData.GetItemConfig(data.Item_ID).Average_Value * data.Item_Count;
            price.transform.parent.gameObject.SetActive(true);
            price.text = val.ToString();
            if (GameLocalManager.Instance.localPlayer.actorManager.NetManager.Data_Coin >= val)
            {
                cell.SleepCell(false);
            }
            else
            {
                cell.SleepCell(true);
            }
        }
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
    /*取出*/
    public override void PutOut(ItemData before, out ItemData after)
    {
        int price = (int)(ItemConfigData.GetItemConfig(before.Item_ID).Average_Value * before.Item_Count);
        if (GameLocalManager.Instance.localPlayer.actorManager.PayCoin(price))
        {
            itemDataList.Remove(before);
            after = before;
        }
        else
        {
            after = new ItemData();
        }
        ChangeInfoToTile();
    }
}
