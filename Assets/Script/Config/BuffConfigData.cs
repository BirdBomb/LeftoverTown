using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffConfigData : MonoBehaviour
{
    public static BuffConfig GetBuffConfig(int ID)
    {
        return buffConfigs.Find((x) => { return x.Buff_ID == ID; });
    }
    public readonly static List<BuffConfig> buffConfigs = new List<BuffConfig>()
    {

  };
}
public struct BuffConfig
{
    public short Buff_ID;
    public bool Buff_Icon;
}
