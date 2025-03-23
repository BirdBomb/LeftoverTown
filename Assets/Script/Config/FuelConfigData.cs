using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelConfigData 
{
    public static FuelConfig GetFuelConfig(int ID)
    {
        return fuelConfigs.Find((x) => { return x.FuelID == ID; });
    }
    public readonly static List<FuelConfig> fuelConfigs = new List<FuelConfig>()
    {
        new FuelConfig(){FuelID = 1000,FuelVal = 100},
        new FuelConfig(){FuelID = 1002,FuelVal = 50},
        new FuelConfig(){FuelID = 1011,FuelVal = 500},
    };
}
public struct FuelConfig
{
    public short FuelID;
    public short FuelVal;
}