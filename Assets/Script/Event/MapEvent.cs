using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
