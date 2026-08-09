using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class TileUI_Digger : TileUI
{
    public Transform transform_Panel;
    public Transform transform_FuelBar;
    public Transform transform_DigBar;
    public UI_GridCell gridCell_Fuel;
    public List<UI_GridCell> gridCells_List;
    private BuildingObj_Machine_Digger buildingObj_Bind;

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
    public void BindBuilding(BuildingObj_Machine_Digger buildingObj)
    {
        buildingObj_Bind = buildingObj;
        buildingObj_Bind.OpenOrCloseAwakeUI(true);
    }
    public void BindAllCell()
    {
        gridCell_Fuel.BindGrid(new ItemPath(ItemFrom.Default, 0), FuelPutIn, FuelPutOut, null, null);
        for (int i = 0; i < gridCells_List.Count; i++)
        {
            int index = i;
            gridCells_List[i].BindGrid(new ItemPath(ItemFrom.Default, index), PutIn, PutOut, null, null);
        }
    }
    public void DrawBar()
    {
        buildingObj_Bind.buildingData_Machine_Digger.ReadLastTimeSign(out int lastTimeSign);
        buildingObj_Bind.buildingData_Machine_Digger.ReadFuelDepletedTimeSign(out int nextFuelSign);
        buildingObj_Bind.buildingData_Machine_Digger.ReadFuelMax(out int fuelBarMax);
        buildingObj_Bind.buildingData_Machine_Digger.ReadDigCompeletSign(out int nextDigSign);
        int digBarMax = buildingObj_Bind.int_DigTime;

        transform_FuelBar.DOKill();
        if (fuelBarMax <= 0) fuelBarMax = 60;
        float fuelVal = (nextFuelSign - lastTimeSign) % fuelBarMax;
        if (fuelVal > 0) transform_FuelBar.DOScaleX(fuelVal / (float)fuelBarMax, 1f).SetEase(Ease.Linear);
        else transform_FuelBar.transform.localScale = new Vector3(0, 1, 1);

        transform_DigBar.DOKill();
        if (digBarMax <= 0) digBarMax = 60;
        float refiningVal = (nextDigSign - lastTimeSign) % digBarMax;
        if (refiningVal > 0) transform_DigBar.DOScaleX(1 - refiningVal / (float)digBarMax, 1f).SetEase(Ease.Linear);
        else transform_DigBar.transform.localScale = new Vector3(0, 1, 1);

    }

    public void DrawEveryCell()
    {
        buildingObj_Bind.buildingData_Machine_Digger.ReadItemDataList(out var itemDatas);
        buildingObj_Bind.buildingData_Machine_Digger.ReadFuelItemData(out ItemData fuelData);
        for (int i = 0; i < gridCells_List.Count; i++)
        {
            if (i < itemDatas.Count)
            {
                if (itemDatas[i].I != 0)
                {
                    gridCells_List[i].UpdateData(itemDatas[i]);
                }
                else
                {
                    gridCells_List[i].CleanItemBase();
                }
            }
            else
            {
                gridCells_List[i].CleanItemBase();
            }
        }
        gridCell_Fuel.UpdateData(fuelData);
    }


    #region//È¼ÁÏ
    private void FuelPutIn(ItemData addData, ItemPath path)
    {
        buildingObj_Bind.buildingData_Machine_Digger.ReadFuelItemData(out var itemData_Fuel);
        buildingObj_Bind.buildingData_Machine_Digger.ReadFuelDepletedTimeSign(out var gameTime_FuelDepletedTimeSign);

        FuelConfig fuelConfig = FuelConfigData.GetFuelConfig(addData.I);
        if (fuelConfig.FuelID != 0)
        {
            if (itemData_Fuel.I == 0)
            {
                itemData_Fuel = addData;
                WorldManager.Instance.GetTime_NowSecond(out int now);
                gameTime_FuelDepletedTimeSign = Mathf.Max(gameTime_FuelDepletedTimeSign, now);
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
        buildingObj_Bind?.buildingData_Machine_Digger.WriteFuelItemData(itemData_Fuel);
        buildingObj_Bind?.buildingData_Machine_Digger.WriteFuelDepletedTimeSign(gameTime_FuelDepletedTimeSign);
        buildingObj_Bind.All_TryToPush();
    }
    private ItemData FuelPutOut(ItemData itemData_From, ItemData itemData_Out, ItemPath itemPath)
    {
        ItemData itemData_Fuel = GameToolManager.Instance.SplitItem(itemData_From, itemData_Out);
        buildingObj_Bind?.buildingData_Machine_Digger.WriteFuelItemData(itemData_Fuel);
        buildingObj_Bind?.All_TryToPush();
        return itemData_Out;
    }
    #endregion
    #region
    public virtual void PutIn(ItemData itemData_Add, ItemPath path)
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Add()
        {
            itemData = itemData_Add,
            itemFrom = ItemFrom.OutSide
        });
    }
    public virtual ItemData PutOut(ItemData itemData_From, ItemData itemData_Out, ItemPath itemPath)
    {
        List<ItemData> itemDatas = new List<ItemData>();
        buildingObj_Bind?.buildingData_Machine_Digger.ReadItemDataList(out itemDatas);
        ItemData itemData_New = GameToolManager.Instance.SplitItem(itemData_From, itemData_Out);
        var itemList = GameToolManager.Instance.ChangeItemList(itemDatas, itemData_New, itemPath.itemIndex);
        buildingObj_Bind?.buildingData_Machine_Digger.WriteItemDataList(itemDatas);
        buildingObj_Bind?.All_TryToPush();
        return itemData_Out;
    }

    #endregion
}
