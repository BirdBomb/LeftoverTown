using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Bullet9004 : BulletBase
{
    [SerializeField]
    private float speedOffset;
    [SerializeField]
    private short damageOffset;
    public SpriteRenderer spriteRenderer;
    public TrailRenderer trailRenderer;
    public Light2D bulletLight;
    public override void InitBullet(Vector3 dir, float speed, ActorNetManager from)
    {
        transform.DOKill();
        transform.localScale = Vector3.one;
        spriteRenderer.enabled = true;
        trailRenderer.enabled = true;
        bulletLight.enabled = true;
        bulletLight.intensity = 1;
        trailRenderer.Clear();
        base.InitBullet(dir, speed + speedOffset, from);

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

        RaycastHit2D[] hit2D = Physics2D.LinecastAll(lastPos, curPos, target);
        for (int i = 0; i < hit2D.Length; i++)
        {
            if (hit2D[i].collider.CompareTag("Actor"))
            {
                if (hit2D[i].transform.TryGetComponent(out ActorManager actor))
                {
                    if (actor == _from.LocalManager) { continue; }
                    else
                    {
                        actor.TakeDamage(1 + damageOffset, _from);
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
    public void Boom()
    {
        moveSpeed = 0;
    }

    public void SpeedDown(float dt)
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
