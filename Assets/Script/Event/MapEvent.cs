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
        /// <summary>
        /// 角色位置
        /// </summary>
        public Vector3Int playerPos;
        /// <summary>
        /// 地图尺寸
        /// </summary>
        public int mapSize;
        /// <summary>
        /// 角色引用
        /// </summary>
        public PlayerRef playerRef;
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
        public int buildingID;
    }
    public class MapEvent_LocalTile_ChangeFloor
    {
        public Vector3Int floorPos;
        public int floorID;
    }
}
