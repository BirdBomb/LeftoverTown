using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController_BodyHead : BodyController_Base
{
    public Transform transform_Body;
    public SpriteRenderer spriteRenderer_Body;
    public Animator animator_Body;
    public AnimaEventListen animaEventListen_Body;
    private Material material_Body;

    public Transform transform_Head;
    public SpriteRenderer spriteRenderer_Head;
    public Animator animator_Head;
    public AnimaEventListen animaEventListen_Head;
    private Material material_Head;

    private Sequence sequence_Body;
    public override void Start()
    {
        material_Body = new Material(spriteRenderer_Body.sharedMaterial);
        spriteRenderer_Body.material = material_Body;
        material_Head = new Material(spriteRenderer_Head.sharedMaterial);
        spriteRenderer_Head.material = material_Head;
        base.Start();
    }

    public override void SetAnimatorTrigger(BodyPart bodyPart, string name)
    {
        if (bodyPart == BodyPart.Body && animator_Body != null)
        {
            animator_Body.SetTrigger(name);
        }
        if (bodyPart == BodyPart.Head && animator_Head != null)
        {
            animator_Head.SetTrigger(name);
        }
        base.SetAnimatorTrigger(bodyPart, name);
    }
    public override void SetAnimatorBool(BodyPart bodyPart, string name, bool val)
    {
        if (bodyPart == BodyPart.Body && animator_Body != null)
        {
            animator_Body.SetBool(name, val);
        }
        if (bodyPart == BodyPart.Head && animator_Head != null)
        {
            animator_Head.SetBool(name, val);
        }
        base.SetAnimatorBool(bodyPart, name, val);
    }
    public override void SetAnimatorFloat(BodyPart bodyPart, string name, float val)
    {
        if (bodyPart == BodyPart.Body && animator_Body != null)
        {
            animator_Body.SetFloat(name, val);
        }
        if (bodyPart == BodyPart.Head && animator_Head != null)
        {
            animator_Head.SetFloat(name, val);
        }
        base.SetAnimatorFloat(bodyPart, name, val);
    }
    public override void SetAnimatorFunc(BodyPart bodyPart, Func<string, bool> func)
    {
        if (bodyPart == BodyPart.Body && animator_Body != null)
        {
            animaEventListen_Body.BindTempFunc(func);
        }
        if (bodyPart == BodyPart.Head && animator_Head != null)
        {
            animaEventListen_Head.BindTempFunc(func);
        }
        base.SetAnimatorFunc(bodyPart, func);
    }
    public override void TurnRight()
    {
        transform_Body.localScale = new Vector3(1, 1, 1);
        transform_Head.localScale = new Vector3(1, 1, 1);
        base.TurnRight();
    }
    public override void TurnLeft()
    {
        transform_Body.localScale = new Vector3(-1, 1, 1);
        transform_Head.localScale = new Vector3(-1, 1, 1);
        base.TurnLeft();
    }
    public override void Flash()
    {
        float light = 1;
        if (sequence_Body != null) sequence_Body.Kill();
        sequence_Body = DOTween.Sequence();
        sequence_Body.Insert(0,
            DOTween.To(() => light, x => light = x, 0, 0.2f).SetEase(Ease.InOutSine));
        sequence_Body.OnUpdate(() =>
        { material_Body.SetFloat("_White", light); material_Head.SetFloat("_White", light); });

        base.Flash();
    }
    public override void Shake()
    {
        transform_Body.DOKill();
        transform_Body.transform.localScale = new Vector3(transform_Body.transform.localScale.x, 1, 1);
        transform_Body.DOPunchScale(new Vector3(0, -0.1f, 0), 0.2f);
    }

}
