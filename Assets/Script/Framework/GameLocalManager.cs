using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLocalManager : SingleTon<GameLocalManager>, ISingleTon
{
    public void Init()
    {
        
    }
    /// <summary>
    /// 创建一个物体
    /// </summary>
    /// <param name="id"></param>
    /// <param name="seed"></param>
    /// <param name="count"></param>
    /// <param name="val"></param>
    /// <returns></returns>
    public ItemData CreateItemData(int id, int seed = 0, int count = 1, int val = 0)
    {
        ItemData item = new ItemData();
        ItemConfig config = ItemConfigData.GetItemConfig(id);
        item.Item_ID = id;
        item.Item_Count = 1;
        item.Item_Val = 0;
        if (seed == 0)
        {
            seed = System.DateTime.Now.Second;
        }
        if (count == 0)
        {
            count = 1;
        }
        if (val == 0)
        {
            if (config.Item_Type == ItemType.Weapon || config.Item_Type == ItemType.Tool)
            {
                Random.InitState(seed);
                val = Random.Range(30, 101);
            }
            if (config.Item_Type == ItemType.Container)
            {

            }
        }

        item.Item_Seed = seed;
        item.Item_Count = count;
        item.Item_Val = val;
        return item;
    }
}
