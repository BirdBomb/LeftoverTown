using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class ActorManager_Zombie_Hook : ActorManager_Monster
{
    [Header("È­»÷ÉËº¦")]
    public int Punch_DamageVal;
    [Header("È­»÷·¶Î§")]
    public float Punch_Range;
    [Header("Ìú¹³ÉËº¦")]
    public int Hook_DamageVal;
    [Header("ÌøÔ¾ÉËº¦")]
    public int Jump_DamageVal;
    [Header("ÌøÔ¾·¶Î§")]
    public float Jump_DamageDistance;
    [Header("ÌøÅü×îÐ¡¾àÀë")]
    public float Jump_MinDistance;
    [Header("ÌøÅü×î´ó¾àÀë")]
    public float Jump_MaxDistance;


    [SerializeField, Header("Ìú¹³")]
    private Bullet_Hook bullet_Hook;
    private System.Random random = new System.Random();
    #region//¼àÌý
    public override void ForState_Listen_MyselfInjured(int parameter, HpChangeReason reason, NetworkId id)
    {
        NetworkObject networkObject = actorNetManager.Runner.FindObject(id);
        if (networkObject != null && networkObject.TryGetComponent(out ActorManager who) && who.actorAuthority.isPlayer)
        {
            State_InAttack(who);
        }
    }
    public override void AllClient_UpdateHpBar(float val)
    {
        UI_BossInfo.Instance.Show(actorNetManager.Net_HpCur, actorNetManager.Local_HpMax, "Ìú¹³½©Ê¬",actorNetManager.Object.Id);
        base.AllClient_UpdateHpBar(val);
    }
    #endregion
    #region//¼ì²é
    public override bool State_CheckNearbyActor()
    {
        if (brainManager.globalTime_Now != GlobalTime.Evening) { return true; }
        List<ActorManager> temp = brainManager.State_GetNearbyActors();
        foreach (ActorManager actor in temp)
        {
            if (actionManager.LookAt(actor, State_CalculateView()))
            {
                if (actor.statusManager.statusType != StatusType.Animal_Common && actor.statusManager.statusType != StatusType.Monster_Common)
                {
                    State_InAttack(actor);
                    return true;
                }
            }
        }
        return false;
    }
    #endregion
    #region//Ë¼¿¼
    public override void State_ThinkByTimeUpdate(int date, int hour, GlobalTime time)
    {
        if (brainManager.state_homePostion.isValue && MapManager.Instance.GetBuilding(brainManager.state_homePostion.position, out _))
        {
            State_Think_GoToStroll_Long(2, 5);
        }
        else
        {
            actionManager.Despawn();
        }
    }
    #endregion
    #region//¼¼ÄÜ
    private enum Skill
    {
        ShotHook,
        Punch,
        Jump
    }
    public override void State_AttackLoop()
    {
        if (brainManager.ForAll_GetAttackTarget(out ActorManager target))
        {
            float realDistance = (target.transform.position - transform.position).sqrMagnitude;
            if (realDistance > 1)
            {
                State_Follow(target.pathManager.vector3Int_CurPos);
            }
        }
        base.State_AttackLoop();
    }
    public override bool State_Attack()
    {
        if (brainManager.ForAll_GetAttackTarget(out ActorManager target))
        {
            if (State_CheckingPunchDistance())
            {
                pathManager.State_Stop(2f);
                State_RsetAttackTime(float_StateAttackCD);
                actorNetManager.RPC_State_NpcUseSkill((int)Skill.Punch, target.pathManager.vector3Int_CurPos, target.actorNetManager.Object.Id);
                return true;
            }
            if (State_CheckingJumpDistance())
            {
                if (random.Next(0, 10) > 5)
                {
                    pathManager.State_Stop(4f);
                    State_RsetAttackTime(float_StateAttackCD);
                    actorNetManager.RPC_State_NpcUseSkill((int)Skill.Jump, target.pathManager.vector3Int_CurPos, target.actorNetManager.Object.Id);
                }
                else
                {
                    pathManager.State_Stop(2f);
                    State_RsetAttackTime(float_StateAttackCD);
                    actorNetManager.RPC_State_NpcUseSkill((int)Skill.ShotHook, target.pathManager.vector3Int_CurPos, target.actorNetManager.Object.Id);
                }
                return true;
            }
        }
        return false;
    }
    public override void ForAll_Listen_NpcAction(int id, Vector3Int vector3, NetworkId networkId)
    {
        switch ((Skill)id)
        {
            case Skill.ShotHook:
                {
                    AllClient_ShotHook(vector3, networkId);
                    break;
                }
            case Skill.Punch:
                {
                    AllClient_Punch(vector3, networkId);
                    break;
                }
            case Skill.Jump:
                {
                    AllClient_Jump(vector3, networkId);
                    break;
                }
        }
        base.ForAll_Listen_NpcAction(id, vector3, networkId);
    }
    #endregion
    #region//·¢ÉäÌú¹³
    private void AllClient_ShotHook(Vector3Int vector3, NetworkId networkId)
    {
        actionManager.TurnTo(actorNetManager.Runner.FindObject(networkId).transform.position - transform.position);
        bodyController.SetAnimatorTrigger(BodyPart.Body, "ShotHook");
        bodyController.SetAnimatorFunc(BodyPart.Body, (str) =>
        {
            if (str.Equals("ShotHook"))
            {
                Vector3 dir = (actorNetManager.Runner.FindObject(networkId).transform.position - bullet_Hook.transform.position).normalized;
                bullet_Hook.InitBullet();
                bullet_Hook.SetPhysics(bullet_Hook.transform.position, dir, 0, 0);
                bullet_Hook.SetDamage(Hook_DamageVal, 0);
                bullet_Hook.SetOwner(this);
                return true;
            }
            else
            {
                return false;
            }
        });
    }
    #endregion
    #region//ÌøÔ¾
    private void AllClient_Jump(Vector3Int vector3, NetworkId networkId)
    {
        if (actorNetManager.Runner.FindObject(networkId) != null)
        {
            Vector2 dir = actorNetManager.Runner.FindObject(networkId).transform.position - transform.position;
            actionManager.TurnTo(dir);
            bodyController.SetAnimatorTrigger(BodyPart.Body, "Jump");
            bodyController.SetAnimatorFunc(BodyPart.Body, (str) =>
            {
                if (str.Equals("StartJump"))
                {
                    actionManager.Client_TakeForce(dir.normalized, 40);
                    return true;
                }
                else
                {
                    return false;
                }
            });
            bodyController.SetAnimatorFunc(BodyPart.Body, (str) =>
            {
                if (str.Equals("OverJump"))
                {
                    GameObject effect = PoolManager.Instance.GetEffectObj("Effect/Effect_ThunderWaves");
                    effect.GetComponent<EffectBase>().SetEffect(Vector3.zero);
                    effect.transform.position = transform.position;

                    RaycastHit2D[] raycastHit2Ds = Physics2D.CircleCastAll(transform.position, Jump_DamageDistance, Vector2.zero);
                    foreach (RaycastHit2D hit2D in raycastHit2Ds)
                    {
                        if (hit2D.collider.isTrigger && hit2D.collider.gameObject.TryGetComponent(out ActorManager actorManager))
                        {
                            if (actionManager.CheckApplyDamageTarget(actorManager, DamageTarget.WithoutMe))
                            {
                                AllClient_JumpActor(actorManager);
                                AllClient_JumpEffect(actorManager);
                            }
                        }
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            });
        }
    }

    private void AllClient_JumpActor(ActorManager actor)
    {
        if (actorAuthority.isLocal)
        {
            actionManager.ApplyDamageToActor(Jump_DamageVal, DamageState.AttackSlashingDamage, DamageTarget.WithoutMe, actor, out _);
        }
    }
    private void AllClient_JumpEffect(ActorManager actor)
    {
        GameObject effect = PoolManager.Instance.GetEffectObj("Effect/Effect_Impact");
        effect.GetComponent<Effect_Impact>().PlayBludgeoning(actor.transform.position - transform.position, true);
        effect.transform.position = actor.transform.position;
    }
    public bool State_CheckingJumpDistance()
    {
        if (brainManager.ForAll_GetAttackTarget(out ActorManager target))
        {
            if ((target.transform.position - transform.position).sqrMagnitude < Jump_MaxDistance * Jump_MaxDistance &&
                (target.transform.position - transform.position).sqrMagnitude > Jump_MinDistance * Jump_MinDistance)
            {
                return true;
            }
        }
        return false;
    }

    #endregion
    #region//È­»÷
    private void AllClient_Punch(Vector3Int vector3, NetworkId networkId)
    {
        actionManager.TurnTo(actorNetManager.Runner.FindObject(networkId).transform.position - transform.position);
        bodyController.SetAnimatorTrigger(BodyPart.Body, "Punch");
        bodyController.SetAnimatorFunc(BodyPart.Body, (str) =>
        {
            if (str.Equals("Punch"))
            {
                RaycastHit2D[] raycastHit2Ds = Physics2D.CircleCastAll(transform.position, Punch_Range, Vector2.zero);
                foreach (RaycastHit2D hit2D in raycastHit2Ds)
                {
                    if (hit2D.collider.isTrigger && hit2D.collider.gameObject.TryGetComponent(out ActorManager actorManager))
                    {
                        if (actionManager.CheckApplyDamageTarget(actorManager, DamageTarget.WithoutMe))
                        {
                            AllClient_PunchActor(actorManager);
                            AllClient_PunchEffect(actorManager);
                        }
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        });
    }
    private void AllClient_PunchActor(ActorManager actor)
    {
        if (actorAuthority.isLocal)
        {
            actor.actionManager.Client_TakeForce((actor.transform.position - transform.position).normalized, 25);
            actionManager.ApplyDamageToActor(Punch_DamageVal, DamageState.AttackBludgeoningDamage, DamageTarget.WithoutMe, actor, out _);
        }
    }
    private void AllClient_PunchEffect(ActorManager actor)
    {
        GameObject effect = PoolManager.Instance.GetEffectObj("Effect/Effect_Impact");
        effect.GetComponent<Effect_Impact>().PlayBludgeoning(actor.transform.position - transform.position, true);
        effect.transform.position = actor.transform.position;
    }

    public bool State_CheckingPunchDistance()
    {
        if (brainManager.ForAll_GetAttackTarget(out ActorManager target))
        {
            return (target.transform.position - transform.position).sqrMagnitude <= Punch_Range * Punch_Range;
        }
        return false;
    }
    #endregion
}
[Serializable]
public struct ActorConfig_ZombieHook
{
    [Header("ÉúÃü")]
    public short short_Hp;
    [Header("»¤¼×")]
    public short short_Armor;
    [Header("Ä§¿¹")]
    public short short_Resistance;
    [Header("ÒÆ¶¯ËÙ¶È")]
    public short short_MoveSpeed;
    [Header("ÒÆ¶¯¾àÀë"), Range(1, 10)]
    public short short_MoveStep;
    [Header("Ìú¹³¹¥»÷¾àÀë"), Range(1, 50)]
    public float float_HookDistance;
    [Header("Ìú¹³ÉËº¦")]
    public int int_HookDamage;
    [Header("È­»÷¹¥»÷¾àÀë"), Range(1, 50)]
    public float float_PunchDistance;
    [Header("È­»÷ÉËº¦")]
    public int int_PunchDamage;
    [Header("ÌøÔ¾¹¥»÷ÉËº¦")]
    public int int_JumpDamage;
    [Header("ÊÓÒ°¾àÀë"), Range(1, 99)]
    public float float_ViewDistance;
}
