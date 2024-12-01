using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffBase 
{
    /// <summary>
    /// �����ڵ�:���ﴴ�����
    /// </summary>
    /// <param name="data"></param>
    public virtual void Listen_AddOnPlayerCreation(ref PlayerData data)
    {

    }
    /// <summary>
    /// �����ڵ�:���ﴴ��ȥ��
    /// </summary>
    /// <param name="data"></param>
    public virtual void Listen_SubOnPlayerCreation(ref PlayerData data)
    {

    }
    /// <summary>
    /// �����ڵ�:��Ϸ��ʼ
    /// </summary>
    public virtual void Listen_GameStart()
    {

    }
    /// <summary>
    /// �����ڵ�:���
    /// </summary>
    public virtual void Listen_AddOnActor(ActorManager actor)
    {

    }
    /// <summary>
    /// �����ڵ�:�Ƴ�
    /// </summary>
    public virtual void Listen_SubFromActor(ActorManager actor)
    {

    }
    /// <summary>
    /// �����ڵ�:���Լ��ƶ�
    /// </summary>
    public virtual void Listen_MyselfMove(ActorManager actor)
    {

    }
}
