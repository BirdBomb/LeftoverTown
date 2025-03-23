using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapModConfigData 
{
    public static MapModConfig GetMapModConfig(int ID)
    {
        return mapModConfigs.Find((x) => { return x.MapMod_ID == ID; });
    }
    public readonly static List<MapModConfig> mapModConfigs = new List<MapModConfig>()
    {
        new MapModConfig(){ MapMod_ID = 0,MapMod_Name = "初始台地"},
        new MapModConfig(){ MapMod_ID = 1,MapMod_Name = "小型贸易站"},
    };
}
[Serializable]
public struct MapModConfig
{
    /// <summary>
    /// 身份编号
    /// </summary>
    [SerializeField]/*编号*/
    public int MapMod_ID;
    /// <summary>
    /// 身份名称
    /// </summary>
    [SerializeField]
    public string MapMod_Name;
}
