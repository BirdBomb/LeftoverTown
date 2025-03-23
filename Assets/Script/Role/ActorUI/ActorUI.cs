using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using static Fusion.Allocator;
using TMPro;
public class ActorUI : MonoBehaviour
{
    public Transform HpPanel;
    public Transform HPBar;
    public SpriteRenderer HPColor;
    public Transform EnPanel;
    public Transform EnBar;
    public SpriteRenderer EnColor;
    public Transform EnReleaseBar;
    public TextMeshPro textMeshPro_Name;
    public TextMeshPro textMeshPro_Text;

    public SpriteRenderer Emoji;
    private SpriteAtlas _emojiAtlas;
    private void Start()
    {
        _emojiAtlas = Resources.Load<SpriteAtlas>("Atlas/EmojiSprite");
    }
    public void UpdateHPBar(float val)
    {
        HPBar.localScale = new Vector3(val, 1, 1);
    }
    public void HighLightENBar()
    {
        EnBar.DOPunchScale(Vector3.one * 0.1f, 0.1f);
    }
    public void UpdateENBar(float val)
    {
        EnBar.localScale = new Vector3(val, 1, 1);
        EnColor.color = new Color(1 - val, 1, 0, 1);
    }
    public void UpdateENReleaseBar(float val)
    {
        EnReleaseBar.localScale = new Vector3(val, 1, 1);
    }
    public void SendEmoji(Emoji emoji)
    {
        Emoji.transform.DOKill();
        Emoji.transform.localScale = Vector3.zero;
        Emoji.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack);
        Emoji.sprite = _emojiAtlas.GetSprite("Emoji_" + (int)emoji);
        textMeshPro_Text.text = ""; 
        if (IsInvoking("ResetEmoji"))
        {
            CancelInvoke("ResetEmoji");
        }
        Invoke("ResetEmoji", 1);
    }
    public void SendText(string text)
    {
        Emoji.transform.DOKill();
        Emoji.transform.localScale = Vector3.zero;
        Emoji.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack);
        Emoji.sprite = _emojiAtlas.GetSprite("Emoji_Empty");
        textMeshPro_Text.text = text;
        if (IsInvoking("ResetEmoji"))
        {
            CancelInvoke("ResetEmoji");
        }
        Invoke("ResetEmoji", 1);
    }
    private void ResetEmoji()
    {
        Emoji.transform.DOScale(Vector3.zero, 0.25f);
        textMeshPro_Text.text = "";
    }
    public void ShowName(string str)
    {
        textMeshPro_Name.text = str;
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
}