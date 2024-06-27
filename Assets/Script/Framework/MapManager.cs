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
    public Tilemap buildingMap;
    public Tilemap floorMap;
    public Grid buildingGrid;
    public Grid floorGrid;
    public NavManager navManager;

    private Dictionary<string, GameObject> TileObjPool = new Dictionary<string, GameObject>();

    private GameObject GetBuildingObj(string Name)
    {
        if (TileObjPool.ContainsKey(Name))
        {
            return TileObjPool[Name];
        }
        else
        {
            try
            {
                GameObject tileObj = Resources.Load<GameObject>("TileObj/Block/" + Name);
                if(tileObj != null)
                {
                    TileObjPool.Add(Name, tileObj);
                }
                return tileObj;
            }
            catch
            {
                return null;
            }
        }
    }
    private MyTile GetBuildingScript(string Name)
    {
        MyTile config = Resources.Load<MyTile>("TileScript/Block/" + Name);
        if (config != null)
        {
            return config;
        }
        else
        {
            return config;
        }
    }
    private MyTile GetFloorScript(string Name)
    {
        MyTile config = Resources.Load<MyTile>("TileScript/Ground/" + Name);
        if (config != null)
        {
            return config;
        }
        else
        {
            return config;
        }
    }
    /// <summary>
    /// 生成地板
    /// </summary>
    /// <param name="tilePos"></param>
    /// <param name="tileName"></param>
    public void CreateFloor(Vector3Int tilePos, string tileName)
    {
        /*创建实例*/
        MyTile tileBase = ScriptableObject.CreateInstance<MyTile>();
        /*绘制瓦片*/
        tileBase.DrawTile(GetFloorScript(tileName));
        /*将瓦片置于正确位置*/
        floorMap.SetTile(tilePos, tileBase);
    }

    /// <summary>
    /// 生成建筑
    /// </summary>
    /// <param name="tilePos"></param>
    /// <param name="tileName"></param>
    public void CreateBuilding(Vector3Int tilePos, string tileName)
    {
        /*创建实例*/
        MyTile tileBase = ScriptableObject.CreateInstance<MyTile>();
        /*初始瓦片*/
        tileBase.InitTile(buildingGrid.CellToWorld(tilePos), tilePos, GetBuildingObj(tileName), GetBuildingScript(tileName));
        /*将瓦片置于正确位置*/
        buildingMap.SetTile(tilePos, tileBase);
    }

    /// <summary>
    /// 获得瓦片
    /// </summary>
    /// <param name="tilePos"></param>
    /// <param name="tileObj"></param>
    public bool GetTileObj(Vector3Int tilePos,out TileObj tileObj)
    {
        tileObj = null;
        MyTile tile = buildingMap.GetTile<MyTile>(tilePos);
        if (tile.bindObj)
        {
            if(tile.bindObj.TryGetComponent(out TileObj obj))
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

    public void Init()
    {
        
    }
}
