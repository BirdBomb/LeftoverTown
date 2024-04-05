using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class ItemBase
{
    public NetworkItemConfig networkItem;
    public ActorManager owner;

    public virtual void Init(NetworkItemConfig config)
    {
        networkItem = config;
    }
    /// <summary>
    /// ����
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="item"></param>
    public virtual void BeHolding(ActorManager owner,Transform item)
    {

    }
    /// <summary>
    /// ʳ��
    /// </summary>
    public virtual void BeEating(ActorManager who)
    {

    }
    /// <summary>
    /// �Ҽ����
    /// </summary>
    public virtual void ClickRightClick(float time, bool hasInputAuthority)
    {

    }
    /// <summary>
    /// �Ҽ���ѹ
    /// </summary>
    public virtual void PressRightClick(float time, bool hasInputAuthority)
    {

    }
    /// <summary>
    /// �Ҽ��ͷ�
    /// </summary>
    public virtual void ReleaseRightClick(float time, bool hasInputAuthority)
    {

    }
    /// <summary>
    /// ������
    /// </summary>
    public virtual void ClickLeftClick(float time, bool hasInputAuthority)
    {

    }
    /// <summary>
    /// �����ѹ
    /// </summary>
    public virtual void PressLeftClick(float time, bool hasInputAuthority)
    {

    }
    /// <summary>
    /// ����ͷ�
    /// </summary>
    public virtual void ReleaseLeftClick(float time, bool hasInputAuthority)
    {

    }
    /// <summary>
    /// ���λ��
    /// </summary>
    public virtual void MousePosition(Vector3 mouse, float time)
    {

    }
}
