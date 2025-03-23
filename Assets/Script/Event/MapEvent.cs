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
    }
    /// <summary>
    /// 请求保存地图数据
    /// </summary>
    public class MapEvent_LocalTile_SaveMapData
    {
    }

    public class MapEvent_LocalTile_TakeDamage
    {
        public Vector3Int pos;
        public int damage;
    }
    public class MapEvent_LocalTile_UpdateBuildingInfo
    {
        public Vector3Int pos;
        public string info;
    }
    /// <summary>
    /// 改变一个区域里的建筑
    /// </summary>
    public class MapEvent_LocalTile_ChangeBuildingArea
    {
        public Vector3Int buildingPos;
        public AreaSize areaSize;
        public short buildingID;
    }
    /// <summary>
    /// 改变一个地面
    /// </summary>
    public class MapEvent_LocalTile_ChangeGround
    {
        public Vector3Int groundPos;
        public short groundID;
    }
    /// <summary>
    /// 修改世界太阳
    /// </summary>
    public class MapEvent_LocalTile_ChangeSunLight
    {
        public int distance;
    }
}
