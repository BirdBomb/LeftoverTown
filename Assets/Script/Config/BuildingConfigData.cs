using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingConfigData : MonoBehaviour
{
    public static BuildingConfig GetItemConfig(int ID)
    {
        return buildConfigs.Find((x) => { return x.Building_ID == ID; });
    }
    public readonly static List<BuildingConfig> buildConfigs = new List<BuildingConfig>()
    {
        new BuildingConfig(){ Building_ID = 0,Building_Name ="空",
            Building_FileName ="Default",Building_SpriteName="Default",Building_RawLevel = 10,
            Building_Raw = new List<BuildingRaw>(){ } },
        new BuildingConfig(){ Building_ID = 1,Building_Name ="建筑建造块",
            Building_FileName ="BuildingBuilder",Building_SpriteName="BuildingBuilder",Building_RawLevel = 10,
            Building_Raw = new List<BuildingRaw>(){ } },
        new BuildingConfig(){ Building_ID = 2,Building_Name ="地板建造块",
            Building_FileName ="FloorBuilder",Building_SpriteName="FloorBuilder",Building_RawLevel = 10,
            Building_Raw = new List<BuildingRaw>(){ } },

        new BuildingConfig(){ Building_ID = 1001,Building_Name ="简易木墙",
            Building_FileName ="WoodWall",Building_SpriteName="WoodWall",Building_RawLevel = 1,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1002,Building_Name ="简易柜子",
            Building_FileName ="CabinetSmall",Building_SpriteName="CabinetSmall",Building_RawLevel = 1,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1003,Building_Name ="鱼缸",
            Building_FileName ="FishTank",Building_SpriteName="FishTank",Building_RawLevel = 1,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1004,Building_Name ="简易木门",
            Building_FileName ="WoodDoor",Building_SpriteName="WoodDoor",Building_RawLevel = 1,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1005,Building_Name ="木床组件",
            Building_FileName ="WoodBed_Part",Building_SpriteName="WoodBed_Part",Building_RawLevel = 1,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1006,Building_Name ="路灯",
            Building_FileName ="Lamp",Building_SpriteName="Lamp",Building_RawLevel = 5,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) }},
        new BuildingConfig(){ Building_ID = 1007,Building_Name ="篝火",
            Building_FileName ="Bonfire",Building_SpriteName="Bonfire",Building_RawLevel = 0,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1008,Building_Name ="巨大告示牌",
            Building_FileName ="Billboard",Building_SpriteName="Billboard",Building_RawLevel = 1,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1009,Building_Name ="骸骨",
            Building_FileName ="Bone",Building_SpriteName="Bone",Building_RawLevel = 1,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1010,Building_Name ="小型冰箱",
            Building_FileName ="Refrigerator",Building_SpriteName="Refrigerator",Building_RawLevel = 2,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1011,Building_Name ="石头篝火",
            Building_FileName ="BonfireStone",Building_SpriteName="BonfireStone",Building_RawLevel = 0,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1012,Building_Name ="厨灶",
            Building_FileName ="Stove",Building_SpriteName="Stove",Building_RawLevel = 2,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1013,Building_Name ="木制工具台",
            Building_FileName ="WoodWorkingBench",Building_SpriteName="WoodWorkingBench",Building_RawLevel = 2,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1014,Building_Name ="柜台",
            Building_FileName ="Desk",Building_SpriteName="Desk",Building_RawLevel = 2,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1015,Building_Name ="收银柜台",
            Building_FileName ="CashDesk",Building_SpriteName="CashDesk",Building_RawLevel = 2,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1016,Building_Name ="木桶",
            Building_FileName ="WoodVat",Building_SpriteName="WoodVat",Building_RawLevel = 4,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1017,Building_Name ="墙灯",
            Building_FileName ="WallLight",Building_SpriteName="WallLight",Building_RawLevel = 4,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1018,Building_Name ="木制货架",
            Building_FileName ="GoodsShelf_Wooden",Building_SpriteName="GoodsShelf_Wooden",Building_RawLevel = 4,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1019,Building_Name ="画",
            Building_FileName ="Painting",Building_SpriteName="Painting",Building_RawLevel = 4,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1020,Building_Name ="木制栅栏",
            Building_FileName ="WoodFence",Building_SpriteName="WoodFence",Building_RawLevel = 4,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1021,Building_Name ="木桌子",
            Building_FileName ="WoodTable",Building_SpriteName="WoodTable",Building_RawLevel = 4,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1022,Building_Name ="椅子",
            Building_FileName ="Chair",Building_SpriteName="Chair",Building_RawLevel = 4,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1023,Building_Name ="岗哨",
            Building_FileName ="SentryStation",Building_SpriteName="SentryStation",Building_RawLevel = 4,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1024,Building_Name ="支撑柱",
            Building_FileName ="WoodShore",Building_SpriteName="WoodShore",Building_RawLevel = 2,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1025,Building_Name ="砖墙",
            Building_FileName ="BrickWall",Building_SpriteName="BrickWall",Building_RawLevel = 2,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1026,Building_Name ="钢制货架",
            Building_FileName ="GoodsShelf_Iron",Building_SpriteName="GoodsShelf_Iron",Building_RawLevel = 2,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1027,Building_Name ="木制宝箱",
            Building_FileName ="WoodenBox",Building_SpriteName="WoodenBox",Building_RawLevel = 2,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1028,Building_Name ="铁门",
            Building_FileName ="IronDoor",Building_SpriteName="IronDoor",Building_RawLevel = 2,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1029,Building_Name ="木床",
            Building_FileName ="WoodBed",Building_SpriteName="WoodBed",Building_RawLevel = 2,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1030,Building_Name ="耕地",
            Building_FileName ="Plowland",Building_SpriteName="Plowland",Building_RawLevel = 10,
            Building_Raw = new List<BuildingRaw>(){ } },

        new BuildingConfig(){ Building_ID = 2001,Building_Name ="树",
            Building_FileName ="Tree",Building_SpriteName="Tree",Building_RawLevel = 10,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 2002,Building_Name ="果树",
            Building_FileName ="TreeFruit",Building_SpriteName="TreeFruit",Building_RawLevel = 10,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 2003,Building_Name ="矿石",
            Building_FileName ="Rock",Building_SpriteName="Rock",Building_RawLevel = 10,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 2004,Building_Name ="灌木",
            Building_FileName ="Bush",Building_SpriteName="Bush",Building_RawLevel = 10,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },

        new BuildingConfig(){ Building_ID = 3001,Building_Name ="工具店招牌",
            Building_FileName ="ShopSign_Tool",Building_SpriteName="ShopSign_Tool",Building_RawLevel = 10,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
    };
}
[Serializable]
public struct BuildingConfig
{
    [SerializeField]/*编号*/
    public int Building_ID;
    [SerializeField]/*名字*/
    public string Building_Name;
    [SerializeField]/*资源名*/
    public string Building_FileName;
    [SerializeField]/*图片名*/
    public string Building_SpriteName;
    [SerializeField]/*合成等级*/
    public int Building_RawLevel;
    /*建筑原料*/
    public List<BuildingRaw>Building_Raw;
}
/// <summary>
/// 建筑原料
/// </summary>
public struct BuildingRaw
{
    public short ID;
    public short Count;
    public BuildingRaw(short id, short count)
    {
        ID = id;
        Count = count;
    }
}