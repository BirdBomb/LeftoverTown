using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairConfigData : MonoBehaviour
{
    public static HairConfig GetHairConfig(int ID)
    {
        return hairConfigs.Find((x) => { return x.Hair_ID == ID; });
    }
    public readonly static List<HairConfig> hairConfigs = new List<HairConfig>()
    {
        new HairConfig(){ Hair_ID = 0,Hair_Name ="" },
        new HairConfig(){ Hair_ID = 100,Hair_Name ="" },
        new HairConfig(){ Hair_ID = 101,Hair_Name ="" },
        new HairConfig(){ Hair_ID = 102,Hair_Name ="" },
    };
}
[Serializable]
public struct HairConfig
{
    [SerializeField]/*±àºÅ*/
    public short Hair_ID;
    [SerializeField]/*Ãû×Ö*/
    public string Hair_Name;
}
