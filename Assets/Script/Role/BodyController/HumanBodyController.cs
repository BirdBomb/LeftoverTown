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
    public AnimaEventListen AnimaEventListen_Head;
    public Animator Animator_Body;
    public AnimaEventListen AnimaEventListen_Body;
    public Animator Animator_Hand;
    public AnimaEventListen AnimaEventListen_Hand;
    public Animator Animator_Leg;
    public AnimaEventListen AnimaEventListen_Leg;
    public override void PlayBodyAction(BodyAction bodyAction, float speed, Action<string> action)
    {
        AnimaEventListen_Body.BindEvent(action);

        if (bodyAction == BodyAction.Dead)
        {
            Animator_Body.SetTrigger("Dead");
            Animator_Head.speed = speed;
            return;
        }

        if (bodyAction == BodyAction.Charge)
        {
            Animator_Body.SetTrigger("Charge");
            Animator_Body.speed = speed;
        }

        if (bodyAction == BodyAction.Move)
        {
            Animator_Body.SetBool("Step", true);
            Animator_Body.SetBool("Idle", false);
            Animator_Body.speed = speed;

        }
        if (bodyAction == BodyAction.Idle)
        {
            Animator_Body.SetBool("Idle", true);
            Animator_Body.SetBool("Step", false);
            Animator_Body.speed = speed;
        }

        base.PlayBodyAction(bodyAction, speed, null);
    }
    public override void PlayHeadAction(HeadAction headAction, float speed, Action<string> action)
    {
        AnimaEventListen_Head.BindEvent(action);
        if (headAction == HeadAction.Dead)
        {
            Animator_Head.SetTrigger("Dead");
            Animator_Head.speed = speed;
            return;
        }
        if (headAction == HeadAction.Move)
        {
            Animator_Head.SetBool("Step", true);
            Animator_Head.SetBool("Idle", false);
            Animator_Head.speed = speed;
            return;
        }
        if (headAction == HeadAction.Idle)
        {
            Animator_Head.SetBool("Step", false);
            Animator_Head.SetBool("Idle", true);
            Animator_Head.speed = speed;
            return;
        }
        if (headAction == HeadAction.Bite)
        {
            Animator_Head.SetTrigger("Bite");
            Animator_Head.speed = speed;
            return;
        }
        if (headAction == HeadAction.LowerHead)
        {
            Animator_Head.SetTrigger("LowerHead");
            Animator_Head.speed = speed;
            return;
        }
        if (headAction == HeadAction.TakeDamage)
        {
            Animator_Head.SetTrigger("TakeDamage");
            Animator_Head.speed = speed;
            return;
        }
        if (headAction == HeadAction.Charge)
        {
            Animator_Head.SetTrigger("Charge");
            Animator_Head.speed = speed;
            return;
        }
        base.PlayHeadAction(headAction, speed, action);
    }
    public override void PlayHandAction(HandAction handAction, float speed, Action<string> action)
    {
        AnimaEventListen_Hand.BindEvent(action);
        if (handAction == HandAction.Dead)
        {
            Animator_Hand.SetTrigger("Dead");
            Animator_Hand.speed = speed;
            return;
        }
        if (handAction == HandAction.PickUp)
        {
            Animator_Hand.SetTrigger("Hand_PickUp");
            Animator_Hand.speed = speed;
            return;
        }
        if (handAction == HandAction.Charge)
        {
            Animator_Hand.SetTrigger("Charge");
            Animator_Hand.speed = speed;
            return;
        }
        if (handAction == HandAction.Slash_Horizontal)
        {
            Animator_Hand.SetTrigger("Slash_Horizontal");
            Animator_Hand.speed = speed;
            return;
        }
        if (handAction == HandAction.Slash_Vertical)
        {
            Animator_Hand.SetTrigger("Slash_Vertical");
            Animator_Hand.speed = speed;
            return;
        }
        if (handAction == HandAction.Slash_Vertical_Ready)
        {
            Animator_Hand.SetTrigger("Slash_Vertical_Ready");
            Animator_Hand.SetBool("Slash_Vertical_Release", false);
            Animator_Hand.ResetTrigger("Slash_Vertical_Play");
            Animator_Hand.speed = speed;
            return;
        }
        if (handAction == HandAction.Slash_Vertical_Play)
        {
            Animator_Hand.SetTrigger("Slash_Vertical_Play");
            Animator_Hand.speed = speed;
            return;
        }
        if (handAction == HandAction.Slash_Vertical_Release)
        {
            Animator_Hand.SetBool("Slash_Vertical_Release",true);
            Animator_Hand.speed = speed;
            return;
        }
        if (handAction == HandAction.Step)
        {
            Animator_Hand.SetBool("Step", true);
            Animator_Hand.SetBool("Idle", false);
            Animator_Hand.speed = speed;
            return;
        }
        if (handAction == HandAction.Idle)
        {
            Animator_Hand.SetBool("Step", false);
            Animator_Hand.SetBool("Idle", true);
            Animator_Hand.speed = speed;
            return;
        }
        base.PlayHandAction(handAction, speed, action);
    }
    public override void PlayLegAction(LegAction legAction, float speed, Action<string> action)
    {
        AnimaEventListen_Leg.BindEvent(action);
        if (legAction == LegAction.Dead)
        {
            Animator_Leg.SetTrigger("Dead");
            Animator_Leg.speed = speed;
            return;
        }
        if (legAction == LegAction.Charge)
        {
            Animator_Leg.SetTrigger("Charge");
            Animator_Leg.speed = speed;
            return;
        }

        if (legAction == LegAction.Step)
        {
            Animator_Leg.SetBool("Step", true);
            Animator_Leg.SetBool("Idle", false);
            Animator_Leg.speed = speed;
            return;
        }
        if (legAction == LegAction.Idle)
        {
            Animator_Leg.SetBool("Step", false);
            Animator_Leg.SetBool("Idle", true);
            Animator_Leg.speed = speed;
            return;
        }
        base.PlayLegAction(legAction, speed, action);
    }
}
