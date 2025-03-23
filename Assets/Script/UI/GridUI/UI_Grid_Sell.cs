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
    [SerializeField, Header("��������")]
    private UI_GridCell gridCell_Sell = new UI_GridCell();
    [SerializeField, Header("�����۸�")]
    private TextMeshProUGUI text_Price = new TextMeshProUGUI();
    [SerializeField, Header("������ť")]
    private Button btn_Sell;
    private ItemData itemData_InSell = new ItemData();
    private int int_Price;
    private void Start()
    {
        btn_Sell.onClick.AddListener(Sell);
        gridCell_Sell.BindAction(PutIn, PutOut, null, null);
    }

    #region//��Ϣ�������ϴ�
    /// <summary>
    /// �ӵؿ��ȡ����
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
    /// �ı���¸��ؿ�
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
    #region//UI����
    /// <summary>
    /// �������и���
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
    /// ��ʾ������ť
    /// </summary>
    private void ShowSellBtn()
    {
        btn_Sell.gameObject.SetActive(true);
        btn_Sell.transform.DOKill();
        btn_Sell.transform.localScale = Vector3.one;
        btn_Sell.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.2f);
    }
    /// <summary>
    /// ����������ť
    /// </summary>
    private void HideSellBtn()
    {
        btn_Sell.gameObject.SetActive(false);
    }
    #endregion
    #region//����
    /// <summary>
    /// ������������
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
    /// ����
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
