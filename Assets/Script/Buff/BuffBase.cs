using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffBase 
{
    public virtual void Init(BuffData buffData)
    {

    }
    #region//��Ϸ�ڼ���
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
    /// <summary>
    /// �����ڵ�:�����
    /// </summary>
    public virtual void Listen_UpdateSecond(ActorManager actor)
    {

    }

    #endregion
    #region//���÷���
    /// <summary>
    /// ������Ч
    /// </summary>
    public virtual void PlayEffect(ActorManager actor, int index)
    {

    }
    /// <summary>
    /// ����ͼ��
    /// </summary>
    /// <param name="buffIcon"></param>
    /// <param name="buffData"></param>
    public virtual void UpdateIcon(UI_BuffIcon buffIcon, BuffData buffData)
    {

    }
    #endregion
}
[Serializable]
public struct BuffData
{
    public short BuffID;
    public short BuffVal;
    public BuffData(short id)
    {
        BuffID = id;
        BuffVal = 0;
    }
}
