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

    /// <summary>
    /// 所有玩家的位置
    /// </summary>
    private Dictionary<PlayerRef,Vector3Int> playerCenterDic = new Dictionary<PlayerRef,Vector3Int>();
    /// <summary>
    /// 所有玩家
    /// </summary>
    private List<PlayerRef> playerRefs = new List<PlayerRef>();
    /// <summary>
    /// 已经加载的建筑区域
    /// </summary>
    private List<Vector3Int> areaList_Building = new List<Vector3Int>();
    /// <summary>
    /// 已经加载的地块区域
    /// </summary>
    private List<Vector3Int> areaList_Floor = new List<Vector3Int>();
    private Dictionary<int, MyTile> FloorTilePool = new Dictionary<int, MyTile>();
    private Dictionary<int, MyTile> BuildTilePool = new Dictionary<int, MyTile>();
    private Dictionary<int, GameObject> BuildObjPool = new Dictionary<int, GameObject>();
    private Dictionary<int, GameObject> FloorObjPool = new Dictionary<int, GameObject>();
    /// <summary>
    /// 最大视野距离
    /// </summary>
    private const float config_MaxView = 60f;
    public void Init()
    {

    }
    /*调整玩家中心*/
    #region
    /// <summary>
    /// 更新地图里的玩家中心
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
    /// 添加一个玩家中心
    /// </summary>
    /// <param name="player"></param>
    /// <param name="center"></param>
    private void AddPlayerCenter(PlayerRef player,Vector3Int center)
    {
        Debug.Log("地图里添加一个玩家中心" + player + "pos" + center);
        playerCenterDic.Add(player, center);
        playerRefs.Add(player);
    }
    /// <summary>
    /// 修改一个玩家中心
    /// </summary>
    /// <param name="player"></param>
    /// <param name="center"></param>
    private void ChangePlayerCenter(PlayerRef player, Vector3Int center)
    {
        playerCenterDic[player] = center;
    }
    /// <summary>
    /// 检查所有玩家中心
    /// </summary>
    /// <param name="player"></param>
    /// <param name="center"></param>
    private void CheckPlayerCenter(int width, int height)
    {
        List<Vector3Int> cullingList = new List<Vector3Int>();
        /*遍历所有已经绘制的区域并记录不靠近任何玩家的区域*/
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
        /*遍历所有已经绘制的区域并剔除已经记录的区域*/
        for (int i = 0; i < cullingList.Count; i++)
        {
            areaList_Floor.Remove(cullingList[i]);
            areaList_Building.Remove(cullingList[i]);
        }
    }
    #endregion
    /*去除一个区域*/
    #region
    /// <summary>
    /// 去除一个绘制好的区域
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
    /*增加一个区域*/
    #region
    /// <summary>
    /// 尝试增加一个未绘制的区域
    /// </summary>
    /// <param name="center"></param>
    /// <returns>可以绘制</returns>
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
    /// 尝试增加一个未绘制的区域
    /// </summary>
    /// <param name="center"></param>
    /// <returns>可以绘制</returns>
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
    /// 生成地板
    /// </summary>
    /// <param name="tilePos"></param>
    /// <param name="tileName"></param>
    public void CreateFloor(Vector3Int tilePos, int tileID)
    {
        /*创建实例*/
        MyTile tileBase = ScriptableObject.CreateInstance<MyTile>();
        tileBase.InitTile(floorGrid.CellToWorld(tilePos), tilePos, GetFloorObj(tileID), GetFloorScript(tileID), floorPool);
        /*将瓦片置于正确位置*/
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
    /// 生成建筑
    /// </summary>
    /// <param name="tilePos"></param>
    /// <param name="tileName"></param>
    public void CreateBuilding(Vector3Int tilePos, int tileID)
    {
        /*创建实例*/
        MyTile tileBase = ScriptableObject.CreateInstance<MyTile>();
        tileBase.InitTile(floorGrid.CellToWorld(tilePos), tilePos, GetBuildingObj(tileID), GetBuildingScript(tileID), buildPool);
        tileBase.ResetDrag(((MyTile)floorMap.GetTile(tilePos)).config_passDrag);
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
    /// 重绘建筑
    /// </summary>
    /// <param name="tilePos"></param>
    public void ReDrawBuilding(Vector3Int tilePos)
    {
        GetBuildingObj(tilePos,out TileObj tileObj);
        if (tileObj) { tileObj.Draw(); }
    }
    #endregion
    /*获得地图信息*/
    #region
    /// <summary>
    /// 获得建筑
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
    /// <summary>
    /// 根据ID获取地板obj
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

    #endregion
}
