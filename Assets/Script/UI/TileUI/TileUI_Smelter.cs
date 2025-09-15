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
        gridCell_RefiningBefore.UpdateData(buildingObj_Bind.itemData_RefiningBefore);
        gridCell_RefiningAfter.UpdateData(buildingObj_Bind.itemData_RefiningAfter);
        gridCell_Fuel.UpdateData(buildingObj_Bind.itemData_Fuel);
    }
    public void DrawBar()
    {
        float fuelVal = buildingObj_Bind.gameTime_NextFuelSign - buildingObj_Bind.gameTime_LastTimeSign;
        if (fuelVal > 0)
        {
            float temp;
            if (fuelVal > buildingObj_Bind.config_Fuel.FuelSecond)
            {
                temp = fuelVal / 60f;
            }
            else
            {
                temp = fuelVal / (float)buildingObj_Bind.config_Fuel.FuelSecond;
            }
            transform_FuelBar.DOKill();
            transform_FuelBar.DOScaleX(temp, 0.5f);
        }
        else
        {
            transform_FuelBar.transform.localScale = new Vector3(0, 1, 1);
        }
        float refiningVal = buildingObj_Bind.gameTime_NextRefiningSign - buildingObj_Bind.gameTime_LastTimeSign;
        if (refiningVal > 0)
        {
            float temp;
            if (refiningVal > buildingObj_Bind.config_Refining.RefiningSecond)
            {
                temp = 0;
            }
            else
            {
                temp = 1 - refiningVal / (float)buildingObj_Bind.config_Refining.RefiningSecond;
            }
            transform_RefiningBar.DOKill();
            transform_RefiningBar.DOScaleY(temp, 0.5f);
        }
        else
        {
            transform_RefiningBar.transform.localScale = new Vector3(1, 0, 1);
        }
    }
    #region//炼制
    private void RefiningBeforePutIn(ItemData addData, ItemPath path)
    {
        RefiningConfig refiningConfig = RefiningConfigData.GetRefiningConfig(addData.Item_ID);
        if (refiningConfig.RefiningBeforeID != 0)
        {
            if (buildingObj_Bind.itemData_RefiningBefore.Item_ID == 0)
            {
                buildingObj_Bind.itemData_RefiningBefore = addData;
                buildingObj_Bind.gameTime_NextRefiningSign = refiningConfig.RefiningSecond + buildingObj_Bind.gameTime_LastTimeSign;
            }
            else if (addData.Item_ID == buildingObj_Bind.itemData_RefiningBefore.Item_ID)
            {
                buildingObj_Bind.itemData_RefiningBefore = GameToolManager.Instance.CombineItem(buildingObj_Bind.itemData_RefiningBefore, addData, out ItemData res);
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
        buildingObj_Bind.WriteInfo();
    }
    private ItemData RefiningBeforePutOut(ItemData itemData_From, ItemData itemData_Out, ItemPath itemPath)
    {
        buildingObj_Bind.itemData_RefiningBefore = GameToolManager.Instance.SplitItem(itemData_From, itemData_Out);
        buildingObj_Bind.WriteInfo();
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
        buildingObj_Bind.itemData_RefiningAfter = GameToolManager.Instance.SplitItem(itemData_From, itemData_Out);
        buildingObj_Bind.WriteInfo();
        return itemData_Out;
    }
    #endregion
    #region//燃料
    private void FuelPutIn(ItemData addData, ItemPath path)
    {
        FuelConfig fuelConfig = FuelConfigData.GetFuelConfig(addData.Item_ID);
        if (fuelConfig.FuelID != 0)
        {
            if (buildingObj_Bind.itemData_Fuel.Item_ID == 0)
            {
                buildingObj_Bind.itemData_Fuel = addData;
                buildingObj_Bind.gameTime_NextFuelSign = buildingObj_Bind.gameTime_LastTimeSign;
            }
            else if (addData.Item_ID == buildingObj_Bind.itemData_Fuel.Item_ID)
            {
                buildingObj_Bind.itemData_Fuel = GameToolManager.Instance.CombineItem(buildingObj_Bind.itemData_Fuel, addData, out ItemData res);
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
        buildingObj_Bind.WriteInfo();
    }
    private ItemData FuelPutOut(ItemData itemData_From, ItemData itemData_Out, ItemPath itemPath)
    {
        buildingObj_Bind.itemData_Fuel = GameToolManager.Instance.SplitItem(itemData_From, itemData_Out);
        buildingObj_Bind.WriteInfo();
        return itemData_Out;
    }
    #endregion
}
