using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponConfigData 
{
    public static WeaponConfig GetWeaponConfig(short id)
    {
        WeaponConfig weaponConfig = weaponConfigs.Find((x) => { return x.ID == id; });
        if(weaponConfig.ID == 0) { weaponConfig.Distance = 1; }
        return weaponConfig;
    }
    public readonly static List<WeaponConfig> weaponConfigs = new List<WeaponConfig>()
    {
        new WeaponConfig(){ID = 2100,Distance = 2},
        new WeaponConfig(){ID = 2101,Distance = 2},
        new WeaponConfig(){ID = 2200,Distance = 10},
        new WeaponConfig(){ID = 2201,Distance = 10},
        new WeaponConfig(){ID = 2202,Distance = 10},
        new WeaponConfig(){ID = 2300,Distance = 10},
        new WeaponConfig(){ID = 2301,Distance = 10},
        new WeaponConfig(){ID = 2302,Distance = 10},
        new WeaponConfig(){ID = 2303,Distance = 10},
        new WeaponConfig(){ID = 2304,Distance = 10},
        new WeaponConfig(){ID = 2305,Distance = 10},
        new WeaponConfig(){ID = 2306,Distance = 10},
    };
} 
public struct WeaponConfig
{
    public short ID;
    public float Distance;
}