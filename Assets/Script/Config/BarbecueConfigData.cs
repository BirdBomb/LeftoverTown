using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarbecueConfigData
{
    public static BarbecueConfig GetBarbecueConfig(int ID)
    {
        return barbecueConfigs.Find((x) => { return x.BarbecueID == ID; });
    }
    public readonly static List<BarbecueConfig> barbecueConfigs = new List<BarbecueConfig>()
    {
        new BarbecueConfig(){BarbecueID = 3000,BarbecueToID = 4006,BarbecueVal = 6},
        new BarbecueConfig(){BarbecueID = 3002,BarbecueToID = 4007,BarbecueVal = 6},
        new BarbecueConfig(){BarbecueID = 3003,BarbecueToID = 4008,BarbecueVal = 6},
        new BarbecueConfig(){BarbecueID = 3100,BarbecueToID = 4001,BarbecueVal = 6},
        new BarbecueConfig(){BarbecueID = 3101,BarbecueToID = 4002,BarbecueVal = 6},
        new BarbecueConfig(){BarbecueID = 3102,BarbecueToID = 4003,BarbecueVal = 6},
        new BarbecueConfig(){BarbecueID = 3103,BarbecueToID = 4004,BarbecueVal = 6},
        new BarbecueConfig(){BarbecueID = 3104,BarbecueToID = 4005,BarbecueVal = 6},
    };

}
public struct BarbecueConfig
{
    public short BarbecueID;
    public short BarbecueToID;
    public short BarbecueVal;
}