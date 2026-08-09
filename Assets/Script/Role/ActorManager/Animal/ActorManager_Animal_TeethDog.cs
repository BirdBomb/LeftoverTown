using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class ActorManager_Animal_TeethDog : ActorManager_Animal
{
    [Header("¿ÐÒ§ÉËº¦")]
    public int Bite_DamageVal;
    [Header("¿ÐÒ§·¶Î§")]
    public float Bite_Range;
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
        if (brainManager.globalTime_Now == GlobalTime.Evening) { return true; }
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
                    if (pathManager.vector3Int_CurPos == brainManager.state_homePostion.position)
                    {
                        actionManager.Despawn();
                    }
                    else
                    {
                        if (!State_Think_GoToHome()) State_Think_GoToStroll_Long(4, 5);
                    }
                    return;
                }
        }
        State_Think_GoToStroll_Long(4, 5);

    }
    #endregion
    #region//¼¼ÄÜ
    private enum Skill
    {
        Bite,Woof
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
        if (brainManager.ForAll_GetAttackTarget(out ActorManager target) && State_CheckingBiteDistance())
        {
            pathManager.State_Stop(1);
            State_RsetAttackTime(float_StateAttackCD);
            actorNetManager.RPC_State_NpcUseSkill((int)Skill.Bite, pathManager.vector3Int_CurPos, target.actorNetManager.Object.Id);
            return true;
        }
        return false;
    }
    public override void ForAll_Listen_NpcAction(int id, Vector3Int vector3, NetworkId networkId)
    {
        if (id == (int)Skill.Bite)
        {
            AllClient_Bite(vector3, networkId);
        }
        base.ForAll_Listen_NpcAction(id, vector3, networkId);
    }
    #endregion
    #region//ËºÒ§
    private void AllClient_Bite(Vector3Int vector3, NetworkId networkId)
    {
        actionManager.TurnTo(((Vector3)(vector3 - pathManager.vector3Int_CurPos)));
        bodyController.SetAnimatorTrigger(BodyPart.Body, "Bite");
        bodyController.SetAnimatorFunc(BodyPart.Body, (str) =>
        {
            if (str.Equals("Bite"))
            {
                RaycastHit2D[] raycastHit2Ds = Physics2D.CircleCastAll(transform.position, Bite_Range, Vector2.zero);
                foreach (RaycastHit2D hit2D in raycastHit2Ds)
                {
                    if (hit2D.collider.isTrigger && hit2D.collider.gameObject.TryGetComponent(out ActorManager actorManager))
                    {
                        if (actionManager.CheckApplyDamageTarget(actorManager, DamageTarget.WithoutMe))
                        {
                            AllClient_BiteActor(actorManager);
                            AllClient_BiteEffect(actorManager.transform.position);
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
    private void AllClient_BiteEffect(Vector2 pos)
    {
        GameObject effect = PoolManager.Instance.GetEffectObj("Effect/Effect_Impact");
        effect.GetComponent<Effect_Impact>().PlaySlash((Vector2)transform.position - pos, false);
        effect.transform.position = pos;
    }
    private void AllClient_BiteActor(ActorManager actor)
    {
        if (actorAuthority.isLocal)
        {
            actionManager.ApplyDamageToActor(Bite_DamageVal, DamageState.AttackSlashingDamage, DamageTarget.WithoutMe, actor, out _);
        }
    }
    public bool State_CheckingBiteDistance()
    {
        if (brainManager.ForAll_GetAttackTarget(out ActorManager target))
        {
            return (target.transform.position - transform.position).sqrMagnitude <= Bite_Range * Bite_Range;
        }
        return false;
    }

    #endregion
}
