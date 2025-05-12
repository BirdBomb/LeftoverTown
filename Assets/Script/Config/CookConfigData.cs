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
        /*失败菜肴*/
        new CookConfig(){Cook_ID = 4100,Cook_Count = 1,
            Cook_Raw_0=new List<short>(){ 4100},
            },
        /*薯果肉煲*/
        new CookConfig(){Cook_ID = 4101,Cook_Count = 1,
            Cook_Raw_0=new List<short>(){ 3003},
            Cook_Raw_1=new List<short>(){ 3100, 3101, 3102, 3103},
            Cook_Raw_2=new List<short>(){ 3003, 3100, 3101, 3102, 3103},
            },
        /*杂果炖*/
        new CookConfig(){Cook_ID = 4102,Cook_Count = 1,
            Cook_Raw_0=new List<short>(){ 3000, 3002},
            Cook_Raw_1=new List<short>(){ 3000, 3002},
            Cook_Raw_2=new List<short>(){ 3000, 3002},
            },
        /*抓肉*/
        new CookConfig(){Cook_ID = 4103,Cook_Count = 1,
            Cook_Raw_0=new List<short>(){ 3100, 3101, 3102, 3103},
            Cook_Raw_1=new List<short>(){ 3100, 3101, 3102, 3103},
            Cook_Raw_2=new List<short>(){ 3100, 3101, 3102, 3103},
            },
        /*果渍肉*/
        new CookConfig(){Cook_ID = 4104,Cook_Count = 1,
            Cook_Raw_0=new List<short>(){ 3000, 3002},
            Cook_Raw_1=new List<short>(){ 3100, 3101, 3102, 3103},
            Cook_Raw_2=new List<short>(){ 3000, 3002, 3100, 3101, 3102, 3103},
            },
        /*水煮鱼*/
        new CookConfig(){Cook_ID = 4105,Cook_Count = 1,
            Cook_Raw_0=new List<short>(){ 3004},
            Cook_Raw_1=new List<short>(){ 3110},
            },
    };

}
public struct CookConfig
{
    [SerializeField]
    public short Cook_ID;
    [SerializeField]
    public short Cook_Count;
    /// <summary>
    /// 必须原材料0
    /// </summary>
    public List<short> Cook_Raw_0;
    /// <summary>
    /// 必须原材料1
    /// </summary>
    public List<short> Cook_Raw_1;
    /// <summary>
    /// 必须原材料1
    /// </summary>
    public List<short> Cook_Raw_2;
}
