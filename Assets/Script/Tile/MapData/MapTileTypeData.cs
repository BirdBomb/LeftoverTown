using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class MapTileTypeData 
{
    [SerializeField]
    public Dictionary<int,int> tileDic = new Dictionary<int, int>();
}
