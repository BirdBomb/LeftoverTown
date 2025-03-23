using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Effect_Blood : EffectBase
{
    public Transform transform_BloodLake;
    public SpriteRenderer spriteRenderer_BloodLake;
    public Sprite[] sprites_BloodLake;
    public override void SetEffect(Vector3 dir)
    {
        transform.up = dir;
        spriteRenderer_BloodLake.sprite = sprites_BloodLake[new System.Random().Next(0, sprites_BloodLake.Length)];
        spriteRenderer_BloodLake.color = new Color(1, 1, 1, 1);
        spriteRenderer_BloodLake.DOFade(0, life).SetEase(Ease.InQuint);
        base.SetEffect(dir);
    }
}
