using DG.Tweening;
using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UI_RoomShow : MonoBehaviour
{
    public Transform transform_Panel;
    public TextMeshProUGUI text_JoinRoomName;
    public Button btn_Close;
    public Button btn_Join;
    private Action action_Join;
    private Action action_Close;
    private SessionInfo bind_SessionInfo;

    public void Bind(Action actionClose,Action actionJoin)
    {
        action_Join = actionJoin;
        action_Close = actionClose;
        btn_Close.onClick.AddListener(Close);
        btn_Join.onClick.AddListener(Join); 
    }
    public void ShowPanel(SessionInfo sessionInfo)
    {
        bind_SessionInfo = sessionInfo;
        text_JoinRoomName.text = sessionInfo.Name;
        transform_Panel.gameObject.SetActive(true);
        transform_Panel.transform.DOPunchScale(new Vector3(0.1f, -0.1f, 0), 0.1f);
    }
    public void HidePanel()
    {
        transform_Panel.gameObject.SetActive(false);
    }
    public void Close()
    {
        if(action_Close != null) { action_Close.Invoke(); }
        HidePanel();
    }
    public void Join()
    {
        if (action_Join != null) { action_Join.Invoke(); }
        MessageBroker.Default.Publish(new NetEvent.NetEvent_JoinGame()
        {
            RoomInfo = bind_SessionInfo,
            RoomName = bind_SessionInfo.Name,
        });
    }
}
