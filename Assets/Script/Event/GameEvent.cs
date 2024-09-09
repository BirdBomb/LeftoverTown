using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class GameEvent 
{
    /// <summary>
    /// ���ض˰󶨱������
    /// </summary>
    public class GameEvent_Local_BindLocalPlayer
    {
        public PlayerController player;
    }
    /// <summary>
    /// ���ض�:ʱ��ı�
    /// </summary>
    public class GameEvent_Local_TimeChange
    {
        public int hour;
        public int date;
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
    /// ���ض�:��������
    /// </summary>
    public class GameEvent_Local_SpawnItem
    {
        public ItemData itemData;
        public Vector3 pos;
    }
    /// <summary>
    /// �����:��������
    /// </summary>
    public class GameEvent_State_SpawnItem
    {
        public ItemData itemData;
        public Vector3 pos;
    }

    /// <summary>
    /// ���ض�:ĳ���ƶ�
    /// </summary>
    public class GameEvent_Local_SomeoneMove
    {
        public ActorManager moveActor;
        public MyTile moveTile;
    }
    /// <summary>
    /// ���ض�:ĳ����ĳ��
    /// </summary>
    public class GameEvent_Local_SomeoneDoSomething
    {
        public ActorManager actor;
        public ActorAction action;
    }
    /// <summary>
    /// ���ض�:ĳ�˷���
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
    /// ���ض�:ĳ��˵ĳ��
    /// </summary>
    public class GameEvent_Local_SomeoneSendEmoji
    {
        public ActorManager actor;
        public int id;
        public float distance;
    }
    /// <summary>
    /// ���ض�:�ҵ��ض��ؿ�
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
