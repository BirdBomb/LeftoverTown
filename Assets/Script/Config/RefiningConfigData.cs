using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefiningConfigData
{
    public static RefiningConfig GetRefiningConfig(int ID)
    {
        return refiningConfigs.Find((x) => { return x.RefiningBeforeID == ID; });
    }
    public readonly static List<RefiningConfig> refiningConfigs = new List<RefiningConfig>()
    {
        new RefiningConfig(){RefiningBeforeID = 1013,RefiningAfterID = 1110,RefiningSecond = 10},
        new RefiningConfig(){RefiningBeforeID = 3000,RefiningAfterID = 4006,RefiningSecond = 5},
        new RefiningConfig(){RefiningBeforeID = 3002,RefiningAfterID = 4007,RefiningSecond = 5},
        new RefiningConfig(){RefiningBeforeID = 3003,RefiningAfterID = 4008,RefiningSecond = 5},
        new RefiningConfig(){RefiningBeforeID = 3100,RefiningAfterID = 4001,RefiningSecond = 5},
        new RefiningConfig(){RefiningBeforeID = 3101,RefiningAfterID = 4002,RefiningSecond = 5},
        new RefiningConfig(){RefiningBeforeID = 3102,RefiningAfterID = 4003,RefiningSecond = 5},
        new RefiningConfig(){RefiningBeforeID = 3103,RefiningAfterID = 4004,RefiningSecond = 5},
        new RefiningConfig(){RefiningBeforeID = 3104,RefiningAfterID = 4005,RefiningSecond = 5},
    };
}
public struct RefiningConfig
{
    public short RefiningBeforeID;
    public short RefiningAfterID;
    public short RefiningSecond;
}