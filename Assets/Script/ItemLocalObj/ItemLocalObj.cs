using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLocalObj : MonoBehaviour
{
    [HideInInspector]
    public ActorManager actorManager;
    [HideInInspector]
    public ItemData itemData;
    public virtual void UpdateDataByLocal(ItemData data)
    {
        itemData = data;
    }
    public virtual void UpdateDataByNet(ItemData data)
    {
        itemData = data;
    }
    public virtual void HoldingByHand(ActorManager owner, BodyController_Human body, ItemData data)
    {

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="time"></param>
    /// <param name="actorAuthority"></param>
    /// <returns>最大值</returns>
    public virtual bool PressLeftMouse(float time, ActorAuthority actorAuthority)
    {
        return false;
    }
    /// <summary>
    /// 最大值
    /// </summary>
    /// <param name="time"></param>
    /// <param name="actorAuthority"></param>
    /// <returns></returns>
    public virtual bool PressRightMouse(float time, ActorAuthority actorAuthority)
    {
        return false;
    }
    public virtual void ReleaseRightMouse()
    {

    }
    public virtual void ReleaseLeftMouse()
    {

    }
    public virtual void UpdateMousePos(Vector3 mouse)
    {

    }
    public virtual void UpdateTime(int second)
    {

    }
}
