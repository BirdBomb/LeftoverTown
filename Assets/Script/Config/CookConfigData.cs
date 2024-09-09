using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookConfigData 
{
    public static CookConfig GetItemConfig(int ID)
    {
        return cookConfigs.Find((x) => { return x.Cook_ID == ID; });
    }
    public readonly static List<CookConfig> cookConfigs = new List<CookConfig>()
    {
        new CookConfig(){Cook_ID = 4009,Cook_Time = 20,Cook_Level = 0,
            Cook_Raw = new List<int>(){ 30053009, 30063009, 30094001, 30094002, 30054008, 30064008, 40014008, 40024008, } },
        new CookConfig(){Cook_ID = 4010,Cook_Time = 20,Cook_Level = 0,
            Cook_Raw = new List<int>(){ 3002, 3003, 4006, 4007, 30023003, 30024006, 30024007, 30034006, 30034007,40064007 } },
        new CookConfig(){Cook_ID = 4011,Cook_Time = 20,Cook_Level = 0,
            Cook_Raw = new List<int>(){ 3005, 3006, 4001, 4002, 30053006, 30054001, 30054002, 30064001, 30064002, 40014002, } },
        new CookConfig(){Cook_ID = 4012,Cook_Time = 20,Cook_Level = 5,
            Cook_Raw = new List<int>(){ 30023005, 30033005, 30023006, 30033006, 30024001, 30034001, 30024002, 30034002, } },
    };

}
public struct CookConfig
{
    [SerializeField]
    public short Cook_ID;
    [SerializeField]
    public short Cook_Time;
    [SerializeField]
    public short Cook_Level;
    [SerializeField]
    public List<int> Cook_Raw;
}
