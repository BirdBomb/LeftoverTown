using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class BuildingObj_Home_TeethDog : BuildingObj_ResourcePoint
{
    private ActorManager actor_Bind;

    public override void All_UpdateHour(int hour)
    {
        if (actor_Bind == null && hour == 1) { CreateActor(); }
        base.All_UpdateHour(hour);
    }
    private void CreateActor()
    {
        MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnActor()
        {
            name = "Actor/Animal_TeethDog",
            pos = transform.position,
            callBack = ((actor) =>
            {
                actor_Bind = actor.GetComponent<ActorManager>();
                actor_Bind.brainManager.State_SetHomePos(buildingTile.tilePos);
            })
        });
    }

}
