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
    }
    /// <summary>
    /// ���󱣴��ͼ����
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
    /// �ı�һ��������Ľ���
    /// </summary>
    public class MapEvent_LocalTile_ChangeBuildingArea
    {
        public Vector3Int buildingPos;
        public AreaSize areaSize;
        public short buildingID;
    }
    /// <summary>
    /// �ı�һ������
    /// </summary>
    public class MapEvent_LocalTile_ChangeGround
    {
        public Vector3Int groundPos;
        public short groundID;
    }
    /// <summary>
    /// �޸�����̫��
    /// </summary>
    public class MapEvent_LocalTile_ChangeSunLight
    {
        public int distance;
    }
}
