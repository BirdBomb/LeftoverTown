using Fusion;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class BuildingObj_RabbitHole : BuildingObj
{
    private ActorManager_Animal_Rabbit rabbit;
    public override void Start()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_All_UpdateHour>().Subscribe(_ =>
        {
            ListenTimeUpdate(_.now);
        }).AddTo(this);
        base.Start();
    }
    private void ListenTimeUpdate(GlobalTime globalTime)
    {
        if (globalTime == GlobalTime.Morning)
        {
            if (rabbit == null) CreateRabbit();
        }
    }
    private void CreateRabbit()
    {
        MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnActor()
        {
            name = "Actor/Animal_Rabbit",
            pos = transform.position,
            callBack = ((actor) =>
            {
                rabbit = actor.GetComponent<ActorManager_Animal_Rabbit>();
                rabbit.State_BindRabbitHole(buildingTile.tilePos);
            })
        });
    }

}
