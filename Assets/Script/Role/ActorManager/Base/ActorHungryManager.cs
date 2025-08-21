using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorHungryManager 
{
    private ActorManager actorManager;
    /// <summary>
    /// ¼¢¶ö¼ÆÊ±Æ÷
    /// </summary>
    private int timer_Hungry;
    /// <summary>
    /// µÖ¿¹¼¢¶öÄÜÁ¦
    /// </summary>
    private int int_ReHungry = 20;
    public void Bind(ActorManager actorManager)
    {
        this.actorManager = actorManager;
    }
    public void Listen_UpdateSecond()
    {
        timer_Hungry += 1;
        if (timer_Hungry > int_ReHungry)
        {
            timer_Hungry = 0;
            SubFood(-1);
        }
    }
    public int SubFood(int val)
    {
        if (actorManager.actorAuthority.isPlayer && actorManager.actorAuthority.isLocal)
        {
            int Net_FoodNew = actorManager.actorNetManager.Net_FoodCur + val;
            if (Net_FoodNew <= 0)
            {
                if (!actorManager.buffManager.Local_CheckBuff(2011))
                {
                    BuffData buffData = new BuffData(2011);
                    actorManager.actorNetManager.Local_AddBuff(buffData);
                }
            }
            else if (Net_FoodNew < 30) 
            {
                if (!actorManager.buffManager.Local_CheckBuff(2010))
                {
                    BuffData buffData = new BuffData(2010);
                    actorManager.actorNetManager.Local_AddBuff(buffData);
                }
            }
            actorManager.actorNetManager.RPC_LocalInput_FoodChange((short)val);
        }
        return actorManager.actorNetManager.Net_FoodCur;
    }
    public int AddFood(int val)
    {
        if (actorManager.actorAuthority.isPlayer && actorManager.actorAuthority.isLocal)
        {
            actorManager.actorNetManager.RPC_LocalInput_FoodChange((short)val);
        }
        return actorManager.actorNetManager.Net_FoodCur;
    }
}
