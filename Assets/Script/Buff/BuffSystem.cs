using Fusion;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public partial class BuffSystem
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
        State0,
        State1,
        State2,
        State3,
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
            if (val <= 0) { ChangeState(BuffState.State0); }
            if (val > 0 && val <= 0.2) { ChangeState(BuffState.State1); }
            if (val > 0.2 && val <= 0.8) { ChangeState(BuffState.State2); }
            if (val > 0.8) { ChangeState(BuffState.State3); }
        }
        base.Listen_Local_UpdateHungry(actor);
    }
    public override void Listen_Local_UpdateSecond(ActorManager actor)
    {
        switch ((BuffState)data.BuffVal)
        {
            case BuffState.Default:
                break;
            case BuffState.State0:
                actor.actorHpManager.TakeDamage(1,DamageState.RealDamage,null);
                break;
            case BuffState.State1:
                actor.sanManager.SubSan(-1);
                break;
            case BuffState.State2:
                break;
            case BuffState.State3:
                break;
        }

        base.Listen_Local_UpdateSecond(actor);
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
            case BuffState.State0:
                MessageBroker.Default.Publish(new UIEvent.UIEvent_AddBuff()
                {
                    buffID = data,
                    image = "Buff_101_State0",
                    desc = LocalizationManager.Instance.GetLocalization("Buff_String", "Buff_101_State0"),
                });
                break;
            case BuffState.State1:
                MessageBroker.Default.Publish(new UIEvent.UIEvent_AddBuff()
                {
                    buffID = data,
                    image = "Buff_101_State1",
                    desc = LocalizationManager.Instance.GetLocalization("Buff_String", "Buff_101_State1"),
                });
                break;
            case BuffState.State2:
                MessageBroker.Default.Publish(new UIEvent.UIEvent_SubBuff()
                {
                    buffID = data.BuffID,
                });
                break;
            case BuffState.State3:
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
        State0,
        State1,
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
            if (val < 0.3f) { ChangeState(BuffState.State0); }
            else { ChangeState(BuffState.State1); }
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
            case BuffState.State0:
                MessageBroker.Default.Publish(new UIEvent.UIEvent_AddBuff()
                {
                    buffID = data,
                    image = "Buff_102_State0",
                    desc = LocalizationManager.Instance.GetLocalization("Buff_String", "Buff_102_State0"),
                });
                break;
            case BuffState.State1:
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
        if (actor.bodyController.bodyAction_Cur != BodyAction.LayToRight && actor.bodyController.bodyAction_Cur != BodyAction.LayToLeft && actor.bodyController.bodyAction_Cur != BodyAction.LayToUp)
        {
            if (MapManager.Instance.GetBuildingObj(data.BuffPos).TryGetComponent(out BuildingObj_Bed bed))
            {
                bed.Local_EndingSleep(actor);
            }
            actor.actorNetManager.Local_RemoveBuff(1001);
        }
        else
        {
            temp++;
            if (temp >= 5)
            {
                temp = 0;
                actor.sanManager.AddSan(1);
            }
        }
        base.Listen_Local_UpdateSecond(actor);
    }
}
/// <summary>
/// ÎÂÅ¯µÄÎ¸1
/// </summary>
public class Buff10000 : BuffBase
{
    private int temp = 0;
    private bool running = false;
    public override void Listen_Local_UpdateSecond(ActorManager actor)
    {
        if(actor.buffManager.Local_CheckBuff(101,out Buff101 buff) && buff.data.BuffVal == (short)Buff101.BuffState.State3)
        {
            if (!running)
            {
                running = true;
                MessageBroker.Default.Publish(new UIEvent.UIEvent_AddBuff()
                {
                    buffID = data,
                    image = "Buff_10000",
                    desc = LocalizationManager.Instance.GetLocalization("Buff_String", "Buff_10000"),
                });
            }
            temp++;
            if (temp >= 5)
            {
                temp = 0;
                actor.actorHpManager.HealHp(1);
            }
        }
        else
        {
            if (running)
            {
                running = false;
                MessageBroker.Default.Publish(new UIEvent.UIEvent_SubBuff()
                {
                    buffID = data.BuffID,
                });
            }
        }
        base.Listen_Local_UpdateSecond(actor);
    }
}
/// <summary>
/// ÎÂÅ¯µÄÎ¸2
/// </summary>
public class Buff10001 : BuffBase
{
    private short armor = 1;
    private bool running = false;
    public override void Listen_Local_UpdateSecond(ActorManager actor)
    {
        if (actor.buffManager.Local_CheckBuff(101, out Buff101 buff))
        {
            if (running)
            {
                if (buff.data.BuffVal != (short)Buff101.BuffState.State3)
                {
                    running = false;
                    actor.actorNetManager.RPC_LocalInput_ChangeArmor((short)-armor);
                    MessageBroker.Default.Publish(new UIEvent.UIEvent_SubBuff()
                    {
                        buffID = data.BuffID,
                    });
                }
            }
            else
            {
                if (buff.data.BuffVal == (short)Buff101.BuffState.State3)
                {
                    running = true;
                    actor.actorNetManager.RPC_LocalInput_ChangeArmor(armor);
                    MessageBroker.Default.Publish(new UIEvent.UIEvent_AddBuff()
                    {
                        buffID = data,
                        image = "Buff_10001",
                        desc = LocalizationManager.Instance.GetLocalization("Buff_String", "Buff_10001"),
                    });
                }
            }
        }
        base.Listen_Local_UpdateSecond(actor);
    }
}
/// <summary>
/// µ¯Á¦Î¸´ü
/// </summary>
public class Buff10002 : BuffBase
{
    private int temp = 0;
    private int state = 0;
    public override void Listen_Local_UpdateSecond(ActorManager actor)
    {
        if (actor.buffManager.Local_CheckBuff(101, out Buff101 buff))
        {
            if (buff.data.BuffVal == (short)Buff101.BuffState.State0)
            {
                temp++;
                if (temp >= 5)
                {
                    temp = 0;
                    actor.hungryManager.AddFood(1); 
                }
                if (state != 2)
                {
                    state = 2;
                    MessageBroker.Default.Publish(new UIEvent.UIEvent_AddBuff()
                    {
                        buffID = data,
                        image = "Buff_10002",
                        desc = LocalizationManager.Instance.GetLocalization("Buff_String", "Buff_10002_State1"),
                    });
                }
            }
            else if (buff.data.BuffVal == (short)Buff101.BuffState.State3)
            {
                temp++;
                if (temp >= 5)
                {
                    temp = 0;
                    actor.hungryManager.SubFood(-1);
                }
                if (state != 1)
                {
                    state = 1;
                    MessageBroker.Default.Publish(new UIEvent.UIEvent_AddBuff()
                    {
                        buffID = data,
                        image = "Buff_10002",
                        desc = LocalizationManager.Instance.GetLocalization("Buff_String", "Buff_10002_State0"),
                    });
                }
            }
            else
            {
                if (state != 0)
                {
                    state = 0;
                    MessageBroker.Default.Publish(new UIEvent.UIEvent_SubBuff()
                    {
                        buffID = data.BuffID,
                    });
                }
            }
        }
        else
        {
            if(state != 0)
            {
                state = 0;
                MessageBroker.Default.Publish(new UIEvent.UIEvent_SubBuff()
                {
                    buffID = data.BuffID,
                });
            }
        }
        base.Listen_Local_UpdateSecond(actor);
    }

}
/// <summary>
/// ÎÂÅ¯Ë¯Ãß1
/// </summary>
public class Buff10010 : BuffBase
{
    private int temp = 0;
    public bool awake;
    private bool running;
    public override void Listen_Local_Init(ActorManager actor)
    {
        if (actor.buffManager.Local_CheckBuff(10011, out BuffBase _)|| actor.buffManager.Local_CheckBuff(10012, out BuffBase _))
        {
            awake = false;
        }
        else
        {
            awake = true;
        }
        base.Listen_Local_Init(actor);
    }
    public override void Listen_Local_UpdateSecond(ActorManager actor)
    {
        if (awake && actor.buffManager.Local_CheckBuff(1001, out BuffBase buff))
        {
            temp++;
            if (temp >= 5)
            {
                temp = 0;
                actor.actorHpManager.HealHp(1);
            }
            if (!running)
            {
                running = true;
                MessageBroker.Default.Publish(new UIEvent.UIEvent_AddBuff()
                {
                    buffID = data,
                    image = "Buff_10010",
                    desc = LocalizationManager.Instance.GetLocalization("Buff_String", "Buff_10010"),
                });
            }
        }
        else
        {
            if (running)
            {
                running = false;
                MessageBroker.Default.Publish(new UIEvent.UIEvent_SubBuff() { buffID = data.BuffID });
            }
        }
        base.Listen_Local_UpdateSecond(actor);
    }

}
/// <summary>
/// ÎÂÅ¯Ë¯Ãß2
/// </summary>
public class Buff10011 : BuffBase
{
    private int temp = 0;
    public bool awake;
    private bool running;
    public override void Listen_Local_Init(ActorManager actor)
    {
        if (actor.buffManager.Local_CheckBuff(10012, out BuffBase _))
        {
            awake = false;
        }
        else
        {
            awake = true;
        }
        if (actor.buffManager.Local_CheckBuff(10010, out Buff10020 buff10010))
        {
            buff10010.awake = false;
        }
        base.Listen_Local_Init(actor);
    }
    public override void Listen_Local_UpdateSecond(ActorManager actor)
    {
        if (awake && actor.buffManager.Local_CheckBuff(1001, out BuffBase buff))
        {
            temp++;
            if (temp >= 2)
            {
                temp = 0;
                actor.actorHpManager.HealHp(1);
            }
            if (!running)
            {
                running = true;
                MessageBroker.Default.Publish(new UIEvent.UIEvent_AddBuff()
                {
                    buffID = data,
                    image = "Buff_10011",
                    desc = LocalizationManager.Instance.GetLocalization("Buff_String", "Buff_10011"),
                });
            }
        }
        else
        {
            if (running)
            {
                running = false;
                MessageBroker.Default.Publish(new UIEvent.UIEvent_SubBuff() { buffID = data.BuffID });
            }
        }
        base.Listen_Local_UpdateSecond(actor);
    }
}
/// <summary>
/// Ë¯Ãß°üÖÎ°Ù²¡
/// </summary>
public class Buff10012 : BuffBase
{
    private int temp = 0;
    public bool awake;
    private bool running;
    public override void Listen_Local_Init(ActorManager actor)
    {
        if (actor.buffManager.Local_CheckBuff(10010, out Buff10010 buff10010))
        {
            buff10010.awake = false;
        }
        if (actor.buffManager.Local_CheckBuff(10011, out Buff10011 buff10011))
        {
            buff10011.awake = false;
        }
        base.Listen_Local_Init(actor);
    }

    public override void Listen_Local_UpdateSecond(ActorManager actor)
    {
        if (awake && actor.buffManager.Local_CheckBuff(1001, out BuffBase buff))
        {
            temp++;
            if (temp >= 1)
            {
                temp = 0;
                actor.actorHpManager.HealHp(1);
            }
            if (!running)
            {
                running = true;
                MessageBroker.Default.Publish(new UIEvent.UIEvent_AddBuff()
                {
                    buffID = data,
                    image = "Buff_10012",
                    desc = LocalizationManager.Instance.GetLocalization("Buff_String", "Buff_10012"),
                });
            }
        }
        else
        {
            if (running)
            {
                running = false;
                MessageBroker.Default.Publish(new UIEvent.UIEvent_SubBuff() { buffID = data.BuffID });
            }
        }
        base.Listen_Local_UpdateSecond(actor);
    }

}
/// <summary>
/// ÐÄÔà¹Äµã
/// </summary>
public class Buff10020 : BuffBase
{
    public bool awake;
    public bool running = false;
    private int temp = 0;
    public override void Listen_Local_Init(ActorManager actor)
    {
        if (actor.buffManager.Local_CheckBuff(10021, out BuffBase _) || 
            actor.buffManager.Local_CheckBuff(10022, out BuffBase _) || 
            actor.buffManager.Local_CheckBuff(10023, out BuffBase _))
        {
            awake = false;
        }
        else
        {
            awake = true;
        }
        base.Listen_Local_Init(actor);
    }
    public override void Listen_Local_UpdateSecond(ActorManager actor)
    {
        if (awake && actor.actorHpManager.GetHpRatio() < 0.2f)
        {
            if (!running)
            {
                running = true;
                MessageBroker.Default.Publish(new UIEvent.UIEvent_AddBuff()
                {
                    buffID = data,
                    image = "Buff_10020",
                    desc = LocalizationManager.Instance.GetLocalization("Buff_String", "Buff_10020"),
                });
            }
            temp++;
            if (temp >= 5)
            {
                temp = 0;
                actor.actorHpManager.HealHp(1);
            }
        }
        else
        {
            if (running)
            {
                running = false;
                MessageBroker.Default.Publish(new UIEvent.UIEvent_SubBuff() { buffID = data.BuffID });
            }
        }
        base.Listen_Local_UpdateSecond(actor);
    }
}
/// <summary>
/// ÐÄÔà¹Äµã
/// </summary>
public class Buff10021 : BuffBase
{
    public bool awake = true;
    public bool running = false;
    private int temp = 0;
    public override void Listen_Local_Init(ActorManager actor)
    {
        if (actor.buffManager.Local_CheckBuff(10022, out BuffBase _) ||
            actor.buffManager.Local_CheckBuff(10023, out BuffBase _))
        {
            awake = false;
        }
        else
        {
            awake = true;
        }
        if(actor.buffManager.Local_CheckBuff(10020, out Buff10020 buff10020))
        {
            buff10020.awake = false;
        }
        base.Listen_Local_Init(actor);
    }
    public override void Listen_Local_UpdateSecond(ActorManager actor)
    {
        if (awake && actor.actorHpManager.GetHpRatio() < 0.2f)
        {
            if (!running)
            {
                running = true;
                MessageBroker.Default.Publish(new UIEvent.UIEvent_AddBuff()
                {
                    buffID = data,
                    image = "Buff_10021",
                    desc = LocalizationManager.Instance.GetLocalization("Buff_String", "Buff_10021"),
                });
            }
            temp++;
            if (temp >= 3)
            {
                temp = 0;
                actor.actorHpManager.HealHp(1);
            }
        }
        else
        {
            if (running)
            {
                running = false;
                MessageBroker.Default.Publish(new UIEvent.UIEvent_SubBuff() { buffID = data.BuffID });
            }
        }
        base.Listen_Local_UpdateSecond(actor);
    }
}
/// <summary>
/// ÈÈÁÒµÄ¹Äµã
/// </summary>
public class Buff10022 : BuffBase
{
    public bool awake = true;
    public bool running = false;
    private int temp = 0;
    public override void Listen_Local_Init(ActorManager actor)
    {
        if (actor.buffManager.Local_CheckBuff(10020, out Buff10020 buff10020))
        {
            buff10020.awake = false;
        }
        if (actor.buffManager.Local_CheckBuff(10021, out Buff10021 buff10021))
        {
            buff10021.awake = false;
        }
        if (actor.buffManager.Local_CheckBuff(10021, out Buff10023 buff10023))
        {
            buff10023.awake = false;
        }
        base.Listen_Local_Init(actor);
    }
    public override void Listen_Local_UpdateSecond(ActorManager actor)
    {
        if (awake && actor.actorHpManager.GetHpRatio() < 0.2f)
        {
            if (!running)
            {
                running = true;
                MessageBroker.Default.Publish(new UIEvent.UIEvent_AddBuff()
                {
                    buffID = data,
                    image = "Buff_10022",
                    desc = LocalizationManager.Instance.GetLocalization("Buff_String", "Buff_10022"),
                });
            }
            temp++;
            if (temp >= 1)
            {
                temp = 0;
                actor.actorHpManager.HealHp(1);
            }
        }
        else
        {
            if (running)
            {
                running = false;
                MessageBroker.Default.Publish(new UIEvent.UIEvent_SubBuff() { buffID = data.BuffID });
            }
        }
        base.Listen_Local_UpdateSecond(actor);
    }
}
/// <summary>
/// Êæ»ºµÄ¹Äµã
/// </summary>
public class Buff10023 : BuffBase
{
    public bool awake = true;
    public bool running = false;
    private int temp = 0;
    public override void Listen_Local_Init(ActorManager actor)
    {
        if (actor.buffManager.Local_CheckBuff(10020, out Buff10020 buff10020))
        {
            buff10020.awake = false;
        }
        if (actor.buffManager.Local_CheckBuff(10021, out Buff10021 buff10021))
        {
            buff10021.awake = false;
        }
        if (actor.buffManager.Local_CheckBuff(10022, out Buff10022 buff10022))
        {
            buff10022.awake = false;
        }
        base.Listen_Local_Init(actor);
    }
    public override void Listen_Local_UpdateSecond(ActorManager actor)
    {
        if (awake && actor.actorHpManager.GetHpRatio() < 0.5f)
        {
            if (!running)
            {
                running = true;
                MessageBroker.Default.Publish(new UIEvent.UIEvent_AddBuff()
                {
                    buffID = data,
                    image = "Buff_10023",
                    desc = LocalizationManager.Instance.GetLocalization("Buff_String", "Buff_10023"),
                });
            }
            temp++;
            if (temp >= 3)
            {
                temp = 0;
                actor.actorHpManager.HealHp(1);
            }
        }
        else
        {
            if (running)
            {
                running = false;
                MessageBroker.Default.Publish(new UIEvent.UIEvent_SubBuff() { buffID = data.BuffID });
            }
        }
        base.Listen_Local_UpdateSecond(actor);
    }

}
