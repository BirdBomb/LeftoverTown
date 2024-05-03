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
    public string actorFilePath;
    /// <summary>
    /// ��ͼ�ļ�·��
    /// </summary>
    public string mapFilePath;

    public void Init()
    {
        
    }
    public MapData LoadMap()
    {
        Debug.Log("��ʼ���ص�ͼ" + mapFilePath);
        string json = FileManager.Instance.ReadFile(mapFilePath);
        if (json == "")
        {
            Debug.Log("δ��ȡ��Ŀ���ļ�");
            return null;
        }
        else
        {
            MapData tiles = JsonConvert.DeserializeObject<MapData>(json);
            if (tiles == null)
            {
                Debug.Log("��ͼjson�ļ�����ʧ��" + json);
                return null;
            }
            else
            {
                Debug.Log("��ͼjson�ļ������ɹ�");
                return tiles;
            }

        }
    }
    public void SaveMap(MapData mapData)
    {

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
