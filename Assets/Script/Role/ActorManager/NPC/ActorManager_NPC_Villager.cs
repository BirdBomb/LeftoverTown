using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Fusion.Sockets.NetBitBuffer;
using static GameEvent;
/// <summary>
/// 村民
/// </summary>
public class ActorManager_NPC_Villager : ActorManager_NPC
{
    #region//行为逻辑
    /// <summary>
    /// 根据时间决定动作(经常触发)
    /// </summary>
    public override void State_ThinkByTimeUpdate(int date, int hour, GlobalTime time)
    {
        switch (time)
        {
            case GlobalTime.Morning:
                {
                    if (State_Think_GoForFood()) return;
                    break;
                }
            case GlobalTime.Highnoon:
                {
                    if (State_Think_GoForFood()) return;
                    break;
                }
            case GlobalTime.Evening:
                {
                    if (State_Think_GoToSleep()) return;
                    break;
                }
        }
        base.State_ThinkByTimeUpdate(date, hour, time);
    }
    /// <summary>
    /// 根据时间变化决定动作(关键时间触发)
    /// </summary>
    public override void State_ThinkByTimeChange(int date, int hour, GlobalTime globalTime)
    {
        if (globalTime == GlobalTime.Highnoon) { StartCoroutine(State_Think_FindFoodPos()); }
        if (globalTime == GlobalTime.Dusk) { StartCoroutine(State_Think_FindFoodPos()); }
        if (globalTime == GlobalTime.Evening) { StartCoroutine(State_Think_FindSleepPos()); }
    }
    #endregion
    #region//交互
    public override bool Local_IsInteractable()
    {
        return false;
    }
    #endregion

}
