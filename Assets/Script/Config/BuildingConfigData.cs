using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingConfigData : MonoBehaviour
{
    public static BuildingConfig GetBuildingConfig(int ID)
    {
        return buildConfigs.Find((x) => { return x.Building_ID == ID; });
    }
    public readonly static List<BuildingConfig> buildConfigs = new List<BuildingConfig>()
    {
        /*��*/new BuildingConfig()
        {
            Building_ID = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*ռλ*/new BuildingConfig()
        {
            Building_ID = 99,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        #region//��Ȼ����1000
        /*ɭ������*/new BuildingConfig()
        {
            Building_ID = 1000,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*ɭ�ֳ��ݴ�*/new BuildingConfig()
        {
            Building_ID = 1001,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*ʯǽ*/new BuildingConfig()
        {
            Building_ID = 1002,Building_Hp = 20,Building_Armor = 2,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*ɭ��ɡ��*/new BuildingConfig()
        {
            Building_ID = 1003,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*������׮*/new BuildingConfig()
        {
            Building_ID = 1004,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*���ֲ��*/new BuildingConfig()
        {
            Building_ID = 1005,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*����ֲ��*/new BuildingConfig()
        {
            Building_ID = 1006,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*���ٹ�ľ*/new BuildingConfig()
        {
            Building_ID = 1007,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*����*/new BuildingConfig()
        {
            Building_ID = 1008,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*�����ʯ*/new BuildingConfig()
        {
            Building_ID = 1009,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*����*/new BuildingConfig()
        {
            Building_ID = 1010,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*ú��ǽ*/new BuildingConfig()
        {
            Building_ID = 1011,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*����ǽ*/new BuildingConfig()
        {
            Building_ID = 1012,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*���ǽ*/new BuildingConfig()
        {
            Building_ID = 1013,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*ˮ��ǽ*/new BuildingConfig()
        {
            Building_ID = 1014,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*���ǽ*/new BuildingConfig()
        {
            Building_ID = 1015,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*��ʯǽ*/new BuildingConfig()
        {
            Building_ID = 1016,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*��ʯǽ*/new BuildingConfig()
        {
            Building_ID = 1017,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*ѩԭ��*/new BuildingConfig()
        {
            Building_ID = 1018,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*ɳĮ�����*/new BuildingConfig()
        {
            Building_ID = 1019,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        #endregion
        #region//����Ȼ����2000
        /*̫����̳*/new BuildingConfig()
        {
            Building_ID = 2000,Building_Hp = int.MaxValue,Building_Armor = int.MaxValue,Building_Size = AreaSize._3X3,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*��������*/new BuildingConfig()
        {
            Building_ID = 2001,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*��ʬ����*/new BuildingConfig()
        {
            Building_ID = 2002,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*��������*/new BuildingConfig()
        {
            Building_ID = 2003,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*������ʬ����*/new BuildingConfig()
        {
            Building_ID = 2004,Building_Hp = int.MaxValue,Building_Armor = int.MaxValue,Building_Size = AreaSize._3X3,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*��������*/new BuildingConfig()
        {
            Building_ID = 2005,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*ҹ����������*/new BuildingConfig()
        {
            Building_ID = 2006,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*�װ���������*/new BuildingConfig()
        {
            Building_ID = 2007,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*ũ������*/new BuildingConfig()
        {
            Building_ID = 2008,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*������������*/new BuildingConfig()
        {
            Building_ID = 2009,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*��ľ������*/new BuildingConfig()
        {
            Building_ID = 2010,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._2X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*��������*/new BuildingConfig()
        {
            Building_ID = 2011,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._2X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*��ʦ����*/new BuildingConfig()
        {
            Building_ID = 2012,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*������*/new BuildingConfig()
        {
            Building_ID = 2013,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._2X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*�Ʊ�����*/new BuildingConfig()
        {
            Building_ID = 2014,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._2X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*��������*/new BuildingConfig()
        {
            Building_ID = 2015,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._2X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*ʯͷ����*/new BuildingConfig()
        {
            Building_ID = 2100,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*����*/new BuildingConfig()
        {
            Building_ID = 2101,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*��̨*/new BuildingConfig()
        {
            Building_ID = 2102,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*ľͰ*/new BuildingConfig()
        {
            Building_ID = 2103,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        #endregion
        #region//ʯ��ʱ������3000
        /*ľǽ*/new BuildingConfig()
        {
            Building_ID = 3000,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Structure,Building_Age = AgeGroup.StoneAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1100,2) },
        },
        /*ľ��*/new BuildingConfig()
        {
            Building_ID = 3001,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Structure,Building_Age = AgeGroup.StoneAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1100,2) },
        },
        /*ľդ��*/new BuildingConfig()
        {
            Building_ID = 3002,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Structure,Building_Age = AgeGroup.StoneAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1100,2) },
        },
        /*ľ�ӹ�̨*/new BuildingConfig()
        {
            Building_ID = 3003,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._2X1,Building_Type = BuildingType.Machine,Building_Age = AgeGroup.StoneAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1000,10) },
        },
        /*ľ����*/new BuildingConfig()
        {
            Building_ID = 3004,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Furniture,Building_Age = AgeGroup.StoneAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1100,4) },
        },
        /*ʯͷ��¯*/new BuildingConfig()
        {
            Building_ID = 3005,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Machine,Building_Age = AgeGroup.StoneAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1100,4),new ItemRaw(1010,4) },
        },
        /*ʯͷ����*/new BuildingConfig()
        {
            Building_ID = 3006,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Machine,Building_Age = AgeGroup.StoneAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1100,4),new ItemRaw(1010,4) },
        },
        /*ľͷ����*/new BuildingConfig()
        {
            Building_ID = 3007,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Machine,Building_Age = AgeGroup.StoneAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1100,4),new ItemRaw(1010,4) },
        },
        /*ľ��*/new BuildingConfig()
        {
            Building_ID = 3008,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._2X1,Building_Type = BuildingType.Machine,Building_Age = AgeGroup.StoneAge,
            Building_Group = new List<short>{ 10010,10011,10012 },
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1100,4),new ItemRaw(1010,4) },
        },
        /*ľ��*/new BuildingConfig()
        {
            Building_ID = 3009,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Machine,Building_Age = AgeGroup.StoneAge,
            Building_Group = new List<short>{ 10020, 10021, 10022, 10023 },
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1100,4),new ItemRaw(1010,4) },
        },
        /*ľ�ƻ���*/new BuildingConfig()
        {
            Building_ID = 3010,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Machine,Building_Age = AgeGroup.StoneAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1100,4),new ItemRaw(1010,4) },
        },
        #endregion
        #region//����ʱ������4000
        /*���Ƽӹ�̨*/new BuildingConfig()
        {
            Building_ID = 4000,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._2X1,Building_Type = BuildingType.Machine,Building_Age = AgeGroup.IronAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1110,6) },
        },
        /*ǹе�ӹ�̨*/new BuildingConfig()
        {
            Building_ID = 4001,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._2X1,Building_Type = BuildingType.Machine,Building_Age = AgeGroup.IronAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1110,6) },
        },
        /*�����*/new BuildingConfig()
        {
            Building_ID = 4002,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Machine,Building_Age = AgeGroup.IronAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1100,4),new ItemRaw(1010,4) },
        },
        /*���̨*/new BuildingConfig()
        {
            Building_ID = 4003,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Machine,Building_Age = AgeGroup.IronAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1100,4),new ItemRaw(1010,4) },
        },
        /*שǽ*/new BuildingConfig()
        {
            Building_ID = 4004,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Structure,Building_Age = AgeGroup.IronAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1010,4) },
        },
        /*����*/new BuildingConfig()
        {
            Building_ID = 4005,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Structure,Building_Age = AgeGroup.IronAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1110, 4) },
        },
        /*���ϻ�*/new BuildingConfig()
        {
            Building_ID = 4006,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Structure,Building_Age = AgeGroup.IronAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1110, 4) },
        },
        #endregion
        #region//��������10000
        /*��ľ��*/new BuildingConfig()
        {
            Building_ID = 10010,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._2X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*��ľ��*/new BuildingConfig()
        {
            Building_ID = 10011,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._2X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*��ľ��*/new BuildingConfig()
        {
            Building_ID = 10012,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._1X2,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*��ľ��*/new BuildingConfig()
        {
            Building_ID = 10020,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*��ľ��*/new BuildingConfig()
        {
            Building_ID = 10021,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*��ľ��*/new BuildingConfig()
        {
            Building_ID = 10022,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*��ľ��*/new BuildingConfig()
        {
            Building_ID = 10023,Building_Hp = 20,Building_Armor = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },

        #endregion
    };
}
[Serializable]
public struct BuildingConfig
{
    [SerializeField]/*�������*/
    public short Building_ID;
    [SerializeField]/*��������ֵ*/
    public int Building_Hp;
    [SerializeField]/*��������*/
    public int Building_Armor;
    [SerializeField]/*�����ߴ�*/
    public AreaSize Building_Size;
    [SerializeField]/*�������*/
    public BuildingType Building_Type;
    [SerializeField]/*����ʱ��*/
    public AgeGroup Building_Age;
    [SerializeField]/*����ԭ��*/
    public List<ItemRaw>Building_Raw;
    [SerializeField]/*����Ⱥ*/
    public List<short> Building_Group;
}
/// <summary>
/// �����ߴ�
/// </summary>
public enum AreaSize
{
    _1X1,
    _1X2,
    _2X1,
    _2X2,
    _3X3,
}
/// <summary>
/// �������
/// </summary>
public enum BuildingType
{
    /// <summary>
    /// ����
    /// </summary>
    All,
    /// <summary>
    /// �ṹ
    /// </summary>
    Structure,
    /// <summary>
    /// �Ҿ�
    /// </summary>
    Furniture,
    /// <summary>
    /// ��ʩ
    /// </summary>
    Machine,
    /// <summary>
    /// ����
    /// </summary>
    Other,
    /// <summary>
    /// ����
    /// </summary>
    Ground,
}
/// <summary>
/// ʱ��
/// </summary>
public enum AgeGroup
{
    /// <summary>
    /// ��Ȼ����
    /// </summary>
    Nature,
    /// <summary>
    /// ����Ȼ����
    /// </summary>
    UnNature,
    /// <summary>
    /// ʯ��ʱ��
    /// </summary>
    StoneAge,
    /// <summary>
    /// ����ʱ��
    /// </summary>
    IronAge,
    /// <summary>
    /// ��ҵʱ��
    /// </summary>
    IndustrialAge_0,
    /// <summary>
    /// ħ��ʱ��
    /// </summary>
    MagicAge_0,
    /// <summary>
    /// ��ҵʱ��
    /// </summary>
    IndustrialAge_1,
    /// <summary>
    /// ��ħ��ʱ��
    /// </summary>
    MagicAge_1,
}