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

    public SpriteAtlas spriteAtlas_Emoji;
    public SpriteRenderer spriteRenderer_Emoji;
    public Transform transform_Singal;

    public void UpdateHPBar(float val)
    {
        transform_HPBar.localScale = new Vector3(val, 1, 1);
    }
    public void SendEmoji(Emoji emoji)
    {
        spriteRenderer_Emoji.transform.DOKill();
        spriteRenderer_Emoji.transform.localScale = Vector3.zero;
        spriteRenderer_Emoji.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack);
        spriteRenderer_Emoji.sprite = spriteAtlas_Emoji.GetSprite("Emoji_" + (int)emoji);
        textMeshPro_Text.text = ""; 
        if (IsInvoking("ResetEmoji"))
        {
            CancelInvoke("ResetEmoji");
        }
        Invoke("ResetEmoji", 1);
    }
    public void SendText(string text)
    {
        spriteRenderer_Emoji.transform.DOKill();
        spriteRenderer_Emoji.transform.localScale = Vector3.zero;
        spriteRenderer_Emoji.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack);
        spriteRenderer_Emoji.sprite = spriteAtlas_Emoji.GetSprite("Emoji_Empty");
        textMeshPro_Text.text = text;
        if (IsInvoking("ResetEmoji"))
        {
            CancelInvoke("ResetEmoji");
        }
        Invoke("ResetEmoji", 1);
    }
    private void ResetEmoji()
    {
        spriteRenderer_Emoji.transform.DOScale(Vector3.zero, 0.25f);
        textMeshPro_Text.text = "";
    }
    public void ShowName(string str)
    {
        textMeshPro_Name.text = str;
    }
    public void ShowSingal(bool on)
    {
        transform_Singal.gameObject.SetActive(on);
        transform_Singal.transform.localScale = Vector3.one;
        transform_Singal.DOPunchScale(new Vector3(-0.1f, 0.1f, 0), 0.2f);
    }
}
public enum Emoji
{
    /// <summary>
    /// 震惊
    /// </summary>
    Shock,
    /// <summary>
    /// 困惑
    /// </summary>
    Puzzled,
    /// <summary>
    /// 大叫
    /// </summary>
    Yell,
    /// <summary>
    /// 问候
    /// </summary>
    Greeting,
    /// <summary>
    /// 威胁
    /// </summary>
    Menace,
    /// <summary>
    /// 恐慌
    /// </summary>
    Panic,
    /// <summary>
    /// 不开心
    /// </summary>
    Unhappy,
    /// <summary>
    /// 开心
    /// </summary>
    Happy,
}