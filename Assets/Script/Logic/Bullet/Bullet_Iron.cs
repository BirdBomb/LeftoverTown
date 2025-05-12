using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Bullet_Iron : BulletBase
{
    public SpriteRenderer spriteRenderer_Bullet;
    public Light2D light2D_Bullet;
    private List<ActorManager> actorManagers_Ignore = new List<ActorManager>();

    [SerializeField, Header("子弹伤害")]
    public short config_BaseDamage;
    [SerializeField, Header("子弹速度")]
    public short config_BaseSpeed;
    [SerializeField, Header("子弹力量")]
    public float config_BaseForce;
    public override void Shot(Vector3 dir, int demage_Offset, float speed_Offset, float force_Offset, ActorNetManager from)
    {
        vectoe3_MoveDir = dir;
        float_BulletDemage = config_BaseDamage + demage_Offset;
        float_BulletSpeed = config_BaseSpeed + speed_Offset;
        float_BulletForce = config_BaseForce + force_Offset;
        vectoe3_CurPos = transform.position;
        vectoe3_LastPos = transform.position;
        actorNetManager_Owner = from;
        actorAuthority_Owner = actorNetManager_Owner.actorManager_Local.actorAuthority;

        actorManagers_Ignore.Clear();
        actorManagers_Ignore.Add(from.actorManager_Local);
        transform.DOKill();
        transform.localScale = Vector3.one;
        spriteRenderer_Bullet.enabled = true;
        light2D_Bullet.enabled = true;
        light2D_Bullet.intensity = 0.5f;
        base.Shot(dir, demage_Offset, speed_Offset, force_Offset, from);
        transform.right = vectoe3_MoveDir;
    }
    public override void Fly(float dt)
    {
        SpeedDown(dt);
        transform.position += vectoe3_MoveDir * float_BulletSpeed * dt;
        base.Fly(dt);
    }
    private void SpeedDown(float dt)
    {
        if (light2D_Bullet.intensity > 0)
        {
            light2D_Bullet.intensity -= dt;
        }
        if (float_BulletSpeed > 0)
        {
            float_BulletSpeed -= dt;
        }
        else
        {
            HideBullet();
        }
    }
    public override void Check(float dt)
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
                        GameObject effect = PoolManager.Instance.GetObject("Effect/Effect_Blood");
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
        base.Check(dt);
    }
    private void Attack(ActorManager actor)
    {
        if (actorAuthority_Owner.isLocal)
        {
            actor.actionManager.AddForce(vectoe3_MoveDir, float_BulletForce);
            actor.AllClient_Listen_TakeAttackDamage(float_BulletDemage, actorNetManager_Owner);
        }
    }
    private void Boom(Vector2 pos)
    {
        GameObject effect = PoolManager.Instance.GetObject("Effect/Effect_BulletBoom");
        effect.transform.localScale = new Vector3(1 - (2 * new System.Random().Next(0, 2)), 1, 1);
        effect.transform.position = pos;
        float_BulletSpeed = 0;
    }

    public override void HideBullet()
    {
        _hide = true;
        spriteRenderer_Bullet.enabled = false;
        light2D_Bullet.enabled = false;
        base.HideBullet();
    }


}
