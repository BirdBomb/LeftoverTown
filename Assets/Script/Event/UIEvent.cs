using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIEvent 
{
    /// <summary>
    /// UI-更新背包物体
    /// </summary>
    public class UIEvent_UpdateItemInBag
    {
        public int bagCapacity;
        public List<ItemData> itemDatas = new List<ItemData>();
    }
    /// <summary>
    /// UI_选中背包物体
    /// </summary>
    public class UIEvent_SwitchItemInBag 
    {
        public int index;
    }
    /// <summary>
    /// UI_添加背包物体
    /// </summary>
    public class UIEvent_PutItemInBag
    {
        public ItemData item;
    }
    /// <summary>
    /// UI_移除背包物体
    /// </summary>
    public class UIEvent_PutItemOutBag
    {
        public ItemData item;
    }
    /// <summary>
    /// UI-打开持有物体
    /// </summary>
    public class UIEvent_OpenItemInHand
    {
        
    }
    /// <summary>
    /// UI-更新持有物体
    /// </summary>
    public class UIEvent_UpdateItemInHand
    {
        public ItemData itemData;
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
        public StatusType statusType;
    }
    /// <summary>
    /// UI-更新个人属性HP
    /// </summary>
    public class UIEvent_UpdateHPData
    {
        public int HP_Cur;
        public int HP_Max;
    }
    /// <summary>
    /// UI-更新个人属性Food
    /// </summary>
    public class UIEvent_UpdateFoodData
    {
        public int Food_Cur;
        public int Food_Max;
    }
    /// <summary>
    /// UI-更新个人属性San
    /// </summary>
    public class UIEvent_UpdateSanData
    {
        public int San_Cur;
        public int San_Max;
    }
    /// <summary>
    /// UI-更新个人属性Armor
    /// </summary>
    public class UIEvent_UpdateArmorData
    {
        public short Armor;
    }
    /// <summary>
    /// UI-更新个人属性Resistance
    /// </summary>
    public class UIEvent_UpdateResistanceData
    {
        public short Resistance;
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
    /// UI-更新延迟
    /// </summary>
    public class UIEvent_UpdatePing
    {
        public double ping;
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
    /// 更新Buff
    /// </summary>
    public class UIEvent_UpdateBuff
    {
        public List<BuffConfig> buffConfigs;
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
    /// 显示鼠标
    /// </summary>
    public class UIEvent_ShowCursorImage
    {
        public Sprite sprite;
    }
    /// <summary>
    /// 隐藏鼠标
    /// </summary>
    public class UIEvent_HideCursorImage
    {

    }
    /// <summary>
    /// 开始持有
    /// </summary>
    public class UIEvent_StartHoldingItem
    {
        public UI_GridCell itemCell;
        /// <summary>
        /// 原本的部分
        /// </summary>
        public ItemData itemData_From;
        /// <summary>
        /// 取出的部分
        /// </summary>
        public ItemData itemData_Out;
    }
}
