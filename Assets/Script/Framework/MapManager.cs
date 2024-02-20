using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UniRx;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

public class MapManager : MonoBehaviour
{
    public Tilemap tilemap;
    public Grid grid;
    public NavManager navManager;

    private Dictionary<string, GameObject> TileObjPool = new Dictionary<string, GameObject>();


    private void Start()
    {
        MessageBroker.Default.Receive<MapEvent.MapEvent_SaveMap>().Subscribe(_ =>
        {
            SaveMap();
        }).AddTo(this);
        MessageBroker.Default.Receive<MapEvent.MapEvent_ChangeTile>().Subscribe(_ =>
        {
            ChangeTile(_.tilePos, _.tileName);
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
    private GameObject GetTileObj(string Name)
    {
        Debug.Log(Name);
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
    /// ������Ƭ
    /// </summary>
    /// <param name="tilePos">��Ƭλ��</param>
    /// <param name="tileName">��Ƭ����</param>
    /// <param name="tileInfo">��Ƭ��Ϣ</param>
    public void LoadTile(Vector3Int tilePos,string tileName, string tileInfo)
    {
        /*����ʵ��*/
        MyTile tileBase = ScriptableObject.CreateInstance<MyTile>();
        /*ʵ����ֵ*/
        tileBase.InstantiateTile(GetTileObj(tileName),GetTileScript(tileName));
        /*��Ƭ��ʼ��*/
        tileBase.InitTile(tilePos.x, tilePos.y, grid.CellToWorld(tilePos));
        /*����Ƭ������ȷλ��*/
        tilemap.SetTile(tilePos, tileBase);
        if (tileInfo != null)
        {
            /*��Ƭ��Ϣ��Ϊ��ʱ��������Ƭ��Ϣ*/
            tileBase.LoadTile(tileInfo);
        }
    }
    /// <summary>
    /// ������Ƭ
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="tileJson"></param>
    public void SaveTile(Vector3Int pos,out string tileJson)
    {
        MyTile tile = tilemap.GetTile<MyTile>(pos);
        if( tile == null )
        {
            tileJson = "Default,";
        }
        else
        {
            tile.SaveTile(out string info);
            tileJson = tile.name + "/*T*/" + info;
        }
    }
    public void ChangeTile(Vector3Int tilePos, string tileName)
    {
        if (tilemap.GetTile<MyTile>(tilePos)!= null)
        {
            Destroy(tilemap.GetTile<MyTile>(tilePos).bindObj);
        }
        /*����ʵ��*/
        MyTile tileBase = ScriptableObject.CreateInstance<MyTile>();
        /*ʵ����ֵ*/
        tileBase.InstantiateTile(GetTileObj(tileName), GetTileScript(tileName));
        /*��Ƭ��ʼ��*/
        tileBase.InitTile(tilePos.x, tilePos.y, grid.CellToWorld(tilePos));
        /*����Ƭ������ȷλ��*/
        tilemap.SetTile(tilePos, tileBase);
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
