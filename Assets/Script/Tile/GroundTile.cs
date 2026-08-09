using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.Tilemaps.Tile;

[System.Serializable]
public class GroundTile : TileBase
{
    [SerializeField, Header("µØ¿éÍ¼Æ¬")]
    public Sprite config_Sprite;
    [SerializeField, Header("µØ¿éÊµÀý")]
    public GameObject config_InstancedGameObject;
    [SerializeField, Header("Í¨ÐÐ")]
    public bool config_Pass;
    [HideInInspector]
    public bool offset_Pass;
    [SerializeField, Header("×èÁ¦")]
    public int config_Drag;
    [HideInInspector]
    public int offset_Drag;
    [HideInInspector]
    public int tileID;
    [HideInInspector]
    public Vector3Int tilePos;
    //[HideInInspector]
    //public Vector2 tileWorldPos;
    [HideInInspector]
    public GroundObj tileObj;
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = config_Sprite;
        tileData.gameObject = config_InstancedGameObject;
        tileData.color = Color.white;
        tileData.transform = Matrix4x4.identity;
    }
    public void InitData(GroundTile groundTile,Vector3Int vector3Int,int id)
    {
        config_InstancedGameObject = groundTile.config_InstancedGameObject;
        config_Pass = groundTile.config_Pass;
        offset_Pass = groundTile.config_Pass;
        config_Drag = groundTile.config_Drag;
        offset_Drag = groundTile.config_Drag;
        tilePos = vector3Int;
        tileID = id;
    }
    public GroundObj BindObj(GameObject gameObject)
    {
        gameObject.GetComponent<GroundObj>().Bind(this, out tileObj);
        return tileObj;
    }
    /// <summary>
    /// ¿¿½üÍßÆ¬
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
    /// Õ¾ÔÚÍßÆ¬ÉÏ
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
    /// Ô¶ÀëÍßÆ¬
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
    public float GetSpeedOffset()
    {
        if (tileObj)
        {
            return tileObj.All_SpeedOffset();
        }
        return 1;
    }
#if UNITY_EDITOR
    [MenuItem("Assets/Create/GroundTile")]
    public static void CreateBaseTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("SaveGroundTile", "GroundTile", "Asset", "Save Road Tile", "Assets/Resources/TileScript");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<GroundTile>(), path);
    }
# endif

}
