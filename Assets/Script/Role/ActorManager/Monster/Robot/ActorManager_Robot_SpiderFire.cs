using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorManager_Robot_SpiderFire : ActorManager_Monster
{
    #region//¼àÌý
    public override void ForState_Listen_MyselfInjured(int parameter, HpChangeReason reason, NetworkId id)
    {
        NetworkObject networkObject = actorNetManager.Runner.FindObject(id);
        if (networkObject != null && networkObject.TryGetComponent(out ActorManager who) && who.actorAuthority.isPlayer)
        {
            State_InAttack(who);
        }
    }
    public override void State_SecondUpdate()
    {
        State_UpdateSkillTimer();
        base.State_SecondUpdate();
    }
    public override void AllClient_UpdateHpBar(float val)
    {
        UI_BossInfo.Instance.Show(actorNetManager.Net_HpCur, actorNetManager.Local_HpMax, "ÆûÓÍ¹Þ", actorNetManager.Object.Id);
        base.AllClient_UpdateHpBar(val);
    }

    #endregion
    #region//¼ì²é
    public override bool State_CheckNearbyActor()
    {
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
        State_Think_GoToStroll_Long(2, 5);
    }
    #endregion
    #region//
    private enum Skill
    {
        JetFire
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
            if (int_JetTimer <= 0)
            {
                actorNetManager.RPC_State_NpcUseSkill((int)Skill.JetFire, pathManager.vector3Int_CurPos, target.actorNetManager.Object.Id);
                int_JetTimer = int_JetCD;
                return true;
            }
        }
        return false;
    }
    public void State_UpdateSkillTimer()
    {
        int_JetTimer--;
    }
    public override void ForAll_Listen_NpcAction(int id, Vector3Int vector3, NetworkId networkId)
    {
        if (id == (int)Skill.JetFire)
        {
            if (coroutine_Jeting == null)
            {
                coroutine_Jeting = StartCoroutine(AllClient_JetPlaying(vector3, networkId));
            }
        }
        base.ForAll_Listen_NpcAction(id, vector3, networkId);
    }
    #endregion
    #region//¼¼ÄÜ_ÅçÉä»ðÑæ
    [Header("-------»ðÑæÅçÉä--------")]
    [Header("»ðÑæÅçÉäËÙ¶È")]
    public float float_BulletSpeed;
    [Header("»ðÑæÅçÉä³ÖÐøÊ±¼ä")]
    public float float_BulletDuration;
    private const int int_BulletCount = 25;
    private const int int_JetCD = 10;
    private int int_JetTimer = 0;
    public ParticleSystem particleSystem_JetFire_0;
    public ParticleSystem particleSystem_JetFire_1;
    private Coroutine coroutine_Jeting;

   
    private IEnumerator AllClient_JetPlaying(Vector3Int vector3, NetworkId networkId)
    {
        particleSystem_JetFire_0.Play();
        particleSystem_JetFire_1.Play();
        actionManager.TurnTo(((Vector3)(vector3 - pathManager.vector3Int_CurPos)));
        bodyController.SetAnimatorTrigger(BodyPart.Head, "JetFire");
        for (int i = 0; i < int_BulletCount; i++)
        {
            CreateBullet();
            yield return new WaitForSeconds(5f/ int_BulletCount);

        }
        particleSystem_JetFire_0.Stop();
        particleSystem_JetFire_1.Stop();
        coroutine_Jeting = null;
    }
    private void CreateBullet()
    {
        GameObject obj_0 = PoolManager.Instance.GetObject("Bullet/Bullet_Jet_Fire");
        GameObject obj_1 = PoolManager.Instance.GetObject("Bullet/Bullet_Jet_Fire");
        if (obj_0.TryGetComponent(out BulletBase bulletBase_0))
        {
            bulletBase_0.InitBullet();
            bulletBase_0.SetPhysics(particleSystem_JetFire_0.transform.position, particleSystem_JetFire_0.transform.right, float_BulletSpeed, 0);
            bulletBase_0.SetLifeTime(float_BulletDuration);
            bulletBase_0.SetDamage(0, 50);
            bulletBase_0.SetOwner(this);
        }
        if (obj_1.TryGetComponent(out BulletBase bulletBase_1))
        {
            bulletBase_1.InitBullet();
            bulletBase_1.SetPhysics(particleSystem_JetFire_1.transform.position, particleSystem_JetFire_1.transform.right, float_BulletSpeed, 0);
            bulletBase_1.SetLifeTime(float_BulletDuration);
            bulletBase_1.SetDamage(0, 50);
            bulletBase_1.SetOwner(this);
        }
    }

    #endregion
}
