using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLocalObj_2012 : ItemLocalObj
{
    public Transform sprite;
    public Transform muzzle;
    
    public void Shoot()
    {
        GameObject obj = PoolManager.Instance.GetObject("Effect/Effect_MuzzleFire101");
        obj.transform.SetParent(muzzle);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        AudioManager.Instance.PlayEffect(1001,transform);
        sprite.DOPunchPosition(new Vector3(-0.1f, -0.1f, 0), 0.1f);
    }
    public void Dull()
    {
        GameObject obj = PoolManager.Instance.GetObject("Effect/Effect_MuzzleFire101");
        obj.transform.SetParent(muzzle);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        AudioManager.Instance.PlayEffect(1001, transform);
        sprite.DOPunchPosition(new Vector3(-0.1f, -0.1f, 0), 0.1f);
    }
}
