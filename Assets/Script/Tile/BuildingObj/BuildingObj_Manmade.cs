using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingObj_Manmade : BuildingObj
{
    public override int Local_TakeDamage(int val, DamageState damageState, ActorNetManager from)
    {
        if (damageState == DamageState.AttackStructureDamage)
        {
            return base.Local_TakeDamage(val, damageState, from);
        }
        else
        {
            Local_IneffectiveDamage(damageState, from);
            return 0;
        }
    }
}
