using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvent 
{
    #region//玩家背包
    /// <summary>
    /// 本地玩家:消耗背包物体
    /// </summary>
    public class PlayerEvent_Local_TryExpendItemInBag
    {
        public int itemID;
        public int itemCount;
    }
    /// <summary>
    /// 本地玩家:添加背包物体
    /// </summary>
    public class PlayerEvent_Local_TryAddItemInBag
    {
        public int index;
        public ItemData itemData;
    }
    /// <summary>
    /// 本地玩家:更改背包物体
    /// </summary>
    public class PlayerEvent_Local_TryChangeItemInBag
    {
        public ItemData itemData;
        public int index;
    }
    /// <summary>
    /// 本地玩家:使用背包物体
    /// </summary>
    public class PlayerEvent_Local_TryUseItemInBag
    {
        public int index;
    }
    #endregion
    #region//玩家手持
    /// <summary>
    /// 本地玩家:添加手部物体
    /// </summary>
    public class PlayerEvent_Local_TryAddItemOnHand
    {
        public ItemData item;
        public Action<ItemData> itemResidueBack;
    }
    /// <summary>
    /// 本地玩家:移除手部物体
    /// </summary>
    public class PlayerEvent_Local_TrySubItemOnHand
    {
        public ItemData item;
    }
    /// <summary>
    /// 本地玩家:更改手部物体
    /// </summary>
    public class PlayerEvent_Local_TryChangeItemOnHand
    {
        public ItemData oldItem;
        public ItemData newItem;
    }
    /// <summary>
    /// 本地玩家:替换背包物体到手上
    /// </summary>
    public class PlayerEvent_Local_TrySwitchItemBetweenHandAndBag 
    {
        public int index;
    }
    /// <summary>
    /// 本地玩家:收起手持物体
    /// </summary>
    public class PlayerEvent_Local_TryPutAwayItemOnHand
    {
        
    }

    #endregion
    #region//玩家头部
    /// <summary>
    /// 本地玩家:尝试从头顶移除物体
    /// </summary>
    public class PlayerEvent_Local_TrySubItemOnHead
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
    /// 本地玩家:更改头部物体
    /// </summary>
    public class PlayerEvent_Local_TryChangeItemOnHead
    {
        public ItemData oldItem;
        public ItemData newItem;
    }
    /// <summary>
    /// 替换背包物体到头上
    /// </summary>
    public class PlayerEvent_Local_TrySwitchItemBetweenHeadAndBag
    {
        public int index;
    }
    /// <summary>
    /// 本地玩家:收起头部物体
    /// </summary>
    public class PlayerEvent_Local_TryPutAwayItemOnHead
    {

    }

    #endregion
    #region//玩家身体
    /// <summary>
    /// 本地玩家:尝试从身体移除物体
    /// </summary>
    public class PlayerEvent_Local_TrySubItemOnBody
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
    /// 本地玩家:更改身体物体
    /// </summary>
    public class PlayerEvent_Local_TryChangeItemOnBody
    {
        public ItemData oldItem;
        public ItemData newItem;
    }
    /// <summary>
    /// 替换背包物体到身体
    /// </summary>
    public class PlayerEvent_Local_TrySwitchItemBetweenBodyAndBag
    {
        public int index;
    }
    /// <summary>
    /// 本地玩家:收起身体物体
    /// </summary>
    public class PlayerEvent_Local_TryPutAwayItemOnBody
    {

    }
    #endregion




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
    /// 本地玩家:创造建筑
    /// </summary>
    public class PlayerEvent_Local_TryBuildBuilding 
    {
        public AreaSize size;
        public bool force = false; 
        public int id;
    }
    /// <summary>
    /// 本地玩家:发送一个表情
    /// </summary>
    public class PlayerEvent_Local_SendEmoji
    {
        public int id;
        public Emoji emoji;
    }
    /// <summary>
    /// 本地玩家:发送一段话
    /// </summary>
    public class PlayerEvent_Local_SendText
    {
        public string text;
    }

    /// <summary>
    /// 本地玩家:绑定技能
    /// </summary>
    public class PlayerEvent_Local_BindSkill
    {
        public List<short> skillIDs = new List<short>();
    }
}
