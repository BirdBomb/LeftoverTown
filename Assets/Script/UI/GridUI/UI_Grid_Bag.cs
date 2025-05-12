using DG.Tweening;
using System;
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
    [SerializeField, Header("排序")]
    private Button btn_PutSort;
    [SerializeField, Header("选定框")]
    private Transform transform_Switch;
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
        MessageBroker.Default.Receive<UIEvent.UIEvent_SwitchItemInBag>().Subscribe(_ =>
        {
            int index = _.index % gridCells_BagCellList.Count;
            transform_Switch.transform.position = gridCells_BagCellList[index].transform.position;
            transform_Switch.transform.DOKill();
            transform_Switch.transform.localScale = Vector3.one;
            transform_Switch.transform.DOPunchScale(new Vector3(0.5f, -0.5f, 0), 0.2f);
        }).AddTo(this);
        MessageBroker.Default.Receive<GameEvent.GameEvent_All_UpdateHour>().Subscribe(_ =>
        {
            BagUpdateItem();
        }).AddTo(this);
        BindAllCell();
    }
    private void BindAllCell()
    {
        for (int i = 0; i < gridCells_BagCellList.Count; i++)
        {
            int index = i;
            gridCells_BagCellList[index].BindGrid(new ItemPath(ItemFrom.Bag, index), PutIn, PutOut, ClickCellLeft, ClickCellRight);
        }
        btn_PutSort.onClick.AddListener(BatchSort);
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
                gridCells_BagCellList[i].CleanItemBase();
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
        if (gridCell._bindItemBase != null) gridCell._bindItemBase.GridCell_LeftClick(gridCell, gridCell._bindItemBase.itemData);
    }
    public void ClickCellRight(UI_GridCell gridCell)
    {
        if (gridCell._bindItemBase != null) gridCell._bindItemBase.GridCell_RightClick(gridCell, gridCell._bindItemBase.itemData);
    }
    public void PutIn(ItemData data, ItemPath path)
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
        {
            index = path.itemIndex,
            itemData = data,
        });
    }
    public ItemData PutOut(ItemData itemData_From,ItemData itemData_Out,ItemPath itemPath)
    {
        ItemData itemData_New = GameToolManager.Instance.SplitItem(itemData_From, itemData_Out);
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemInBag()
        {
            itemData = itemData_New,
            index = itemPath.itemIndex
        });
        return itemData_Out;
    }
    private void BatchSort()
    {
        List<ItemData> itemDatas = GameLocalManager.Instance.playerCoreLocal.actorManager_Bind.actorNetManager.Local_GetBagItem();
        itemDatas = GameToolManager.Instance.SortItemList(itemDatas);
        GameLocalManager.Instance.playerCoreLocal.actorManager_Bind.actorNetManager.Local_SetBagItem(itemDatas);
    }
    #endregion
}
