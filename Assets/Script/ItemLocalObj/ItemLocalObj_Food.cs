using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.U2D;

public class ItemLocalObj_Food : ItemLocalObj
{
    public SpriteRenderer spriteRenderer_Food;
    public SpriteAtlas spriteAtlas_Item;
    public ParticleSystem bitsParticle;
    public Action<ActorManager> action_Bind;
    private InputData inputData = new InputData();
    private float float_NextEatTiming = 0;
    private const float float_EatCd = 1.2f;
    private void FixedUpdate()
    {
        if (inputData.leftPressTimer == 0 && float_NextEatTiming > 0)
        {
            float_NextEatTiming -= Time.fixedDeltaTime;
        }
    }

    public override void HoldingStart(ActorManager owner, BodyController_Human body)
    {
        actorManager = owner;

        body.AddItemOnRightHand(gameObject, Vector3.zero, Quaternion.identity, Vector3.one);
        spriteRenderer_Food.sprite = spriteAtlas_Item.GetSprite("Item_" + itemData.I.ToString());

        base.HoldingStart(owner, body);
    }
    public void BindFoodAction(Action<ActorManager> action)
    {
        action_Bind = action;
    }

    public override bool PressLeftMouse(float time, ActorAuthority actorAuthority)
    {
        if (inputData.leftPressTimer >= float_NextEatTiming && CanBeEat())
        {
            float_NextEatTiming += float_EatCd;
            PlayEatStart();
            actorManager.bodyController.SetAnimatorTrigger(BodyPart.Hand, "Eat");
            actorManager.bodyController.SetAnimatorTrigger(BodyPart.Head, "Eat");
            actorManager.bodyController.SetAnimatorFunc(BodyPart.Head, TryToEat);

        }
        inputData.leftPressTimer = time;
        return base.PressLeftMouse(time, actorAuthority);
    }
    public override void ReleaseLeftMouse()
    {
        if (inputData.leftPressTimer > 0)
        {
            float_NextEatTiming -= inputData.leftPressTimer;
            inputData.leftPressTimer = 0;
        }
        base.ReleaseLeftMouse();
    }
    public bool TryToEat(string str)
    {
        if (str.Equals("Eat"))
        {
            PlayEatEnd();
            if (action_Bind != null && actorManager.actorAuthority.isLocal) action_Bind(actorManager);
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool CanBeEat()
    {
        return actorManager.actorNetManager.Net_FoodCur < actorManager.actorNetManager.Local_FoodMax;
    }

    public virtual void PlayEatStart()
    {
        PlayParticle();
        if (spriteRenderer_Food)
        {
            spriteRenderer_Food.transform.DOKill();
            spriteRenderer_Food.transform.localScale = Vector3.one;
            spriteRenderer_Food.transform.localPosition = Vector3.zero;
            spriteRenderer_Food.transform.DOScale(new Vector3(0.5f, 0.5f, 1), 2f);
        }
        AudioManager.Instance.Play3DEffect(2004, transform.position);
    }
    public virtual void PlayEatEnd()
    {
        StopParticle();
        if (spriteRenderer_Food)
        {
            spriteRenderer_Food.transform.DOKill();
            spriteRenderer_Food.transform.localScale = Vector3.one;
            spriteRenderer_Food.transform.localPosition = Vector3.zero;
        }
        AudioManager.Instance.Play3DEffect(2005, transform.position);
    }
    public void PlayParticle()
    {
        if (bitsParticle != null) bitsParticle?.Play();
    }
    public void StopParticle()
    {
        if (bitsParticle != null) bitsParticle?.Stop();
    }
}
