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
    public float speed = 0;


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