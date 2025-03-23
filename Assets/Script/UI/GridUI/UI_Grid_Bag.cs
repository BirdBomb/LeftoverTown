using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Grid_Bag : UI_Grid
{
    [SerializeField, Header("背包格子")]
    private List<UI_GridCell> gridCells_BagCellList = new List<UI_GridCell>();
    [SerializeField, Header("背包锁")]
    private List<Image> images_BagLockList = new List<Image>();
    private List<ItemData> itemDatas_BagList = new List<ItemData>();
    private int _bagCapacity;
    private void Start()
    {
        MessageBroker.Default.Receive<UIEvent.UIEvent_UpdateItemInBag>().Subscribe(_ =>
        {
            _bagCapacity = _.bagCapacity;
            itemDatas_BagList = new List<ItemData>(_.itemDatas);
            BagUpdateItem();
            BagDrawEveryLock();

        }).AddTo(this);
        MessageBroker.Default.Receive<GameEvent.GameEvent_AllClient_UpdateTime>().Subscribe(_ =>
        {
            BagUpdateItem();
        }).AddTo(this);
        BindAllCell();
    }
    private void BindAllCell()
    {
        for (int i = 0; i < gridCells_BagCellList.Count; i++)
        {
            gridCells_BagCellList[i].BindAction(PutIn, PutOut, ClickCellLeft, ClickCellRight);
        }
    }
    #region//绘制
    private void BagUpdateItem()
    {
        for (int i = 0; i < gridCells_BagCellList.Count; i++)
        {
            if (i < itemDatas_BagList.Count)
            {
                gridCells_BagCellList[i].UpdateData(itemDatas_BagList[i]);
            }
            else
            {
                gridCells_BagCellList[i].CleanData();
            }
        }
    }
    private void BagDrawEveryLock()
    {
        for (int i = 0; i < images_BagLockList.Count; i++)
        {
            if (i < _bagCapacity)
            {
                images_BagLockList[i].enabled = false;
            }
            else
            {
                images_BagLockList[i].enabled = true;
            }
        }

    }
    #endregion
    #region//绑定
    public void ClickCellLeft(UI_GridCell gridCell)
    {
        if (gridCell._bindItemBase != null) gridCell._bindItemBase.LeftClickGridCell(gridCell, gridCell._bindItemBase.itemData);
    }
    public void ClickCellRight(UI_GridCell gridCell)
    {
        if (gridCell._bindItemBase != null) gridCell._bindItemBase.RightClickGridCell(gridCell, gridCell._bindItemBase.itemData);
    }
    public void PutIn(ItemData data)
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
        {
            item = data,
        });
    }
    public ItemData PutOut(ItemData data)
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TrySubItemInBag()
        {
            item = data
        });
        return data;
    }
    #endregion
}
