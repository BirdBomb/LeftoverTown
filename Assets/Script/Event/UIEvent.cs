using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIEvent 
{
    /// <summary>
    /// UI-���±�������
    /// </summary>
    public class UIEvent_UpdateItemInBag
    {
        public int bagCapacity;
        public List<ItemData> itemDatas = new List<ItemData>();
    }
    /// <summary>
    /// UI_��ӱ�������
    /// </summary>
    public class UIEvent_PutItemInBag
    {
        public ItemData item;
    }
    /// <summary>
    /// UI_�Ƴ���������
    /// </summary>
    public class UIEvent_PutItemOutBag
    {
        public ItemData item;
    }
    /// <summary>
    /// UI_ʹ�ñ�������
    /// </summary>
    public class UIEvent_TryUseItemInBag 
    {
        public int index;
    }

    /// <summary>
    /// UI-���³�������
    /// </summary>
    public class UIEvent_ItemHand_Update
    {
        public ItemData itemData;
    }
    /// <summary>
    /// UI-����ͷ������
    /// </summary>
    public class UIEvent_ItemHead_Update
    {
        public ItemData itemData;
    }
    /// <summary>
    /// UI-������������
    /// </summary>
    public class UIEvent_ItemBody_Update
    {
        public ItemData itemData;
    }
    /// <summary>
    /// UI-������Ʒ����
    /// </summary>
    public class UIEvent_ItemAccessory_Update
    {
        public ItemData itemData;
    }
    /// <summary>
    /// UI-���ºĲ�����
    /// </summary>
    public class UIEvent_ItemConsumables_Update
    {
        public ItemData itemData;
    }
    /// <summary>
    /// UI-������������
    /// </summary>
    public class UIEvent_UpdateStatus
    {
        public StatusType statusType;
    }
    /// <summary>
    /// UI-���¸�������HP
    /// </summary>
    public class UIEvent_UpdateHPData
    {
        public int HP_Cur;
        public int HP_Max;
    }
    /// <summary>
    /// UI-���¸�������Food
    /// </summary>
    public class UIEvent_UpdateFoodData
    {
        public int Food_Cur;
        public int Food_Max;
    }
    /// <summary>
    /// UI-���¸�������San
    /// </summary>
    public class UIEvent_UpdateSanData
    {
        public int San_Cur;
        public int San_Max;
    }
    /// <summary>
    /// UI-���¸�������Armor
    /// </summary>
    public class UIEvent_UpdateArmorData
    {
        public short Armor;
    }
    /// <summary>
    /// UI-���¸�������Resistance
    /// </summary>
    public class UIEvent_UpdateResistanceData
    {
        public short Resistance;
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
    /// UI-�����ӳ�
    /// </summary>
    public class UIEvent_UpdatePing
    {
        public double ping;
    }
    /// <summary>
    /// ���buff
    /// </summary>
    public class UIEvent_AddBuff
    {
        public BuffData buffData;
    }
    /// <summary>
    /// �Ƴ�buff
    /// </summary>
    public class UIEvent_SubBuff
    {
        public short buffID;
    }
    /// <summary>
    /// ����Buff�б�
    /// </summary>
    public class UIEvent_UpdateBuffList
    {
        public List<BuffData> buffList;
    }
    /// <summary>
    /// ����Buff
    /// </summary>
    public class UIEvent_UpdateBuff
    {
        public List<BuffConfig> buffConfigs;
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
    /// ��ʾ���
    /// </summary>
    public class UIEvent_ShowCursorImage
    {
        public Sprite sprite;
    }
    /// <summary>
    /// �������
    /// </summary>
    public class UIEvent_HideCursorImage
    {

    }
    /// <summary>
    /// ��ʼ����
    /// </summary>
    public class UIEvent_StartHoldingItem
    {
        public UI_GridCell itemCell;
        /// <summary>
        /// ԭ���Ĳ���
        /// </summary>
        public ItemData itemData_From;
        /// <summary>
        /// ȡ���Ĳ���
        /// </summary>
        public ItemData itemData_Out;
    }
    /// <summary>
    /// �򿪸����ʱ
    /// </summary>
    public class UIEvent_OpenReviveCountdown
    { 
        public float time;
    }
    /// <summary>
    /// �رո����ʱ
    /// </summary>
    public class UIEvent_CloseReviveCountdown
    {

    }
}
