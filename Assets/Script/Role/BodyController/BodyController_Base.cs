using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �������������
/// </summary>
public class BodyController_Base : MonoBehaviour
{
    [HideInInspector]
    public float speed = 0;


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
    #region//����
    public virtual void SetAnimatorTrigger(BodyPart bodyPart, string name)
    {

    }
    public virtual void SetAnimatorFloat(BodyPart bodyPart, string name, float val)
    {

    }
    public virtual void SetAnimatorBool(BodyPart bodyPart, string name, bool val)
    {

    }
    public virtual void SetAnimatorAction(BodyPart bodyPart, Action<string> action)
    {

    }
    #endregion
    #region//ת��
    [HideInInspector]
    public Vector2 faceDir;
    [HideInInspector]
    public Vector2 turnDir;
    protected bool turnRight = true;
    protected bool faceRight = true;

    /// <summary>
    /// �������
    /// </summary>
    public void FaceLeft()
    {
        if (faceRight)
        {
            faceRight = false;
            Face(faceRight);
        }
    }
    /// <summary>
    /// �����ұ�
    /// </summary>
    public void FaceRight()
    {
        if (!faceRight)
        {
            faceRight = true;
            Face(faceRight);
        }
    }
    public virtual void Face(bool right)
    {
    }
    public void TurnLeft()
    {
        if (turnRight)
        {
            turnRight = false;
            Turn(turnRight);
            Face(faceRight);
        }
    }
    public void TurnRight()
    {
        if (!turnRight)
        {
            turnRight = true;
            Turn(turnRight);
            Face(faceRight);
        }
    }
    public virtual void Turn(bool right)
    {
    }
    #endregion
    #region//����
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
    #region//����
    public virtual void Dead()
    {

    }
    #endregion
}
public enum BodyPart
{
    Body,
    Head,
    Hand,
}