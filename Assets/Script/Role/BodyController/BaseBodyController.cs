using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �������������
/// </summary>
public class BaseBodyController : MonoBehaviour
{
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


    /// <summary>
    /// �������嶯��
    /// </summary>
    public virtual void PlayBodyAction(BodyAction bodyAction, float speed, Action<string> action)
    {

    }
    /// <summary>
    /// ����ͷ������
    /// </summary>
    /// <param name="headAction"></param>
    public virtual void PlayHeadAction(HeadAction headAction, float speed, Action<string> action)
    {

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
    /// <summary>
    /// �����Ȳ�����
    /// </summary>
    /// <param name="legAction"></param>
    /// <param name="speed"></param>
    public virtual void PlayLegAction(LegAction legAction,float speed,Action<string> action)
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
    Bite,
    Charge,
    LowerHead,
    TakeDamage,
    Dead
}
public enum HandAction
{
    Slash_Horizontal,//ˮƽ����
    Slash_Horizontal_Ready,//ˮƽ����
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
    Idle,
    /// <summary>
    /// ����
    /// </summary>
    Step,
    /// <summary>
    /// ���
    /// </summary>
    Charge,
    Dead
}
public enum LegAction
{
    Idle,//��ֹ
    Step,//����
    Charge,//���
    Dead
}