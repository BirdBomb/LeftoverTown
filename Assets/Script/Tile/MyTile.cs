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

    [Header("��Ƭͨ������")]
    public TilePassType passType;
    [Header("��Ƭ��������")]
    public TileInteractiveType interactiveType;
    [Header("��Ƭ����")]
    public float passOffset;

    [HideInInspector]
    public Vector2 _posInWorld;
    [HideInInspector]
    public Vector3Int _posInCell;

    [HideInInspector]
    public GameObject bindObj;

    #region//Ѱ·���
    /// <summary>
    /// ��Ȩ��
    /// </summary>
    [HideInInspector]
    public float _temp_DistanceMain;
    /// <summary>
    /// �������ľ���
    /// </summary>
    [HideInInspector]
    public float _temp_DistanceToFrom;
    /// <summary>
    /// �����յ�ľ���
    /// </summary>
    [HideInInspector]
    public float _temp_DistanceToTarget;
    /// <summary>
    /// ��һ����Ƭ
    /// </summary>
    [HideInInspector]
    public MyTile _temp_fatherTile;

    /// <summary>
    /// ����·����Ϣ
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
    /// ��ʼ�ؿ�
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
    /// ���Ƶؿ�
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
    /// ������Ƭ
    /// </summary>
    public virtual void InvokeTile(PlayerController player,KeyCode code)
    {
        if (bindObj && bindObj.TryGetComponent(out TileObj obj))
        {
            obj.Invoke(player, code);
        }
    }
    /// <summary>
    /// ������Ƭ
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
    /// Զ����Ƭ
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
    /// ������Ƭ
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
    /// �ͷ���Ƭ
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
    #region//����
    //public Sprite[] m_Sprites;
    //public Sprite m_Preview;
    ///// <summary>
    ///// ���Ƶؿ�
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
    ///// ��ȡ��Ƭ
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
    ///// ������Ƭ
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
    ///// ������Ƭ
    ///// </summary>
    //public virtual void UpdateTile(string json)
    //{

    //}

    //// ˢ�������Լ������������ͶԽ���ʽ���ڵ� RoadTile
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
    //// �������ڵ� RoadTile ȷ��ʹ���ĸ����飬��������ת����Ӧ������Ƭ��
    ////������תȡ���� RoadTile�����Ϊ��Ƭ���� TileFlags.OverrideTransform��
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
    //// �⽫ȷ����λ�õ���Ƭ�Ƿ�����ͬ�� RoadTile��
    //private bool HasRoadTile(ITilemap tilemap, Vector3Int position)
    //{
    //    return tilemap.GetTile(position) == this;
    //}
    ////����������� RoadTile ������ȷ��Ҫʹ�õľ���
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
    //// ����������� RoadTile ��λ��ȷ��Ҫʹ�õ���ת
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
    // ��������Ӳ˵����Դ��� RoadTile ��Դ�� helper ����
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
    /// ��������
    /// </summary>
    PassNormal,
    /// <summary>
    /// Ӧ������
    /// </summary>
    PassEmergency,
    /// <summary>
    /// �ϰ�����
    PassStop,
}
public enum TileInteractiveType
{
    None,
    Cabinet,
}