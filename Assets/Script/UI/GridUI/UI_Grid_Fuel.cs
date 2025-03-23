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
    private UI_GridCell gridCell_Fuel;
    [SerializeField, Header("燃料剩余")]
    private Text text_FuelVal;
    [SerializeField, Header("燃烧燃值")]
    private TextMeshProUGUI text_FuelCount;
    [SerializeField, Header("燃烧按钮")]
    private Button btn_Ignite;
    private ItemData itemData_Fuel = new ItemData();
    private Action<ItemData> action_AddFuel;
    private Action<short> action_IgniteFuel;

    private void Start()
    {
        Bind();
    }
    private void Bind()
    {
        btn_Ignite.onClick.AddListener(Ignite);
        gridCell_Fuel.BindAction(PutIn, PutOut, null, null);
    }
    public void BindAction_Fuel(Action<ItemData> add, Action<short> ignite)
    {
        action_AddFuel = add;
        action_IgniteFuel = ignite;
    }
    #region//信息更新与上传
    public void UpdateInfo(short fuelVal, short fuelMax, ItemData fuel)
    {
        itemData_Fuel = fuel;
        if (itemData_Fuel.Item_ID > 0) { gridCell_Fuel.UpdateData(itemData_Fuel); }
        else { gridCell_Fuel.CleanData(); }
        text_FuelVal.text = ((int)((float)fuelVal / fuelMax * 100)).ToString() + "%";
        if (FuelConfigData.GetFuelConfig(fuel.Item_ID).FuelID != 0)
        {
            text_FuelCount.text = "+" + FuelConfigData.GetFuelConfig(fuel.Item_ID).FuelVal.ToString();
        }
        else
        {
            text_FuelCount.text = "+0";
        }
    }
    public void ChangeInfo()
    {
        if (action_AddFuel != null)
        {
            action_AddFuel.Invoke(itemData_Fuel);
        };
        if (action_ChangeInfo != null)
        {
            action_ChangeInfo.Invoke("");
        };
    }
    #endregion
    #region//燃烧
    private void Ignite()
    {
        if (itemData_Fuel.Item_Count > 0)
        {
            if (action_IgniteFuel != null)
            {
                gridCell_Fuel.DOKill();
                gridCell_Fuel.transform.localScale = Vector3.one;
                gridCell_Fuel.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack).OnComplete(() =>
                {
                    action_IgniteFuel.Invoke(FuelConfigData.GetFuelConfig(itemData_Fuel.Item_ID).FuelVal);
                    itemData_Fuel = new ItemData();
                    ChangeInfo();
                });
            };
        }
    }
    #endregion
    public void PutIn(ItemData data)
    {
        FuelConfig fuel = FuelConfigData.GetFuelConfig(data.Item_ID);
        if (fuel.FuelID != 0)
        {
            itemData_Fuel = GameToolManager.Instance.PutInItemSingle(itemData_Fuel, data, out data);
        }
        if (data.Item_Count != 0)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
            {
                item = data,
            });
        }
        ChangeInfo();
    }
    public ItemData PutOut(ItemData data)
    {
        itemData_Fuel = GameToolManager.Instance.PutOutItemSingle(itemData_Fuel, data);
        ChangeInfo();
        return data;
    }
}
