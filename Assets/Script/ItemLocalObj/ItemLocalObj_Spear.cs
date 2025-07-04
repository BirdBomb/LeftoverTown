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
    [SerializeField, Header("�����ٶ�")]
    private float config_AttackSpeed = 1;
    [SerializeField, Header("�����˺�")]
    private short config_StabDamage;
    [SerializeField, Header("���̾���")]
    private short config_StabDistance;
    [SerializeField, Header("���̷�Χ")]
    private short config_StabRange;
    [SerializeField, Header("��ɨ�˺�")]
    private short config_HackDamage;
    [SerializeField, Header("��ɨ����")]
    private short config_HackDistance;
    [SerializeField, Header("��ɨ��Χ")]
    private short config_HackRange;
    /// <summary>
    /// ��������ʱ��
    /// </summary>
    private float config_AttackDuraction = 1;
    /// <summary>
    /// ����CD
    /// </summary>
    private float config_AttackCD;
    /// <summary>
    /// ����CD����
    /// </summary>
    private float config_AttackCDRec;
    /// <summary>
    /// �´ι���ʱ��
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

    public override void HoldingByHand(ActorManager owner, BodyController_Human body, ItemData data)
    {
        actorManager = owner;
        itemData = data;

        transform.SetParent(body.transform_ItemInRightHand);
        body.gameObjects_ItemInHand.Add(gameObject);
        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
        spriteRenderer_LeftHand.color = body.transform_LeftHand.GetComponent<SpriteRenderer>().color;
        spriteRenderer_RightHand.color = body.transform_RightHand.GetComponent<SpriteRenderer>().color;
        body.transform_LeftHand.GetComponent<SpriteRenderer>().enabled = false;
        body.transform_RightHand.GetComponent<SpriteRenderer>().enabled = false;

        config_AttackCD = config_AttackDuraction / config_AttackSpeed;
        config_AttackCDRec = config_AttackSpeed / config_AttackDuraction;

        base.HoldingByHand(owner, body, data);
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
            animator.speed = config_AttackSpeed;
        }
        float_TempDistance = config_StabDistance;
        float_TempRange = config_StabRange;
        inputData.leftPressTimer = time;
        return base.PressLeftMouse(time, actorAuthority);
    }
    public override bool PressRightMouse(float time, ActorAuthority actorAuthority)
    {
        if (inputData.rightPressTimer >= float_NextAttackTiming)
        {
            float_NextAttackTiming += config_AttackCD + 0.1f;
            animator.SetTrigger("Hack");
            animator.speed = config_AttackSpeed;
        }
        float_TempDistance = config_HackDistance;
        float_TempRange = config_HackRange;
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
            sbyte temp = 0;
            skillIndicators.Shake_SkillIndicators(new Vector3(0.2f, 0.2f, 0), 0.1f);
            skillIndicators.Checkout_SkillIndicators(inputData.mousePosition, config_StabDistance, config_StabRange, out Collider2D[] colliders);
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
    public void Hack()
    {
        if (actorManager.actorAuthority.isLocal)
        {
            sbyte temp = 0;
            skillIndicators.Shake_SkillIndicators(new Vector3(0.2f, 0.2f, 0), 0.1f);
            skillIndicators.Checkout_SkillIndicators(inputData.mousePosition, config_HackDistance, config_HackRange, out Collider2D[] colliders);
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
    private void ChangeDurability(sbyte val)
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
