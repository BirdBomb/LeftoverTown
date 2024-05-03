using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �������������
/// </summary>
public class BaseBodyController : MonoBehaviour
{
    [HideInInspector]
    public float speed;
    [SerializeField, Header("���ڵ�")]
    public Transform Root;
    [SerializeField, Header("ͷ���ڵ�")]
    public Transform Head;
    [SerializeField, Header("����ڵ�")]
    public Transform Body;
    [SerializeField, Header("��֫�ڵ�")]
    public Transform Hand;
    [SerializeField, Header("��֫�ڵ�")]
    public Transform Leg;
    [SerializeField, Header("���ֽڵ�")]
    public Transform Hand_Right;
    [SerializeField, Header("���ֽڵ�")]
    public Transform Hand_Left;

    [SerializeField, Header("������Ʒ")]
    public Transform Hand_RightItem;
    [SerializeField, Header("������Ʒ")]
    public Transform Hand_LeftItem;

    public Animator Animator_Head;
    public AnimaEventListen AnimaEventListen_Head;
    public Animator Animator_Body;
    public AnimaEventListen AnimaEventListen_Body;
    public Animator Animator_Hand;
    public AnimaEventListen AnimaEventListen_Hand;
    public Animator Animator_Leg;
    public AnimaEventListen AnimaEventListen_Leg;

    public virtual void SetHeadTrigger(string name, float speed, Action<string> action)
    {
        Animator_Head.SetTrigger(name);
        Animator_Head.speed=speed;
        AnimaEventListen_Head.BindEvent(action);
    }
    public virtual void SetHeadBool(string name, bool p, float speed, Action<string> action)
    {
        Animator_Head.SetBool(name, p);
        Animator_Head.speed = speed;
        AnimaEventListen_Head.BindEvent(action);
    }
    public virtual void SetBodyTrigger(string name, float speed, Action<string> action)
    {
        Animator_Body.SetTrigger(name);
        Animator_Body.speed = speed;
        AnimaEventListen_Body.BindEvent(action);
    }
    public virtual void SetBodyBool(string name, bool p, float speed, Action<string> action)
    {
        Animator_Body.SetBool(name, p);
        Animator_Body.speed = speed;
        AnimaEventListen_Body.BindEvent(action);
    }

    public virtual void SetHandTrigger(string name, float speed, Action<string> action)
    {
        Animator_Hand.SetTrigger(name);
        Animator_Hand.speed = speed;
        AnimaEventListen_Hand.BindEvent(action);
    }
    public virtual void SetHandBool(string name, bool p, float speed, Action<string> action)
    {
        Animator_Hand.SetBool(name, p);
        Animator_Hand.speed = speed;
        AnimaEventListen_Hand.BindEvent(action);
    }
    public virtual void SetLegTrigger(string name, float speed, Action<string> action)
    {
        Animator_Leg.SetTrigger(name);
        Animator_Leg.speed = speed;
        AnimaEventListen_Leg.BindEvent(action);
    }
    public virtual void SetLegBool(string name, bool p, float speed, Action<string> action)
    {
        Animator_Leg.SetBool(name, p);
        Animator_Leg.speed = speed;
        AnimaEventListen_Leg.BindEvent(action);
    }

    /// <summary>
    /// �����ֲ�����
    /// </summary>
    /// <param name="handAction"></param>
    /// <param name="speed"></param>
    /// <param name="action"></param>
    public virtual void PlayHandAction(HandAction handAction,float speed,Action<string> action)
    {

    }

}
public enum BodyAction
{
    Idle,
    Move,
    Charge,
    Dead
}
public enum HeadAction
{
    Idle,
    Move,
    Eat,
    Bite,
    Charge,
    LowerHead,
    TakeDamage,
    Dead
}
public enum HandAction
{
    /// <summary>
    /// ˮƽ����
    /// </summary>
    Slash_Horizontal,
    /// <summary>
    /// ˮƽ����׼��
    /// </summary>
    Slash_Horizontal_Ready,
    /// <summary>
    /// ��ֱ����
    /// </summary>
    Slash_Vertical,
    /// <summary>
    /// ��ֱ����׼��
    /// </summary>
    Slash_Vertical_Ready,
    /// <summary>
    /// ��ֱ��������
    /// </summary>
    Slash_Vertical_Play,
    /// <summary>
    /// ��ֱ�����ͷ�
    /// </summary>
    Slash_Vertical_Release,
    /// <summary>
    /// ����׼��
    /// </summary>
    Bow_Ready,
    /// <summary>
    /// �����ͷ�
    /// </summary>
    Bow_Play,
    /// <summary>
    /// �����ͷ�
    /// </summary>
    Bow_Release,

    /// <summary>
    /// ����
    /// </summary>
    Spear,
    /// <summary>
    /// ����
    /// </summary>
    Lift,
    /// <summary>
    /// ������
    /// </summary>
    Recoil,
    /// <summary>
    /// ʰ��
    /// </summary>
    PickUp,
    /// <summary>
    /// ����
    /// </summary>
    PutUp,
    /// <summary>
    /// ����
    /// </summary>
    Idle,
    /// <summary>
    /// ����
    /// </summary>
    Step,
    /// <summary>
    /// ���
    /// </summary>
    Charge,
    /// <summary>
    /// ��
    /// </summary>
    Eat,
    /// <summary>
    /// ����
    /// </summary>
    Dead
}
public enum LegAction
{
    Idle,//��ֹ
    Step,//����
    Charge,//���
    Dead
}
public enum BodyPart
{
    Head,
    Body,
    Hand,
    Leg,
}