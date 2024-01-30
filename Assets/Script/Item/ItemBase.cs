using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public virtual void ClickRightBtn()
    {

    }
    /// <summary>
    /// �Ҽ���ѹ
    /// </summary>
    public virtual void PressRightBtn()
    {

    }
    /// <summary>
    /// �Ҽ��ͷ�
    /// </summary>
    public virtual void ReleaseRightBtn()
    {

    }
    /// <summary>
    /// ������
    /// </summary>
    public virtual void ClickLeftBtn()
    {

    }
    /// <summary>
    /// �����ѹ
    /// </summary>
    public virtual void PressLeftBtn()
    {

    }
}
