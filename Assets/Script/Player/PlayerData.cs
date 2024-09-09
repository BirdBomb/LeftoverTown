using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerData 
{
    public PlayerData()
    {
        Name = "������";
        CurHp = 100;
        MaxHp = 100;
        Armor = 0;
        CurFood = 100;
        MaxFood = 100;
        Water = 5;
        CurSan = 100;
        MaxSan = 100;
        Happy = 5;
        Coin = 100;
        En = 3000;
        CommonSpeed = 20;
        MaxSpeed = 40;

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
    /// λ��
    /// </summary>
    [SerializeField]
    public Vector3Int Pos;
    /// <summary>
    /// ��̬�ٶ�
    /// </summary>
    [SerializeField]
    public short CommonSpeed;
    /// <summary>
    /// ����ٶ�
    /// </summary>
    [SerializeField]
    public short MaxSpeed;
    /// <summary>
    /// ����
    /// </summary>
    [SerializeField]
    public int En;
    /// <summary>
    /// ��ǰ����ֵ
    /// </summary>
    [SerializeField]
    public short CurHp;
    /// <summary>
    /// �������ֵ
    /// </summary>
    [SerializeField]
    public int MaxHp;
    /// <summary>
    /// ����ֵ
    /// </summary>
    [SerializeField]
    public short Armor;
    /// <summary>
    /// ��ǰʳ��ֵ
    /// </summary>
    [SerializeField]
    public short CurFood;
    /// <summary>
    /// ���ʳ��ֵ
    /// </summary>
    [SerializeField]
    public short MaxFood;
    /// <summary>
    /// ȱˮֵ
    /// </summary>
    [SerializeField]
    public short Water;
    /// <summary>
    /// ��ǰ����ֵ
    /// </summary>
    [SerializeField]
    public short CurSan;
    /// <summary>
    /// �����ֵ
    /// </summary>
    [SerializeField]
    public short MaxSan;
    /// <summary>
    /// ����ֵ
    /// </summary>
    [SerializeField]
    public short Happy;
    /// <summary>
    /// ���
    /// </summary>
    [SerializeField]
    public short Coin;
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
    public int HairID;
    /// <summary>
    /// ͷ����ɫ
    /// </summary>
    [SerializeField]
    public Color32 HairColor;
    /// <summary>
    /// �۾�ID
    /// </summary>
    [SerializeField]
    public int EyeID;
}
