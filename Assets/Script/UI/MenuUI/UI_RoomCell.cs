using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_RoomCell : MonoBehaviour
{
    public Button btn_ShowRoom;
    public TextMeshProUGUI text_Name;
    public TextMeshProUGUI text_PlayerCount;
    [HideInInspector]
    public SessionInfo bindInfo;
    public Action<UI_RoomCell> bindAction;
    public void InitRoomCell(SessionInfo sessionInfo, Action<UI_RoomCell> action)
    {
        bindAction = action;
        bindInfo = sessionInfo;
        text_Name.text =sessionInfo.Name;
        text_PlayerCount.text = sessionInfo.PlayerCount.ToString();

        btn_ShowRoom.onClick.AddListener(ClickRoomCell);
    }
    private void ClickRoomCell()
    {
        if (bindAction!=null)
        {
            bindAction.Invoke(this);
        }
    }
}
