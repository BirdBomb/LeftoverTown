using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Grid_OnHand : UI_Grid
{
    [SerializeField, Header("手部格子")]
    private UI_GridCell gridCell;
    private ItemData itemData;
    private void Start()
    {
        MessageBroker.Default.Receive<UIEvent.UIEvent_UpdateItemInHand>().Subscribe(_ =>
        {
            itemData = _.itemData;
            gridCell.UpdateData(itemData);
        }).AddTo(this);
        MessageBroker.Default.Receive<GameEvent.GameEvent_AllClient_UpdateTime>().Subscribe(_ =>
        {
            gridCell.UpdateData(itemData);
        }).AddTo(this);
        BindAllCell();
    }
    private void BindAllCell()
    {
        gridCell.BindAction(PutIn, PutOut, ClickCellLeft, ClickCellRight);
    }
    #region//绑定
    public void PutIn(ItemData data)
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemOnHand()
        {
            item = data
        });
    }
    public ItemData PutOut(ItemData data)
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TrySubItemOnHand()
        {
            item = data
        });
        return data;
    }
    public void ClickCellLeft(UI_GridCell gridCell)
    {
        if (gridCell._bindItemBase != null) gridCell._bindItemBase.LeftClickGridCell(gridCell, gridCell._bindItemBase.itemData);
    }
    public void ClickCellRight(UI_GridCell gridCell)
    {
        if (gridCell._bindItemBase != null) gridCell._bindItemBase.RightClickGridCell(gridCell, gridCell._bindItemBase.itemData);
    }
    #endregion
}
