using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Arrow : BulletBase
{
    [SerializeField, Header("¹­¼ýËÙ¶ÈË¥¼õ(m/s2)")]
    private float speedDown;
    /// <summary>
    /// ºöÂÔ
    /// </summary>
    private List<ActorManager> ignoreList = new List<ActorManager>();
    public override void InitBullet(Vector3 dir, float speed,float force, ActorNetManager from)
    {
        transform.DOKill();
        transform.localScale = Vector3.one;

        ignoreList.Clear();
        ignoreList.Add(from.actorManager_Local);

        base.InitBullet(dir, speed, force, from);

        transform.right = vectoe3_MoveDir;
    }
    public override void Fly(float dt)
    {
        SpeedDown(dt);
        transform.position += vectoe3_MoveDir * float_MoveSpeed * dt;
        base.Fly(dt);
    }
    public override void Check(float dt)
    {
        vectoe3_LastPos = vectoe3_CurPos;
        vectoe3_CurPos = transform.position;

        RaycastHit2D[] hit2D = Physics2D.LinecastAll(vectoe3_LastPos, vectoe3_CurPos, layerMask_Target);
        for (int i = 0; i < hit2D.Length; i++)
        {
            if (hit2D[i].collider.CompareTag("Actor"))
            {
                if (hit2D[i].transform.TryGetComponent(out ActorManager actor))
                {
                    if (ignoreList.Contains(actor)) { continue; }
                    else
                    {
                        ignoreList.Add(actor);
                        GameObject effect = PoolManager.Instance.GetObject("Effect/Effect_Blood");
                        effect.GetComponent<EffectBase>().SetEffect(vectoe3_MoveDir);
                        effect.transform.position = actor.transform.position;
                        TryAttack(actor);
                        Boom();
                    }
                }
            }
            else
            {
                Boom();
            }
        }
        base.Check(dt);
    }
    /// <summary>
    /// ³¢ÊÔ¹¥»÷
    /// </summary>
    private void TryAttack(ActorManager actor)
    {
        if (actorAuthority_Owner.isLocal)
        {
            actor.AllClient_Listen_TakeDamage(config_Damage, actorNetManager_Owner);
        }
    }
    /// <summary>
    /// ¼õËÙ
    /// </summary>
    /// <param name="dt"></param>
    public void SpeedDown(float dt)
    {
        if (float_MoveSpeed > 0)
        {
            float_MoveSpeed -= dt * speedDown;
        }
        if (float_MoveSpeed < 0)
        {
            HideBullet();
        }
    }
    /// <summary>
    /// ±¬Õ¨
    /// </summary>
    public void Boom()
    {
        float_MoveSpeed = 0;
        HideBullet();
    }
}
