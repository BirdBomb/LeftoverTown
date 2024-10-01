using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bullet100 : BulletBase
{
    public override void Fly(float dt)
    {
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
                        TryAttack(actor);
                        HideBullet();
                        PoolManager.Instance.GetObject("Effect/Effect_Bullet100").transform.position = hit2D[i].point;
                    }
                }
            }
            else
            {
                HideBullet();
                PoolManager.Instance.GetObject("Effect/Effect_Bullet100").transform.position = hit2D[i].point;
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

}
