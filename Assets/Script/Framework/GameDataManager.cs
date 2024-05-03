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
    public string actorFilePath;
    /// <summary>
    /// 地图文件路径
    /// </summary>
    public string mapFilePath;

    public void Init()
    {
        
    }
    public MapData LoadMap()
    {
        Debug.Log("开始加载地图" + mapFilePath);
        string json = FileManager.Instance.ReadFile(mapFilePath);
        if (json == "")
        {
            Debug.Log("未获取到目标文件");
            return null;
        }
        else
        {
            MapData tiles = JsonConvert.DeserializeObject<MapData>(json);
            if (tiles == null)
            {
                Debug.Log("地图json文件解析失败" + json);
                return null;
            }
            else
            {
                Debug.Log("地图json文件解析成功");
                return tiles;
            }

        }
    }
    public void SaveMap(MapData mapData)
    {

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
