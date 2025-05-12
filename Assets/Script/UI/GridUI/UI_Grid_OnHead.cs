using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Grid_OnHead : UI_Grid
{
    [SerializeField, Header("头部格子")]
    private UI_GridCell gridCell;
    private ItemData itemData;

    private void Start()
    {
        MessageBroker.Default.Receive<UIEvent.UIEvent_UpdateItemOnHead>().Subscribe(_ =>
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
        gridCell.BindGrid(new ItemPath(ItemFrom.Head, 0), PutIn, PutOut, null, null);
    }
    #region//绑定

    #endregion
    public void PutIn(ItemData data, ItemPath path)
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemOnHead()
        {
            item = data
        });
    }
    public ItemData PutOut(ItemData itemData_From, ItemData data, ItemPath itemPath)
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TrySubItemOnHead()
        {
            item = data
        });
        return data;
    }

}
