using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UniRx;

public class UI_InputText : MonoBehaviour
{
    public TMP_InputField text_Input;
    public Button btn_Send;
    private string info;    
    private void Start()
    {
        btn_Send.onClick.AddListener(Send);
        text_Input.onValueChanged.AddListener(Change);
    }
    private void Send()
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_SendText()
        {
            text = info,
        });
    }
    private void Change(string str)
    {
        info = str;
    }
}
