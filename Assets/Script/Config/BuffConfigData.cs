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
        /*����Buff*/
        /*�Ա�����(�ָ�����)*/new BuffConfig(){ Buff_ID = 1000, Buff_Icon = true },
        /*�������(���ٶ�)*/new BuffConfig(){ Buff_ID = 1001, Buff_Icon = true },

        /*����Buff*/
        /*���񲻼�(���ٶ�)*/new BuffConfig(){ Buff_ID = 2000, Buff_Icon = true },
        /*����ή��(����ԭ��˯��)*/new BuffConfig(){ Buff_ID = 2001, Buff_Icon = true },
        /*��ȼ���(���;���)*/new BuffConfig(){ Buff_ID = 2010, Buff_Icon = true },
        /*���ȼ���(��������ֵ)*/new BuffConfig(){ Buff_ID = 2011, Buff_Icon = true },
        /*��Ѫ(��������ֵ)*/new BuffConfig(){ Buff_ID = 2020, Buff_Icon = true },
        /*�ж�(���ٶ�)*/new BuffConfig(){ Buff_ID = 2021, Buff_Icon = true },
        /*�����*/new BuffConfig(){ Buff_ID = 2030, Buff_Icon = true },
    };
}
public struct BuffConfig
{
    public short Buff_ID;
    public bool Buff_Icon;
}
