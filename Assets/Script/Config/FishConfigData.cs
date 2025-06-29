using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishConfigData 
{
    public readonly static List<FishConfig> fishConfigs = new List<FishConfig>()
    {
        new FishConfig(){ AreaID = 9000,ItemID = 3110,ItemWeight = 20,FishType = FishType.Fish,},
        new FishConfig(){ AreaID = 9000,ItemID = 1001,ItemWeight = 10,FishType = FishType.Other,},
        new FishConfig(){ AreaID = 9000,ItemID = 1002,ItemWeight = 10,FishType = FishType.Other,},
    };
}
[SerializeField]
public struct FishConfig
{
    public short AreaID;
    public short ItemID;
    public short ItemWeight;
    public FishType FishType;
}
public enum FishType 
{ 
    Fish,
    Other,
}