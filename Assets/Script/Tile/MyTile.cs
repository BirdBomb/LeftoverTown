using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
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
    public Vector2 _posInWorld;
    [HideInInspector]
    public Vector3Int _posInCell;

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

    /// <summary>
    /// 重置路径信息
    /// </summary>
    public void ResetTilePathInfo()
    {
        _temp_DistanceToFrom = 0;
        _temp_DistanceToTarget = 0;
        _temp_DistanceMain = 0;

        _temp_fatherTile = null;
    }
    #endregion
    /// <summary>
    /// 初始地块
    /// </summary>
    public virtual void InitTile(Vector2 posInWorld, Vector3Int posInCell, GameObject tileObj, MyTile tileScript,Transform pool)
    {
        _posInWorld = posInWorld + new Vector2(0.5f, 0.5f);
        _posInCell = posInCell;

        CopyData(tileScript);

        if (tileObj)
        {
            bindObj = Instantiate(tileObj, pool);
            bindObj.transform.position = posInWorld + new Vector2(0.5f, 0.5f);
            if (bindObj.TryGetComponent(out TileObj tile))
            {
                tile.Init(this);
            }
        }
    }
    /// <summary>
    /// 复制地块
    /// </summary>
    /// <param name="tileScript"></param>
    private void CopyData(MyTile tileScript)
    {
        name = tileScript.name;
        passType = tileScript.passType;
        interactiveType = tileScript.interactiveType;
        passOffset = tileScript.passOffset;
        color = tileScript.color;
        flags = tileScript.flags;
        colliderType = tileScript.colliderType;
    }

    /// <summary>
    /// 唤醒瓦片
    /// </summary>
    public virtual void InvokeTile(PlayerController player,KeyCode code)
    {
        if (bindObj && bindObj.TryGetComponent(out TileObj obj))
        {
            obj.Invoke(player, code);
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
    /// <summary>
    /// 持有瓦片
    /// </summary>
    /// <param name="who"></param>
    /// <returns></returns>
    public virtual bool HoldingTileByPlayer(PlayerController who)
    {
        if (bindObj && bindObj.TryGetComponent(out TileObj obj))
        {
            return obj.PlayerHolding(who);
        }
        return false;
    }
    /// <summary>
    /// 释放瓦片
    /// </summary>
    /// <param name="who"></param>
    /// <returns></returns>
    public virtual bool ReleaseTileByPlayer(PlayerController who)
    {
        if (bindObj && bindObj.TryGetComponent(out TileObj obj))
        {
            return obj.PlayerRelease(who);
        }
        return false;
    }

    #endregion
    #region//弃用
    //public Sprite[] m_Sprites;
    //public Sprite m_Preview;
    ///// <summary>
    ///// 绘制地块
    ///// </summary>
    ///// <param name="tileConfig"></param>
    ///// <returns></returns>
    //public virtual void DrawTile(MyTile tileConfig)
    //{
    //    if (tileConfig.m_Sprites != null && tileConfig.m_Sprites.Length > 0)
    //    {
    //        sprite = tileConfig.m_Sprites[UnityEngine.Random.Range(0, tileConfig.m_Sprites.Length)];
    //    }
    //    else
    //    {
    //        sprite = tileConfig.sprite;
    //    }
    //}
    ///// <summary>
    ///// 读取瓦片
    ///// </summary>
    //public virtual void LoadTile(string json)
    //{
    //    if (bindObj && bindObj.TryGetComponent(out TileObj obj))
    //    {
    //        obj.Init(this);
    //        obj.Load(json);
    //    }
    //}
    ///// <summary>
    ///// 保存瓦片
    ///// </summary>
    //public virtual void SaveTile(out string json)
    //{
    //    if (bindObj && bindObj.TryGetComponent(out TileObj obj))
    //    {
    //        obj.Save(out json);
    //    }
    //    else
    //    {
    //        json = string.Empty;
    //    }
    //}
    ///// <summary>
    ///// 更新瓦片
    ///// </summary>
    //public virtual void UpdateTile(string json)
    //{

    //}

    //// 刷新自身以及其他以正交和对角形式相邻的 RoadTile
    //public override void RefreshTile(Vector3Int location, ITilemap tilemap)
    //{
    //    for (int yd = -1; yd <= 1; yd++)
    //        for (int xd = -1; xd <= 1; xd++)
    //        {
    //            Vector3Int position = new Vector3Int(location.x + xd, location.y + yd, location.z);
    //            if (HasRoadTile(tilemap, position))
    //                tilemap.RefreshTile(position);
    //        }
    //}
    //// 根据相邻的 RoadTile 确定使用哪个精灵，并将其旋转以适应其他瓦片。
    ////由于旋转取决于 RoadTile，因此为瓦片设置 TileFlags.OverrideTransform。
    //public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
    //{
    //    int mask = HasRoadTile(tilemap, location + new Vector3Int(0, 1, 0)) ? 1 : 0;
    //    mask += HasRoadTile(tilemap, location + new Vector3Int(1, 0, 0)) ? 2 : 0;
    //    mask += HasRoadTile(tilemap, location + new Vector3Int(0, -1, 0)) ? 4 : 0;
    //    mask += HasRoadTile(tilemap, location + new Vector3Int(-1, 0, 0)) ? 8 : 0;
    //    int index = GetIndex((byte)mask);
    //    if (index >= 0 && index < m_Sprites.Length)
    //    {
    //        tileData.sprite = m_Sprites[index];
    //        tileData.color = Color.white;
    //        var m = tileData.transform;
    //        m.SetTRS(Vector3.zero, GetRotation((byte)mask), Vector3.one);
    //        tileData.transform = m;
    //        tileData.flags = TileFlags.LockTransform;
    //        tileData.colliderType = ColliderType.None;
    //    }
    //    else
    //    {
    //        Debug.LogWarning("Not enough sprites in RoadTile instance");
    //    }
    //}
    //// 这将确定该位置的瓦片是否是相同的 RoadTile。
    //private bool HasRoadTile(ITilemap tilemap, Vector3Int position)
    //{
    //    return tilemap.GetTile(position) == this;
    //}
    ////下面根据相邻 RoadTile 的数量确定要使用的精灵
    //private int GetIndex(byte mask)
    //{
    //    switch (mask)
    //    {
    //        case 0: return 0;
    //        case 3:
    //        case 6:
    //        case 9:
    //        case 12: return 1;
    //        case 1:
    //        case 2:
    //        case 4:
    //        case 5:
    //        case 10:
    //        case 8: return 2;
    //        case 7:
    //        case 11:
    //        case 13:
    //        case 14: return 3;
    //        case 15: return 4;
    //    }
    //    return -1;
    //}
    //// 下面根据相邻 RoadTile 的位置确定要使用的旋转
    //private Quaternion GetRotation(byte mask)
    //{
    //    switch (mask)
    //    {
    //        case 9:
    //        case 10:
    //        case 7:
    //        case 2:
    //        case 8:
    //            return Quaternion.Euler(0f, 0f, -90f);
    //        case 3:
    //        case 14:
    //            return Quaternion.Euler(0f, 0f, -180f);
    //        case 6:
    //        case 13:
    //            return Quaternion.Euler(0f, 0f, -270f);
    //    }
    //    return Quaternion.Euler(0f, 0f, 0f);
    //}
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