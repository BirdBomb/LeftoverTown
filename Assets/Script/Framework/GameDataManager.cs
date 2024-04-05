using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using Newtonsoft.Json;
using UniRx;
using UnityEngine;
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
    /// <summary>
    /// 读取当前地块信息
    /// </summary>
    /// <param name="mapName"></param>
    /// <param name="mapManager"></param>
    public void LoadMapData(string mapName,MapManager mapManager)
    {
        string json = FileManager.Instance.ReadFile(mapName); 
        if(json == "")
        {
            Debug.Log("未获取到目标文件");
            return;
        }
        MapData tiles = JsonConvert.DeserializeObject<MapData>(json);
        if( tiles == null )
        {
            Debug.Log( json );
            Debug.Log("地图json文件解析失败");
            return;
        }
        else
        {
            Debug.Log("地图json文件解析成功");
            foreach (var vec2 in tiles.tiles.Keys)
            {
                string[] pos = vec2.Split(",");
                string[] info = tiles.tiles[vec2].Split("/*T*/");

                mapManager.LoadTile(new Vector3Int(int.Parse(pos[0]), int.Parse(pos[1])), info[0], info[1]);
            }
        }
    }
    /// <summary>
    /// 存储当前地块信息
    /// </summary>
    /// <param name="saveMap"></param>
    public void SaveMapData(string mapName, MapManager mapManager)
    {
        BoundsInt area = mapManager.tilemap.cellBounds;
        MapData tileGrid = new MapData();
        for(int x = area.xMin; x < area.xMax; x++ )
        {
            for (int y = area.yMin; y < area.yMax; y++)
            {
                mapManager.SaveTile(new Vector3Int(x, y, 0),out string json);
                tileGrid.tiles[x + "," + y] = json;
            }
        }
        Debug.Log(JsonConvert.SerializeObject(tileGrid));
        FileManager.Instance.WriteFile(mapName, JsonConvert.SerializeObject(tileGrid));
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
