using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Fusion;
using System;
using static GameEvent;
/// <summary>
/// »¤ÎÀ
/// </summary>
public class ActorManager_NPC_Guard : ActorManager_NPC
{
    private float float_ViewDistance = 10;
    #region//¼àÌý
    public override void ForState_Listen_RoleCommit(GameEvent.GameEvent_AllClient_SomeoneCommit eventData)
    {
        if (actionManager.LookAt(eventData.actor, State_CalculateView()))
        {
            eventData.actor.actionManager.AllClient_SetFine(eventData.commit, eventData.fine);
        }
        base.ForState_Listen_RoleCommit(eventData);
    }
    #endregion
    #region//¼ì²é
    public override bool State_CheckNearbyActor()
    {
        var temp = brainManager.State_GetNearbyActors();
        foreach (var actor in temp)
        {
            if (!actionManager.LookAt(actor, State_CalculateView())) continue;
            if (actor.actorNetManager.Local_Fine > 0)
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
    #region//ÐÐÎªÂß¼­
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
        }
        if (State_Think_GoToWork()) return;
        base.State_ThinkByTimeUpdate(date, hour, time);

    }
    public override void State_ThinkByTimeChange(int date, int hour, GlobalTime globalTime)
    {
        switch (globalTime)
        {
            case GlobalTime.Morning:
                StartCoroutine(State_Think_FindWorkPos());
                State_PutOnHand(State_ChooseWeapon);
                break;
            case GlobalTime.Forenoon:
                State_PutDownHand();
                StartCoroutine(State_Think_FindSleepPos());
                break;
            case GlobalTime.Highnoon:
                StartCoroutine(State_Think_FindFoodPos());
                break;
            case GlobalTime.Afternoon:
                StartCoroutine(State_Think_FindWorkPos());
                State_PutOnHand(State_ChooseWeapon);
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
    public override void State_OutAttack()
    {
        if (brainManager.globalTime_Now == GlobalTime.Forenoon || brainManager.globalTime_Now == GlobalTime.Highnoon)
        {
            State_PutDownHand();
        }
        base.State_OutAttack();
    }
    public override bool State_Think_CheckWorkPlace(BuildingTile buildingTile)
    {
        return buildingTile.tileID == 2011;
    }
    #endregion
    #region//ÍþÐ²Âß¼­
    public override void State_InThreatened(ActorManager actor)
    {
        
    }
    #endregion
    #region//½»»¥
    public override bool Local_IsInteractable()
    {
        return false;
    }
    #endregion
}
