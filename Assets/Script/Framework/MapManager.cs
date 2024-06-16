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
    public Tilemap tilemap;
    public Grid grid;
    public NavManager navManager;

    private Dictionary<string, GameObject> TileObjPool = new Dictionary<string, GameObject>();

    private GameObject GetTileObj(string Name)
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
    private MyTile GetTileScript(string Name)
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
    /// <summary>
    /// 生成瓦片
    /// </summary>
    /// <param name="tilePos"></param>
    /// <param name="tileName"></param>
    public void CreateTile(Vector3Int tilePos, string tileName)
    {
        /*创建实例*/
        MyTile tileBase = ScriptableObject.CreateInstance<MyTile>();
        /*实例赋值*/
        tileBase.InstantiateTile(GetTileObj(tileName), GetTileScript(tileName));
        /*瓦片初始化*/
        tileBase.InitTile(grid.CellToWorld(tilePos), tilePos);
        /*将瓦片置于正确位置*/
        tilemap.SetTile(tilePos, tileBase);
    }
    /// <summary>
    /// 获得瓦片
    /// </summary>
    /// <param name="tilePos"></param>
    /// <param name="tileObj"></param>
    public bool GetTileObj(Vector3Int tilePos,out TileObj tileObj)
    {
        tileObj = null;
        MyTile tile = tilemap.GetTile<MyTile>(tilePos);
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
