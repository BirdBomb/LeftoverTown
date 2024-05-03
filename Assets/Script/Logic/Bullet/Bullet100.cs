using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bullet100 : BulletBase
{
    public override void Fly(float dt)
    {
        transform.position += moveDir * moveSpeed * dt;
        lastPos = curPos;
        curPos = transform.position;
        RaycastHit2D hit2D = Physics2D.Linecast(lastPos, curPos, target);
        if (hit2D)
        {
            HideBullet();
            PoolManager.Instance.GetObject("Effect/Effect_Bullet100").transform.position = hit2D.point;
            if(hit2D.transform.TryGetComponent(out ActorManager actor))
            {
                actor.TakeDamage(5, ownerId);
            }
        }
        base.Fly(dt);
    }
    public override void HideBullet()
    {
        PoolManager.Instance.GetObject("Effect/Effect_Bullet100").transform.position = transform.position;
        base.HideBullet();
    }
}
