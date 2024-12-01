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
        public ItemData itemData;
    }
    /// <summary>
    /// UI-更新背包物体
    /// </summary>
    public class UIEvent_UpdateItemInBag
    {
        public int bagCapacity;
        public List<ItemData> itemDatas = new List<ItemData>();
    }
    /// <summary>
    /// UI-更新头顶物体
    /// </summary>
    public class UIEvent_UpdateItemOnHead
    {
        public ItemData itemData;
    }
    /// <summary>
    /// UI-更新身体物体
    /// </summary>
    public class UIEvent_UpdateItemOnBody
    {
        public ItemData itemData;
    }
    /// <summary>
    /// UI-更新人物描述
    /// </summary>
    public class UIEvent_UpdateStatus
    {
        public short statusId;
    }
    /// <summary>
    /// UI-更新个人属性HP
    /// </summary>
    public class UIEvent_UpdateHPData
    {
        public int HP;
        public int MaxHP;
    }
    /// <summary>
    /// UI-更新个人属性Food
    /// </summary>
    public class UIEvent_UpdateFoodData
    {
        public int Food;
        public int MaxFood;
    }
    /// <summary>
    /// UI-更新个人属性San
    /// </summary>
    public class UIEvent_UpdateSanData
    {
        public int San;
        public int MaxSan;
    }
    /// <summary>
    /// UI-更新个人属性Armor
    /// </summary>
    public class UIEvent_UpdateArmorData
    {
        public short Armor;
    }
    /// <summary>
    /// UI-更新个人属性Water
    /// </summary>
    public class UIEvent_UpdateWaterData
    {
        public short Water;
    }
    /// <summary>
    /// UI-更新个人属性Happy
    /// </summary>
    public class UIEvent_UpdateHappyData
    {
        public short Happy;
    }
    /// <summary>
    /// UI-更新个人属性Coin
    /// </summary>
    public class UIEvent_UpdateCoinData
    {
        public int Coin;
    }
    /// <summary>
    /// UI-更新个人属性Fine
    /// </summary>
    public class UIEvent_UpdateFineData
    {
        public int Fine;
    }
    /// <summary>
    /// 添加buff
    /// </summary>
    public class UIEvent_AddBuff
    {
        public BuffConfig buffConfig;
    }
    /// <summary>
    /// 移除buff
    /// </summary>
    public class UIEvent_SubBuff
    {
        public BuffConfig buffConfig;
    }
    /// <summary>
    /// 显示信息文字
    /// </summary>
    public class UIEvent_ShowInfoTextUI
    {
        public Vector2 anchor;
        public string text;
    }
    /// <summary>
    /// 隐藏文字信息
    /// </summary>
    public class UIEvent_HidenfoTextUI
    {

    }
    /// <summary>
    /// 显示全局文字
    /// </summary>
    public class UIEvent_ShowGlobalTextUI
    {
        public string text;
    }

}
