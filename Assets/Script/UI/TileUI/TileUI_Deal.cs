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
    public ActorManager_NPC actorManager_Bind;
    private void Awake()
    {
       BindAllCell();
    }
    private void BindAllCell()
    {
        gridCell_Sell.BindGrid(new ItemPath(ItemFrom.Default, 0), SellPutIn, SellPutOut, null, null);
        for (int i = 0; i < list_GoodCells.Count; i++)
        {
            int index = i;
            list_GoodCells[i].BindGrid(new ItemPath(ItemFrom.Default, index), GoodsPutIn, GoodsPutOut, null, null);
            list_GoodCells[i].BindUI(this);
        }
        btn_Sell.onClick.AddListener(Sell);
    }
    public void Init(ActorManager_NPC npc)
    {
        transform_Panel.gameObject.SetActive(true);
        transform_Panel.localScale = Vector3.one;
        transform_Panel.DOPunchScale(new Vector3(0.1f, -0.1f, 0), 0.1f);

        actorManager_Bind = npc;
        list_GoodDatas = npc.Local_GetGood();

        DrawGoodCell();
        DrawSellCell();
    }

    #region//买
    public List<ItemData> list_GoodDatas = new List<ItemData>();
    public List<TileUI_Deal_GoodCell> list_GoodCells = new List<TileUI_Deal_GoodCell>();
    public void DrawGoodCell()
    {
        for (int i = 0; i < list_GoodCells.Count; i++)
        {
            if (i < list_GoodDatas.Count)
            {
                list_GoodCells[i].Init(list_GoodDatas[i]);
                list_GoodCells[i].gameObject.SetActive(true); ;
            }
            else
            {
                list_GoodCells[i].Clean();
                list_GoodCells[i].gameObject.SetActive(false);
            }
        }

    }
    public void GoodsPutIn(ItemData itemData, ItemPath path)
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Add()
        {
            itemData = itemData,
            itemFrom = ItemFrom.OutSide
        });
    }
    public ItemData GoodsPutOut(ItemData itemData_From, ItemData itemData_Out, ItemPath itemPath)
    {
        return new ItemData();
    }
    #endregion
    #region//卖
    [Header("出售格子")]
    public UI_GridCell gridCell_Sell;
    [Header("出售价格")]
    public Text text_Price;
    public Text text_PriceBack;
    [Header("出售按钮")]
    public Button btn_Sell;
    [Header("我方剩余金币")]
    public Text text_MyCoins;
    public Text text_MyCoinsBack;
    [Header("对方剩余金币")]
    public Text text_YourCoins;
    public Text text_YourCoinsBack;
    private int int_YourCoins;
    private int int_MyCoins;

    public LocalizeStringEvent localizeStringEvent_SellDesc;
    private ItemData itemData_Sell;
    private int? int_Price;
    public void SellPutIn(ItemData itemData, ItemPath path)
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Add()
        {
            itemData = itemData,
            itemFrom = ItemFrom.OutSide
        });
        if (actorManager_Bind != null)
        {
            float commonPrice = (ItemConfigData.GetItemConfig(itemData.I).Item_Value * itemData.C);
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
        }
    }
    public ItemData SellPutOut(ItemData itemData_From, ItemData itemData, ItemPath itemPath)
    {
        DrawSellCell();
        return new ItemData();
    }
    public void Sell()
    {
        List<ItemData> temp = WorldActorManager.Instance.GetPlayer().actorManager_Bind.actorNetManager.Local_ItemBag_Get();
        if (itemData_Sell.I != 0 && temp.Contains(itemData_Sell))
        {
            
            int? coinCount = (int_Price > actorManager_Bind.actorNetManager.Local_Coin) ? actorManager_Bind.actorNetManager.Local_Coin : int_Price;
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryEarn() { coin = (int)coinCount });
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Change() { index = temp.IndexOf(itemData_Sell),itemData = new ItemData()});
            actorManager_Bind.actionManager.PayCoin((int)coinCount);
            localizeStringEvent_SellDesc.StringReference.SetReference("Role_String", "DealDone");
            itemData_Sell = new ItemData();
        }
        DrawSellCell();
        DrawGoodCell();
    }
    private void DrawSellCell()
    {
        int_Price = itemData_Sell.I > 0 ? actorManager_Bind.Local_Offer(itemData_Sell) : null;
        text_Price.text = (int_Price != null) ? int_Price.ToString() : "";
        text_PriceBack.text = (int_Price != null) ? int_Price.ToString() : "";

        if (itemData_Sell.I > 0)
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
    private void UpdateCoin()
    {
        int_YourCoins = actorManager_Bind.actorNetManager.Local_Coin;
        int_MyCoins = WorldActorManager.Instance.GetPlayer().actorManager_Bind.actorNetManager.Local_Coin;
        text_YourCoins.text = int_YourCoins.ToString();
        text_YourCoinsBack.text = int_YourCoins.ToString();
        text_MyCoins.text = int_MyCoins.ToString();
        text_MyCoinsBack.text = int_MyCoins.ToString();
    }
    #endregion
}
