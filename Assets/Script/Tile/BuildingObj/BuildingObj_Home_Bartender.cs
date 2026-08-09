using Fusion;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class BuildingObj_Home_Bartender : BuildingObj_Manmade
{
    private ActorManager actor_Bind;
    public override void Start()
    {
        CreateActor();
        base.Start();
    }
    private void CreateActor()
    {
        if (actor_Bind) return;
        MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnActor()
        {
            name = "Actor/NPC_Bartender",
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
