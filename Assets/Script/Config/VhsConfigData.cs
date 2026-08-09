using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VhsConfigData
{
    public static VhsConfig GetVhsConfig(int ID)
    {
        return vhsConfigs.Find((x) => { return x.Vhs_ID == ID; });
    }

    public readonly static List<VhsConfig> vhsConfigs = new List<VhsConfig>() 
    {
        new VhsConfig(){ Vhs_ID = 9800,Vhs_Color = new Color32(255,165,0,255)},
        new VhsConfig(){ Vhs_ID = 9801,Vhs_Color = new Color32(124,223,0,255)},
        new VhsConfig(){ Vhs_ID = 9802,Vhs_Color = new Color32(164,0,223,255)},
        new VhsConfig(){ Vhs_ID = 9803,Vhs_Color = new Color32(0,223,22,255)},
        new VhsConfig(){ Vhs_ID = 9804,Vhs_Color = new Color32(223,0,115,255)},
    };

}
public struct VhsConfig
{
    public short Vhs_ID;
    public Color32 Vhs_Color;
}