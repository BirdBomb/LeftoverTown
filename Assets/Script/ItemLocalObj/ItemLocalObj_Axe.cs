using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Windows;
using static Fusion.Sockets.NetBitBuffer;
/// <summary>
/// ¸«×Ó
/// </summary>
public class ItemLocalObj_Axe : ItemLocalObj
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private SpriteRenderer spriteRenderer_Hand;
    [SerializeField]
    private SkillIndicators skillIndicators;
    /// <summary>
    /// Åü¿³ÉËº¦
    /// </summary>
    private int HackDamage;
    /// <summary>
    /// Åü¿³ËÙ¶È
    /// </summary>
    private float HackSpeed;
    /// <summary>
    /// Åü¿³¾àÀë
    /// </summary>
    private float HackDistance;
    /// <summary>
    /// Åü¿³·¶Î§
    /// </summary>
    private float HackRange = 60;
    /// <summary>
    /// Åü¿³Ä¥Ëð
    /// </summary>
    private float HackAbrasion;
    private float HackAbrasion_Temp;
    /// <summary>
    /// Åü¿³¶¯»­Ê±³¤
    /// </summary>
    private float config_HackDuraction = 1;
    /// <summary>
    /// Åü¿³CD
    /// </summary>
    private float config_HackCD;
    /// <summary>
    /// ÏÂ´ÎÅü¿³Ê±¼ä
    /// </summary>
    private float float_NextHackTiming = 0;

    private InputData inputData = new InputData();
    public void UpdateAexData(int hackDamage, float hackSpeed, float hackDistance, float hackExpend, ItemQuality itemQuality)
    {
        HackDamage = hackDamage;
        HackSpeed = hackSpeed;
        HackDistance = hackDistance;
        HackAbrasion = hackExpend;

        config_HackCD = config_HackDuraction / HackSpeed;
    }
    private void FixedUpdate()
    {
        if (inputData.leftPressTimer == 0 && float_NextHackTiming > 0)
        {
            float_NextHackTiming -= Time.fixedDeltaTime;
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
    public override bool PressLeftMouse(float time, ActorAuthority actorAuthority)
    {
        if (inputData.leftPressTimer >= float_NextHackTiming)
        {
            float_NextHackTiming += config_HackCD + 0.1f;
            animator.SetTrigger("Hack");
            animator.speed = HackSpeed;
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
            float alpht = (float_NextHackTiming - inputData.leftPressTimer) / config_HackCD;
            skillIndicators.Draw_SkillIndicators(inputData.mousePosition, HackDistance, HackRange, alpht);
        }
        base.UpdateMousePos(mouse);
    }
    public void Hack()
    {
        if (actorManager.actorAuthority.isLocal)
        {
            float temp = 0;
            skillIndicators.Shake_SkillIndicators(new Vector3(0.2f, 0.2f, 0), 0.1f);
            skillIndicators.Checkout_SkillIndicators(inputData.mousePosition, HackDistance, HackRange, out Collider2D[] colliders);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].tag.Equals("BuildingNatural"))
                {
                    if (colliders[i].TryGetComponent(out BuildingObj building))
                    {
                        building.Local_TakeDamage(HackDamage);
                        temp = HackAbrasion;
                    }
                }
                else if (colliders[i].tag.Equals("Actor"))
                {
                    if (colliders[i].isTrigger && colliders[i].transform.TryGetComponent(out ActorManager actor))
                    {
                        if (actor == actorManager) { continue; }
                        else
                        {
                            actor.AllClient_Listen_TakeAttackDamage(HackDamage, actorManager.actorNetManager);
                            temp = HackAbrasion;
                        }
                    }
                }
            }
            AddAbrasion(temp);
        }
    }
    /// <summary>
    /// ÀÛ¼ÆËðºÄ
    /// </summary>
    /// <param name="val"></param>
    public void AddAbrasion(float val)
    {
        HackAbrasion_Temp += val;
        if (HackAbrasion_Temp >= 1)
        {
            int offset = (int)Math.Floor(HackAbrasion_Temp);
            HackAbrasion_Temp = HackAbrasion_Temp - offset;
            if (val != 0 && actorManager.actorAuthority.isPlayer)
            {
                ItemData _oldItem = itemData;
                ItemData _newItem = itemData;
                if (_newItem.Item_Durability - offset <= 0)
                {
                    MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TrySubItemOnHand()
                    {
                        item = itemData,
                    });
                }
                else
                {
                    _newItem.Item_Durability -= (sbyte)offset;
                    MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemOnHand()
                    {
                        oldItem = _oldItem,
                        newItem = _newItem,
                    });
                }
            }

        }
    }
}
