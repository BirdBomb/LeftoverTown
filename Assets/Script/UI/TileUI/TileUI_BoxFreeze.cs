using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class TileUI_BoxFreeze : TileUI
{
    [SerializeField, Header("格子面板")]
    private Transform transform_Panel;
    [SerializeField, Header("格子列表")]
    private List<UI_GridCell> gridCells_List = new List<UI_GridCell>();

    private BuildingObj_BoxFreeze buildingObj_Bind;

    private void Awake()
    {
        BindAllCell();
    }
    public void BindBuilding(BuildingObj_BoxFreeze buildingObj)
    {
        buildingObj_Bind = buildingObj;
        transform_Panel.DOKill();
        transform_Panel.localScale = Vector3.one;
        transform_Panel.DOPunchScale(new Vector3(0.1f, -0.1f, 0), 0.1f);
    }
    private void BindAllCell()
    {
        for (int i = 0; i < gridCells_List.Count; i++)
        {
            int index = i;
            gridCells_List[i].BindGrid(new ItemPath(ItemFrom.Default, index), PutIn, PutOut, null, null);
        }
    }
    public void DrawEveryCell()
    {
        for (int i = 0; i < gridCells_List.Count; i++)
        {
            if (i < buildingObj_Bind.itemDatas_List.Count)
            {
                if (buildingObj_Bind.itemDatas_List[i].Item_ID != 0)
                {
                    gridCells_List[i].UpdateData(buildingObj_Bind.itemDatas_List[i]);
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
    public void PutIn(ItemData addData, ItemPath path)
    {
        ItemConfig itemConfig = ItemConfigData.GetItemConfig(addData.Item_ID);
        if (itemConfig.Item_Type == ItemType.Food || itemConfig.Item_Type == ItemType.Dishes) 
        {
            addData.Item_Info = 0;
        }

        GameToolManager.Instance.PutInItemList(buildingObj_Bind.itemDatas_List, addData, path.itemIndex, gridCells_List.Count, out ItemData resData);
        if (resData.Item_ID > 0 && resData.Item_Count != 0)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
            {
                itemData = resData,
            });
        }
        buildingObj_Bind.WriteInfo();
    }
    public ItemData PutOut(ItemData itemData_From, ItemData itemData_Out, ItemPath itemPath)
    {
        ItemData itemData_New = GameToolManager.Instance.SplitItem(itemData_From, itemData_Out);
        buildingObj_Bind.itemDatas_List = GameToolManager.Instance.ChangeItemList(buildingObj_Bind.itemDatas_List, itemData_New, itemPath.itemIndex);

        ItemConfig itemConfig = ItemConfigData.GetItemConfig(itemData_Out.Item_ID);
        if (itemConfig.Item_Type == ItemType.Food || itemConfig.Item_Type == ItemType.Dishes)
        {
            itemData_Out.Item_Info = 100;
        }

        buildingObj_Bind.WriteInfo();
        return itemData_Out;
    }
}
