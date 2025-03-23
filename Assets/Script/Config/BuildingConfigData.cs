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
    /*1000-1999����*/
    /*2000-2999�Ҿ�*/
    /*3000-3999��е*/
    /*4000-4999*/
    /*8000-8999��Ϊ�ؿ�*/
    /*9000-9999��Ȼ�ؿ�*/
    public readonly static  List<BuildingConfig> buildConfigs = new List<BuildingConfig>()
    {
        #region//(���)���̽���
        new BuildingConfig(){ Building_ID = 0,Building_Name ="��",
            Building_Armor = 10,Building_RawLevel = 10,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Lock,
            Building_Raw = new List<RawItem>(){ } },
        new BuildingConfig(){ Building_ID = 99,Building_Name ="����ռλ",
           Building_Armor = 10,Building_RawLevel = 0,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Lock,
            Building_Raw = new List<RawItem>(){ } },
        #endregion
        #region//(���)�ṹ��
        new BuildingConfig(){ Building_ID = 1000,Building_Name ="ľǽ",
            Building_Armor = 10,Building_RawLevel = 1,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Structure,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        new BuildingConfig(){ Building_ID = 1001,Building_Name ="ľ��",
           Building_Armor = 10,Building_RawLevel = 1,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Structure,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        new BuildingConfig(){ Building_ID = 1002,Building_Name ="ľդ��",
            Building_Armor = 10,Building_RawLevel = 4,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Structure,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        new BuildingConfig(){ Building_ID = 1003,Building_Name ="ľ��",
            Building_Armor = 10,Building_RawLevel = 2,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Structure,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        new BuildingConfig(){ Building_ID = 1010,Building_Name ="שǽ",
            Building_Armor = 10,Building_RawLevel = 2,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Structure,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        new BuildingConfig(){ Building_ID = 1021,Building_Name ="����",
            Building_Armor = 10,Building_RawLevel = 2,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Structure,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        #endregion
        #region//(���)�Ҿ�
        #region//����
        new BuildingConfig(){ Building_ID = 2000,Building_Name ="ľ��",
            Building_Armor = 10,Building_RawLevel = 1,Building_Size = AreaSize._1X2,Building_Type = BuildingType.Furniture,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) },Building_Group = new List<short>(){ 20000,20001,20002 } },
        #region//ľ��
        new BuildingConfig(){ Building_ID = 20000,Building_Name ="ľ��(��)",
            Building_Armor = 10,Building_RawLevel = 1,Building_Size = AreaSize._1X2,Building_Type = BuildingType.Furniture,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        new BuildingConfig(){ Building_ID = 20001,Building_Name ="ľ��(��)",
            Building_Armor = 10,Building_RawLevel = 1,Building_Size = AreaSize._2X1,Building_Type = BuildingType.Furniture,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        new BuildingConfig(){ Building_ID = 20002,Building_Name ="ľ��(��)",
            Building_Armor = 10,Building_RawLevel = 1,Building_Size = AreaSize._2X1,Building_Type = BuildingType.Furniture,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        #endregion
        new BuildingConfig(){ Building_ID = 2010,Building_Name ="ľ����(��)",
            Building_Armor = 10,Building_RawLevel = 1,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Furniture,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        new BuildingConfig(){ Building_ID = 2011,Building_Name ="ľ����(��)",
            Building_Armor = 10,Building_RawLevel = 1,Building_Size = AreaSize._2X1,Building_Type = BuildingType.Furniture,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        new BuildingConfig(){ Building_ID = 2012,Building_Name ="ľ����",
            Building_Armor = 10,Building_RawLevel = 1,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Furniture,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        new BuildingConfig(){ Building_ID = 2013,Building_Name ="ľ������",
            Building_Armor = 10,Building_RawLevel = 1,Building_Size = AreaSize._2X2,Building_Type = BuildingType.Furniture,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        new BuildingConfig(){ Building_ID = 2014,Building_Name ="ľ�¹�",
            Building_Armor = 10,Building_RawLevel = 1,Building_Size = AreaSize._2X1,Building_Type = BuildingType.Furniture,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        new BuildingConfig(){ Building_ID = 2015,Building_Name ="ľͰ",
            Building_Armor = 10,Building_RawLevel = 4,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Furniture,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        new BuildingConfig(){ Building_ID = 2016,Building_Name ="����",
            Building_Armor = 10,Building_RawLevel = 4,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Furniture,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        #endregion
        #region//����
        new BuildingConfig(){ Building_ID = 2100,Building_Name ="���",
            Building_Armor = 10,Building_RawLevel = 1,Building_Size = AreaSize._2X1,Building_Type = BuildingType.Furniture,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        new BuildingConfig(){ Building_ID = 2101,Building_Name ="ľ����",
            Building_Armor = 10,Building_RawLevel = 4,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Furniture,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        new BuildingConfig(){ Building_ID = 2102,Building_Name ="ľ��",
            Building_Armor = 10,Building_RawLevel = 4,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Furniture,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5),new RawItem(1001, 5) },Building_Group = new List<short>(){ 21020, 21021, 21022, 21023 } },
        #region//ľ��
        new BuildingConfig(){ Building_ID = 21020,Building_Name ="ľ��(��)",
            Building_Armor = 10,Building_RawLevel = 4,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Furniture,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        new BuildingConfig(){ Building_ID = 21021,Building_Name ="ľ��(��)",
            Building_Armor = 10,Building_RawLevel = 4,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Furniture,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        new BuildingConfig(){ Building_ID = 21022,Building_Name ="ľ��(��)",
            Building_Armor = 10,Building_RawLevel = 4,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Furniture,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        new BuildingConfig(){ Building_ID = 21023,Building_Name ="ľ��(��)",
            Building_Armor = 10,Building_RawLevel = 4,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Furniture,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        #endregion
        #endregion
        #region//����
        new BuildingConfig(){ Building_ID = 2200,Building_Name ="����",
            Building_Armor = 10,Building_RawLevel = 0,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Furniture,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        new BuildingConfig(){ Building_ID = 2201,Building_Name ="ʯͷ����",
            Building_Armor = 10,Building_RawLevel = 0,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Furniture,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        new BuildingConfig(){ Building_ID = 2210,Building_Name ="ʯͷ����",
            Building_Armor = 10,Building_RawLevel = 0,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Furniture,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },

        #endregion
        #region//�Ƽ�
        new BuildingConfig(){ Building_ID = 2300,Building_Name ="����̨",
            Building_Armor = 10,Building_RawLevel = 0,Building_Size = AreaSize._2X1,Building_Type = BuildingType.Furniture,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        #endregion
        #endregion

        #region//(���)��е
        #region//����
        new BuildingConfig(){ Building_ID = 3000,Building_Name ="��ص�",
            Building_Armor = 10,Building_RawLevel = 5,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Machine,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        new BuildingConfig(){ Building_ID = 3001,Building_Name ="·��",
           Building_Armor = 10,Building_RawLevel = 5,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Machine,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        #endregion
        #region//����
        new BuildingConfig(){ Building_ID = 3100,Building_Name ="����",
            Building_Armor = 10,Building_RawLevel = 2,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Machine,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        new BuildingConfig(){ Building_ID = 3101,Building_Name ="����",
            Building_Armor = 10,Building_RawLevel = 2,Building_Size = AreaSize._2X1,Building_Type = BuildingType.Machine,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        #endregion
        #endregion

        #region//(���)��ɫ�����ؿ�
        #region//���
        new BuildingConfig(){ Building_ID = 8000,Building_Name ="׹��̫����̳",
            Building_Armor = 9999,Building_RawLevel = 1,Building_Size = AreaSize._3X3,Building_Type =BuildingType.Lock,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        #endregion
        #region//����
        new BuildingConfig(){ Building_ID = 8100,Building_Name ="���Ӷ�",
            Building_Armor = 10,Building_RawLevel = 1,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Lock,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        #endregion
        #region//����
        #endregion
        #region//NPC
        new BuildingConfig(){ Building_ID = 8300,Building_Name ="��������",
            Building_Armor = 10,Building_RawLevel = 1,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Lock,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        new BuildingConfig(){ Building_ID = 8301,Building_Name ="���˹���̨",
            Building_Armor = 10,Building_RawLevel = 1,Building_Size = AreaSize._2X1,Building_Type =BuildingType.Lock,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        #endregion
        #region//Boss
        #endregion
        #endregion
        #region//(���)��Ȼ�ؿ�
        #region//ֲ��
        new BuildingConfig(){ Building_ID = 9000,Building_Name ="������(��ͨ)", 
            Building_Armor = 0,Building_RawLevel = 10,Building_Size = AreaSize._1X1,Building_Type= BuildingType.Lock,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        new BuildingConfig(){ Building_ID = 9001,Building_Name ="������(���)", 
            Building_Armor = 0,Building_RawLevel = 10,Building_Size = AreaSize._1X1,Building_Type= BuildingType.Lock,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        new BuildingConfig(){ Building_ID = 9010,Building_Name ="�̹�ľ", 
           Building_Armor = 0,Building_RawLevel = 10,Building_Size = AreaSize._1X1,Building_Type= BuildingType.Lock,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        new BuildingConfig(){ Building_ID = 9020,Building_Name ="������ֲ��",
            Building_Armor = 0,Building_RawLevel = 10,Building_Size = AreaSize._1X1,Building_Type= BuildingType.Lock,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        #endregion
        #region//����
        new BuildingConfig(){ Building_ID = 9100,Building_Name ="ʯͷ",
            Building_Armor = 10,Building_RawLevel = 10,Building_Size = AreaSize._1X1,Building_Type= BuildingType.Lock,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        #endregion
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
    [SerializeField]/*��Դ��*/
    public string Building_FileName;
    [SerializeField]/*����Ӳ��*/
    public short Building_Armor;
    [SerializeField]/*�����ȼ�*/
    public short Building_RawLevel;
    [SerializeField]/*�����ߴ�*/
    public AreaSize Building_Size;
    [SerializeField]/*�������*/
    public BuildingType Building_Type;
    [SerializeField]/*����ԭ��*/
    public List<RawItem>Building_Raw;
    [SerializeField]/*����Ⱥ*/
    public List<short> Building_Group;
}
/// <summary>
/// ����ԭ��
/// </summary>
public struct RawItem
{
    public short ID;
    public short Count;
    public RawItem(short id, short count)
    {
        ID = id;
        Count = count;
    }
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
    /// ���ܽ���
    /// </summary>
    Lock,
    /// <summary>
    /// �ṹ
    /// </summary>
    Structure,
    /// <summary>
    /// �Ҿ�
    /// </summary>
    Furniture,
    /// <summary>
    /// ��е
    /// </summary>
    Machine,
}