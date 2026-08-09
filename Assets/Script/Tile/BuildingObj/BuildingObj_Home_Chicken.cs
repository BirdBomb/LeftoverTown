using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class BuildingObj_Home_Chicken : BuildingObj_ResourcePoint
{
    private ActorManager actor_Bind;
    [Header("角色生成时间")]
    public int time_CreateActor = 1;
    public override void All_OnHourUpdate(GameEvent.GameEvent_All_UpdateHour eventData)
    {
        if (eventData.hour == time_CreateActor) All_CreateActor();
        base.All_OnHourUpdate(eventData);
    }
    private void All_CreateActor()
    {
        if (actor_Bind) return;
        MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnActor()
        {
            name = "Actor/Animal_Chicken",
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
