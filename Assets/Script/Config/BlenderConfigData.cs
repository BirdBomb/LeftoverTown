using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlenderConfigData 
{
    public static BlenderConfig GetBlenderConfig(int ID)
    {
        return blenderConfigs.Find((x) => { return x.blender_FromID == ID; });
    }

    public readonly static List<BlenderConfig> blenderConfigs = new List<BlenderConfig>()
    {
        new BlenderConfig(){ blender_FromID=1016, blender_ToID=1114,blender_ToCount = 4},
        new BlenderConfig(){ blender_FromID=6102, blender_ToID=3200,blender_ToCount = 1},
    };
}
public struct BlenderConfig
{
    public int blender_FromID;
    public int blender_ToID;
    public int blender_ToCount;
}
