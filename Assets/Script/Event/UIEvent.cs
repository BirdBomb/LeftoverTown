using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIEvent 
{
    /// <summary>
    /// UI-更新持有物体
    /// </summary>
    public class UIEvent_AddItemInHand
    {
        public int playerID;
        public ItemConfig itemConfig;
    }
    /// <summary>
    /// UI-更新背包物体
    /// </summary>
    public class UIEvent_UpdateItemInBag
    {
        public List<ItemConfig> itemConfigs;
    }

}
