using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using static Fusion.Allocator;
using TMPro;
public class ActorUI : MonoBehaviour
{
    public Transform transform_HpPanel;
    public Transform transform_HPBar;
    public TextMeshPro textMeshPro_Name;
    public TextMeshPro textMeshPro_Text;

    public Transform transform_Bubble;
    public Transform transform_Emoji;
    public SpriteAtlas spriteAtlas_Emoji;
    public SpriteRenderer spriteRenderer_Emoji;
    public Transform transform_Singal;
    public Transform transform_SingalF;
    public Transform transform_SingalTalk;

    public void UpdateHPBar(float val)
    {
        transform_HPBar.localScale = new Vector3(val, 1, 1);
    }
    public void SendEmoji(Emoji emoji)
    {
        if (!transform_Singal.gameObject.activeSelf)
        {
            transform_Emoji.transform.DOKill();
            transform_Emoji.transform.localScale = Vector3.zero;
            transform_Emoji.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack);
            spriteRenderer_Emoji.sprite = spriteAtlas_Emoji.GetSprite("Emoji_" + (int)emoji);
            textMeshPro_Text.text = "";
            if (IsInvoking("ResetEmoji"))
            {
                CancelInvoke("ResetEmoji");
            }
            Invoke("ResetEmoji", 1);
        }
    }
    private void ResetEmoji()
    {
        transform_Emoji.transform.DOScale(Vector3.zero, 0.25f);
        textMeshPro_Text.text = "";
    }
    public void SendText(string text)
    {
        if (!transform_Singal.gameObject.activeSelf)
        {
            transform_Bubble.transform.DOKill();
            transform_Bubble.transform.localScale = Vector3.zero;
            transform_Bubble.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack);
            textMeshPro_Text.text = text;
            if (IsInvoking("ResetText"))
            {
                CancelInvoke("ResetText");
            }
            Invoke("ResetText", 1);
        }
    }
    private void ResetText()
    {
        transform_Bubble.transform.DOScale(Vector3.zero, 0.25f);
        textMeshPro_Text.text = "";
    }
    public void ShowName(string str)
    {
        if (!str.Equals(""))
        {
            textMeshPro_Name.text = str;
        }
    }
    public void ShowSingal()
    {
        ResetEmoji();
        ResetText();
        transform_Singal.DOKill();
        transform_Singal.gameObject.SetActive(true);
        transform_Singal.transform.localScale = Vector3.one;
        transform_Singal.DOPunchScale(new Vector3(-0.1f, 0.1f, 0), 0.2f);
        if (IsInvoking("HideSingal"))
        {
            CancelInvoke("HideSingal");
        }
        Invoke("HideSingal", 5);
    }
    public void ShowSingalF()
    {
        ResetEmoji();
        ResetText();
        transform_Singal.DOKill();
        transform_Singal.gameObject.SetActive(true);
        transform_SingalF.gameObject.SetActive(true);
        transform_SingalTalk.gameObject.SetActive(false);
        transform_Singal.transform.localScale = Vector3.one;
        transform_Singal.DOPunchScale(new Vector3(-0.1f, 0.1f, 0), 0.2f);
    }
    public void ShowSingalTalk()
    {
        ResetEmoji();
        ResetText();
        transform_Singal.DOKill();
        transform_Singal.gameObject.SetActive(true);
        transform_SingalTalk.gameObject.SetActive(true);
        transform_SingalF.gameObject.SetActive(false);
        transform_Singal.transform.localScale = Vector3.one;
        transform_Singal.DOPunchScale(new Vector3(-0.1f, 0.1f, 0), 0.2f);
    }
    public void HideSingal()
    {
        transform_Singal.gameObject.SetActive(false);
        textMeshPro_Text.text = "";
    }
}
public enum Emoji
{
    /// <summary>
    /// Õð¾ª
    /// </summary>
    Shock,
    /// <summary>
    /// À§»ó
    /// </summary>
    Puzzled,
    /// <summary>
    /// ´ó½Ð
    /// </summary>
    Yell,
    /// <summary>
    /// ÎÊºò
    /// </summary>
    Greeting,
    /// <summary>
    /// ÍþÐ²
    /// </summary>
    Menace,
    /// <summary>
    /// ¿Ö»Å
    /// </summary>
    Panic,
    /// <summary>
    /// ²»¿ªÐÄ
    /// </summary>
    Unhappy,
    /// <summary>
    /// ¿ªÐÄ
    /// </summary>
    Happy,
    /// <summary>
    /// ¹¥»÷
    /// </summary>
    Attack,
    /// <summary>
    /// ËÑÑ°
    /// </summary>
    Search,
}