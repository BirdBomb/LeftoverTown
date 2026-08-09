using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;
using System.Text;

public class BuildingObj_Home_Villager : BuildingObj_Supply
{
    private ActorManager actor_Bind = new ActorManager();
    public override void Start()
    {
        All_CreateActor();
        base.Start();
    }
    public void All_CreateActor()
    {
        if (actor_Bind) return;
        MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnActor()
        {
            name = "Actor/NPC_Villager",
            pos = transform.position,
            callBack = ((actor) =>
            {
                actor_Bind = actor.GetComponent<ActorManager>();
                actor.brainManager.ForState_SetHomePos(buildingTile.tilePos);
                actor_Bind.brainManager.ForState_SetActivityPos(buildingTile.tilePos);
            })
        });
    }
}
