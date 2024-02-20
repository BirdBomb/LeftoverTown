using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameDataManager :SingleTon<GameDataManager> ,ISingleTon
{
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
        MyTileGrid tiles = JsonConvert.DeserializeObject<MyTileGrid>(json);
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
        MyTileGrid tileGrid = new MyTileGrid();
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
}
