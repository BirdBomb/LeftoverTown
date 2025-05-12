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
        new BuildingConfig()
        {
            Building_ID = 0,Building_Name ="��",
            Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()
        {
            Building_ID = 99,Building_Name ="ռλ",
            Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        #region//��Ȼ����
        new BuildingConfig()
        {
            Building_ID = 1000,Building_Name ="ɭ������",
            Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()
        {
            Building_ID = 1001,Building_Name ="ɭ�ֳ��ݴ�",
            Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()
        {
            Building_ID = 1002,Building_Name ="ʯǽ",
            Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()
        {
            Building_ID = 1003,Building_Name ="ɭ��ɡ��",
            Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()
        {
            Building_ID = 1004,Building_Name ="������׮",
            Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        #endregion
        #region//����Ȼ����
        new BuildingConfig()
        {
            Building_ID = 2000,Building_Name ="̫����̳",
            Building_Size = AreaSize._3X3,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()
        {
            Building_ID = 2001,Building_Name ="���Ӷ�",
            Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()
        {
            Building_ID = 2002,Building_Name ="Ĺ��",
            Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()
        {
            Building_ID = 2003,Building_Name ="���˹���̨",
            Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()
        {
            Building_ID = 2004,Building_Name ="���",
            Building_Size = AreaSize._3X3,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        #endregion
        #region//ʯ��ʱ������
        new BuildingConfig()
        {
            Building_ID = 3000,Building_Name ="ľǽ",
            Building_Size = AreaSize._1X1,Building_Type = BuildingType.Structure,Building_Age = AgeGroup.StoneAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1100,2) },
        },
        new BuildingConfig()
        {
            Building_ID = 3001,Building_Name ="ľ��",
            Building_Size = AreaSize._1X1,Building_Type = BuildingType.Structure,Building_Age = AgeGroup.StoneAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1100,2) },
        },
        new BuildingConfig()
        {
            Building_ID = 3002,Building_Name ="ľդ��",
            Building_Size = AreaSize._1X1,Building_Type = BuildingType.Structure,Building_Age = AgeGroup.StoneAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1100,2) },
        },
        new BuildingConfig()
        {
            Building_ID = 3003,Building_Name ="ľ�ӹ�̨",
            Building_Size = AreaSize._2X1,Building_Type = BuildingType.Machine,Building_Age = AgeGroup.StoneAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1000,10) },
        },
        new BuildingConfig()
        {
            Building_ID = 3004,Building_Name ="ľ����",
            Building_Size = AreaSize._1X1,Building_Type = BuildingType.Furniture,Building_Age = AgeGroup.StoneAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1100,4) },
        },
        new BuildingConfig()
        {
            Building_ID = 3005,Building_Name ="ʯͷ��¯",
            Building_Size = AreaSize._1X1,Building_Type = BuildingType.Machine,Building_Age = AgeGroup.StoneAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1100,4),new ItemRaw(1010,4) },
        },
        #endregion
        #region//����ʱ������
        new BuildingConfig()
        {
            Building_ID = 4000,Building_Name ="���Ƽӹ�̨",
            Building_Size = AreaSize._2X1,Building_Type = BuildingType.Machine,Building_Age = AgeGroup.IronAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1110,6) },
        },
        new BuildingConfig()
        {
            Building_ID = 4001,Building_Name ="ǹе�ӹ�̨",
            Building_Size = AreaSize._2X1,Building_Type = BuildingType.Machine,Building_Age = AgeGroup.IronAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1110,6) },
        },
        new BuildingConfig()
        {
            Building_ID = 4002,Building_Name ="�����",
            Building_Size = AreaSize._1X1,Building_Type = BuildingType.Machine,Building_Age = AgeGroup.IronAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1100,4),new ItemRaw(1010,4) },
        },
        new BuildingConfig()
        {
            Building_ID = 4003,Building_Name ="���̨",
            Building_Size = AreaSize._1X1,Building_Type = BuildingType.Machine,Building_Age = AgeGroup.IronAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1100,4),new ItemRaw(1010,4) },
        },
        new BuildingConfig()
        {
            Building_ID = 4004,Building_Name ="שǽ",
            Building_Size = AreaSize._1X1,Building_Type = BuildingType.Structure,Building_Age = AgeGroup.IronAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1010,4) },
        },
        #endregion
    };
}
[Serializable]
public struct BuildingConfig
{
    [SerializeField]/*���*/
    public short Building_ID;
    [SerializeField]/*����*/
    public string Building_Name;
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
    /// ��ҵʱ��
    /// </summary>
    IndustrialAge_1,
    /// <summary>
    /// ħ��ʱ��
    /// </summary>
    MagicSAge_0,
    /// <summary>
    /// ��ħ��ʱ��
    /// </summary>
    MagicSAge_1,
}