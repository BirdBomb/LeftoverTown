using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class TileUI_DealUI : TileUI
{
    public Transform transform_Panel;
    private ActorManager actorManager_Actor;

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
    public void Init(ActorManager actor, List<ItemData> goods, Func<ItemData, int> offer)
    {
        actorManager_Actor = actor;
        transform_Panel.gameObject.SetActive(true);
        transform_Panel.localScale = Vector3.one;
        transform_Panel.DOPunchScale(new Vector3(0.1f, -0.1f, 0), 0.1f);

        func_SellOffer = offer;
        itemDatas_Goods.Clear();
        for (int i = 0; i < goods.Count; i++)
        {
            itemDatas_Goods.Add(goods[i]);
        }
        DrawGoodsCell();
        DrawSellCell();
    }
    #region
    [Header("---¹ºÂò---")]
    public List<UI_GridCell> gridCells_Goods = new List<UI_GridCell>();
    public List<Text> texts_Goods = new List<Text>();
    private List<ItemData> itemDatas_Goods = new List<ItemData>();
    public void GoodsPutIn(ItemData itemData, ItemPath path)
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
        {
            itemData = itemData,
        });
    }
    public ItemData GoodsPutOut(ItemData itemData_From, ItemData itemData_Out, ItemPath itemPath)
    {
        int price = (int)(ItemConfigData.GetItemConfig(itemData_Out.Item_ID).Item_Value * itemData_Out.Item_Count);
        if (GameLocalManager.Instance.playerCoreLocal.actorManager_Bind.actionManager.PayCoin(price))
        {
            itemDatas_Goods = GameToolManager.Instance.PutOutItemList(itemDatas_Goods, itemData_Out);
            ItemData itemData_New = itemData_From;
            itemData_New.Item_Count = (short)(itemData_From.Item_Count - itemData_Out.Item_Count);
            //actorManager_Actor.actorNetManager.RPC_LocalInput_ChangeItemInBag(itemData_From, itemData_New);
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
                    texts_Goods[i].text = (ItemConfigData.GetItemConfig(itemDatas_Goods[i].Item_ID).Item_Value * itemDatas_Goods[i].Item_Count).ToString();
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

    }
    #endregion
    #region    
    [Header("---³öÊÛ---")]
    public UI_GridCell gridCell_Sell;
    private ItemData itemData_Sell;
    public Button btn_Sell;
    public LocalizeStringEvent localizeStringEvent_SellDesc;
    public int int_Price;
    public Text text_Price;
    private Func<ItemData, int> func_SellOffer = null;
    public void SellPutIn(ItemData itemData, ItemPath path)
    {
        if (func_SellOffer != null)
        {
            float commonPrice = (ItemConfigData.GetItemConfig(itemData.Item_ID).Item_Value * itemData.Item_Count);
            int_Price = func_SellOffer(itemData);
            text_Price.text = int_Price.ToString();
            itemData_Sell = itemData;
            if (int_Price == 0)
            {
                localizeStringEvent_SellDesc.StringReference.SetReference("Role_String", "NoPrice");
            }
            else if (int_Price < commonPrice)
            {
                localizeStringEvent_SellDesc.StringReference.SetReference("Role_String", "LowPrice");
            }
            else
            {
                localizeStringEvent_SellDesc.StringReference.SetReference("Role_String", "HighPrice");
            }
            DrawSellCell();
        }
        else
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
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
            GameLocalManager.Instance.playerCoreLocal.actorManager_Bind.actionManager.EarnCoin(int_Price);
            //actorManager_Actor.actorNetManager.RPC_LocalInput_AddItemInBag(itemData_Sell);
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
    }

    #endregion
}
