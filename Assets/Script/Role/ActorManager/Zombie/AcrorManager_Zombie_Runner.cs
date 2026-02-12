using Fusion;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class AcrorManager_Zombie_Runner : ActorManager_Animal
{
    [Header("Åü¿³ÉËº¦")]
    public int Hack_DamageVal;
    [Header("Åü¿³·¶Î§")]
    public float Hack_Range;
    [Header("ÌøÅüÉËº¦")]
    public int Jump_DamageVal;
    [Header("ÌøÅü·¶Î§")]
    public float Jump_Range;
    [Header("ÌøÅü×îĞ¡¾àÀë")]
    public float Jump_MinDistance;
    [Header("ÌøÅü×î´ó¾àÀë")]
    public float Jump_MaxDistance;
    #region//¼àÌı
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
        Hack,
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
        if (State_CheckingHackDistance())
        {
            pathManager.State_SetFrezzeTime(1f);
            State_RsetAttackTime(float_StateAttackCD);
            actorNetManager.RPC_State_NpcUseSkill((int)Skill.Hack, brainManager.allClient_actorManager_AttackTarget.pathManager.vector3Int_CurPos, brainManager.allClient_actorManager_AttackTarget.actorNetManager.Object.Id);
            return true;
        }
        if (State_CheckingJumpDistance())
        {
            if (new System.Random().Next(0, 10) > 5)
            {
                pathManager.State_SetFrezzeTime(1f);
                State_RsetAttackTime(float_StateAttackCD);
                actorNetManager.RPC_State_NpcUseSkill((int)Skill.Jump, brainManager.allClient_actorManager_AttackTarget.pathManager.vector3Int_CurPos, brainManager.allClient_actorManager_AttackTarget.actorNetManager.Object.Id);
            }
            return true;
        }
        return false;
    }

    public override void AllClient_Listen_NpcAction(int id, Vector3Int vector3, NetworkId networkId)
    {
        if (id == (int)Skill.Hack)
        {
            AllClient_Hack(vector3, networkId);
        }
        if (id == (int)Skill.Jump)
        {
            AllClient_Jump(vector3, networkId);
        }
        base.AllClient_Listen_NpcAction(id, vector3, networkId);
    }
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
                        if (actorManager.statusManager.statusType != StatusType.Monster_Common)
                        {
                            if (actorManager.actorAuthority.isLocal)
                            {
                                Local_HackAtor(actorManager);
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
        bodyController.SetAnimatorFunc(BodyPart.Body, (str) =>
        {
            if (str.Equals("Hack_1"))
            {
                RaycastHit2D[] raycastHit2Ds = Physics2D.CircleCastAll(transform.position, 1.5f, Vector2.zero);
                foreach (RaycastHit2D hit2D in raycastHit2Ds)
                {
                    if (hit2D.collider.isTrigger && hit2D.collider.gameObject.TryGetComponent(out ActorManager actorManager))
                    {
                        if (actorManager.statusManager.statusType != StatusType.Monster_Common)
                        {
                            if (actorManager.actorAuthority.isLocal)
                            {
                                Local_HackAtor(actorManager);
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
    private void Local_HackAtor(ActorManager actor)
    {
        GameObject effect = PoolManager.Instance.GetEffectObj("Effect/Effect_Impact");
        effect.GetComponent<Effect_Impact>().PlaySlash(actor.transform.position - transform.position, true);
        effect.transform.position = actor.transform.position;

        actor.actorHpManager.TakeDamage(Hack_DamageVal, DamageState.AttackSlashingDamage, actorNetManager);
    }

    /// <summary>
    /// ¼ì²é¹¥»÷¾àÀë
    /// </summary>
    /// <returns></returns>
    public bool State_CheckingHackDistance()
    {
        if (Vector3.Distance(brainManager.allClient_actorManager_AttackTarget.transform.position, transform.position) < Hack_Range)
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
                    RaycastHit2D[] raycastHit2Ds = Physics2D.CircleCastAll(transform.position, Jump_Range, Vector2.zero);
                    foreach (RaycastHit2D hit2D in raycastHit2Ds)
                    {
                        if (hit2D.collider.isTrigger && hit2D.collider.gameObject.TryGetComponent(out ActorManager actorManager))
                        {
                            if (actorManager.statusManager.statusType != StatusType.Monster_Common)
                            {
                                if (actorManager.actorAuthority.isLocal)
                                {
                                    Local_JumpAtor(actorManager);
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
    private void Local_JumpAtor(ActorManager actor)
    {
        GameObject effect = PoolManager.Instance.GetEffectObj("Effect/Effect_Impact");
        effect.GetComponent<Effect_Impact>().PlaySlash(actor.transform.position - transform.position, true);
        effect.transform.position = actor.transform.position;

        actor.actorHpManager.TakeDamage(Jump_DamageVal, DamageState.AttackSlashingDamage, actorNetManager);
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
    [Header("×¼±¸ÌøÔ¾µÄ×îĞ¡¾àÀë"), Range(0, 25)]
    public float float_MinJumpDistance;
    [Header("×¼±¸ÌøÔ¾µÄ×î´ó¾àÀë"), Range(0, 25)]
    public float float_MaxJumpDistance;
    [Header("ÌøÔ¾ÉËº¦")]
    public int int_JumpDamage;
    [Header("Åü¿³ÉËº¦")]
    public int int_HackDamage;
    [Header("ÊÓÒ°¾àÀë"), Range(1, 99)]
    public float float_ViewDistance;
    [Header("»ù±¾µôÂäÁĞ±í")]
    public List<BaseLootInfo> lootInfos_Base;
    [Header("¶îÍâµôÂäÁĞ±í")]
    public List<ExtraLootInfo> lootInfos_Extra;
}
