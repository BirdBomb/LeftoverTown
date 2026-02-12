using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorSanManager 
{
    private ActorManager actorManager;
    /// <summary>
    /// 精神计时器
    /// </summary>
    private int timer_San;
    /// <summary>
    /// 抵抗精神下降的能力
    /// </summary>
    private int int_ReSan = 25;
    public void Bind(ActorManager actorManager)
    {
        this.actorManager = actorManager;
    }
    public void Listen_UpdateSecond()
    {
        timer_San += 1;
        if (timer_San > int_ReSan)
        {
            timer_San = 0;
            SubSan(-1);
        }
    }
    public float GetSanRatio()
    {
        if (actorManager.actorNetManager.Local_SanMax > 0)
        {
            return (float)actorManager.actorNetManager.Net_SanCur / (float)actorManager.actorNetManager.Local_SanMax;
        }
        else
        {
            return 0;
        }
    }

    public int SubSan(int val)
    {
        if (actorManager.actorAuthority.isPlayer && actorManager.actorAuthority.isLocal)
        {
            actorManager.actorNetManager.RPC_LocalInput_SanChange((short)val);
        }
        return actorManager.actorNetManager.Net_SanCur;
    }
    public int AddSan(int val)
    {
        if (actorManager.actorAuthority.isPlayer && actorManager.actorAuthority.isLocal)
        {
            actorManager.actorNetManager.RPC_LocalInput_SanChange((short)val);
        }
        return actorManager.actorNetManager.Net_SanCur;
    }

}
