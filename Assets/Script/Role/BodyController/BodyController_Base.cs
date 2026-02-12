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
    public BodyAction bodyAction_Cur;
    protected float time_LockBody = 0;
    protected Vector2 vector2_Last;
    protected Vector2 vector2_Cur;
    public virtual void Local_CheckPos(float dt)
    {
        vector2_Cur = transform.position;
        float distance = Vector2.Distance(vector2_Last, vector2_Cur);
        turnDir = (vector2_Cur - vector2_Last).normalized;
        vector2_Last = vector2_Cur;
        float speed = distance / dt;
        if (speed > 0.1f && time_LockBody <= 0)
        {
            PlayWalk(speed);
            if (turnDir.x > 0.1f)
            {
                
                TurnRight();
            }
            if (turnDir.x < -0.1f)
            {

                TurnLeft();
            }
        }
        else
        {
            time_LockBody -= dt;
            PlayStop(0);
        }
    }
    public void Local_ResetPos()
    {
        vector2_Cur = transform.position;
        vector2_Last = transform.position;
    }
    public void PlayWalk(float speed)
    {
        float_Speed = speed;
        if (speed > 2) { speed = 2 + (speed - 2) * 0.2f; }
        bodyAction_Cur = BodyAction.Walk;
        SetAnimatorFloat(BodyPart.Body, "Speed", speed);
        SetAnimatorBool(BodyPart.Body, "Walk", true);
        SetAnimatorFloat(BodyPart.Hand, "Speed", speed);
        SetAnimatorBool(BodyPart.Hand, "Walk", true);
        SetAnimatorFloat(BodyPart.Head, "Speed", speed);
        SetAnimatorBool(BodyPart.Head, "Walk", true);
    }
    public void PlayStop(float speed)
    {
        float_Speed = speed;
        if(bodyAction_Cur == BodyAction.Walk) bodyAction_Cur = BodyAction.Idle;
        SetAnimatorFloat(BodyPart.Body, "Speed", 1);
        SetAnimatorBool(BodyPart.Body, "Walk", false);
        SetAnimatorFloat(BodyPart.Hand, "Speed", 1);
        SetAnimatorBool(BodyPart.Hand, "Walk", false);
        SetAnimatorFloat(BodyPart.Head, "Speed", 1);
        SetAnimatorBool(BodyPart.Head, "Walk", false);
    }
    public void PlayBodyAction(BodyAction bodyAction)
    {
        switch(bodyAction)
        {
            case BodyAction.LayToUp:
                time_LockBody = 0.5f;
                bodyAction_Cur = bodyAction;
                SetAnimatorTrigger(BodyPart.Body, "LayVertical");
                SetAnimatorTrigger(BodyPart.Head, "Sleep");

                break;
            case BodyAction.LayToLeft:
                time_LockBody = 0.5f;
                bodyAction_Cur = bodyAction;
                TurnRight();
                SetAnimatorFloat(BodyPart.Body, "Speed", 1);
                SetAnimatorTrigger(BodyPart.Body, "LayHorizontal");
                SetAnimatorTrigger(BodyPart.Head, "Sleep");
                break;
            case BodyAction.LayToRight:
                time_LockBody = 0.5f;
                bodyAction_Cur = bodyAction;
                TurnLeft();
                SetAnimatorFloat(BodyPart.Body, "Speed", 1);
                SetAnimatorTrigger(BodyPart.Body, "LayHorizontal");
                SetAnimatorTrigger(BodyPart.Head, "Sleep");
                break;
        }
    }
    public void PlayHeadAction(HeadAction headAction)
    {
        switch (headAction)
        {
            case HeadAction.Eat:
                SetAnimatorTrigger(BodyPart.Head, "Eat");
                break;
        }
    }
    public void PlayHandAction(HandAction handAction)
    {
        switch (handAction)
        {
            case HandAction.Eat:
                SetAnimatorTrigger(BodyPart.Hand, "Eat");
                break;
        }
    }

    public void PlayLayToLeft(float speed)
    {
    }
    public void PlayLayToRight(float speed)
    {
    }
    public void PlayLayToUp(float speed)
    {
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
    public virtual void SetAnimatorFunc(BodyPart bodyPart, Func<string, bool> func)
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
    /// <summary>
    /// 面向左边
    /// </summary>
    public virtual void FaceLeft()
    {
        faceRight = false;
    }
    /// <summary>
    /// 面向右边
    /// </summary>
    public virtual void FaceRight()
    {
        faceRight = true;
    }
    /// <summary>
    /// 转向左边
    /// </summary>
    public virtual void TurnLeft()
    {
        turnRight = false;
        if (faceRight) { FaceRight(); }
        else { FaceLeft(); }
    }
    /// <summary>
    /// 转向右边
    /// </summary>
    public virtual void TurnRight()
    {
        turnRight = true;
        if (faceRight) { FaceRight(); }
        else { FaceLeft(); }
    }
    #endregion
    #region//步伐
    protected int int_StepAudioIndex = 0;
    public void SetStep(int stepAudioIndex)
    {
        int_StepAudioIndex = stepAudioIndex;
    }
    public virtual void PlayStep()
    {
        if(int_StepAudioIndex>0) AudioManager.Instance.Play3DEffect(int_StepAudioIndex + new System.Random().Next(0, 9), transform.position);
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
public enum BodyAction
{
    Default,
    Idle,
    Walk,
    Stagger,
    LayToRight,
    LayToLeft,
    LayToUp,
}
public enum HeadAction
{
    Default,
    Eat
}
public enum HandAction
{
    Default,
    Eat
}