using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 基本身体控制器
/// </summary>
public class BaseBodyController : MonoBehaviour
{
    [HideInInspector]
    public float speed;
    [HideInInspector]
    public bool locking = false;
    [SerializeField, Header("根节点")]
    public Transform Tran_Root;
    [SerializeField, Header("头部节点")]
    public Transform Tran_Head;
    [SerializeField, Header("身体节点")]
    public Transform Tran_Body;
    [SerializeField, Header("上肢节点")]
    public Transform Tran_BothHand;
    [SerializeField, Header("下肢节点")]
    public Transform Tran_BothLeg;
    [SerializeField, Header("右手节点")]
    public Transform Tran_RightHand;
    [SerializeField, Header("左手节点")]
    public Transform Tran_LeftHand;

    [SerializeField, Header("右手物品")]
    public Transform Tran_RightItemInHand;
    [SerializeField, Header("左手物品")]
    public Transform Tran_LeftItemInHand;
    [SerializeField, Header("头部物品")]
    public Transform Tran_ItemOnHead;
    [SerializeField, Header("身体物品")]
    public Transform Tran_ItemOnBody;


    public Animator Animator_Head;
    public AnimaEventListen AnimaEventListen_Head;
    public Animator Animator_Body;
    public AnimaEventListen AnimaEventListen_Body;
    public Animator Animator_Hand;
    public AnimaEventListen AnimaEventListen_Hand;
    public Animator Animator_Leg;
    public AnimaEventListen AnimaEventListen_Leg;
    #region//初始化
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
    #region//头部
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
    #region//身体
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
    #region//手部
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
    #region//腿部
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

    #region//转向
    [HideInInspector]
    public Vector2 faceDir;
    private bool turnRight = false;
    private bool turnLeft = false;
    private bool faceRight = false;
    private bool faceLeft = false;

    /// <summary>
    /// 面向左边
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
    /// 面向右边
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
    /// 转向左边
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
    /// 转向右边
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
    #region//预设
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
    /// 水平劈砍
    /// </summary>
    Slash_Horizontal,
    /// <summary>
    /// 水平劈砍准备
    /// </summary>
    Slash_Horizontal_Ready,
    /// <summary>
    /// 竖直劈砍
    /// </summary>
    Slash_Vertical,
    /// <summary>
    /// 竖直劈砍准备
    /// </summary>
    Slash_Vertical_Ready,
    /// <summary>
    /// 竖直劈砍播放
    /// </summary>
    Slash_Vertical_Play,
    /// <summary>
    /// 竖直劈砍释放
    /// </summary>
    Slash_Vertical_Release,
    /// <summary>
    /// 拉弓准备
    /// </summary>
    Bow_Ready,
    /// <summary>
    /// 拉弓释放
    /// </summary>
    Bow_Play,
    /// <summary>
    /// 拉弓释放
    /// </summary>
    Bow_Release,

    /// <summary>
    /// 戳刺
    /// </summary>
    Spear,
    /// <summary>
    /// 举起
    /// </summary>
    Lift,
    /// <summary>
    /// 后坐力
    /// </summary>
    Recoil,
    /// <summary>
    /// 拾起
    /// </summary>
    PickUp,
    /// <summary>
    /// 举起
    /// </summary>
    PutUp,
    /// <summary>
    /// 呼吸
    /// </summary>
    Idle,
    /// <summary>
    /// 步伐
    /// </summary>
    Step,
    /// <summary>
    /// 冲刺
    /// </summary>
    Charge,
    /// <summary>
    /// 吃
    /// </summary>
    Eat,
    /// <summary>
    /// 死亡
    /// </summary>
    Dead
}
public enum LegAction
{
    Idle,//静止
    Step,//步伐
    Charge,//冲刺
    Dead
}
public enum BodyPart
{
    Head,
    Body,
    Hand,
    Leg,
}