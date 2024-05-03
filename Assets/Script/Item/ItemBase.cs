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
    public virtual void ClickRightClick(float dt, bool state, bool input, bool showSI)
    {

    }
    /// <summary>
    /// ÓÒ¼ü°´Ñ¹
    /// </summary>
    public virtual void PressRightClick(float dt, bool state, bool input, bool showSI)
    {

    }
    /// <summary>
    /// ÓÒ¼üÊÍ·Å
    /// </summary>
    public virtual void ReleaseRightClick(float dt, bool state, bool input, bool showSI)
    {

    }
    /// <summary>
    /// ×ó¼üµã»÷
    /// </summary>
    public virtual void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {

    }
    /// <summary>
    /// ×ó¼ü°´Ñ¹
    /// </summary>
    public virtual void PressLeftClick(float dt, bool state, bool input, bool showSI)
    {

    }
    /// <summary>
    /// ×ó¼üÊÍ·Å
    /// </summary>
    public virtual void ReleaseLeftClick(float dt, bool state, bool input, bool showSI)
    {

    }
    /// <summary>
    /// Êó±êÎ»ÖÃ
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
