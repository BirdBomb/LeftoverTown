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
        /*正面Buff*/
        /*吃饱喝足(恢复精神)*/new BuffConfig(){ Buff_ID = 1000, Buff_Icon = true },
        /*精神焕发(高速度)*/new BuffConfig(){ Buff_ID = 1001, Buff_Icon = true },

        /*负面Buff*/
        /*精神不佳(低速度)*/new BuffConfig(){ Buff_ID = 2000, Buff_Icon = true },
        /*精神萎靡(概率原地睡眠)*/new BuffConfig(){ Buff_ID = 2001, Buff_Icon = true },
        /*轻度饥饿(降低精神)*/new BuffConfig(){ Buff_ID = 2010, Buff_Icon = true },
        /*极度饥饿(降低生命值)*/new BuffConfig(){ Buff_ID = 2011, Buff_Icon = true },
        /*流血(降低生命值)*/new BuffConfig(){ Buff_ID = 2020, Buff_Icon = true },
        /*中毒(低速度)*/new BuffConfig(){ Buff_ID = 2021, Buff_Icon = true },
        /*天黑了*/new BuffConfig(){ Buff_ID = 2030, Buff_Icon = true },
    };
}
public struct BuffConfig
{
    public short Buff_ID;
    public bool Buff_Icon;
}
