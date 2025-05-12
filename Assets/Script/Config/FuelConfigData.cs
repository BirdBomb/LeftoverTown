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
        new FuelConfig(){FuelID = 1000,FuelSecond = 10},
        new FuelConfig(){FuelID = 1001,FuelSecond = 10},
        new FuelConfig(){FuelID = 1011,FuelSecond = 60},
    };
}
public struct FuelConfig
{
    /// <summary>
    /// ID
    /// </summary>
    public short FuelID;
    /// <summary>
    /// »º…’√Î ˝
    /// </summary>
    public short FuelSecond;
}