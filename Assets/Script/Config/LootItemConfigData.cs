using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LootItemConfigData 
{
    public static LootRandomConfig GetLootRandomConfig(int ID)
    {
        return loorRandomConfigs.Find((x) => { return x.Loot_ID == ID; });
    }
    public readonly static List<LootRandomConfig> loorRandomConfigs = new List<LootRandomConfig>()
    {
        new LootRandomConfig(){Loot_ID = 11200,Loot_List = new LootRandomInfo[]
        {
            new LootRandomInfo(1011,1,2,50),
            new LootRandomInfo(1012,1,2,50),
            new LootRandomInfo(1013,1,2,50),
            new LootRandomInfo(1014,1,2,50),
            new LootRandomInfo(1015,1,2,50),
        }},
        new LootRandomConfig(){Loot_ID = 11201,Loot_List = new LootRandomInfo[]
        {
            new LootRandomInfo(1011,1,2,50),
            new LootRandomInfo(1012,1,2,50),
            new LootRandomInfo(1013,1,2,50),
            new LootRandomInfo(1014,1,2,50),
            new LootRandomInfo(1015,1,2,50),
        }},

        new LootRandomConfig(){Loot_ID = 12000,Loot_List = new LootRandomInfo[]
        {
            new LootRandomInfo(6100,1,2,50),
            new LootRandomInfo(6101,1,2,50),
            new LootRandomInfo(6102,1,2,50),
        }},
        new LootRandomConfig(){Loot_ID = 12001,Loot_List = new LootRandomInfo[]
        {
            new LootRandomInfo(6100,1,2,50),
            new LootRandomInfo(6101,1,2,50),
            new LootRandomInfo(6102,1,2,50),
        }},
        new LootRandomConfig(){Loot_ID = 1204,Loot_List = new LootRandomInfo[]
        {
            new LootRandomInfo(1011,1,2,50),
            new LootRandomInfo(1012,1,2,50),
            new LootRandomInfo(1013,1,2,50),
            new LootRandomInfo(1014,1,2,50),
            new LootRandomInfo(1015,1,2,50),
        }},

        new LootRandomConfig(){Loot_ID = 2101,Loot_List = new LootRandomInfo[]{new LootRandomInfo(3105,1,2,50),}},
        new LootRandomConfig(){Loot_ID = 2202,Loot_List = new LootRandomInfo[]
        {
            new LootRandomInfo(6100,2,5,50),
            new LootRandomInfo(6101,2,5,50),
            new LootRandomInfo(6102,2,5,50),
        }},
        new LootRandomConfig(){Loot_ID = 2204,Loot_List = new LootRandomInfo[]
        {
            new LootRandomInfo(1000,2,5,100),
            new LootRandomInfo(1001,2,5,50),
            new LootRandomInfo(1002,2,5,50),
            new LootRandomInfo(1004,2,5,50),
            new LootRandomInfo(1100,2,5,50),
        }},
        new LootRandomConfig(){Loot_ID = 2209,Loot_List = new LootRandomInfo[]
        {
            new LootRandomInfo(1115,1,3,50),
            new LootRandomInfo(9900,1,30,100),
            new LootRandomInfo(9902,1,30,200),
            new LootRandomInfo(9700,1,1,200),
        }},
        new LootRandomConfig(){Loot_ID = 2300,Loot_List = new LootRandomInfo[]
        {
            new LootRandomInfo(9010,1,30,50),
            new LootRandomInfo(9011,1,30,20),
            new LootRandomInfo(9901,1,30,200),
            new LootRandomInfo(2303,1,1,25),
            new LootRandomInfo(2304,1,1,25),
        }},
        new LootRandomConfig(){Loot_ID = 2401,Loot_List = new LootRandomInfo[]
        {
            new LootRandomInfo(1021,2,4,50),
            new LootRandomInfo(1022,2,4,50),
            new LootRandomInfo(1023,1,5,50),
        }},

        new LootRandomConfig(){Loot_ID = 2601,Loot_List = new LootRandomInfo[]
        {
            new LootRandomInfo(2000,1,1,50),
            new LootRandomInfo(2010,1,1,50),
            new LootRandomInfo(2011,1,1,50),
            new LootRandomInfo(2012,1,1,50),
            new LootRandomInfo(2020,1,1,50),
            new LootRandomInfo(2021,1,1,50),
            new LootRandomInfo(2030,1,1,50),
            new LootRandomInfo(2040,1,1,50),
            new LootRandomInfo(2050,1,1,50),
            new LootRandomInfo(2060,1,1,50),
        }},
        new LootRandomConfig(){Loot_ID = 2602,Loot_List = new LootRandomInfo[]
        {
            new LootRandomInfo(3100,1,1,50),
            new LootRandomInfo(3101,1,1,50),
            new LootRandomInfo(3102,1,1,50),
            new LootRandomInfo(3103,1,1,50),
            new LootRandomInfo(3105,1,1,50),
            new LootRandomInfo(3110,1,1,50),
        }},
        new LootRandomConfig(){Loot_ID = 2603,Loot_List = new LootRandomInfo[]
        {
            new LootRandomInfo(3200,1,1,50),
            new LootRandomInfo(3201,1,1,50),
        }},
        new LootRandomConfig(){Loot_ID = 2604,Loot_List = new LootRandomInfo[]
        {
            new LootRandomInfo(5700,1,1,50),
            new LootRandomInfo(5701,1,1,50),
            new LootRandomInfo(5702,1,1,50),
            new LootRandomInfo(5703,1,1,50),
            new LootRandomInfo(5704,1,1,50),
            new LootRandomInfo(5705,1,1,50),
            new LootRandomInfo(5706,1,1,50),
            new LootRandomInfo(5707,1,1,50),
            new LootRandomInfo(5708,1,1,50),
        }},
        new LootRandomConfig(){Loot_ID = 2605,Loot_List = new LootRandomInfo[]
        {
            new LootRandomInfo(9800,1,1,50),
            new LootRandomInfo(9801,1,1,50),
            new LootRandomInfo(9802,1,1,50),
            new LootRandomInfo(9803,1,1,50),
            new LootRandomInfo(9804,1,1,50),
        }},
        new LootRandomConfig(){Loot_ID = 2606,Loot_List = new LootRandomInfo[]
        {
            new LootRandomInfo(9701,1,1,50),
            new LootRandomInfo(9702,1,1,50),
            new LootRandomInfo(9703,1,1,50),
        }},

        new LootRandomConfig(){Loot_ID = 2904,Loot_List = new LootRandomInfo[]
        {
            new LootRandomInfo(1100,2,4,100),
            new LootRandomInfo(1200,2,4,10),
            new LootRandomInfo(1201,1,2,10),
            new LootRandomInfo(1202,1,2,10),
        }},
        new LootRandomConfig(){Loot_ID = 2906,Loot_List = new LootRandomInfo[]
        {
            new LootRandomInfo(1200,1,1,100),
            new LootRandomInfo(1201,1,1,10),
            new LootRandomInfo(1202,1,1,10),
        }},
        new LootRandomConfig(){Loot_ID = 2907,Loot_List = new LootRandomInfo[]
        {
            new LootRandomInfo(1016,1,1,100),
        }},
        new LootRandomConfig(){Loot_ID = 2908,Loot_List = new LootRandomInfo[]
            {
                new LootRandomInfo(2011,1,1,100),
                new LootRandomInfo(2021,1,1,100),
                new LootRandomInfo(2030,1,1,100),
                new LootRandomInfo(2040,1,1,100),
                new LootRandomInfo(2050,1,1,100),
                new LootRandomInfo(2060,1,1,100),
                new LootRandomInfo(9701,1,1,50),
                new LootRandomInfo(9702,1,1,50),
                new LootRandomInfo(9703,1,1,50),
                new LootRandomInfo(9800,1,1,50),
                new LootRandomInfo(9801,1,1,50),
                new LootRandomInfo(9802,1,1,50),
                new LootRandomInfo(9803,1,1,50),
                new LootRandomInfo(9804,1,1,50),
                new LootRandomInfo(9900,1,15,900),
                new LootRandomInfo(9911,1,15,900),
            }},
        new LootRandomConfig(){Loot_ID = 4202,Loot_List = new LootRandomInfo[]{new LootRandomInfo(4200,1,1,100),}},
        new LootRandomConfig(){Loot_ID = 5204,Loot_List = new LootRandomInfo[]
        {
            new LootRandomInfo(1010,1,1,200),
            new LootRandomInfo(1011,1,1,100),
            new LootRandomInfo(1012,1,1,100),
            new LootRandomInfo(1013,1,1,50),
            new LootRandomInfo(1014,1,1,50),
            new LootRandomInfo(1015,1,1,20),
        }},
    };
    public static LootFixedConfig GetLootFixedConfig(int ID)
    {
        return loorFixedConfigs.Find((x) => { return x.Loot_ID == ID; });
    }
    public readonly static List<LootFixedConfig> loorFixedConfigs = new List<LootFixedConfig>()
    {
        //new LootFixedConfig(){Loot_ID = 10000,Loot_List = new LootFixedInfo[]{new LootFixedInfo(1000,1,1),}},

        new LootFixedConfig(){Loot_ID = 10000,Loot_List = new LootFixedInfo[]{new LootFixedInfo(1000,1,1),}},
        new LootFixedConfig(){Loot_ID = 10001,Loot_List = new LootFixedInfo[]{new LootFixedInfo(1000,3,4),new LootFixedInfo(1001,1,1),}},
        new LootFixedConfig(){Loot_ID = 10002,Loot_List = new LootFixedInfo[]{new LootFixedInfo(1000,3,4),new LootFixedInfo(1001,1,1),new LootFixedInfo(3000,1,1),}},

        new LootFixedConfig(){Loot_ID = 10010,Loot_List = new LootFixedInfo[]{new LootFixedInfo(1000,1,1),}},
        new LootFixedConfig(){Loot_ID = 10011,Loot_List = new LootFixedInfo[]{new LootFixedInfo(1000,3,4),new LootFixedInfo(1001,1,1),}},

        new LootFixedConfig(){Loot_ID = 10020,Loot_List = new LootFixedInfo[]{new LootFixedInfo(1000,1,1),}},
        new LootFixedConfig(){Loot_ID = 10021,Loot_List = new LootFixedInfo[]{new LootFixedInfo(1000,3,4),new LootFixedInfo(1001,1,1),}},
        new LootFixedConfig(){Loot_ID = 10022,Loot_List = new LootFixedInfo[]{new LootFixedInfo(1000,3,4),new LootFixedInfo(1001,1,1),new LootFixedInfo(3006,1,1),}},

        new LootFixedConfig(){Loot_ID = 10030,Loot_List = new LootFixedInfo[]{new LootFixedInfo(1000,1,1),}},
        new LootFixedConfig(){Loot_ID = 10031,Loot_List = new LootFixedInfo[]{new LootFixedInfo(1000,3,4),new LootFixedInfo(1001,1,1),}},

        new LootFixedConfig(){Loot_ID = 10040,Loot_List = new LootFixedInfo[]{new LootFixedInfo(1001,1,1),}},
        new LootFixedConfig(){Loot_ID = 10041,Loot_List = new LootFixedInfo[]{new LootFixedInfo(1001,3,4),}},

        new LootFixedConfig(){Loot_ID = 1110,Loot_List = new LootFixedInfo[]{new LootFixedInfo(1010,4,5),}},
        new LootFixedConfig(){Loot_ID = 1111,Loot_List = new LootFixedInfo[]{new LootFixedInfo(1010,1,1),new LootFixedInfo(1011,3,4),}},
        new LootFixedConfig(){Loot_ID = 1112,Loot_List = new LootFixedInfo[]{new LootFixedInfo(1010,1,1),new LootFixedInfo(1012,3,4),}},
        new LootFixedConfig(){Loot_ID = 1113,Loot_List = new LootFixedInfo[]{new LootFixedInfo(1010,1,1),new LootFixedInfo(1013,3,4),}},
        new LootFixedConfig(){Loot_ID = 1114,Loot_List = new LootFixedInfo[]{new LootFixedInfo(1010,1,1),new LootFixedInfo(1014,3,4),}},
        new LootFixedConfig(){Loot_ID = 1115,Loot_List = new LootFixedInfo[]{new LootFixedInfo(1010,1,1),new LootFixedInfo(1015,3,4),}},

        new LootFixedConfig(){Loot_ID = 11200,Loot_List = new LootFixedInfo[]{new LootFixedInfo(1010,1,2),}},
        new LootFixedConfig(){Loot_ID = 11201,Loot_List = new LootFixedInfo[]{new LootFixedInfo(1010,1,2),}},

        new LootFixedConfig(){Loot_ID = 12000,Loot_List = new LootFixedInfo[]{new LootFixedInfo(1002,2,3),}},
        new LootFixedConfig(){Loot_ID = 12001,Loot_List = new LootFixedInfo[]{new LootFixedInfo(1002,2,3),}},

        new LootFixedConfig(){Loot_ID = 12010,Loot_List = new LootFixedInfo[]{new LootFixedInfo(1002,2,3),}},
        new LootFixedConfig(){Loot_ID = 12011,Loot_List = new LootFixedInfo[]{new LootFixedInfo(3002,1,1),}},

        new LootFixedConfig(){Loot_ID = 1202,Loot_List = new LootFixedInfo[]{new LootFixedInfo(1000,2,3),new LootFixedInfo(1001,2,3),}},

        new LootFixedConfig(){Loot_ID = 12030,Loot_List = new LootFixedInfo[]{new LootFixedInfo(1002,2,3)}},
        new LootFixedConfig(){Loot_ID = 12031,Loot_List = new LootFixedInfo[]{new LootFixedInfo(1003,2,3)}},

        new LootFixedConfig(){Loot_ID = 12050,Loot_List = new LootFixedInfo[]{new LootFixedInfo(3005,1,1)}},
        new LootFixedConfig(){Loot_ID = 12051,Loot_List = new LootFixedInfo[]{new LootFixedInfo(3005,1,2)}},

        new LootFixedConfig(){Loot_ID = 12060,Loot_List = new LootFixedInfo[]{new LootFixedInfo(1004,1,1)}},
        new LootFixedConfig(){Loot_ID = 12061,Loot_List = new LootFixedInfo[]{new LootFixedInfo(1004,2,4)}},

        new LootFixedConfig(){Loot_ID = 13000,Loot_List = new LootFixedInfo[]{new LootFixedInfo(6102,1,1)}},
        new LootFixedConfig(){Loot_ID = 13001,Loot_List = new LootFixedInfo[]{new LootFixedInfo(6102,1,1)}},
        new LootFixedConfig(){Loot_ID = 13002,Loot_List = new LootFixedInfo[]{new LootFixedInfo(6102,1,3)}},

        new LootFixedConfig(){Loot_ID = 13010,Loot_List = new LootFixedInfo[]{new LootFixedInfo(6100,1,1)}},
        new LootFixedConfig(){Loot_ID = 13011,Loot_List = new LootFixedInfo[]{new LootFixedInfo(6100,1,1)}},
        new LootFixedConfig(){Loot_ID = 13012,Loot_List = new LootFixedInfo[]{ new LootFixedInfo(6100, 2, 2),new LootFixedInfo(3003,1,2)}},

        new LootFixedConfig(){Loot_ID = 13020,Loot_List = new LootFixedInfo[]{new LootFixedInfo(6101,1,1)}},
        new LootFixedConfig(){Loot_ID = 13021,Loot_List = new LootFixedInfo[]{ new LootFixedInfo(6101, 2, 2), new LootFixedInfo(3004,1,2)}},

    };
}
public struct LootRandomConfig
{
    public int Loot_ID;
    public LootRandomInfo[] Loot_List;
}
public struct LootFixedConfig
{
    public int Loot_ID;
    public LootFixedInfo[] Loot_List;
}