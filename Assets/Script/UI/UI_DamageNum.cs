using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class UI_DamageNum : MonoBehaviour
{
    [SerializeField]
    private Text Num;

    public void Play(int num,Color32 color)
    {
        Num.DOKill();

        if (IsInvoking("Ending")) { CancelInvoke("Ending"); }
        Num.text = num.ToString();
        Num.color = color;
        Num.transform.localScale = Vector3.zero;
        Num.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            Num.DOFade(0, 0.5f);
        });
        Invoke("Ending",1);
    }
    public void Ending()
    {
        UIManager.Instance.HideUI("UI/UI_DamageNum", this.gameObject);
    }
}
