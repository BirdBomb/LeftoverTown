using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class ItemBase
{
    public ItemData data;
    public ActorManager owner;
    public virtual void Init(ItemData config)
    {
        data = config;
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
    public virtual void ClickRightClick(float dt, bool state, bool input, bool showSI)
    {

    }
    /// <summary>
    /// �Ҽ���ѹ
    /// </summary>
    public virtual void PressRightClick(float dt, bool state, bool input, bool showSI)
    {

    }
    /// <summary>
    /// �Ҽ��ͷ�
    /// </summary>
    public virtual void ReleaseRightClick(float dt, bool state, bool input, bool showSI)
    {

    }
    /// <summary>
    /// ������
    /// </summary>
    public virtual void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {

    }
    /// <summary>
    /// �����ѹ
    /// </summary>
    public virtual void PressLeftClick(float dt, bool state, bool input, bool showSI)
    {

    }
    /// <summary>
    /// ����ͷ�
    /// </summary>
    public virtual void ReleaseLeftClick(float dt, bool state, bool input, bool showSI)
    {

    }
    /// <summary>
    /// ���λ��
    /// </summary>
    public virtual void FaceTo(Vector3 mouse, float time)
    {

    }
}
[Serializable]
public struct ItemData : INetworkStruct
{
    public int Item_ID;
    public int Item_Seed;
}
