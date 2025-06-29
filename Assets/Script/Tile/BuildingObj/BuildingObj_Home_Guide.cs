using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class BuildingObj_Home_Guide : BuildingObj
{
    private ActorManager_Animal_Guide guide;
    public override void Start()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_All_UpdateHour>().Subscribe(_ =>
        {
            ListenTimeUpdate(_.now);
        }).AddTo(this);
        CreateRabbit();
        base.Start();
    }
    private void ListenTimeUpdate(GlobalTime globalTime)
    {
        CreateRabbit();
    }
    private void CreateRabbit()
    {
        if(guide == null)
        {
            MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnActor()
            {
                name = "Actor/Animal_Guide",
                pos = transform.position,
                callBack = ((actor) =>
                {
                    guide = actor.GetComponent<ActorManager_Animal_Guide>();
                    guide.State_BindRabbitHole(buildingTile.tilePos);
                })
            });
        }
    }
}
