using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Grid_OnBody : UI_Grid
{
    [SerializeField, Header("身体格子")]
    private UI_GridCell gridCell;
    private ItemData itemData;
    private void Start()
    {
        MessageBroker.Default.Receive<UIEvent.UIEvent_UpdateItemOnBody>().Subscribe(_ =>
        {
            itemData = _.itemData;
            gridCell.UpdateData(itemData);
        }).AddTo(this);
        MessageBroker.Default.Receive<GameEvent.GameEvent_All_UpdateHour>().Subscribe(_ =>
        {
            gridCell.UpdateData(itemData);
        }).AddTo(this);
        BindAllCell();
    }
    private void BindAllCell()
    {
        gridCell.BindGrid(new ItemPath(ItemFrom.Body, 0), PutIn, PutOut, null, null);
    }
    #region//绑定
    public void PutIn(ItemData data, ItemPath path)
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemOnBody()
        {
            item = data
        });
    }
    public ItemData PutOut(ItemData itemData_From, ItemData data, ItemPath itemPath)
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TrySubItemOnBody()
        {
            item = data
        });
        return data;
    }
    #endregion
}
