using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class MapTileInfoData 
{
    [SerializeField]
    public string name;
    [SerializeField]
    public string seed;
    [SerializeField]
    public int date;
    [SerializeField]
    public int hour;
    [SerializeField]
    public Dictionary<int, string> tileDic = new Dictionary<int, string>();
}
