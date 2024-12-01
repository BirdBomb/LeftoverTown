using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UniRx;
using Fusion;

public class UI_Text : MonoBehaviour
{
    public Text UI_text;
    public Image UI_back;
    bool show = false;
    private void Start()
    {
        MessageBroker.Default.Receive<UIEvent.UIEvent_ShowGlobalTextUI>().Subscribe(_ =>
        {
            ShowText(_.text);
        }).AddTo(this);
    }
    private void ShowText(string str)
    {
        show = true;
        UI_text.text = str;
        UI_back.enabled = true;
    }
    private void HideText()
    {
        show = false;
        UI_text.text = "";
        UI_back.enabled = false;
    }
    private void Update()
    {
        if (show && Input.GetKeyDown(KeyCode.Mouse0))
        {
            HideText();
        }
    }
}
