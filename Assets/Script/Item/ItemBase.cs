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
    /// ³ÖÎÕ
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="item"></param>
    public virtual void BeHolding(ActorManager owner,Transform item)
    {

    }
    /// <summary>
    /// Ê³ÓÃ
    /// </summary>
    public virtual void BeEating(ActorManager who)
    {

    }
    /// <summary>
    /// ÓÒ¼üµã»÷
    /// </summary>
    public virtual void ClickRightClick(float time, bool hasInputAuthority)
    {

    }
    /// <summary>
    /// ÓÒ¼ü°´Ñ¹
    /// </summary>
    public virtual void PressRightClick(float time, bool hasInputAuthority)
    {

    }
    /// <summary>
    /// ÓÒ¼üÊÍ·Å
    /// </summary>
    public virtual void ReleaseRightClick(float time, bool hasInputAuthority)
    {

    }
    /// <summary>
    /// ×ó¼üµã»÷
    /// </summary>
    public virtual void ClickLeftClick(float time, bool hasInputAuthority)
    {

    }
    /// <summary>
    /// ×ó¼ü°´Ñ¹
    /// </summary>
    public virtual void PressLeftClick(float time, bool hasInputAuthority)
    {

    }
    /// <summary>
    /// ×ó¼üÊÍ·Å
    /// </summary>
    public virtual void ReleaseLeftClick(float time, bool hasInputAuthority)
    {

    }
    /// <summary>
    /// Êó±êÎ»ÖÃ
    /// </summary>
    public virtual void MousePosition(Vector3 mouse, float time)
    {

    }
}
