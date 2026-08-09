using Fusion;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public partial class BuffSystem_100
{
}
/// <summary>
/// ÑªÁ¿
/// </summary>
public class Buff100 : BuffBase
{

}
/// <summary>
/// ¼¢¶ö
/// </summary>
public class Buff101 : BuffBase
{
    public enum BuffState
    {
        Default,
        VeryHungry,
        LittleHungry,
        NotHungry,
        Full,
    }
    public BuffState state;
    public override void Listen_Local_Init(ActorManager actor)
    {
        UpdateState();
        base.Listen_Local_Init(actor);
    }
    public override void Listen_Local_UpdateHungry(ActorManager actor)
    {
        if (actor.actorAuthority.isPlayer)
        {
            float val = actor.hungryManager.GetFoodRatio();
            if (val <= 0) { ChangeState(BuffState.VeryHungry); }
            if (val > 0 && val <= 0.2) { ChangeState(BuffState.LittleHungry); }
            if (val > 0.2 && val <= 0.8) { ChangeState(BuffState.NotHungry); }
            if (val > 0.8) { ChangeState(BuffState.Full); }
        }
        base.Listen_Local_UpdateHungry(actor);
    }
    public override void Listen_Local_UpdateSecond(ActorManager actor)
    {
        switch ((BuffState)data.BuffVal)
        {
            case BuffState.Default:
                break;
            case BuffState.VeryHungry:
                OnVeryHungry(actor);
                break;
            case BuffState.LittleHungry:
                OnLittleHungry(actor);
                break;
            case BuffState.NotHungry:
                break;
            case BuffState.Full:
                break;
        }

        base.Listen_Local_UpdateSecond(actor);
    }
    private void OnVeryHungry(ActorManager actor)
    {
        actor.actionManager.TakeDamage(5, DamageState.RealDamage, null);
    }
    private void OnLittleHungry(ActorManager actor)
    {
        actor.sanManager.SubSan(1);
    }
    private void ChangeState(BuffState buffState)
    {
        if(data.BuffVal == (short)buffState) { return; }
        data.BuffVal = (short)buffState;
        UpdateState();
    }
    private void UpdateState()
    {
        switch ((BuffState)data.BuffVal)
        {
            case BuffState.Default:
                break;
            case BuffState.VeryHungry:
                MessageBroker.Default.Publish(new UIEvent.UIEvent_AddBuff()
                {
                    buffID = data,
                    image = "Buff_101_State0",
                    desc = LocalizationManager.Instance.GetLocalization("Buff_String", "Buff_101_State0"),
                });
                break;
            case BuffState.LittleHungry:
                MessageBroker.Default.Publish(new UIEvent.UIEvent_AddBuff()
                {
                    buffID = data,
                    image = "Buff_101_State1",
                    desc = LocalizationManager.Instance.GetLocalization("Buff_String", "Buff_101_State1"),
                });
                break;
            case BuffState.NotHungry:
                MessageBroker.Default.Publish(new UIEvent.UIEvent_SubBuff()
                {
                    buffID = data.BuffID,
                });
                break;
            case BuffState.Full:
                MessageBroker.Default.Publish(new UIEvent.UIEvent_AddBuff()
                {
                    buffID = data,
                    image = "Buff_101_State3",
                    desc = LocalizationManager.Instance.GetLocalization("Buff_String", "Buff_101_State3"),
                });
                break;
        }

    }
}
/// <summary>
/// ¾«Éñ
/// </summary>
public class Buff102 : BuffBase
{
    public enum BuffState
    {
        Default,
        LowSanity,
        NormalSanity,
    }
    public override void Listen_Local_Init(ActorManager actor)
    {
        UpdateState();
        base.Listen_Local_Init(actor);
    }
    public override void Listen_Local_UpdateSan(ActorManager actor)
    {
        if (actor.actorAuthority.isPlayer)
        {
            float val = actor.sanManager.GetSanRatio();
            if (val < 0.3f) { ChangeState(BuffState.LowSanity); }
            else { ChangeState(BuffState.NormalSanity); }
        }
        base.Listen_Local_UpdateSan(actor);
    }
    private void ChangeState(BuffState buffState)
    {
        if (data.BuffVal == (short)buffState) { return; }
        data.BuffVal = (short)buffState;
        UpdateState();
    }
    private void UpdateState()
    {
        switch ((BuffState)data.BuffVal)
        {
            case BuffState.Default:
                break;
            case BuffState.LowSanity:
                MessageBroker.Default.Publish(new UIEvent.UIEvent_AddBuff()
                {
                    buffID = data,
                    image = "Buff_102_State0",
                    desc = LocalizationManager.Instance.GetLocalization("Buff_String", "Buff_102_State0"),
                });
                break;
            case BuffState.NormalSanity:
                MessageBroker.Default.Publish(new UIEvent.UIEvent_AddBuff()
                {
                    buffID = data,
                    image = "Buff_102_State1",
                    desc = LocalizationManager.Instance.GetLocalization("Buff_String", "Buff_102_State1"),
                });
                break;
        }

    }
}
