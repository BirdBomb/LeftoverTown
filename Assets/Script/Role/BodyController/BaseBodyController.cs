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
    [HideInInspector]
    public bool locking = false;
    [SerializeField, Header("���ڵ�")]
    public Transform Tran_Root;
    [SerializeField, Header("ͷ���ڵ�")]
    public Transform Tran_Head;
    [SerializeField, Header("����ڵ�")]
    public Transform Tran_Body;
    [SerializeField, Header("��֫�ڵ�")]
    public Transform Tran_BothHand;
    [SerializeField, Header("��֫�ڵ�")]
    public Transform Tran_BothLeg;
    [SerializeField, Header("���ֽڵ�")]
    public Transform Tran_RightHand;
    [SerializeField, Header("���ֽڵ�")]
    public Transform Tran_LeftHand;

    [SerializeField, Header("������Ʒ")]
    public Transform Tran_RightItemInHand;
    [SerializeField, Header("������Ʒ")]
    public Transform Tran_LeftItemInHand;
    [SerializeField, Header("ͷ����Ʒ")]
    public Transform Tran_ItemOnHead;
    [SerializeField, Header("������Ʒ")]
    public Transform Tran_ItemOnBody;


    public Animator Animator_Head;
    public AnimaEventListen AnimaEventListen_Head;
    public Animator Animator_Body;
    public AnimaEventListen AnimaEventListen_Body;
    public Animator Animator_Hand;
    public AnimaEventListen AnimaEventListen_Hand;
    public Animator Animator_Leg;
    public AnimaEventListen AnimaEventListen_Leg;
    #region//��ʼ��
    private void Start()
    {
        AnimaEventListen_Leg.BindCommonEvent((str) =>
        {
            if (str == "LegStep")
            {
                //GameObject effect = PoolManager.Instance.GetObject("Effect/Effect_BombSmoke");
                //effect.transform.position = Leg.position;
                AudioManager.Instance.PlayEffect(1007,transform.position);
            }
        });
    }
    #endregion
    #region//����
    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="hairID"></param>
    /// <param name="eyeID"></param>
    /// <param name="hairColor"></param>
    public virtual void InitFace(int hairID, int eyeID, Color32 hairColor)
    {

    }
    #endregion
    #region//ͷ��
    public virtual void SetHeadTrigger(string name, float speed, Action<string> action)
    {
        if (!locking)
        {
            Animator_Head.SetTrigger(name);
            Animator_Head.speed = speed;
            AnimaEventListen_Head.BindTempEvent(action);
        }
    }
    public virtual void SetHeadBool(string name, bool p, float speed, Action<string> action)
    {
        if (!locking)
        {
            Animator_Head.SetBool(name, p);
            Animator_Head.speed = speed;
            AnimaEventListen_Head.BindTempEvent(action);
        }
    }
    public virtual void SetHeadFloat(string name, float p)
    {
        if (!locking)
        {
            Animator_Head.SetFloat(name, p);
        }
    }
    #endregion
    #region//����
    public virtual void SetBodyTrigger(string name, float speed, Action<string> action)
    {
        if (!locking)
        {
            Animator_Body.SetTrigger(name);
            Animator_Body.speed = speed;
            AnimaEventListen_Body.BindTempEvent(action);
        }
    }
    public virtual void SetBodyBool(string name, bool p, float speed, Action<string> action)
    {
        if (!locking)
        {
            Animator_Body.SetBool(name, p);
            Animator_Body.speed = speed;
            AnimaEventListen_Body.BindTempEvent(action);
        }
    }
    public virtual void SetBodyFloat(string name, float p)
    {
        if (!locking)
        {
            Animator_Body.SetFloat(name, p);
        }
    }
    #endregion
    #region//�ֲ�
    public virtual void SetHandTrigger(string name, float speed, Action<string> action)
    {
        if (!locking)
        {
            Animator_Hand.SetTrigger(name);
            Animator_Hand.speed = speed;
            AnimaEventListen_Hand.BindTempEvent(action);
        }
    }
    public virtual void SetHandBool(string name, bool p, float speed, Action<string> action)
    {
        if (!locking)
        {
            Animator_Hand.SetBool(name, p);
            Animator_Hand.speed = speed;
            AnimaEventListen_Hand.BindTempEvent(action);
        }
    }
    public virtual void SetHandFloat(string name, float p)
    {
        if (!locking)
        {
            Animator_Hand.SetFloat(name, p);
        }
    }
    #endregion
    #region//�Ȳ�
    public virtual void SetLegTrigger(string name, float speed, Action<string> action)
    {
        if (!locking)
        {
            Animator_Leg.SetTrigger(name);
            Animator_Leg.speed = speed;
            AnimaEventListen_Leg.BindTempEvent(action);
        }
    }
    public virtual void SetLegBool(string name, bool p, float speed, Action<string> action)
    {
        if (!locking)
        {
            Animator_Leg.SetBool(name, p);
            Animator_Leg.speed = speed;
            AnimaEventListen_Leg.BindTempEvent(action);
        }
    }
    public virtual void SetLegFloat(string name, float p)
    {
        if (!locking)
        {
            Animator_Leg.SetFloat(name, p);
        }
    }
    #endregion

    #region//ת��
    [HideInInspector]
    public Vector2 faceDir;
    private bool turnRight = false;
    private bool turnLeft = false;
    private bool faceRight = false;
    private bool faceLeft = false;

    /// <summary>
    /// �������
    /// </summary>
    public virtual void FaceLeft()
    {
        if (!faceLeft)
        {
            faceLeft = true;
            faceRight = false;
            Tran_Head.localScale = new Vector3(-1, 1, 1);
            Tran_Body.localScale = new Vector3(-1, 1, 1);
            Tran_BothHand.localScale = new Vector3(-1, 1, 1);
        }
    }
    /// <summary>
    /// �����ұ�
    /// </summary>
    public virtual void FaceRight()
    {
        if (!faceRight)
        {
            faceLeft = false;
            faceRight = true;
            Tran_Head.localScale = new Vector3(1, 1, 1);
            Tran_Body.localScale = new Vector3(1, 1, 1);
            Tran_BothHand.localScale = new Vector3(1, 1, 1);
        }
    }
    /// <summary>
    /// ת�����
    /// </summary>
    public void TurnLeft()
    {
        if (!turnLeft)
        {
            turnLeft = true;
            turnRight = false;
            Tran_BothLeg.localScale = new Vector3(-1, 1, 1);
        }
    }
    /// <summary>
    /// ת���ұ�
    /// </summary>
    public void TurnRight()
    {
        if (!turnRight)
        {
            turnRight = true;
            turnLeft = false;
            Tran_BothLeg.localScale = new Vector3(1, 1, 1);
        }
    }

    #endregion
    #region//Ԥ��
    /// <summary>
    /// ���ؽ�ɫ
    /// </summary>
    public virtual void HideActor()
    {

    }
    /// <summary>
    /// ��ʾ��ɫ
    /// </summary>
    public virtual void ShowActor()
    {

    }
    #endregion
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