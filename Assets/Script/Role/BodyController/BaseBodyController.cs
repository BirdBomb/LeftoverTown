using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 基本身体控制器
/// </summary>
public class BaseBodyController : MonoBehaviour
{
    public float speed;
    [SerializeField, Header("根节点")]
    public Transform Root;
    [SerializeField, Header("头部节点")]
    public Transform Head;
    [SerializeField, Header("身体节点")]
    public Transform Body;
    [SerializeField, Header("上肢节点")]
    public Transform Hand;
    [SerializeField, Header("下肢节点")]
    public Transform Leg;
    [SerializeField, Header("右手节点")]
    public Transform Hand_Right;
    [SerializeField, Header("左手节点")]
    public Transform Hand_Left;

    [SerializeField, Header("右手物品")]
    public Transform Hand_RightItem;
    [SerializeField, Header("左手物品")]
    public Transform Hand_LeftItem;
    /// <summary>
    /// 播放身体动画
    /// </summary>
    public virtual void PlayBodyAction(BodyAction bodyAction, float speed, Action<string> action)
    {

    }
    /// <summary>
    /// 播放头部动画
    /// </summary>
    /// <param name="headAction"></param>
    public virtual void PlayHeadAction(HeadAction headAction, float speed, Action<string> action)
    {

    }
    /// <summary>
    /// 播放手部动画
    /// </summary>
    /// <param name="handAction"></param>
    /// <param name="speed"></param>
    /// <param name="action"></param>
    public virtual void PlayHandAction(HandAction handAction,float speed,Action<string> action)
    {

    }
    /// <summary>
    /// 播放腿部动画
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