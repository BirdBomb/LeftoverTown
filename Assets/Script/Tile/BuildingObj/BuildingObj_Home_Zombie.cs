using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class BuildingObj_Home_Zombie : BuildingObj
{
    private ActorManager zombie;
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
        if (globalTime == GlobalTime.Evening)
        {
            if (zombie == null) CreateZombie();
        }
    }
    private void CreateZombie()
    {
        int random = new System.Random().Next(0, 100);
        if (random < 50)
        {
            MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnActor()
            {
                name = "Actor/Zombie_Common",
                pos = transform.position,
                callBack = ((actor) =>
                {
                    zombie = actor.GetComponent<ActorManager>();
                    zombie.brainManager.SetHome(buildingTile.tilePos);
                    zombie.brainManager.SetTime(GlobalTime.Evening);
                })
            });
        }
        else if(random < 80)
        {
            MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnActor()
            {
                name = "Actor/Zombie_Spray",
                pos = transform.position,
                callBack = ((actor) =>
                {
                    zombie = actor.GetComponent<ActorManager>();
                    zombie.brainManager.SetHome(buildingTile.tilePos);
                    zombie.brainManager.SetTime(GlobalTime.Evening);
                })
            });
        }
        else
        {
            MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnActor()
            {
                name = "Actor/Zombie_Runner",
                pos = transform.position,
                callBack = ((actor) =>
                {
                    zombie = actor.GetComponent<ActorManager>();
                    zombie.brainManager.SetHome(buildingTile.tilePos);
                    zombie.brainManager.SetTime(GlobalTime.Evening);
                })
            });
        }
    }
}
