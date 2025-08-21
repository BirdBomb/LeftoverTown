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
        Debug.Log("开始地图信息" + bind_MapInfoPath);
        Debug.Log("开始加载地板(类别)" + bind_MapFloorTypeFilePath);
        Debug.Log("开始加载建筑(类别)" + bind_MapBuildingTypeFilePath);
        Debug.Log("开始加载建筑(信息)" + bind_MapBuildingInfoFilePath);
        string mapInfoJson = FileManager.Instance.ReadFile(bind_MapInfoPath);
        if (mapInfoJson == "")
        {
            Debug.Log("未获取到目标文件");
            mapInfoData = new MapInfoData();
            FileManager.Instance.WriteFile(bind_MapInfoPath, JsonConvert.SerializeObject(mapInfoData));
        }
        else
        {
            MapInfoData data = JsonConvert.DeserializeObject<MapInfoData>(mapInfoJson);
            if (data == null)
            {
                Debug.Log("地图信息json文件解析失败" + mapInfoJson);
                mapInfoData = new MapInfoData();
                FileManager.Instance.WriteFile(bind_MapInfoPath, JsonConvert.SerializeObject(mapInfoData));
            }
            else
            {
                Debug.Log("地图信息json文件解析成功");
                mapInfoData = data;
            }
        }
        string buildTypeJson = FileManager.Instance.ReadFile(bind_MapBuildingTypeFilePath);
        if (buildTypeJson == "")
        {
            Debug.Log("未获取到目标文件");
            builingTileTypeData = new MapTileTypeData();
            FileManager.Instance.WriteFile(bind_MapBuildingTypeFilePath, JsonConvert.SerializeObject(builingTileTypeData));
        }
        else
        {
            MapTileTypeData tiles = JsonConvert.DeserializeObject<MapTileTypeData>(buildTypeJson);
            if (tiles == null)
            {
                Debug.Log("建筑类别json文件解析失败" + buildTypeJson);
                builingTileTypeData = new MapTileTypeData();
                FileManager.Instance.WriteFile(bind_MapBuildingTypeFilePath, JsonConvert.SerializeObject(builingTileTypeData));
            }
            else
            {
                Debug.Log("建筑类别json文件解析成功");
                builingTileTypeData = tiles;
            }

        }

        string buildInfoJson = FileManager.Instance.ReadFile(bind_MapBuildingInfoFilePath);
        if (buildInfoJson == "")
        {
            Debug.Log("未获取到目标文件");
            builingTileInfoData = new MapTileInfoData();
            FileManager.Instance.WriteFile(bind_MapBuildingInfoFilePath, JsonConvert.SerializeObject(builingTileInfoData));
        }
        else
        {
            MapTileInfoData tiles = JsonConvert.DeserializeObject<MapTileInfoData>(buildInfoJson);
            if (tiles == null)
            {
                Debug.Log("建筑信息json文件解析失败" + buildInfoJson);
                builingTileInfoData = new MapTileInfoData();
                FileManager.Instance.WriteFile(bind_MapBuildingInfoFilePath, JsonConvert.SerializeObject(builingTileInfoData));
            }
            else
            {
                Debug.Log("建筑信息json文件解析成功");
                builingTileInfoData = tiles;
            }
        }

        string floorTypeJson = FileManager.Instance.ReadFile(bind_MapFloorTypeFilePath);
        if (floorTypeJson == "")
        {
            Debug.Log("未获取到目标文件");
            floorTileTypeData = new MapTileTypeData();
            FileManager.Instance.WriteFile(bind_MapFloorTypeFilePath, JsonConvert.SerializeObject(floorTileTypeData));
        }
        else
        {
            MapTileTypeData tiles = JsonConvert.DeserializeObject<MapTileTypeData>(floorTypeJson);
            if (tiles == null)
            {
                Debug.Log("地块类别json文件解析失败" + floorTypeJson);
                floorTileTypeData = new MapTileTypeData();
                FileManager.Instance.WriteFile(bind_MapFloorTypeFilePath, JsonConvert.SerializeObject(floorTileTypeData));
            }
            else
            {
                Debug.Log("地块类别json文件解析成功");
                floorTileTypeData = tiles;
            }
        }

    }
    public void SaveMap(MapInfoData mapInfoData, MapTileTypeData buildingTileTypeData, MapTileInfoData buildingTileInfoData, MapTileTypeData floorTileTypeData)
    {
        Debug.Log("开始保存地图" + bind_MapBuildingTypeFilePath);
        FileManager.Instance.WriteFile(bind_MapInfoPath, JsonConvert.SerializeObject(mapInfoData));
        FileManager.Instance.WriteFile(bind_MapBuildingTypeFilePath, JsonConvert.SerializeObject(buildingTileTypeData));
        FileManager.Instance.WriteFile(bind_MapBuildingInfoFilePath, JsonConvert.SerializeObject(buildingTileInfoData));
        FileManager.Instance.WriteFile(bind_MapFloorTypeFilePath, JsonConvert.SerializeObject(floorTileTypeData));
    }
    public void LoadPlayer(out PlayerData playerData)
    {
        Debug.Log("开始加载人物" + bind_PlayerDataPath);
        string json = FileManager.Instance.ReadFile(bind_PlayerDataPath);
        if (json == "")
        {
            Debug.Log("未获取到目标文件");
            playerData = new PlayerData();
            FileManager.Instance.WriteFile(bind_PlayerDataPath, JsonConvert.SerializeObject(playerData));
        }
        else
        {
            playerData = JsonConvert.DeserializeObject<PlayerData>(json);
            if (playerData == null)
            {
                Debug.Log("人物json文件解析失败" + json);
            }
            else
            {
                Debug.Log("人物json文件解析成功");
            }
        }
    }
    public void SavePlayer(PlayerData playerData)
    {
        Debug.Log("开始保存角色" + bind_PlayerDataPath);
        FileManager.Instance.WriteFile(bind_PlayerDataPath, JsonConvert.SerializeObject(playerData));
    }
}
//public class UnityTypeConverter : JsonConverter
//{
//    public override bool CanRead => true;
//    public override bool CanWrite => true;
//    public override bool CanConvert(Type objectType)
//    {
//        return typeof(Vector2Int) == objectType;
//    }
//    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
//    {
//        //JObject jo = new JObject();
//        //if (value is Vector2Int vector2)
//        //{
//        //    jo.Add("x", vector2.x);
//        //    jo.Add("y", vector2.y);
//        //}
//        //jo.WriteTo(writer);
//        writer.WriteStartObject();
//        switch (value)
//        {
//            case Vector2Int v:
//                writer.WritePropertyName("x");
//                writer.WriteValue(v.x);
//                writer.WritePropertyName("y");
//                writer.WriteValue(v.y);
//                break;
//            default:
//                throw new Exception("Unexpected Error Occurred");
//        }
//        writer.WriteEndObject(); 
//    }
//    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
//    {
//        //JObject jo = JObject.Load(reader);
//        //if (objectType == typeof(Vector2Int))
//        //{
//        //    return new Vector2Int((int)jo["x"], (int)jo["y"]);
//        //}
//        //return null;
//        return objectType switch
//        {
//            var t when t == typeof(Vector2Int) => JsonConvert.DeserializeObject<Vector2Int>(serializer.Deserialize(reader).ToString()),
//            _ => throw new Exception("Unexpected Error Occurred"),
//        }; 
//    }
//}
