using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class TileUI_DialogOption : MonoBehaviour
{
    [SerializeField]
    private Transform panel;
    [SerializeField]
    private Button button;
    [SerializeField]
    private LocalizeStringEvent localizeStringEvent;
    private Action action_Bind;
    public void Init(string nameTable, string nameEntry, Action action)
    {
        panel.gameObject.SetActive(true);
        panel.DOKill();
        panel.localScale = Vector3.one;
        panel.DOPunchScale(new Vector3(0.1f, -0.1f, 0), 0.1f);

        button.onClick.AddListener(Click);
        localizeStringEvent.StringReference.SetReference(nameTable, nameEntry);
        action_Bind = action;
    }
    private void Click()
    {
        if (action_Bind != null) { action_Bind(); }
    }
}
