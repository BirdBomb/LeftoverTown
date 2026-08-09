using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public partial class BuffSystem_10000 
{
    
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
        if (actor.buffManager.Local_CheckBuff(101, out Buff101 buff) && buff.data.BuffVal == (short)Buff101.BuffState.Full)
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
                actor.actorHpManager.HealHp(10);
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
    private const short short_Armor = 1;
    private bool bool_Running = false;
    public override void Listen_Local_UpdateSecond(ActorManager actor)
    {
        if (actor.buffManager.Local_CheckBuff(101, out Buff101 buff))
        {
            if (bool_Running)
            {
                if (buff.data.BuffVal != (short)Buff101.BuffState.Full)
                {
                    bool_Running = false;
                    actor.actionManager.Local_ResetArmor();
                    MessageBroker.Default.Publish(new UIEvent.UIEvent_SubBuff()
                    {
                        buffID = data.BuffID,
                    });
                }
            }
            else
            {
                if (buff.data.BuffVal == (short)Buff101.BuffState.Full)
                {
                    bool_Running = true;
                    actor.actionManager.Local_ResetArmor();
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
    public override int Local_CalculateArmor(int armor)
    {
        return bool_Running ? armor + short_Armor : armor;
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
            if (buff.data.BuffVal == (short)Buff101.BuffState.VeryHungry)
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
            else if (buff.data.BuffVal == (short)Buff101.BuffState.Full)
            {
                temp++;
                if (temp >= 5)
                {
                    temp = 0;
                    actor.hungryManager.SubFood(1);
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
            if (state != 0)
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
        if (actor.buffManager.Local_CheckBuff(10011, out BuffBase _) || actor.buffManager.Local_CheckBuff(10012, out BuffBase _))
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
                actor.actorHpManager.HealHp(10);
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
                actor.actorHpManager.HealHp(10);
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
                actor.actorHpManager.HealHp(10);
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
        if (actor.buffManager.Local_CheckBuff(10021, out BuffBase _) || actor.buffManager.Local_CheckBuff(10022, out BuffBase _) || actor.buffManager.Local_CheckBuff(10023, out BuffBase _))
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
                actor.actorHpManager.HealHp(10);
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
        if (actor.buffManager.Local_CheckBuff(10022, out BuffBase _) || actor.buffManager.Local_CheckBuff(10023, out BuffBase _))
        {
            awake = false;
        }
        else
        {
            awake = true;
        }
        if (actor.buffManager.Local_CheckBuff(10020, out Buff10020 buff10020))
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
                actor.actorHpManager.HealHp(10);
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
                actor.actorHpManager.HealHp(10);
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
                actor.actorHpManager.HealHp(10);
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
/// ³¬ÈËÌåÖÊ
/// </summary>
public class Buff10030 : BuffBase
{

}
public class Buff10100 : BuffBase
{

}
public class Buff10101 : BuffBase
{

}
public class Buff10102 : BuffBase
{

}
public class Buff10110 : BuffBase
{

}
public class Buff10111 : BuffBase
{

}
public class Buff10112 : BuffBase
{

}
public class Buff10120 : BuffBase
{

}
public class Buff10121 : BuffBase
{

}
public class Buff10122 : BuffBase
{

}
public class Buff10130 : BuffBase
{

}
public class Buff10131 : BuffBase
{

}
public class Buff10200 : BuffBase
{

}
public class Buff10210 : BuffBase
{

}
public class Buff10220 : BuffBase
{

}
public class Buff10230 : BuffBase
{

}
public class Buff10240 : BuffBase
{

}
public class Buff10300 : BuffBase
{

}
public class Buff10301 : BuffBase
{

}
public class Buff10302 : BuffBase
{

}
public class Buff10303 : BuffBase
{

}
public class Buff10304 : BuffBase
{

}
public class Buff10310 : BuffBase
{

}
public class Buff10320 : BuffBase
{

}
public class Buff10321 : BuffBase
{

}
public class Buff10330 : BuffBase
{

}
public class Buff10331 : BuffBase
{

}
public class Buff10332 : BuffBase
{

}
public class Buff10400 : BuffBase
{

}
public class Buff10401 : BuffBase
{

}
public class Buff10402 : BuffBase
{

}
public class Buff10403 : BuffBase
{

}
public class Buff10404 : BuffBase
{

}
public class Buff10405 : BuffBase
{

}
public class Buff10406 : BuffBase
{

}
public class Buff10407: BuffBase
{

}
public class Buff10408 : BuffBase
{

}
public class Buff10409 : BuffBase
{

}
public class Buff10410 : BuffBase
{

}
public class Buff10411 : BuffBase
{

}