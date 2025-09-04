using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class ItemLocalObj_Spear : ItemLocalObj
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Transform transform_Root;
    [SerializeField]
    private SpriteRenderer spriteRenderer_LeftHand;
    [SerializeField]
    private SpriteRenderer spriteRenderer_RightHand;
    [SerializeField]
    private SkillIndicators skillIndicators;

    private int PiercingDamage;
    private float PiercingRange = 10;
    private int SlashingDamage;
    private float SlashingRange = 60;
    private float AttackSpeed;
    private float AttackDistance;
    private float AttackExpend;
    private float AttackAbrasion_Temp;

    /// <summary>
    /// 攻击动画时长
    /// </summary>
    private float config_AttackDuraction = 1;
    /// <summary>
    /// 攻击CD
    /// </summary>
    private float config_AttackCD;
    /// <summary>
    /// 攻击CD倒数
    /// </summary>
    private float config_AttackCDRec;
    /// <summary>
    /// 下次攻击时间
    /// </summary>
    private float float_NextAttackTiming = 0;
    private float float_TempDistance;
    private float float_TempRange;
    private InputData inputData = new InputData();
    private void FixedUpdate()
    {
        if (inputData.leftPressTimer == 0 && inputData.rightPressTimer == 0 && float_NextAttackTiming > 0)
        {
            float_NextAttackTiming -= Time.fixedDeltaTime;
        }
    }

    public override void HoldingStart(ActorManager owner, BodyController_Human body)
    {
        actorManager = owner;

        transform.SetParent(body.transform_ItemInRightHand);
        body.gameObjects_ItemInHand.Add(gameObject);
        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
        spriteRenderer_LeftHand.color = body.transform_LeftHand.GetComponent<SpriteRenderer>().color;
        spriteRenderer_RightHand.color = body.transform_RightHand.GetComponent<SpriteRenderer>().color;
        body.transform_LeftHand.GetComponent<SpriteRenderer>().enabled = false;
        body.transform_RightHand.GetComponent<SpriteRenderer>().enabled = false;

        base.HoldingStart(owner, body);
    }
    public void UpdateSpearData(int piercingDamage, int slashingDamage, float attackSpeed, float attackDistance, float attackExpend, ItemQuality itemQuality)
    {
        PiercingDamage = piercingDamage;
        SlashingDamage = slashingDamage;
        AttackSpeed = attackSpeed;
        AttackDistance = attackDistance;
        AttackExpend = attackExpend;

        config_AttackCD = config_AttackDuraction / attackSpeed;
    }
    public void FaceTo(Vector3 dir)
    {
        if (dir.x >= 0)
        {
            transform_Root.right = dir;
        }
        if (dir.x < 0)
        {
            transform_Root.right = -dir;
        }

    }
    public override void UpdateMousePos(Vector3 mouse)
    {
        inputData.mousePosition = mouse;
        FaceTo(mouse);
        if (actorManager.actorAuthority.isLocal && actorManager.actorAuthority.isPlayer)
        {
            float alpht = (float_NextAttackTiming - inputData.leftPressTimer) * config_AttackCDRec;
            skillIndicators.Draw_SkillIndicators(inputData.mousePosition, float_TempDistance, float_TempRange, alpht);
        }

        base.UpdateMousePos(mouse);
    }
    public override bool PressLeftMouse(float time, ActorAuthority actorAuthority)
    {
        if (inputData.leftPressTimer >= float_NextAttackTiming)
        {
            float_NextAttackTiming += config_AttackCD + 0.1f;
            animator.SetTrigger("Stab");
            animator.speed = AttackSpeed;
        }
        float_TempDistance = AttackDistance;
        float_TempRange = PiercingRange;
        inputData.leftPressTimer = time;
        return base.PressLeftMouse(time, actorAuthority);
    }
    public override bool PressRightMouse(float time, ActorAuthority actorAuthority)
    {
        if (inputData.rightPressTimer >= float_NextAttackTiming)
        {
            float_NextAttackTiming += config_AttackCD + 0.1f;
            animator.SetTrigger("Hack");
            animator.speed = AttackSpeed;
        }
        float_TempDistance = AttackDistance;
        float_TempRange = SlashingRange;
        inputData.rightPressTimer = time;
        return base.PressRightMouse(time, actorAuthority);
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
    public override void ReleaseRightMouse()
    {
        if (inputData.rightPressTimer > 0)
        {
            float_NextAttackTiming -= inputData.rightPressTimer;
            inputData.rightPressTimer = 0;
        }
        base.ReleaseRightMouse();
    }
    public void Stab()
    {
        if (actorManager.actorAuthority.isLocal)
        {
            float temp = 0;
            skillIndicators.Shake_SkillIndicators(new Vector3(0.2f, 0.2f, 0), 0.1f);
            skillIndicators.Checkout_SkillIndicators(inputData.mousePosition, AttackDistance, PiercingRange, out Collider2D[] colliders);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].tag.Equals("Actor"))
                {
                    if (colliders[i].isTrigger && colliders[i].transform.TryGetComponent(out ActorManager actor))
                    {
                        if (actor == actorManager) { continue; }
                        else
                        {
                            actor.AllClient_Listen_TakeDamage(PiercingDamage, DamageState.AttackPiercingDamage, actorManager.actorNetManager);
                            temp = AttackExpend;
                        }
                    }
                }
            }

            AddAbrasion(temp);
        }
    }
    public void Hack()
    {
        if (actorManager.actorAuthority.isLocal)
        {
            float temp = 0;
            skillIndicators.Shake_SkillIndicators(new Vector3(0.2f, 0.2f, 0), 0.1f);
            skillIndicators.Checkout_SkillIndicators(inputData.mousePosition, AttackDistance, SlashingRange, out Collider2D[] colliders);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].tag.Equals("Actor"))
                {
                    if (colliders[i].isTrigger && colliders[i].transform.TryGetComponent(out ActorManager actor))
                    {
                        if (actor == actorManager) { continue; }
                        else
                        {
                            actor.AllClient_Listen_TakeDamage(SlashingDamage, DamageState.AttackSlashingDamage, actorManager.actorNetManager);
                            temp = AttackExpend;
                        }
                    }
                }
            }
            AddAbrasion(temp);
        }
    }
    /// <summary>
    /// 累计损耗
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
                if (_newItem.Item_Durability - offset <= 0)
                {
                    MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemHand_Sub()
                    {
                        item = itemData,
                    });
                }
                else
                {
                    _newItem.Item_Durability -= (sbyte)offset;
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
