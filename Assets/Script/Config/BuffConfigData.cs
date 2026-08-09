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
        /*基础Buff*/
        new BuffConfig(){ Buff_ID = 100 },/*血量*/
        new BuffConfig(){ Buff_ID = 101 },/*饥饿*/
        new BuffConfig(){ Buff_ID = 102 },/*精神*/
        /*正面Buff*/
        new BuffConfig(){ Buff_ID = 1001 },/*睡眠*/
        new BuffConfig(){ Buff_ID = 1002 },/*坐下*/

        /*负面Buff*/
        new BuffConfig(){ Buff_ID = 2000 },/*侵袭*/
        new BuffConfig(){ Buff_ID = 2001 },/*牵引*/

        /*天赋*/
        new BuffConfig(){ Buff_ID = 10000 }/*消化1*/,
        new BuffConfig(){ Buff_ID = 10001 }/*消化2*/,
        new BuffConfig(){ Buff_ID = 10002 }/*消化3*/,
        new BuffConfig(){ Buff_ID = 10010 }/*睡眠1*/,
        new BuffConfig(){ Buff_ID = 10011 }/*睡眠2*/,
        new BuffConfig(){ Buff_ID = 10012 }/*睡眠3*/,
        new BuffConfig(){ Buff_ID = 10020 }/*鼓点1*/,
        new BuffConfig(){ Buff_ID = 10021 }/*鼓点2*/,
        new BuffConfig(){ Buff_ID = 10022 }/*鼓点3*/,
        new BuffConfig(){ Buff_ID = 10023 }/*鼓点4*/,
    };
}
public struct BuffConfig
{
    public short Buff_ID;
}
