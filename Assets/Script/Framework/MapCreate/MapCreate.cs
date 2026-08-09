using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Accessibility;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using static Fusion.Allocator;
using static Fusion.Sockets.NetBitBuffer;
public partial class MapCreate 
{
    public MapConfig config_Map;
    public int seed_Offset;
    public int seed_Acc;
    public Slider slider_Waiting;
    public TextMeshProUGUI text_Waiting;
    /// <summary>
    /// 地面数据
    /// </summary>
    public MapTileTypeData data_mapGroundData;
    /// <summary>
    /// 建筑数据
    /// </summary>
    public MapTileTypeData data_mapBuildingData;

    #region//生成器
    public MapCreate_Land_Main mapCreate_Land = new MapCreate_Land_Main();
    public MapCreate_Land_Snow mapCreate_Snowland = new MapCreate_Land_Snow();
    public MapCreate_Land_Desert mapCreate_Desert = new MapCreate_Land_Desert();
    public MapCreate_Sea_Inland mapCreate_InlandSea = new MapCreate_Sea_Inland();
    public MapCreate_Land_SubBiome mapCreate_Land_SubBiome = new MapCreate_Land_SubBiome();
    public MapCreate_Land_Noise mapCreate_Noise = new MapCreate_Land_Noise();   

    public MapCreate_River mapCreate_River = new MapCreate_River();
    public MapCreate_Lakes mapCreate_Lakes = new MapCreate_Lakes();
    public MapCreate_Roads mapCreate_Road = new MapCreate_Roads();

    public MapCreate_Forests mapCreate_Forest = new MapCreate_Forests();
    public MapCreate_Mines mapCreate_Mine = new MapCreate_Mines();
    public MapCreate_Animal mapCreate_Animal = new MapCreate_Animal();
    public MapCreate_Monster mapCreate_Monster = new MapCreate_Monster();

    public MapCreate_RandomBuilding mapCreate_RandomBuilding = new MapCreate_RandomBuilding();
    public MapCreate_ConfigBuilding mapCreate_ConfigBuilding = new MapCreate_ConfigBuilding();

    public MapCreate_Supply mapCreate_Supply = new MapCreate_Supply();
    public MapCreate_GroupPlant mapCreate_GroupPlant = new MapCreate_GroupPlant();
    public MapCreate_SinglePlant mapCreate_SinglePlant = new MapCreate_SinglePlant();
    #endregion
    public async Task CreateMapGroundAndBuilding(MapConfig config, Slider slider, TextMeshProUGUI text, Action<MapTileTypeData, MapTileTypeData> callBack_ReturnData)
    {
        seed_Offset = config.map_Seed;
        config_Map = config;
        slider_Waiting = slider;
        text_Waiting = text;
        data_mapGroundData = new MapTileTypeData();
        data_mapBuildingData = new MapTileTypeData();

        #region 大陆
        await mapCreate_Land.CreateLandArea(this);
        await mapCreate_Snowland.CreateSnowland(this);
        await mapCreate_Desert.CreateDesert(this);
        await mapCreate_InlandSea.CreateInlandSea(this);
        await mapCreate_InlandSea.CreateIslet(this);
        await mapCreate_Land_SubBiome.CreateSubBiome(this);
        #endregion

        if (config.map_Type != MapType.Plane)
        {
            #region 矿区 
            await mapCreate_Mine.CreateMining_Rock(this);
            await mapCreate_Mine.CreateMining_ColorRock(this);
            #endregion

            #region 水域
            await mapCreate_River.CreateRiver(this);
            await mapCreate_Lakes.CreateLake(this);
            #endregion

            #region 森林
            await mapCreate_Forest.CreateForests(this);
            await mapCreate_SinglePlant.CreateSinglePlant(this);
            await mapCreate_GroupPlant.CreateGroupPlant(this);
            #endregion

            #region 生物
            await mapCreate_Animal.CreateAnimal(this);
            await mapCreate_Monster.CreateMonster(this);
            #endregion

            #region 人造
            await mapCreate_Road.CreateRoad(this);
            await mapCreate_Supply.CreateSupply(this);
            await mapCreate_RandomBuilding.CreateRandomRuins(this);
            await mapCreate_RandomBuilding.CreateRandomBuilding(this);
            await mapCreate_ConfigBuilding.CreateConfigBuilding(this);
            #endregion
        }
        await mapCreate_ConfigBuilding.CreateSunBuilding(this);
        await mapCreate_Noise.CreateNoise(this);

        callBack_ReturnData.Invoke(data_mapGroundData, data_mapBuildingData);
    }

}
public struct MapConfig
    {
    /// <summary>
    /// 地图尺寸(半径)
    /// </summary>
    public int map_Size;
    /// <summary>
    /// 地图模式
    /// </summary>
    public MapType map_Type;
    /// <summary>
    /// 地图种子
    /// </summary>
    public int map_Seed;
    }
public struct NoiseConfig
    {
        /// <summary>
        /// 噪声尺寸 1-20
        /// </summary>
        public int noise_Sacle;
        /// <summary>
        /// 噪声出现几率 0-1
        /// </summary>
        public float noise_Prob;
        /// <summary>
        /// 噪声偏移
        /// </summary>
        public Vector2 noise_Offset;
    }
public struct AreaConfig
    {
        /// <summary>
        /// 地区尺寸
        /// </summary>
        public int area_Size;
        /// <summary>
        /// 地区中心
        /// </summary>
        public Vector2Int area_Center;
    }
public enum MapType 
{
    Default,Plane
}

