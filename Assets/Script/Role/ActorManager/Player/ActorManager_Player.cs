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
        inputManager.Local_AddInputKeycodeAction(Local_Input);
    }
    public override void AllClient_Listen_RoleInView(ActorManager actor)
    {
        if (actorAuthority.isLocal)
        {
            brainManager.actorManagers_Nearby.Add(actor);
            actor.Local_InPlayerView(this);
        }
        base.AllClient_Listen_RoleInView(actor);
    }
    public override void AllClient_Listen_RoleOutView(ActorManager actor)
    {
        if (actorAuthority.isLocal)
        {
            brainManager.actorManagers_Nearby.Remove(actor);
            actor.Local_OutPlayerView(this);
        }
        base.AllClient_Listen_RoleOutView(actor);
    }
    public void Local_Input(ActorManager actor,KeyCode keyCode)
    {
        if(keyCode == KeyCode.F)
        {
            for (int i = 0; i < brainManager.actorManagers_Nearby.Count; i++)
            {
                brainManager.actorManagers_Nearby[i].Local_GetPlayerInput(keyCode,this);
            }
        }
    }
}
