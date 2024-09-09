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
    /// <summary>
    /// ��ɫ�ļ�·��
    /// </summary>
    public string actorFilePath { get; set; }
    public string mapBuildingTypeFilePath{ get; set; }
    public string mapBuildingInfoFilePath { get; set; }
    public string mapFloorTypeFilePath { get; set; }
    public void Init()
    {
        
    }
    public void LoadMap(out MapTileTypeData mapTileTypeData,out MapTileInfoData mapTileInfoData,out MapTileTypeData floorTileTypeData)
    {
        Debug.Log("��ʼ���ؽ���(���)" + mapBuildingTypeFilePath);
        Debug.Log("��ʼ���ؽ���(��Ϣ)" + mapBuildingInfoFilePath);
        Debug.Log("��ʼ���صذ�(���)" + mapFloorTypeFilePath);
        string buildTypeJson = FileManager.Instance.ReadFile(mapBuildingTypeFilePath);
        if (buildTypeJson == "")
        {
            Debug.Log("δ��ȡ��Ŀ���ļ�");
            mapTileTypeData = new MapTileTypeData();
            FileManager.Instance.WriteFile(mapBuildingTypeFilePath, JsonConvert.SerializeObject(mapTileTypeData));
        }
        else
        {
            MapTileTypeData tiles = JsonConvert.DeserializeObject<MapTileTypeData>(buildTypeJson);
            if (tiles == null)
            {
                Debug.Log("��ͼjson�ļ�����ʧ��" + buildTypeJson);
                mapTileTypeData = new MapTileTypeData();
                FileManager.Instance.WriteFile(mapBuildingTypeFilePath, JsonConvert.SerializeObject(mapTileTypeData));
            }
            else
            {
                Debug.Log("��ͼjson�ļ������ɹ�");
                mapTileTypeData = tiles;
            }

        }

        string buildInfoJson = FileManager.Instance.ReadFile(mapBuildingInfoFilePath);
        if (buildInfoJson == "")
        {
            Debug.Log("δ��ȡ��Ŀ���ļ�");
            mapTileInfoData = new MapTileInfoData();
            FileManager.Instance.WriteFile(mapBuildingInfoFilePath, JsonConvert.SerializeObject(mapTileInfoData));
        }
        else
        {
            MapTileInfoData tiles = JsonConvert.DeserializeObject<MapTileInfoData>(buildInfoJson);
            if (tiles == null)
            {
                Debug.Log("��ͼjson�ļ�����ʧ��" + buildInfoJson);
                mapTileInfoData = new MapTileInfoData();
                FileManager.Instance.WriteFile(mapBuildingInfoFilePath, JsonConvert.SerializeObject(mapTileInfoData));
            }
            else
            {
                Debug.Log("��ͼjson�ļ������ɹ�");
                mapTileInfoData = tiles;
            }
        }

        string floorTypeJson = FileManager.Instance.ReadFile(mapFloorTypeFilePath);
        if (floorTypeJson == "")
        {
            Debug.Log("δ��ȡ��Ŀ���ļ�");
            floorTileTypeData = new MapTileTypeData();
            FileManager.Instance.WriteFile(mapFloorTypeFilePath, JsonConvert.SerializeObject(floorTileTypeData));
        }
        else
        {
            MapTileTypeData tiles = JsonConvert.DeserializeObject<MapTileTypeData>(floorTypeJson);
            if (tiles == null)
            {
                Debug.Log("��ͼjson�ļ�����ʧ��" + floorTypeJson);
                floorTileTypeData = new MapTileTypeData();
                FileManager.Instance.WriteFile(mapFloorTypeFilePath, JsonConvert.SerializeObject(floorTileTypeData));
            }
            else
            {
                Debug.Log("��ͼjson�ļ������ɹ�");
                floorTileTypeData = tiles;
            }
        }

    }
    public void SaveMap(MapTileTypeData mapTileTypeData, MapTileInfoData mapTileInfoData, MapTileTypeData floorTileTypeData)
    {
        Debug.Log("��ʼ�����ͼ" + mapBuildingTypeFilePath);
        FileManager.Instance.WriteFile(mapBuildingTypeFilePath, JsonConvert.SerializeObject(mapTileTypeData));
        FileManager.Instance.WriteFile(mapBuildingInfoFilePath, JsonConvert.SerializeObject(mapTileInfoData));
        FileManager.Instance.WriteFile(mapFloorTypeFilePath, JsonConvert.SerializeObject(floorTileTypeData));
    }
    public void LoadPlayer(out PlayerData playerData)
    {
        Debug.Log("��ʼ��������" + actorFilePath);
        string json = FileManager.Instance.ReadFile(actorFilePath);
        if (json == "")
        {
            Debug.Log("δ��ȡ��Ŀ���ļ�");
            playerData = new PlayerData();
            FileManager.Instance.WriteFile(actorFilePath, JsonConvert.SerializeObject(playerData));
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
