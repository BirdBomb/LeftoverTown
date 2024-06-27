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
    /// ��ɫ�ļ�·��
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
        Debug.Log("��ʼ���ؽ���(���)" + mapBuildingTypeFilePath);
        Debug.Log("��ʼ���ؽ���(��Ϣ)" + mapBuildingInfoFilePath);
        Debug.Log("��ʼ���صذ�(���)" + mapFloorTypeFilePath);
        string typeJson = FileManager.Instance.ReadFile(mapBuildingTypeFilePath);
        if (typeJson == "")
        {
            Debug.Log("δ��ȡ��Ŀ���ļ�");
            mapTileTypeData = new MapTileTypeData();
            FileManager.Instance.WriteFile(mapBuildingTypeFilePath, JsonConvert.SerializeObject(mapTileTypeData));
        }
        else
        {
            MapTileTypeData tiles = JsonConvert.DeserializeObject<MapTileTypeData>(typeJson);
            if (tiles == null)
            {
                Debug.Log("��ͼjson�ļ�����ʧ��" + typeJson);
                mapTileTypeData = new MapTileTypeData();
                FileManager.Instance.WriteFile(mapBuildingTypeFilePath, JsonConvert.SerializeObject(mapTileTypeData));
            }
            else
            {
                Debug.Log("��ͼjson�ļ������ɹ�");
                mapTileTypeData = tiles;
            }

        }

        string infoJson = FileManager.Instance.ReadFile(mapBuildingInfoFilePath);
        if (infoJson == "")
        {
            Debug.Log("δ��ȡ��Ŀ���ļ�");
            mapTileInfoData = new MapTileInfoData();
            FileManager.Instance.WriteFile(mapBuildingInfoFilePath, JsonConvert.SerializeObject(mapTileInfoData));
        }
        else
        {
            MapTileInfoData tiles = JsonConvert.DeserializeObject<MapTileInfoData>(infoJson);
            if (tiles == null)
            {
                Debug.Log("��ͼjson�ļ�����ʧ��" + infoJson);
                mapTileInfoData = new MapTileInfoData();
                FileManager.Instance.WriteFile(mapBuildingInfoFilePath, JsonConvert.SerializeObject(mapTileInfoData));
            }
            else
            {
                Debug.Log("��ͼjson�ļ������ɹ�");
                mapTileInfoData = tiles;
            }
        }

        string floorInfoJson = FileManager.Instance.ReadFile(mapFloorTypeFilePath);
        if (floorInfoJson == "")
        {
            Debug.Log("δ��ȡ��Ŀ���ļ�");
            floorTileTypeData = new MapTileInfoData();
            FileManager.Instance.WriteFile(mapFloorTypeFilePath, JsonConvert.SerializeObject(floorTileTypeData));
        }
        else
        {
            MapTileInfoData tiles = JsonConvert.DeserializeObject<MapTileInfoData>(floorInfoJson);
            if (tiles == null)
            {
                Debug.Log("��ͼjson�ļ�����ʧ��" + floorInfoJson);
                floorTileTypeData = new MapTileInfoData();
                FileManager.Instance.WriteFile(mapFloorTypeFilePath, JsonConvert.SerializeObject(floorTileTypeData));
            }
            else
            {
                Debug.Log("��ͼjson�ļ������ɹ�");
                floorTileTypeData = tiles;
            }
        }

    }
    public void SaveMap(MapTileTypeData mapTileTypeData, MapTileInfoData mapTileInfoData, MapTileInfoData floorTileInfoData)
    {
        Debug.Log("��ʼ�����ͼ" + mapBuildingTypeFilePath);
        FileManager.Instance.WriteFile(mapBuildingTypeFilePath, JsonConvert.SerializeObject(mapTileTypeData));
        FileManager.Instance.WriteFile(mapBuildingInfoFilePath, JsonConvert.SerializeObject(mapTileInfoData));
        FileManager.Instance.WriteFile(mapFloorTypeFilePath, JsonConvert.SerializeObject(floorTileInfoData));
    }
    public PlayerData LoadPlayerData()
    {
        Debug.Log("��ʼ��������" + actorFilePath);
        string json = FileManager.Instance.ReadFile(actorFilePath);
        if (json == "")
        {
            Debug.Log("δ��ȡ��Ŀ���ļ�");
            PlayerData playerData = new PlayerData();
            FileManager.Instance.WriteFile(actorFilePath, JsonConvert.SerializeObject(playerData));
            return null;
        }
        else
        {
            PlayerData playerData = JsonConvert.DeserializeObject<PlayerData>(json);
            if (playerData == null)
            {
                Debug.Log("����json�ļ�����ʧ��" + json);
                return null;
            }
            else
            {
                Debug.Log("����json�ļ������ɹ�");
                return playerData;
            }
        }
    }
}
