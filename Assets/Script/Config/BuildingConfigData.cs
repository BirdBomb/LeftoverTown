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
    /*1000-1999构造*/
    /*2000-2999家具*/
    /*3000-3999机械*/
    /*4000-4999*/
    /*8000-8999人为地块*/
    /*9000-9999自然地块*/
    public readonly static  List<BuildingConfig> buildConfigs = new List<BuildingConfig>()
    {
        #region//(类别)工程建筑
        new BuildingConfig(){ Building_ID = 0,Building_Name ="空",
            Building_Armor = 10,Building_RawLevel = 10,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Lock,
            Building_Raw = new List<RawItem>(){ } },
        new BuildingConfig(){ Building_ID = 99,Building_Name ="建筑占位",
           Building_Armor = 10,Building_RawLevel = 0,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Lock,
            Building_Raw = new List<RawItem>(){ } },
        #endregion
        #region//(类别)结构体
        new BuildingConfig(){ Building_ID = 1000,Building_Name ="木墙",
            Building_Armor = 10,Building_RawLevel = 1,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Structure,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        new BuildingConfig(){ Building_ID = 1001,Building_Name ="木门",
           Building_Armor = 10,Building_RawLevel = 1,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Structure,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        new BuildingConfig(){ Building_ID = 1002,Building_Name ="木栅栏",
            Building_Armor = 10,Building_RawLevel = 4,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Structure,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        new BuildingConfig(){ Building_ID = 1003,Building_Name ="木柱",
            Building_Armor = 10,Building_RawLevel = 2,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Structure,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        new BuildingConfig(){ Building_ID = 1010,Building_Name ="砖墙",
            Building_Armor = 10,Building_RawLevel = 2,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Structure,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        new BuildingConfig(){ Building_ID = 1021,Building_Name ="铁门",
            Building_Armor = 10,Building_RawLevel = 2,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Structure,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        #endregion
        #region//(类别)家具
        #region//容器
        new BuildingConfig(){ Building_ID = 2000,Building_Name ="木床",
            Building_Armor = 10,Building_RawLevel = 1,Building_Size = AreaSize._1X2,Building_Type = BuildingType.Furniture,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) },Building_Group = new List<short>(){ 20000,20001,20002 } },
        #region//木床
        new BuildingConfig(){ Building_ID = 20000,Building_Name ="木床(下)",
            Building_Armor = 10,Building_RawLevel = 1,Building_Size = AreaSize._1X2,Building_Type = BuildingType.Furniture,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        new BuildingConfig(){ Building_ID = 20001,Building_Name ="木床(右)",
            Building_Armor = 10,Building_RawLevel = 1,Building_Size = AreaSize._2X1,Building_Type = BuildingType.Furniture,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        new BuildingConfig(){ Building_ID = 20002,Building_Name ="木床(左)",
            Building_Armor = 10,Building_RawLevel = 1,Building_Size = AreaSize._2X1,Building_Type = BuildingType.Furniture,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        #endregion
        new BuildingConfig(){ Building_ID = 2010,Building_Name ="木柜子(短)",
            Building_Armor = 10,Building_RawLevel = 1,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Furniture,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        new BuildingConfig(){ Building_ID = 2011,Building_Name ="木柜子(长)",
            Building_Armor = 10,Building_RawLevel = 1,Building_Size = AreaSize._2X1,Building_Type = BuildingType.Furniture,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        new BuildingConfig(){ Building_ID = 2012,Building_Name ="木箱子",
            Building_Armor = 10,Building_RawLevel = 1,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Furniture,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        new BuildingConfig(){ Building_ID = 2013,Building_Name ="木板条箱",
            Building_Armor = 10,Building_RawLevel = 1,Building_Size = AreaSize._2X2,Building_Type = BuildingType.Furniture,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        new BuildingConfig(){ Building_ID = 2014,Building_Name ="木衣柜",
            Building_Armor = 10,Building_RawLevel = 1,Building_Size = AreaSize._2X1,Building_Type = BuildingType.Furniture,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        new BuildingConfig(){ Building_ID = 2015,Building_Name ="木桶",
            Building_Armor = 10,Building_RawLevel = 4,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Furniture,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        new BuildingConfig(){ Building_ID = 2016,Building_Name ="铁架",
            Building_Armor = 10,Building_RawLevel = 4,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Furniture,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        #endregion
        #region//观赏
        new BuildingConfig(){ Building_ID = 2100,Building_Name ="鱼缸",
            Building_Armor = 10,Building_RawLevel = 1,Building_Size = AreaSize._2X1,Building_Type = BuildingType.Furniture,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        new BuildingConfig(){ Building_ID = 2101,Building_Name ="木桌子",
            Building_Armor = 10,Building_RawLevel = 4,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Furniture,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        new BuildingConfig(){ Building_ID = 2102,Building_Name ="木椅",
            Building_Armor = 10,Building_RawLevel = 4,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Furniture,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5),new RawItem(1001, 5) },Building_Group = new List<short>(){ 21020, 21021, 21022, 21023 } },
        #region//木椅
        new BuildingConfig(){ Building_ID = 21020,Building_Name ="木椅(下)",
            Building_Armor = 10,Building_RawLevel = 4,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Furniture,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        new BuildingConfig(){ Building_ID = 21021,Building_Name ="木椅(上)",
            Building_Armor = 10,Building_RawLevel = 4,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Furniture,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        new BuildingConfig(){ Building_ID = 21022,Building_Name ="木椅(左)",
            Building_Armor = 10,Building_RawLevel = 4,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Furniture,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        new BuildingConfig(){ Building_ID = 21023,Building_Name ="木椅(右)",
            Building_Armor = 10,Building_RawLevel = 4,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Furniture,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        #endregion
        #endregion
        #region//功能
        new BuildingConfig(){ Building_ID = 2200,Building_Name ="篝火",
            Building_Armor = 10,Building_RawLevel = 0,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Furniture,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        new BuildingConfig(){ Building_ID = 2201,Building_Name ="石头篝火",
            Building_Armor = 10,Building_RawLevel = 0,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Furniture,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        new BuildingConfig(){ Building_ID = 2210,Building_Name ="石头厨灶",
            Building_Armor = 10,Building_RawLevel = 0,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Furniture,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },

        #endregion
        #region//科技
        new BuildingConfig(){ Building_ID = 2300,Building_Name ="工具台",
            Building_Armor = 10,Building_RawLevel = 0,Building_Size = AreaSize._2X1,Building_Type = BuildingType.Furniture,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        #endregion
        #endregion

        #region//(类别)机械
        #region//照明
        new BuildingConfig(){ Building_ID = 3000,Building_Name ="落地灯",
            Building_Armor = 10,Building_RawLevel = 5,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Machine,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        new BuildingConfig(){ Building_ID = 3001,Building_Name ="路灯",
           Building_Armor = 10,Building_RawLevel = 5,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Machine,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        #endregion
        #region//功能
        new BuildingConfig(){ Building_ID = 3100,Building_Name ="冰箱",
            Building_Armor = 10,Building_RawLevel = 2,Building_Size = AreaSize._1X1,Building_Type = BuildingType.Machine,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        new BuildingConfig(){ Building_ID = 3101,Building_Name ="冰柜",
            Building_Armor = 10,Building_RawLevel = 2,Building_Size = AreaSize._2X1,Building_Type = BuildingType.Machine,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        #endregion
        #endregion

        #region//(类别)角色关联地块
        #region//玩家
        new BuildingConfig(){ Building_ID = 8000,Building_Name ="坠落太阳祭坛",
            Building_Armor = 9999,Building_RawLevel = 1,Building_Size = AreaSize._3X3,Building_Type =BuildingType.Lock,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        #endregion
        #region//动物
        new BuildingConfig(){ Building_ID = 8100,Building_Name ="兔子洞",
            Building_Armor = 10,Building_RawLevel = 1,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Lock,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        #endregion
        #region//怪物
        #endregion
        #region//NPC
        new BuildingConfig(){ Building_ID = 8300,Building_Name ="护卫岗哨",
            Building_Armor = 10,Building_RawLevel = 1,Building_Size = AreaSize._1X1,Building_Type =BuildingType.Lock,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        new BuildingConfig(){ Building_ID = 8301,Building_Name ="猎人工作台",
            Building_Armor = 10,Building_RawLevel = 1,Building_Size = AreaSize._2X1,Building_Type =BuildingType.Lock,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        #endregion
        #region//Boss
        #endregion
        #endregion
        #region//(类别)自然地块
        #region//植物
        new BuildingConfig(){ Building_ID = 9000,Building_Name ="长青树(普通)", 
            Building_Armor = 0,Building_RawLevel = 10,Building_Size = AreaSize._1X1,Building_Type= BuildingType.Lock,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        new BuildingConfig(){ Building_ID = 9001,Building_Name ="长青树(结果)", 
            Building_Armor = 0,Building_RawLevel = 10,Building_Size = AreaSize._1X1,Building_Type= BuildingType.Lock,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        new BuildingConfig(){ Building_ID = 9010,Building_Name ="绿灌木", 
           Building_Armor = 0,Building_RawLevel = 10,Building_Size = AreaSize._1X1,Building_Type= BuildingType.Lock,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        new BuildingConfig(){ Building_ID = 9020,Building_Name ="马铃薯植株",
            Building_Armor = 0,Building_RawLevel = 10,Building_Size = AreaSize._1X1,Building_Type= BuildingType.Lock,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        #endregion
        #region//矿物
        new BuildingConfig(){ Building_ID = 9100,Building_Name ="石头",
            Building_Armor = 10,Building_RawLevel = 10,Building_Size = AreaSize._1X1,Building_Type= BuildingType.Lock,
            Building_Raw = new List<RawItem>(){ new RawItem(1000, 4),new RawItem(1001, 5) } },
        #endregion
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
    [SerializeField]/*资源名*/
    public string Building_FileName;
    [SerializeField]/*建筑硬度*/
    public short Building_Armor;
    [SerializeField]/*建筑等级*/
    public short Building_RawLevel;
    [SerializeField]/*建筑尺寸*/
    public AreaSize Building_Size;
    [SerializeField]/*建筑类别*/
    public BuildingType Building_Type;
    [SerializeField]/*建筑原料*/
    public List<RawItem>Building_Raw;
    [SerializeField]/*建筑群*/
    public List<short> Building_Group;
}
/// <summary>
/// 建筑原料
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
    /// 不能建造
    /// </summary>
    Lock,
    /// <summary>
    /// 结构
    /// </summary>
    Structure,
    /// <summary>
    /// 家具
    /// </summary>
    Furniture,
    /// <summary>
    /// 机械
    /// </summary>
    Machine,
}