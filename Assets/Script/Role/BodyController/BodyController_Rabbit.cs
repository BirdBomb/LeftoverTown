using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController_Rabbit : BodyController_Base
{
    [SerializeField]
    private Transform transform_Body;
    [SerializeField]
    private Animator animator_Body;
    [SerializeField]
    private AnimaEventListen animaEventListen_Body;
    public override void SetAnimatorTrigger(BodyPart bodyPart, string name)
    {
        if (bodyPart == BodyPart.Body && animator_Body != null)
        {
            animator_Body.SetTrigger(name);
        }
        base.SetAnimatorTrigger(bodyPart, name);
    }
    public override void SetAnimatorBool(BodyPart bodyPart, string name, bool val)
    {
        if (bodyPart == BodyPart.Body && animator_Body != null)
        {
            animator_Body.SetBool(name, val);
        }
        base.SetAnimatorBool(bodyPart, name, val);
    }
    public override void SetAnimatorFloat(BodyPart bodyPart, string name, float val)
    {
        if (bodyPart == BodyPart.Body && animator_Body != null)
        {
            animator_Body.SetFloat(name, val);
        }
        base.SetAnimatorFloat(bodyPart, name, val);
    }
    public override void SetAnimatorAction(BodyPart bodyPart, Action<string> action)
    {
        if (bodyPart == BodyPart.Body && animator_Body != null)
        {
            animaEventListen_Body.BindTempEvent(action);
        }
        base.SetAnimatorAction(bodyPart, action);
    }
    public override void Turn(bool right)
    {
        if (right)
        {
            transform_Body.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform_Body.localScale = new Vector3(-1, 1, 1);
        }
        base.Turn(right);
    }
}
