using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class ItemBase
{
    private ItemConfig itemConfig;
    public BaseBehaviorController owner;

    public virtual void Init(ItemConfig config)
    {
        itemConfig = config;
    }
    /// <summary>
    /// ����
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="hand"></param>
    public virtual void BeHolding(BaseBehaviorController owner,Transform hand)
    {

    }
    /// <summary>
    /// �Ҽ����
    /// </summary>
    public virtual void ClickRightClick(float time, bool hasInputAuthority = false)
    {

    }
    /// <summary>
    /// �Ҽ���ѹ
    /// </summary>
    public virtual void PressRightClick(float time, bool hasInputAuthority = false)
    {

    }
    /// <summary>
    /// �Ҽ��ͷ�
    /// </summary>
    public virtual void ReleaseRightClick(float time, bool hasInputAuthority = false)
    {

    }
    /// <summary>
    /// ������
    /// </summary>
    public virtual void ClickLeftClick(float time, bool hasInputAuthority = false)
    {

    }
    /// <summary>
    /// �����ѹ
    /// </summary>
    public virtual void PressLeftClick(float time, bool hasInputAuthority = false)
    {

    }
    /// <summary>
    /// ����ͷ�
    /// </summary>
    public virtual void ReleaseLeftClick(float time, bool hasInputAuthority = false)
    {

    }
    /// <summary>
    /// ���λ��
    /// </summary>
    public virtual void MousePosition(Vector3 mouse, float time)
    {

    }
}
