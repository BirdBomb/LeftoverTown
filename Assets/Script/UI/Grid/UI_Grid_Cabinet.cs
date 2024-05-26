using ExitGames.Client.Photon.StructWrapping;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Grid_Cabinet : UI_Grid
{
    [SerializeField,Header("格子列表")]
    private List<UI_GridCell> cellList = new List<UI_GridCell>();
    [HideInInspector]
    public List<ItemData> itemDataList = new List<ItemData>();

    private TileObj cabinet;

    public override void Open(TileObj tileObj)
    {
        cabinet = tileObj;
        base.Open(tileObj);
    }
    public override void Close(TileObj tileObj)
    {
        base.Open(tileObj);
    }


    /// <summary>
    /// 从地块获取更新
    /// </summary>
    /// <param name="info"></param>
    public void UpdateInfoFromTile(string info)
    {
        itemDataList.Clear();
        string[] strings = info.Split("/*I*/");
        for (int i = 0; i < strings.Length; i++)
        {
            if (strings[i] != "")
            {
                ItemData data = JsonUtility.FromJson<ItemData>(strings[i]);
                itemDataList.Add(data);
            }
        }
        DrawEveryCell();
    }
    /// <summary>
    /// 改变更新给地块
    /// </summary>
    public void ChangeInfoToTile()
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < itemDataList.Count; i++)
        {
            if (i == 0)
            {
                builder.Append(JsonUtility.ToJson(itemDataList[i]));
            }
            else
            {
                builder.Append("/*I*/" + JsonUtility.ToJson(itemDataList[i]));
            }
        }
        cabinet.TryToChangeInfo(builder.ToString());
    }
    /// <summary>
    /// 绘制所有格子
    /// </summary>
    private void DrawEveryCell()
    {
        for (int i = 0; i < cellList.Count; i++)
        {
            if (i < itemDataList.Count)
            {
                DrawCell(itemDataList[i], cellList[i]);
            }
            else
            {
                ResetCell(cellList[i]);
            }
        }
    }
    /// <summary>
    /// 重置一个格子
    /// </summary>
    /// <param name="cell"></param>
    private void ResetCell(UI_GridCell cell)
    {
        cell.ClearGridCell();
    }
    /// <summary>
    /// 绘制一个格子
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="config"></param>
    private void DrawCell(ItemData data,UI_GridCell cell)
    {
        cell.UpdateGridCell(data);
        cell.BindClickAction(ClickCellLeft, ClickCellRight);
        cell.BindDragAction(CellDragBegin, CellDragIn, CellDragEnd);
    }
    public void ClickCellLeft(UI_GridCell gridCell)
    {
        
    }
    public void ClickCellRight(UI_GridCell gridCell)
    {
        
    }
    public override void CellDragBegin(UI_GridCell gridCell, ItemData itemData, PointerEventData pointerEventData)
    {

    }
    public override void CellDragIn(UI_GridCell gridCell, ItemData itemData, PointerEventData pointerEventData)
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(gridCell.image_Icon.rectTransform, Input.mousePosition, Camera.main,out Vector3 pos);
        gridCell.image_Icon.transform.position = pos;
    }
    public override void CellDragEnd(UI_GridCell gridCell, ItemData itemData, PointerEventData pointerEventData)
    {
        pointerEventData.position = Input.mousePosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);
        if (raycastResults.Count > 0)
        {
            foreach (RaycastResult result in raycastResults)
            {
                if (result.gameObject.TryGetComponent(out UI_Grid grid))
                {
                    grid.ListenDragOn(this, gridCell, itemData);
                    return;
                }
            }
        }
    }
    public override void ListenDragOn<T>(T grid, UI_GridCell cell, ItemData itemData)
    {
        if (grid.GetType() == typeof(UI_GameSenceUI))
        {
            Calculate(itemData, out ItemData back);
            if (back.Item_Count == 0)//可以全部放入
            {
                PutIn(itemData);
                MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryRemoveItemFromBag()
                {
                    item = itemData
                });
            }
            else if (back.Item_Count < itemData.Item_Count)//可以放入部分
            {
                PutIn(itemData);
                ItemData residue = new ItemData();
                residue.Item_ID = itemData.Item_ID;
                residue.Item_Seed = itemData.Item_Seed;
                residue.Item_Count = back.Item_Count;

                MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryRemoveItemFromBag()
                {
                    item = itemData
                });
                MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryDropItem()
                {
                    item = residue
                });
            }
            else if (back.Item_Count == itemData.Item_Count)//一个都没放入
            {

            }
        }
        base.ListenDragOn<T>(grid, cell, itemData);
    }
    /*取出*/
    public override void PutOut(ItemData data)
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
        {
            item = data,
            itemResidueBack = ((residueItem) => 
            {
                if (residueItem.Item_Count == 0)//可以全部放入
                {
                    itemDataList.Remove(data);
                    ChangeInfoToTile();
                }
                else if (residueItem.Item_Count < data.Item_Count)//可以放入部分
                {
                    int index = itemDataList.IndexOf(data);
                    itemDataList[index] = residueItem;
                    ChangeInfoToTile();
                }
                else if (residueItem.Item_Count == data.Item_Count)//一个都没放入
                {

                }

            })
        });
    }
    /*放入*/
    public override void PutIn(ItemData itemData)
    {
        int index = -1;
        int add = 0;
        for (int i = 0; i < itemDataList.Count; i++)
        {
            if (itemDataList[i].Item_ID == itemData.Item_ID)
            {
                /*背包里面有同名物体*/
                ItemConfig item = ItemConfigData.GetItemConfig(itemData.Item_ID);
                int maxCount = item.Item_MaxCount;
                if (itemData.Item_Count + itemDataList[i].Item_Count <= maxCount)
                {
                    /*背包里面有同名物体,且可以叠加*/
                    index = i;
                    add = itemData.Item_Count;
                }
                else
                {
                    /*背包里面有同名物体,不可叠加*/
                    index = i;
                    add = maxCount - itemDataList[i].Item_Count;
                }
            }
        }

        if (index == -1)
        {
            /*背包里面无同名物体*/
            if (itemDataList.Count < cellList.Count)
            {
                itemDataList.Add(itemData);
            }
        }
        else
        {
            ItemData targetItem = itemDataList[index];
            targetItem.Item_Count += add;
            itemDataList[index] = targetItem;
            /*如果还有剩余*/
            itemData.Item_Count -= add;
            if (itemData.Item_Count > 0 && itemDataList.Count < cellList.Count)
            {
                itemDataList.Add(itemData);
            }
        }
        ChangeInfoToTile();
    }
    /*计算*/
    public override void Calculate(ItemData before, out ItemData after)
    {
        int index = -1;
        int add = 0;
        for (int i = 0; i < itemDataList.Count; i++)
        {
            if (itemDataList[i].Item_ID == before.Item_ID)
            {
                /*背包里面有同名物体*/
                ItemConfig item = ItemConfigData.GetItemConfig(before.Item_ID);
                int maxCount = item.Item_MaxCount;
                if (before.Item_Count + itemDataList[i].Item_Count <= maxCount)
                {
                    /*背包里面有同名物体,且可以叠加*/
                    index = i;
                    add = before.Item_Count;
                }
                else
                {
                    /*背包里面有同名物体,不可叠加*/
                    index = i;
                    add = maxCount - itemDataList[i].Item_Count;
                }
            }
        }

        if (index == -1)
        {
            if (itemDataList.Count < cellList.Count)
            {
                /*背包里面没有同名物体且背包未达上限*/
                before.Item_Count = 0;
            }
        }
        else
        {
            if (itemDataList.Count < cellList.Count)/*塞一个新的*/
            {
                before.Item_Count = 0;
            }
            else
            {
                before.Item_Count -= add;
            }
        }
        after = before;
    }

}
