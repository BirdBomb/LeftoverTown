using System.Collections;
using System.Collections.Generic;
using TMPro;
using TreeEditor;
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
    public int x;
    [HideInInspector]
    public int y;
    [HideInInspector]
    public Vector2 pos;

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
    /// ������Ƭ
    /// </summary>
    /// <param name="tile"></param>
    public virtual void CopyTile(MyTile tile)
    {
        name = tile.name;

        passType = tile.passType;
        interactiveType = tile.interactiveType;
        passOffset = tile.passOffset;

        gameObject = tile.gameObject;

        sprite = tile.sprite;
        color = tile.color;
        flags = tile.flags; 
        colliderType = tile.colliderType;
    }
    /// <summary>
    /// ��ʼ��Ƭ
    /// </summary>
    public virtual void InitTile(int x,int y,Vector2 pos)
    {
        this.x = x;
        this.y = y;
        this.pos = pos;
    }
    /// <summary>
    /// ��ȡ��Ƭ
    /// </summary>
    public virtual void LoadTile(string json)
    {

    }
    /// <summary>
    /// ������Ƭ
    /// </summary>
    public virtual string SaveTile()
    {
        return null;
    }
    /// <summary>
    /// ������Ƭ
    /// </summary>
    public virtual void UpdateTile(string json)
    {

    }

    /// <summary>
    /// ��ʾ�ź�
    /// </summary>
    public virtual MyTile ShowSignal()
    {
        return null;
    }
    /// <summary>
    /// �����ź�
    /// </summary>
    public virtual void HideSignal()
    {
       
    }
    /// <summary>
    /// ����
    /// </summary>
    public virtual void Interactive()
    {

    }
    #endregion

    public void Init(MyTileData tileData)
    {
        //sprite = res
    }

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