using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Bomb : Bullet_Throwable
{
    private int BludgeoningDamage;
    private float ExplodeRange;
    //public override void SetOwner(ActorManager owner)
    //{
    //    actorManager_Owner = owner;
    //    actorAuthority_Owner = actorManager_Owner.actorAuthority;
    //    base.SetOwner(owner);
    //}

    public void SetBomb(int bludgeoningDamage,float explodeRange)
    {
        BludgeoningDamage = bludgeoningDamage;
        ExplodeRange = explodeRange;
    }
    public override void OnArrived()
    {
        GameObject effect = PoolManager.Instance.GetEffectObj("Effect/Effect_Explode3X3");
        effect.transform.position = transform.position;
        RaycastHit2D[] raycastHit2Ds = Physics2D.CircleCastAll(transform.position, ExplodeRange, Vector2.zero);
        foreach (RaycastHit2D hit2D in raycastHit2Ds)
        {
            if (hit2D.collider.CompareTag("Actor"))
            {
                if (hit2D.collider.isTrigger && hit2D.transform.TryGetComponent(out ActorManager actor))
                {
                    AttackActor(actor);
                }
            }
        }
        base.OnArrived();
    }
    private void AttackActor(ActorManager actorManager)
    {
        if (actorManager.actorAuthority.isLocal)
        {
            actorManager_Owner.actionManager.ApplyDamageToActor
                    (BludgeoningDamage, DamageState.AttackBludgeoningDamage, DamageTarget.All, actorManager, out ApplyActorDamageCallBack callBack_0);
        }
    }
}
