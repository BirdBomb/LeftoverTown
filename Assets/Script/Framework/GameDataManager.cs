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
    public string mapTileTypeFilePath{ get; set; }
    public string mapTileInfoFilePath { get; set; }
    public void Init()
    {
        
    }
    public void LoadMap(out MapTileTypeData mapTileTypeData,out MapTileInfoData mapTileInfoData)
    {
        Debug.Log("开始加载地图(类别)" + mapTileTypeFilePath);
        Debug.Log("开始加载地图(信息)" + mapTileInfoFilePath);
        string typeJson = FileManager.Instance.ReadFile(mapTileTypeFilePath);
        if (typeJson == "")
        {
            Debug.Log("未获取到目标文件");
            mapTileTypeData = new MapTileTypeData();
            FileManager.Instance.WriteFile(mapTileTypeFilePath, JsonConvert.SerializeObject(mapTileTypeData));
        }
        else
        {
            MapTileTypeData tiles = JsonConvert.DeserializeObject<MapTileTypeData>(typeJson);
            if (tiles == null)
            {
                Debug.Log("地图json文件解析失败" + typeJson);
                mapTileTypeData = new MapTileTypeData();
                FileManager.Instance.WriteFile(mapTileTypeFilePath, JsonConvert.SerializeObject(mapTileTypeData));
            }
            else
            {
                Debug.Log("地图json文件解析成功");
                mapTileTypeData = tiles;
            }

        }

        string infoJson = FileManager.Instance.ReadFile(mapTileInfoFilePath);
        if (infoJson == "")
        {
            Debug.Log("未获取到目标文件");
            mapTileInfoData = new MapTileInfoData();
            FileManager.Instance.WriteFile(mapTileInfoFilePath, JsonConvert.SerializeObject(mapTileInfoData));
        }
        else
        {
            MapTileInfoData tiles = JsonConvert.DeserializeObject<MapTileInfoData>(infoJson);
            if (tiles == null)
            {
                Debug.Log("地图json文件解析失败" + infoJson);
                mapTileInfoData = new MapTileInfoData();
                FileManager.Instance.WriteFile(mapTileInfoFilePath, JsonConvert.SerializeObject(mapTileInfoData));
            }
            else
            {
                Debug.Log("地图json文件解析成功");
                mapTileInfoData = tiles;
            }
        }

    }
    public void SaveMap(MapTileTypeData mapTileTypeData, MapTileInfoData mapTileInfoData)
    {
        Debug.Log("开始保存地图" + mapTileTypeFilePath);
        FileManager.Instance.WriteFile(mapTileTypeFilePath, JsonConvert.SerializeObject(mapTileTypeData));
        FileManager.Instance.WriteFile(mapTileInfoFilePath, JsonConvert.SerializeObject(mapTileInfoData));
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
