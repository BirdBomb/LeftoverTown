using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UI_BossInfo : SingleTon<UI_BossInfo>,ISingleTon
{
    [SerializeField, Header("背景")]
    private GameObject gameObject_BackGround;
    [SerializeField, Header("名称")]
    private TextMeshProUGUI textMeshProUGUI_Name;
    [SerializeField, Header("生命值")]
    private Text text_Hp;
    [SerializeField, Header("生命条")]
    private Transform tran_HpBar;

    public void Init()
    {
        
    }
    public void Show()
    {
        CancelInvoke("Hide");
        Invoke("Hide", 15f);
        if (!gameObject_BackGround.activeSelf)
        {
            gameObject_BackGround.transform.DOKill();
            gameObject_BackGround.SetActive(true);
            gameObject_BackGround.transform.localScale = Vector3.one;
            gameObject_BackGround.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
        }
    }
    public void Hide()
    {
        if (gameObject_BackGround.activeSelf)
        {
            gameObject_BackGround.transform.DOKill();
            gameObject_BackGround.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
            {
                gameObject_BackGround.SetActive(false);
            });
        }
    }
    public void UpdateHpBar(float val)
    {
        tran_HpBar.DOKill();
        tran_HpBar.DOScaleX(val, 0.1f);
    }
    public void UpdateHpVal(int hp)
    {
        text_Hp.text = hp.ToString();
    }
    public void UpdateName(string str)
    {
        textMeshProUGUI_Name.text = str.ToString();
    }
}
