using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishConfigData 
{
    public readonly static List<FishConfig> fishConfigs = new List<FishConfig>()
    {
        new FishConfig(){ AreaType = AreaType.Water,ItemID = 3110,ItemWeight = 20,FishType = FishType.Other,},
    };
}
[SerializeField]
public struct FishConfig
{
    public AreaType AreaType;
    public short ItemID;
    public short ItemWeight;
    public FishType FishType;
}
public enum AreaType
{
    Water,
}
public enum FishType 
{ 
    Fish,
    Other,
}