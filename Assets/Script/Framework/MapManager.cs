using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UniRx;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class MapManager : MonoBehaviour
{
    public Tilemap tilemap;
    public Grid grid;
    public NavManager navManager;

    private Dictionary<string, MyTile> TilePool = new Dictionary<string, MyTile>();


    private void Start()
    {
        MessageBroker.Default.Receive<MapEvent.MapEvent_SaveMap>().Subscribe(_ =>
        {
            SaveMap();
        }).AddTo(this);
        LoadMap();
    }
    private void InitMap()
    {
        BoundsInt bounds = tilemap.cellBounds;
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                LoadTile(new Vector3Int(x, y, 0),"Default","");
            }
        }
    }
    public MyTile GetTileAsset(string Name)
    {
        if (TilePool.ContainsKey(Name)) 
        { 
            return TilePool[Name]; 
        }
        else
        {
            MyTile tile = Resources.Load<MyTile>("TileScript/Block/" + Name);
            TilePool.Add(Name, tile);
            return tile;
        }
    }
    public MyTile GetTileScriptObj(string Name)
    {
        return (MyTile)ScriptableObject.CreateInstance(Name);
    }
    public void LoadTile(Vector3Int pos,string tileName,string tileInfo)
    {
        //Debug.Log(tileName);
        //Debug.Log(GetTileAsset(tileName).GetType());

        MyTile tileBase = (MyTile)Activator.CreateInstance(GetTileAsset(tileName).GetType());
        tileBase.CopyTile(GetTileAsset(tileName));

        tileBase.InitTile(pos.x, pos.y, grid.CellToWorld(pos));
        tilemap.SetTile(pos, tileBase);
        if (tileInfo != null)
        {
            tileBase.LoadTile(tileInfo);
        }

    }
    public string SaveTile(Vector3Int pos)
    {
        MyTile tile = tilemap.GetTile<MyTile>(pos);
        if( tile == null )
        {
            return "Default,";
        }
        else
        {
            Debug.Log(tile.name);
            return tile.name + "/*T*/" + tile.SaveTile();
        }
    }

    public void SaveMap()
    {
        GameDataManager.Instance.SaveMapData("Test", this);
    }
    public void LoadMap()
    {
        GameDataManager.Instance.LoadMapData("Test", this);
    }
}
