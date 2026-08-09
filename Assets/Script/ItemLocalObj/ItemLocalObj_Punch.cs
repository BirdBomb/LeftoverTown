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

        body.AddItemOnBothHand(gameObject,Vector3.zero,Quaternion.identity,Vector3.zero);

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
                actorManager.bodyController.SetAnimatorTrigger(BodyPart.Hand,"PunchRight");
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
                skillIndicators?.Shake_SkillIndicators(new Vector3(0.2f, 0.2f, 0), 0.1f);
                skillIndicators.Checkout_SkillIndicators(inputData.mousePosition, config_AttackDistance, config_AttackRange, out Collider2D[] colliders);
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
                PunchBuildings(catchBuildings);
                PunchActors(catchActors);
                return true;
            }
        }
        return false;
    }
    private int PunchActors(List<ActorManager> actors)
    {
        int temp = 0;
        actorManager.actionManager.ApplyDamageToActors
            (config_AttackDamage, DamageState.AttackBludgeoningDamage, DamageTarget.WithoutMe, actors, out List<ApplyActorDamageCallBack> callBackList);
        foreach (ApplyActorDamageCallBack callBack in callBackList)
        {
            GameObject effect = PoolManager.Instance.GetEffectObj("Effect/Effect_Impact");
            effect.GetComponent<Effect_Impact>().PlayBludgeoning(callBack.target.transform.position - actorManager.transform.position);
            effect.transform.position = callBack.target.transform.position;
            temp += callBack.realDamage;
        }
        return temp;
    }
    private int PunchBuildings(List<BuildingObj> buildings)
    {
        int temp = 0;
        actorManager.actionManager.ApplyDamageToBuilidngs
            (config_AttackDamage, DamageState.AttackReapDamage, buildings, out List<ApplyBuildingDamageCallBack> callBackList_0);
        actorManager.actionManager.ApplyDamageToBuilidngs
            (config_AttackDamage, DamageState.AttackSlashingDamage, buildings, out List<ApplyBuildingDamageCallBack> callBackList_1);
        actorManager.actionManager.ApplyDamageToBuilidngs
            (config_AttackDamage, DamageState.AttackBludgeoningDamage, buildings, out List<ApplyBuildingDamageCallBack> callBackList_2);
        foreach (ApplyBuildingDamageCallBack callBack in callBackList_0)
        {
            GameObject effect = PoolManager.Instance.GetEffectObj("Effect/Effect_Impact");
            effect.GetComponent<Effect_Impact>().PlayBludgeoning(callBack.target.transform.position - actorManager.transform.position);
            effect.transform.position = callBack.target.transform.position;
            temp += callBack.realDamage;
        }
        foreach (ApplyBuildingDamageCallBack callBack in callBackList_1)
        {
            GameObject effect = PoolManager.Instance.GetEffectObj("Effect/Effect_Impact");
            effect.GetComponent<Effect_Impact>().PlayBludgeoning(callBack.target.transform.position - actorManager.transform.position);
            effect.transform.position = callBack.target.transform.position;
            temp += callBack.realDamage;
        }
        foreach (ApplyBuildingDamageCallBack callBack in callBackList_2)
        {
            GameObject effect = PoolManager.Instance.GetEffectObj("Effect/Effect_Impact");
            effect.GetComponent<Effect_Impact>().PlayBludgeoning(callBack.target.transform.position - actorManager.transform.position);
            effect.transform.position = callBack.target.transform.position;
            temp += callBack.realDamage;
        }
        return temp;
    }

}
