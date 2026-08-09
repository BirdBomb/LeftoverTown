using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.U2D;

public class ItemLocalObj_Potion : ItemLocalObj
{
    public SpriteRenderer spriteRenderer_Potion;
    public SpriteAtlas spriteAtlas_Item;
    public ParticleSystem bitsParticle;
    public Action<ActorManager> action_Bind;
    private InputData inputData = new InputData();
    private float float_NextDrinkTiming = 0;
    private const float float_DrinkCd = 1.2f;

    private void FixedUpdate()
    {
        if (inputData.leftPressTimer == 0 && float_NextDrinkTiming > 0)
        {
            float_NextDrinkTiming -= Time.fixedDeltaTime;
        }
    }

    public override void HoldingStart(ActorManager owner, BodyController_Human body)
    {
        actorManager = owner;

        body.AddItemOnRightHand(gameObject, Vector3.zero, Quaternion.identity, Vector3.one);
        spriteRenderer_Potion.sprite = spriteAtlas_Item.GetSprite("Item_" + itemData.I.ToString());

        base.HoldingStart(owner, body);
    }
    public void BindPotionAction(Action<ActorManager> action)
    {
        action_Bind = action;
    }

    public override bool PressLeftMouse(float time, ActorAuthority actorAuthority)
    {
        if (inputData.leftPressTimer >= float_NextDrinkTiming)
        {
            float_NextDrinkTiming += float_DrinkCd;
            PlayDrinkStart();
            actorManager.bodyController.SetAnimatorTrigger(BodyPart.Hand, "Drink");
            actorManager.bodyController.SetAnimatorTrigger(BodyPart.Head, "Drink");
            actorManager.bodyController.SetAnimatorFunc(BodyPart.Head, TryToDrink);

        }
        inputData.leftPressTimer = time;
        return base.PressLeftMouse(time, actorAuthority);
    }
    public override void ReleaseLeftMouse()
    {
        if (inputData.leftPressTimer > 0)
        {
            float_NextDrinkTiming -= inputData.leftPressTimer;
            inputData.leftPressTimer = 0;
        }
        base.ReleaseLeftMouse();
    }
    public bool TryToDrink(string str)
    {
        if (str.Equals("Drink"))
        {
            PlayDrinkEnd();
            if (action_Bind != null && actorManager.actorAuthority.isLocal) action_Bind(actorManager);
            return true;
        }
        else
        {
            return false;
        }
    }

    public virtual void PlayDrinkStart()
    {
        PlayParticle();
        if (spriteRenderer_Potion)
        {
            spriteRenderer_Potion.transform.DOKill();
            spriteRenderer_Potion.transform.localScale = Vector3.one;
            spriteRenderer_Potion.transform.localPosition = Vector3.zero;
            spriteRenderer_Potion.transform.DOScale(new Vector3(0.5f, 0.5f, 1), 2f);
        }
        AudioManager.Instance.Play3DEffect(2005, transform.position);
    }
    public virtual void PlayDrinkEnd()
    {
        StopParticle();
        if (spriteRenderer_Potion)
        {
            spriteRenderer_Potion.transform.DOKill();
            spriteRenderer_Potion.transform.localScale = Vector3.one;
            spriteRenderer_Potion.transform.localPosition = Vector3.zero;
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
