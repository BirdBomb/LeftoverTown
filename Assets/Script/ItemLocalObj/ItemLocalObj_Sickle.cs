using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
/// <summary>
/// Á­µ¶
/// </summary>
public class ItemLocalObj_Sickle : ItemLocalObj
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private SpriteRenderer spriteRenderer_Hand;
    [SerializeField]
    private SkillIndicators skillIndicators;
    private int AttackDamage;
    private float AttackSpeed;
    private float AttackDistance = 1;
    private float AttackRange = 60;
    private float AttackAbrasion;
    private float AttackAbrasion_Temp;
    private float config_AttackDuraction = 1;
    private float config_AttackCD;
    private float float_NextAttackTiming = 0;

    private InputData inputData = new InputData();
    public void UpdateSickleData(int attackDamage, float hackSpeed, float hackExpend, ItemQuality itemQuality)
    {
        AttackDamage = attackDamage;
        AttackSpeed = hackSpeed;
        AttackAbrasion = hackExpend;

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

        body.AddItemOnRightHand(gameObject, Vector3.zero, Quaternion.identity, Vector3.one);
        spriteRenderer_Hand.color = body.ShowRightHand(false).color;
        base.HoldingStart(owner, body);
    }
    public override bool PressLeftMouse(float time, ActorAuthority actorAuthority)
    {
        if (inputData.leftPressTimer >= float_NextAttackTiming)
        {
            float_NextAttackTiming += config_AttackCD + 0.1f;
            animator.SetTrigger("Reap");
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
    public void Reap()
    {
        if (actorManager.actorAuthority.isLocal)
        {
            skillIndicators.Shake_SkillIndicators(new Vector3(0.2f, 0.2f, 0), 0.1f);
            skillIndicators.Checkout_SkillIndicators(inputData.mousePosition, AttackDistance, AttackRange, out Collider2D[] colliders);
            List<ActorManager> catchActors = new List<ActorManager>();
            List<BuildingObj> catchBuildings = new List<BuildingObj>();
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].tag.Equals("TileObj"))
                {
                    if (colliders[i].TryGetComponent(out BuildingObj building))
                    {
                        catchBuildings.Add(building);
                    }
                }
                else if (colliders[i].tag.Equals("Actor"))
                {
                    if (colliders[i].isTrigger && colliders[i].transform.TryGetComponent(out ActorManager actor))
                    {
                        catchActors.Add(actor);
                    }
                }
            }
            int damageCount = ReapBuildings(catchBuildings) + ReapActors(catchActors);
            if (damageCount > 0) AddAbrasion(AttackAbrasion);
        }
    }
    private int ReapActors(List<ActorManager> actors)
    {
        int temp = 0;
        actorManager.actionManager.ApplyDamageToActors
            (AttackDamage, DamageState.AttackReapDamage, DamageTarget.WithoutMe, actors, out List<ApplyActorDamageCallBack> callBackList);
        foreach (ApplyActorDamageCallBack callBack in callBackList)
        {
            GameObject effect = PoolManager.Instance.GetEffectObj("Effect/Effect_Impact");
            effect.GetComponent<Effect_Impact>().PlaySlash(callBack.target.transform.position - actorManager.transform.position, true);
            effect.transform.position = callBack.target.transform.position;
            temp += callBack.realDamage;
        }
        return temp;
    }
    private int ReapBuildings(List<BuildingObj> buildings)
    {
        int temp = 0;
        actorManager.actionManager.ApplyDamageToBuilidngs
            (AttackDamage, DamageState.AttackReapDamage, buildings, out List<ApplyBuildingDamageCallBack> callBackList);
        foreach (ApplyBuildingDamageCallBack callBack in callBackList)
        {
            GameObject effect = PoolManager.Instance.GetEffectObj("Effect/Effect_Impact");
            effect.GetComponent<Effect_Impact>().PlaySlash(callBack.target.transform.position - actorManager.transform.position);
            effect.transform.position = callBack.target.transform.position;
            temp += callBack.realDamage;
        }
        return temp;
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
