using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
/// <summary>
/// ¸ä×Ó
/// </summary>
public class ItemLocalObj_Pickaxe : ItemLocalObj
{
    [Header("¸ä×Ó×²»÷µã")]
    public Transform transform_PickaxeHit;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private SpriteRenderer spriteRenderer_Hand;
    [SerializeField]
    private SkillIndicators skillIndicators;
    private int BludgeoningDamage;
    private float AttackSpeed;
    private float AttackDistance;
    private float AttackRange = 60;
    private float AttackAbrasion;
    private float AttackAbrasion_Temp;
    private float config_AttackDuraction = 1;
    private float config_AttackCD;
    private float float_NextAttackTiming = 0;

    private InputData inputData = new InputData();
    public void UpdatePickaxeData(int damage, float speed, float distance, float expend, ItemQuality itemQuality)
    {
        BludgeoningDamage = damage;
        AttackSpeed = speed;
        AttackDistance = distance;
        AttackAbrasion = expend;

        config_AttackCD = config_AttackDuraction / AttackSpeed;
    }
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
    public override bool PressLeftMouse(float time, ActorAuthority actorAuthority)
    {
        if (inputData.leftPressTimer >= float_NextAttackTiming)
        {
            float_NextAttackTiming += config_AttackCD + 0.1f;
            animator.SetTrigger("Hack");
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
            skillIndicators.Draw_SkillIndicators(inputData.mousePosition, AttackDistance, AttackRange, alpht);
        }
        base.UpdateMousePos(mouse);
    }
    public void Hack()
    {
        if (actorManager.actorAuthority.isLocal)
        {
            skillIndicators.Shake_SkillIndicators(new Vector3(0.2f, 0.2f, 0), 0.1f);
            skillIndicators.Checkout_SkillIndicators(inputData.mousePosition, AttackDistance, AttackRange, out Collider2D[] colliders);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].tag.Equals("TileObj"))
                {
                    if (colliders[i].TryGetComponent(out BuildingObj building))
                    {
                        HackBuilding(building);
                    }
                }
                else if (colliders[i].tag.Equals("Actor"))
                {
                    if (colliders[i].isTrigger && colliders[i].transform.TryGetComponent(out ActorManager actor))
                    {
                        if (actor != actorManager) { HackActor(actor); }
                    }
                }
            }
        }
    }
    private void HackActor(ActorManager actor)
    {
        GameObject effect = PoolManager.Instance.GetEffectObj("Effect/Effect_Impact");
        effect.GetComponent<Effect_Impact>().PlayBludgeoning(actor.transform.position - actorManager.transform.position, true);
        effect.transform.position = actor.transform.position;

        actor.actorHpManager.TakeDamage(BludgeoningDamage, DamageState.AttackBludgeoningDamage, actorManager.actorNetManager);
        AddAbrasion(AttackAbrasion);
    }
    private void HackBuilding(BuildingObj building)
    {
        int damage = 0;
        damage += building.Local_TakeDamage(BludgeoningDamage, DamageState.AttackBludgeoningDamage, actorManager.actorNetManager);
        if (damage > 0)
        {
            GameObject effect = PoolManager.Instance.GetEffectObj("Effect/Effect_Impact");
            effect.GetComponent<Effect_Impact>().PlayBludgeoning(building.transform.position - actorManager.transform.position, true);
            effect.transform.position = building.transform.position;
        }
        AddAbrasion(AttackAbrasion);
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
