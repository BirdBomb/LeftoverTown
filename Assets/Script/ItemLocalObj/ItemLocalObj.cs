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
    /// ��ѹ���
    /// </summary>
    /// <param name="time"></param>
    /// <param name="actorAuthority"></param>
    /// <returns>���ֵ</returns>
    public virtual bool PressLeftMouse(float time, ActorAuthority actorAuthority)
    {
        return false;
    }
    /// <summary>
    /// ��ѹ�Ҽ�
    /// </summary>
    /// <param name="time"></param>
    /// <param name="actorAuthority"></param>
    /// <returns></returns>
    public virtual bool PressRightMouse(float time, ActorAuthority actorAuthority)
    {
        return false;
    }
    /// <summary>
    /// �ͷ����
    /// </summary>
    public virtual void ReleaseRightMouse()
    {

    }
    /// <summary>
    /// �ͷ��Ҽ�
    /// </summary>
    public virtual void ReleaseLeftMouse()
    {

    }
    /// <summary>
    /// �������λ��
    /// </summary>
    /// <param name="mouse"></param>
    public virtual void UpdateMousePos(Vector3 mouse)
    {

    }
    /// <summary>
    /// ����ʱ��
    /// </summary>
    /// <param name="second"></param>
    public virtual void UpdateTime(int second)
    {

    }
}
