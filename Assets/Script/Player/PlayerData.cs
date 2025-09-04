using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerData 
{
    public PlayerData()
    {
        Name = "";
        Hp_Cur = 100;
        Hp_Max = 100;
        Armor_Cur = 0;
        Resistance_Cur = 0;
        Food_Cur = 100;
        Food_Max = 100;
        Water_Cur = 5;
        San_Cur = 100;
        San_Max = 100;
        Happy_Cur = 5;
        Coin_Cur = 100;
        Speed_Common = 60;
    }
    /// <summary>
    /// ����
    /// </summary>
    [SerializeField]
    public string Name;
    /// <summary>
    /// ��̬�ٶ�
    /// </summary>
    [SerializeField]
    public short Speed_Common;
    /// <summary>
    /// ��ǰ����ֵ
    /// </summary>
    [SerializeField]
    public short Hp_Cur;
    /// <summary>
    /// �������ֵ
    /// </summary>
    [SerializeField]
    public short Hp_Max;
    /// <summary>
    /// ����ֵ
    /// </summary>
    [SerializeField]
    public short Armor_Cur;
    /// <summary>
    /// ħ��ֵ
    /// </summary>
    [SerializeField]
    public short Resistance_Cur;
    /// <summary>
    /// ��ǰʳ��ֵ
    /// </summary>
    [SerializeField]
    public short Food_Cur;
    /// <summary>
    /// ���ʳ��ֵ
    /// </summary>
    [SerializeField]
    public short Food_Max;
    /// <summary>
    /// ȱˮֵ
    /// </summary>
    [SerializeField]
    public short Water_Cur;
    /// <summary>
    /// ��ǰ����ֵ
    /// </summary>
    [SerializeField]
    public short San_Cur;
    /// <summary>
    /// �����ֵ
    /// </summary>
    [SerializeField]
    public short San_Max;
    /// <summary>
    /// ����ֵ
    /// </summary>
    [SerializeField]
    public short Happy_Cur;
    /// <summary>
    /// ���
    /// </summary>
    [SerializeField]
    public int Coin_Cur;
    /// <summary>
    /// �ͽ�
    /// </summary>
    [SerializeField]
    public int Fine_Cur;
    /// <summary>
    /// BUFF
    /// </summary>
    public List<BuffData> BuffList = new List<BuffData>();
    /// <summary>
    /// ��������
    /// </summary>
    [SerializeField]
    public List<ItemData> BagItems = new List<ItemData>();
    /// <summary>
    /// �ֲ�����
    /// </summary>
    [SerializeField]
    public ItemData HandItem;
    /// <summary>
    /// ͷ������
    /// </summary>
    [SerializeField]
    public ItemData HeadItem;
    /// <summary>
    /// ��������
    /// </summary>
    [SerializeField]
    public ItemData BodyItem;
    /// <summary>
    /// ������Ʒ
    /// </summary>
    [SerializeField]
    public ItemData ItemAccessory;
    /// <summary>
    /// ����Ĳ�
    /// </summary>
    [SerializeField]
    public ItemData ItemConsumables;
    /// <summary>
    /// �۾�ID
    /// </summary>
    [SerializeField]
    public short Eye_ID;
    /// <summary>
    /// ͷ��ID
    /// </summary>
    [SerializeField]
    public short Hair_ID;
    /// <summary>
    /// ͷ����ɫ
    /// </summary>
    [SerializeField]
    public Color32 Hair_Color;
}
