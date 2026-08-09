using Fusion;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class BuildingObj_Home_Mayor : BuildingObj_Manmade
{
    private ActorManager actor_Bind;
    public override void Start()
    {
        All_CreateActor();
        base.Start();
    }
    private void All_CreateActor()
    {
        if (actor_Bind != null) return;
        MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnActor()
        {
            name = "Actor/NPC_Mayor",
            pos = transform.position,
            callBack = ((actor) =>
            {
                actor_Bind = actor.GetComponent<ActorManager>();
                actor_Bind.brainManager.ForState_SetHomePos(buildingTile.tilePos);
                actor_Bind.brainManager.ForState_SetActivityPos(buildingTile.tilePos);
            })
        });
    }
}
