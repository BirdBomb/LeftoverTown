using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

public class WorldActorManager : SingleTon<WorldActorManager>, ISingleTon
{
    private System.Random random = new System.Random();
    public void Init()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_Local_BindLocalPlayer>().Subscribe(_ =>
        {
            SetPlayer(_.playerCore);
        }).AddTo(this);
    }
    private PlayerCoreLocal playerCoreLocal;
    private List<ActorManager> list_ActorManagers = new List<ActorManager>();
    public PlayerCoreLocal GetPlayer()
    {
        return playerCoreLocal;
    }
    public bool GetPlayer(out PlayerCoreLocal var)
    {
        var = playerCoreLocal;
        return var != null;
    }
    public List<ActorManager> GetActorList()
    {
        return list_ActorManagers;
    }
    public void SetPlayer(PlayerCoreLocal player)
    {
        playerCoreLocal = player;
    }
    public void AddActor(ActorManager actorManager)
    {
        list_ActorManagers.Add(actorManager);
    }
    public void SubActor(ActorManager actorManager)
    {
        list_ActorManagers.Remove(actorManager);
    }
    public bool FindPlayer(out ActorManager player)
    {
        for (int i = 0; i < list_ActorManagers.Count; i++)
        {
            if (list_ActorManagers[i] != null && list_ActorManagers[i].actorAuthority.isPlayer)
            {
                player = list_ActorManagers[i];
                return true;
            }
        }
        player = null;
        return false;
    }
    public bool FindActor(out ActorManager actor)
    {
        if (list_ActorManagers.Count > 0)
        {
            actor = list_ActorManagers[random.Next(0, list_ActorManagers.Count)];
            return true;
        }
        else
        {
            actor = null;
            return false;
        }
    }


    private Queue queue_ActorCreateQuests = new Queue();
    public void ForState_CreateActor(ActorSpawnQuest createQuest)
    {
        queue_ActorCreateQuests.Enqueue(createQuest);
    }
}
public class ActorSpawnQuest
{
    public string path;
    public Vector3 pos;
    public Action<ActorManager> callBack;

    public ActorSpawnQuest(string str,Vector3 v3,Action<ActorManager> action)
    {
        path = str; pos = v3; callBack = action;
    }
}