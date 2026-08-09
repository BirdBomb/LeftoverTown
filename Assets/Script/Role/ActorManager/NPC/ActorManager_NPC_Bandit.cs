using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Fusion;
using System;
/// <summary>
/// 土匪
/// </summary>
public class ActorManager_NPC_Bandit : ActorManager_NPC
{
    #region//检查
    /// <summary>
    /// 检查附近角色
    /// </summary>
    /// <returns>终止思考</returns>
    public override bool State_CheckNearbyActor()
    {
        var temp = brainManager.State_GetNearbyActors();
        foreach (var actor in temp)
        {
            if (!actionManager.LookAt(actor, State_CalculateView())) continue;
            if (actor.actorAuthority.isPlayer)
            {
                State_InAttack(actor);
                return true;
            }
            if (actor.actorNetManager.Local_Fine < 100)
            {
                State_InAttack(actor);
                return true;
            }
            if (actor.statusManager.statusType == StatusType.Monster_Common)
            {
                State_InAttack(actor);
                return true;
            }
        }
        return false;
    }
    #endregion
    #region//行为逻辑
    public override void State_ThinkByTimeUpdate(int date, int hour, GlobalTime time)
    {
        switch (time)
        {
            case GlobalTime.Forenoon:
                {
                    if (State_Think_GoToSleep()) return;
                    break;
                }
            case GlobalTime.Highnoon:
                {
                    if (State_Think_GoForFood()) return;
                    break;
                }
            case GlobalTime.Dusk:
                {
                    if (State_Think_GoToWork()) return;
                    break;
                }
            case GlobalTime.Evening:
                {
                    if (State_Think_GoToWork()) return;
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
        switch (globalTime)
        {
            case GlobalTime.Morning:
                State_PutDownHand();
                break;
            case GlobalTime.Forenoon:
                StartCoroutine(State_Think_FindSleepPos());
                break;
            case GlobalTime.Highnoon:
                StartCoroutine(State_Think_FindFoodPos());
                break;
            case GlobalTime.Dusk:
                StartCoroutine(State_Think_FindWorkPos());
                State_PutOnHand(State_ChooseWeapon);
                break;
            case GlobalTime.Evening:
                StartCoroutine(State_Think_FindWorkPos());
                State_PutOnHand(State_ChooseWeapon);
                break;
        }
    }
    public override bool State_Think_CheckWorkPlace(BuildingTile buildingTile)
    {
        return buildingTile.tileID == 2101;
    }
    #endregion
    #region//攻击逻辑
    public override void State_OutAttack()
    {
        if (brainManager.globalTime_Now != GlobalTime.Evening && brainManager.globalTime_Now != GlobalTime.Dusk)
        {
            State_PutDownHand();
        }
        base.State_OutAttack();
    }
    #endregion
    #region//威胁逻辑
    public override void State_InThreatened(ActorManager actor)
    {

    }
    #endregion
    #region//交互
    public override bool Local_IsInteractable()
    {
        return false;
    }
    #endregion
}
