using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Fusion;

public class GameEvent 
{
    /// <summary>
    /// ���ض˰󶨱������
    /// </summary>
    public class GameEvent_Local_BindLocalPlayer
    {
        public PlayerCoreLocal playerCore;
    }
    /// <summary>
    /// ���ض�:����һСʱ
    /// </summary>
    public class GameEvent_State_AddOneHour
    {

    }
    /// <summary>
    /// ���ض�:ʱ����
    /// </summary>
    public class GameEvent_State_ChangeTime
    {
        public short hour;
    }
    /// <summary>
    /// ���ض�:��������
    /// </summary>
    public class GameEvent_Local_ChangeWeather
    {
        public short index;
    }
    /// <summary>
    /// �����:ʱ�����
    /// </summary>
    public class GameEvent_State_UpdateTime
    {
        public int hour;
        public int date;
        public GlobalTime now;
    }
    /// <summary>
    /// �ͻ���:�����
    /// </summary>
    public class GameEvent_All_UpdateSecond
    {
        public int second;
        public int hour;
        public int day;
        public GlobalTime now;
    }
    /// <summary>
    /// �ͻ���:Сʱ����
    /// </summary>
    public class GameEvent_All_UpdateHour
    {
        public int hour;
        public int day;
        public GlobalTime now;
    }
    /// <summary>
    /// ���ض�:�ٻ�����
    /// </summary>
    public class GameEvent_Local_SpawnActor
    {
        public string name;
        public Vector3 pos;
    }
    /// <summary>
    /// �����:�ٻ�����
    /// </summary>
    public class GameEvent_State_SpawnActor
    {
        public string name;
        public Vector3 pos;
        public System.Action<ActorManager> callBack;
    }
    /// <summary>
    /// �����:ɱ�����
    /// </summary>
    public class GameEvent_State_KillPlayer
    {
        public PlayerRef playerRef;
    }
    /// <summary>
    /// �����:��ԭ���
    /// </summary>
    public class GameEvent_State_RevivePlayer
    {
        public PlayerRef playerRef;
        public float reviveTime = 10;
    }
    /// <summary>
    /// ���ض�:��������
    /// </summary>
    public class GameEvent_Local_SpawnItem
    {
        public ItemData itemData;
        public NetworkId itemOwner;
        public Vector3 pos;
    }
    /// <summary>
    /// �����:��������
    /// </summary>
    public class GameEvent_State_SpawnItem
    {
        public ItemData itemData;
        public NetworkId itemOwner;
        public Vector3 pos;
    }

    /// <summary>
    /// ���пͻ���:ĳ���ƶ�
    /// </summary>
    public class GameEvent_AllClient_SomeoneMove
    {
        public ActorManager moveActor;
        public Vector3Int movePos; 
    }
    /// <summary>
    /// ���ض�:ĳ����ĳ��
    /// </summary>
    public class GameEvent_AllClient_SomeoneDoSomething
    {
        public ActorManager actor;
        public ActorAction action;
    }
    /// <summary>
    /// ���ض�:ĳ�˷���
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
    /// ���ض�:ĳ��˵ĳ��
    /// </summary>
    public class GameEvent_AllClient_SomeoneSendEmoji
    {
        public ActorManager actor;
        public Emoji emoji;
        public float distance;
    }
    /// <summary>
    /// ���ض�:һ����Χ���ҵ��ض��ؿ�
    /// </summary>
    public class GameEvent_Local_SomeoneFindTargetTile
    {
        public ActorManager actor;
        public int id;
        public Vector3 pos;
        public float distance;
    }
}
