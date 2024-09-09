using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class GameEvent 
{
    /// <summary>
    /// 本地端绑定本地玩家
    /// </summary>
    public class GameEvent_Local_BindLocalPlayer
    {
        public PlayerController player;
    }
    /// <summary>
    /// 本地端:时间改变
    /// </summary>
    public class GameEvent_Local_TimeChange
    {
        public int hour;
        public int date;
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
        public ActorAction action;
    }
    /// <summary>
    /// 本地端:某人犯法
    /// </summary>
    public class GameEvent_Local_SomeoneCommit
    {
        public ActorManager actor;
        public short fine;
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
        public float distance;
    }
    /// <summary>
    /// 本地端:找到特定地块
    /// </summary>
    public class GameEvent_Local_SomeoneFindTargetTile
    {
        public ActorManager actor;
        public int id;
        public Vector3 pos;
        public float distance;
        public Action<TileObj> action;
    }
}
