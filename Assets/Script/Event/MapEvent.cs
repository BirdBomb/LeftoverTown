using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class MapEvent : MonoBehaviour
{
    /// <summary>
    /// 请求加载地图数据
    /// </summary>
    public class MapEvent_LocalTile_RequestMapData
    {
        public Vector3Int pos;
        public PlayerRef player;
    }
    /// <summary>
    /// 请求保存地图数据
    /// </summary>
    public class MapEvent_LocalTile_SaveMapData
    {
    }

    public class MapEvent_LocalTile_TakeDamage
    {
        public TileObj tileObj;
        public int damage;
    }
    public class MapEvent_LocalTile_UpdateBuildingInfo
    {
        public TileObj tileObj;
        public string tileInfo;
    }
    public class MapEvent_LocalTile_ChangeBuilding
    {
        public Vector3Int buildingPos;
        public string buildingName;
    }
    public class MapEvent_LocalTile_ChangeFloor
    {
        public Vector3Int floorPos;
        public string floorName;
    }
}
