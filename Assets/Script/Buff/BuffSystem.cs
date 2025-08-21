using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BuffSystem
{
}
/*吃饱喝足*/
public class Buff1000 : BuffBase
{

}
/*精神焕发*/
public class Buff1001 : BuffBase
{

}
/*精神不佳*/
public class Buff2000 : BuffBase 
{

}
/*精神萎靡*/
public class Buff2001 : BuffBase
{

}
/*轻度饥饿*/
public class Buff2010 : BuffBase
{
    public override void Listen_UpdateSecond(ActorManager actor)
    {
        if(actor.actorAuthority.isPlayer && actor.actorAuthority.isLocal)
        {
            Local_Check(actor);
            Local_LoseSan(actor);
        }
        base.Listen_UpdateSecond(actor);
    }
    private void Local_Check(ActorManager actor)
    {
        if (actor.actorNetManager.Net_FoodCur > 30)
        {
            actor.actorNetManager.Local_SubBuff(2010);
        }
    }
    /// <summary>
    /// 损失精神值的CD
    /// </summary>
    private int sanLose_CD = 5;
    private int temp;
    private void Local_LoseSan(ActorManager actor)
    {
        if (temp > sanLose_CD)
        {
            actor.actorNetManager.RPC_LocalInput_SanChange(-1);
            temp = 0;
        }
        else
        {
            temp++;
        }
    }
}
/*极度饥饿*/
public class Buff2011 : BuffBase
{
    public override void Listen_UpdateSecond(ActorManager actor)
    {
        if (actor.actorAuthority.isPlayer && actor.actorAuthority.isLocal)
        {
            Local_Check(actor);
            Local_LoseHp(actor);
        }
        base.Listen_UpdateSecond(actor);
    }
    private void Local_Check(ActorManager actor)
    {
        if (actor.actorNetManager.Net_FoodCur > 0)
        {
            actor.actorNetManager.Local_SubBuff(2011);
        }
    }
    private void Local_LoseHp(ActorManager actor)
    {
        actor.actorNetManager.RPC_AllClient_HpChange(-1, (int)HpChangeReason.RealDamage, new NetworkId());
    }
}
/*流血*/
public class Buff2020 : BuffBase
{

}
/*中毒*/
public class Buff2021 : BuffBase
{

}
/*天黑*/
public class Buff2030 : BuffBase
{

}


