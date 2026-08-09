using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class BuildingObj_Home_Zombie : BuildingObj_Manmade
{
    private ActorManager actor_Bind;
    [Header("角色生成时间")]
    public int time_CreateActor = 1;
    public override void Start()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_All_UpdateHour>().Subscribe(All_OnHourUpdate).AddTo(this);
        base.Start();
    }
    public void All_OnHourUpdate(GameEvent.GameEvent_All_UpdateHour eventData)
    {
        if (eventData.hour == time_CreateActor) All_CreateActor();
    }

    private void All_CreateActor()
    {
        if (actor_Bind) return;
        int random = new System.Random().Next(0, 100);
        if (random < 100)
        {
            MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnActor()
            {
                name = "Actor/Zombie_Spray",
                pos = transform.position,
                callBack = ((actor) =>
                {
                    actor_Bind = actor.GetComponent<ActorManager>();
                    actor_Bind.brainManager.ForState_SetHomePos(buildingTile.tilePos);
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
                    actor_Bind = actor.GetComponent<ActorManager>();
                    actor_Bind.brainManager.ForState_SetHomePos(buildingTile.tilePos);
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
                    actor_Bind = actor.GetComponent<ActorManager>();
                    actor_Bind.brainManager.ForState_SetHomePos(buildingTile.tilePos);
                })
            });
        }
    }
}
