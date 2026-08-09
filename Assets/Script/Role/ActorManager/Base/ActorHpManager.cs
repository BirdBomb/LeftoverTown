using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class ActorHpManager 
{
    private ActorManager actorManager;
    public void Bind(ActorManager actorManager)
    {
        this.actorManager = actorManager;
    }
    public float GetHpRatio()
    {
        if (actorManager.actorNetManager.Local_HpMax > 0)
        {
            return (float)actorManager.actorNetManager.Net_HpCur / actorManager.actorNetManager.Local_HpMax;
        }
        else
        {
            return 0;
        }
    }
    /// <summary>
    /// 治疗
    /// </summary>
    /// <param name="val"></param>
    public void HealHp(int val)
    {
        if (actorManager.actorState == ActorState.Dead) return;
        actorManager.actorNetManager.RPC_AllClient_HpChange(val, (int)HpChangeReason.Healing, new NetworkId());
    }
    /// <summary>
    /// 增加上限
    /// </summary>
    /// <param name="val"></param>
    public void IncreaseHP(int val)
    {
        if (actorManager.actorState == ActorState.Dead) return;
        actorManager.actorNetManager.RPC_AllClient_MaxHpChange((short)val, new NetworkId());
    }
}
