using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class BuildingObj_Tombstone : BuildingObj
{
    private ActorManager_Zombie_Common zombie;
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
            if (zombie == null) CreateZombie();
        }
    }
    private void CreateZombie()
    {
        MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnActor()
        {
            name = "Actor/Zombie_Common",
            pos = transform.position,
            callBack = ((actor) =>
            {
                zombie = actor.GetComponent<ActorManager_Zombie_Common>();
                zombie.State_BindHome(buildingTile.tilePos);
            })
        });
    }
}
