using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class MapEvent : MonoBehaviour
{
    public class MapEvent_SaveMap
    {

    }
    public class MapEvent_LoadMap
    {

    }
    public class MapEvent_ChangeTile
    {
        public Vector3Int tilePos;
        public string tileName;
    }
    public class MapEvent_InvokeTile
    {
        public Vector3Int tilePos;
        public bool hasState;
        public bool hasInput;
    }
    public class MapEvent_DamageTile_Upload
    {
        public Vector3Int tilePos;
        public int damageValue;
    }
    public class MapEvent_DamageTile
    {
        public Vector3Int tilePos;
        public int damageValue;
    }
    /// <summary>
    /// 某人请求地图数据
    /// </summary>
    public class MapEvent_RequestMapData
    {
        public Vector3Int pos;
        public PlayerRef player;
    }
}
