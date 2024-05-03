using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class MapEvent : MonoBehaviour
{
    public class MapEvent_LocalTile_RequestMapData
    {
        public Vector3Int pos;
        public PlayerRef player;
    }
    public class MapEvent_LocalTile_TakeDamage
    {
        public TileObj tileObj;
        public int damage;
    }
    public class MapEvent_LocalTile_UpdateInfo
    {
        public TileObj tileObj;
        public string tileInfo;
    }
}
