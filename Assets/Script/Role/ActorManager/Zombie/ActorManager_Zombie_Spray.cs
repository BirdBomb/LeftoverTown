using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class ActorManager_Zombie_Spray : ActorManager_Animal
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
    #region//¼àÌý
    public override void State_Listen_MyselfHpChange(int parameter, HpChangeReason reason, NetworkId id)
    {
        NetworkObject networkObject = actorNetManager.Runner.FindObject(id);
        if (networkObject != null && parameter < 0)
        {
            ActorManager who = networkObject.GetComponent<ActorManager>();
            if (who.actorAuthority.isPlayer)
            {
                State_InAttack(who);
            }
        }
    }
    #endregion
    #region//¼ì²é
    public override bool State_CheckNearbyActor()
    {
        if (brainManager.globalTime_Now != GlobalTime.Evening) { return true; }
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
    public override void State_ThinkByTimeChange(int date, int hour, GlobalTime time)
    {
        if (time != GlobalTime.Evening)
        {
            State_OutAttack();
            State_OutThreatened();
            State_Think_GoToHome();
        }
        base.State_ThinkByTimeUpdate(date, hour, time);
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
        if (State_CheckingBumpDistance())
        {
            pathManager.State_SetFrezzeTime(1f);
            State_RsetAttackTime(float_StateAttackCD);
            actorNetManager.RPC_State_NpcUseSkill((int)Skill.Bump, brainManager.allClient_actorManager_AttackTarget.pathManager.vector3Int_CurPos, brainManager.allClient_actorManager_AttackTarget.actorNetManager.Object.Id);
            return true;
        }
        if (State_CheckingSprayDistance())
        {
            if (new System.Random().Next(0, 10) > 5)
            {
                pathManager.State_SetFrezzeTime(1f);
                State_RsetAttackTime(float_StateAttackCD);
                actorNetManager.RPC_State_NpcUseSkill((int)Skill.Spray, brainManager.allClient_actorManager_AttackTarget.pathManager.vector3Int_CurPos, brainManager.allClient_actorManager_AttackTarget.actorNetManager.Object.Id);
            }
            return true;
        }
        return false;
    }

    public override void AllClient_Listen_NpcAction(int id, Vector3Int vector3, NetworkId networkId)
    {
        if (id == (int)Skill.Bump)
        {
            AllClient_Bump(vector3, networkId);
        }
        if (id == (int)Skill.Spray)
        {
            AllClient_Spray(vector3, networkId);
        }
        base.AllClient_Listen_NpcAction(id, vector3, networkId);
    }
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
                        if (actorManager.statusManager.statusType != StatusType.Monster_Common)
                        {
                            if (actorManager.actorAuthority.isLocal)
                            {
                                Local_BumpActor(actorManager);
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
    private void Local_BumpActor(ActorManager actor)
    {
        GameObject effect = PoolManager.Instance.GetEffectObj("Effect/Effect_Impact");
        effect.GetComponent<Effect_Impact>().PlayBludgeoning(actor.transform.position - transform.position);
        effect.transform.position = actor.transform.position;
        actor.actorHpManager.TakeDamage(Bump_DamageVal, DamageState.AttackBludgeoningDamage, actorNetManager);
        actor.actionManager.Client_TakeForce((actor.transform.position - transform.position).normalized, 25);
    }
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
    /// <summary>
    /// ¼ì²é¹¥»÷¾àÀë
    /// </summary>
    /// <returns></returns>
    public bool State_CheckingBumpDistance()
    {
        if (Vector3.Distance(brainManager.allClient_actorManager_AttackTarget.transform.position, transform.position) < Bump_DamageVal)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// ¼ì²é¼¼ÄÜ¾àÀë
    /// </summary>
    /// <returns></returns>
    public bool State_CheckingSprayDistance()
    {
        float distance = Vector3.Distance(brainManager.allClient_actorManager_AttackTarget.transform.position, transform.position);
        if (distance < Spray_MaxDistance)
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
    [Header("»ù±¾µôÂäÁÐ±í")]
    public List<BaseLootInfo> lootInfos_Base;
    [Header("¶îÍâµôÂäÁÐ±í")]
    public List<ExtraLootInfo> lootInfos_Extra;
}
