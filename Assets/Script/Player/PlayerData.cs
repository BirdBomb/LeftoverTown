using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerData 
{
    public PlayerData()
    {
        Name = "������";
        Hp_Cur = 100;
        Hp_Max = 100;
        Armor_Cur = 0;
        Food_Cur = 100;
        Food_Max = 100;
        Water_Cur = 5;
        San_Cur = 100;
        San_Max = 100;
        Happy_Cur = 5;
        Coin_Cur = 100;
        En_Cur = 3000;
        Speed_Common = 60;
        Speed_Max = 80;

        Point_Strength = 1;
        Point_Intelligence = 1;
        Point_SPower = 1;
        Point_Focus = 1;
        Point_Agility = 1;
        Point_Make = 1;
        Point_Build = 1;
        Point_Cook = 1;
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
    /// ����ٶ�
    /// </summary>
    [SerializeField]
    public short Speed_Max;
    /// <summary>
    /// ����
    /// </summary>
    [SerializeField]
    public int En_Cur;
    /// <summary>
    /// ��ǰ����ֵ
    /// </summary>
    [SerializeField]
    public short Hp_Cur;
    /// <summary>
    /// �������ֵ
    /// </summary>
    [SerializeField]
    public int Hp_Max;
    /// <summary>
    /// ����ֵ
    /// </summary>
    [SerializeField]
    public short Armor_Cur;
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
    public short Coin_Cur;
    /// <summary>
    /// ����
    /// </summary>
    [SerializeField]
    public short Point_Strength;
    /// <summary>
    /// ����
    /// </summary>
    [SerializeField]
    public short Point_Intelligence;
    /// <summary>
    /// ����
    /// </summary>
    [SerializeField]
    public short Point_SPower;
    /// <summary>
    /// רע
    /// </summary>
    [SerializeField]
    public short Point_Focus;
    /// <summary>
    /// ����
    /// </summary>
    [SerializeField]
    public short Point_Agility;
    /// <summary>
    /// ����
    /// </summary>
    [SerializeField]
    public short Point_Make;
    /// <summary>
    /// ����
    /// </summary>
    [SerializeField]
    public short Point_Build;
    /// <summary>
    /// ���
    /// </summary>
    [SerializeField]
    public short Point_Cook;

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
    /// ���ѧ��ļ���
    /// </summary>
    [SerializeField]
    public List<short> SkillKnowList = new List<short>();
    /// <summary>
    /// ���ʹ�õļ���
    /// </summary>
    [SerializeField]
    public List<short> SkillUseList = new List<short>();
    /// <summary>
    /// ���Buff�б�
    /// </summary>
    [SerializeField]
    public List<short> BuffList = new List<short>();
    /// <summary>
    /// ���Buff����
    /// </summary>
    [SerializeField]
    public int BuffPoint;
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
    /// <summary>
    /// �۾�ID
    /// </summary>
    [SerializeField]
    public short Eye_ID;
}
