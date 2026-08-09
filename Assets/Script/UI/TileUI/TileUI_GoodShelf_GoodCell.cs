using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class TileUI_GoodShelf_GoodCell : MonoBehaviour
{
    public UI_GridCell cell;
    public Text text_Price;
    public Text text_PriceBack;
    public Button button_Buy;
    public Button button_Steal;
    private TileUI_GoodShelf bindGoodShelfUI;

    private ItemData itemData_Bind;
    public void BindGrid(ItemPath itemPath, Action<ItemData, ItemPath> putIn, Func<ItemData, ItemData, ItemPath, ItemData> putOut, Action<UI_GridCell> clickLeft, Action<UI_GridCell> clickRight)
    {
        cell.BindGrid(itemPath, putIn, putOut, clickLeft, clickRight);
        button_Buy.onClick.AddListener(Buy);
        button_Steal.onClick.AddListener(Steal);
    }
    public void BindUI(TileUI_GoodShelf tileUI_Deal)
    {
        bindGoodShelfUI = tileUI_Deal;
    }
    public void Init(ItemData itemData)
    {
        Clean();
        PunchScale();
        itemData_Bind = itemData;
        if (itemData_Bind.I <= 0) return;
        int temp = ItemConfigData.GetItemConfig(itemData_Bind.I).Item_Value * itemData_Bind.C;
        cell.UpdateData(itemData_Bind);
        text_Price.text = temp.ToString();
        text_PriceBack.text = temp.ToString();
        text_Price.color = temp > WorldActorManager.Instance.GetPlayer().actorManager_Bind.actorNetManager.Local_Coin ? Color.red : Color.yellow;
    }
    public void Clean()
    {
        cell.CleanItemBase();
        text_Price.text = "";
        text_PriceBack.text = "";
    }
    public void Buy()
    {
        int price = (int)(ItemConfigData.GetItemConfig(itemData_Bind.I).Item_Value * itemData_Bind.C);
        if (WorldActorManager.Instance.GetPlayer().actorManager_Bind.actionManager.PayCoin(price))
        {
            bindGoodShelfUI.GoodPutOut(itemData_Bind);
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Add()
            {
                itemData = itemData_Bind,
                itemFrom = ItemFrom.OutSide
            });
        }
        bindGoodShelfUI.DrawEveryCell();
    }
    public void Steal()
    {
        bindGoodShelfUI.GoodPutOut(itemData_Bind);
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Add()
        {
            itemData = itemData_Bind,
            itemFrom = ItemFrom.OutSide
        });
        bindGoodShelfUI.DrawEveryCell();
    }
    public void PunchScale()
    {
        transform.DOKill();
        transform.localScale = Vector3.one;
        transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.1f);
    }

}
