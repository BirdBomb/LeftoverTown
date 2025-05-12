using Fusion;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class BuildingObj_ChoppingBoard : BuildingObj
{
    private ActorManager_NPC_Hunter hunter;
    public override void Start()
    {
        CreateHunter();
        base.Start();
    }
    private void CreateHunter()
    {
        MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnActor()
        {
            name = "Actor/NPC_Hunter",
            pos = transform.position,
            callBack = ((actor) =>
            {
                hunter = actor.GetComponent<ActorManager_NPC_Hunter>();
                if (hunter.TryGetComponent(out ActorManager_NPC_Hunter actorManager_NPC_Hunter))
                {
                    hunter.State_BindChoppingBoard(buildingTile.tilePos);
                }
            })

        });
    }

}
