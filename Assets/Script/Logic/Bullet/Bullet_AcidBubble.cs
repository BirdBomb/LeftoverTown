using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Bullet_AcidBubble : BulletBase
{
    [Header("子弹贴图")]
    public SpriteRenderer spriteRenderer_Bullet;
    [Header("子弹物理伤害")]
    public short config_BaseAttackDamage;
    [Header("子弹魔法伤害")]
    public short config_BaseMagicDamage;
    [Header("子弹速度")]
    public short config_BaseSpeed;
    [Header("子弹力量")]
    public float config_BaseForce;
    private List<ActorManager> actorManagers_Ignore = new List<ActorManager>();
    public override void InitBullet()
    {
        spriteRenderer_Bullet.enabled = true;
        base.InitBullet();
    }
    public override void SetPhysics(Vector3 pos, Vector2 dir, float speedOffset, float forceOffset)
    {
        transform.position = pos;
        vectoe3_CurPos = transform.position;
        vectoe3_LastPos = transform.position;
        vectoe3_MoveDir = dir;
        float_BulletSpeed = config_BaseSpeed + speedOffset;
        float_BulletForce = config_BaseForce + forceOffset;
        if (float_BulletSpeed < 0) { float_BulletSpeed = 1; }
        if (float_BulletForce < 0) { float_BulletSpeed = 0; }
        transform.right = vectoe3_MoveDir;
        base.SetPhysics(pos, dir, speedOffset, forceOffset);
    }
    public override void SetDamage(int AdOffset, int MdOffset)
    {
        float_BulletAttackDemage = config_BaseAttackDamage + AdOffset;
        float_BulletMagicDemage = config_BaseMagicDamage + AdOffset;
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
            SpeedDown(Time.fixedDeltaTime);
            Check(Time.fixedDeltaTime);
        }
    }
    private void Move(float dt)
    {
        transform.position += vectoe3_MoveDir * float_BulletSpeed * dt;
    }
    private void SpeedDown(float dt)
    {
        if (float_BulletSpeed > 0)
        {
            float_BulletSpeed -= dt;
        }
        else
        {
            HideBullet();
        }
    }
    private void Check(float dt)
    {
        vectoe3_LastPos = vectoe3_CurPos;
        vectoe3_CurPos = transform.position;
        RaycastHit2D[] hit2D = Physics2D.LinecastAll(vectoe3_LastPos, vectoe3_CurPos + vectoe3_MoveDir * float_BulletSpeed * dt, layerMask_Target);
        for (int i = 0; i < hit2D.Length; i++)
        {
            if (hit2D[i].collider.CompareTag("Actor"))
            {
                if (hit2D[i].collider.isTrigger && hit2D[i].transform.TryGetComponent(out ActorManager actor) && !actorManagers_Ignore.Contains(actor))
                {
                    actorManagers_Ignore.Add(actor);
                    if (actorManager_Owner.actionManager.CheckApplyDamageTarget(actor, DamageTarget.WithoutMe))
                    {
                        AttackActor(actor);
                        Boom(hit2D[i].point);
                        return;
                    }
                }
            }
            else
            {
                AttackObj(hit2D[i]);
                Boom(hit2D[i].point);
            }

        }
    }
    private void AttackActor(ActorManager actor)
    {
        if (actorAuthority_Owner.isLocal)
        {
            if (float_BulletAttackDemage > 0)
            {
                actorManager_Owner.actionManager.ApplyDamageToActor
                        (float_BulletAttackDemage, DamageState.AttackPiercingDamage, DamageTarget.WithoutMe, actor, out ApplyActorDamageCallBack callBack_0);
            }
            if (float_BulletMagicDemage > 0)
            {
                actorManager_Owner.actionManager.ApplyDamageToActor
                    (float_BulletMagicDemage, DamageState.MagicDamage, DamageTarget.WithoutMe, actor, out ApplyActorDamageCallBack callBack_1);
            }

        }
    }
    private void AttackObj(RaycastHit2D hit2D)
    {
        hit2D.transform.DOKill();
        hit2D.transform.localScale = Vector3.one;
        hit2D.transform.DOPunchScale(new Vector3(0.1f, -0.1f, 0), 0.1f);
    }
    private void Boom(Vector2 pos)
    {
        GameObject effect = PoolManager.Instance.GetEffectObj("Effect/Effect_AcidBubbleBoom");
        effect.transform.localScale = new Vector3(1 - (2 * new System.Random().Next(0, 2)), 1, 1);
        effect.transform.position = pos;
        float_BulletSpeed = 0;
    }
    public override void HideBullet()
    {
        _hide = true;
        spriteRenderer_Bullet.enabled = false;
        base.HideBullet();
    }
}
