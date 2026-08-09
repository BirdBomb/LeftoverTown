using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class ItemLocalObj_Throwable : ItemLocalObj
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private SpriteRenderer spriteRenderer_Hand;
    private float config_ThrowCD = 1;
    private float float_NextThrowTiming = 0;

    protected InputData inputData = new InputData();
    private void FixedUpdate()
    {
        if (inputData.leftPressTimer == 0 && float_NextThrowTiming > 0)
        {
            float_NextThrowTiming -= Time.fixedDeltaTime;
        }
    }
    public override void HoldingStart(ActorManager owner, BodyController_Human body)
    {
        actorManager = owner;

        body.AddItemOnRightHand(gameObject, Vector3.zero, Quaternion.identity, Vector3.one);
        spriteRenderer_Hand.color = body.ShowRightHand(false).color;

        base.HoldingStart(owner, body);
    }
    public override bool PressLeftMouse(float time, ActorAuthority actorAuthority)
    {
        if (inputData.leftPressTimer >= float_NextThrowTiming)
        {
            float_NextThrowTiming += config_ThrowCD + 0.1f;
            animator.SetTrigger("Throw");
        }
        inputData.leftPressTimer = time;
        return base.PressLeftMouse(time, actorAuthority);
    }
    public override void ReleaseLeftMouse()
    {
        if (inputData.leftPressTimer > 0)
        {
            float_NextThrowTiming -= inputData.leftPressTimer;
            inputData.leftPressTimer = 0;
        }
        base.ReleaseLeftMouse();
    }
    public override void UpdateMousePos(Vector3 mouse)
    {
        inputData.mousePosition = mouse;
        base.UpdateMousePos(mouse);
    }
    public virtual void Throw()
    {
        GameObject obj_2 = PoolManager.Instance.GetObject("Bullet/Bullet_Grenade");
        if (obj_2.TryGetComponent(out Bullet_Throwable bulletBase_2))
        {
            bulletBase_2.SetPath(transform.position, transform.position + inputData.mousePosition, 9);

            bulletBase_2.SetOwner(actorManager);
        }

    }
}
