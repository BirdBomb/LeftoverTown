using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class MapEvent : MonoBehaviour
{
    /// <summary>
    /// ������ص�ͼ����
    /// </summary>
    public class MapEvent_LocalTile_RequestMapData
    {
        /// <summary>
        /// ��ɫλ��
        /// </summary>
        public Vector3Int playerPos;
        /// <summary>
        /// ��ͼ�ߴ�
        /// </summary>
        public int mapSize;
        /// <summary>
        /// ��ɫ����
        /// </summary>
        public PlayerRef playerRef;
    }
    /// <summary>
    /// ���󱣴��ͼ����
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
