using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public partial class BuffSystem_1000 
{
    
}
/// <summary>
/// Ë¯Ãß
/// </summary>
public class Buff1001 : BuffBase
{
    private int temp = 0;
    public override void Listen_Local_Init(ActorManager actor)
    {
        Listen_Local_UpdateHungry(actor);
        if (actor.actorAuthority.isPlayer)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_AddBuff()
            {
                buffID = data,
                image = "Buff_1001_Lv1",
                desc = LocalizationManager.Instance.GetLocalization("Buff_String", "Buff_1001_Lv1"),
            });
        }
        base.Listen_Local_Init(actor);
    }
    public override void Listen_Local_UpdateSecond(ActorManager actor)
    {
        var bodyActionType = actor.bodyController.GetBodyAction().bodyActionType;
        if (bodyActionType != BodyActionType.LayToRight && bodyActionType != BodyActionType.LayToLeft && bodyActionType != BodyActionType.LayToUp)
        {
            if (MapManager.Instance.GetBuildingObj(data.BuffPos).TryGetComponent(out BuildingObj_Bed bed))
            {
                bed.Local_EndingSleep(actor.actorNetManager.Object.Id);
            }
            actor.actorNetManager.Local_RemoveBuff(data.BuffID);
        }
        else
        {
            temp++;
            if (temp >= 5)
            {
                temp = 0;
                actor.sanManager.AddSan(1);
            }
            var obj = PoolManager.Instance.GetEffectObj("Effect/Effect_ZZZ");
            obj.transform.position = actor.transform.position;
        }
        base.Listen_Local_UpdateSecond(actor);
    }
}
public class Buff1002 : BuffBase
{
    private int temp = 0;
    public override void Listen_Local_Init(ActorManager actor)
    {
        Listen_Local_UpdateHungry(actor);
        if (actor.actorAuthority.isPlayer)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_AddBuff()
            {
                buffID = data,
                image = "Buff_1002",
                desc = LocalizationManager.Instance.GetLocalization("Buff_String", "Buff_1002"),
            });
        }
        base.Listen_Local_Init(actor);
    }
    public override void Listen_Local_UpdateSecond(ActorManager actor)
    {
        var bodyActionType = actor.bodyController.GetBodyAction().bodyActionType;
        if (bodyActionType != BodyActionType.SitToDown && bodyActionType != BodyActionType.SitToLeft && bodyActionType != BodyActionType.SitToRight)
        {
            var obj = MapManager.Instance.GetBuildingObj(data.BuffPos);
            if (obj && obj.TryGetComponent(out BuildingObj_Chair chair))
            {
                chair.Local_EndingSit(actor.actorNetManager.Object.Id);
            }
            actor.actorNetManager.Local_RemoveBuff(data.BuffID);
        }
        else
        {
            temp++;
            if (temp >= 5)
            {
                temp = 0;
            }
        }
        base.Listen_Local_UpdateSecond(actor);
    }
}
