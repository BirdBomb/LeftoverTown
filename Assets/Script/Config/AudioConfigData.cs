using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioConfigData 
{
    /// <summary>
    /// 1000-1999 UI
    /// 2000-2999 Item
    /// 3000-3999 Tile
    /// 4000-4999 Actor
    /// 5000-5999 World
    /// 9000-9999 Other
    /// </summary>
    public readonly static List<AudioConfig> audioConfigs = new List<AudioConfig>()
    {
        new AudioConfig(){ Audio_ID = 1001,Audio_Name = "Shoot_0",Audio_MaxDistance = 200},
        new AudioConfig(){ Audio_ID = 1002,Audio_Name = "Dull_0",Audio_MaxDistance = 20},
        new AudioConfig(){ Audio_ID = 1003,Audio_Name = "LoadRounds_0",Audio_MaxDistance = 20},
        new AudioConfig(){ Audio_ID = 1004,Audio_Name = "HitRock",Audio_MaxDistance = 10},
        new AudioConfig(){ Audio_ID = 1005,Audio_Name = "BombRock",Audio_MaxDistance = 10},
        new AudioConfig(){ Audio_ID = 1006,Audio_Name = "Coins",Audio_MaxDistance = 10},
        new AudioConfig(){ Audio_ID = 1007,Audio_Name = "Step",Audio_MaxDistance = 20},
        new AudioConfig(){ Audio_ID = 1008,Audio_Name = "TryUnlock",Audio_MaxDistance = 20},
        new AudioConfig(){ Audio_ID = 1009,Audio_Name = "DoorClose",Audio_MaxDistance = 20},
        new AudioConfig(){ Audio_ID = 1010,Audio_Name = "Shake",Audio_MaxDistance = 20},
        new AudioConfig(){ Audio_ID = 1011,Audio_Name = "DoorOpen",Audio_MaxDistance = 20},


        new AudioConfig(){ Audio_ID = 2000,Audio_Name = "Shoot_0",Audio_MaxDistance = 200},
        new AudioConfig(){ Audio_ID = 2001,Audio_Name = "Shoot_1",Audio_MaxDistance = 200},

        new AudioConfig(){ Audio_ID = 4000,Audio_Name = "ZombieHook_Pound",Audio_MaxDistance = 200},
        new AudioConfig(){ Audio_ID = 4001,Audio_Name = "ZombieHook_Hook",Audio_MaxDistance = 200},
};
}
[Serializable]
public struct AudioConfig
{
    [SerializeField]/*±àºÅ*/
    public short Audio_ID;
    [SerializeField]/*Ãû×Ö*/
    public string Audio_Name;
    [SerializeField]/*×î´ó¾àÀë*/
    public float Audio_MaxDistance;
}
