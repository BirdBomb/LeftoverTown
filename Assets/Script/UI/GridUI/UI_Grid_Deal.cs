using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class UI_Grid_Deal : UI_Grid
{
    [SerializeField]
    private Transform transform_Panel;

    [SerializeField]
    private TextMeshProUGUI text_Name;
    [SerializeField]
    private TextMeshProUGUI text_Desc;
    private ActorManager actorManager_Actor;

    public void Start()
    {
        MessageBroker.Default.Receive<UIEvent.UIEvent_StartDeal>().Subscribe(_ =>
        {
            DealStart(_.dealActor, _.dealGoods, _.dealOffer);
        }).AddTo(this);
        MessageBroker.Default.Receive<UIEvent.UIEvent_OverDeal>().Subscribe(_ =>
        {
            if (_.dealActor == actorManager_Actor)
            {
                DealOver();
            }
        }).AddTo(this);
        BindAllCell();
    }
    private void BindAllCell()
    {
        gridCell_Sell.BindGrid(new ItemPath(ItemFrom.Default, 0), SellPutIn, SellPutOut, null, null);
        for (int i = 0; i < gridCells_Goods.Count; i++)
        {
            int index = i;
            gridCells_Goods[i].BindGrid(new ItemPath(ItemFrom.Default, index), GoodsPutIn,GoodsPutOut,null,null);
        }
        btn_Sell.onClick.AddListener(Sell);
    }
    #region//购买
    [Header("---购买---")]
    [SerializeField]
    private List<UI_GridCell> gridCells_Goods = new List<UI_GridCell>();
    [SerializeField]
    private List<TextMeshProUGUI> texts_Goods = new List<TextMeshProUGUI>();
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
        int price = (int)(ItemConfigData.GetItemConfig(itemData_Out.Item_ID).Average_Value * itemData_Out.Item_Count);
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
                    texts_Goods[i].text = (ItemConfigData.GetItemConfig(itemDatas_Goods[i].Item_ID).Average_Value * itemDatas_Goods[i].Item_Count).ToString();
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
    #region//出售
    [Header("---出售---")]
    [SerializeField]
    private UI_GridCell gridCell_Sell;
    [SerializeField]
    private TextMeshProUGUI texts_Sell;
    [SerializeField]
    private Button btn_Sell;
    private ItemData itemData_Sell;
    private int offerPrice;
    private Func<ItemData, int> func_SellOffer = null;
    public void SellPutIn(ItemData itemData, ItemPath path)
    {
        if (func_SellOffer != null)
        {
            float commonPrice = (ItemConfigData.GetItemConfig(itemData.Item_ID).Average_Value * itemData.Item_Count);
            offerPrice = func_SellOffer(itemData);
            if (offerPrice == 0)
            {
                text_Desc.text = $"没有购买这个东西的意愿，不过你可以白送给他";
            }
            else
            {
                if (offerPrice < commonPrice)
                {
                    text_Desc.text = $"对这个东西不太感兴趣，愿意用*{offerPrice}*枚金币低价买入";
                }
                else
                {
                    text_Desc.text = $"很喜欢这个东西,愿意用*{offerPrice}*枚金币高价买入";
                }
            }
            texts_Sell.text=offerPrice.ToString();
            itemData_Sell = itemData;
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
        text_Desc.text = "";
        itemData_Sell = new ItemData();
        DrawSellCell();
        return itemData;
    }
    public void Sell()
    {
        if (itemData_Sell.Item_ID!=0)
        {
            GameLocalManager.Instance.playerCoreLocal.actorManager_Bind.actionManager.EarnCoin(offerPrice);
            //actorManager_Actor.actorNetManager.RPC_LocalInput_AddItemInBag(itemData_Sell);
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

    public void DealStart(ActorManager actor,List<ItemData> goods,Func<ItemData, int> offer)
    {
        actorManager_Actor = actor;
        transform_Panel.gameObject.SetActive(true);
        transform_Panel.transform.localScale = Vector3.zero;
        transform_Panel.transform.DOScale(Vector3.one, 0.2f);
        text_Name.text = actorManager_Actor.actorNetManager.Local_Name;
        text_Desc.text = "";
        func_SellOffer = offer;
        itemDatas_Goods.Clear();
        for(int i = 0;i < goods.Count;i++)
        {
            itemDatas_Goods.Add(goods[i]);
        }
        DrawGoodsCell();
        DrawSellCell();
    }
    public void DealOver()
    {
        transform_Panel.gameObject.SetActive(false);
        transform_Panel.transform.DOScale(Vector3.zero, 0.2f);
    }
}
