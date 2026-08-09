using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class TileUI_Toilet : TileUI
{
    [SerializeField, Header("格子面板")]
    private Transform transform_Panel;
    [SerializeField, Header("格子列表")]
    protected List<UI_GridCell> gridCells_List = new List<UI_GridCell>();
    [SerializeField, Header("生成按钮")]
    protected Button button_Create;
    protected BuildingObj_Chair_Toilet buildingObj_Bind;
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
        base.Hide();
    }
    public void BindBuilding(BuildingObj_Chair_Toilet buildingObj)
    {
        buildingObj_Bind = buildingObj;
        buildingObj_Bind.OpenOrCloseAwakeUI(true);
    }
    public virtual void BindAllCell()
    {
        for (int i = 0; i < gridCells_List.Count; i++)
        {
            int index = i;
            gridCells_List[i].BindGrid(new ItemPath(ItemFrom.Default, index), PutIn, PutOut, null, null);
        }
        button_Create.onClick.AddListener(ClickUseBtn);
    }
    public void DrawEveryCell()
    {
        buildingObj_Bind.buildingData_Chair.ReadItemDataList(out var itemDatas_Box);
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
    public virtual void PutIn(ItemData addData, ItemPath path)
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Add()
        {
            itemData = addData,
            itemFrom = ItemFrom.OutSide
        });
    }
    public virtual ItemData PutOut(ItemData itemData_From, ItemData itemData_Out, ItemPath itemPath)
    {
        buildingObj_Bind.buildingData_Chair.ReadItemDataList(out var oldList);
        ItemData itemData_New = GameToolManager.Instance.SplitItem(itemData_From, itemData_Out);
        var itemList = GameToolManager.Instance.ChangeItemList(oldList, itemData_New, itemPath.itemIndex);
        buildingObj_Bind.buildingData_Chair.WriteItemDataList(oldList);
        buildingObj_Bind.TryToPush();
        return itemData_Out;
    }
    public void ClickUseBtn()
    {
        buildingObj_Bind.All_UseToilet();
    }
}
