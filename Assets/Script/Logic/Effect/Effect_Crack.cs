using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Crack : EffectBase
{
    public SpriteRenderer spriteRenderer_Crack;
    public override void OnEnable()
    {
        spriteRenderer_Crack.color = new Color(1, 1, 1, 1);
        spriteRenderer_Crack.DOFade(0, life).SetEase(Ease.InQuint);
        base.OnEnable();
    }
}
