using Fusion;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class BuildingObj_Home_Hunter : BuildingObj_Manmade
{
    private ActorManager actor_Bind;
    public override void Start()
    {
        CreateActor();
        MessageBroker.Default.Receive<GameEvent.GameEvent_All_UpdateHour>().Subscribe(_ =>
        {
            All_UpdateHour(_.hour);
        }).AddTo(this);
        base.Start();
    }
    public void All_UpdateHour(int hour)
    {
        if (actor_Bind == null && hour == 0) { CreateActor(); }
    }
    private void CreateActor()
    {
        MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnActor()
        {
            name = "Actor/NPC_Hunter",
            pos = transform.position,
            callBack = ((actor) =>
            {
                actor_Bind = actor.GetComponent<ActorManager>();
                actor_Bind.brainManager.State_SetHomePos(buildingTile.tilePos);
                actor_Bind.brainManager.State_SetActivityPos(buildingTile.tilePos);
            })
        });
    }

}
