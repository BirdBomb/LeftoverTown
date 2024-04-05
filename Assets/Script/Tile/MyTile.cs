using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TreeEditor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;
using static UnityEngine.Tilemaps.Tile;

[System.Serializable]
public class MyTile : UnityEngine.Tilemaps.Tile
{
    #region
    [Header("瓦片通行类型")]
    public TilePassType passType;
    [Header("瓦片交互类型")]
    public TileInteractiveType interactiveType;
    [Header("瓦片阻力")]
    public float passOffset;

    [HideInInspector]
    public int x;
    [HideInInspector]
    public int y;
    [HideInInspector]
    public Vector2 pos;
    [HideInInspector]
    public GameObject bindObj;
    #region//寻路相关
    /// <summary>
    /// 总权重
    /// </summary>
    [HideInInspector]
    public float _temp_DistanceMain;
    /// <summary>
    /// 距离起点的距离
    /// </summary>
    [HideInInspector]
    public float _temp_DistanceToFrom;
    /// <summary>
    /// 距离终点的距离
    /// </summary>
    [HideInInspector]
    public float _temp_DistanceToTarget;
    /// <summary>
    /// 上一个瓦片
    /// </summary>
    [HideInInspector]
    public MyTile _temp_fatherTile;

    public void ResetTilePathInfo()
    {
        _temp_DistanceToFrom = 0;
        _temp_DistanceToTarget = 0;
        _temp_DistanceMain = 0;

        _temp_fatherTile = null;
    }
    #endregion
    /// <summary>
    /// 实例瓦片
    /// </summary>
    public virtual void InstantiateTile(GameObject tileObj,MyTile tileScript)
    {
        name = tileScript.name;

        passType = tileScript.passType;
        interactiveType = tileScript.interactiveType;
        passOffset = tileScript.passOffset;

        sprite = tileScript.sprite;
        color = tileScript.color;
        flags = tileScript.flags;
        colliderType = tileScript.colliderType;

        if (tileObj)
        {
            bindObj = GameObject.Instantiate(tileObj);
        }
    }
    /// <summary>
    /// 初始瓦片
    /// </summary>
    public virtual void InitTile(int x,int y,Vector2 pos)
    {
        this.x = x;
        this.y = y;
        this.pos = pos;
        if (bindObj)
        {
            bindObj.transform.position = pos + new Vector2(0.5f,0.5f);
        }
    }
    /// <summary>
    /// 读取瓦片
    /// </summary>
    public virtual void LoadTile(string json)
    {
        if(bindObj && bindObj.TryGetComponent(out TileObj obj))
        {
            obj.Init(this); 
            obj.Load(json);
        }
    }
    /// <summary>
    /// 保存瓦片
    /// </summary>
    public virtual void SaveTile(out string json)
    {
        if (bindObj && bindObj.TryGetComponent(out TileObj obj))
        {
            obj.Save(out json);
        }
        else
        {
            json = string.Empty;
        }
    }
    /// <summary>
    /// 更新瓦片
    /// </summary>
    public virtual void UpdateTile(string json)
    {

    }

    /// <summary>
    /// 唤醒瓦片
    /// </summary>
    public virtual void InvokeTile()
    {
        if (bindObj && bindObj.TryGetComponent(out TileObj obj))
        {
            obj.Invoke();
        }
    }
    /// <summary>
    /// 靠近瓦片
    /// </summary>
    public virtual bool NearbyTileByPlayer(PlayerController who)
    {
        if (bindObj && bindObj.TryGetComponent(out TileObj obj))
        {
            return obj.PlayerNearby(who);
        }
        return false;
    }
    /// <summary>
    /// 远离瓦片
    /// </summary>
    /// <returns></returns>
    public virtual bool FarawayTileByPlayer(PlayerController who)
    {
        if (bindObj && bindObj.TryGetComponent(out TileObj obj))
        {
            return obj.PlayerFaraway(who);
        }
        return false;
    }
    #endregion
#if UNITY_EDITOR
    // 下面是添加菜单项以创建 RoadTile 资源的 helper 函数
    [MenuItem("Assets/Create/MyTile/Base")]
    public static void CreateBaseTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Road Tile", "New Road Tile", "Asset", "Save Road Tile", "Assets");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<MyTile>(), path);
    }
# endif
}
[System.Serializable]
public struct MyTileData
{
    string TileSprite;
    string TileScript;
}
public enum TilePassType
{
    /// <summary>
    /// 正常格子
    /// </summary>
    PassNormal,
    /// <summary>
    /// 应急格子
    /// </summary>
    PassEmergency,
    /// <summary>
    /// 障碍格子
    PassStop,
}
public enum TileInteractiveType
{
    None,
    Cabinet,
}