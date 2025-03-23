using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Bullet9010 : BulletBase
{
    [Header("×Óµ¯")]
    public SpriteRenderer spriteRenderer;
    [Header("ÍÏÎ²")]
    public TrailRenderer trailRenderer;
    [Header("¹âÔ´")]
    public Light2D bulletLight;
    /// <summary>
    /// ºöÂÔ
    /// </summary>
    private List<ActorManager> ignoreList = new List<ActorManager>();
    private float float_BaseForce = 5;
    private float float_ExtraForce = 0;
    public override void InitBullet(Vector3 dir, float speed, float force, ActorNetManager from)
    {
        transform.DOKill();
        transform.localScale = Vector3.one;
        spriteRenderer.enabled = true;
        trailRenderer.enabled = true;
        bulletLight.enabled = true;
        bulletLight.intensity = 1;
        trailRenderer.Clear();
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
        RaycastHit2D[] hit2D = Physics2D.LinecastAll(vectoe3_LastPos, vectoe3_CurPos + vectoe3_MoveDir * float_MoveSpeed * dt, layerMask_Target);
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
                    }
                }
            }
            else
            {
                Boom(hit2D[i].point);
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
            actor.actionManager.AddForce(vectoe3_MoveDir, float_Force);
            actor.AllClient_Listen_TakeDamage(config_Damage, actorNetManager_Owner);
        }
    }
    /// <summary>
    /// ±¬Õ¨
    /// </summary>
    /// <param name="pos"></param>
    private void Boom(Vector2 pos)
    {
        GameObject effect = PoolManager.Instance.GetObject("Effect/Effect_BulletBoom");
        effect.transform.position = pos;
        float_MoveSpeed = 0;
    }
    /// <summary>
    /// ¼õËÙ
    /// </summary>
    /// <param name="dt"></param>
    private void SpeedDown(float dt)
    {
        if (bulletLight.intensity > 0)
        {
            bulletLight.intensity -= dt;
        }
        if (float_MoveSpeed > 0)
        {
            float_MoveSpeed -= dt;
        }
        else
        {
            HideBullet();
        }
    }
    public override void HideBullet()
    {
        _hide = true;
        spriteRenderer.enabled = false;
        trailRenderer.enabled = false;
        bulletLight.enabled = false;
        base.HideBullet();
    }
}
