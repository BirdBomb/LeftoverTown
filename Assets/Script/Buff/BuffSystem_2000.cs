using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public partial class BuffSystem_2000 
{
    
}
/// <summary>
/// ÇÖÏ®
/// </summary>
public class Buff2000 : BuffBase
{
    private int temp = 0;
    public override void Listen_Local_Init(ActorManager actor)
    {
        if (actor.actorAuthority.isPlayer)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_AddBuff()
            {
                buffID = data,
                image = "Buff_2000",
                desc = LocalizationManager.Instance.GetLocalization("Buff_String", "Buff_2000"),
            });
        }
        base.Listen_Local_Init(actor);
    }
    public override void Listen_Local_UpdateSecond(ActorManager actor)
    {
        temp += 10;
        actor.actionManager.TakeDamage(temp, DamageState.RealDamage, null);
        base.Listen_Local_UpdateSecond(actor);
    }
}
