using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

public class GameDataManager :SingleTon<GameDataManager> ,ISingleTon
{
    public string bind_PlayerDataPath { get; set; }
    public string bind_MapInfoPath { get; set; }
    public string bind_MapBuildingTypeFilePath { get; set; }
    public string bind_MapBuildingInfoFilePath { get; set; }
    public string bind_MapFloorTypeFilePath { get; set; }
    public void Init()
    {
        
    }
    public void LoadMap(out MapInfoData mapInfoData,out MapTileTypeData builingTileTypeData,out MapTileInfoData builingTileInfoData, out MapTileTypeData floorTileTypeData)
    {
        //Debug.Log("开始地图信息" + bind_MapInfoPath);
        //Debug.Log("开始加载地板(类别)" + bind_MapFloorTypeFilePath);
        //Debug.Log("开始加载建筑(类别)" + bind_MapBuildingTypeFilePath);
        //Debug.Log("开始加载建筑(信息)" + bind_MapBuildingInfoFilePath);

        mapInfoData = new MapInfoData();
        floorTileTypeData = new MapTileTypeData();
        builingTileTypeData = new MapTileTypeData();
        builingTileInfoData = new MapTileInfoData();

        mapInfoData = FileManager.Instance.ReadMapInfoData(bind_MapInfoPath);
        floorTileTypeData = FileManager.Instance.ReadTileTypeData(bind_MapFloorTypeFilePath);
        builingTileTypeData = FileManager.Instance.ReadTileTypeData(bind_MapBuildingTypeFilePath);
        builingTileInfoData = FileManager.Instance.ReadTileInfoData(bind_MapBuildingInfoFilePath);
    }
    public async void SaveMap(MapInfoData mapInfoData, MapTileTypeData buildingTileTypeData, MapTileInfoData buildingTileInfoData, MapTileTypeData floorTileTypeData)
    {
        //Debug.Log("开始保存地图" + bind_MapBuildingTypeFilePath);
        await FileManager.Instance.WriteBytes(bind_MapInfoPath, mapInfoData);
        await FileManager.Instance.WriteBytes(bind_MapBuildingTypeFilePath, buildingTileTypeData);
        await FileManager.Instance.WriteBytes(bind_MapFloorTypeFilePath, floorTileTypeData);
        await FileManager.Instance.WriteBytes(bind_MapBuildingInfoFilePath, buildingTileInfoData);
    }
    public void LoadPlayer(out PlayerData playerData)
    {
        //Debug.Log("开始加载人物" + bind_PlayerDataPath);
        playerData =  FileManager.Instance.ReadPlayerData(bind_PlayerDataPath);
    }
    public async void SavePlayer(PlayerData playerData)
    {
        //Debug.Log("开始保存角色" + bind_PlayerDataPath);
        await FileManager.Instance.WriteBytes(bind_PlayerDataPath, playerData);
    }
}
