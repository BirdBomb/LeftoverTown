using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class ItemLocalObj_Bomb : ItemLocalObj_Throwable
{
    private int BludgeoningDamage;
    private float ExplodeRange;
    public void UpdateBombData(int bludgeoningDamage,float explodeRange,ItemQuality itemQuality)
    {
        BludgeoningDamage = bludgeoningDamage;
        ExplodeRange = explodeRange;
    }
    public override void Throw()
    {
        GameObject obj = PoolManager.Instance.GetObject("Bullet/Bullet_Grenade");
        if (obj.TryGetComponent(out Bullet_Bomb bulletBase))
        {
            bulletBase.SetPath(transform.position, transform.position + inputData.mousePosition, 9);
            bulletBase.SetBomb(BludgeoningDamage, ExplodeRange);
            bulletBase.SetOwner(actorManager);
        }
        if (actorManager.actorAuthority.isPlayer && actorManager.actorAuthority.isLocal)
        {
            ItemData _oldItemHand = actorManager.actorNetManager.Local_ItemHand;
            ItemData _newItemHand = _oldItemHand;
            _newItemHand.C--;
            if (_newItemHand.C <= 0)
            {
                MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemHand_Sub()
                {
                    item = _oldItemHand,
                });
            }
            else
            {
                MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemHand_Change()
                {
                    oldItem = _oldItemHand,
                    newItem = _newItemHand,
                });
            }
        }

    }
}
