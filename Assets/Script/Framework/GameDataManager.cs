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
    public string mapTileTypeFilePath{ get; set; }
    public string mapTileInfoFilePath { get; set; }
    public void Init()
    {
        
    }
    public void LoadMap(out MapTileTypeData mapTileTypeData,out MapTileInfoData mapTileInfoData)
    {
        Debug.Log("��ʼ���ص�ͼ(���)" + mapTileTypeFilePath);
        Debug.Log("��ʼ���ص�ͼ(��Ϣ)" + mapTileInfoFilePath);
        string typeJson = FileManager.Instance.ReadFile(mapTileTypeFilePath);
        if (typeJson == "")
        {
            Debug.Log("δ��ȡ��Ŀ���ļ�");
            mapTileTypeData = new MapTileTypeData();
            FileManager.Instance.WriteFile(mapTileTypeFilePath, JsonConvert.SerializeObject(mapTileTypeData));
        }
        else
        {
            MapTileTypeData tiles = JsonConvert.DeserializeObject<MapTileTypeData>(typeJson);
            if (tiles == null)
            {
                Debug.Log("��ͼjson�ļ�����ʧ��" + typeJson);
                mapTileTypeData = new MapTileTypeData();
                FileManager.Instance.WriteFile(mapTileTypeFilePath, JsonConvert.SerializeObject(mapTileTypeData));
            }
            else
            {
                Debug.Log("��ͼjson�ļ������ɹ�");
                mapTileTypeData = tiles;
            }

        }

        string infoJson = FileManager.Instance.ReadFile(mapTileInfoFilePath);
        if (infoJson == "")
        {
            Debug.Log("δ��ȡ��Ŀ���ļ�");
            mapTileInfoData = new MapTileInfoData();
            FileManager.Instance.WriteFile(mapTileInfoFilePath, JsonConvert.SerializeObject(mapTileInfoData));
        }
        else
        {
            MapTileInfoData tiles = JsonConvert.DeserializeObject<MapTileInfoData>(infoJson);
            if (tiles == null)
            {
                Debug.Log("��ͼjson�ļ�����ʧ��" + infoJson);
                mapTileInfoData = new MapTileInfoData();
                FileManager.Instance.WriteFile(mapTileInfoFilePath, JsonConvert.SerializeObject(mapTileInfoData));
            }
            else
            {
                Debug.Log("��ͼjson�ļ������ɹ�");
                mapTileInfoData = tiles;
            }
        }

    }
    public void SaveMap(MapTileTypeData mapTileTypeData, MapTileInfoData mapTileInfoData)
    {
        Debug.Log("��ʼ�����ͼ" + mapTileTypeFilePath);
        FileManager.Instance.WriteFile(mapTileTypeFilePath, JsonConvert.SerializeObject(mapTileTypeData));
        FileManager.Instance.WriteFile(mapTileInfoFilePath, JsonConvert.SerializeObject(mapTileInfoData));
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
