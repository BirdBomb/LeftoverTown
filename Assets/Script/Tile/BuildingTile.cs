using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
[System.Serializable]
public class BuildingTile : TileBase
{
    [SerializeField, Header("µØ¿éÍ¼Æ¬")]
    public Sprite config_Sprite;
    [SerializeField, Header("µØ¿éÊµÀý")]
    public GameObject config_InstancedGameObject;
    [SerializeField, Header("Í¨ÐÐ")]
    public bool config_Pass;
    [SerializeField, Header("×èÁ¦")]
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
    /// »½ÐÑÍßÆ¬
    /// </summary>
    public virtual void ActorInputKeycode(ActorManager actor, KeyCode code)
    {
        if (tileObj)
        {
            tileObj.ActorInputKeycode(actor, code);
        }
    }
    /// <summary>
    /// ¿¿½üÍßÆ¬
    /// </summary>
    public virtual bool NearbyTileByActor(ActorManager who)
    {
        if (tileObj)
        {
            return tileObj.ActorNearby(who);
        }
        return false;
    }
    /// <summary>
    /// Õ¾ÔÚÍßÆ¬ÉÏ
    /// </summary>
    /// <param name="who"></param>
    public virtual void StandOnTileByActor(ActorManager who)
    {
        if (tileObj)
        {
            tileObj.ActorStandOn(who);
        }
    }
    /// <summary>
    /// Ô¶ÀëÍßÆ¬
    /// </summary>
    /// <returns></returns>
    public virtual bool FarawayTileByActor(ActorManager who)
    {
        if (tileObj)
        {
            return tileObj.ActorFaraway(who);
        }
        return false;
    }

    /// <summary>
    /// ³ÖÓÐÍßÆ¬
    /// </summary>
    /// <param name="who"></param>
    /// <returns></returns>
    public virtual bool HoldingTileByPlayer(PlayerCoreLocal player)
    {
        if (tileObj)
        {
            return tileObj.PlayerHolding(player);
        }
        return false;
    }
    /// <summary>
    /// ÊÍ·ÅÍßÆ¬
    /// </summary>
    /// <param name="who"></param>
    /// <returns></returns>
    public virtual bool ReleaseTileByPlayer(PlayerCoreLocal player)
    {
        if (tileObj)
        {
            return tileObj.PlayerRelease(player);
        }
        return false;
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
# endif

}
