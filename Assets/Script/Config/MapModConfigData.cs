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
        /*太阳祭坛*/new MapModConfig(){ MapMod_ID = 0,MapMod_BaseGround = new short[]{-1}, MapMod_Ruins = false},
        /*-----敌对定居点-----*/
        /*铁钩僵尸*/new MapModConfig(){ MapMod_ID = 100,MapMod_BaseGround = new short[]{-1}, MapMod_Ruins = false},
        /*旧实验室*/new MapModConfig(){ MapMod_ID = 101,MapMod_BaseGround = new short[]{-1}, MapMod_Ruins = false},
        /*-----友善定居点-----*/
        /*三季镇*/new MapModConfig(){ MapMod_ID = 200,MapMod_BaseGround = new short[]{-1}, MapMod_Ruins = false},

        /*-----人造建筑-----*/
        /*猎人小屋*/new MapModConfig(){ MapMod_ID = 10010, MapMod_BaseGround = new short[]{1001}, MapMod_Ruins = false},
        /*农夫小屋*/new MapModConfig(){ MapMod_ID = 10011, MapMod_BaseGround = new short[]{1001}, MapMod_Ruins = false},
        /*矿商小屋*/new MapModConfig(){ MapMod_ID = 10012, MapMod_BaseGround = new short[]{1001}, MapMod_Ruins = false},
        /*伐木小屋*/new MapModConfig(){ MapMod_ID = 10013, MapMod_BaseGround = new short[]{1001}, MapMod_Ruins = false},
        /*土匪小屋*/new MapModConfig(){ MapMod_ID = 10050, MapMod_BaseGround = new short[]{1005}, MapMod_Ruins = false},

        /*-----遗迹-----*/
        new MapModConfig(){ MapMod_ID = 9001, MapMod_BaseGround = new short[]{ 1000, 1001, 1002, 1003, 1004, 1005 }, MapMod_Ruins = true},/*遗迹_破屋与宝箱*/
        new MapModConfig(){ MapMod_ID = 9002, MapMod_BaseGround = new short[]{ 1000, 1001, 1002, 1003, 1004, 1005 }, MapMod_Ruins = true},/*遗迹_墓地*/
        new MapModConfig(){ MapMod_ID = 9003, MapMod_BaseGround = new short[]{ 1000, 1001, 1002, 1003, 1004, 1005 }, MapMod_Ruins = true},/*遗迹_水井*/
        new MapModConfig(){ MapMod_ID = 9004, MapMod_BaseGround = new short[]{ 1000, 1001, 1002, 1003, 1004, 1005 }, MapMod_Ruins = true},/*遗迹_墙体与宝箱*/
        new MapModConfig(){ MapMod_ID = 9005, MapMod_BaseGround = new short[]{ 1000, 1001, 1002, 1003, 1004, 1005 }, MapMod_Ruins = true},/*遗迹_岩石区*/
        new MapModConfig(){ MapMod_ID = 9006, MapMod_BaseGround = new short[]{ 1000, 1001, 1002, 1003, 1004, 1005 }, MapMod_Ruins = true},/*遗迹_路灯与尸体*/
        new MapModConfig(){ MapMod_ID = 9007, MapMod_BaseGround = new short[]{ 1000, 1001, 1002, 1003, 1004, 1005 }, MapMod_Ruins = true},/*遗迹_公厕*/
        new MapModConfig(){ MapMod_ID = 9008, MapMod_BaseGround = new short[]{ 1000, 1001, 1002, 1003, 1004, 1005 }, MapMod_Ruins = true},/*遗迹_深坑与宝箱*/
        new MapModConfig(){ MapMod_ID = 9009, MapMod_BaseGround = new short[]{ 1000, 1001, 1002, 1003, 1004, 1005 }, MapMod_Ruins = true},/*遗迹_环形小径*/

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
    public short[] MapMod_BaseGround;
    [SerializeField]

    public short[] MapMod_BaseBuilding;
    /// <summary>
    /// 是否是废墟
    /// </summary>
    [SerializeField]
    public bool MapMod_Ruins;
}
