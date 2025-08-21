using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class ItemLocalObj_Punch : ItemLocalObj
{
    [SerializeField]
    private SkillIndicators skillIndicators;

    [SerializeField, Header("È­»÷ÉËº¦")]
    private short config_PunchDamage;
    [SerializeField, Header("È­»÷¾àÀë")]
    private float config_PunchDistance;
    [SerializeField, Header("È­»÷·¶Î§")]
    private float config_PunchRange;
    [SerializeField, Header("È­»÷ËÙ¶È")]
    private float config_PunchSpeed = 1;
    /// <summary>
    /// È­»÷¶¯»­Ê±³¤
    /// </summary>
    private float config_PunchDuraction = 1;
    /// <summary>
    /// È­»÷CD
    /// </summary>
    private float config_PunchCD;
    /// <summary>
    /// È­»÷CDµ¹Êý
    /// </summary>
    private float config_PunchCDRec;
    /// <summary>
    /// ÏÂ´ÎÈ­»÷Ê±¼ä
    /// </summary>
    private float float_NextPunchTiming = 0;
    private InputData inputData = new InputData();
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

        config_PunchCD = config_PunchDuraction / config_PunchSpeed;
        config_PunchCDRec = config_PunchSpeed / config_PunchDuraction;
        base.HoldingStart(owner, body);
    }
    public override bool PressLeftMouse(float time, ActorAuthority actorAuthority)
    {
        if (inputData.leftPressTimer >=  float_NextPunchTiming)
        {
            float_NextPunchTiming += config_PunchCD + 0.1f;
            if (new System.Random().Next(0, 2) == 0)
            {
                actorManager.bodyController.SetAnimatorTrigger(BodyPart.Hand, "PunchRight");
                actorManager.bodyController.SetAnimatorFloat(BodyPart.Hand, "PunchSpeed", config_PunchSpeed);
                actorManager.bodyController.SetAnimatorFunc(BodyPart.Hand, Punch);
            }
            else
            {
                actorManager.bodyController.SetAnimatorTrigger(BodyPart.Hand, "PunchLeft");
                actorManager.bodyController.SetAnimatorFloat(BodyPart.Hand, "PunchSpeed", config_PunchSpeed);
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
            float alpht = (float_NextPunchTiming - inputData.leftPressTimer) * config_PunchCDRec;
            skillIndicators.Draw_SkillIndicators(inputData.mousePosition, config_PunchDistance, config_PunchRange, alpht);
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
                skillIndicators.Checkout_SkillIndicators(inputData.mousePosition, config_PunchDistance, config_PunchRange, out Collider2D[] colliders);
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].tag.Equals("BuildingNatural"))
                    {
                        if (colliders[i].TryGetComponent(out BuildingObj building))
                        {
                            building.Local_TakeDamage(config_PunchDamage);
                        }
                    }
                    else if (colliders[i].tag.Equals("Actor"))
                    {
                        if (colliders[i].isTrigger && colliders[i].transform.TryGetComponent(out ActorManager actor))
                        {
                            if (actor == actorManager) { continue; }
                            else
                            {
                                actor.AllClient_Listen_TakeAttackDamage(config_PunchDamage, actorManager.actorNetManager);
                            }
                        }
                    }
                }
                return true;
            }
        }
        return false;
    }
}
