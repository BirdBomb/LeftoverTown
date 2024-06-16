using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using static Fusion.Allocator;

public class ActorUI : MonoBehaviour
{
    public Transform HpPanel;
    public Transform HPBar;
    public Transform EnPanel;
    public Transform EnBar;
    public Transform EnReleaseBar;

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
    }
    public void UpdateENReleaseBar(float val)
    {
        EnReleaseBar.localScale = new Vector3(val, 1, 1);
    }
    public void SendEmoji(EmojiConfig emoji)
    {
        Emoji.transform.DOKill();
        Emoji.transform.localScale = Vector3.zero;
        Emoji.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack);
        Emoji.sprite = _emojiAtlas.GetSprite("Emoji_" + emoji.Emoji_ID);
        if (IsInvoking("ResetEmoji"))
        {
            CancelInvoke("ResetEmoji");
        }
        Invoke("ResetEmoji",1);
    }
    private void ResetEmoji()
    {
        Emoji.transform.DOScale(Vector3.zero, 0.25f);
    }
}
