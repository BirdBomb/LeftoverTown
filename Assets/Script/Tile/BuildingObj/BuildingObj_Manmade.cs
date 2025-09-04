using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingObj_Manmade : BuildingObj
{
    public override void Local_TakeDamage(int val, DamageState damageState, ActorNetManager from)
    {
        if (damageState == DamageState.AttackStructureDamage)
        {
            base.Local_TakeDamage(val, damageState, from);
        }
        else
        {
            Local_IneffectiveDamage(damageState, from);
        }
    }
}
