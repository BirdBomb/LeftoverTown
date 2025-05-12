using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class MapEvent : MonoBehaviour
{
    /// <summary>
    /// ������ص�ͼ����
    /// </summary>
    public class MapEvent_Local_RequestMapData
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
    /// �ı�һ��������Ľ���
    /// </summary>
    public class MapEvent_Local_ChangeBuildingArea
    {
        public Vector3Int buildingPos;
        public AreaSize areaSize;
        public short buildingID;
    }
    /// <summary>
    /// �ı�һ��������Ľ���
    /// </summary>
    public class MapEvent_State_ChangeBuildingArea
    {
        public Vector3Int buildingPos;
        public AreaSize areaSize;
        public short buildingID;
    }
    /// <summary>
    /// �ı�һ������
    /// </summary>
    public class MapEvent_Local_ChangeGround
    {
        public Vector3Int groundPos;
        public short groundID;
    }
    /// <summary>
    /// �޸�����̫��
    /// </summary>
    public class MapEvent_Local_ChangeSunLight
    {
        public short distance;
    }
}
