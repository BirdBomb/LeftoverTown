using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using static Fusion.Allocator;
using TMPro;
using UniRx;
using UnityEngine.Localization.Components;

public class ActorUI : MonoBehaviour
{
    public SpriteAtlas spriteAtlas_Emoji;
    private ActorManager actorManager_Bind;


    public void Bind(ActorManager actorManager)
    {
        actorManager_Bind = actorManager;
    }
    public void HideAllSingal()
    {
        ShowSingal_R(false);
        ShowSingal_Talk(false);
        ShowSingal_Emoji(false);
    }
    #region//Singal_Emoji
    private GameObject obj_SingalUI_Emoji;
    private SpriteRenderer spriteRenderer_Emoji;
    private EmojiQuest emojiQuest;
    private Coroutine emojiLoop;
    public void SendEmoji(Emoji emoji, float duration, bool isLoop, float distance)
    {
        emojiQuest.emoji = emoji;
        emojiQuest.loop = isLoop;
        emojiQuest.distance = distance;
        if (emojiLoop != null) { StopCoroutine(emojiLoop); }
        emojiLoop = StartCoroutine(EmojiLoop(duration));
    }
    private IEnumerator EmojiLoop(float duration)
    {
        do
        {
            ShowSingal_Emoji(true);
            yield return new WaitForSeconds(duration);
        }
        while (emojiQuest.loop);
        ShowSingal_Emoji(false);
    }
    private void ShowSingal_Emoji(bool on)
    {
        if (on)
        {
            HideAllSingal();
            obj_SingalUI_Emoji = obj_SingalUI_Emoji ? obj_SingalUI_Emoji : PoolManager.Instance.GetObject("UI/ActorUI/SignalEmoji");
            obj_SingalUI_Emoji.transform.SetParent(transform);
            obj_SingalUI_Emoji.transform.localPosition = Vector3.zero;
            obj_SingalUI_Emoji.transform.localScale = Vector3.one;
            obj_SingalUI_Emoji.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
            obj_SingalUI_Emoji.TryGetComponent(out spriteRenderer_Emoji);
            if (emojiQuest.emoji == Emoji.Talking)
            {
                int random = new System.Random().Next(0, 10);
                spriteRenderer_Emoji.sprite = spriteAtlas_Emoji.GetSprite($"Emoji_{(int)emojiQuest.emoji}_{random}");
            }
            else
            {
                spriteRenderer_Emoji.sprite = spriteAtlas_Emoji.GetSprite($"Emoji_{(int)emojiQuest.emoji}");
            }
            MessageBroker.Default.Publish(new GameEvent.GameEvent_AllClient_SomeoneSendEmoji
            {
                actor = actorManager_Bind,
                emoji = emojiQuest.emoji,
                distance = emojiQuest.distance,
            });
        }
        else
        {
            PoolManager.Instance.ReleaseObject("UI/ActorUI/SignalEmoji", obj_SingalUI_Emoji);
            obj_SingalUI_Emoji = null;
            spriteRenderer_Emoji = null;
        }
    }

    #endregion
    #region//Singal_Text
    private GameObject obj_SingalUI_Text;
    private SpriteRenderer spriteRenderer_Text;
    private TextQuest textQuest;
    private Coroutine textLoop;

    public void SendText(string text, Emoji emoji, float duration, bool isLoop, float distance)
    {
        textQuest.text = text;
        textQuest.emoji = emoji;
        textQuest.loop = isLoop;
        textQuest.distance = distance;
        if (textLoop != null) { StopCoroutine(textLoop); }
        textLoop = StartCoroutine(TextLoop(duration));

    }
    private IEnumerator TextLoop(float duration)
    {
        do
        {
            ShowSingal_Text(true);
            yield return new WaitForSeconds(duration);
        }
        while (textQuest.loop);
        ShowSingal_Text(false);
    }
    private void ShowSingal_Text(bool on)
    {
        if (on)
        {
            HideAllSingal();
            obj_SingalUI_Text = obj_SingalUI_Text ? obj_SingalUI_Text : PoolManager.Instance.GetObject("UI/ActorUI/SignalText");
            obj_SingalUI_Text.transform.SetParent(transform);
            obj_SingalUI_Text.transform.localPosition = Vector3.zero;
            obj_SingalUI_Text.transform.localScale = Vector3.one;
            obj_SingalUI_Text.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
            obj_SingalUI_Text.transform.Find("Canvas/Text").TryGetComponent(out TextMeshProUGUI text);
            text.text = textQuest.text;
            MessageBroker.Default.Publish(new GameEvent.GameEvent_AllClient_SomeoneSendEmoji
            {
                actor = actorManager_Bind,
                emoji = textQuest.emoji,
                distance = textQuest.distance,
            });
        }
        else
        {
            PoolManager.Instance.ReleaseObject("UI/ActorUI/SignalText", obj_SingalUI_Text);
            obj_SingalUI_Text = null;
            spriteRenderer_Emoji = null;
        }
    }

    #endregion
    #region//Singal_R
    private GameObject obj_SingalUI_R;
    public void ShowSingal_R(bool on)
    {
        if(on)
        {
            HideAllSingal();
            obj_SingalUI_R = obj_SingalUI_R ? obj_SingalUI_R : PoolManager.Instance.GetObject("UI/ActorUI/SignalR");
            obj_SingalUI_R.transform.SetParent(transform);
            obj_SingalUI_R.transform.localPosition = Vector3.zero;
            obj_SingalUI_R.transform.localScale = Vector3.one;
            obj_SingalUI_R.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);

        }
        else
        {
            PoolManager.Instance.ReleaseObject("UI/ActorUI/SignalR", obj_SingalUI_R);
            obj_SingalUI_R = null;
        }
    }
    #endregion
    #region//Singal_Talk
    private GameObject obj_SingalUI_Talk;
    public void ShowSingal_Talk(bool on)
    {
        if (on)
        {
            HideAllSingal();
            obj_SingalUI_Talk = obj_SingalUI_Talk ? obj_SingalUI_Talk : PoolManager.Instance.GetObject("UI/ActorUI/SignalTalk");
            obj_SingalUI_Talk.transform.SetParent(transform);
            obj_SingalUI_Talk.transform.localPosition = Vector3.zero;
            obj_SingalUI_Talk.transform.localScale = Vector3.one;
            obj_SingalUI_Talk.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
        }
        else
        {
            PoolManager.Instance.ReleaseObject("UI/ActorUI/SignalTalk", obj_SingalUI_Talk);
            obj_SingalUI_Talk = null;
        }
    }

    #endregion
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
    /// ¿ªÐÄ
    /// </summary>
    Happy,
    /// <summary>
    /// ²»¿ªÐÄ
    /// </summary>
    Unhappy,
    /// <summary>
    /// ¹¥»÷
    /// </summary>
    Attack,
    /// <summary>
    /// ËÑÑ°
    /// </summary>
    Search,
    /// <summary>
    /// ¿ªÊ¼ÏÐÁÄ
    /// </summary>
    Talking,
    /// <summary>
    /// ½áÊøÏÐÁÄ
    /// </summary>
    TalkEnd,
}
public struct EmojiQuest { public Emoji emoji; public bool loop; public float distance; }
public struct TextQuest { public string text;public Emoji emoji; public bool loop; public float distance; }