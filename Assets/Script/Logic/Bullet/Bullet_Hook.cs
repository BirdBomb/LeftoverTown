using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Hook : BulletBase
{
    [SerializeField, Header("钩子弹头")]
    public Transform transform_Bullet;
    [SerializeField, Header("钩子线")]
    public LineRenderer lineRenderer;
    [SerializeField, Header("钩子物理伤害")]
    public short config_BaseAttackDamage;
    [SerializeField, Header("钩子魔法伤害")]
    public short config_BaseMagicDamage;

    [SerializeField, Header("钩子速度")]
    public short config_BaseSpeed;
    [SerializeField, Header("衰减速度")]
    public float config_DownSpeed;
    [SerializeField, Header("钩子力量")]
    public float config_BaseForce;
    private bool _show = false;
    private List<ActorManager> actorManagers_Ignore = new List<ActorManager>();

    public override void InitBullet()
    {
        ShowHook();
        transform.DOKill();
        transform.localScale = Vector3.one;
    }
    public override void SetPhysics(Vector3 pos, Vector2 dir, float speedOffset, float forceOffset)
    {
        vectoe3_CurPos = transform.position;
        vectoe3_LastPos = transform.position;
        vectoe3_MoveDir = dir;
        float_BulletSpeed = config_BaseSpeed + speedOffset;
        float_BulletForce = config_BaseForce + forceOffset;
        if (float_BulletSpeed < 0) { float_BulletSpeed = 1; }
        if (float_BulletForce < 0) { float_BulletSpeed = 0; }
        transform_Bullet.localPosition = Vector3.zero;
        lineRenderer.SetPosition(0, transform_Bullet.position);
        lineRenderer.SetPosition(1, transform_Bullet.position);
        if (transform.lossyScale.x > 0)
        {
            transform.right = vectoe3_MoveDir;
        }
        else
        {
            transform.right = -vectoe3_MoveDir;
        }
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
        if (_show)
        {
            Move(Time.fixedDeltaTime);
            SpeedDown(Time.fixedDeltaTime);
            Check(Time.fixedDeltaTime);
        }
    }
    private void Move(float dt)
    {
        if (float_BulletSpeed > 0)
        {
            transform_Bullet.position += vectoe3_MoveDir * float_BulletSpeed * dt;
        }
        else
        {
            transform_Bullet.localPosition += transform_Bullet.localPosition.normalized * float_BulletSpeed * dt;
            if (transform_Bullet.localPosition.magnitude < 0.5f) { HideHook(); }
        }
        transform_Bullet.position += vectoe3_MoveDir * float_BulletSpeed * dt;
        lineRenderer.SetPosition(0, transform_Bullet.position);
        lineRenderer.SetPosition(1, transform.position);
    }
    private void SpeedDown(float dt)
    {
        float_BulletSpeed -= dt * config_DownSpeed;
    }
    private void Check(float dt)
    {
        vectoe3_LastPos = vectoe3_CurPos;
        vectoe3_CurPos = transform_Bullet.position;
        if (float_BulletSpeed < 0)
        {
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
                            GameObject effect = PoolManager.Instance.GetObject("Effect/Effect_Blood");
                            effect.GetComponent<EffectBase>().SetEffect(-vectoe3_MoveDir);
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
    }
    private void Attack(ActorManager actor)
    {
        if (actorAuthority_Owner.isLocal)
        {
            actor.actionManager.AddForce(-vectoe3_MoveDir, float_BulletForce);
            if (float_BulletAttackDemage > 0)
            {
                actor.AllClient_Listen_TakeAttackDamage(float_BulletAttackDemage, actorManager_Owner.actorNetManager);
            }
            if (float_BulletMagicDemage > 0)
            {
                actor.AllClient_Listen_TakeMagicDamage(float_BulletMagicDemage, actorManager_Owner.actorNetManager);
            }
        }
    }
    private void Boom(Vector2 pos)
    {
        GameObject effect = PoolManager.Instance.GetObject("Effect/Effect_BulletBoom");
        effect.transform.localScale = new Vector3(1 - (2 * new System.Random().Next(0, 2)), 1, 1);
        effect.transform.position = pos;
    }
    public void ShowHook()
    {
        _show = true;
        gameObject.SetActive(true);
    }
    public void HideHook()
    {
        _show = false;
        gameObject.SetActive(false);
    }
    public override void HideBullet()
    {
    }
}
