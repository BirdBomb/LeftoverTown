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
    private BuildingObj_Cook buildingObj_Bind;
    private void Awake()
    {
        BindAllCell();
    }
    public void BindBuilding(BuildingObj_Cook buildingObj)
    {
        buildingObj_Bind = buildingObj;
        transform_Panel.DOKill();
        transform_Panel.localScale = Vector3.one;
        transform_Panel.DOPunchScale(new Vector3(0.1f, -0.1f, 0), 0.1f);
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
        gridCell_Raw0.UpdateData(buildingObj_Bind.itemData_Raw0);
        gridCell_Raw1.UpdateData(buildingObj_Bind.itemData_Raw1);
        gridCell_Raw2.UpdateData(buildingObj_Bind.itemData_Raw2);
        gridCell_Food.UpdateData(buildingObj_Bind.itemData_Food);
    }
    public void DrawBar(float val)
    {
        transform_Bar.DOKill();
        transform_Bar.DOScaleX(val, 1f).SetEase(Ease.Linear);
    }
    public void ShakeRaw()
    {
        gridCell_Raw0.transform.DOShakePosition(0.5f);
        gridCell_Raw1.transform.DOShakePosition(0.5f);
        gridCell_Raw2.transform.DOShakePosition(0.5f);
    }
    #region//取出放入
    public void Raw0PutIn(ItemData addData, ItemPath path)
    {
        ItemConfig itemConfig = ItemConfigData.GetItemConfig(addData.Item_ID);
        if (itemConfig.Item_Type == ItemType.Food && buildingObj_Bind.itemData_Raw0.Item_ID == 0)
        {
            ItemData putIn = addData;
            putIn.Item_Count = 1;
            buildingObj_Bind.itemData_Raw0 = putIn;
            ItemData resData = GameToolManager.Instance.SplitItem(addData, putIn);
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
            {
                itemData = resData,
            });
            buildingObj_Bind.WriteInfo();
        }
        else
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
            {
                itemData = addData,
            });
        }
    }
    public ItemData Raw0PutOut(ItemData itemData_From, ItemData itemData_Out, ItemPath itemPath)
    {
        buildingObj_Bind.itemData_Raw0 = GameToolManager.Instance.SplitItem(itemData_From, itemData_Out);
        buildingObj_Bind.WriteInfo();
        return itemData_Out;
    }
    public void Raw1PutIn(ItemData addData, ItemPath path)
    {
        ItemConfig itemConfig = ItemConfigData.GetItemConfig(addData.Item_ID);
        if (itemConfig.Item_Type == ItemType.Food && buildingObj_Bind.itemData_Raw1.Item_ID == 0)
        {
            ItemData putIn = addData;
            putIn.Item_Count = 1;
            buildingObj_Bind.itemData_Raw1 = putIn;
            ItemData resData = GameToolManager.Instance.SplitItem(addData, putIn);
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
            {
                itemData = resData,
            });
            buildingObj_Bind.WriteInfo();
        }
        else
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
            {
                itemData = addData,
            });
        }
    }
    public ItemData Raw1PutOut(ItemData itemData_From, ItemData itemData_Out, ItemPath itemPath)
    {
        buildingObj_Bind.itemData_Raw1 = GameToolManager.Instance.SplitItem(itemData_From, itemData_Out);
        buildingObj_Bind.WriteInfo();
        return itemData_Out;
    }
    public void Raw2PutIn(ItemData addData, ItemPath path)
    {
        ItemConfig itemConfig = ItemConfigData.GetItemConfig(addData.Item_ID);
        if (itemConfig.Item_Type == ItemType.Food && buildingObj_Bind.itemData_Raw2.Item_ID == 0)
        {
            ItemData putIn = addData;
            putIn.Item_Count = 1;
            buildingObj_Bind.itemData_Raw2 = putIn;
            ItemData resData = GameToolManager.Instance.SplitItem(addData, putIn);
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
            {
                itemData = resData,
            });
            buildingObj_Bind.WriteInfo();
        }
        else
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
            {
                itemData = addData,
            });
        }
    }
    public ItemData Raw2PutOut(ItemData itemData_From, ItemData itemData_Out, ItemPath itemPath)
    {
        buildingObj_Bind.itemData_Raw2 = GameToolManager.Instance.SplitItem(itemData_From, itemData_Out);
        buildingObj_Bind.WriteInfo();
        return itemData_Out;
    }
    public void FoodPutIn(ItemData addData, ItemPath path)
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
        {
            itemData = addData,
        });
    }
    public ItemData FoodPutOut(ItemData itemData_From, ItemData itemData_Out, ItemPath itemPath)
    {
        buildingObj_Bind.itemData_Food = GameToolManager.Instance.SplitItem(itemData_From, itemData_Out);
        buildingObj_Bind.WriteInfo();
        return itemData_Out;
    }
    #endregion
}
