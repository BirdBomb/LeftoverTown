using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class ActorManager_Zombie_Hook : ActorManager_Animal
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
    #region//¼àÌý
    public override void AllClient_UpdateHpBar(float val)
    {
        UI_BossInfo.Instance.Show(actorNetManager.Net_HpCur,val,"Ìú¹³½©Ê¬");
        base.AllClient_UpdateHpBar(val);
    }
    #endregion
    #region//¼ì²é
    public override bool State_CheckNearbyActor()
    {
        for (int i = 0; i < brainManager.actorManagers_Nearby.Count; i++)
        {
            if (actionManager.LookAt(brainManager.actorManagers_Nearby[i], State_CalculateView()))
            {
                if (brainManager.actorManagers_Nearby[i].statusManager.statusType != StatusType.Animal_Common &&
                    brainManager.actorManagers_Nearby[i].statusManager.statusType != StatusType.Monster_Common)
                {
                    State_InAttack(brainManager.actorManagers_Nearby[i]);
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
        if (time != GlobalTime.Evening)
        {
            if (pathManager.vector3Int_CurPos == brainManager.state_homePostion.position)
            {
                actionManager.Despawn();
                return;
            }
            else
            {
                State_Think_GoToHome();
                return;
            }
        }
        State_Think_GoToStroll_Long(2, 5);
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
        ActorManager target = brainManager.allClient_actorManager_AttackTarget;
        float realDistance = Vector3.Distance(target.transform.position, transform.position);
        if (realDistance > 1)
        {
            State_Follow(brainManager.allClient_actorManager_AttackTarget.pathManager.vector3Int_CurPos);
        }
        base.State_AttackLoop();
    }
    public override bool State_Attack()
    {
        ActorManager target = brainManager.allClient_actorManager_AttackTarget;
        float realDistance = Vector3.Distance(target.transform.position, transform.position);
        if (State_CheckingPunchDistance())
        {
            pathManager.State_SetFrezzeTime(2f);
            State_RsetAttackTime(float_StateAttackCD);
            actorNetManager.RPC_State_NpcUseSkill((int)Skill.Punch, brainManager.allClient_actorManager_AttackTarget.pathManager.vector3Int_CurPos, brainManager.allClient_actorManager_AttackTarget.actorNetManager.Object.Id);
            return true;
        }
        if (State_CheckingJumpDistance())
        {
            if (new System.Random().Next(0, 10) > 5)
            {
                pathManager.State_SetFrezzeTime(4f);
                State_RsetAttackTime(float_StateAttackCD);
                actorNetManager.RPC_State_NpcUseSkill((int)Skill.Jump, brainManager.allClient_actorManager_AttackTarget.pathManager.vector3Int_CurPos, brainManager.allClient_actorManager_AttackTarget.actorNetManager.Object.Id);
            }
            else
            {
                pathManager.State_SetFrezzeTime(2f);
                State_RsetAttackTime(float_StateAttackCD);
                actorNetManager.RPC_State_NpcUseSkill((int)Skill.ShotHook, brainManager.allClient_actorManager_AttackTarget.pathManager.vector3Int_CurPos, brainManager.allClient_actorManager_AttackTarget.actorNetManager.Object.Id);
            }
            return true;
        }
        return false;
    }

    public override void AllClient_Listen_NpcAction(int id, Vector3Int vector3, NetworkId networkId)
    {
        if (id == (int)Skill.ShotHook)
        {
            AllClient_ShotHook(vector3, networkId);
        }
        if (id == (int)Skill.Punch)
        {
            AllClient_Punch(vector3, networkId);
        }
        if (id == (int)Skill.Jump)
        {
            AllClient_Jump(vector3, networkId);
        }
        base.AllClient_Listen_NpcAction(id, vector3, networkId);
    }
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
                        if (actorManager.statusManager.statusType != StatusType.Monster_Common)
                        {
                            if (actorManager.actorAuthority.isLocal)
                            {
                                Local_PunchActor(actorManager);
                            }
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
    private void Local_PunchActor(ActorManager actor)
    {
        GameObject effect = PoolManager.Instance.GetEffectObj("Effect/Effect_Impact");
        effect.GetComponent<Effect_Impact>().PlayBludgeoning(actor.transform.position - transform.position);
        effect.transform.position = actor.transform.position;
        actor.actorHpManager.TakeDamage(Punch_DamageVal, DamageState.AttackBludgeoningDamage, actorNetManager);
        actor.actionManager.Client_TakeForce((actor.transform.position - transform.position).normalized, 25);
    }

    /// <summary>
    /// ¼ì²é¹¥»÷¾àÀë
    /// </summary>
    /// <returns></returns>
    public bool State_CheckingPunchDistance()
    {
        if (Vector3.Distance(brainManager.allClient_actorManager_AttackTarget.transform.position, transform.position) < Punch_Range)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
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
                            if (actorManager.statusManager.statusType != StatusType.Monster_Common)
                            {
                                if (actorManager.actorAuthority.isLocal)
                                {
                                    Local_JumpActor(actorManager); 
                                }
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
    private void Local_JumpActor(ActorManager actor)
    {
        actor.actorHpManager.TakeDamage(Jump_DamageVal, DamageState.MagicDamage, actorNetManager);
    }
    /// <summary>
    /// ¼ì²é¼¼ÄÜ¾àÀë
    /// </summary>
    /// <returns></returns>
    public bool State_CheckingJumpDistance()
    {
        float distance = Vector3.Distance(brainManager.allClient_actorManager_AttackTarget.transform.position, transform.position);
        if (distance < Jump_MaxDistance && distance > Jump_MinDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
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
    [Header("»ù±¾µôÂäÁÐ±í")]
    public List<BaseLootInfo> lootInfos_Base;
    [Header("¶îÍâµôÂäÁÐ±í")]
    public List<ExtraLootInfo> lootInfos_Extra;
}
