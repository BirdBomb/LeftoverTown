using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapMod : MonoBehaviour
{
    [Header("µØÍ¼±àºÅ")]
    public int ID;
    public Dictionary<Vector2Int, short> dic_mapFloorData = new Dictionary<Vector2Int, short>();
    public Dictionary<Vector2Int, short> dic_mapBuildingData = new Dictionary<Vector2Int, short>();
}
