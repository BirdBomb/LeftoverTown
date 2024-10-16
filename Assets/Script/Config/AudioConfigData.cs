using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioConfigData 
{
    public readonly static List<AudioConfig> audioConfigs = new List<AudioConfig>()
    {
        new AudioConfig(){ Audio_ID = 1001,Audio_Name = "Shoot_0",Audio_MaxDistance = 200},
        new AudioConfig(){ Audio_ID = 1002,Audio_Name = "Dull_0",Audio_MaxDistance = 20},
        new AudioConfig(){ Audio_ID = 1003,Audio_Name = "LoadRounds_0",Audio_MaxDistance = 20},
        new AudioConfig(){ Audio_ID = 1004,Audio_Name = "HitRock",Audio_MaxDistance = 10},
        new AudioConfig(){ Audio_ID = 1005,Audio_Name = "BombRock",Audio_MaxDistance = 10},
        new AudioConfig(){ Audio_ID = 1006,Audio_Name = "Coins",Audio_MaxDistance = 10},
        new AudioConfig(){ Audio_ID = 1007,Audio_Name = "Step",Audio_MaxDistance = 20},
    };
}
[Serializable]
public struct AudioConfig
{
    [SerializeField]/*±àºÅ*/
    public int Audio_ID;
    [SerializeField]/*Ãû×Ö*/
    public string Audio_Name;
    [SerializeField]/*×î´ó¾àÀë*/
    public float Audio_MaxDistance;
}
