using System;
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
        public Action<ItemData> itemResidueBack;
    }
    /// <summary>
    /// 本地玩家:尝试给背包修改物体
    /// </summary>
    public class PlayerEvent_Local_TryChangeItemInBag
    {
        public ItemData oldItem;
        public ItemData newItem;
        public Action<ItemData> itemResidueBack;
    }

    /// <summary>
    /// 本地玩家:尝试从手部移除物体
    /// </summary>
    public class PlayerEvent_Local_TryRemoveItemOnHand
    {
        public ItemData item;
    }
    /// <summary>
    /// 本地玩家:尝试给手部添加物体
    /// </summary>
    public class PlayerEvent_Local_TryAddItemOnHand
    {
        public ItemData item;
        public Action<ItemData> itemResidueBack;
    }



    /// <summary>
    /// 本地玩家:尝试从头顶移除物体
    /// </summary>
    public class PlayerEvent_Local_TryRemoveItemOnHead
    {
        public ItemData item;
    }
    /// <summary>
    /// 本地玩家:尝试给头顶添加物体
    /// </summary>
    public class PlayerEvent_Local_TryAddItemOnHead
    {
        public ItemData item;
        public Action<ItemData> itemResidueBack;
    }

    /// <summary>
    /// 本地玩家:尝试从身体移除物体
    /// </summary>
    public class PlayerEvent_Local_TryRemoveItemOnBody
    {
        public ItemData item;
    }
    /// <summary>
    /// 本地玩家:尝试给身体添加物体
    /// </summary>
    public class PlayerEvent_Local_TryAddItemOnBody
    {
        public ItemData item;
        public Action<ItemData> itemResidueBack;
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
    /// <summary>
    /// 本地玩家:发送一个表情
    /// </summary>
    public class PlayerEvent_Local_Emoji
    {
        public int id;
    }
}
