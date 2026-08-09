using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Jet_Fire : BulletBase
{
    [Header("×Óµ¯Ä§·¨ÉËº¦")]
    public short config_BaseMagicDamage;
    public float config_Radiu = 1;

    private List<ActorManager> actorManagers_Ignore = new List<ActorManager>();
    public override void InitBullet()
    {
        transform.DOKill();
        transform.localScale = Vector3.one;
    }
    public override void SetPhysics(Vector3 pos, Vector2 dir, float speedOffset, float forceOffset)
    {
        transform.position = pos;
        vectoe3_CurPos = transform.position;
        vectoe3_LastPos = transform.position;
        vectoe3_MoveDir = dir;
        float_BulletSpeed = speedOffset;
        if (float_BulletSpeed < 0) { float_BulletSpeed = 1; }
        if (float_BulletForce < 0) { float_BulletSpeed = 0; }
        transform.right = vectoe3_MoveDir;
        base.SetPhysics(pos, dir, speedOffset, forceOffset);
    }
    public override void SetDamage(int AdOffset, int MdOffset)
    {
        float_BulletMagicDemage = config_BaseMagicDamage + MdOffset;
        if (float_BulletAttackDemage < 0) { float_BulletAttackDemage = 0; }
        if (float_BulletMagicDemage < 0) { float_BulletMagicDemage = 0; }
        base.SetDamage(AdOffset, MdOffset);
    }
    public override void SetOwner(ActorManager owner)
    {
        actorManager_Owner = owner;
        actorAuthority_Owner = actorManager_Owner.actorAuthority;
        actorManagers_Ignore.Clear();
        actorManagers_Ignore.Add(actorManager_Owner);
        base.SetOwner(owner);
    }
    public void FixedUpdate()
    {
        if (!_hide)
        {
            Move(Time.fixedDeltaTime);
            Check(Time.fixedDeltaTime);
        }
    }
    private void Move(float dt)
    {
        transform.position += vectoe3_MoveDir * float_BulletSpeed * dt;
    }
    private void Check(float dt)
    {
        vectoe3_LastPos = vectoe3_CurPos;
        vectoe3_CurPos = transform.position;
        RaycastHit2D[] hits = Physics2D.CircleCastAll(vectoe3_LastPos, 0.2f, vectoe3_MoveDir, float_BulletSpeed * dt, layerMask_Target);
        foreach (var hit in hits)
        {
            if (hit.collider.CompareTag("Actor"))
            {
                if (hit.collider.isTrigger && hit.transform.TryGetComponent(out ActorManager actor) && !actorManagers_Ignore.Contains(actor))
                {
                    actorManagers_Ignore.Add(actor);
                    if (actorManager_Owner.actionManager.CheckApplyDamageTarget(actor, DamageTarget.WithoutMe))
                    {
                        AttackActor(actor);
                        PlayEffect(actor.transform.position);
                    }
                    continue;
                }
            }
            else
            {
                AttackObj(hit);
            }
        }

    }
    private void AttackActor(ActorManager actor)
    {
        if (actorAuthority_Owner.isLocal)
        {
            actor.actionManager.Client_TakeForce(vectoe3_MoveDir, (short)float_BulletForce);
            if (float_BulletMagicDemage > 0)
            {
                actorManager_Owner.actionManager.ApplyDamageToActor
                    (float_BulletMagicDemage, DamageState.MagicDamage, DamageTarget.WithoutMe, actor, out ApplyActorDamageCallBack callBack_1);
            }
        }
    }
    private void AttackObj(RaycastHit2D hit)
    {
        PlayEffect(hit.point);
        hit.transform.DOKill();
        hit.transform.localScale = Vector3.one;
        hit.transform.DOPunchScale(new Vector3(0.1f, -0.1f, 0), 0.1f);
        float_BulletSpeed = 0;
        HideBullet();
    }

    private void PlayEffect(Vector3 pos)
    {
        GameObject fire = PoolManager.Instance.GetEffectObj("Effect/Effect_Fire");
        fire.transform.position = pos;
    }
}
