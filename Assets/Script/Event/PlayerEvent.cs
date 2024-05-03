using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvent 
{
    /// <summary>
    /// 本地玩家:尝试从背包移除物体
    /// </summary>
    public class PlayerEvent_Local_TryRemoveItemFromBag
    {
        public ItemData item;
    }
    /// <summary>
    /// 本地玩家:尝试给背包添加物体
    /// </summary>
    public class PlayerEvent_Local_TryAddItemInBag
    {
        public ItemData item;
    }

    /// <summary>
    /// 本地玩家:掉落一个物体
    /// </summary>
    public class PlayerEvent_Local_TryDropItem
    {
        public ItemData item;
    }
    /// <summary>
    /// 本地玩家:召唤一个生物
    /// </summary>
    public class PlayerEvent_Local_TrySpawnActor
    {
        public string name;
    }

}
