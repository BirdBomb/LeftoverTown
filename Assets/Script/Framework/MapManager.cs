using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

public class MapManager : SingleTon<MapManager>,ISingleTon
{
    [Header("����map")]
    public Tilemap buildingMap;
    [Header("����grid")]
    public Grid buildingGrid;
    [Header("����pool")]
    public Transform buildPool;
    [Header("�ؿ�map")]
    public Tilemap floorMap;
    [Header("�ؿ�grid")]
    public Grid floorGrid;
    [Header("�ؿ�pool")]
    public Transform floorPool;
    [Header("Ѱ·")]
    public NavManager navManager;
    [Header("�������")]
    public MapNetManager mapNetManager;
    [HideInInspector]
    public int mapSeed;
    private List<Vector3Int> mapAreaList = new List<Vector3Int>();
    private Dictionary<int, MyTile> FloorTilePool = new Dictionary<int, MyTile>();
    private Dictionary<int, MyTile> BuildTilePool = new Dictionary<int, MyTile>();
    private Dictionary<int, GameObject> BuildObjPool = new Dictionary<int, GameObject>();

    public void Init()
    {

    }
    /// <summary>
    /// ���µ�ͼ����
    /// </summary>
    public void UpdateMapCenter(Vector3Int center, int width, int height, int seed)
    {
        Debug.Log("��ʼ����ͼ����" + seed);
        mapSeed = seed;
        List<Vector3Int> cullingList = new List<Vector3Int>();
        /*���������Ѿ����Ƶ����򲢼�¼�����Ĺ�Զ������*/
        for (int i = 0; i < mapAreaList.Count; i++)
        {
            if (Vector3Int.Distance(mapAreaList[i], center) > 60)
            {
                Vector3Int temp = mapAreaList[i];
                SubNewAreaInMap(temp, width, height);
                cullingList.Add(temp);
            }
        }
        /*���������Ѿ����Ƶ������޳��Ѿ���¼������*/
        for (int i = 0; i < cullingList.Count; i++)
        {
            mapAreaList.Remove(cullingList[i]);
        }
    }
    /// <summary>
    /// ���һ�����ƺõ�����
    /// </summary>
    public bool AddNewAreaInMap(Vector3Int center,int widht,int height)
    {
        if (!mapAreaList.Contains(center))
        {
            mapAreaList.Add(center);
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// ȥ��һ�����ƺõ�����
    /// </summary>
    /// <param name="center"></param>
    /// <param name="widht"></param>
    /// <param name="height"></param>
    public void SubNewAreaInMap(Vector3Int center,int widht,int height)
    {
        int index = 0;
        for (int x = -widht / 2; x < widht / 2; x++)
        {
            for (int y = -height / 2; y < height / 2; y++)
            {
                DeleteBuilding(new Vector3Int(center.x + x, center.y + y, 0));
                DeleteFloor(new Vector3Int(center.x + x, center.y + y, 0));
                index++;
            }
        }

    }
    /// <summary>
    /// ɾ������
    /// </summary>
    /// <param name="tilePos"></param>
    public void DeleteBuilding(Vector3Int tilePos)
    {
        MyTile tile = buildingMap.GetTile<MyTile>(tilePos);
        if (tile != null)
        {
            if (tile.bindObj)
            {
                if (tile.bindObj.TryGetComponent(out TileObj obj))
                {
                    Destroy(obj.gameObject);
                }
            }
        }
        buildingMap.SetTile(tilePos, null);
    }
    /// <summary>
    /// ɾ���ؿ�
    /// </summary>
    /// <param name="tilePos"></param>
    public void DeleteFloor(Vector3Int tilePos)
    {
        if (floorMap.HasTile(tilePos))
        {
            Destroy(floorMap.GetTile(tilePos));
            floorMap.SetTile(tilePos, null);
        }
    }
    /// <summary>
    /// ���ɵذ�
    /// </summary>
    /// <param name="tilePos"></param>
    /// <param name="tileName"></param>
    public void CreateFloor(Vector3Int tilePos, int tileID)
    {
        /*����ʵ��*/
        MyTile tileBase = ScriptableObject.CreateInstance<MyTile>();
        /*������Ƭ*/
        tileBase.DrawTile(GetFloorScript(tileID));
        /*����Ƭ������ȷλ��*/
        floorMap.SetTile(tilePos, tileBase);
    }
    /// <summary>
    /// ���ɽ���
    /// </summary>
    /// <param name="tilePos"></param>
    /// <param name="tileName"></param>
    public void CreateBuilding(Vector3Int tilePos, int tileID)
    {
        /*����ʵ��*/
        MyTile tileBase = ScriptableObject.CreateInstance<MyTile>();
        /*��ʼ��Ƭ*/
        tileBase.InitTile(buildingGrid.CellToWorld(tilePos), tilePos, GetBuildingObj(tileID), GetBuildingScript(tileID),buildPool);
        /*����Ƭ������ȷλ��*/
        if (buildingMap.HasTile(tilePos))
        {
            DeleteBuilding(tilePos);
            buildingMap.SetTile(tilePos, tileBase);
        }
        else
        {
            buildingMap.SetTile(tilePos, tileBase);
        }
    }
    /// <summary>
    /// �����Ƭ
    /// </summary>
    /// <param name="tilePos"></param>
    /// <param name="tileObj"></param>
    public bool GetBuildObj(Vector3Int tilePos,out TileObj tileObj)
    {
        tileObj = null;
        MyTile tile = buildingMap.GetTile<MyTile>(tilePos);
        if (tile && tile.bindObj)
        {
            if (tile.bindObj.TryGetComponent(out TileObj obj))
            {
                tileObj = obj;
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// �޸���Ƭ
    /// </summary>
    public void ChangeTileObj(Vector3Int tilePos)
    {

    }
    /// <summary>
    /// �������ֻ�ȡ�ؿ�tile
    /// </summary>
    /// <param name="Name"></param>
    /// <returns></returns>
    private MyTile GetFloorScript(int id)
    {
        if (FloorTilePool.ContainsKey(id))
        {
            return FloorTilePool[id];
        }
        else
        {
            MyTile config = Resources.Load<MyTile>("TileScript/Ground/" + FloorConfigData.GetItemConfig(id).Floor_FileName);
            if (config != null)
            {
                FloorTilePool.Add(id, config);
                return config;
            }
            else
            {
                return config;
            }
        }
    }
    /// <summary>
    /// �������ֻ�ȡ����tile
    /// </summary>
    /// <param name="Name"></param>
    /// <returns></returns>
    private MyTile GetBuildingScript(int id)
    {
        if (BuildTilePool.ContainsKey(id))
        {
            return BuildTilePool[id];
        }
        else
        {
            MyTile config = Resources.Load<MyTile>("TileScript/Block/" + BuildingConfigData.GetItemConfig(id).Building_FileName);
            if (config != null)
            {
                BuildTilePool.Add(id, config);
                return config;
            }
            else
            {
                return config;
            }
        }

    }
    /// <summary>
    /// ����ID��ȡ����obj
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private GameObject GetBuildingObj(int id)
    {
        if (BuildObjPool.ContainsKey(id))
        {
            return BuildObjPool[id];
        }
        else
        {
            try
            {
                GameObject tileObj = Resources.Load<GameObject>("TileObj/Block/" + BuildingConfigData.GetItemConfig(id).Building_FileName);
                if (tileObj != null)
                {
                    BuildObjPool.Add(id, tileObj);
                }
                return tileObj;
            }
            catch
            {
                return null;
            }
        }
    }
}
