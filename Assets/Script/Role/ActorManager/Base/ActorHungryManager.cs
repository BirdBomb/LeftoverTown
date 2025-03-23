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
    private int int_ReHungry = 10;
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
            if (actorManager.actorNetManager.Net_FoodCur + val > 0)
            {
                actorManager.actorNetManager.RPC_LocalInput_FoodChange((short)(actorManager.actorNetManager.Net_FoodCur + val));
            }
            else
            {
                actorManager.actorNetManager.RPC_LocalInput_FoodChange(0);
            }
        }
        return actorManager.actorNetManager.Net_FoodCur;
    }
    public int AddFood(int val)
    {
        if (actorManager.actorAuthority.isPlayer && actorManager.actorAuthority.isLocal)
        {
            if (actorManager.actorNetManager.Net_FoodCur + val <= actorManager.actorNetManager.Net_FoodMax)
            {
                actorManager.actorNetManager.RPC_LocalInput_FoodChange((short)(actorManager.actorNetManager.Net_FoodCur + val));
            }
            else
            {
                actorManager.actorNetManager.RPC_LocalInput_FoodChange(actorManager.actorNetManager.Net_FoodMax);
            }
        }
        return actorManager.actorNetManager.Net_FoodCur;
    }
}
