using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UniRx;
using UnityEngine;

public class TileUI_Smelter : TileUI
{
    [SerializeField, Header("格子面板")]
    private Transform transform_Panel;
    [SerializeField, Header("燃料进度条")]
    private Transform transform_FuelBar;
    [SerializeField, Header("炼制进度条")]
    private Transform transform_RefiningBar; 
    [SerializeField, Header("炼制前")]
    private UI_GridCell gridCell_RefiningBefore;
    [SerializeField, Header("炼制后")]
    private UI_GridCell gridCell_RefiningAfter;
    [SerializeField, Header("炼制原料")]
    private UI_GridCell gridCell_Fuel;
    private BuildingObj_Machine_Smelter buildingObj_Bind;
    private void Awake()
    {
        BindAllCell();
    }
    public override void Show()
    {
        transform_Panel.DOKill();
        transform_Panel.localScale = Vector3.one;
        transform_Panel.DOPunchScale(new Vector3(0.1f, -0.1f, 0), 0.1f);
        base.Show();
    }
    public override void Hide()
    {
        buildingObj_Bind.OpenOrCloseAwakeUI(false);
        base.Hide();
    }
    public void BindBuilding(BuildingObj_Machine_Smelter buildingObj)
    {
        buildingObj_Bind = buildingObj;
        buildingObj_Bind.OpenOrCloseAwakeUI(true);
    }
    public void BindAllCell()
    {
        gridCell_RefiningBefore.BindGrid(new ItemPath(ItemFrom.Default, 0), RefiningBeforePutIn, RefiningBeforePutOut, null, null);
        gridCell_RefiningAfter.BindGrid(new ItemPath(ItemFrom.Default, 0), RefiningAfterPutIn, RefiningAfterPutOut, null, null);
        gridCell_Fuel.BindGrid(new ItemPath(ItemFrom.Default, 0), FuelPutIn, FuelPutOut, null, null);
    }
    public void DrawEveryCell()
    {
        buildingObj_Bind.buildingData_Machine_Smelter.ReadFuelItemData(out ItemData itemData_Fuel);
        buildingObj_Bind.buildingData_Machine_Smelter.ReadItemRefiningBefore(out ItemData itemData_RefiningBefore);
        buildingObj_Bind.buildingData_Machine_Smelter.ReadItemRefiningAfter(out ItemData itemData_RefiningAfter);
        gridCell_RefiningBefore.UpdateData(itemData_RefiningBefore);
        gridCell_RefiningAfter.UpdateData(itemData_RefiningAfter);
        gridCell_Fuel.UpdateData(itemData_Fuel);
    }
    public void DrawBar()
    {
        buildingObj_Bind.buildingData_Machine_Smelter.ReadFuelMax(out int fuelBarMax);
        buildingObj_Bind.buildingData_Machine_Smelter.ReadFuelDepletedTimeSign(out int nextFuelSign);
        buildingObj_Bind.buildingData_Machine_Smelter.ReadRefiningMax(out int refiningBarMax);
        buildingObj_Bind.buildingData_Machine_Smelter.ReadRefiningCompeletSign(out int nextRefiningSign);
        buildingObj_Bind.buildingData_Machine_Smelter.ReadLastTimeSign(out int lastTimeSign);


        transform_FuelBar.DOKill();
        if (fuelBarMax <= 0) fuelBarMax = 60;
        float fuelVal = (nextFuelSign - lastTimeSign) % fuelBarMax;
        if (fuelVal > 0) 
        {
            float val = fuelVal / (float)fuelBarMax;
            if (val == 1) transform_FuelBar.localScale = new Vector3(1, 1, 1);
            transform_FuelBar.DOScaleX(fuelVal / (float)fuelBarMax, 1f).SetEase(Ease.Linear); 
        }
        else transform_FuelBar.transform.localScale = new Vector3(0, 1, 1);

        transform_RefiningBar.DOKill();
        if (refiningBarMax <= 0) refiningBarMax = 60;
        float refiningVal = (nextRefiningSign - lastTimeSign) % refiningBarMax;
        if (refiningVal > 0) transform_RefiningBar.DOScaleY(1 - refiningVal / (float)refiningBarMax, 1f).SetEase(Ease.Linear);
        else transform_RefiningBar.transform.localScale = new Vector3(1, 0, 1);
    }
    #region//炼制
    private void RefiningBeforePutIn(ItemData addData, ItemPath path)
    {
        buildingObj_Bind.buildingData_Machine_Smelter.ReadItemRefiningBefore(out ItemData itemData_RefiningBefore);
        buildingObj_Bind.buildingData_Machine_Smelter.ReadRefiningCompeletSign(out int gameTime_NextRefiningSign);
        buildingObj_Bind.buildingData_Machine_Smelter.ReadLastTimeSign(out int gameTime_LastTimeSign);
        RefiningConfig addItemFuelConfig = RefiningConfigData.GetRefiningConfig(addData.I);
        if (addItemFuelConfig.RefiningBeforeID != 0)
        {
            if (itemData_RefiningBefore.I == 0)
            {
                itemData_RefiningBefore = addData;
                gameTime_NextRefiningSign = addItemFuelConfig.RefiningSecond + gameTime_LastTimeSign;
            }
            else if (addData.I == itemData_RefiningBefore.I)
            {
                itemData_RefiningBefore = GameToolManager.Instance.CombineItem(itemData_RefiningBefore, addData, out ItemData res);
                MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Add()
                {
                    itemData = res,
                    itemFrom = ItemFrom.OutSide
                });
            }
            else
            {
                MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Add()
                {
                    itemData = addData,
                    itemFrom = ItemFrom.OutSide
                });
            }
        }
        else
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Add()
            {
                itemData = addData,
                itemFrom = ItemFrom.OutSide
            });
        }
        buildingObj_Bind?.buildingData_Machine_Smelter.WriteRefiningCompeletSign(gameTime_NextRefiningSign);
        buildingObj_Bind?.buildingData_Machine_Smelter.WriteItemRefiningBefore(itemData_RefiningBefore);
        buildingObj_Bind?.All_TryToPush();
    }
    private ItemData RefiningBeforePutOut(ItemData itemData_From, ItemData itemData_Out, ItemPath itemPath)
    {
        ItemData itemData_RefiningBefore = GameToolManager.Instance.SplitItem(itemData_From, itemData_Out);
        buildingObj_Bind?.buildingData_Machine_Smelter.WriteItemRefiningBefore(itemData_RefiningBefore);
        buildingObj_Bind?.All_TryToPush();
        return itemData_Out;
    }
    private void RefiningAfterPutIn(ItemData addData, ItemPath path)
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Add()
        {
            itemData = addData,
            itemFrom = ItemFrom.OutSide
        });
    }
    private ItemData RefiningAfterPutOut(ItemData itemData_From, ItemData itemData_Out, ItemPath itemPath)
    {
        ItemData itemData_RefiningAfter = GameToolManager.Instance.SplitItem(itemData_From, itemData_Out);
        buildingObj_Bind?.buildingData_Machine_Smelter.WriteItemRefiningAfter(itemData_RefiningAfter);
        buildingObj_Bind?.All_TryToPush();
        return itemData_Out;
    }
    #endregion
    #region//燃料
    private void FuelPutIn(ItemData addData, ItemPath path)
    {
        buildingObj_Bind.buildingData_Machine_Smelter.ReadFuelItemData(out ItemData itemData_Fuel);
        buildingObj_Bind.buildingData_Machine_Smelter.ReadFuelDepletedTimeSign(out int gameTime_NextFuelSign);
        WorldManager.Instance.GetTime_NowSecond(out int now);
        FuelConfig fuelConfig = FuelConfigData.GetFuelConfig(addData.I);
        if (fuelConfig.FuelID != 0)
        {
            if (itemData_Fuel.I == 0)
            {
                itemData_Fuel = addData;
                gameTime_NextFuelSign = Mathf.Max(gameTime_NextFuelSign, now);
            }
            else if (addData.I == itemData_Fuel.I)
            {
                itemData_Fuel = GameToolManager.Instance.CombineItem(itemData_Fuel, addData, out ItemData res);
                MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Add()
                {
                    itemData = res,
                    itemFrom = ItemFrom.OutSide
                });
            }
            else
            {
                MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Add()
                {
                    itemData = addData,
                    itemFrom = ItemFrom.OutSide
                });
            }
        }
        else
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Add()
            {
                itemData = addData,
                itemFrom = ItemFrom.OutSide
            });
        }
        buildingObj_Bind.buildingData_Machine_Smelter.WriteFuelItemData(itemData_Fuel);
        buildingObj_Bind.buildingData_Machine_Smelter.WriteFuelDepletedTimeSign(gameTime_NextFuelSign);
        buildingObj_Bind.All_TryToPush();
    }
    private ItemData FuelPutOut(ItemData itemData_From, ItemData itemData_Out, ItemPath itemPath)
    {
        ItemData itemData_Fuel = GameToolManager.Instance.SplitItem(itemData_From, itemData_Out);
        buildingObj_Bind?.buildingData_Machine_Smelter.WriteFuelItemData(itemData_Fuel);
        buildingObj_Bind?.All_TryToPush();
        return itemData_Out;
    }
    #endregion
}
