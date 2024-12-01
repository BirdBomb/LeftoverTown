using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using TMPro;
using static UnityEditor.PlayerSettings;
using UnityEngine.UI;
using DG.Tweening;

public class UI_InfoPanel : MonoBehaviour
{
    [SerializeField]
    private CanvasScaler canvasScaler_Panel;
    [SerializeField]
    private RectTransform rectTransform_Info;
    [SerializeField]
    private RectTransform rectTransform_BG;
    [SerializeField]
    private TextMeshProUGUI text_Info;
    private Vector2 vector_InfoPos;
    private Vector2 vector_InfoOffset;
    void Start()
    {
        MessageBroker.Default.Receive<UIEvent.UIEvent_ShowInfoTextUI>().Subscribe(_ =>
        {
            if (_.text.Length > 0)
            {
                ShowInfoText(_.text);
                ShakeInfoText();
                AdaptingInfoText(_.anchor);
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<UIEvent.UIEvent_HidenfoTextUI>().Subscribe(_ =>
        {
            HideInfoText();
        }).AddTo(this);
    }
    private void ShakeInfoText()
    {
        rectTransform_Info.DOKill();
        rectTransform_Info.localScale = Vector3.one;
        rectTransform_Info.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.2f);
    }
    private void ShowInfoText(string info)
    {
        text_Info.gameObject.SetActive(true);
        text_Info.text = info;
    }
    private void HideInfoText()
    {
        text_Info.gameObject.SetActive(false);
        text_Info.text = "";
    }
    private void AdaptingInfoText(Vector2 anchor)
    {
        vector_InfoPos = anchor;
        vector_InfoOffset = Vector2.zero;
        if (vector_InfoPos.x < Camera.main.scaledPixelWidth * 0.75f)
        {
            float x = rectTransform_BG.rect.width * Camera.main.scaledPixelWidth / canvasScaler_Panel.referenceResolution.x;
            vector_InfoOffset += new Vector2(x * 0.5f, 0);
        }
        else
        {
            float x = rectTransform_BG.rect.width * Camera.main.scaledPixelWidth / canvasScaler_Panel.referenceResolution.x;
            vector_InfoOffset -= new Vector2(x * 0.5f, 0);
        }
        if (vector_InfoPos.y < Camera.main.scaledPixelHeight * 0.25f)
        {
            float y = rectTransform_BG.rect.height * Camera.main.scaledPixelHeight / canvasScaler_Panel.referenceResolution.y;
            vector_InfoOffset += new Vector2(0, y * 0.5f);
        }
        else
        {
            float y = rectTransform_BG.rect.height * Camera.main.scaledPixelHeight / canvasScaler_Panel.referenceResolution.y;
            vector_InfoOffset -= new Vector2(0, y * 0.5f);
        }
        rectTransform_Info.position = vector_InfoPos + vector_InfoOffset;
    }
}
