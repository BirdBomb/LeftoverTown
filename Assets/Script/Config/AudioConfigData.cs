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
    };
}
[Serializable]
public struct AudioConfig
{
    [SerializeField]/*���*/
    public int Audio_ID;
    [SerializeField]/*����*/
    public string Audio_Name;
    [SerializeField]/*������*/
    public float Audio_MaxDistance;
}