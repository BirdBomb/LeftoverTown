using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using System;

public class GameUI_BagPanel : MonoBehaviour
{
    [Header("面板")]
    public Transform tran_Panel;
    [Header("背包格子")]
    public List<UI_GridCell> gridCells_BagCellList = new List<UI_GridCell>();
    [Header("排序按钮")]
    public Button btn_PutSort;
    [Header("背包锁")]
    public List<Image> images_BagLockList = new List<Image>();
    private List<ItemData> itemDatas_BagList = new List<ItemData>();
    private void Start()
    {
        MessageBroker.Default.Receive<UIEvent.UIEvent_UpdateItemInBag>().Subscribe(_ =>
        {
            itemDatas_BagList = new List<ItemData>(_.itemDatas);
            BagUpdateItem();
            BagDrawEveryLock();

        }).AddTo(this);
        MessageBroker.Default.Receive<GameEvent.GameEvent_All_UpdateHour>().Subscribe(_ =>
        {
            BagUpdateItem();
        }).AddTo(this);
        MessageBroker.Default.Receive<UIEvent.UIEvent_TryUseItemInBag>().Subscribe(_ =>
        {
            gridCells_BagCellList[_.index]._bindItemBase.InBag_Use();
        }).AddTo(this);
        BindAllCell();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (bool_Show) HidePanel();
            else ShowPanel();
        }
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
            if (i < itemDatas_BagList.Count)
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
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Add()
        {
            index = path.itemIndex,
            itemData = data,
            itemFrom = ItemFrom.Bag
        });
    }
    public ItemData PutOut(ItemData itemData_From, ItemData itemData_Out, ItemPath itemPath)
    {
        ItemData itemData_New = GameToolManager.Instance.SplitItem(itemData_From, itemData_Out);
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Change()
        {
            itemData = itemData_New,
            index = itemPath.itemIndex
        });
        return itemData_Out;
    }
    private void BatchSort()
    {
        List<ItemData> itemDatas = WorldActorManager.Instance.GetPlayer().actorManager_Bind.actorNetManager.Local_ItemBag_Get();
        itemDatas = GameToolManager.Instance.SortItemList(itemDatas);
        WorldActorManager.Instance.GetPlayer().actorManager_Bind.actorNetManager.Local_ItemBag_Set(itemDatas);
    }
    #endregion
    #region//打开隐藏
    public bool bool_Show = false;
    /// <summary>
    /// 显示
    /// </summary>
    public void ShowPanel()
    {
        bool_Show = true;
        tran_Panel.DOKill();
        tran_Panel.DOLocalMoveY(214, 0.2f);
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_Action()
        {
            action = PlayerAction.OpenBag
        });
    }
    /// <summary>
    /// 隐藏
    /// </summary>
    public void HidePanel()
    {
        bool_Show = false;
        tran_Panel.DOKill();
        tran_Panel.DOLocalMoveY(-62, 0.2f);
    }
    #endregion
}
