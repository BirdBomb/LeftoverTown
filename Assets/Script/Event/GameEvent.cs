using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public class GameEvent 
{
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
}
