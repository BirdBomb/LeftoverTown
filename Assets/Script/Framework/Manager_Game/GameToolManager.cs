using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Fusion.Allocator;

public class GameToolManager : SingleTon<GameToolManager>, ISingleTon
{
    public void Init()
    {
        
    }
    #region//����
    /// <summary>
    /// �������������
    /// </summary>
    /// <param name="targetData">Ŀ��</param>
    /// <param name="addData">���</param>
    /// <param name="resData">ʣ��</param>
    /// <returns></returns>
    public ItemData PutInItemSingle(ItemData targetData, ItemData addData, out ItemData resData)
    {
        if (targetData.Item_ID > 0)
        {
            ItemConfig config = ItemConfigData.GetItemConfig(targetData.Item_ID);
            if (config.Item_Size == ItemSize.AsGroup)
            {
                if (targetData.Equals(addData))
                {
                    Type type = Type.GetType("Item_" + targetData.Item_ID.ToString());
                    ((ItemBase)Activator.CreateInstance(type)).StaticAction_PileUp(targetData, addData, config.Item_MaxCount, out ItemData newData, out addData);
                    targetData = newData;
                }
            }
        }
        else
        {
            targetData = addData;
            addData = new ItemData(0);
        }
        resData = addData;
        return targetData;
    }
    /// <summary>
    /// ����������б�
    /// </summary>
    /// <param name="targetDatas">Ŀ���б�</param>
    /// <param name="maxCount">�������</param>
    /// <param name="addData">���</param>
    /// <param name="resData">ʣ��</param>
    /// <returns></returns>
    public List<ItemData> PutInItemList(List<ItemData> targetDatas, int maxCount, ItemData addData, out ItemData resData)
    {
        if (addData.Item_ID > 0)
        {
            ItemConfig config = ItemConfigData.GetItemConfig(addData.Item_ID);
            if (config.Item_Size == ItemSize.AsGroup)
            {
                Type type = Type.GetType("Item_" + addData.Item_ID.ToString());
                for (int i = 0; i < targetDatas.Count; i++)
                {
                    if (targetDatas[i].Item_ID == addData.Item_ID)
                    {
                        ((ItemBase)Activator.CreateInstance(type)).StaticAction_PileUp(targetDatas[i], addData, config.Item_MaxCount, out ItemData newData, out addData);
                        targetDatas[i] = newData;
                    }
                }
                if (addData.Item_Count > 0)
                {
                    if (targetDatas.Count < maxCount)
                    {
                        targetDatas.Add(addData);
                        addData.Item_Count = 0;
                    }
                }
            }
            else
            {
                /*���ɶѵ�*/
                if (targetDatas.Count < maxCount)
                {
                    targetDatas.Add(addData);
                    addData.Item_Count = 0;
                }
            }

        }
        resData = addData;
        return targetDatas;
    }
    /// <summary>
    /// ����������б�
    /// </summary>
    /// <param name="targetDatas"></param>
    /// <param name="listMaxCount"></param>
    /// <param name="itemMaxCount"></param>
    /// <param name="addData"></param>
    /// <param name="resData"></param>
    /// <returns></returns>
    public List<ItemData> PutInItemList(List<ItemData> targetDatas, int listMaxCount, short itemMaxCount, ItemData addData, out ItemData resData)
    {
        if (addData.Item_ID > 0)
        {
            ItemConfig config = ItemConfigData.GetItemConfig(addData.Item_ID);
            if (config.Item_Size == ItemSize.AsGroup)
            {
                Type type = Type.GetType("Item_" + addData.Item_ID.ToString());
                for (int i = 0; i < targetDatas.Count; i++)
                {
                    if (targetDatas[i].Item_ID == addData.Item_ID)
                    {
                        ((ItemBase)Activator.CreateInstance(type)).StaticAction_PileUp(targetDatas[i], addData, itemMaxCount, out ItemData newData, out addData);
                        targetDatas[i] = newData;
                    }
                }
                if (addData.Item_Count > 0)
                {
                    if (targetDatas.Count < listMaxCount)
                    {
                        targetDatas.Add(addData);
                        addData.Item_Count = 0;
                    }
                }
            }
            else
            {
                /*���ɶѵ�*/
                if (targetDatas.Count < listMaxCount)
                {
                    targetDatas.Add(addData);
                    addData.Item_Count = 0;
                }
            }

        }
        resData = addData;
        return targetDatas;
    }

    /// <summary>
    /// ����������б�
    /// </summary>
    /// <param name="targetDatas">Ŀ���б�</param>
    /// <param name="maxCount">�������</param>
    /// <param name="addData">���</param>
    /// <param name="resData">ʣ��</param>
    /// <returns></returns>
    public NetworkLinkedList<ItemData> PutInItemList(NetworkLinkedList<ItemData> targetDatas, int maxCount, ItemData addData, out ItemData resData)
    {
        if (addData.Item_ID > 0)
        {
            ItemConfig config = ItemConfigData.GetItemConfig(addData.Item_ID);
            if (config.Item_Size == ItemSize.AsGroup)
            {
                Type type = Type.GetType("Item_" + addData.Item_ID.ToString());
                for (int i = 0; i < targetDatas.Count; i++)
                {
                    if (targetDatas[i].Item_ID == addData.Item_ID)
                    {
                        ((ItemBase)Activator.CreateInstance(type)).StaticAction_PileUp(targetDatas[i], addData, config.Item_MaxCount, out ItemData newData, out addData);
                        targetDatas[i] = newData;
                    }
                }
                if (addData.Item_Count > 0)
                {
                    if (targetDatas.Count < maxCount)
                    {
                        targetDatas.Add(addData);
                        addData.Item_Count = 0;
                    }
                }
            }
            else
            {
                /*���ɶѵ�*/
                if (targetDatas.Count < maxCount)
                {
                    targetDatas.Add(addData);
                    addData.Item_Count = 0;
                }
            }

        }
        resData = addData;
        return targetDatas;
    }
    #endregion
    #region//ȡ��
    /// <summary>
    /// ������ȡ������
    /// </summary>
    /// <param name="targetData"></param>
    /// <param name="subData"></param>
    /// <returns></returns>
    public ItemData PutOutItemSingle(ItemData targetData, ItemData subData)
    {
        ItemConfig config = ItemConfigData.GetItemConfig(targetData.Item_ID);
        if (config.Item_Size == ItemSize.AsGroup)
        {
            if (targetData.Equals(subData))
            {
                targetData.Item_Count -= subData.Item_Count;
            }
            if (targetData.Item_Count <= 0)
            {
                targetData = new ItemData(0);
            }
        }
        else
        {
            targetData = new ItemData(0);
        }
        return targetData;
    }
    /// <summary>
    /// ���б�ȡ������
    /// </summary>
    /// <param name="targetDatas">Ŀ���б�</param>
    /// <param name="subData">��ȥ</param>
    /// <returns></returns>
    public List<ItemData> PutOutItemList(List<ItemData> targetDatas, ItemData subData)
    {
        for (int i = 0; i < targetDatas.Count; i++)
        {
            if (targetDatas[i].Equals(subData))
            {
                ItemData itemData = targetDatas[i];
                short temp = itemData.Item_Count;
                if (temp >= subData.Item_Count)
                {
                    itemData.Item_Count -= subData.Item_Count;
                    subData.Item_Count = 0;
                }
                else
                {
                    itemData.Item_Count = 0;
                    subData.Item_Count -= temp;
                }
                if (itemData.Item_Count <= 0)
                {
                    ItemData empty = new ItemData();
                    targetDatas[i] = empty;
                    targetDatas.Remove(empty);
                }
                else
                {
                    targetDatas[i] = itemData;
                }
            }
        }
        return targetDatas;
    }
    /// <summary>
    /// ���б�ȡ������
    /// </summary>
    /// <param name="targetDatas">Ŀ���б�</param>
    /// <param name="subData">��ȥ</param>
    /// <returns></returns>
    public NetworkLinkedList<ItemData> PutOutItemList(NetworkLinkedList<ItemData> targetDatas, ItemData subData)
    {
        for (int i = 0; i < targetDatas.Count; i++)
        {
            if (targetDatas[i].Equals(subData))
            {
                ItemData itemData = targetDatas[i];
                short temp = itemData.Item_Count;
                if (temp >= subData.Item_Count)
                {
                    itemData.Item_Count -= subData.Item_Count;
                    subData.Item_Count = 0;
                }
                else
                {
                    itemData.Item_Count = 0;
                    subData.Item_Count -= temp;
                }
                if (itemData.Item_Count <= 0)
                {
                    ItemData empty = new ItemData();
                    targetDatas[i] = empty;
                    targetDatas.Remove(empty);
                }
                else
                {
                    targetDatas[i] = itemData;
                }
            }
        }
        return targetDatas;
    }
    #endregion
}
