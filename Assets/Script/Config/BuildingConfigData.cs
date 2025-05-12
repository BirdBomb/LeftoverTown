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
            Building_ID = 0,Building_Name ="空",
            Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()
        {
            Building_ID = 99,Building_Name ="占位",
            Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        #region//自然建筑
        new BuildingConfig()
        {
            Building_ID = 1000,Building_Name ="森林青树",
            Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()
        {
            Building_ID = 1001,Building_Name ="森林长草丛",
            Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()
        {
            Building_ID = 1002,Building_Name ="石墙",
            Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()
        {
            Building_ID = 1003,Building_Name ="森林伞树",
            Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()
        {
            Building_ID = 1004,Building_Name ="空心树桩",
            Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        #endregion
        #region//非自然建筑
        new BuildingConfig()
        {
            Building_ID = 2000,Building_Name ="太阳祭坛",
            Building_Size = AreaSize._3X3,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()
        {
            Building_ID = 2001,Building_Name ="兔子洞",
            Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()
        {
            Building_ID = 2002,Building_Name ="墓碑",
            Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()
        {
            Building_ID = 2003,Building_Name ="猎人工具台",
            Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        new BuildingConfig()
        {
            Building_ID = 2004,Building_Name ="深坑",
            Building_Size = AreaSize._3X3,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        #endregion
        #region//石器时代建筑
        new BuildingConfig()
        {
            Building_ID = 3000,Building_Name ="木墙",
            Building_Size = AreaSize._1X1,Building_Type = BuildingType.Structure,Building_Age = AgeGroup.StoneAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1100,2) },
        },
        new BuildingConfig()
        {
            Building_ID = 3001,Building_Name ="木门",
            Building_Size = AreaSize._1X1,Building_Type = BuildingType.Structure,Building_Age = AgeGroup.StoneAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1100,2) },
        },
        new BuildingConfig()
        {
            Building_ID = 3002,Building_Name ="木栅栏",
            Building_Size = AreaSize._1X1,Building_Type = BuildingType.Structure,Building_Age = AgeGroup.StoneAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1100,2) },
        },
        new BuildingConfig()
        {
            Building_ID = 3003,Building_Name ="木加工台",
            Building_Size = AreaSize._2X1,Building_Type = BuildingType.Machine,Building_Age = AgeGroup.StoneAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1000,10) },
        },
        new BuildingConfig()
        {
            Building_ID = 3004,Building_Name ="木箱子",
            Building_Size = AreaSize._1X1,Building_Type = BuildingType.Furniture,Building_Age = AgeGroup.StoneAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1100,4) },
        },
        new BuildingConfig()
        {
            Building_ID = 3005,Building_Name ="石头熔炉",
            Building_Size = AreaSize._1X1,Building_Type = BuildingType.Machine,Building_Age = AgeGroup.StoneAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1100,4),new ItemRaw(1010,4) },
        },
        #endregion
        #region//铁器时代建筑
        new BuildingConfig()
        {
            Building_ID = 4000,Building_Name ="铁制加工台",
            Building_Size = AreaSize._2X1,Building_Type = BuildingType.Machine,Building_Age = AgeGroup.IronAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1110,6) },
        },
        new BuildingConfig()
        {
            Building_ID = 4001,Building_Name ="枪械加工台",
            Building_Size = AreaSize._2X1,Building_Type = BuildingType.Machine,Building_Age = AgeGroup.IronAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1110,6) },
        },
        new BuildingConfig()
        {
            Building_ID = 4002,Building_Name ="粉碎机",
            Building_Size = AreaSize._1X1,Building_Type = BuildingType.Machine,Building_Age = AgeGroup.IronAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1100,4),new ItemRaw(1010,4) },
        },
        new BuildingConfig()
        {
            Building_ID = 4003,Building_Name ="烹饪台",
            Building_Size = AreaSize._1X1,Building_Type = BuildingType.Machine,Building_Age = AgeGroup.IronAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1100,4),new ItemRaw(1010,4) },
        },
        new BuildingConfig()
        {
            Building_ID = 4004,Building_Name ="砖墙",
            Building_Size = AreaSize._1X1,Building_Type = BuildingType.Structure,Building_Age = AgeGroup.IronAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1010,4) },
        },
        #endregion
    };
}
[Serializable]
public struct BuildingConfig
{
    [SerializeField]/*编号*/
    public short Building_ID;
    [SerializeField]/*名字*/
    public string Building_Name;
    [SerializeField]/*建筑尺寸*/
    public AreaSize Building_Size;
    [SerializeField]/*建筑类别*/
    public BuildingType Building_Type;
    [SerializeField]/*建筑时代*/
    public AgeGroup Building_Age;
    [SerializeField]/*建筑原料*/
    public List<ItemRaw>Building_Raw;
    [SerializeField]/*建筑群*/
    public List<short> Building_Group;
}
/// <summary>
/// 建筑尺寸
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
/// 建筑类别
/// </summary>
public enum BuildingType
{
    /// <summary>
    /// 所有
    /// </summary>
    All,
    /// <summary>
    /// 结构
    /// </summary>
    Structure,
    /// <summary>
    /// 家具
    /// </summary>
    Furniture,
    /// <summary>
    /// 设施
    /// </summary>
    Machine,
    /// <summary>
    /// 其他
    /// </summary>
    Other,
    /// <summary>
    /// 地面
    /// </summary>
    Ground,
}
/// <summary>
/// 时代
/// </summary>
public enum AgeGroup
{
    /// <summary>
    /// 自然造物
    /// </summary>
    Nature,
    /// <summary>
    /// 非自然造物
    /// </summary>
    UnNature,
    /// <summary>
    /// 石器时代
    /// </summary>
    StoneAge,
    /// <summary>
    /// 铁器时代
    /// </summary>
    IronAge,
    /// <summary>
    /// 工业时代
    /// </summary>
    IndustrialAge_0,
    /// <summary>
    /// 后工业时代
    /// </summary>
    IndustrialAge_1,
    /// <summary>
    /// 魔法时代
    /// </summary>
    MagicSAge_0,
    /// <summary>
    /// 超魔法时代
    /// </summary>
    MagicSAge_1,
}