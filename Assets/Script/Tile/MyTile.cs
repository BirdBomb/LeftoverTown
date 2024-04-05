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
    [Header("��Ƭͨ������")]
    public TilePassType passType;
    [Header("��Ƭ��������")]
    public TileInteractiveType interactiveType;
    [Header("��Ƭ����")]
    public float passOffset;

    [HideInInspector]
    public int x;
    [HideInInspector]
    public int y;
    [HideInInspector]
    public Vector2 pos;
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

    public void ResetTilePathInfo()
    {
        _temp_DistanceToFrom = 0;
        _temp_DistanceToTarget = 0;
        _temp_DistanceMain = 0;

        _temp_fatherTile = null;
    }
    #endregion
    /// <summary>
    /// ʵ����Ƭ
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
    /// ��ʼ��Ƭ
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
    /// ��ȡ��Ƭ
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
    /// ������Ƭ
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
    /// ������Ƭ
    /// </summary>
    public virtual void UpdateTile(string json)
    {

    }

    /// <summary>
    /// ������Ƭ
    /// </summary>
    public virtual void InvokeTile()
    {
        if (bindObj && bindObj.TryGetComponent(out TileObj obj))
        {
            obj.Invoke();
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