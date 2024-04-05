using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIEvent 
{
    /// <summary>
    /// UI-更新持有物体
    /// </summary>
    public class UIEvent_UpdateItemInHand
    {
        public int playerID;
        public ItemConfig itemConfig;
        public NetworkItemConfig networkItemConfig;
    }
    /// <summary>
    /// UI-更新背包物体
    /// </summary>
    public class UIEvent_UpdateItemInBag
    {
        public List<ItemConfig> itemConfigs;
    }
    /// <summary>
    /// UI-更新个人属性
    /// </summary>
    public class UIEvent_UpdateData
    {
        public int HP;
        public int MaxHP;
        public int Food;
        public int MaxFood;
        public int Water;
        public int MaxWater;
    }
}
