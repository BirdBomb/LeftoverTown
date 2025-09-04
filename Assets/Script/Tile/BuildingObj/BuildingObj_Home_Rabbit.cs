using Fusion;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class BuildingObj_Home_Rabbit : BuildingObj_Manmade
{
    private ActorManager actor_Bind;
    public override void Start()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_All_UpdateHour>().Subscribe(_ =>
        {
            All_UpdateHour(_.hour);
        }).AddTo(this);
        base.Start();
    }
    public void All_UpdateHour(int hour)
    {
        if (actor_Bind == null && hour == 1) { CreateActor(); }
    }
    private void CreateActor()
    {
        MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnActor()
        {
            name = "Actor/Animal_Rabbit",
            pos = transform.position,
            callBack = ((actor) =>
            {
                actor_Bind = actor.GetComponent<ActorManager>();
                actor_Bind.brainManager.SetHome(buildingTile.tilePos);
            })
        });
    }
}
