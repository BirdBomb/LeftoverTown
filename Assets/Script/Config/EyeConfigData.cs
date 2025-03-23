using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeConfigData : MonoBehaviour
{
    public static EyeConfig GetItemConfig(int ID)
    {
        return eyeConfigs.Find((x) => { return x.Eye_ID == ID; });
    }
    public readonly static List<EyeConfig> eyeConfigs = new List<EyeConfig>()
    {
        new EyeConfig(){ Eye_ID = 100 },
        new EyeConfig(){ Eye_ID = 101 },
        new EyeConfig(){ Eye_ID = 102 },
        new EyeConfig(){ Eye_ID = 103 },
        new EyeConfig(){ Eye_ID = 104 },
    };
}
[Serializable]
public struct EyeConfig
{
    [SerializeField]/*±àºÅ*/
    public short Eye_ID;
}
