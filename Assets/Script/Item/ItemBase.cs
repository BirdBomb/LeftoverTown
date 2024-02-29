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
    public virtual void ClickRightClick(float time, bool hasInputAuthority = false)
    {

    }
    /// <summary>
    /// ÓÒ¼ü°´Ñ¹
    /// </summary>
    public virtual void PressRightClick(float time, bool hasInputAuthority = false)
    {

    }
    /// <summary>
    /// ÓÒ¼üÊÍ·Å
    /// </summary>
    public virtual void ReleaseRightClick(float time, bool hasInputAuthority = false)
    {

    }
    /// <summary>
    /// ×ó¼üµã»÷
    /// </summary>
    public virtual void ClickLeftClick(float time, bool hasInputAuthority = false)
    {

    }
    /// <summary>
    /// ×ó¼ü°´Ñ¹
    /// </summary>
    public virtual void PressLeftClick(float time, bool hasInputAuthority = false)
    {

    }
    /// <summary>
    /// ×ó¼üÊÍ·Å
    /// </summary>
    public virtual void ReleaseLeftClick(float time, bool hasInputAuthority = false)
    {

    }
    /// <summary>
    /// Êó±êÎ»ÖÃ
    /// </summary>
    public virtual void MousePosition(Vector3 mouse, float time)
    {

    }
}
