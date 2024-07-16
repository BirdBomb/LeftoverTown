using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLocalObj_2013 : ItemLocalObj
{
    public Transform sprite;
    public Transform muzzle;
    public void Shoot()
    {
        GameObject obj = PoolManager.Instance.GetObject("Effect/Effect_MuzzleFire101");
        obj.transform.position = muzzle.position;
        obj.transform.rotation = muzzle.rotation;
        AudioManager.Instance.PlayEffect(1001);
        sprite.DOPunchPosition(new Vector3(-0.1f, -0.1f, 0), 0.1f);
    }
}
