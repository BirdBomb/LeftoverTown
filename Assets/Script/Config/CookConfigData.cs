using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookConfigData 
{
    public static CookConfig GetCookConfig(int ID)
    {
        return cookConfigs.Find((x) => { return x.Cook_ID == ID; });
    }
    public readonly static List<CookConfig> cookConfigs = new List<CookConfig>()
    {
        new CookConfig(){Cook_ID = 4100,Cook_Time = 20,Cook_Level = 0,
            CooK_Raw_Main = new List<short>(){ },
            CooK_Raw_Add = new List<short>(){ } },
        /* Ìπ˚»‚Ï“*/
        new CookConfig(){Cook_ID = 4101,Cook_Time = 20,Cook_Level = 0,
            CooK_Raw_Main = new List<short>(){ 3100, 3101, 3102, 4001, 4002, 4003 },
            CooK_Raw_Add = new List<short>(){ 3003, 4008 } },
        /*‘”π˚Ï¿*/
        new CookConfig(){Cook_ID = 4102,Cook_Time = 20,Cook_Level = 0,
            CooK_Raw_Main = new List<short>(){ 3000, 3001, 3002, 4006, 4007 },
            CooK_Raw_Add = new List<short>(){ 3000, 3001, 3002, 4006, 4007 } },
        /*◊•»‚*/
        new CookConfig(){Cook_ID = 4103,Cook_Time = 20,Cook_Level = 0,
            CooK_Raw_Main = new List<short>(){ 3100, 3101, 3102, 4001, 4002, 4003, },
            CooK_Raw_Add = new List<short>(){  3100, 3101, 3102, 4001, 4002, 4003, } },
        /*π˚+»‚*/
        new CookConfig(){Cook_ID = 4104,Cook_Time = 20,Cook_Level = 5,
            CooK_Raw_Main = new List<short>(){ 3100, 3101 },
            CooK_Raw_Add = new List<short>(){ 3000, 3001, 3002 } },
        /*ÀÆ÷Û”„*/
        new CookConfig(){Cook_ID = 4105,Cook_Time = 20,Cook_Level = 0,
            CooK_Raw_Main = new List<short>(){ 3110, },
            CooK_Raw_Add = new List<short>(){ 3004 } },
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
    public List<short> CooK_Raw_Main;
    [SerializeField]
    public List<short> CooK_Raw_Add;
}
