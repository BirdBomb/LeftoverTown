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
    public float GetFoodRatio()
    {
        if (actorManager.actorNetManager.Local_FoodMax > 0)
        {
            return (float)actorManager.actorNetManager.Net_FoodCur / (float)actorManager.actorNetManager.Local_FoodMax;
        }
        else
        {
            return 0;
        }
    }
    public int SubFood(int val)
    {
        if (actorManager.actorAuthority.isPlayer && actorManager.actorAuthority.isLocal)
        {
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
