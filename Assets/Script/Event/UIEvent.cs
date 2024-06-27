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
    /// UI-������������
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
    }
    /// <summary>
    /// ��ʾ����
    /// </summary>
    public class UIEvent_ShowTextUI
    {
        public string text;
    }
}
