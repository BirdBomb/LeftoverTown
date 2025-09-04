using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class TileUI_Deal : TileUI
{
    public Transform transform_Panel;
    private ActorManager_NPC actorManager_Bind;
    private void Awake()
    {
       BindAllCell();
    }
    private void BindAllCell()
    {
        gridCell_Sell.BindGrid(new ItemPath(ItemFrom.Default, 0), SellPutIn, SellPutOut, null, null);
        for (int i = 0; i < gridCells_Goods.Count; i++)
        {
            int index = i;
            gridCells_Goods[i].BindGrid(new ItemPath(ItemFrom.Default, index), GoodsPutIn, GoodsPutOut, null, null);
        }
        btn_Sell.onClick.AddListener(Sell);
    }
    public void Init(ActorManager_NPC npc)
    {
        transform_Panel.gameObject.SetActive(true);
        transform_Panel.localScale = Vector3.one;
        transform_Panel.DOPunchScale(new Vector3(0.1f, -0.1f, 0), 0.1f);

        actorManager_Bind = npc;
        itemDatas_Goods = npc.Local_GetGood();

        DrawGoodsCell();
        DrawSellCell();
    }
    #region//买
    [Header("---购买---")]
    [Header("所有购买格子")]
    public List<UI_GridCell> gridCells_Goods = new List<UI_GridCell>();
    [Header("所有购买价格")]
    public List<Text> texts_Goods = new List<Text>();
    [Header("我方剩余金币")]
    public TextMeshProUGUI textMeshProUGUI_MyCoins;
    private int int_MyCoins;
    private List<ItemData> itemDatas_Goods = new List<ItemData>();
    public void GoodsPutIn(ItemData itemData, ItemPath path)
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Add()
        {
            itemData = itemData,
        });
    }
    public ItemData GoodsPutOut(ItemData itemData_From, ItemData itemData_Out, ItemPath itemPath)
    {
        int price = (int)(ItemConfigData.GetItemConfig(itemData_Out.Item_ID).Item_Value * itemData_Out.Item_Count);
        if (GameLocalManager.Instance.playerCoreLocal.actorManager_Bind.actionManager.PayCoin(price))
        {
            if (actorManager_Bind)
            {
                actorManager_Bind.actionManager.EarnCoin(price);
            }
            itemDatas_Goods = GameToolManager.Instance.PutOutItemList(itemDatas_Goods, itemData_Out);
            ItemData itemData_New = itemData_From;
            itemData_New.Item_Count = (short)(itemData_From.Item_Count - itemData_Out.Item_Count);
        }
        else
        {
            itemData_Out = new ItemData();
        }
        DrawGoodsCell();
        return itemData_Out;
    }
    private void DrawGoodsCell()
    {
        for (int i = 0; i < gridCells_Goods.Count; i++)
        {
            if (i < itemDatas_Goods.Count)
            {
                if (itemDatas_Goods[i].Item_ID != 0)
                {
                    gridCells_Goods[i].UpdateData(itemDatas_Goods[i]);
                    int temp = ItemConfigData.GetItemConfig(itemDatas_Goods[i].Item_ID).Item_Value * itemDatas_Goods[i].Item_Count;
                    texts_Goods[i].text = temp.ToString();
                    if (temp > int_MyCoins)
                    {
                        texts_Goods[i].color = Color.red;
                    }
                    else
                    {
                        texts_Goods[i].color = Color.yellow;
                    }
                }
                else
                {
                    gridCells_Goods[i].CleanItemBase();
                    texts_Goods[i].text = "";
                }
            }
            else
            {
                gridCells_Goods[i].CleanItemBase();
                texts_Goods[i].text = "";
            }
        }
        UpdateCoin();
    }
    #endregion
    #region//卖
    [Header("---出售---")]
    [Header("出售格子")]
    public UI_GridCell gridCell_Sell;
    [Header("出售价格")]
    public Text text_Price;
    [Header("出售按钮")]
    public Button btn_Sell;
    [Header("对方剩余金币")]
    public TextMeshProUGUI textMeshProUGUI_YourCoins;
    private int int_YourCoins;
    public LocalizeStringEvent localizeStringEvent_SellDesc;
    private ItemData itemData_Sell;
    private int int_Price;
    public void SellPutIn(ItemData itemData, ItemPath path)
    {
        if (actorManager_Bind != null)
        {
            float commonPrice = (ItemConfigData.GetItemConfig(itemData.Item_ID).Item_Value * itemData.Item_Count);
            int_Price = actorManager_Bind.Local_Offer(itemData);
            text_Price.text = int_Price.ToString();
            itemData_Sell = itemData;
            DrawSellCell();

            if (int_Price == 0)
            {
                localizeStringEvent_SellDesc.StringReference.SetReference("Role_String", "NoPrice");
                btn_Sell.gameObject.SetActive(false);
            }
            else if (int_Price < commonPrice)
            {
                localizeStringEvent_SellDesc.StringReference.SetReference("Role_String", "LowPrice");
                btn_Sell.gameObject.SetActive(true);
            }
            else
            {
                localizeStringEvent_SellDesc.StringReference.SetReference("Role_String", "HighPrice");
                btn_Sell.gameObject.SetActive(true);
            }
            if (int_Price > actorManager_Bind.actorNetManager.Local_Coin)
            {
                localizeStringEvent_SellDesc.StringReference.SetReference("Role_String", "CannotPay");
                btn_Sell.gameObject.SetActive(false);
            }
        }
        else
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Add()
            {
                itemData = itemData,
            });
        }
    }
    public ItemData SellPutOut(ItemData itemData_From, ItemData itemData, ItemPath itemPath)
    {
        int_Price = 0;
        text_Price.text = "";
        itemData_Sell = new ItemData();
        DrawSellCell();
        return itemData;
    }
    public void Sell()
    {
        if (itemData_Sell.Item_ID != 0)
        {
            if (int_Price > actorManager_Bind.actorNetManager.Local_Coin)
            {
                GameLocalManager.Instance.playerCoreLocal.actorManager_Bind.actionManager.EarnCoin(actorManager_Bind.actorNetManager.Local_Coin);
                actorManager_Bind.actionManager.PayCoin(actorManager_Bind.actorNetManager.Local_Coin);
            }
            else
            {
                GameLocalManager.Instance.playerCoreLocal.actorManager_Bind.actionManager.EarnCoin(int_Price);
                actorManager_Bind.actionManager.PayCoin(int_Price);
            }
            localizeStringEvent_SellDesc.StringReference.SetReference("Role_String", "DealDone");
            itemData_Sell = new ItemData();
        }
        DrawSellCell();
    }
    private void DrawSellCell()
    {
        if (itemData_Sell.Item_ID > 0)
        {
            gridCell_Sell.UpdateData(itemData_Sell);
            btn_Sell.gameObject.SetActive(true);
        }
        else
        {
            gridCell_Sell.CleanItemBase();
            btn_Sell.gameObject.SetActive(false);
        }
        UpdateCoin();
    }
    #endregion
    private void UpdateCoin()
    {
        int_YourCoins = actorManager_Bind.actorNetManager.Local_Coin;
        int_MyCoins = GameLocalManager.Instance.playerCoreLocal.actorManager_Bind.actorNetManager.Local_Coin;
        textMeshProUGUI_YourCoins.text = "对方:" + int_YourCoins.ToString();
        textMeshProUGUI_MyCoins.text = "我方:" + int_MyCoins.ToString();
    }
}
