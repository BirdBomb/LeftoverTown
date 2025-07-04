using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 基本身体控制器
/// </summary>
public class BodyController_Base : MonoBehaviour
{
    [HideInInspector]
    public float float_Speed = 0;

    #region//检查
    private Vector2 vector2_Last;
    private Vector2 vector2_Cur;
    public void Local_CheckPos(float dt)
    {
        vector2_Cur = transform.position;
        float distance = Vector2.Distance(vector2_Last, vector2_Cur);
        float speed = distance / dt;
        if (speed > 0.1f)
        {
            PlayMove(speed);
            TurnTo(vector2_Cur - vector2_Last);
        }
        else
        {
            PlayStop(0);
        }
        vector2_Last = vector2_Cur;
    }
    public void PlayMove(float speed)
    {
        float_Speed = speed;
        SetAnimatorFloat(BodyPart.Body, "Speed", speed);
        SetAnimatorBool(BodyPart.Body, "Walk", true);
        SetAnimatorFloat(BodyPart.Hand, "Speed", speed);
        SetAnimatorBool(BodyPart.Hand, "Walk", true);
    }
    public void PlayStop(float speed)
    {
        float_Speed = speed;
        SetAnimatorFloat(BodyPart.Body, "Speed", 1);
        SetAnimatorBool(BodyPart.Body, "Walk", false);
        SetAnimatorFloat(BodyPart.Hand, "Speed", 1);
        SetAnimatorBool(BodyPart.Hand, "Walk", false);
    }
    #endregion
    #region//脸部
    /// <summary>
    /// 绘制脸庞
    /// </summary>
    /// <param name="hairID"></param>
    /// <param name="eyeID"></param>
    /// <param name="hairColor"></param>
    public virtual void InitFace(int hairID, int eyeID, Color32 hairColor)
    {

    }
    #endregion
    #region//动画
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
    public virtual void Shake()
    {

    }
    public virtual void Flash()
    {

    }
    #endregion
    #region//转向
[HideInInspector]
    public Vector2 faceDir;
    [HideInInspector]
    public Vector2 turnDir;
    protected bool turnRight = true;
    protected bool faceRight = true;
    public void FaceTo(Vector2 dir)
    {
        faceDir = dir.normalized;
        if (dir.x > 0) FaceRight();
        else FaceLeft();
    }
    /// <summary>
    /// 面向左边
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
    /// 面向右边
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
    public void TurnTo(Vector2 dir)
    {
        turnDir = dir.normalized;
        if (dir.x > 0.1f) TurnRight();
        if (dir.x < -0.1f) TurnLeft();
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
    #region//隐藏
    /// <summary>
    /// 隐藏角色
    /// </summary>
    public virtual void HideActor()
    {

    }
    /// <summary>
    /// 显示角色
    /// </summary>
    public virtual void ShowActor()
    {

    }
    #endregion
    #region//死亡
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