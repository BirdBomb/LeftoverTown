using Fusion;
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

    /// <summary>
    /// ������ҵ�λ��
    /// </summary>
    private Dictionary<PlayerRef,Vector3Int> playerCenterDic = new Dictionary<PlayerRef,Vector3Int>();
    /// <summary>
    /// �������
    /// </summary>
    private List<PlayerRef> playerRefs = new List<PlayerRef>();
    /// <summary>
    /// �Ѿ����صĽ�������
    /// </summary>
    private List<Vector3Int> areaList_Building = new List<Vector3Int>();
    /// <summary>
    /// �Ѿ����صĵؿ�����
    /// </summary>
    private List<Vector3Int> areaList_Floor = new List<Vector3Int>();
    private Dictionary<int, MyTile> FloorTilePool = new Dictionary<int, MyTile>();
    private Dictionary<int, MyTile> BuildTilePool = new Dictionary<int, MyTile>();
    private Dictionary<int, GameObject> BuildObjPool = new Dictionary<int, GameObject>();
    private Dictionary<int, GameObject> FloorObjPool = new Dictionary<int, GameObject>();
    /// <summary>
    /// �����Ұ����
    /// </summary>
    private const float config_MaxView = 60f;
    public void Init()
    {

    }
    /*�����������*/
    #region
    /// <summary>
    /// ���µ�ͼ����������
    /// </summary>
    public void UpdatePlayerCenterInMap(PlayerRef player, Vector3Int center, int width, int height, int seed)
    {
        mapSeed = seed;
        if (playerCenterDic.ContainsKey(player)) 
        { 
            ChangePlayerCenter(player, center); 
        }
        else
        {
            AddPlayerCenter(player, center);
        }
        CheckPlayerCenter(width, height);
    }
    /// <summary>
    /// ���һ���������
    /// </summary>
    /// <param name="player"></param>
    /// <param name="center"></param>
    private void AddPlayerCenter(PlayerRef player,Vector3Int center)
    {
        Debug.Log("��ͼ�����һ���������" + player + "pos" + center);
        playerCenterDic.Add(player, center);
        playerRefs.Add(player);
    }
    /// <summary>
    /// �޸�һ���������
    /// </summary>
    /// <param name="player"></param>
    /// <param name="center"></param>
    private void ChangePlayerCenter(PlayerRef player, Vector3Int center)
    {
        playerCenterDic[player] = center;
    }
    /// <summary>
    /// ��������������
    /// </summary>
    /// <param name="player"></param>
    /// <param name="center"></param>
    private void CheckPlayerCenter(int width, int height)
    {
        List<Vector3Int> cullingList = new List<Vector3Int>();
        /*���������Ѿ����Ƶ����򲢼�¼�������κ���ҵ�����*/
        for (int i = 0; i < areaList_Floor.Count; i++)
        {
            bool culling = true;
            for (int j = 0; j < playerRefs.Count; j++)
            {
                if (Vector3Int.Distance(areaList_Floor[i], playerCenterDic[playerRefs[j]]) <= config_MaxView)
                {
                    culling = false;    
                }
            }
            if (culling)
            {
                Vector3Int temp = areaList_Building[i];
                SubAreaInMap(temp, width, height);
                cullingList.Add(temp);
            }
        }
        /*���������Ѿ����Ƶ������޳��Ѿ���¼������*/
        for (int i = 0; i < cullingList.Count; i++)
        {
            areaList_Floor.Remove(cullingList[i]);
            areaList_Building.Remove(cullingList[i]);
        }
    }
    #endregion
    /*ȥ��һ������*/
    #region
    /// <summary>
    /// ȥ��һ�����ƺõ�����
    /// </summary>
    /// <param name="center"></param>
    /// <param name="widht"></param>
    /// <param name="height"></param>
    public void SubAreaInMap(Vector3Int center, int widht, int height)
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
        MyTile tile = floorMap.GetTile<MyTile>(tilePos);
        if (tile != null)
        {
            if (tile.bindObj && tile.bindObj.TryGetComponent(out TileObj obj))
            {
                Destroy(obj.gameObject);
            }
            Destroy(tile);
        }
        floorMap.SetTile(tilePos, null);
    }

    #endregion
    /*����һ������*/
    #region
    /// <summary>
    /// ��������һ��δ���Ƶ�����
    /// </summary>
    /// <param name="center"></param>
    /// <returns>���Ի���</returns>
    public bool AddBuildingAreaInMap(Vector3Int center)
    {
        if (!areaList_Building.Contains(center))
        {
            areaList_Building.Add(center);
            return true;
        }
        return false;
    }
    /// <summary>
    /// ��������һ��δ���Ƶ�����
    /// </summary>
    /// <param name="center"></param>
    /// <returns>���Ի���</returns>
    public bool AddFloorAreaInMap(Vector3Int center)
    {
        if (!areaList_Floor.Contains(center))
        {
            areaList_Floor.Add(center);
            return true;
        }
        return false;
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
        tileBase.InitTile(floorGrid.CellToWorld(tilePos), tilePos, GetFloorObj(tileID), GetFloorScript(tileID), floorPool);
        /*����Ƭ������ȷλ��*/
        if (floorMap.HasTile(tilePos))
        {
            DeleteFloor(tilePos);
            floorMap.SetTile(tilePos, tileBase);
        }
        else
        {
            floorMap.SetTile(tilePos, tileBase);
        }
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
        tileBase.InitTile(floorGrid.CellToWorld(tilePos), tilePos, GetBuildingObj(tileID), GetBuildingScript(tileID), buildPool);
        tileBase.ResetDrag(((MyTile)floorMap.GetTile(tilePos)).config_passDrag);
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
    /// �ػ潨��
    /// </summary>
    /// <param name="tilePos"></param>
    public void ReDrawBuilding(Vector3Int tilePos)
    {
        GetBuildingObj(tilePos,out TileObj tileObj);
        if (tileObj) { tileObj.Draw(); }
    }
    #endregion
    /*��õ�ͼ��Ϣ*/
    #region
    /// <summary>
    /// ��ý���
    /// </summary>
    /// <param name="tilePos"></param>
    /// <param name="tileObj"></param>
    public bool GetBuildingObj(Vector3Int tilePos, out TileObj tileObj)
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
    /// <summary>
    /// ����ID��ȡ�ذ�obj
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private GameObject GetFloorObj(int id)
    {
        if (FloorObjPool.ContainsKey(id))
        {
            return FloorObjPool[id];
        }
        else
        {
            try
            {
                GameObject tileObj = Resources.Load<GameObject>("TileObj/Floor/" + FloorConfigData.GetItemConfig(id).Floor_FileName);
                if (tileObj != null)
                {
                    FloorObjPool.Add(id, tileObj);
                }
                return tileObj;
            }
            catch
            {
                return null;
            }
        }

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
            MyTile config = Resources.Load<MyTile>("TileScript/Floor/" + FloorConfigData.GetItemConfig(id).Floor_FileName);
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

    #endregion
}
