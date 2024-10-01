using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Bullet9004 : BulletBase
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
    public override void InitBullet(Vector3 dir, float speed, ActorNetManager from)
    {
        transform.DOKill();
        transform.localScale = Vector3.one;
        spriteRenderer.enabled = true;
        trailRenderer.enabled = true;
        bulletLight.enabled = true;
        bulletLight.intensity = 1;
        trailRenderer.Clear();
        ignoreList.Clear();
        ignoreList.Add(from.LocalManager);
        base.InitBullet(dir, speed, from);

        transform.right = moveDir;
    }
    public override void Fly(float dt)
    {
        SpeedDown(dt);
        transform.position += moveDir * moveSpeed * dt;
        base.Fly(dt);
    }
    public override void Check(float dt)
    {
        lastPos = curPos;
        curPos = transform.position;
        RaycastHit2D[] hit2D = Physics2D.LinecastAll(lastPos, curPos + moveDir * moveSpeed * dt, target);
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
        if (_input)
        {
            actor.AllClient_TakeDamage(configDamage, _from);
        }
    }
    /// <summary>
    /// ±¬Õ¨
    /// </summary>
    /// <param name="pos"></param>
    private void Boom(Vector2 pos)
    {
        GameObject effect = PoolManager.Instance.GetObject("Effect/Effect_BulletBoom105");
        effect.transform.position = pos;
        moveSpeed = 0;
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
        if (moveSpeed > 0)
        {
            moveSpeed -= dt;
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
