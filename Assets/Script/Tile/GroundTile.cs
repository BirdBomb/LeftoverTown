using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.Tilemaps.Tile;

[System.Serializable]
public class GroundTile : TileBase
{
    [SerializeField, Header("地块图片")]
    public Sprite config_Sprite;
    [SerializeField, Header("地块实例")]
    public GameObject config_InstancedGameObject;
    [SerializeField, Header("通行")]
    public bool config_Pass;
    [HideInInspector]
    public bool offset_Pass;
    [SerializeField, Header("阻力")]
    public int config_Drag;
    [HideInInspector]
    public int offset_Drag;
    [HideInInspector]
    public int tileID;
    [HideInInspector]
    public Vector3Int tilePos;
    [HideInInspector]
    public Vector2 tileWorldPos;
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
    public void BindObj(GameObject gameObject)
    {
        gameObject.GetComponent<GroundObj>().Bind(this, out tileObj);
    }

    #region//路径信息
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
    public GroundTile _temp_fatherTile;

    public void ResetTilePathInfo()
    {
        _temp_DistanceToFrom = 0;
        _temp_DistanceToTarget = 0;
        _temp_DistanceMain = 0;

        _temp_fatherTile = null;
    }
    #endregion
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
