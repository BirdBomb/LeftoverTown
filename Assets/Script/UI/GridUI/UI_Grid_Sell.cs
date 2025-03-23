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

public class UI_Grid_Sell : UI_Grid
{
    [SerializeField, Header("售卖格子")]
    private UI_GridCell gridCell_Sell = new UI_GridCell();
    [SerializeField, Header("售卖价格")]
    private TextMeshProUGUI text_Price = new TextMeshProUGUI();
    [SerializeField, Header("售卖按钮")]
    private Button btn_Sell;
    private ItemData itemData_InSell = new ItemData();
    private int int_Price;
    private void Start()
    {
        btn_Sell.onClick.AddListener(Sell);
        gridCell_Sell.BindAction(PutIn, PutOut, null, null);
    }

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
    public void ChangeInfo()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(JsonUtility.ToJson(itemData_InSell));
        if (action_ChangeInfo != null)
        {
            action_ChangeInfo.Invoke(builder.ToString());
        }
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
            gridCell_Sell.UpdateData(itemData_InSell);
        }
        else
        {
            HideSellBtn();
            gridCell_Sell.CleanData();
        }
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
            int_Price = val;
            text_Price.text = int_Price.ToString();
        }
        else
        {
            int_Price = 0;
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
            GameLocalManager.Instance.playerCoreLocal.actorManager_Bind.actionManager.EarnCoin(int_Price);
            itemData_InSell = new ItemData();
            ChangeInfo();
        }
    }
    #endregion
    public void PutIn(ItemData data)
    {
        itemData_InSell = GameToolManager.Instance.PutInItemSingle(itemData_InSell, data, out ItemData res);
        if (res.Item_Count != 0)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
            {
                item = res,
            });
        }
        ChangeInfo();
    }
    public ItemData PutOut(ItemData data)
    {
        itemData_InSell = GameToolManager.Instance.PutOutItemSingle(itemData_InSell, data);
        ChangeInfo();
        return data;
    }

}
