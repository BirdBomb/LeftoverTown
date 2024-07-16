using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet9004 : BulletBase
{
    [SerializeField]
    private float speedOffset;
    [SerializeField]
    private int damageOffset;
    public SpriteRenderer spriteRenderer;
    public TrailRenderer trailRenderer;
    public override void InitBullet(Vector3 dir, float speed, ActorNetManager from)
    {
        transform.DOKill();
        transform.localScale = Vector3.one;
        spriteRenderer.enabled = true;
        trailRenderer.enabled = true;
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
        spriteRenderer.enabled = false;
        _hide = true;
        moveSpeed = 0;
        CancelInvoke();
        Invoke("HideBullet", 20);
    }

    public void SpeedDown(float dt)
    {
        if (moveSpeed > 0)
        {
            moveSpeed -= dt;
        }
        if (moveSpeed < 0)
        {
            HideBullet();
        }
    }
    public override void HideBullet()
    {
        trailRenderer.enabled = false;
        base.HideBullet();
    }
}
