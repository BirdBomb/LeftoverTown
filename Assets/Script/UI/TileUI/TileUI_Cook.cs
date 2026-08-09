using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class TileUI_Cook : TileUI
{
    [SerializeField, Header("格子面板")]
    private Transform transform_Panel;
    [SerializeField, Header("进度条")]
    private Transform transform_Bar;
    [SerializeField]
    private UI_GridCell gridCell_Raw0;
    [SerializeField]
    private UI_GridCell gridCell_Raw1;
    [SerializeField]
    private UI_GridCell gridCell_Raw2;
    [SerializeField]
    private UI_GridCell gridCell_Food;
    private BuildingObj_Machine_Cook buildingObj_Bind;
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
        buildingObj_Bind.All_Open(true);
        base.Hide();
    }

    public void BindBuilding(BuildingObj_Machine_Cook buildingObj)
    {
        buildingObj_Bind = buildingObj;
        transform_Panel.DOKill();
        transform_Panel.localScale = Vector3.one;
        transform_Panel.DOPunchScale(new Vector3(0.1f, -0.1f, 0), 0.1f);
        buildingObj_Bind.All_Open(false);
    }
    public void BindAllCell()
    {
        gridCell_Raw0.BindGrid(new ItemPath(ItemFrom.Default, 0), Raw0PutIn, Raw0PutOut, null, null);
        gridCell_Raw1.BindGrid(new ItemPath(ItemFrom.Default, 0), Raw1PutIn, Raw1PutOut, null, null);
        gridCell_Raw2.BindGrid(new ItemPath(ItemFrom.Default, 0), Raw2PutIn, Raw2PutOut, null, null);
        gridCell_Food.BindGrid(new ItemPath(ItemFrom.Default, 0), FoodPutIn, FoodPutOut, null, null);
    }
    public void DrawAllCell()
    {
        buildingObj_Bind.buildingData_Machine_Cook.ReadItemRaw0(out ItemData itemData_Raw0);
        buildingObj_Bind.buildingData_Machine_Cook.ReadItemRaw1(out ItemData itemData_Raw1);
        buildingObj_Bind.buildingData_Machine_Cook.ReadItemRaw2(out ItemData itemData_Raw2);
        buildingObj_Bind.buildingData_Machine_Cook.ReadItemFood(out ItemData itemData_Food);
        gridCell_Raw0.UpdateData(itemData_Raw0);
        gridCell_Raw1.UpdateData(itemData_Raw1);
        gridCell_Raw2.UpdateData(itemData_Raw2);
        gridCell_Food.UpdateData(itemData_Food);
    }
    public void PlayBarAnima(float val)
    {
        if (val == 0) { transform_Bar.localScale = new Vector3(0, 1, 1); }
        transform_Bar.DOKill();
        transform_Bar.DOScaleX(val, 1f).SetEase(Ease.Linear);
        gridCell_Raw0.transform.DOShakePosition(0.5f);
        gridCell_Raw1.transform.DOShakePosition(0.5f);
        gridCell_Raw2.transform.DOShakePosition(0.5f);
    }
    #region//取出放入
    public void Raw0PutIn(ItemData addData, ItemPath path)
    {
        buildingObj_Bind.buildingData_Machine_Cook.ReadItemRaw0(out ItemData itemData);
        ItemConfig itemConfig = ItemConfigData.GetItemConfig(addData.I);
        if (itemConfig.Item_Type == ItemType.Food && itemData.I == 0)
        {
            ItemData putIn = addData;
            putIn.C = 1;
            itemData = putIn;
            ItemData resData = GameToolManager.Instance.SplitItem(addData, putIn);
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Add()
            {
                itemData = resData,
                itemFrom = ItemFrom.OutSide
            });
            buildingObj_Bind.buildingData_Machine_Cook.WriteItemRaw0(itemData);
            buildingObj_Bind.TryToPush();
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
    public ItemData Raw0PutOut(ItemData itemData_From, ItemData itemData_Out, ItemPath itemPath)
    {
        buildingObj_Bind.buildingData_Machine_Cook.WriteItemRaw0(GameToolManager.Instance.SplitItem(itemData_From, itemData_Out));
        buildingObj_Bind.TryToPush();
        return itemData_Out;
    }
    public void Raw1PutIn(ItemData addData, ItemPath path)
    {
        buildingObj_Bind.buildingData_Machine_Cook.ReadItemRaw1(out ItemData itemData);
        ItemConfig itemConfig = ItemConfigData.GetItemConfig(addData.I);
        if (itemConfig.Item_Type == ItemType.Food && itemData.I == 0)
        {
            ItemData putIn = addData;
            putIn.C = 1;
            itemData = putIn;
            ItemData resData = GameToolManager.Instance.SplitItem(addData, putIn);
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Add()
            {
                itemData = resData,
                itemFrom = ItemFrom.OutSide
            });
            buildingObj_Bind.buildingData_Machine_Cook.WriteItemRaw1(itemData);
            buildingObj_Bind.TryToPush();
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
    public ItemData Raw1PutOut(ItemData itemData_From, ItemData itemData_Out, ItemPath itemPath)
    {
        buildingObj_Bind.buildingData_Machine_Cook.WriteItemRaw1(GameToolManager.Instance.SplitItem(itemData_From, itemData_Out));
        buildingObj_Bind.TryToPush();
        return itemData_Out;
    }
    public void Raw2PutIn(ItemData addData, ItemPath path)
    {
        buildingObj_Bind.buildingData_Machine_Cook.ReadItemRaw2(out ItemData itemData);
        ItemConfig itemConfig = ItemConfigData.GetItemConfig(addData.I);
        if (itemConfig.Item_Type == ItemType.Food && itemData.I == 0)
        {
            ItemData putIn = addData;
            putIn.C = 1;
            itemData = putIn;
            ItemData resData = GameToolManager.Instance.SplitItem(addData, putIn);
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Add()
            {
                itemData = resData,
                itemFrom = ItemFrom.OutSide,
            });
            buildingObj_Bind.buildingData_Machine_Cook.WriteItemRaw2(itemData);
            buildingObj_Bind.TryToPush();
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
    public ItemData Raw2PutOut(ItemData itemData_From, ItemData itemData_Out, ItemPath itemPath)
    {
        buildingObj_Bind.buildingData_Machine_Cook.WriteItemRaw2(GameToolManager.Instance.SplitItem(itemData_From, itemData_Out));
        buildingObj_Bind.TryToPush();
        return itemData_Out;
    }
    public void FoodPutIn(ItemData addData, ItemPath path)
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Add()
        {
            itemData = addData,
            itemFrom = ItemFrom.OutSide
        });
    }
    public ItemData FoodPutOut(ItemData itemData_From, ItemData itemData_Out, ItemPath itemPath)
    {
        buildingObj_Bind.buildingData_Machine_Cook.WriteItemFood(GameToolManager.Instance.SplitItem(itemData_From, itemData_Out));
        buildingObj_Bind.TryToPush();
        return itemData_Out;
    }
    #endregion
}
