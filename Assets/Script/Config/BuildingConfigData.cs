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
        new BuildingConfig(){ Building_ID = 0,Building_Name ="��",
            Building_FileName ="Default",Building_SpriteName="Default",Building_RawLevel = 10,
            Building_Raw = new List<BuildingRaw>(){ } },
        new BuildingConfig(){ Building_ID = 1,Building_Name ="���������",
            Building_FileName ="BuildingBuilder",Building_SpriteName="BuildingBuilder",Building_RawLevel = 10,
            Building_Raw = new List<BuildingRaw>(){ } },
        new BuildingConfig(){ Building_ID = 2,Building_Name ="�ذ彨���",
            Building_FileName ="FloorBuilder",Building_SpriteName="FloorBuilder",Building_RawLevel = 10,
            Building_Raw = new List<BuildingRaw>(){ } },

        new BuildingConfig(){ Building_ID = 1001,Building_Name ="����ľǽ",
            Building_FileName ="WoodWall",Building_SpriteName="WoodWall",Building_RawLevel = 1,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1002,Building_Name ="���׹���",
            Building_FileName ="CabinetSmall",Building_SpriteName="CabinetSmall",Building_RawLevel = 1,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1003,Building_Name ="���",
            Building_FileName ="FishTank",Building_SpriteName="FishTank",Building_RawLevel = 1,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1004,Building_Name ="����ľ��",
            Building_FileName ="WoodDoor",Building_SpriteName="WoodDoor",Building_RawLevel = 1,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1005,Building_Name ="ľ�����",
            Building_FileName ="WoodBed_Part",Building_SpriteName="WoodBed_Part",Building_RawLevel = 1,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1006,Building_Name ="·��",
            Building_FileName ="Lamp",Building_SpriteName="Lamp",Building_RawLevel = 5,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) }},
        new BuildingConfig(){ Building_ID = 1007,Building_Name ="����",
            Building_FileName ="Bonfire",Building_SpriteName="Bonfire",Building_RawLevel = 0,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1008,Building_Name ="�޴��ʾ��",
            Building_FileName ="Billboard",Building_SpriteName="Billboard",Building_RawLevel = 1,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1009,Building_Name ="����",
            Building_FileName ="Bone",Building_SpriteName="Bone",Building_RawLevel = 1,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1010,Building_Name ="С�ͱ���",
            Building_FileName ="Refrigerator",Building_SpriteName="Refrigerator",Building_RawLevel = 2,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1011,Building_Name ="ʯͷ����",
            Building_FileName ="BonfireStone",Building_SpriteName="BonfireStone",Building_RawLevel = 0,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1012,Building_Name ="����",
            Building_FileName ="Stove",Building_SpriteName="Stove",Building_RawLevel = 2,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1013,Building_Name ="ľ�ƹ���̨",
            Building_FileName ="WoodWorkingBench",Building_SpriteName="WoodWorkingBench",Building_RawLevel = 2,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1014,Building_Name ="��̨",
            Building_FileName ="Desk",Building_SpriteName="Desk",Building_RawLevel = 2,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1015,Building_Name ="������̨",
            Building_FileName ="CashDesk",Building_SpriteName="CashDesk",Building_RawLevel = 2,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1016,Building_Name ="ľͰ",
            Building_FileName ="WoodVat",Building_SpriteName="WoodVat",Building_RawLevel = 4,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1017,Building_Name ="ǽ��",
            Building_FileName ="WallLight",Building_SpriteName="WallLight",Building_RawLevel = 4,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1018,Building_Name ="ľ�ƻ���",
            Building_FileName ="GoodsShelf_Wooden",Building_SpriteName="GoodsShelf_Wooden",Building_RawLevel = 4,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1019,Building_Name ="��",
            Building_FileName ="Painting",Building_SpriteName="Painting",Building_RawLevel = 4,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1020,Building_Name ="ľ��դ��",
            Building_FileName ="WoodFence",Building_SpriteName="WoodFence",Building_RawLevel = 4,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1021,Building_Name ="ľ����",
            Building_FileName ="WoodTable",Building_SpriteName="WoodTable",Building_RawLevel = 4,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1022,Building_Name ="����",
            Building_FileName ="Chair",Building_SpriteName="Chair",Building_RawLevel = 4,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1023,Building_Name ="����",
            Building_FileName ="SentryStation",Building_SpriteName="SentryStation",Building_RawLevel = 4,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1024,Building_Name ="֧����",
            Building_FileName ="WoodShore",Building_SpriteName="WoodShore",Building_RawLevel = 2,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1025,Building_Name ="שǽ",
            Building_FileName ="BrickWall",Building_SpriteName="BrickWall",Building_RawLevel = 2,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1026,Building_Name ="���ƻ���",
            Building_FileName ="GoodsShelf_Iron",Building_SpriteName="GoodsShelf_Iron",Building_RawLevel = 2,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1027,Building_Name ="ľ�Ʊ���",
            Building_FileName ="WoodenBox",Building_SpriteName="WoodenBox",Building_RawLevel = 2,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1028,Building_Name ="����",
            Building_FileName ="IronDoor",Building_SpriteName="IronDoor",Building_RawLevel = 2,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1029,Building_Name ="ľ��",
            Building_FileName ="WoodBed",Building_SpriteName="WoodBed",Building_RawLevel = 2,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 1030,Building_Name ="����",
            Building_FileName ="Plowland",Building_SpriteName="Plowland",Building_RawLevel = 10,
            Building_Raw = new List<BuildingRaw>(){ } },

        new BuildingConfig(){ Building_ID = 2001,Building_Name ="��",
            Building_FileName ="Tree",Building_SpriteName="Tree",Building_RawLevel = 10,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 2002,Building_Name ="����",
            Building_FileName ="TreeFruit",Building_SpriteName="TreeFruit",Building_RawLevel = 10,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 2003,Building_Name ="��ʯ",
            Building_FileName ="Rock",Building_SpriteName="Rock",Building_RawLevel = 10,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
        new BuildingConfig(){ Building_ID = 2004,Building_Name ="��ľ",
            Building_FileName ="Bush",Building_SpriteName="Bush",Building_RawLevel = 10,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },

        new BuildingConfig(){ Building_ID = 3001,Building_Name ="���ߵ�����",
            Building_FileName ="ShopSign_Tool",Building_SpriteName="ShopSign_Tool",Building_RawLevel = 10,
            Building_Raw = new List<BuildingRaw>(){ new BuildingRaw(1001,4),new BuildingRaw(1002,5) } },
    };
}
[Serializable]
public struct BuildingConfig
{
    [SerializeField]/*���*/
    public int Building_ID;
    [SerializeField]/*����*/
    public string Building_Name;
    [SerializeField]/*��Դ��*/
    public string Building_FileName;
    [SerializeField]/*ͼƬ��*/
    public string Building_SpriteName;
    [SerializeField]/*�ϳɵȼ�*/
    public int Building_RawLevel;
    /*����ԭ��*/
    public List<BuildingRaw>Building_Raw;
}
/// <summary>
/// ����ԭ��
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