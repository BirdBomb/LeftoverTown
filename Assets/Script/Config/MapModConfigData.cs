using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapModConfigData
{
    public static MapModConfig GetMapModConfig(int ID)
    {
        return mapModConfigs.Find((x) => { return x.MapMod_ID == ID; });
    }
    public readonly static List<MapModConfig> mapModConfigs = new List<MapModConfig>()
    {
        /*太阳祭坛*/new MapModConfig(){ MapMod_ID = 0,MapMod_BaseGround = -1},
        /*铁钩僵尸*/new MapModConfig(){ MapMod_ID = 100,MapMod_BaseGround = -1},
        /*三季镇*/new MapModConfig(){ MapMod_ID = 200,MapMod_BaseGround = -1},
        /*猎人小屋*/new MapModConfig(){ MapMod_ID = 10010,MapMod_BaseGround = 1001},
        /*农夫小屋*/new MapModConfig(){ MapMod_ID = 10011,MapMod_BaseGround = 1001},
        /*矿商小屋*/new MapModConfig(){ MapMod_ID = 10012,MapMod_BaseGround = 1001},
        /*伐木小屋*/new MapModConfig(){ MapMod_ID = 10013,MapMod_BaseGround = 1001},
        /*土匪小屋*/new MapModConfig(){ MapMod_ID = 10050,MapMod_BaseGround = 1005},
    };
}
[Serializable]
public struct MapModConfig
{
    /// <summary>
    /// 地图预设编号
    /// </summary>
    [SerializeField]/*编号*/
    public int MapMod_ID;
    /// <summary>
    /// 地图预设地基种类
    /// </summary>
    [SerializeField]
    public short MapMod_BaseGround;
}
