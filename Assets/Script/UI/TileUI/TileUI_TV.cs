using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UniRx;
using UnityEngine;


public class TileUI_TV : TileUI
{
    [SerializeField, Header("格子面板")]
    private Transform transform_Panel;
    [SerializeField, Header("燃料进度条")]
    private Transform transform_FuelBar;
    [SerializeField, Header("燃料格子")]
    private UI_GridCell gridCell_Fuel;
    [SerializeField, Header("VHS格子")]
    private UI_GridCell gridCell_VHS;
    private BuildingObj_Machine_TvByFuel buildingObj_Bind;
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
    public void BindBuilding(BuildingObj_Machine_TvByFuel buildingObj)
    {
        buildingObj_Bind = buildingObj;
        buildingObj_Bind.OpenOrCloseAwakeUI(true);
    }
    public void BindAllCell()
    {
        gridCell_Fuel.BindGrid(new ItemPath(ItemFrom.Default, 0), FuelPutIn, FuelPutOut, null, null);
        gridCell_VHS.BindGrid(new ItemPath(ItemFrom.Default, 0), VhsPutIn, VhsPutOut, null, null);
    }
    public void DrawEveryCell()
    {
        buildingObj_Bind.buildingData_Machine_TvByFuel.ReadFuelItemData(out var itemData_Fuel);
        buildingObj_Bind.buildingData_Machine_TvByFuel.ReadItemDataVHS(out var itemData_VHS);
        gridCell_Fuel.UpdateData(itemData_Fuel);
        gridCell_VHS.UpdateData(itemData_VHS);
    }
    public void DrawBar()
    {
        buildingObj_Bind.buildingData_Machine_TvByFuel.ReadFuelMax(out int fuelBarMax);
        buildingObj_Bind.buildingData_Machine_TvByFuel.ReadFuelDepletedTimeSign(out int nextFuelSign);
        buildingObj_Bind.buildingData_Machine_TvByFuel.ReadLastTimeSign(out int lastTimeSign);

        transform_FuelBar.DOKill();
        if (fuelBarMax <= 0) fuelBarMax = 60;
        float fuelVal = (nextFuelSign - lastTimeSign) % fuelBarMax;
        if (fuelVal > 0) transform_FuelBar.DOScaleX(fuelVal / (float)fuelBarMax, 1f).SetEase(Ease.Linear);
        else transform_FuelBar.transform.localScale = new Vector3(0, 1, 1);
    }
    #region//燃料
    private void FuelPutIn(ItemData addData, ItemPath path)
    {
        buildingObj_Bind.buildingData_Machine_TvByFuel.ReadFuelItemData(out var itemData_Fuel);
        buildingObj_Bind.buildingData_Machine_TvByFuel.ReadFuelDepletedTimeSign(out var gameTime_NextFuelSign);
        buildingObj_Bind.buildingData_Machine_TvByFuel.ReadLastTimeSign(out var gameTime_LastTimeSign);

        FuelConfig fuelConfig = FuelConfigData.GetFuelConfig(addData.I);
        if (fuelConfig.FuelID != 0)
        {
            if (itemData_Fuel.I == 0)
            {
                itemData_Fuel = addData;
                gameTime_NextFuelSign = gameTime_LastTimeSign;
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
        buildingObj_Bind.buildingData_Machine_TvByFuel.WriteFuelItemData(itemData_Fuel);
        buildingObj_Bind.buildingData_Machine_TvByFuel.WriteFuelDepletedTimeSign(gameTime_NextFuelSign);
    }
    private ItemData FuelPutOut(ItemData itemData_From, ItemData itemData_Out, ItemPath itemPath)
    {
        ItemData itemData_Fuel = GameToolManager.Instance.SplitItem(itemData_From, itemData_Out);
        buildingObj_Bind?.buildingData_Machine_TvByFuel.WriteFuelItemData(itemData_Fuel);
        return itemData_Out;
    }
    #endregion
    #region//VHS
    private void VhsPutIn(ItemData addData, ItemPath path)
    {
        buildingObj_Bind.buildingData_Machine_TvByFuel.ReadItemDataVHS(out ItemData itemData_VHS);

        if (itemData_VHS.I == 0)
        {
            itemData_VHS = addData;
        }
        else if (addData.I == itemData_VHS.I)
        {
            itemData_VHS = GameToolManager.Instance.CombineItem(itemData_VHS, addData, out ItemData res);
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

        buildingObj_Bind?.buildingData_Machine_TvByFuel.WriteItemDataVHS(itemData_VHS);
        buildingObj_Bind?.All_TryToPush();
    }
    private ItemData VhsPutOut(ItemData itemData_From, ItemData itemData_Out, ItemPath itemPath)
    {
        ItemData itemData_VHS = GameToolManager.Instance.SplitItem(itemData_From, itemData_Out);
        buildingObj_Bind?.buildingData_Machine_TvByFuel.WriteItemDataVHS(itemData_VHS);
        buildingObj_Bind?.All_TryToPush();
        return itemData_Out;
    }

    #endregion
}
