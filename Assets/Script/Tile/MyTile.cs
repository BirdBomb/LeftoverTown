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
    [Header("瓦片ID")]
    public short config_tileID;
    [Header("瓦片通行类型")]
    public TilePassType config_passType;
    [Header("瓦片阻力系数")]
    public int config_passDrag;

    [HideInInspector]
    public Vector2 _posInWorld;
    [HideInInspector]
    public Vector3Int _posInCell;

    [HideInInspector]
    public GameObject bindObj;

    #region//寻路相关
    /// <summary>
    /// 瓦片复合阻力
    /// </summary>
    [HideInInspector]
    public int temp_PassDrag;
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
        }
    }
    /// <summary>
    /// 重设瓦片阻力
    /// </summary>
    /// <param name="offset"></param>
    public void ResetDrag(int offset)
    {
        temp_PassDrag = config_passDrag + offset;
    }
    /// <summary>
    /// 复制地块
    /// </summary>
    /// <param name="tileScript"></param>
    private void CopyData(MyTile tileScript)
    {
        name = tileScript.name;
        sprite = tileScript.sprite;
        config_tileID = tileScript.config_tileID;
        config_passType = tileScript.config_passType;
        config_passDrag = tileScript.config_passDrag;
        color = tileScript.color;
        flags = tileScript.flags;
        colliderType = tileScript.colliderType;
        base.gameObject = tileScript.gameObject;
    }

    /// <summary>
    /// 唤醒瓦片
    /// </summary>
    public virtual void ActorInputKeycode(ActorManager actor,KeyCode code)
    {
    }
    /// <summary>
    /// 靠近瓦片
    /// </summary>
    public virtual bool NearbyTileByActor(ActorManager who)
    {
        return false;
    }
    /// <summary>
    /// 站在瓦片上
    /// </summary>
    /// <param name="who"></param>
    public virtual void StandOnTileByActor(ActorManager who)
    {
    }
    /// <summary>
    /// 远离瓦片
    /// </summary>
    /// <returns></returns>
    public virtual bool FarawayTileByActor(ActorManager who)
    {
        return false;
    }
    /// <summary>
    /// 持有瓦片
    /// </summary>
    /// <param name="who"></param>
    /// <returns></returns>
    public virtual bool HoldingTileByPlayer(PlayerCoreLocal player)
    {
        return false;
    }
    /// <summary>
    /// 释放瓦片
    /// </summary>
    /// <param name="who"></param>
    /// <returns></returns>
    public virtual bool ReleaseTileByPlayer(PlayerCoreLocal player)
    {
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