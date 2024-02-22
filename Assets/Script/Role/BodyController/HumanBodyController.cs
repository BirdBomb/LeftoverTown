using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
/// <summary>
/// 人型身体控制器
/// </summary>

public class HumanBodyController : BaseBodyController
{
    public Animator Animator_Head;
    public NetworkMecanimAnimator Animator_Head_Network;
    public AnimaEventListen AnimaEventListen_Head;
    public Animator Animator_Body;
    public NetworkMecanimAnimator Animator_Body_Network;
    public AnimaEventListen AnimaEventListen_Body;
    public Animator Animator_Hand;
    public NetworkMecanimAnimator Animator_Hand_Network;
    public AnimaEventListen AnimaEventListen_Hand;
    public Animator Animator_Leg;
    public NetworkMecanimAnimator Animator_Leg_Network;
    public AnimaEventListen AnimaEventListen_Leg;
    public override void PlayBodyAction(BodyAction bodyAction, float speed, Action<string> action)
    {
        if (bodyAction == BodyAction.Move)
        {
            Animator_Body_Network.Animator.SetBool("Step", true);
            Animator_Body_Network.Animator.SetBool("Idle", false);
            Animator_Body_Network.Animator.speed = speed;

        }
        if (bodyAction == BodyAction.Idle)
        {
            Animator_Body_Network.Animator.SetBool("Step", false);
            Animator_Body_Network.Animator.SetBool("Idle", true);
            Animator_Body_Network.Animator.speed = speed;
        }

        base.PlayBodyAction(bodyAction, speed, null);
    }
    public override void PlayHeadAction(HeadAction headAction, float speed, Action<string> action)
    {
        if (headAction == HeadAction.Move)
        {
            Animator_Head_Network.Animator.SetBool("Step", true);
            Animator_Head_Network.Animator.SetBool("Idle", false);
            Animator_Head_Network.Animator.speed = speed;

        }
        if (headAction == HeadAction.Idle)
        {
            Animator_Head_Network.Animator.SetBool("Step", false);
            Animator_Head_Network.Animator.SetBool("Idle", true);
            Animator_Head_Network.Animator.speed = speed;
        }
        if( headAction == HeadAction.LowerHead)
        {
            Animator_Head_Network.SetTrigger("LowerHead");
            Animator_Head_Network.Animator.speed = speed;
        }
        base.PlayHeadAction(headAction, speed, action);
    }
    public override void PlayHandAction(HandAction handAction, float speed, Action<string> action)
    {
        AnimaEventListen_Hand.BindEvent(action);
        if (handAction == HandAction.PickUp)
        {
            Animator_Hand_Network.SetTrigger("Hand_PickUp");
            Animator_Hand_Network.Animator.speed = speed;
        }
        if (handAction == HandAction.Slash_Horizontal)
        {
            Animator_Hand_Network.SetTrigger("Slash_Horizontal");
            Animator_Hand_Network.Animator.speed = speed;
        }
        if (handAction == HandAction.Slash_Vertical)
        {
            Animator_Hand_Network.SetTrigger("Slash_Vertical");
            Animator_Hand_Network.Animator.speed = speed;
        }
        if (handAction == HandAction.Slash_Vertical_Ready)
        {
            Animator_Hand_Network.SetTrigger("Slash_Vertical_Ready");
            Animator_Hand_Network.Animator.SetBool("Slash_Vertical_Release", false);
            Animator_Hand_Network.Animator.ResetTrigger("Slash_Vertical_Play");
            Animator_Hand_Network.Animator.speed = speed;
        }
        if (handAction == HandAction.Slash_Vertical_Play)
        {
            Animator_Hand_Network.SetTrigger("Slash_Vertical_Play");
            Animator_Hand_Network.Animator.speed = speed;
        }
        if (handAction == HandAction.Slash_Vertical_Release)
        {
            Animator_Hand_Network.Animator.SetBool("Slash_Vertical_Release",true);
            Animator_Hand_Network.Animator.speed = speed;
        }
        if (handAction == HandAction.Step)
        {
            Animator_Hand_Network.Animator.SetBool("Step", true);
            Animator_Hand_Network.Animator.SetBool("Idle", false);
            Animator_Hand_Network.Animator.speed = speed;
        }
        if (handAction == HandAction.Idle)
        {
            Animator_Hand_Network.Animator.SetBool("Step", false);
            Animator_Hand_Network.Animator.SetBool("Idle", true);
            Animator_Hand_Network.Animator.speed = speed;
        }

        base.PlayHandAction(handAction, speed, action);
    }
    public override void PlayLegAction(LegAction legAction, float speed)
    {
        if(legAction == LegAction.Step)
        {
            Animator_Leg_Network.Animator.SetBool("Step", true);
            Animator_Leg_Network.Animator.SetBool("Idle", false);
            Animator_Leg_Network.Animator.speed = speed;
        }
        if (legAction == LegAction.Idle)
        {
            Animator_Leg_Network.Animator.SetBool("Step", false);
            Animator_Leg_Network.Animator.SetBool("Idle", true);
            Animator_Leg_Network.Animator.speed = speed;
        }
        base.PlayLegAction(legAction, speed);
    }
}
