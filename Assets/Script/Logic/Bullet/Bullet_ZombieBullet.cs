using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bullet_ZombieBullet : BulletBase
{
    public override void Fly(float dt)
    {
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
                    if (actor == actorNetManager_Owner.actorManager_Local) { continue; }
                    else
                    {
                        TryAttack(actor);
                        HideBullet();
                        PoolManager.Instance.GetObject("Effect/Effect_ZombieBulletBoom").transform.position = hit2D[i].point;
                    }
                }
            }
            else
            {
                HideBullet();
                PoolManager.Instance.GetObject("Effect/Effect_ZombieBulletBoom").transform.position = hit2D[i].point;
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

}
