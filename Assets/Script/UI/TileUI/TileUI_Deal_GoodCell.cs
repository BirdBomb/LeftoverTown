using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;

public class TileUI_Deal_GoodCell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UI_GridCell cell;
    public Text text_Price;
    public Text text_PriceBack;
    public Button button_Buy;
    private TileUI_Deal bindDealUI;

    private ItemData itemData_Bind;
    public void BindGrid(ItemPath itemPath, Action<ItemData, ItemPath> putIn, Func<ItemData, ItemData, ItemPath, ItemData> putOut, Action<UI_GridCell> clickLeft, Action<UI_GridCell> clickRight)
    {
        cell.BindGrid(itemPath, putIn, putOut, clickLeft, clickRight);
        button_Buy.onClick.AddListener(Buy);
    }
    public void BindUI(TileUI_Deal tileUI_Deal)
    {
        bindDealUI = tileUI_Deal;
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
            bindDealUI.actorManager_Bind?.actionManager.EarnCoin(price);
            bindDealUI.list_GoodDatas = GameToolManager.Instance.PutOutItemList(bindDealUI.list_GoodDatas, itemData_Bind);
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Add()
            {
                itemData = itemData_Bind,
                itemFrom = ItemFrom.OutSide
            });
        }
        bindDealUI.DrawGoodCell();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }
    public void PunchScale()
    {
        transform.DOKill();
        transform.localScale = Vector3.one;
        transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.1f);
    }
}
