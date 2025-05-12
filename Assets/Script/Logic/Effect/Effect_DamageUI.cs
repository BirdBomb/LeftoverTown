using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class Effect_DamageUI : EffectBase
{
    [SerializeField]
    private Transform transform_Root;
    [SerializeField]
    private Text text_Num;
    public void PlayShow(string val, Color32 color32, Vector3 offset)
    {
        transform_Root.DOKill();
        text_Num.DOKill();
        text_Num.text = val;
        text_Num.color = color32;
        transform_Root.localScale = Vector3.zero;
        transform.DOLocalJump(transform.position + offset, 0.5f, 1, 0.2f);
        transform_Root.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
        CancelInvoke("PlayHide");
        Invoke("PlayHide", 0.5f);
    }
    private void PlayHide()
    {
        transform_Root.DOScale(Vector2.zero, 0.25f);
        text_Num.DOFade(0, 0.25f);
    }
    public override void OnDisable()
    {
        text_Num.text = "";
        text_Num.color = new Color32();
        transform_Root.localScale = Vector3.zero;
        base.OnDisable();
    }
}
