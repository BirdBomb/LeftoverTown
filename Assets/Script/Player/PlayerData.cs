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
        Water = 100;
        CurSan = 100;
        MaxSan = 100;
        Happy = 100;

        CommonSpeed = 2;
        MaxSpeed = 2;

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
    public int CommonSpeed;
    /// <summary>
    /// ����ٶ�
    /// </summary>
    [SerializeField]
    public int MaxSpeed;
    /// <summary>
    /// ����
    /// </summary>
    [SerializeField]
    public int En;
    /// <summary>
    /// ��ǰ����ֵ
    /// </summary>
    [SerializeField]
    public int CurHp;
    /// <summary>
    /// �������ֵ
    /// </summary>
    [SerializeField]
    public int MaxHp;
    /// <summary>
    /// ����ֵ
    /// </summary>
    [SerializeField]
    public int Armor;
    /// <summary>
    /// ��ǰʳ��ֵ
    /// </summary>
    [SerializeField]
    public int CurFood;
    /// <summary>
    /// ���ʳ��ֵ
    /// </summary>
    [SerializeField]
    public int MaxFood;
    /// <summary>
    /// ȱˮֵ
    /// </summary>
    [SerializeField]
    public int Water;
    /// <summary>
    /// ��ǰ����ֵ
    /// </summary>
    [SerializeField]
    public int CurSan;
    /// <summary>
    /// �����ֵ
    /// </summary>
    [SerializeField]
    public int MaxSan;
    /// <summary>
    /// ����ֵ
    /// </summary>
    [SerializeField]
    public int Happy;
    /// <summary>
    /// ����
    /// </summary>
    [SerializeField]
    public int Point_Strength;
    /// <summary>
    /// ����
    /// </summary>
    [SerializeField]
    public int Point_Intelligence;
    /// <summary>
    /// ����
    /// </summary>
    [SerializeField]
    public int Point_SPower;
    /// <summary>
    /// רע
    /// </summary>
    [SerializeField]
    public int Point_Focus;
    /// <summary>
    /// ����
    /// </summary>
    [SerializeField]
    public int Point_Agility;
    /// <summary>
    /// ����
    /// </summary>
    [SerializeField]
    public int Point_Make;
    /// <summary>
    /// ����
    /// </summary>
    [SerializeField]
    public int Point_Build;
    /// <summary>
    /// ���
    /// </summary>
    [SerializeField]
    public int Point_Cook;

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
    public List<int> SkillKnowList = new List<int>();
    /// <summary>
    /// ���ʹ�õļ���
    /// </summary>
    [SerializeField]
    public List<int> SkillUseList = new List<int>();
    /// <summary>
    /// ���Buff�б�
    /// </summary>
    [SerializeField]
    public List<int> BuffList = new List<int>();
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
