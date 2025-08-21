using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

public class ActorManager_Player : ActorManager
{
    public void Start()
    {
        inputManager.Local_AddInputKeycodeAction(Local_Listen_Input);
    }
    public override void FixedUpdate()
    {
        if (actorAuthority.isLocal)
        {
            Local_UpdateClosestActor();
        }
        base.FixedUpdate();
    }
    #region//监听
    public override void AllClient_AddListener()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_All_UpdateHour>().Subscribe(_ =>
        {
            AllClient_Listen_UpdateTime(_.hour, _.day, _.now);
        }).AddTo(this);
        base.AllClient_AddListener();
    }
    public override void AllClient_Listen_UpdateTime(int hour, int date, GlobalTime globalTime)
    {
        base.AllClient_Listen_UpdateTime(hour, date, globalTime);
    }
    public override void AllClient_Listen_ItemInView(ItemNetObj obj)
    {
        base.State_Listen_ItemInView(obj);
    }
    #endregion
    #region//交流
    /// <summary>
    /// 最近的可以对话的角色
    /// </summary>
    public ActorManager actorManager_Closest = null;
    public void Local_UpdateClosestActor()
    {
        if (brainManager.actorManagers_Nearby.Count > 0)
        {
            if (brainManager.actorManagers_Nearby.Count == 1)
            {
                if (actorManager_Closest != brainManager.actorManagers_Nearby[0])
                {
                    if (actorManager_Closest != null)
                    { 
                        actorManager_Closest.Local_PlayerFaraway(this);
                        actorManager_Closest = null;
                    }
                    actorManager_Closest = brainManager.actorManagers_Nearby[0];
                    actorManager_Closest.Local_PlayerClose(this);
                }
            }
            else
            {
                float distance_Temp = float.MaxValue;
                ActorManager actorManager_Temp = null;
                for (int i = 0; i < brainManager.actorManagers_Nearby.Count; i++)
                {
                    float temp = Vector3.Distance(transform.position, brainManager.actorManagers_Nearby[i].transform.position);
                    if (temp < distance_Temp)
                    {
                        distance_Temp = temp;
                        actorManager_Temp = brainManager.actorManagers_Nearby[i];
                    }
                }

                if (actorManager_Closest != actorManager_Temp)
                {
                    if (actorManager_Closest != null)
                    {
                        actorManager_Closest.Local_PlayerFaraway(this);
                        actorManager_Closest = null;
                    }
                    actorManager_Closest = actorManager_Temp;
                    actorManager_Closest.Local_PlayerClose(this);
                }
            }
        }
        else
        {
            if (actorManager_Closest != null)
            {
                actorManager_Closest.Local_PlayerFaraway(this);
                actorManager_Closest = null;
            }
        }
    }
    public override void AllClient_Listen_RoleInView(ActorManager actor)
    {
        if (actorAuthority.isLocal)
        {
            if (actor.Local_CanDialog())
            {
                brainManager.actorManagers_Nearby.Add(actor);
            }
        }
        base.AllClient_Listen_RoleInView(actor);
    }
    public override void AllClient_Listen_RoleOutView(ActorManager actor)
    {
        if (actorAuthority.isLocal)
        {
            brainManager.actorManagers_Nearby.Remove(actor);
        }
        base.AllClient_Listen_RoleOutView(actor);
    }
    public void Local_Listen_Input(ActorManager actor, KeyCode keyCode)
    {
        if (keyCode == KeyCode.R)
        {
            if (actorManager_Closest != null && brainManager.actorManagers_Nearby.Contains(actorManager_Closest))
            {
                actorManager_Closest.Local_GetPlayerInput_R(this);
            }
        }
    }

    #endregion
}
