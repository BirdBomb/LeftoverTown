using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using WebSocketSharp;

public class BuildingObj_Home_SpiderMachine : BuildingObj_Manmade
{
    ActorManager actor_Bind;
    public override void Start()
    {
        All_CreateActor();
        SubscribeToEvents();
        LoadInitialState();
        base.Start();
    }
    #region ³õÊ¼»¯
    public void All_CreateActor()
    {
        if (actor_Bind) return;
        MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnActor()
        {
            name = "Actor/Robot_SpiderThrow",
            pos = transform.position,
            callBack = ((actor) =>
            {
                actor_Bind = actor.GetComponent<ActorManager>();
                actor.brainManager.ForState_SetHomePos(buildingTile.tilePos);
                actor_Bind.brainManager.ForState_SetActivityPos(buildingTile.tilePos);
            })
        });
        MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnActor()
        {
            name = "Actor/Robot_SpiderFire",
            pos = transform.position,
            callBack = ((actor) =>
            {
                actor_Bind = actor.GetComponent<ActorManager>();
                actor.brainManager.ForState_SetHomePos(buildingTile.tilePos);
                actor_Bind.brainManager.ForState_SetActivityPos(buildingTile.tilePos);
            })
        });
    }


    public virtual void SubscribeToEvents()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_All_UpdateHour>()
            .Subscribe(All_OnHourUpdate)
            .AddTo(this);
    }
    public virtual void LoadInitialState()
    {
        WorldManager.Instance.GetTime_Detail(out int day, out int hour, out _);
    }

    #endregion
    public void All_OnHourUpdate(GameEvent.GameEvent_All_UpdateHour eventData)
    {
    }

}
