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
        public List<ItemConfig> itemConfigs = new List<ItemConfig>();
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
    /// UI-更新个人属性
    /// </summary>
    public class UIEvent_UpdateData
    {
        public int HP;
        public int MaxHP;
    }
    /// <summary>
    /// 显示文字
    /// </summary>
    public class UIEvent_ShowTextUI
    {
        public string text;
    }
}
