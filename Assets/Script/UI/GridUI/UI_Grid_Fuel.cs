using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Text;
using TMPro;
using DG.Tweening;

public class UI_Grid_Fuel : UI_Grid
{
    [SerializeField, Header("燃料格子")]
    private UI_GridCell cell_Fuel;
    [SerializeField, Header("燃料剩余")]
    private Text text_FuelVal;
    [SerializeField, Header("燃烧按钮")]
    private Button btn_Ignite;
    [SerializeField, Header("燃烧燃值")]
    private TextMeshProUGUI text_Offset;
    private ItemData itemData_Fuel = new ItemData();
    private Action<ItemData> action_AddFuel;
    private Action<short> action_IgniteFuel;
    private TileObj bindTileObj;

    private void Start()
    {
        Bind();
    }
    private void Bind()
    {
        btn_Ignite.onClick.AddListener(Ignite);
    }
    public void BindAction(Action<ItemData> add, Action<short> ignite)
    {
        action_AddFuel = add;
        action_IgniteFuel = ignite;
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
    #region//信息更新与上传
    public void UpdateInfoFromTile(short fuelVal, short fuelMax, ItemData fuel)
    {
        itemData_Fuel = fuel;
        DrawCell(itemData_Fuel, cell_Fuel);
        text_FuelVal.text = ((int)((float)fuelVal / fuelMax * 100)).ToString() + "%";
        if (FuelConfigData.GetFuelConfig(fuel.Item_ID).FuelID != 0)
        {
            text_Offset.text = "+" + FuelConfigData.GetFuelConfig(fuel.Item_ID).FuelVal.ToString();
        }
        else
        {
            text_Offset.text = "+0";
        }
    }
    public void ChangeInfoToTile()
    {
        if (action_AddFuel != null)
        {
            action_AddFuel.Invoke(itemData_Fuel);
        };
        bindTileObj.TryToChangeInfo("");
    }
    #endregion
    #region//UI绘制
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
    #region//放入取出
    public override void PutIn(ItemData before, out ItemData after)
    {
        ItemData resData = before;
        FuelConfig fuel = FuelConfigData.GetFuelConfig(before.Item_ID);
        if(fuel.FuelID != 0)
        {
            ItemData itemData = before;
            itemData.Item_Count = 0;
            Type type = Type.GetType("Item_" + before.Item_ID.ToString());
            ((ItemBase)Activator.CreateInstance(type)).StaticAction_PileUp(itemData, resData, 1, out ItemData newData, out resData);
            after = resData;
            itemData_Fuel = newData;
            ChangeInfoToTile();
        }
        else
        {
            base.PutIn(before, out after);
        }
    }
    public override void PutOut(ItemData before, out ItemData after)
    {
        itemData_Fuel = new ItemData();
        ChangeInfoToTile();
        base.PutOut(before, out after);
    }
    #endregion
    #region//燃烧
    private void Ignite()
    {
        if (itemData_Fuel.Item_Count > 0)
        {
            if (action_IgniteFuel != null)
            {
                cell_Fuel.DOKill();
                cell_Fuel.transform.localScale = Vector3.one;
                cell_Fuel.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack).OnComplete(() =>
                {
                    action_IgniteFuel.Invoke(FuelConfigData.GetFuelConfig(itemData_Fuel.Item_ID).FuelVal);
                    itemData_Fuel = new ItemData();
                    ChangeInfoToTile();
                });
            };
        }
    }
    #endregion
}
