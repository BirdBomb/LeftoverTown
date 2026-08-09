using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class TileUI_DialogOption : MonoBehaviour
{
    public Transform panel;
    public Button button;
    public LocalizeStringEvent localizeStringEvent;
    public UI_ShakeText shakeText; 
    private Action action_Bind;
    public void Init(string nameTable, string nameEntry, Action action)
    {
        Show();
        button.onClick.AddListener(Click);
        localizeStringEvent.StringReference.SetReference(nameTable, nameEntry);
        shakeText.ReShake();
        action_Bind = action;
    }
    public void Show()
    {
        button.interactable = true;
        panel.gameObject.SetActive(true);
        panel.DOKill();
        panel.localScale = Vector3.one;
        panel.DOPunchScale(new Vector3(0.1f, -0.1f, 0), 0.1f);
    }
    public void Hide()
    {
        button.interactable = false;
        panel.gameObject.SetActive(false);
        action_Bind = null;
    }
    private void Click()
    {
        if (action_Bind != null) { action_Bind(); }
    }
}
