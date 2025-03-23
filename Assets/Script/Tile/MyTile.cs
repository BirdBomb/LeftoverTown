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
    [Header("��ƬID")]
    public short config_tileID;
    [Header("��Ƭͨ������")]
    public TilePassType config_passType;
    [Header("��Ƭ����ϵ��")]
    public int config_passDrag;

    [HideInInspector]
    public Vector2 _posInWorld;
    [HideInInspector]
    public Vector3Int _posInCell;

    [HideInInspector]
    public GameObject bindObj;

    #region//Ѱ·���
    /// <summary>
    /// ��Ƭ��������
    /// </summary>
    [HideInInspector]
    public int temp_PassDrag;
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
        }
    }
    /// <summary>
    /// ������Ƭ����
    /// </summary>
    /// <param name="offset"></param>
    public void ResetDrag(int offset)
    {
        temp_PassDrag = config_passDrag + offset;
    }
    /// <summary>
    /// ���Ƶؿ�
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
    /// ������Ƭ
    /// </summary>
    public virtual void ActorInputKeycode(ActorManager actor,KeyCode code)
    {
    }
    /// <summary>
    /// ������Ƭ
    /// </summary>
    public virtual bool NearbyTileByActor(ActorManager who)
    {
        return false;
    }
    /// <summary>
    /// վ����Ƭ��
    /// </summary>
    /// <param name="who"></param>
    public virtual void StandOnTileByActor(ActorManager who)
    {
    }
    /// <summary>
    /// Զ����Ƭ
    /// </summary>
    /// <returns></returns>
    public virtual bool FarawayTileByActor(ActorManager who)
    {
        return false;
    }
    /// <summary>
    /// ������Ƭ
    /// </summary>
    /// <param name="who"></param>
    /// <returns></returns>
    public virtual bool HoldingTileByPlayer(PlayerCoreLocal player)
    {
        return false;
    }
    /// <summary>
    /// �ͷ���Ƭ
    /// </summary>
    /// <param name="who"></param>
    /// <returns></returns>
    public virtual bool ReleaseTileByPlayer(PlayerCoreLocal player)
    {
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