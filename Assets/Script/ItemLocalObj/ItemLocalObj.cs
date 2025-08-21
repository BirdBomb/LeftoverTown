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
    public virtual void InitData(ItemData data)
    {
        itemData = data;
    }
    public virtual void UpdateDataByLocal(ItemData data)
    {
        itemData = data;
    }
    public virtual void UpdateDataByNet(ItemData data)
    {
        itemData = data;
    }
    public virtual void HoldingStart(ActorManager owner, BodyController_Human body)
    {

    }
    public virtual void HoldingOver()
    {

    }
    /// <summary>
    /// 按压左键
    /// </summary>
    /// <param name="time"></param>
    /// <param name="actorAuthority"></param>
    /// <returns>最大值</returns>
    public virtual bool PressLeftMouse(float time, ActorAuthority actorAuthority)
    {
        return false;
    }
    /// <summary>
    /// 按压右键
    /// </summary>
    /// <param name="time"></param>
    /// <param name="actorAuthority"></param>
    /// <returns></returns>
    public virtual bool PressRightMouse(float time, ActorAuthority actorAuthority)
    {
        return false;
    }
    /// <summary>
    /// 释放左键
    /// </summary>
    public virtual void ReleaseRightMouse()
    {

    }
    /// <summary>
    /// 释放右键
    /// </summary>
    public virtual void ReleaseLeftMouse()
    {

    }
    /// <summary>
    /// 更新鼠标位置
    /// </summary>
    /// <param name="mouse"></param>
    public virtual void UpdateMousePos(Vector3 mouse)
    {

    }
    /// <summary>
    /// 更新时间
    /// </summary>
    /// <param name="second"></param>
    public virtual void UpdateTime(int second)
    {

    }
}
