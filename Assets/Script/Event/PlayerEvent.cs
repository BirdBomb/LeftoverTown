using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvent 
{
    public class PlayerEvent_AddItemInHand
    {
        public int playerID;
        public ItemConfig itemConfig;
    }
    public class PlayerEvent_AddItemInBag
    {
        public int playerID;
        public ItemConfig itemConfig;
    }
    /// <summary>
    /// 玩家UI-更新持有物体
    /// </summary>
    public class PlayerEvent_UI_AddItemInHand
    {
        public int playerID;
        public ItemConfig itemConfig;
    }
    /// <summary>
    /// 玩家UI-更新背包物体
    /// </summary>
    public class PlayerEvent_UI_UpdateItemInBag
    {
        public List<ItemConfig> itemConfigs;
    }
}
