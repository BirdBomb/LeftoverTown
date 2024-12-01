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
        public int bagCapacity;
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
    /// UI-������������
    /// </summary>
    public class UIEvent_UpdateStatus
    {
        public short statusId;
    }
    /// <summary>
    /// UI-���¸�������HP
    /// </summary>
    public class UIEvent_UpdateHPData
    {
        public int HP;
        public int MaxHP;
    }
    /// <summary>
    /// UI-���¸�������Food
    /// </summary>
    public class UIEvent_UpdateFoodData
    {
        public int Food;
        public int MaxFood;
    }
    /// <summary>
    /// UI-���¸�������San
    /// </summary>
    public class UIEvent_UpdateSanData
    {
        public int San;
        public int MaxSan;
    }
    /// <summary>
    /// UI-���¸�������Armor
    /// </summary>
    public class UIEvent_UpdateArmorData
    {
        public short Armor;
    }
    /// <summary>
    /// UI-���¸�������Water
    /// </summary>
    public class UIEvent_UpdateWaterData
    {
        public short Water;
    }
    /// <summary>
    /// UI-���¸�������Happy
    /// </summary>
    public class UIEvent_UpdateHappyData
    {
        public short Happy;
    }
    /// <summary>
    /// UI-���¸�������Coin
    /// </summary>
    public class UIEvent_UpdateCoinData
    {
        public int Coin;
    }
    /// <summary>
    /// UI-���¸�������Fine
    /// </summary>
    public class UIEvent_UpdateFineData
    {
        public int Fine;
    }
    /// <summary>
    /// ���buff
    /// </summary>
    public class UIEvent_AddBuff
    {
        public BuffConfig buffConfig;
    }
    /// <summary>
    /// �Ƴ�buff
    /// </summary>
    public class UIEvent_SubBuff
    {
        public BuffConfig buffConfig;
    }
    /// <summary>
    /// ��ʾ��Ϣ����
    /// </summary>
    public class UIEvent_ShowInfoTextUI
    {
        public Vector2 anchor;
        public string text;
    }
    /// <summary>
    /// ����������Ϣ
    /// </summary>
    public class UIEvent_HidenfoTextUI
    {

    }
    /// <summary>
    /// ��ʾȫ������
    /// </summary>
    public class UIEvent_ShowGlobalTextUI
    {
        public string text;
    }

}
