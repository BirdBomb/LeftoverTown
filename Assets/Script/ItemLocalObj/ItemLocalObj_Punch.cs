using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class ItemLocalObj_Punch : ItemLocalObj
{
    [SerializeField]
    private SkillIndicators skillIndicators;

    private short config_AttackDamage;
    private float config_AttackDistance;
    private float config_AttackRange;
    private float config_AttackSpeed;
    private float config_AttackDuraction = 1;
    private float config_AttackCD;
    private float config_AttackCDRec;
    /// <summary>
    /// 下次拳击时间
    /// </summary>
    private float float_NextPunchTiming = 0;
    private InputData inputData = new InputData();
    public void UpdatePunchData(short damage, float speed, float range, float distance)
    {
        config_AttackDamage = damage;
        config_AttackRange = range;
        config_AttackDistance = distance;
        config_AttackSpeed = speed;

        config_AttackCD = config_AttackDuraction / config_AttackSpeed;
        config_AttackCDRec = config_AttackSpeed / config_AttackDuraction;
    }
    private void FixedUpdate()
    {
        if (inputData.leftPressTimer == 0 && float_NextPunchTiming > 0)
        {
            float_NextPunchTiming -= Time.fixedDeltaTime;
        }
    }
    public override void HoldingStart(ActorManager owner, BodyController_Human body)
    {
        actorManager = owner;

        transform.SetParent(body.transform_Hand);
        body.gameObjects_ItemInHand.Add(gameObject);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;

        config_AttackCD = config_AttackDuraction / config_AttackSpeed;
        config_AttackCDRec = config_AttackSpeed / config_AttackDuraction;
        base.HoldingStart(owner, body);
    }
    public override bool PressLeftMouse(float time, ActorAuthority actorAuthority)
    {
        if (inputData.leftPressTimer >=  float_NextPunchTiming)
        {
            float_NextPunchTiming += config_AttackCD + 0.1f;
            if (new System.Random().Next(0, 2) == 0)
            {
                actorManager.bodyController.SetAnimatorTrigger(BodyPart.Hand, "PunchRight");
                actorManager.bodyController.SetAnimatorFloat(BodyPart.Hand, "PunchSpeed", config_AttackSpeed);
                actorManager.bodyController.SetAnimatorFunc(BodyPart.Hand, Punch);
            }
            else
            {
                actorManager.bodyController.SetAnimatorTrigger(BodyPart.Hand, "PunchLeft");
                actorManager.bodyController.SetAnimatorFloat(BodyPart.Hand, "PunchSpeed", config_AttackSpeed);
                actorManager.bodyController.SetAnimatorFunc(BodyPart.Hand, Punch);
            }
        }
        inputData.leftPressTimer = time;
        return base.PressLeftMouse(time, actorAuthority);
    }
    public override void ReleaseLeftMouse()
    {
        if (inputData.leftPressTimer > 0)
        {
            float_NextPunchTiming -= inputData.leftPressTimer;
            inputData.leftPressTimer = 0;
        }
        base.ReleaseLeftMouse();
    }
    public override void UpdateMousePos(Vector3 mouse)
    {
        inputData.mousePosition = mouse;
        if (actorManager.actorAuthority.isLocal && actorManager.actorAuthority.isPlayer)
        {
            float alpht = (float_NextPunchTiming - inputData.leftPressTimer) * config_AttackCDRec;
            skillIndicators.Draw_SkillIndicators(inputData.mousePosition, config_AttackDistance, config_AttackRange, alpht);
        }
        base.UpdateMousePos(mouse);
    }
    private bool Punch(string str)
    {
        if (actorManager.actorAuthority.isLocal)
        {
            if(str.Equals("PunchRight")|| str.Equals("PunchLeft"))
            {
                skillIndicators.Shake_SkillIndicators(new Vector3(0.2f, 0.2f, 0), 0.1f);
                skillIndicators.Checkout_SkillIndicators(inputData.mousePosition, config_AttackDistance, config_AttackRange, out Collider2D[] colliders);
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].tag.Equals("TileObj"))
                    {
                        if (colliders[i].TryGetComponent(out BuildingObj building))
                        {
                            PunchBuilding(building);
                        }
                    }
                    else if (colliders[i].tag.Equals("Actor") && colliders[i].isTrigger)
                    {
                        if (colliders[i].transform.TryGetComponent(out ActorManager actor))
                        {
                            if (actor != actorManager) { PunchActor(actor); }
                        }
                    }
                }
                return true;
            }
        }
        return false;
    }
    private void PunchActor(ActorManager actor)
    {
        GameObject effect = PoolManager.Instance.GetEffectObj("Effect/Effect_Impact");
        effect.GetComponent<Effect_Impact>().PlayBludgeoning(actor.transform.position - actorManager.transform.position, true);
        effect.transform.position = actor.transform.position;

        actor.actorHpManager.TakeDamage(config_AttackDamage, DamageState.AttackBludgeoningDamage, actorManager.actorNetManager);
    }
    private void PunchBuilding(BuildingObj building)
    {
        int damage = 0;
        damage += building.Local_TakeDamage(config_AttackDamage, DamageState.AttackReapDamage, actorManager.actorNetManager);
        damage += building.Local_TakeDamage(config_AttackDamage, DamageState.AttackSlashingDamage, actorManager.actorNetManager);
        damage += building.Local_TakeDamage(config_AttackDamage, DamageState.AttackBludgeoningDamage, actorManager.actorNetManager);
        if (damage > 0)
        {
            GameObject effect = PoolManager.Instance.GetEffectObj("Effect/Effect_Impact");
            effect.GetComponent<Effect_Impact>().PlayBludgeoning(building.transform.position - actorManager.transform.position);
            effect.transform.position = building.transform.position;
        }
    }
}
