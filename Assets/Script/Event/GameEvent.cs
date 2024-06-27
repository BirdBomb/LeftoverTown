using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public class GameEvent 
{
    /// <summary>
    /// 本地端:时间改变
    /// </summary>
    public class GameEvent_Local_TimeChange
    {
        public GlobalTime now;
    }
    /// <summary>
    /// 本地端:召唤生物
    /// </summary>
    public class GameEvent_Local_SpawnActor
    {
        public string name;
        public Vector3 pos;
    }
    /// <summary>
    /// 服务端:召唤生物
    /// </summary>
    public class GameEvent_State_SpawnActor
    {
        public string name;
        public Vector3 pos;
        public System.Action<ActorManager> callBack;
    }

    /// <summary>
    /// 本地端:生成物体
    /// </summary>
    public class GameEvent_Local_SpawnItem
    {
        public ItemData itemData;
        public Vector3 pos;
    }
    /// <summary>
    /// 服务端:生成物体
    /// </summary>
    public class GameEvent_State_SpawnItem
    {
        public ItemData itemData;
        public Vector3 pos;
    }

    /// <summary>
    /// 本地端:某人移动
    /// </summary>
    public class GameEvent_Local_SomeoneMove
    {
        public ActorManager moveActor;
        public MyTile moveTile;
    }
    /// <summary>
    /// 本地端:某人做某事
    /// </summary>
    public class GameEvent_Local_SomeoneDoSomething
    {
        public ActorManager actor;
        public ActorAction actorAction;
    }
    public enum ActorAction
    {
        Holding,
        Attack,
        Dead,
    }
    /// <summary>
    /// 本地端:某人说某话
    /// </summary>
    public class GameEvent_Local_SomeoneSendEmoji
    {
        public ActorManager actor;
        public int id;
    }
}
