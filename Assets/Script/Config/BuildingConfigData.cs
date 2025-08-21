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
        /*空*/new BuildingConfig()
        {
            Building_ID = 0,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*占位*/new BuildingConfig()
        {
            Building_ID = 99,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        #region//自然建筑
        /*森林青树*/new BuildingConfig()
        {
            Building_ID = 1000,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*森林长草丛*/new BuildingConfig()
        {
            Building_ID = 1001,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*石墙*/new BuildingConfig()
        {
            Building_ID = 1002,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*森林伞树*/new BuildingConfig()
        {
            Building_ID = 1003,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*空心树桩*/new BuildingConfig()
        {
            Building_ID = 1004,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*薯果植株*/new BuildingConfig()
        {
            Building_ID = 1005,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*辣椒植株*/new BuildingConfig()
        {
            Building_ID = 1006,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*柑橘灌木*/new BuildingConfig()
        {
            Building_ID = 1007,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*荆棘*/new BuildingConfig()
        {
            Building_ID = 1008,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*大块岩石*/new BuildingConfig()
        {
            Building_ID = 1009,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*花丛*/new BuildingConfig()
        {
            Building_ID = 1010,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*煤矿墙*/new BuildingConfig()
        {
            Building_ID = 1011,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*铁矿墙*/new BuildingConfig()
        {
            Building_ID = 1012,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*金矿墙*/new BuildingConfig()
        {
            Building_ID = 1013,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*水晶墙*/new BuildingConfig()
        {
            Building_ID = 1014,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*翡翠墙*/new BuildingConfig()
        {
            Building_ID = 1015,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*硝石墙*/new BuildingConfig()
        {
            Building_ID = 1016,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*宝石墙*/new BuildingConfig()
        {
            Building_ID = 1017,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*雪原树*/new BuildingConfig()
        {
            Building_ID = 1018,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*沙漠面包树*/new BuildingConfig()
        {
            Building_ID = 1019,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.Nature,
            Building_Raw = new List<ItemRaw>(){},
        },
        #endregion
        #region//非自然建筑
        /*太阳祭坛*/new BuildingConfig()
        {
            Building_ID = 2000,Building_Size = AreaSize._3X3,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*兔子生成*/new BuildingConfig()
        {
            Building_ID = 2001,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*僵尸生成*/new BuildingConfig()
        {
            Building_ID = 2002,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*猎人生成*/new BuildingConfig()
        {
            Building_ID = 2003,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*铁钩僵尸生成*/new BuildingConfig()
        {
            Building_ID = 2004,Building_Size = AreaSize._3X3,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*护卫生成*/new BuildingConfig()
        {
            Building_ID = 2005,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*夜班土匪生成*/new BuildingConfig()
        {
            Building_ID = 2006,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*白班土匪生成*/new BuildingConfig()
        {
            Building_ID = 2007,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*农民生成*/new BuildingConfig()
        {
            Building_ID = 2008,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*矿物商人生成*/new BuildingConfig()
        {
            Building_ID = 2009,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*伐木工生成*/new BuildingConfig()
        {
            Building_ID = 2010,Building_Size = AreaSize._2X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*警卫生成*/new BuildingConfig()
        {
            Building_ID = 2011,Building_Size = AreaSize._2X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*厨师生成*/new BuildingConfig()
        {
            Building_ID = 2012,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*镇长生成*/new BuildingConfig()
        {
            Building_ID = 2013,Building_Size = AreaSize._2X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*酒保生成*/new BuildingConfig()
        {
            Building_ID = 2014,Building_Size = AreaSize._2X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*富豪生成*/new BuildingConfig()
        {
            Building_ID = 2015,Building_Size = AreaSize._2X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*石头灯柱*/new BuildingConfig()
        {
            Building_ID = 2100,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*岗哨*/new BuildingConfig()
        {
            Building_ID = 2101,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*柜台*/new BuildingConfig()
        {
            Building_ID = 2102,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*木桶*/new BuildingConfig()
        {
            Building_ID = 2103,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        #endregion
        #region//石器时代建筑
        /*木墙*/new BuildingConfig()
        {
            Building_ID = 3000,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Structure,Building_Age = AgeGroup.StoneAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1100,2) },
        },
        /*木门*/new BuildingConfig()
        {
            Building_ID = 3001,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Structure,Building_Age = AgeGroup.StoneAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1100,2) },
        },
        /*木栅栏*/new BuildingConfig()
        {
            Building_ID = 3002,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Structure,Building_Age = AgeGroup.StoneAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1100,2) },
        },
        /*木加工台*/new BuildingConfig()
        {
            Building_ID = 3003,Building_Size = AreaSize._2X1,Building_Type = BuildingType.Machine,Building_Age = AgeGroup.StoneAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1000,10) },
        },
        /*木箱子*/new BuildingConfig()
        {
            Building_ID = 3004,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Furniture,Building_Age = AgeGroup.StoneAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1100,4) },
        },
        /*石头熔炉*/new BuildingConfig()
        {
            Building_ID = 3005,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Machine,Building_Age = AgeGroup.StoneAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1100,4),new ItemRaw(1010,4) },
        },
        /*石头灯箱*/new BuildingConfig()
        {
            Building_ID = 3006,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Machine,Building_Age = AgeGroup.StoneAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1100,4),new ItemRaw(1010,4) },
        },
        /*木头桌子*/new BuildingConfig()
        {
            Building_ID = 3007,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Machine,Building_Age = AgeGroup.StoneAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1100,4),new ItemRaw(1010,4) },
        },
        /*木床*/new BuildingConfig()
        {
            Building_ID = 3008,Building_Size = AreaSize._2X1,Building_Type = BuildingType.Machine,Building_Age = AgeGroup.StoneAge,
            Building_Group = new List<short>{ 30080,30081,30082 },
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1100,4),new ItemRaw(1010,4) },
        },
        /*木椅*/new BuildingConfig()
        {
            Building_ID = 3009,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Machine,Building_Age = AgeGroup.StoneAge,
            Building_Group = new List<short>{ 30090, 30091, 30092, 30093 },
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1100,4),new ItemRaw(1010,4) },
        },
        /*木制花盆*/new BuildingConfig()
        {
            Building_ID = 3010,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Machine,Building_Age = AgeGroup.StoneAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1100,4),new ItemRaw(1010,4) },
        },
        #endregion
        #region//铁器时代建筑
        /*铁制加工台*/new BuildingConfig()
        {
            Building_ID = 4000,Building_Size = AreaSize._2X1,Building_Type = BuildingType.Machine,Building_Age = AgeGroup.IronAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1110,6) },
        },
        /*枪械加工台*/new BuildingConfig()
        {
            Building_ID = 4001,Building_Size = AreaSize._2X1,Building_Type = BuildingType.Machine,Building_Age = AgeGroup.IronAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1110,6) },
        },
        /*粉碎机*/new BuildingConfig()
        {
            Building_ID = 4002,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Machine,Building_Age = AgeGroup.IronAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1100,4),new ItemRaw(1010,4) },
        },
        /*烹饪台*/new BuildingConfig()
        {
            Building_ID = 4003,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Machine,Building_Age = AgeGroup.IronAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1100,4),new ItemRaw(1010,4) },
        },
        /*砖墙*/new BuildingConfig()
        {
            Building_ID = 4004,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Structure,Building_Age = AgeGroup.IronAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1010,4) },
        },
        /*冰箱*/new BuildingConfig()
        {
            Building_ID = 4005,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Structure,Building_Age = AgeGroup.IronAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1110, 4) },
        },
        /*饮料机*/new BuildingConfig()
        {
            Building_ID = 4006,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Structure,Building_Age = AgeGroup.IronAge,
            Building_Raw = new List<ItemRaw>(){ new ItemRaw(1110, 4) },
        },
        #endregion
        #region//衍生建筑
        /*左木床*/new BuildingConfig()
        {
            Building_ID = 30080,Building_Size = AreaSize._2X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*右木床*/new BuildingConfig()
        {
            Building_ID = 30081,Building_Size = AreaSize._2X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*上木床*/new BuildingConfig()
        {
            Building_ID = 30082,Building_Size = AreaSize._1X2,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*下木椅*/new BuildingConfig()
        {
            Building_ID = 30090,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*左木椅*/new BuildingConfig()
        {
            Building_ID = 30091,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*右木椅*/new BuildingConfig()
        {
            Building_ID = 30092,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },
        /*上木椅*/new BuildingConfig()
        {
            Building_ID = 30093,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Other,Building_Age = AgeGroup.UnNature,
            Building_Raw = new List<ItemRaw>(){},
        },

        #endregion
    };
}
[Serializable]
public struct BuildingConfig
{
    [SerializeField]/*编号*/
    public short Building_ID;
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
    /// 魔法时代
    /// </summary>
    MagicAge_0,
    /// <summary>
    /// 后工业时代
    /// </summary>
    IndustrialAge_1,
    /// <summary>
    /// 超魔法时代
    /// </summary>
    MagicAge_1,
}