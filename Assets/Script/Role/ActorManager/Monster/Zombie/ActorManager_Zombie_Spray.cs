using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class ActorManager_Zombie_Spray : ActorManager_Monster
{
    [Header("½©Ê¬Ç¹¿Ú")]
    public Transform trans_Muzzle;
    [Header("×²»÷ÉËº¦")]
    public int Bump_DamageVal;
    [Header("×²»÷·¶Î§")]
    public float Bump_Range;
    [Header("ËáÒºÉËº¦")]
    public int Spray_DamageVal;
    [Header("ËáÒº×î´ó¾àÀë")]
    public float Spray_MaxDistance;
    public System.Random random = new System.Random();
    #region//¼àÌý
    public override void ForState_Listen_MyselfInjured(int parameter, HpChangeReason reason, NetworkId id)
    {
        NetworkObject networkObject = actorNetManager.Runner.FindObject(id);
        if (networkObject != null && networkObject.TryGetComponent(out ActorManager who) && who.actorAuthority.isPlayer)
        {
            State_InAttack(who);
        }
        base.ForState_Listen_MyselfInjured(parameter, reason, id);
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
        /// <summary>
        /// ×²»÷
        /// </summary>
        Bump,
        /// <summary>
        /// ÅçÉä
        /// </summary>
        Spray
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
            if (State_CheckingBumpDistance())
            {
                pathManager.State_Stop(1f);
                State_RsetAttackTime(float_StateAttackCD);
                actorNetManager.RPC_State_NpcUseSkill((int)Skill.Bump, target.pathManager.vector3Int_CurPos, target.actorNetManager.Object.Id);
                return true;
            }
            if (State_CheckingSprayDistance())
            {
                if (random.Next(0, 10) > 5)
                {
                    pathManager.State_Stop(1f);
                    State_RsetAttackTime(float_StateAttackCD);
                    actorNetManager.RPC_State_NpcUseSkill((int)Skill.Spray, target.pathManager.vector3Int_CurPos, target.actorNetManager.Object.Id);
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
            case Skill.Bump:
                {
                    AllClient_Bump(vector3, networkId);
                    break;
                }
            case Skill.Spray:
                {
                    AllClient_Spray(vector3, networkId);
                    break;
                }
        }
        base.ForAll_Listen_NpcAction(id, vector3, networkId);
    }

    #endregion
    #region//×²»÷
    private void AllClient_Bump(Vector3Int vector3, NetworkId networkId)
    {
        actionManager.TurnTo(actorNetManager.Runner.FindObject(networkId).transform.position - transform.position);
        bodyController.SetAnimatorTrigger(BodyPart.Body, "Bump");
        bodyController.SetAnimatorFunc(BodyPart.Body, (str) =>
        {
            if (str.Equals("Bump"))
            {
                RaycastHit2D[] raycastHit2Ds = Physics2D.CircleCastAll(transform.position, Bump_Range, Vector2.zero);
                foreach (RaycastHit2D hit2D in raycastHit2Ds)
                {
                    if (hit2D.collider.isTrigger && hit2D.collider.gameObject.TryGetComponent(out ActorManager actorManager))
                    {
                        if (actionManager.CheckApplyDamageTarget(actorManager, DamageTarget.WithoutMe))
                        {
                            AllClient_BumpActor(actorManager);
                            AllClient_BumpEffect(actorManager.transform.position);
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
    private void AllClient_BumpEffect(Vector2 pos)
    {
        GameObject effect = PoolManager.Instance.GetEffectObj("Effect/Effect_Impact");
        effect.GetComponent<Effect_Impact>().PlayBludgeoning((Vector2)transform.position - pos, false);
        effect.transform.position = pos;
    }
    private void AllClient_BumpActor(ActorManager actor)
    {
        if (actorAuthority.isLocal)
        {
            actionManager.ApplyDamageToActor(Bump_DamageVal, DamageState.AttackBludgeoningDamage, DamageTarget.WithoutMe, actor, out _);
        }
    }
    public bool State_CheckingBumpDistance()
    {
        if (brainManager.ForAll_GetAttackTarget(out ActorManager target))
        {
            return (target.transform.position - transform.position).sqrMagnitude <= Bump_Range * Bump_Range;
        }
        return false;
    }
    #endregion
    #region//ÅçÉä
    private void AllClient_Spray(Vector3Int vector3, NetworkId networkId)
    {
        if (actorNetManager.Runner.FindObject(networkId) != null)
        {
            Vector2 dir = actorNetManager.Runner.FindObject(networkId).transform.position - transform.position;
            actionManager.TurnTo(dir);
            bodyController.SetAnimatorTrigger(BodyPart.Body, "Spray");
            bodyController.SetAnimatorFunc(BodyPart.Body, (str) =>
            {
                if (str.Equals("Spray"))
                {
                    GameObject obj = PoolManager.Instance.GetObject("Bullet/Bullet_100");
                    if (obj.TryGetComponent(out BulletBase bulletBase))
                    {
                        bulletBase.InitBullet();
                        bulletBase.SetPhysics(trans_Muzzle.transform.position, dir, 0, 0);
                        bulletBase.SetDamage(0, Spray_DamageVal);
                        bulletBase.SetOwner(this);
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
    public bool State_CheckingSprayDistance()
    {
        if (brainManager.ForAll_GetAttackTarget(out ActorManager target))
        {
            return (target.transform.position - transform.position).sqrMagnitude <= Spray_MaxDistance * Spray_MaxDistance;
        }
        return false;
    }

    #endregion
}
[Serializable]
public struct ActorConfig_ZombieSpray
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
    [Header("×²»÷¹¥»÷¾àÀë"), Range(1, 5)]
    public float float_BumpDistance;
    [Header("×²»÷ÉËº¦")]
    public int int_BumpDamage;
    [Header("ÅçÉä¹¥»÷¾àÀë"), Range(1, 25)]
    public float float_SprayDistance;
    [Header("ÊÓÒ°¾àÀë"), Range(1, 99)]
    public float float_ViewDistance;
}
