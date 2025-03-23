using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class MapTileTypeData 
{
    [SerializeField]
    public Dictionary<int,short> tileDic = new Dictionary<int, short>();
}
