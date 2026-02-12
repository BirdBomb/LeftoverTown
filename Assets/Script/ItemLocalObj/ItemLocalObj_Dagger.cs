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

    private int PiercingDamage;
    private float AttackSpeed;
    private float AttackExpend;
    private float AttackAbrasion_Temp;

    /// <summary>
    /// ´Á´Ì¶¯»­Ê±³¤
    /// </summary>
    private float config_AttackDuraction = 1;
    /// <summary>
    /// ´Á´Ì¾àÀë
    /// </summary>
    private float config_AttackMaxDistance = 1;
    /// <summary>
    /// ´Á´Ì·¶Î§
    /// </summary>
    private float config_AttackMaxRange = 20;
    /// <summary>
    /// ´Á´ÌCD
    /// </summary>
    private float config_AttackCD;
    /// <summary>
    /// ÏÂ´Î´Á´ÌÊ±¼ä
    /// </summary>
    private float float_NextAttackTiming = 0;

    private InputData inputData = new InputData();
    private void FixedUpdate()
    {
        if (inputData.leftPressTimer == 0 && float_NextAttackTiming > 0)
        {
            float_NextAttackTiming -= Time.fixedDeltaTime;
        }
    }

    public override void HoldingStart(ActorManager owner, BodyController_Human body)
    {
        actorManager = owner;

        transform.SetParent(body.transform_ItemInRightHand);
        body.gameObjects_ItemInHand.Add(gameObject);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;


        spriteRenderer_Hand.color = body.transform_RightHand.GetComponent<SpriteRenderer>().color;
        body.transform_RightHand.GetComponent<SpriteRenderer>().enabled = false;
        base.HoldingStart(owner, body);
    }
    public void UpdateDaggerData(int attackDamage,float attackSpeed,float attackExpend,ItemQuality itemQuality)
    {
        PiercingDamage = attackDamage;
        AttackSpeed = attackSpeed;
        AttackExpend = attackExpend;
        config_AttackCD = config_AttackDuraction / AttackSpeed;
    }
    public override bool PressLeftMouse(float time, ActorAuthority actorAuthority)
    {
        if (inputData.leftPressTimer >= float_NextAttackTiming)
        {
            float_NextAttackTiming += config_AttackCD + 0.1f;
            animator.SetTrigger("Stab");
            animator.speed = AttackSpeed;
        }
        inputData.leftPressTimer = time;
        return base.PressLeftMouse(time, actorAuthority);
    }
    public override void ReleaseLeftMouse()
    {
        if (inputData.leftPressTimer > 0)
        {
            float_NextAttackTiming -= inputData.leftPressTimer;
            inputData.leftPressTimer = 0;
        }
        base.ReleaseLeftMouse();
    }
    public override void UpdateMousePos(Vector3 mouse)
    {
        inputData.mousePosition = mouse;
        if (actorManager.actorAuthority.isLocal && actorManager.actorAuthority.isPlayer)
        {
            float alpht = (float_NextAttackTiming - inputData.leftPressTimer) / config_AttackCD;
            skillIndicators.Draw_SkillIndicators(inputData.mousePosition, config_AttackMaxDistance, config_AttackMaxRange, alpht);
        }
        base.UpdateMousePos(mouse);
    }
    public void Stab()
    {
        if (actorManager.actorAuthority.isLocal)
        {
            float temp = 0;
            skillIndicators.Shake_SkillIndicators(new Vector3(0.2f, 0.2f, 0), 0.1f);
            skillIndicators.Checkout_SkillIndicators(inputData.mousePosition, config_AttackMaxDistance, config_AttackMaxRange, out Collider2D[] colliders);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].tag.Equals("Actor"))
                {
                    if (colliders[i].isTrigger && colliders[i].transform.TryGetComponent(out ActorManager actor))
                    {
                        if (actor != actorManager) { StabActor(actor); }
                        else
                        {
                            temp = AttackExpend;
                        }
                    }
                }
            }
        }
    }
    private void StabActor(ActorManager actor)
    {
        GameObject effect = PoolManager.Instance.GetEffectObj("Effect/Effect_Impact");
        effect.GetComponent<Effect_Impact>().PlayPiercing(actor.transform.position - actorManager.transform.position);
        effect.transform.position = actor.transform.position;

        actor.actorHpManager.TakeDamage(PiercingDamage, DamageState.AttackPiercingDamage, actorManager.actorNetManager);
        AddAbrasion(AttackExpend);
    }
    /// <summary>
    /// ÀÛ¼ÆËðºÄ
    /// </summary>
    /// <param name="val"></param>
    public void AddAbrasion(float val)
    {
        AttackAbrasion_Temp += val;
        if (AttackAbrasion_Temp >= 1)
        {
            int offset = (int)Math.Floor(AttackAbrasion_Temp);
            AttackAbrasion_Temp = AttackAbrasion_Temp - offset;
            if (val != 0 && actorManager.actorAuthority.isPlayer)
            {
                ItemData _oldItem = itemData;
                ItemData _newItem = itemData;
                if (_newItem.D - offset <= 0)
                {
                    MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemHand_Sub()
                    {
                        item = itemData,
                    });
                }
                else
                {
                    _newItem.D -= (sbyte)offset;
                    MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemHand_Change()
                    {
                        oldItem = _oldItem,
                        newItem = _newItem,
                    });
                }
            }

        }
    }
}
