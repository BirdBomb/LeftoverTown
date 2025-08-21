using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class GameUI_BodyPanel : MonoBehaviour
{
    private void Start()
    {
        MessageBroker.Default.Receive<UIEvent.UIEvent_UpdateItemInHand>().Subscribe(_ =>
        {
            itemData_Hand = _.itemData;
            gridCell_Hand.UpdateData(itemData_Hand);
        }).AddTo(this);
        MessageBroker.Default.Receive<UIEvent.UIEvent_UpdateItemOnHead>().Subscribe(_ =>
        {
            itemData_Head = _.itemData;
            gridCell_Head.UpdateData(itemData_Head);
        }).AddTo(this);
        MessageBroker.Default.Receive<UIEvent.UIEvent_UpdateItemOnBody>().Subscribe(_ =>
        {
            itemData_Body = _.itemData;
            gridCell_Body.UpdateData(itemData_Body);
        }).AddTo(this);
        MessageBroker.Default.Receive<GameEvent.GameEvent_All_UpdateHour>().Subscribe(_ =>
        {
            gridCell_Hand.UpdateData(itemData_Hand);
            gridCell_Head.UpdateData(itemData_Head);
            gridCell_Body.UpdateData(itemData_Body);
        }).AddTo(this);
        BindAllCell();
    }
    private void BindAllCell()
    {
        gridCell_Hand.BindGrid(new ItemPath(ItemFrom.Hand, 0), HandPutIn, HandPutOut, HandClickCellLeft, HandClickCellRight);
        gridCell_Head.BindGrid(new ItemPath(ItemFrom.Hand, 0), HeadPutIn, HeadPutOut, null, null);
        gridCell_Body.BindGrid(new ItemPath(ItemFrom.Hand, 0), BodyPutIn, BodyPutOut, null, null);
    }
    #region// ÷
    public UI_GridCell gridCell_Hand;
    private ItemData itemData_Hand;

    public void HandPutIn(ItemData data, ItemPath path)
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemOnHand()
        {
            item = data
        });
    }
    public ItemData HandPutOut(ItemData itemData_From, ItemData data, ItemPath itemPath)
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TrySubItemOnHand()
        {
            item = data
        });
        return data;
    }
    public void HandClickCellLeft(UI_GridCell gridCell)
    {
        if (gridCell._bindItemBase != null) gridCell._bindItemBase.GridCell_LeftClick(gridCell, gridCell._bindItemBase.itemData);
    }
    public void HandClickCellRight(UI_GridCell gridCell)
    {
        if (gridCell._bindItemBase != null) gridCell._bindItemBase.GridCell_RightClick(gridCell, gridCell._bindItemBase.itemData);
    }

    #endregion
    #region//Õ∑
    public UI_GridCell gridCell_Head;
    private ItemData itemData_Head;

    public void HeadPutIn(ItemData data, ItemPath path)
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemOnHead()
        {
            item = data
        });
    }
    public ItemData HeadPutOut(ItemData itemData_From, ItemData data, ItemPath itemPath)
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TrySubItemOnHead()
        {
            item = data
        });
        return data;
    }

    #endregion
    #region//…Ì
    public UI_GridCell gridCell_Body;
    private ItemData itemData_Body;
    public void BodyPutIn(ItemData data, ItemPath path)
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemOnBody()
        {
            item = data
        });
    }
    public ItemData BodyPutOut(ItemData itemData_From, ItemData data, ItemPath itemPath)
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TrySubItemOnBody()
        {
            item = data
        });
        return data;
    }

    #endregion
}
