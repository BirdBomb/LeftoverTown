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
        new BarbecueConfig(){BarbecueID = 3001,BarbecueToID = 4005,BarbecueVal = 6},
        new BarbecueConfig(){BarbecueID = 3002,BarbecueToID = 4006,BarbecueVal = 6},
        new BarbecueConfig(){BarbecueID = 3003,BarbecueToID = 4007,BarbecueVal = 6},
        new BarbecueConfig(){BarbecueID = 3005,BarbecueToID = 4001,BarbecueVal = 6},
        new BarbecueConfig(){BarbecueID = 3006,BarbecueToID = 4002,BarbecueVal = 6},
        new BarbecueConfig(){BarbecueID = 3007,BarbecueToID = 4003,BarbecueVal = 6},
        new BarbecueConfig(){BarbecueID = 3008,BarbecueToID = 4004,BarbecueVal = 6},
        new BarbecueConfig(){BarbecueID = 3009,BarbecueToID = 4008,BarbecueVal = 6},
    };

}
public struct BarbecueConfig
{
    public short BarbecueID;
    public short BarbecueToID;
    public short BarbecueVal;
}