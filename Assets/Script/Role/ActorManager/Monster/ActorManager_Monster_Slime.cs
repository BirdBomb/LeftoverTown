using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ActorManager_Monster_Slime : ActorManager_Animal
{
    [Header("¹¥»÷ÉËº¦")]
    public int Attack_DamageVal;
    [Header("¹¥»÷·¶Î§")]
    public float Attack_Range;
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
        if (time == GlobalTime.Evening)
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
        if (time == GlobalTime.Evening)
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
        Attack
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
        if (State_CheckingBiteDistance())
        {
            pathManager.State_SetFrezzeTime(1);
            State_RsetAttackTime(float_StateAttackCD);
            actorNetManager.RPC_State_NpcUseSkill((int)Skill.Attack, pathManager.vector3Int_CurPos, target.actorNetManager.Object.Id);
            return true;
        }
        else
        {
            return false;
        }
    }
    public override void AllClient_Listen_NpcAction(int id, Vector3Int vector3, NetworkId networkId)
    {
        if (id == (int)Skill.Attack)
        {
            AllClient_Bite(vector3, networkId);
        }
        base.AllClient_Listen_NpcAction(id, vector3, networkId);
    }
    private void AllClient_Bite(Vector3Int vector3, NetworkId networkId)
    {
        actionManager.TurnTo(((Vector3)(vector3 - pathManager.vector3Int_CurPos)));
        bodyController.SetAnimatorTrigger(BodyPart.Body, "Attack");
        bodyController.SetAnimatorFunc(BodyPart.Body, (str) =>
        {
            if (str.Equals("Attack"))
            {
                RaycastHit2D[] raycastHit2Ds = Physics2D.CircleCastAll(transform.position, Attack_Range, Vector2.zero);
                foreach (RaycastHit2D hit2D in raycastHit2Ds)
                {
                    if (hit2D.collider.isTrigger && hit2D.collider.gameObject.TryGetComponent(out ActorManager actorManager))
                    {
                        if (actorManager.statusManager.statusType != StatusType.Monster_Common)
                        {
                            if (actorManager.actorAuthority.isLocal)
                            {
                                Local_AttackAtor(actorManager);
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
    private void Local_AttackAtor(ActorManager actor)
    {
        GameObject effect = PoolManager.Instance.GetEffectObj("Effect/Effect_Impact");
        effect.GetComponent<Effect_Impact>().PlayBludgeoning(actor.transform.position - transform.position, false);
        effect.transform.position = actor.transform.position;

        actor.actorHpManager.TakeDamage(Attack_DamageVal, DamageState.AttackBludgeoningDamage, actorNetManager);
    }

    public bool State_CheckingBiteDistance()
    {
        if (Vector3.Distance(brainManager.allClient_actorManager_AttackTarget.transform.position, transform.position) <= Attack_Range)
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
