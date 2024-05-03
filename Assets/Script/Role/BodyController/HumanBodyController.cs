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
    public override void PlayHandAction(HandAction handAction, float speed, Action<string> action)
    {
        AnimaEventListen_Hand.BindEvent(action);
        if (handAction == HandAction.PutUp)
        {
            Animator_Hand.SetTrigger("PutUp");
            Animator_Hand.speed = speed;
            return;
        }
        else if (handAction == HandAction.Eat)
        {
            Animator_Hand.SetTrigger("Eat");
            Animator_Hand.speed = speed;
            return;
        }
        else if (handAction == HandAction.Charge)
        {
            Animator_Hand.SetTrigger("Charge");
            Animator_Hand.speed = speed;
            return;
        }
        else if (handAction == HandAction.Bow_Ready)
        {
            Animator_Hand.SetTrigger("Bow_Ready");
            Animator_Hand.SetBool("Bow_Release", false);
            Animator_Hand.ResetTrigger("Bow_Play");
            Animator_Hand.speed = speed;
            return;
        }
        else if (handAction == HandAction.Bow_Play)
        {
            Animator_Hand.SetTrigger("Bow_Play");
            Animator_Hand.speed = speed;
            return;
        }
        else if (handAction == HandAction.Bow_Release)
        {
            Animator_Hand.SetBool("Bow_Release", true);
            Animator_Hand.speed = speed;
            return;
        }
        base.PlayHandAction(handAction, speed, action);
    }
}
