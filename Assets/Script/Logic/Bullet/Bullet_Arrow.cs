using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet_Arrow : BulletBase
{
    [Header("弓箭速度衰减(m/s2)")]
    private float float_ArrowSpeedDown;
    [Header("箭矢物理伤害")]
    public short config_BaseAttackDamage;
    [Header("箭矢魔法伤害")]
    public short config_BaseMagicDamage;
    [Header("箭矢速度")]
    public short config_BaseSpeed;
    [Header("箭矢力量")]
    public float config_BaseForce;
    [Header("弓箭ID")]
    public short int_ArrowID;
    [Header("弓箭回收几率%1000")]
    public int int_ArrowRecoveryProbability;
    private List<ActorManager> actorManagers_Ignore = new List<ActorManager>();
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
            float_BulletSpeed -= dt * float_ArrowSpeedDown;
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
                if (hit2D[i].collider.isTrigger && hit2D[i].transform.TryGetComponent(out ActorManager actor))
                {
                    if (!actorManagers_Ignore.Contains(actor))
                    {
                        actorManagers_Ignore.Add(actor);
                        GameObject effect = PoolManager.Instance.GetEffectObj("Effect/Effect_Blood");
                        effect.GetComponent<EffectBase>().SetEffect(vectoe3_MoveDir);
                        effect.transform.position = actor.transform.position;
                        Attack(actor);
                    }
                }
            }
            else
            {
                hit2D[i].transform.DOKill();
                hit2D[i].transform.localScale = Vector3.one;
                hit2D[i].transform.DOPunchScale(new Vector3(0.1f, -0.1f, 0), 0.1f);
                Boom(hit2D[i].point);
            }
        }
    }
    private void Attack(ActorManager actor)
    {
        if (actorAuthority_Owner.isLocal)
        {
            if (float_BulletAttackDemage > 0)
            {
                GameObject effect = PoolManager.Instance.GetEffectObj("Effect/Effect_Impact");
                effect.GetComponent<Effect_Impact>().PlayPiercing(vectoe3_MoveDir);
                effect.transform.position = actor.transform.position;

                actor.actorHpManager.TakeDamage(float_BulletAttackDemage, DamageState.AttackPiercingDamage, actorManager_Owner.actorNetManager);
            }
            if (float_BulletMagicDemage > 0)
            {
                actor.actorHpManager.TakeDamage(float_BulletMagicDemage, DamageState.MagicDamage, actorManager_Owner.actorNetManager);
            }
        }
    }
    private void Boom(Vector2 pos)
    {
        GameObject effect = PoolManager.Instance.GetEffectObj("Effect/Effect_WoodBoom");
        effect.transform.localScale = new Vector3(1 - (2 * new System.Random().Next(0, 2)), 1, 1);
        effect.transform.position = pos;
        if (float_BulletSpeed > 0)
        {
            float_BulletSpeed = 0;
            Recycle(pos);
        }
    }
    private void Recycle(Vector2 pos)
    {
        if (actorAuthority_Owner.isState)
        {
            if (new System.Random().Next(0, 1001) <= int_ArrowRecoveryProbability)
            {
                Type type = Type.GetType("Item_" + int_ArrowID.ToString());
                ((ItemBase)Activator.CreateInstance(type)).StaticAction_InitData(int_ArrowID, out ItemData initData);
                initData.C = 1;
                MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnItem()
                {
                    itemData = initData,
                    itemOwner = new Fusion.NetworkId(),
                    pos = (Vector3)pos - vectoe3_MoveDir,
                });
            }
        }
    }
}
