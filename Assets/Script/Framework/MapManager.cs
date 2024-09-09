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
    [Header("建筑map")]
    public Tilemap buildingMap;
    [Header("建筑grid")]
    public Grid buildingGrid;
    [Header("建筑pool")]
    public Transform buildPool;
    [Header("地块map")]
    public Tilemap floorMap;
    [Header("地块grid")]
    public Grid floorGrid;
    [Header("地块pool")]
    public Transform floorPool;
    [Header("寻路")]
    public NavManager navManager;
    [Header("网络组件")]
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
    /// 更新地图中心
    /// </summary>
    public void UpdateMapCenter(Vector3Int center, int width, int height, int seed)
    {
        Debug.Log("初始化地图种子" + seed);
        mapSeed = seed;
        List<Vector3Int> cullingList = new List<Vector3Int>();
        /*遍历所有已经绘制的区域并记录离中心过远的区域*/
        for (int i = 0; i < mapAreaList.Count; i++)
        {
            if (Vector3Int.Distance(mapAreaList[i], center) > 60)
            {
                Vector3Int temp = mapAreaList[i];
                SubNewAreaInMap(temp, width, height);
                cullingList.Add(temp);
            }
        }
        /*遍历所有已经绘制的区域并剔除已经记录的区域*/
        for (int i = 0; i < cullingList.Count; i++)
        {
            mapAreaList.Remove(cullingList[i]);
        }
    }
    /// <summary>
    /// 添加一个绘制好的区域
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
    /// 去除一个绘制好的区域
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
    /// 删除建筑
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
    /// 删除地块
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
    /// 生成地板
    /// </summary>
    /// <param name="tilePos"></param>
    /// <param name="tileName"></param>
    public void CreateFloor(Vector3Int tilePos, int tileID)
    {
        /*创建实例*/
        MyTile tileBase = ScriptableObject.CreateInstance<MyTile>();
        /*绘制瓦片*/
        tileBase.DrawTile(GetFloorScript(tileID));
        /*将瓦片置于正确位置*/
        floorMap.SetTile(tilePos, tileBase);
    }
    /// <summary>
    /// 生成建筑
    /// </summary>
    /// <param name="tilePos"></param>
    /// <param name="tileName"></param>
    public void CreateBuilding(Vector3Int tilePos, int tileID)
    {
        /*创建实例*/
        MyTile tileBase = ScriptableObject.CreateInstance<MyTile>();
        /*初始瓦片*/
        tileBase.InitTile(buildingGrid.CellToWorld(tilePos), tilePos, GetBuildingObj(tileID), GetBuildingScript(tileID),buildPool);
        /*将瓦片置于正确位置*/
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
    /// 获得瓦片
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
    /// 修改瓦片
    /// </summary>
    public void ChangeTileObj(Vector3Int tilePos)
    {

    }
    /// <summary>
    /// 根据名字获取地块tile
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
    /// 根据名字获取建筑tile
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
    /// 根据ID获取建筑obj
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
