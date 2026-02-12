using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
public class TileUI_VillagerBox : TileUI
{
    [SerializeField, Header("格子面板")]
    private Transform transform_Panel;
    [SerializeField, Header("格子列表")]
    private List<UI_GridCell> gridCells_List = new List<UI_GridCell>();
    [SerializeField, Header("放入")]
    private Button btn_PutIn;
    [SerializeField, Header("取出")]
    private Button btn_PutOut;
    [SerializeField, Header("排序")]
    private Button btn_PutSort;
    private BuildingObj_Home_Villager buildingObj_Bind;

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

    public void BindBuilding(BuildingObj_Home_Villager buildingObj)
    {
        buildingObj_Bind = buildingObj;
        buildingObj_Bind.OpenOrCloseAwakeUI(true);
    }
    public void BindAllCell()
    {
        for (int i = 0; i < gridCells_List.Count; i++)
        {
            int index = i;
            gridCells_List[i].BindGrid(new ItemPath(ItemFrom.Default, index), PutIn, PutOut, null, null);
        }
        btn_PutIn.onClick.AddListener(BatchPutIn);
        btn_PutOut.onClick.AddListener(BatchPutOut);
        btn_PutSort.onClick.AddListener(BatchSort);
    }
    public void DrawEveryCell()
    {
        for (int i = 0; i < gridCells_List.Count; i++)
        {
            if (i < buildingObj_Bind.itemDatas_List.Count)
            {
                if (buildingObj_Bind.itemDatas_List[i].I != 0)
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
        GameToolManager.Instance.PutInItemList(buildingObj_Bind.itemDatas_List, addData, path.itemIndex, gridCells_List.Count, out ItemData resData);
        if (resData.I > 0 && resData.C != 0)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Add()
            {
                itemData = resData,
                itemFrom = ItemFrom.OutSide
            });
        }
        buildingObj_Bind.WriteInfo();
    }
    public ItemData PutOut(ItemData itemData_From, ItemData itemData_Out, ItemPath itemPath)
    {
        CalculateFine(itemData_Out);
        ItemData itemData_New = GameToolManager.Instance.SplitItem(itemData_From, itemData_Out);
        buildingObj_Bind.itemDatas_List = GameToolManager.Instance.ChangeItemList(buildingObj_Bind.itemDatas_List, itemData_New, itemPath.itemIndex);
        buildingObj_Bind.WriteInfo();
        return itemData_Out;
    }
    private void BatchPutIn()
    {
        List<ItemData> itemDatas = WorldManager.Instance.playerCoreLocal.actorManager_Bind.actorNetManager.Local_ItemBag_Get();
        for (int i = 0; i < itemDatas.Count; i++)
        {
            ItemData itemData = itemDatas[i];
            ItemConfig itemConfig = ItemConfigData.GetItemConfig(itemData.I);
            int indexInBag = i;
            int indexInBox = buildingObj_Bind.itemDatas_List.FindIndex((x) => { return x.I == itemData.I; });
            if (indexInBox >= 0 && itemConfig.Item_Size == ItemSize.Gro)
            {
                MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Change()
                {
                    index = indexInBag,
                    itemData = new ItemData()
                });
                GameToolManager.Instance.PutInItemList(buildingObj_Bind.itemDatas_List, itemData, indexInBox, buildingObj_Bind.itemDatas_List.Count, out ItemData itemData_Res);
                if (itemData_Res.I > 0 && itemData_Res.C != 0)
                {
                    MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Add()
                    {
                        itemData = itemData_Res,
                        itemFrom = ItemFrom.OutSide
                    });
                }
            }
        }
        buildingObj_Bind.WriteInfo();
    }
    private void BatchPutOut()
    {
        CalculateFine();
        List<ItemData> itemDatas = WorldManager.Instance.playerCoreLocal.actorManager_Bind.actorNetManager.Local_ItemBag_Get();
        for (int i = 0; i < buildingObj_Bind.itemDatas_List.Count; i++)
        {
            ItemData itemData = buildingObj_Bind.itemDatas_List[i];
            ItemConfig itemConfig = ItemConfigData.GetItemConfig(itemData.I);
            int indexInBox = i;
            int indexInBag = itemDatas.FindIndex((x) => { return x.I == itemData.I; });
            if (indexInBag >= 0 && itemConfig.Item_Size == ItemSize.Gro)
            {
                MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Add()
                {
                    index = indexInBag,
                    itemData = itemData,
                    itemFrom = ItemFrom.OutSide
                });
            }
            else
            {
                MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Add()
                {
                    index = 0,
                    itemData = itemData,
                    itemFrom = ItemFrom.OutSide
                });
            }
        }
        buildingObj_Bind.itemDatas_List = new List<ItemData>();
        buildingObj_Bind.WriteInfo();
    }
    private void BatchSort()
    {
        buildingObj_Bind.itemDatas_List = GameToolManager.Instance.SortItemList(buildingObj_Bind.itemDatas_List);
        buildingObj_Bind.WriteInfo();
    }
    private void CalculateFine(ItemData itemData)
    {
        ItemConfig itemConfig = ItemConfigData.GetItemConfig(itemData.I);
        WorldManager.Instance.playerCoreLocal.actorManager_Bind.actorNetManager.
            RPC_LocalInput_Commit((short)CommitState.Steal, (short)(itemConfig.Item_Value * itemData.C));
    }
    private void CalculateFine()
    {
        int fine = 0;
        for (int i = 0; i < buildingObj_Bind.itemDatas_List.Count; i++)
        {
            ItemData itemData = buildingObj_Bind.itemDatas_List[i];
            ItemConfig itemConfig = ItemConfigData.GetItemConfig(itemData.I);

            fine += itemConfig.Item_Value * itemData.C;
        }
        if (fine > 0)
        {
            WorldManager.Instance.playerCoreLocal.actorManager_Bind.actorNetManager.
                RPC_LocalInput_Commit((short)CommitState.Steal, (short)fine);
        }
    }

}
