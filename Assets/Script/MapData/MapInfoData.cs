using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class MapInfoData 
{
    [SerializeField]
    public string name;
    [SerializeField]
    public string seed;
    [SerializeField] 
    public short distance = 600;
    [SerializeField]
    public short date;
    [SerializeField]
    public short hour;
}

