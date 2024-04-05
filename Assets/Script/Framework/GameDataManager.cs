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
    /// <summary>
    /// ��ȡ��ǰ�ؿ���Ϣ
    /// </summary>
    /// <param name="mapName"></param>
    /// <param name="mapManager"></param>
    public void LoadMapData(string mapName,MapManager mapManager)
    {
        string json = FileManager.Instance.ReadFile(mapName); 
        if(json == "")
        {
            Debug.Log("δ��ȡ��Ŀ���ļ�");
            return;
        }
        MapData tiles = JsonConvert.DeserializeObject<MapData>(json);
        if( tiles == null )
        {
            Debug.Log( json );
            Debug.Log("��ͼjson�ļ�����ʧ��");
            return;
        }
        else
        {
            Debug.Log("��ͼjson�ļ������ɹ�");
            foreach (var vec2 in tiles.tiles.Keys)
            {
                string[] pos = vec2.Split(",");
                string[] info = tiles.tiles[vec2].Split("/*T*/");

                mapManager.LoadTile(new Vector3Int(int.Parse(pos[0]), int.Parse(pos[1])), info[0], info[1]);
            }
        }
    }
    /// <summary>
    /// �洢��ǰ�ؿ���Ϣ
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
