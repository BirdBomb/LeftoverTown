using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioConfigData 
{
    public readonly static List<AudioConfig> audioConfigs = new List<AudioConfig>()
    {
        new AudioConfig(){ Audio_ID = 1001,Audio_Name = "Shoot_0"},
    };
}
[Serializable]
public struct AudioConfig
{
    [SerializeField]/*±àºÅ*/
    public int Audio_ID;
    [SerializeField]/*Ãû×Ö*/
    public string Audio_Name;
}
