using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class GameUI_BodyPanel : MonoBehaviour
{
    private void Start()
    {
        MessageBroker.Default.Receive<UIEvent.UIEvent_ItemHand_Update>().Subscribe(_ =>
        {
            itemData_Hand = _.itemData;
            gridCell_Hand.UpdateData(itemData_Hand);
        }).AddTo(this);
        MessageBroker.Default.Receive<UIEvent.UIEvent_ItemHead_Update>().Subscribe(_ =>
        {
            itemData_Head = _.itemData;
            gridCell_Head.UpdateData(itemData_Head);
        }).AddTo(this);
        MessageBroker.Default.Receive<UIEvent.UIEvent_ItemBody_Update>().Subscribe(_ =>
        {
            itemData_Body = _.itemData;
            gridCell_Body.UpdateData(itemData_Body);
        }).AddTo(this);
        MessageBroker.Default.Receive<UIEvent.UIEvent_ItemAccessory_Update>().Subscribe(_ =>
        {
            itemData_Accessory = _.itemData;
            gridCell_Accessory.UpdateData(itemData_Accessory);
        }).AddTo(this);
        MessageBroker.Default.Receive<UIEvent.UIEvent_ItemConsumables_Update>().Subscribe(_ =>
        {
            itemData_Consumables = _.itemData;
            gridCell_Consumables.UpdateData(itemData_Consumables);
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
        gridCell_Head.BindGrid(new ItemPath(ItemFrom.Head, 0), HeadPutIn, HeadPutOut, HandClickCellLeft, HandClickCellRight);
        gridCell_Body.BindGrid(new ItemPath(ItemFrom.Body, 0), BodyPutIn, BodyPutOut, HandClickCellLeft, HandClickCellRight);
        gridCell_Accessory.BindGrid(new ItemPath(ItemFrom.Accessory, 0), Accessory_PutIn, Accessory_PutOut, HandClickCellLeft, HandClickCellRight);
        gridCell_Consumables.BindGrid(new ItemPath(ItemFrom.Consumables, 0), ConsumablesPutIn, ConsumablesPutOut, HandClickCellLeft, HandClickCellRight);
    }
    #region//手
    public UI_GridCell gridCell_Hand;
    private ItemData itemData_Hand;

    public void HandPutIn(ItemData data, ItemPath path)
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemHand_Add()
        {
            item = data
        });
    }
    public ItemData HandPutOut(ItemData itemData_From, ItemData data, ItemPath itemPath)
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemHand_Sub()
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
    #region//头
    public UI_GridCell gridCell_Head;
    private ItemData itemData_Head;

    public void HeadPutIn(ItemData data, ItemPath path)
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemHead_Add()
        {
            item = data
        });
    }
    public ItemData HeadPutOut(ItemData itemData_From, ItemData data, ItemPath itemPath)
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemHead_Sub()
        {
            item = data
        });
        return data;
    }

    #endregion
    #region//身
    public UI_GridCell gridCell_Body;
    private ItemData itemData_Body;
    public void BodyPutIn(ItemData data, ItemPath path)
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBody_Add()
        {
            item = data
        });
    }
    public ItemData BodyPutOut(ItemData itemData_From, ItemData data, ItemPath itemPath)
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBody_Sub()
        {
            item = data
        });
        return data;
    }

    #endregion
    #region//饰品
    public UI_GridCell gridCell_Accessory;
    private ItemData itemData_Accessory;
    public void Accessory_PutIn(ItemData data, ItemPath path)
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemAccessory_Add()
        {
            item = data
        });
    }
    public ItemData Accessory_PutOut(ItemData itemData_From, ItemData data, ItemPath itemPath)
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemAccessory_Sub()
        {
            item = data
        });
        return data;
    }
    #endregion
    #region//耗材
    public UI_GridCell gridCell_Consumables;
    private ItemData itemData_Consumables;
    public void ConsumablesPutIn(ItemData data, ItemPath path)
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemConsumables_Add()
        {
            item = data
        });
    }
    public ItemData ConsumablesPutOut(ItemData itemData_From, ItemData data, ItemPath itemPath)
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemConsumables_Sub()
        {
            item = data
        });
        return data;
    }

    #endregion
}
