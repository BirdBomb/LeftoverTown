using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class MapTileInfoData 
{
    [SerializeField]
    public Dictionary<int, string> tileDic = new Dictionary<int, string>();
    [SerializeField]
    public Dictionary<int, byte[]> mapData = new Dictionary<int, byte[]>();
}
