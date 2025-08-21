using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController_OnlyBody : BodyController_Base
{
    [SerializeField]
    private Transform transform_Body;
    [SerializeField]
    private SpriteRenderer spriteRenderer_Body;
    [SerializeField]
    private Animator animator_Body;
    [SerializeField]
    private AnimaEventListen animaEventListen_Body;
    private Sequence sequence;
    private Material material;
    public void Start()
    {
        material = new Material(spriteRenderer_Body.sharedMaterial);
        spriteRenderer_Body.material = material;
    }

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
    public override void SetAnimatorFunc(BodyPart bodyPart, Func<string, bool> func)
    {
        if (bodyPart == BodyPart.Body && animator_Body != null)
        {
            animaEventListen_Body.BindTempFunc(func);
        }
        base.SetAnimatorFunc(bodyPart, func);
    }
    public override void TurnRight()
    {
        transform_Body.localScale = new Vector3(1, 1, 1);
        base.TurnRight();
    }
    public override void TurnLeft()
    {
        transform_Body.localScale = new Vector3(-1, 1, 1);
        base.TurnLeft();
    }
    public override void Flash()
    {
        float light = 1;
        if (sequence != null) sequence.Kill();
        sequence = DOTween.Sequence();
        sequence.Insert(0,
            DOTween.To(() => light, x => light = x, 0, 0.2f).SetEase(Ease.InOutSine));
        sequence.OnUpdate(() =>
        { material.SetFloat("_White", light); });

        base.Flash();
    }
    public override void Shake()
    {
        transform_Body.DOKill();
        transform_Body.transform.localScale = new Vector3(transform_Body.transform.localScale.x, 1, 1);
        transform_Body.DOPunchScale(new Vector3(0, -0.1f, 0), 0.2f);
    }
}
