using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class TileObj_SentryStation : TileObj
{
    private ActorManager onlyState_owner;
    private void Start()
    {
        CreateNPC();
    }
    private void OnDestroy()
    {
        DestroyNPC();
    }

    #region// ’“¯Ã®
    private void CreateNPC()
    {
        MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnActor()
        {
            name = "Actor/NPC_TownPatrol",
            pos = transform.position,
            callBack = BindOwner
        });
    }
    private void DestroyNPC()
    {
        if (onlyState_owner != null)
        {
            Destroy(onlyState_owner.gameObject);
        }
    }
    public void BindOwner(ActorManager actor)
    {
        onlyState_owner = actor;
    }
    #endregion

}
