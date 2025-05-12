using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class BuildingObj_SecurityStation : BuildingObj
{
    private ActorManager_NPC_Security security;
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
            if (security == null) CreateRabbit();
        }
    }
    private void CreateRabbit()
    {
        MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnActor()
        {
            name = "Actor/NPC_Security",
            pos = transform.position,
            callBack = ((actor) =>
            {
                security = actor.GetComponent<ActorManager_NPC_Security>();
                security.State_BindStation(buildingTile.tilePos);
            })
        });
    }
}
