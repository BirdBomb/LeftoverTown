using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using Newtonsoft.Json;
using UniRx;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

public class GameDataManager :SingleTon<GameDataManager> ,ISingleTon
{
    /// <summary>
    /// 角色文件路径
    /// </summary>
    public string actorFilePath { get; set; }
    public string mapBuildingTypeFilePath{ get; set; }
    public string mapBuildingInfoFilePath { get; set; }
    public string mapFloorTypeFilePath { get; set; }
    public void Init()
    {
        
    }
    public void LoadMap(out MapTileTypeData mapTileTypeData,out MapTileInfoData mapTileInfoData,out MapTileInfoData floorTileTypeData)
    {
        Debug.Log("开始加载建筑(类别)" + mapBuildingTypeFilePath);
        Debug.Log("开始加载建筑(信息)" + mapBuildingInfoFilePath);
        Debug.Log("开始加载地板(类别)" + mapFloorTypeFilePath);
        string typeJson = FileManager.Instance.ReadFile(mapBuildingTypeFilePath);
        if (typeJson == "")
        {
            Debug.Log("未获取到目标文件");
            mapTileTypeData = new MapTileTypeData();
            FileManager.Instance.WriteFile(mapBuildingTypeFilePath, JsonConvert.SerializeObject(mapTileTypeData));
        }
        else
        {
            MapTileTypeData tiles = JsonConvert.DeserializeObject<MapTileTypeData>(typeJson);
            if (tiles == null)
            {
                Debug.Log("地图json文件解析失败" + typeJson);
                mapTileTypeData = new MapTileTypeData();
                FileManager.Instance.WriteFile(mapBuildingTypeFilePath, JsonConvert.SerializeObject(mapTileTypeData));
            }
            else
            {
                Debug.Log("地图json文件解析成功");
                mapTileTypeData = tiles;
            }

        }

        string infoJson = FileManager.Instance.ReadFile(mapBuildingInfoFilePath);
        if (infoJson == "")
        {
            Debug.Log("未获取到目标文件");
            mapTileInfoData = new MapTileInfoData();
            FileManager.Instance.WriteFile(mapBuildingInfoFilePath, JsonConvert.SerializeObject(mapTileInfoData));
        }
        else
        {
            MapTileInfoData tiles = JsonConvert.DeserializeObject<MapTileInfoData>(infoJson);
            if (tiles == null)
            {
                Debug.Log("地图json文件解析失败" + infoJson);
                mapTileInfoData = new MapTileInfoData();
                FileManager.Instance.WriteFile(mapBuildingInfoFilePath, JsonConvert.SerializeObject(mapTileInfoData));
            }
            else
            {
                Debug.Log("地图json文件解析成功");
                mapTileInfoData = tiles;
            }
        }

        string floorInfoJson = FileManager.Instance.ReadFile(mapFloorTypeFilePath);
        if (floorInfoJson == "")
        {
            Debug.Log("未获取到目标文件");
            floorTileTypeData = new MapTileInfoData();
            FileManager.Instance.WriteFile(mapFloorTypeFilePath, JsonConvert.SerializeObject(floorTileTypeData));
        }
        else
        {
            MapTileInfoData tiles = JsonConvert.DeserializeObject<MapTileInfoData>(floorInfoJson);
            if (tiles == null)
            {
                Debug.Log("地图json文件解析失败" + floorInfoJson);
                floorTileTypeData = new MapTileInfoData();
                FileManager.Instance.WriteFile(mapFloorTypeFilePath, JsonConvert.SerializeObject(floorTileTypeData));
            }
            else
            {
                Debug.Log("地图json文件解析成功");
                floorTileTypeData = tiles;
            }
        }

    }
    public void SaveMap(MapTileTypeData mapTileTypeData, MapTileInfoData mapTileInfoData, MapTileInfoData floorTileInfoData)
    {
        Debug.Log("开始保存地图" + mapBuildingTypeFilePath);
        FileManager.Instance.WriteFile(mapBuildingTypeFilePath, JsonConvert.SerializeObject(mapTileTypeData));
        FileManager.Instance.WriteFile(mapBuildingInfoFilePath, JsonConvert.SerializeObject(mapTileInfoData));
        FileManager.Instance.WriteFile(mapFloorTypeFilePath, JsonConvert.SerializeObject(floorTileInfoData));
    }
    public PlayerData LoadPlayerData()
    {
        Debug.Log("开始加载人物" + actorFilePath);
        string json = FileManager.Instance.ReadFile(actorFilePath);
        if (json == "")
        {
            Debug.Log("未获取到目标文件");
            PlayerData playerData = new PlayerData();
            FileManager.Instance.WriteFile(actorFilePath, JsonConvert.SerializeObject(playerData));
            return null;
        }
        else
        {
            PlayerData playerData = JsonConvert.DeserializeObject<PlayerData>(json);
            if (playerData == null)
            {
                Debug.Log("人物json文件解析失败" + json);
                return null;
            }
            else
            {
                Debug.Log("人物json文件解析成功");
                return playerData;
            }
        }
    }
}
