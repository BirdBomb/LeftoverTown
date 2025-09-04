using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Fusion;

public class GameEvent 
{
    /// <summary>
    /// 本地端绑定本地玩家
    /// </summary>
    public class GameEvent_Local_BindLocalPlayer
    {
        public PlayerCoreLocal playerCore;
    }
    /// <summary>
    /// 本地端:增加一小时
    /// </summary>
    public class GameEvent_State_AddOneHour
    {

    }
    /// <summary>
    /// 本地端:时间变更
    /// </summary>
    public class GameEvent_State_ChangeTime
    {
        public short hour;
    }
    /// <summary>
    /// 本地端:更改天气
    /// </summary>
    public class GameEvent_Local_ChangeWeather
    {
        public short index;
    }
    /// <summary>
    /// 服务端:时间更新
    /// </summary>
    public class GameEvent_State_UpdateTime
    {
        public int hour;
        public int date;
        public GlobalTime now;
    }
    /// <summary>
    /// 客户端:秒更新
    /// </summary>
    public class GameEvent_All_UpdateSecond
    {
        public int second;
        public int hour;
        public int day;
        public GlobalTime now;
    }
    /// <summary>
    /// 客户端:小时更新
    /// </summary>
    public class GameEvent_All_UpdateHour
    {
        public int hour;
        public int day;
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
    /// 服务端:杀死玩家
    /// </summary>
    public class GameEvent_State_KillPlayer
    {
        public PlayerRef playerRef;
    }
    /// <summary>
    /// 服务端:复原玩家
    /// </summary>
    public class GameEvent_State_RevivePlayer
    {
        public PlayerRef playerRef;
        public float reviveTime = 10;
    }
    /// <summary>
    /// 本地端:生成物体
    /// </summary>
    public class GameEvent_Local_SpawnItem
    {
        public ItemData itemData;
        public NetworkId itemOwner;
        public Vector3 pos;
    }
    /// <summary>
    /// 服务端:生成物体
    /// </summary>
    public class GameEvent_State_SpawnItem
    {
        public ItemData itemData;
        public NetworkId itemOwner;
        public Vector3 pos;
    }

    /// <summary>
    /// 所有客户端:某人移动
    /// </summary>
    public class GameEvent_AllClient_SomeoneMove
    {
        public ActorManager moveActor;
        public Vector3Int movePos; 
    }
    /// <summary>
    /// 本地端:某人做某事
    /// </summary>
    public class GameEvent_AllClient_SomeoneDoSomething
    {
        public ActorManager actor;
        public ActorAction action;
    }
    /// <summary>
    /// 本地端:某人犯法
    /// </summary>
    public class GameEvent_AllClient_SomeoneCommit
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
    public class GameEvent_AllClient_SomeoneSendEmoji
    {
        public ActorManager actor;
        public Emoji emoji;
        public float distance;
    }
    /// <summary>
    /// 本地端:一定范围内找到特定地块
    /// </summary>
    public class GameEvent_Local_SomeoneFindTargetTile
    {
        public ActorManager actor;
        public int id;
        public Vector3 pos;
        public float distance;
    }
}
