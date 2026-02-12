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
    public class PlayerEvent_Local_ItemBag_Expend
    {
        public int itemID;
        public int itemCount;
    }
    /// <summary>
    /// 本地玩家:添加背包物体
    /// </summary>
    public class PlayerEvent_Local_ItemBag_Add
    {
        public int index;
        public ItemData itemData;
        public ItemFrom itemFrom;
    }
    /// <summary>
    /// 本地玩家:更改背包物体
    /// </summary>
    public class PlayerEvent_Local_ItemBag_Change
    {
        public ItemData itemData;
        public int index;
    }
    /// <summary>
    /// 本地玩家:使用背包物体
    /// </summary>
    public class PlayerEvent_Local_ItemBag_Use
    {
        public int index;
    }
    #endregion
    #region//玩家手持
    /// <summary>
    /// 本地玩家:添加手部物体
    /// </summary>
    public class PlayerEvent_Local_ItemHand_Add
    {
        public ItemData item;
        public Action<ItemData> itemResidueBack;
    }
    /// <summary>
    /// 本地玩家:移除手部物体
    /// </summary>
    public class PlayerEvent_Local_ItemHand_Sub
    {
        public ItemData item;
    }
    /// <summary>
    /// 本地玩家:更改手部物体
    /// </summary>
    public class PlayerEvent_Local_ItemHand_Change
    {
        public ItemData oldItem;
        public ItemData newItem;
    }
    /// <summary>
    /// 本地玩家:切换手持物体
    /// </summary>
    public class PlayerEvent_Local_ItemHand_Switch 
    {
        public int index;
    }
    /// <summary>
    /// 本地玩家:收起手持物体
    /// </summary>
    public class PlayerEvent_Local_ItemHand_PutAway
    {
        
    }

    #endregion
    #region//玩家头部
    /// <summary>
    /// 本地玩家:移除头顶物体
    /// </summary>
    public class PlayerEvent_Local_ItemHead_Sub
    {
        public ItemData item;
    }
    /// <summary>
    /// 本地玩家:添加头顶物体
    /// </summary>
    public class PlayerEvent_Local_ItemHead_Add
    {
        public ItemData item;
        public Action<ItemData> itemResidueBack;
    }
    /// <summary>
    /// 本地玩家:更改头部物体
    /// </summary>
    public class PlayerEvent_Local_ItemHead_Change
    {
        public ItemData oldItem;
        public ItemData newItem;
    }
    /// <summary>
    /// 本地玩家:切换头部物体
    /// </summary>
    public class PlayerEvent_Local_ItemHead_Switch
    {
        public int index;
    }
    /// <summary>
    /// 本地玩家:收起头部物体
    /// </summary>
    public class PlayerEvent_Local_ItemHead_PutAway
    {

    }

    #endregion
    #region//玩家身体
    /// <summary>
    /// 本地玩家:移除身体物体
    /// </summary>
    public class PlayerEvent_Local_ItemBody_Sub
    {
        public ItemData item;
    }
    /// <summary>
    /// 本地玩家:添加身体物体
    /// </summary>
    public class PlayerEvent_Local_ItemBody_Add
    {
        public ItemData item;
        public Action<ItemData> itemResidueBack;
    }
    /// <summary>
    /// 本地玩家:更改身体物体
    /// </summary>
    public class PlayerEvent_Local_ItemBody_Change
    {
        public ItemData oldItem;
        public ItemData newItem;
    }
    /// <summary>
    /// 本地玩家:替换身体物体
    /// </summary>
    public class PlayerEvent_Local_ItemBody_Switch
    {
        public int index;
    }
    /// <summary>
    /// 本地玩家:收起身体物体
    /// </summary>
    public class PlayerEvent_Local_ItemBody_PutAway
    {

    }
    #endregion
    #region//玩家饰品
    /// <summary>
    /// 本地玩家:移除饰品
    /// </summary>
    public class PlayerEvent_Local_ItemAccessory_Sub
    {
        public ItemData item;
    }
    /// <summary>
    /// 本地玩家:添加饰品
    /// </summary>
    public class PlayerEvent_Local_ItemAccessory_Add
    {
        public ItemData item;
        public Action<ItemData> itemResidueBack;
    }
    /// <summary>
    /// 本地玩家:更改饰品
    /// </summary>
    public class PlayerEvent_Local_ItemAccessory_Change
    {
        public ItemData oldItem;
        public ItemData newItem;
    }
    /// <summary>
    /// 本地玩家:替换饰品
    /// </summary>
    public class PlayerEvent_Local_ItemAccessory_Switch
    {
        public int index;
    }
    /// <summary>
    /// 本地玩家:收起饰品
    /// </summary>
    public class PlayerEvent_Local_ItemAccessory_PutAway
    {

    }

    #endregion
    #region//玩家耗材
    /// <summary>
    /// 本地玩家:移除耗材
    /// </summary>
    public class PlayerEvent_Local_ItemConsumables_Sub
    {
        public ItemData item;
    }
    /// <summary>
    /// 本地玩家:添加耗材
    /// </summary>
    public class PlayerEvent_Local_ItemConsumables_Add
    {
        public ItemData item;
        public Action<ItemData> itemResidueBack;
    }
    /// <summary>
    /// 本地玩家:更改耗材
    /// </summary>
    public class PlayerEvent_Local_ItemConsumables_Change
    {
        public ItemData oldItem;
        public ItemData newItem;
    }
    /// <summary>
    /// 本地玩家:替换耗材
    /// </summary>
    public class PlayerEvent_Local_ItemConsumables_Switch
    {
        public int index;
    }
    /// <summary>
    /// 本地玩家:收起耗材
    /// </summary>
    public class PlayerEvent_Local_ItemConsumables_PutAway
    {

    }

    #endregion
    #region//玩家技能
    /// <summary>
    /// 本地玩家:升级
    /// </summary>
    public class PlayerEvent_Local_LevelUp
    {
        public short count;
    }
    /// <summary>
    /// 本地玩家:升级获得经验
    /// </summary>
    public class PlayerEvent_Local_AddExp
    {
        public short exp;
    }
    /// <summary>
    /// 本地玩家:升级技能
    /// </summary>
    public class PlayerEvent_Local_AddSkill
    {
        public short id;
    }
    /// <summary>
    /// 本地玩家:清空技能
    /// </summary>
    public class PlayerEvent_Local_ClearSkill
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

}
