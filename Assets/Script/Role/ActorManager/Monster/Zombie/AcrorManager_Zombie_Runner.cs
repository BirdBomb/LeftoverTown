using Fusion;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class AcrorManager_Zombie_Runner : ActorManager_Monster
{
    [Header("Åü¿³ÉËº¦")]
    public int Hack_DamageVal;
    [Header("Åü¿³·¶Î§")]
    public float Hack_Range;
    [Header("ÌøÅüÉËº¦")]
    public int Jump_DamageVal;
    [Header("ÌøÅü·¶Î§")]
    public float Jump_Range;
    [Header("ÌøÅü×îÐ¡¾àÀë")]
    public float Jump_MinDistance;
    [Header("ÌøÅü×î´ó¾àÀë")]
    public float Jump_MaxDistance;
    public System.Random random = new System.Random();
    #region//¼àÌý
    public override void ForState_Listen_MyselfInjured(int parameter, HpChangeReason reason, NetworkId id)
    {
        NetworkObject networkObject = actorNetManager.Runner.FindObject(id);
        if (networkObject != null && networkObject.TryGetComponent(out ActorManager who) && who.actorAuthority.isPlayer)
        {
            State_InAttack(who);
        }
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
        switch (time)
        {
            case GlobalTime.Evening:
                {
                    State_Think_GoToStroll_Long(2, 5);
                    return;
                }
        }
        if (pathManager.vector3Int_CurPos == brainManager.state_homePostion.position)
        {
            actionManager.Despawn();
        }
        else
        {
            if (!State_Think_GoToHome()) State_Think_GoToStroll_Long(2, 5);
        }
        actionManager.TakeDamage(10, DamageState.RealDamage, null);
    }
    #endregion
    #region//¼¼ÄÜ
    private enum Skill
    {
        Hack,
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
            if (State_CheckingHackDistance())
            {
                pathManager.State_Stop(1f);
                State_RsetAttackTime(float_StateAttackCD);
                actorNetManager.RPC_State_NpcUseSkill((int)Skill.Hack, target.pathManager.vector3Int_CurPos, target.actorNetManager.Object.Id);
                return true;
            }
            if (State_CheckingJumpDistance())
            {
                if (random.Next(0, 10) > 5)
                {
                    pathManager.State_Stop(1f);
                    State_RsetAttackTime(float_StateAttackCD);
                    actorNetManager.RPC_State_NpcUseSkill((int)Skill.Jump, target.pathManager.vector3Int_CurPos, target.actorNetManager.Object.Id);
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
            case Skill.Hack:
                {
                    AllClient_Hack(vector3, networkId);
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
    #region//Åü¿³
    private void AllClient_Hack(Vector3Int vector3, NetworkId networkId)
    {
        actionManager.TurnTo(actorNetManager.Runner.FindObject(networkId).transform.position - transform.position);
        bodyController.SetAnimatorTrigger(BodyPart.Body, "Hack");
        bodyController.SetAnimatorFunc(BodyPart.Body, (str) =>
        {
            if (str.Equals("Hack_0"))
            {
                RaycastHit2D[] raycastHit2Ds = Physics2D.CircleCastAll(transform.position, 1.5f, Vector2.zero);
                foreach (RaycastHit2D hit2D in raycastHit2Ds)
                {
                    if (hit2D.collider.isTrigger && hit2D.collider.gameObject.TryGetComponent(out ActorManager actorManager))
                    {
                        if (actionManager.CheckApplyDamageTarget(actorManager, DamageTarget.WithoutMe))
                        {
                            AllClient_HackActor(actorManager);
                            AllClient_HackEffect(actorManager);
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
        bodyController.SetAnimatorFunc(BodyPart.Body, (str) =>
        {
            if (str.Equals("Hack_1"))
            {
                RaycastHit2D[] raycastHit2Ds = Physics2D.CircleCastAll(transform.position, 1.5f, Vector2.zero);
                foreach (RaycastHit2D hit2D in raycastHit2Ds)
                {
                    if (hit2D.collider.isTrigger && hit2D.collider.gameObject.TryGetComponent(out ActorManager actorManager))
                    {
                        if (actionManager.CheckApplyDamageTarget(actorManager, DamageTarget.WithoutMe))
                        {
                            AllClient_HackActor(actorManager);
                            AllClient_HackEffect(actorManager);
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
    private void AllClient_HackActor(ActorManager actor)
    {
        if (actorAuthority.isLocal)
        {
            actionManager.ApplyDamageToActor(Hack_DamageVal, DamageState.AttackSlashingDamage, DamageTarget.WithoutMe, actor, out _);
        }
    }
    private void AllClient_HackEffect(ActorManager actor)
    {
        GameObject effect = PoolManager.Instance.GetEffectObj("Effect/Effect_Impact");
        effect.GetComponent<Effect_Impact>().PlaySlash(actor.transform.position - transform.position, true);
        effect.transform.position = actor.transform.position;
    }

    public bool State_CheckingHackDistance()
    {
        if (brainManager.ForAll_GetAttackTarget(out ActorManager target))
        {
            return (target.transform.position - transform.position).sqrMagnitude <= Hack_Range * Hack_Range;
        }
        return false;
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
                    RaycastHit2D[] raycastHit2Ds = Physics2D.CircleCastAll(transform.position, Jump_Range, Vector2.zero);
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
}
[Serializable]
public struct ActorConfig_ZombieRunner
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
    [Header("Åü¿³¹¥»÷¾àÀë"), Range(1, 5)]
    public float float_HackDistance;
    [Header("×¼±¸ÌøÔ¾µÄ×îÐ¡¾àÀë"), Range(0, 25)]
    public float float_MinJumpDistance;
    [Header("×¼±¸ÌøÔ¾µÄ×î´ó¾àÀë"), Range(0, 25)]
    public float float_MaxJumpDistance;
    [Header("ÌøÔ¾ÉËº¦")]
    public int int_JumpDamage;
    [Header("Åü¿³ÉËº¦")]
    public int int_HackDamage;
    [Header("ÊÓÒ°¾àÀë"), Range(1, 99)]
    public float float_ViewDistance;
}
