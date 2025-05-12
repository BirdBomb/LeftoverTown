using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class MapEvent : MonoBehaviour
{
    /// <summary>
    /// 请求加载地图数据
    /// </summary>
    public class MapEvent_Local_RequestMapData
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
    public class MapEvent_Local_SaveMapData
    {
    }
    public class MapEvent_Local_ChangeBuildingHp
    {
        public Vector3Int pos;
        public int offset;
    }
    public class MapEvent_Local_ChangeBuildingInfo
    {
        public Vector3Int pos;
        public string info;
    }
    /// <summary>
    /// 改变一个区域里的建筑
    /// </summary>
    public class MapEvent_Local_ChangeBuildingArea
    {
        public Vector3Int buildingPos;
        public AreaSize areaSize;
        public short buildingID;
    }
    /// <summary>
    /// 改变一个区域里的建筑
    /// </summary>
    public class MapEvent_State_ChangeBuildingArea
    {
        public Vector3Int buildingPos;
        public AreaSize areaSize;
        public short buildingID;
    }
    /// <summary>
    /// 改变一个地面
    /// </summary>
    public class MapEvent_Local_ChangeGround
    {
        public Vector3Int groundPos;
        public short groundID;
    }
    /// <summary>
    /// 修改世界太阳
    /// </summary>
    public class MapEvent_Local_ChangeSunLight
    {
        public short distance;
    }
}
