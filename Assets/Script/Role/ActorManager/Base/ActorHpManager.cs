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
            return (float)actorManager.actorNetManager.Net_HpCur / (float)actorManager.actorNetManager.Local_HpMax;
        }
        else
        {
            return 0;
        }
    }
    public void HealHp(int val)
    {
        if (actorManager.actorState != ActorState.Dead)
        {
            actorManager.actorNetManager.RPC_AllClient_HpChange(val, (int)HpChangeReason.Healing, new NetworkId());
        }
    }
    public void IncreaseHP(int val)
    {
        if (actorManager.actorState != ActorState.Dead)
        {
            actorManager.actorNetManager.RPC_AllClient_MaxHpChange((short)val, new NetworkId());
        }
    }
    public void TakeDamage(int val, DamageState damageState, ActorNetManager from)
    {
        if (actorManager.actorState != ActorState.Dead)
        {
            NetworkId networkId = new NetworkId();
            if (from) { networkId = from.Object.Id; }
            if (damageState == DamageState.AttackPiercingDamage)
            {
                val -= actorManager.actorNetManager.Net_Armor;
                if (val > 0)
                {
                    actorManager.actorNetManager.RPC_AllClient_HpChange(-val, (int)HpChangeReason.AttackDamage, networkId);
                }
                else
                {
                    actorManager.actorNetManager.RPC_AllClient_HpChange(0, (int)HpChangeReason.AttackDamage, networkId);
                }
            }
            else if (damageState == DamageState.AttackSlashingDamage)
            {
                val -= actorManager.actorNetManager.Net_Armor;
                if (val > 0)
                {
                    actorManager.actorNetManager.RPC_AllClient_HpChange(-val, (int)HpChangeReason.AttackDamage, networkId);
                }
                else
                {
                    actorManager.actorNetManager.RPC_AllClient_HpChange(0, (int)HpChangeReason.AttackDamage, networkId);
                }
            }
            else if (damageState == DamageState.AttackBludgeoningDamage)
            {
                val -= actorManager.actorNetManager.Net_Armor;
                if (val > 0)
                {
                    actorManager.actorNetManager.RPC_AllClient_HpChange(-val, (int)HpChangeReason.AttackDamage, networkId);
                }
                else
                {
                    actorManager.actorNetManager.RPC_AllClient_HpChange(0, (int)HpChangeReason.AttackDamage, networkId);
                }
            }
            else if (damageState == DamageState.MagicDamage)
            {
                val -= actorManager.actorNetManager.Net_Resistance;
                if (val > 0)
                {
                    actorManager.actorNetManager.RPC_AllClient_HpChange(-val, (int)HpChangeReason.MagicDamage, networkId);
                }
                else
                {
                    actorManager.actorNetManager.RPC_AllClient_HpChange(0, (int)HpChangeReason.MagicDamage, networkId);
                }
            }
            else if (damageState == DamageState.RealDamage)
            {
                actorManager.actorNetManager.RPC_AllClient_HpChange(-val, (int)HpChangeReason.RealDamage, networkId);
            }
            actorManager.bodyController.Flash();
            actorManager.bodyController.Shake();
        }
    }
}
