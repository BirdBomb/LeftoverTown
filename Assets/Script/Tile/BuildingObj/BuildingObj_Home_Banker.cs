using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UniRx;
using UnityEngine;

public class BuildingObj_Home_Banker : BuildingObj_Supply
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
            name = "Actor/NPC_Banker",
            pos = transform.position,
            callBack = ((actor) =>
            {
                actor_Bind = actor.GetComponent<ActorManager>();
                actor.brainManager.ForState_SetHomePos(buildingTile.tilePos);
                actor.brainManager.ForState_SetActivityPos(buildingTile.tilePos);
            })
        });
    }
}
