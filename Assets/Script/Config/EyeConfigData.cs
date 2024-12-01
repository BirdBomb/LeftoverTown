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
    };
}
[Serializable]
public struct EyeConfig
{
    [SerializeField]/*±àºÅ*/
    public int Eye_ID;
}
