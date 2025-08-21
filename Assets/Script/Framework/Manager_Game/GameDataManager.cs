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
        Debug.Log("��ʼ��ͼ��Ϣ" + bind_MapInfoPath);
        Debug.Log("��ʼ���صذ�(���)" + bind_MapFloorTypeFilePath);
        Debug.Log("��ʼ���ؽ���(���)" + bind_MapBuildingTypeFilePath);
        Debug.Log("��ʼ���ؽ���(��Ϣ)" + bind_MapBuildingInfoFilePath);
        string mapInfoJson = FileManager.Instance.ReadFile(bind_MapInfoPath);
        if (mapInfoJson == "")
        {
            Debug.Log("δ��ȡ��Ŀ���ļ�");
            mapInfoData = new MapInfoData();
            FileManager.Instance.WriteFile(bind_MapInfoPath, JsonConvert.SerializeObject(mapInfoData));
        }
        else
        {
            MapInfoData data = JsonConvert.DeserializeObject<MapInfoData>(mapInfoJson);
            if (data == null)
            {
                Debug.Log("��ͼ��Ϣjson�ļ�����ʧ��" + mapInfoJson);
                mapInfoData = new MapInfoData();
                FileManager.Instance.WriteFile(bind_MapInfoPath, JsonConvert.SerializeObject(mapInfoData));
            }
            else
            {
                Debug.Log("��ͼ��Ϣjson�ļ������ɹ�");
                mapInfoData = data;
            }
        }
        string buildTypeJson = FileManager.Instance.ReadFile(bind_MapBuildingTypeFilePath);
        if (buildTypeJson == "")
        {
            Debug.Log("δ��ȡ��Ŀ���ļ�");
            builingTileTypeData = new MapTileTypeData();
            FileManager.Instance.WriteFile(bind_MapBuildingTypeFilePath, JsonConvert.SerializeObject(builingTileTypeData));
        }
        else
        {
            MapTileTypeData tiles = JsonConvert.DeserializeObject<MapTileTypeData>(buildTypeJson);
            if (tiles == null)
            {
                Debug.Log("�������json�ļ�����ʧ��" + buildTypeJson);
                builingTileTypeData = new MapTileTypeData();
                FileManager.Instance.WriteFile(bind_MapBuildingTypeFilePath, JsonConvert.SerializeObject(builingTileTypeData));
            }
            else
            {
                Debug.Log("�������json�ļ������ɹ�");
                builingTileTypeData = tiles;
            }

        }

        string buildInfoJson = FileManager.Instance.ReadFile(bind_MapBuildingInfoFilePath);
        if (buildInfoJson == "")
        {
            Debug.Log("δ��ȡ��Ŀ���ļ�");
            builingTileInfoData = new MapTileInfoData();
            FileManager.Instance.WriteFile(bind_MapBuildingInfoFilePath, JsonConvert.SerializeObject(builingTileInfoData));
        }
        else
        {
            MapTileInfoData tiles = JsonConvert.DeserializeObject<MapTileInfoData>(buildInfoJson);
            if (tiles == null)
            {
                Debug.Log("������Ϣjson�ļ�����ʧ��" + buildInfoJson);
                builingTileInfoData = new MapTileInfoData();
                FileManager.Instance.WriteFile(bind_MapBuildingInfoFilePath, JsonConvert.SerializeObject(builingTileInfoData));
            }
            else
            {
                Debug.Log("������Ϣjson�ļ������ɹ�");
                builingTileInfoData = tiles;
            }
        }

        string floorTypeJson = FileManager.Instance.ReadFile(bind_MapFloorTypeFilePath);
        if (floorTypeJson == "")
        {
            Debug.Log("δ��ȡ��Ŀ���ļ�");
            floorTileTypeData = new MapTileTypeData();
            FileManager.Instance.WriteFile(bind_MapFloorTypeFilePath, JsonConvert.SerializeObject(floorTileTypeData));
        }
        else
        {
            MapTileTypeData tiles = JsonConvert.DeserializeObject<MapTileTypeData>(floorTypeJson);
            if (tiles == null)
            {
                Debug.Log("�ؿ����json�ļ�����ʧ��" + floorTypeJson);
                floorTileTypeData = new MapTileTypeData();
                FileManager.Instance.WriteFile(bind_MapFloorTypeFilePath, JsonConvert.SerializeObject(floorTileTypeData));
            }
            else
            {
                Debug.Log("�ؿ����json�ļ������ɹ�");
                floorTileTypeData = tiles;
            }
        }

    }
    public void SaveMap(MapInfoData mapInfoData, MapTileTypeData buildingTileTypeData, MapTileInfoData buildingTileInfoData, MapTileTypeData floorTileTypeData)
    {
        Debug.Log("��ʼ�����ͼ" + bind_MapBuildingTypeFilePath);
        FileManager.Instance.WriteFile(bind_MapInfoPath, JsonConvert.SerializeObject(mapInfoData));
        FileManager.Instance.WriteFile(bind_MapBuildingTypeFilePath, JsonConvert.SerializeObject(buildingTileTypeData));
        FileManager.Instance.WriteFile(bind_MapBuildingInfoFilePath, JsonConvert.SerializeObject(buildingTileInfoData));
        FileManager.Instance.WriteFile(bind_MapFloorTypeFilePath, JsonConvert.SerializeObject(floorTileTypeData));
    }
    public void LoadPlayer(out PlayerData playerData)
    {
        Debug.Log("��ʼ��������" + bind_PlayerDataPath);
        string json = FileManager.Instance.ReadFile(bind_PlayerDataPath);
        if (json == "")
        {
            Debug.Log("δ��ȡ��Ŀ���ļ�");
            playerData = new PlayerData();
            FileManager.Instance.WriteFile(bind_PlayerDataPath, JsonConvert.SerializeObject(playerData));
        }
        else
        {
            playerData = JsonConvert.DeserializeObject<PlayerData>(json);
            if (playerData == null)
            {
                Debug.Log("����json�ļ�����ʧ��" + json);
            }
            else
            {
                Debug.Log("����json�ļ������ɹ�");
            }
        }
    }
    public void SavePlayer(PlayerData playerData)
    {
        Debug.Log("��ʼ�����ɫ" + bind_PlayerDataPath);
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
