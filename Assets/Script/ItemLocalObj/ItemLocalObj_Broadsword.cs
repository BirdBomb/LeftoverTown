using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class ItemLocalObj_Broadsword : ItemLocalObj
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private SpriteRenderer spriteRenderer_Hand;
    [SerializeField]
    private SkillIndicators skillIndicators;
    [SerializeField, Header("Åü¿³ÉËº¦")]
    private short config_HackDamage;
    [SerializeField, Header("Åü¿³ËÙ¶È")]
    private float config_hackSpeed = 1;
    [SerializeField, Header("Åü¿³·¶Î§")]
    private float config_HackMaxRange;
    [SerializeField, Header("Åü¿³¾àÀë")]
    private float config_HackMaxDistance;
    /// <summary>
    /// Åü¿³¶¯»­Ê±³¤
    /// </summary>
    private float config_HackDuraction = 1;
    /// <summary>
    /// Åü¿³CD
    /// </summary>
    private float config_HackCD;
    /// <summary>
    /// Åü¿³CDµ¹Êý
    /// </summary>
    private float config_HackCDRec;
    /// <summary>
    /// ÏÂ´ÎÈ­»÷Ê±¼ä
    /// </summary>
    private float float_NextHackTiming = 0;


    private InputData inputData = new InputData();
    private void FixedUpdate()
    {
        if (inputData.leftPressTimer == 0 && float_NextHackTiming > 0)
        {
            float_NextHackTiming -= Time.fixedDeltaTime;
        }
    }

    public override void HoldingByHand(ActorManager owner, BodyController_Human body, ItemData data)
    {
        itemData = data;
        actorManager = owner;

        transform.SetParent(body.transform_ItemInRightHand);
        body.gameObjects_ItemInHand.Add(gameObject);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;

        config_HackCD = config_HackDuraction / config_hackSpeed;
        config_HackCDRec = config_hackSpeed / config_HackDuraction;

        spriteRenderer_Hand.sprite = body.transform_RightHand.GetComponent<SpriteRenderer>().sprite;
        body.transform_RightHand.GetComponent<SpriteRenderer>().enabled = false;
        base.HoldingByHand(owner, body, data);
    }

    public override bool PressLeftMouse(float time, ActorAuthority actorAuthority)
    {
        if (inputData.leftPressTimer >= float_NextHackTiming)
        {
            float_NextHackTiming += config_HackCD;
            animator.SetTrigger("Hack");
            animator.speed = config_hackSpeed;
        }
        inputData.leftPressTimer = time;
        return base.PressLeftMouse(time, actorAuthority);
    }
    public override void ReleaseLeftMouse()
    {
        if (inputData.leftPressTimer > 0)
        {
            float_NextHackTiming -= inputData.leftPressTimer;
            inputData.leftPressTimer = 0;
        }
        base.ReleaseLeftMouse();
    }
    public override void UpdateMousePos(Vector3 mouse)
    {
        inputData.mousePosition = mouse;
        if (actorManager.actorAuthority.isLocal && actorManager.actorAuthority.isPlayer)
        {
            float alpht = (float_NextHackTiming - inputData.leftPressTimer) * config_HackCDRec;
            skillIndicators.Draw_SkillIndicators(inputData.mousePosition, config_HackMaxDistance, config_HackMaxRange, alpht);
        }
        base.UpdateMousePos(mouse);
    }
    public void Hack()
    {
        if (actorManager.actorAuthority.isLocal)
        {
            sbyte temp = 0;
            skillIndicators.Shake_SkillIndicators(new Vector3(0.2f, 0.2f, 0), 0.1f);
            skillIndicators.Checkout_SkillIndicators(inputData.mousePosition, config_HackMaxDistance, config_HackMaxRange, out Collider2D[] colliders);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].tag.Equals("Actor"))
                {
                    if (colliders[i].isTrigger && colliders[i].transform.TryGetComponent(out ActorManager actor))
                    {
                        if (actor == actorManager) { continue; }
                        else
                        {
                            actor.AllClient_Listen_TakeAttackDamage(config_HackDamage, actorManager.actorNetManager);
                            temp = -2;
                        }
                    }
                }
            }
            ChangeDurability(temp);
        }
    }
    public void ChangeDurability(sbyte val)
    {
        if (val != 0 && actorManager.actorAuthority.isPlayer)
        {
            ItemData _oldItem = itemData;
            ItemData _newItem = itemData;
            if (_newItem.Item_Durability + val <= 0)
            {
                MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TrySubItemOnHand()
                {
                    item = itemData,
                });
            }
            else
            {
                _newItem.Item_Durability += val;
                MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemOnHand()
                {
                    oldItem = _oldItem,
                    newItem = _newItem,
                });
            }
        }
    }
}
