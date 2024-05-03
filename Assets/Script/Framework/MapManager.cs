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
                    Debug.Log(Name);
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
    /// <param name="tilePos"></param>
    /// <param name="tileName"></param>
    public void CreateTile(Vector3Int tilePos, string tileName)
    {
        /*����ʵ��*/
        MyTile tileBase = ScriptableObject.CreateInstance<MyTile>();
        /*ʵ����ֵ*/
        tileBase.InstantiateTile(GetTileObj(tileName), GetTileScript(tileName));
        /*��Ƭ��ʼ��*/
        tileBase.InitTile(grid.CellToWorld(tilePos), tilePos);
        /*����Ƭ������ȷλ��*/
        tilemap.SetTile(tilePos, tileBase);
    }
    /// <summary>
    /// �����Ƭ
    /// </summary>
    /// <param name="tilePos"></param>
    /// <param name="tileObj"></param>
    public void GetTileObj(Vector3Int tilePos,out TileObj tileObj)
    {
        MyTile tile = tilemap.GetTile<MyTile>(tilePos);
        Debug.Log(tilePos);
        tileObj = tile.bindObj.GetComponent<TileObj>();
    }
    /// <summary>
    /// �޸���Ƭ
    /// </summary>
    public void ChangeTileObj(Vector3Int tilePos)
    {

    }
}
