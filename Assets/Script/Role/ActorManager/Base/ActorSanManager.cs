using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorSanManager 
{
    private ActorManager actorManager;
    /// <summary>
    /// �����ʱ��
    /// </summary>
    private int timer_San;
    /// <summary>
    /// �ֿ������½�������
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
