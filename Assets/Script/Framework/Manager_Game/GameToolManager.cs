using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Fusion.Allocator;

public class GameToolManager : SingleTon<GameToolManager>, ISingleTon
{
    public void Init()
    {
        
    }
    /// <summary>
    /// 合并物体
    /// </summary>
    /// <param name="itemData_CombineA">合并物体A</param>
    /// <param name="itemData_CombineB">合并物体B</param>
    /// <param name="itemData_Res">合并剩余</param>
    /// <returns>合并结果</returns>
    public ItemData CombineItem(ItemData itemData_CombineA, ItemData itemData_CombineB, out ItemData itemData_Res)
    {
        if (itemData_CombineA.Item_ID > 0)
        {
            ItemConfig config = ItemConfigData.GetItemConfig(itemData_CombineA.Item_ID);
            if (config.Item_Size == ItemSize.AsGroup)
            {
                if (itemData_CombineA.Item_ID == itemData_CombineB.Item_ID)
                {
                    Type type = Type.GetType("Item_" + itemData_CombineA.Item_ID.ToString());
                    ((ItemBase)Activator.CreateInstance(type)).StaticAction_Combine(itemData_CombineA, itemData_CombineB, config.Item_MaxCount, out ItemData newData, out itemData_CombineB);
                    itemData_CombineA = newData;
                }
            }
        }
        else
        {
            itemData_CombineA = itemData_CombineB;
            itemData_CombineB = new ItemData(0);
        }
        itemData_Res = itemData_CombineB;
        return itemData_CombineA;
    }
    /// <summary>
    /// 拆分物体
    /// </summary>
    /// <param name="itemData_SplitBase">被拆分的物体</param>
    /// <param name="itemData_Split">拆分出的物体</param>
    /// <returns>拆分结果</returns>
    public ItemData SplitItem(ItemData itemData_SplitBase, ItemData itemData_Split)
    {
        ItemConfig config = ItemConfigData.GetItemConfig(itemData_SplitBase.Item_ID);
        if (config.Item_Size == ItemSize.AsGroup)
        {
            /*可以拆分*/
            if (itemData_SplitBase.Item_ID == itemData_Split.Item_ID)
            {
                itemData_SplitBase.Item_Count -= itemData_Split.Item_Count;
            }
            if (itemData_SplitBase.Item_Count <= 0)
            {
                itemData_SplitBase = new ItemData(0);
            }
        }
        else
        {
            /*不可以拆分*/
            itemData_SplitBase = new ItemData(0);
        }
        return itemData_SplitBase;
    }
    /// <summary>
    /// 从列表取出物体
    /// </summary>
    /// <param name="itemDatas_List">目标列表</param>
    /// <param name="itemData_Sub">取出物体</param>
    /// <returns></returns>
    public List<ItemData> PutOutItemList(List<ItemData> itemDatas_List, ItemData itemData_Sub)
    {
        for (int i = 0; i < itemDatas_List.Count; i++)
        {
            if (itemDatas_List[i].Equals(itemData_Sub))
            {
                ItemData itemData = itemDatas_List[i];
                short temp = itemData.Item_Count;
                if (temp >= itemData_Sub.Item_Count)
                {
                    itemData.Item_Count -= itemData_Sub.Item_Count;
                    itemData_Sub.Item_Count = 0;
                }
                else
                {
                    itemData.Item_Count = 0;
                    itemData_Sub.Item_Count -= temp;
                }
                if (itemData.Item_Count <= 0)
                {
                    ItemData empty = new ItemData();
                    itemDatas_List[i] = empty;
                    itemDatas_List.Remove(empty);
                }
                else
                {
                    itemDatas_List[i] = itemData;
                }
            }
        }
        return itemDatas_List;
    }
    /// <summary>
    /// 根据Index从列表修改物体
    /// </summary>
    /// <param name="itemDatas_List"></param>
    /// <param name="itemData_New"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public List<ItemData> ChangeItemList(List<ItemData> itemDatas_List, ItemData itemData_New, int index)
    {
        while (!(itemDatas_List.Count > index))
        {
            itemDatas_List.Add(new ItemData());
        }
        if (itemDatas_List.Count > index)
        {
            itemDatas_List[index] = itemData_New;
        }
        else
        {
            
            itemDatas_List[index] = itemData_New;
            Debug.Log("物品修改失败");
        }
        return itemDatas_List;
    }
    /// <summary>
    /// 根据Index给列表添加物体
    /// </summary>
    /// <param name="itemDatas_List"></param>
    /// <param name="itemData_Add"></param>
    /// <param name="index"></param>
    /// <param name="itemData_Res"></param>
    /// <returns></returns>
    public List<ItemData> PutInItemList(List<ItemData> itemDatas_List, ItemData itemData_Add, int index, int maxCount, out ItemData itemData_Res)
    {
        itemData_Res = new ItemData();
        while (!(itemDatas_List.Count >= maxCount))
        {
            itemDatas_List.Add(new ItemData());
        }
        if (itemDatas_List.Count > index)
        {
            if (itemDatas_List[index].Item_ID == 0)
            {
                itemDatas_List[index] = itemData_Add;
            }
            else
            {
                ItemConfig config = ItemConfigData.GetItemConfig(itemData_Add.Item_ID);
                if (config.Item_Size == ItemSize.AsGroup && itemDatas_List[index].Item_ID == itemData_Add.Item_ID)
                {
                    itemDatas_List[index] = CombineItem(itemDatas_List[index], itemData_Add, out itemData_Add);
                }
                if (itemData_Add.Item_ID > 0 && itemData_Add.Item_Count > 0)
                {
                    for (int i = 0; i < itemDatas_List.Count; i++)
                    {
                        if (itemData_Add.Item_ID <= 0 || itemData_Add.Item_Count <= 0)
                        {
                            break;
                        }
                        if (itemDatas_List[i].Item_ID == itemData_Add.Item_ID && config.Item_Size == ItemSize.AsGroup)
                        {
                            ItemData itemData_A = itemDatas_List[i];
                            ItemData itemData_B = itemData_Add;
                            itemDatas_List[i] = CombineItem(itemData_A, itemData_B, out itemData_Add);
                        }
                        if (itemDatas_List[i].Item_ID == 0)
                        {
                            itemDatas_List[i] = itemData_Add;
                            itemData_Add = new ItemData();
                            break;
                        }
                    }
                    itemData_Res = itemData_Add;
                }
            }
        }
        else
        {
            Debug.Log("物品修改失败");
        }
        return itemDatas_List;
    }
    /// <summary>
    /// 给列表排序
    /// </summary>
    /// <param name="itemDatas_List"></param>
    /// <returns></returns>
    public List<ItemData> SortItemList(List<ItemData> itemDatas_List)
    {
        List<ItemData> itemDatas_New = new List<ItemData>(itemDatas_List.Count);
        while (!(itemDatas_New.Count >= itemDatas_List.Count))
        {
            itemDatas_New.Add(new ItemData());
        }

        for (int i = 0; i < itemDatas_List.Count; i++)
        {
            if (itemDatas_List[i].Item_ID != 0)
            {
                ItemData itemData = itemDatas_List[i];
                ItemConfig config = ItemConfigData.GetItemConfig(itemData.Item_ID);
                for (int j = 0; j < itemDatas_New.Count; j++)
                {
                    if (itemData.Item_ID <= 0 || itemData.Item_Count <= 0)
                    {
                        break;
                    }
                    if (itemDatas_New[j].Item_ID == itemData.Item_ID && config.Item_Size == ItemSize.AsGroup)
                    {
                        ItemData itemData_A = itemDatas_New[j];
                        ItemData itemData_B = itemData;
                        itemDatas_New[j] = CombineItem(itemData_A, itemData_B, out itemData);
                    }
                    if (itemDatas_New[j].Item_ID == 0)
                    {
                        itemDatas_New[j] = itemData;
                        itemData = new ItemData();
                        break;
                    }
                }
            }
        }
        return itemDatas_New;
    }
    /// <summary>
    /// 从列表消耗物体
    /// </summary>
    /// <param name="itemDatas_List">目标列表</param>
    /// <param name="id">消耗id</param>
    /// <param name="count">消耗数量</param>
    /// <returns></returns>
    public List<ItemData> ExpendItemList(List<ItemData> itemDatas_List, int id, int count)
    {
        for (int i = 0; i < itemDatas_List.Count; i++)
        {
            if (itemDatas_List[i].Item_ID == id && count > 0)
            {
                ItemData itemData_New = itemDatas_List[i];
                if (itemData_New.Item_Count <= count)
                {
                    count -= itemData_New.Item_Count;
                    itemData_New.Item_Count = 0;
                    itemDatas_List[i] = itemData_New;
                    itemDatas_List.Remove(itemData_New);
                }
                else
                {
                    itemData_New.Item_Count -= (short)count;
                    count = 0;
                    itemDatas_List[i] = itemData_New;
                }
            }
        }
        return itemDatas_List;
    }
}
