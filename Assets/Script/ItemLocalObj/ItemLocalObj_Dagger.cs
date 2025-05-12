using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class ItemLocalObj_Dagger : ItemLocalObj
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private SpriteRenderer spriteRenderer_Hand;
    [SerializeField]
    private SkillIndicators skillIndicators;
    [SerializeField, Header("´Á´ÌÉËº¦")]
    private short config_StabDamage;
    [SerializeField, Header("´Á´Ì·¶Î§")]
    private float config_StabMaxRange;
    [SerializeField, Header("´Á´Ì¾àÀë")]
    private float config_StabMaxDistance;
    [SerializeField, Header("´Á´ÌËÙ¶È")]
    private float config_StabSpeed = 1;
    /// <summary>
    /// Åü¿³¶¯»­Ê±³¤
    /// </summary>
    private float config_StabDuraction = 1;
    /// <summary>
    /// Åü¿³CD
    /// </summary>
    private float config_StabCD;
    /// <summary>
    /// Åü¿³CDµ¹Êý
    /// </summary>
    private float config_StabCDRec;
    /// <summary>
    /// ÏÂ´ÎÈ­»÷Ê±¼ä
    /// </summary>
    private float float_NextStabTiming = 0;

    private InputData inputData = new InputData();
    private void FixedUpdate()
    {
        if (inputData.leftPressTimer == 0 && float_NextStabTiming > 0)
        {
            float_NextStabTiming -= Time.fixedDeltaTime;
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

        config_StabCD = config_StabDuraction / config_StabSpeed;
        config_StabCDRec = config_StabSpeed / config_StabDuraction;

        spriteRenderer_Hand.sprite = body.transform_RightHand.GetComponent<SpriteRenderer>().sprite;
        body.transform_RightHand.GetComponent<SpriteRenderer>().enabled = false;
        base.HoldingByHand(owner, body, data);
    }

    public override bool PressLeftMouse(float time, ActorAuthority actorAuthority)
    {
        if (inputData.leftPressTimer >= float_NextStabTiming)
        {
            float_NextStabTiming += config_StabCD + 0.1f;
            animator.SetTrigger("Stab");
            animator.speed = config_StabSpeed;
        }
        inputData.leftPressTimer = time;
        return base.PressLeftMouse(time, actorAuthority);
    }
    public override void ReleaseLeftMouse()
    {
        if (inputData.leftPressTimer > 0)
        {
            float_NextStabTiming -= inputData.leftPressTimer;
            inputData.leftPressTimer = 0;
        }
        base.ReleaseLeftMouse();
    }
    public override void UpdateMousePos(Vector3 mouse)
    {
        inputData.mousePosition = mouse;
        if (actorManager.actorAuthority.isLocal && actorManager.actorAuthority.isPlayer)
        {
            float alpht = (float_NextStabTiming - inputData.leftPressTimer) * config_StabCDRec;
            skillIndicators.Draw_SkillIndicators(inputData.mousePosition, config_StabMaxDistance, config_StabMaxRange, alpht);
        }
        base.UpdateMousePos(mouse);
    }
    public void Stab()
    {
        if (actorManager.actorAuthority.isLocal)
        {
            sbyte temp = 0;
            skillIndicators.Shake_SkillIndicators(new Vector3(0.2f, 0.2f, 0), 0.1f);
            skillIndicators.Checkout_SkillIndicators(inputData.mousePosition, config_StabMaxDistance, config_StabMaxRange, out Collider2D[] colliders);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].tag.Equals("Actor"))
                {
                    if (colliders[i].isTrigger && colliders[i].transform.TryGetComponent(out ActorManager actor))
                    {
                        if (actor == actorManager) { continue; }
                        else
                        {
                            actor.AllClient_Listen_TakeAttackDamage(config_StabDamage, actorManager.actorNetManager);
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
