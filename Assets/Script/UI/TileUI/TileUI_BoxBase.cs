using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class TileUI_BoxBase : TileUI
{
    [SerializeField, Header("格子面板")]
    private Transform transform_Panel;
    [SerializeField, Header("格子列表")]
    protected List<UI_GridCell> gridCells_List = new List<UI_GridCell>();
    protected BuildingObj_Box buildingObj_Bind;

    private void Awake()
    {
        BindAllCell();
    }
    public override void Show()
    {
        transform_Panel.DOKill();
        transform_Panel.localScale = Vector3.one;
        transform_Panel.DOPunchScale(new Vector3(0.1f, -0.1f, 0), 0.1f);
        base.Show();
    }
    public override void Hide()
    {
        buildingObj_Bind.OpenOrCloseAwakeUI(false); 
        buildingObj_Bind.All_ChangeBoxState(BuildingObj_Box.BoxState.Close);
        base.Hide();
    }
    public void BindBuilding(BuildingObj_Box buildingObj)
    {
        buildingObj_Bind = buildingObj;
        buildingObj_Bind.OpenOrCloseAwakeUI(true);
        buildingObj_Bind.All_ChangeBoxState(BuildingObj_Box.BoxState.Open);
    }
    public virtual void BindAllCell()
    {
        for (int i = 0; i < gridCells_List.Count; i++)
        {
            int index = i;
            gridCells_List[i].BindGrid(new ItemPath(ItemFrom.Default, index), PutIn, PutOut, null, null);
        }
    }
    public void DrawEveryCell()
    {
        buildingObj_Bind.buildingData_Box.ReadItemDataList(out var itemDatas_Box);
        Debug.Log(gridCells_List.Count);
        for (int i = 0; i < gridCells_List.Count; i++)
        {
            if (i < itemDatas_Box.Count)
            {
                if (itemDatas_Box[i].I != 0)
                {
                    gridCells_List[i].UpdateData(itemDatas_Box[i]);
                }
                else
                {
                    gridCells_List[i].CleanItemBase();
                }
            }
            else
            {
                gridCells_List[i].CleanItemBase();
            }
        }
    }
    public virtual void PutIn(ItemData itemData_Add, ItemPath path)
    {
        buildingObj_Bind.buildingData_Box.ReadItemDataList(out var oldList);
        var newList = GameToolManager.Instance.PutInItemList(oldList, itemData_Add, path.itemIndex, gridCells_List.Count, out ItemData resData);
        buildingObj_Bind.buildingData_Box.WriteItemDataList(newList);
        buildingObj_Bind.All_TryToPush();
        if (resData.I > 0 && resData.C != 0)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Add()
            {
                itemData = resData,
                itemFrom = ItemFrom.OutSide
            });
        }
    }
    public virtual ItemData PutOut(ItemData itemData_From, ItemData itemData_Out, ItemPath itemPath)
    {
        ItemData itemData_New = GameToolManager.Instance.SplitItem(itemData_From, itemData_Out);
        buildingObj_Bind.buildingData_Box.ReadItemDataList(out var oldList);
        var newList = GameToolManager.Instance.ChangeItemList(oldList, itemData_New, itemPath.itemIndex);
        buildingObj_Bind.buildingData_Box.WriteItemDataList(newList);
        buildingObj_Bind.All_TryToPush();

        return itemData_Out;
    }
}
