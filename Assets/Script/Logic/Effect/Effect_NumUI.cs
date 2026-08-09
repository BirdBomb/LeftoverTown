using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class Effect_NumUI : EffectBase
{
    public Transform transform_Root;
    public Text text_Num;
    public Text text_BackGround;
    private System.Random random = new System.Random();
    public void Init(string val, Color32 color32, NumPlayType playType)
    {
        Clean();
        text_BackGround.text = val;
        text_BackGround.color = UnityEngine.Color.black;
        text_Num.text = val;
        text_Num.color = color32;
        StartCoroutine(Life(playType));
    }
    public void Clean()
    {
        transform_Root.DOKill();
        text_Num.DOKill();
        text_BackGround.DOKill();

    }
    public IEnumerator Life(NumPlayType playType)
    {
        Vector3 posOffset = 0.01f * new Vector2(random.Next(-40, 40), random.Next(-20, 20));
        float scaleOffset = 0.01f * random.Next(75, 125);

        switch (playType)
        {
            case NumPlayType.Float:
                {
                    transform.position = transform.position + posOffset;
                    transform_Root.localScale = Vector3.zero;
                    transform_Root.DOScale(Vector3.one, 0.25f);
                    transform_Root.DOLocalMoveY(1f, 1f);
                    yield return new WaitForSeconds(.75f);
                    text_BackGround.DOFade(0, 0.25f);
                    text_Num.DOFade(0, 0.25f);
                    break;
                }
            case NumPlayType.Jump:
                {
                    transform.DOLocalJump(transform.position + posOffset, 0.5f, 1, 0.2f);
                    transform_Root.localScale = Vector3.zero;
                    transform_Root.DOScale(Vector3.one * scaleOffset, 0.2f).SetEase(Ease.OutBack);
                    yield return new WaitForSeconds(1f);
                    transform_Root.DOScale(Vector2.zero, 0.25f);
                    text_BackGround.DOFade(0, 0.25f);
                    text_Num.DOFade(0, 0.25f);
                    break;
                }
        }

    }
    public override void OnDisable()
    {
        Clean();
        base.OnDisable();
    }
}
public enum NumPlayType
{
    Float,
    Jump,
}