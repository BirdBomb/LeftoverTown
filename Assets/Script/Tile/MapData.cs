using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class MapData 
{
    [SerializeField]
    public Dictionary<string, string> tiles = new Dictionary<string, string>();
}
