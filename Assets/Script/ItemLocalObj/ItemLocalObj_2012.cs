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
        GameObject muzzleFire101 = PoolManager.Instance.GetObject("Effect/Effect_MuzzleFire101");
        muzzleFire101.transform.SetParent(muzzle);
        muzzleFire101.transform.localScale = Vector3.one;
        muzzleFire101.transform.localPosition = Vector3.zero;
        muzzleFire101.transform.localRotation = Quaternion.identity;

        GameObject muzzleSmoke = PoolManager.Instance.GetObject("Effect/Effect_MuzzleSmoke");
        muzzleSmoke.transform.localScale = Vector3.one;
        muzzleSmoke.transform.position = muzzle.position;
        muzzleSmoke.transform.rotation = Quaternion.identity;
        AudioManager.Instance.PlayEffect(1001, transform);

        sprite.DOPunchPosition(new Vector3(-0.1f, -0.1f, 0), 0.1f);
    }
    public void Dull()
    {
        AudioManager.Instance.PlayEffect(1002, transform);
    }
}