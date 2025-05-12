using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Hook : BulletBase
{
    private List<ActorManager> actorManagers_Ignore = new List<ActorManager>();
    public Transform transform_Bullet;
    public LineRenderer lineRenderer;
    [SerializeField, Header("钩子伤害")]
    public short config_BaseDamage;
    [SerializeField, Header("钩子速度")]
    public short config_BaseSpeed;
    [SerializeField, Header("衰减速度")]
    public float config_DownSpeed;
    [SerializeField, Header("钩子力量")]
    public float config_BaseForce;
    public override void Shot(Vector3 dir, int demage_Offset, float speed_Offset, float force_Offset, ActorNetManager from)
    {
        gameObject.SetActive(true);
        transform_Bullet.localPosition = Vector3.zero;
        lineRenderer.SetPosition(0, transform_Bullet.position);
        lineRenderer.SetPosition(1, transform_Bullet.position);
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
        base.Shot(dir, demage_Offset, speed_Offset, force_Offset, from);
        if (transform.lossyScale.x > 0)
        {
            transform.right = vectoe3_MoveDir;
        }
        else
        {
            transform.right = -vectoe3_MoveDir;
        }
    }

    public override void Fly(float dt)
    {
        SpeedDown(dt);
        transform_Bullet.position += vectoe3_MoveDir * float_BulletSpeed * dt;
        lineRenderer.SetPosition(0, transform_Bullet.position);
        lineRenderer.SetPosition(1, transform.position);
        base.Fly(dt);
    }
    public override void Check(float dt)
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
        base.Check(dt);
    }
    private void Attack(ActorManager actor)
    {
        if (actorAuthority_Owner.isLocal)
        {
            actor.actionManager.AddForce(-vectoe3_MoveDir, float_BulletForce);
            actor.AllClient_Listen_TakeAttackDamage(float_BulletDemage, actorNetManager_Owner);
        }
    }
    private void Boom(Vector2 pos)
    {
        GameObject effect = PoolManager.Instance.GetObject("Effect/Effect_BulletBoom");
        effect.transform.localScale = new Vector3(1 - (2 * new System.Random().Next(0, 2)), 1, 1);
        effect.transform.position = pos;
    }
    public override void HideBullet()
    {
        gameObject.SetActive(false);
        float_BulletSpeed = 0;
    }
    public void SpeedDown(float dt)
    {
        float_BulletSpeed -= dt * config_DownSpeed;
    }

}
