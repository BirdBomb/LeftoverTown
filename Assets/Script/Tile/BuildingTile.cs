using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
[System.Serializable]
public class BuildingTile : TileBase
{
    [SerializeField, Header("�ؿ�ͼƬ")]
    public Sprite config_Sprite;
    [SerializeField, Header("�ؿ�ʵ��")]
    public GameObject config_InstancedGameObject;
    [SerializeField, Header("ͨ��")]
    public bool config_Pass;
    [SerializeField, Header("����")]
    public int config_Drag;

    [HideInInspector]
    public int tileID;
    [HideInInspector]
    public Vector3Int tilePos;
    [HideInInspector]
    public Vector2 tileWorldPos;
    [HideInInspector]
    public BuildingObj tileObj;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = config_Sprite;
        tileData.gameObject = config_InstancedGameObject;
        tileData.color = Color.white;
        base.GetTileData(position, tilemap, ref tileData);
    }
    public void InitData(BuildingTile buildingTile, Vector3Int vector3Int, int id)
    {
        tileID = id;
        tilePos = vector3Int;
        config_Pass = buildingTile.config_Pass;
        config_Drag = buildingTile.config_Drag;
        config_InstancedGameObject = buildingTile.config_InstancedGameObject;
    }
    public void BindObj(GameObject gameObject)
    {
        gameObject.GetComponent<BuildingObj>().Bind(this, out tileObj);
    }

    /// <summary>
    /// ������Ƭ
    /// </summary>
    public virtual void ActorInputKeycode(ActorManager actor, KeyCode code)
    {
        if (tileObj)
        {
            tileObj.All_ActorInputKeycode(actor, code);
        }
    }
    /// <summary>
    /// ������Ƭ
    /// </summary>
    public virtual bool NearbyTileByActor(ActorManager who)
    {
        if (tileObj)
        {
            return tileObj.All_ActorNearby(who);
        }
        return false;
    }
    /// <summary>
    /// վ����Ƭ��
    /// </summary>
    /// <param name="who"></param>
    public virtual void StandOnTileByActor(ActorManager who)
    {
        if (tileObj)
        {
            tileObj.All_ActorStandOn(who);
        }
    }
    /// <summary>
    /// Զ����Ƭ
    /// </summary>
    /// <returns></returns>
    public virtual bool FarawayTileByActor(ActorManager who)
    {
        if (tileObj)
        {
            return tileObj.All_ActorFaraway(who);
        }
        return false;
    }

    /// <summary>
    /// ��ҿ�����Ƭ
    /// </summary>
    public virtual void NearbyTileByPlayer(PlayerCoreLocal player)
    {
        if (tileObj)
        {
            tileObj.All_PlayerNearby();
        }
    }
    /// <summary>
    /// ���Զ����Ƭ
    /// </summary>
    public virtual void FarawayTileByPlayer(PlayerCoreLocal player)
    {
        if (tileObj)
        {
            tileObj.All_PlayerFaraway();
        }
    }
    /// <summary>
    /// ��Ҹ�����Ƭ
    /// </summary>
    public virtual void HighlightTileByPlayer(bool on)
    {
        if (tileObj)
        {
            tileObj.All_PlayerHighlight(on);
        }
    }
    /// <summary>
    /// �Ǹ�����Ƭ
    /// </summary>
    public bool CanHighlight()
    {
        if (tileObj)
        {
            return tileObj.CanHighlight();
        }
        else
        {
            return false;
        }
    }


#if UNITY_EDITOR
    [MenuItem("Assets/Create/BuildingTile")]
    public static void CreateBaseTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("SaveBuildingTile", "BuildingTile", "Asset", "Save Road Tile", "Assets/Resources/TileScript");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<BuildingTile>(), path);
    }
#endif

}
