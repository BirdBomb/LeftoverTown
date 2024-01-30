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
    /// ³ÖÎÕ
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="hand"></param>
    public virtual void BeHolding(BaseBehaviorController owner,Transform hand)
    {

    }
    /// <summary>
    /// ÓÒ¼üµã»÷
    /// </summary>
    public virtual void ClickRightBtn()
    {

    }
    /// <summary>
    /// ÓÒ¼ü°´Ñ¹
    /// </summary>
    public virtual void PressRightBtn()
    {

    }
    /// <summary>
    /// ÓÒ¼üÊÍ·Å
    /// </summary>
    public virtual void ReleaseRightBtn()
    {

    }
    /// <summary>
    /// ×ó¼üµã»÷
    /// </summary>
    public virtual void ClickLeftBtn()
    {

    }
    /// <summary>
    /// ×ó¼ü°´Ñ¹
    /// </summary>
    public virtual void PressLeftBtn()
    {

    }
}
