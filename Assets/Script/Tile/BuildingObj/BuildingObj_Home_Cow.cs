using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class BuildingObj_Home_Cow : BuildingObj_Manmade
{
    private ActorManager actor_Bind;
    [Header("角色生成时间")]
    public int time_CreateActor = 1;
    public override void Start()
    {
        All_CreateActor();
        MessageBroker.Default.Receive<GameEvent.GameEvent_All_UpdateHour>().Subscribe(All_OnHourUpdate).AddTo(this);
        base.Start();
    }
    public void All_OnHourUpdate(GameEvent.GameEvent_All_UpdateHour eventData)
    {
        if (eventData.hour == time_CreateActor) All_CreateActor();
    }
    private void All_CreateActor()
    {
        if (actor_Bind != null) return;
        MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnActor()
        {
            name = "Actor/Animal_Cow",

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
