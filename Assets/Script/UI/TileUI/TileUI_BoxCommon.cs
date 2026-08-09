using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class TileUI_BoxCommon : TileUI_BoxBase
{
    [SerializeField, Header("放入")]
    private Button btn_PutIn;
    [SerializeField, Header("取出")]
    private Button btn_PutOut;
    [SerializeField, Header("排序")]
    private Button btn_PutSort;
    public override void BindAllCell()
    {
        btn_PutIn.onClick.AddListener(BatchPutIn);
        btn_PutOut.onClick.AddListener(BatchPutOut);
        btn_PutSort.onClick.AddListener(BatchSort);
        base.BindAllCell();
    }
    public virtual void BatchPutIn()
    {
        List<ItemData> itemDatas_Bag = WorldActorManager.Instance.GetPlayer().actorManager_Bind.actorNetManager.Local_ItemBag_Get();
        buildingObj_Bind.buildingData_Box.ReadItemDataList(out List<ItemData> itemDatas_Box);
        for (int i = 0; i < itemDatas_Bag.Count; i++)
        {
            ItemData itemData = itemDatas_Bag[i];
            ItemConfig itemConfig = ItemConfigData.GetItemConfig(itemData.I);
            int indexInBag = i;
            int indexInBox = itemDatas_Box.FindIndex((x) => { return x.I == itemData.I; });
            if (indexInBox >= 0 && itemConfig.Item_Size == ItemSize.Gro)
            {
                MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Change()
                {
                    index = indexInBag,
                    itemData = new ItemData()
                });
                itemDatas_Box = GameToolManager.Instance.PutInItemList(itemDatas_Box, itemData, indexInBox, itemDatas_Box.Count, out ItemData itemData_Res);
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
        buildingObj_Bind.buildingData_Box.WriteItemDataList(itemDatas_Box);
        buildingObj_Bind.All_TryToPush();
    }
    public virtual void BatchPutOut()
    {
        List<ItemData> itemDatas_Bag = WorldActorManager.Instance.GetPlayer().actorManager_Bind.actorNetManager.Local_ItemBag_Get();
        buildingObj_Bind.buildingData_Box.ReadItemDataList(out List<ItemData> itemDatas_Box);
        for (int i = 0; i < itemDatas_Box.Count; i++)
        {
            ItemData itemData = itemDatas_Box[i];
            ItemConfig itemConfig = ItemConfigData.GetItemConfig(itemData.I);
            int indexInBox = i;
            int indexInBag = itemDatas_Bag.FindIndex((x) => { return x.I == itemData.I; });
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
        buildingObj_Bind.buildingData_Box.WriteItemDataList(new List<ItemData>());
        buildingObj_Bind.All_TryToPush();
    }
    public virtual void BatchSort()
    {
        buildingObj_Bind.buildingData_Box.ReadItemDataList(out List<ItemData> itemDatas_Box);
        buildingObj_Bind.buildingData_Box.WriteItemDataList(GameToolManager.Instance.SortItemList(itemDatas_Box));
        buildingObj_Bind.All_TryToPush();
    }
}
