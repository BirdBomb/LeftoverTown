using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIEvent 
{
    /// <summary>
    /// UI-���³�������
    /// </summary>
    public class UIEvent_UpdateItemInHand
    {
        public int playerID;
        public ItemData itemData;
    }
    /// <summary>
    /// UI-���±�������
    /// </summary>
    public class UIEvent_UpdateItemInBag
    {
        public List<ItemConfig> itemConfigs = new List<ItemConfig>();
        public List<ItemData> itemDatas = new List<ItemData>();
    }
    /// <summary>
    /// UI-����ͷ������
    /// </summary>
    public class UIEvent_UpdateItemOnHead
    {
        public ItemData itemData;
    }
    /// <summary>
    /// UI-����ͷ������
    /// </summary>
    public class UIEvent_UpdateItemOnBody
    {
        public ItemData itemData;
    }

    /// <summary>
    /// UI-���¸�������
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
    /// <summary>
    /// UI-��һ���ⲿGrid
    /// </summary>
    public class UIEvent_OpenGridUI
    {
        public UI_Grid bindUI;
    }
    /// <summary>
    /// UI-�ر�һ���ⲿGrid
    /// </summary>
    public class UIEvent_CloseGridUI
    {
        public UI_Grid bindUI;
    }
}
