using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public class GameEvent 
{
    /// <summary>
    /// ���ض�:ʱ��ı�
    /// </summary>
    public class GameEvent_Local_TimeChange
    {
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
        public ActorAction actorAction;
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
    }
}
