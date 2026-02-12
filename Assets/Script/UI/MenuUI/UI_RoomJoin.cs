using DG.Tweening;
using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UI_RoomJoin : MonoBehaviour
{
    public Transform transform_Panel;
    public Transform transform_Pool;
    public Button btn_Refresh;
    public Button btn_Close;
    public GameObject prefab_RoomCell;
    private List<SessionInfo> sessionInfos = new List<SessionInfo>();
    private List<UI_RoomCell> roomCells = new List<UI_RoomCell>();
    private Action action_Close;
    private string bind_ActorPath;
    public UI_RoomShow roomShow;
    public void Init(string actorPath)
    {
        bind_ActorPath = actorPath;
    }
    public void Bind(Action actionClose)
    {
        action_Close = actionClose;
        MessageBroker.Default.Receive<NetEvent.NetEvent_SessionListUpdated>().Subscribe(_ =>
        {
            sessionInfos.Clear();
            foreach (var sessionInfo in _.SessionList)
            {
                sessionInfos.Add(sessionInfo);
            }
        }).AddTo(this);

        btn_Refresh.onClick.AddListener(Refresh);
        btn_Close.onClick.AddListener(Close);
        roomShow.Bind(ShowPanel, () =>
        {
            GameDataManager.Instance.bind_PlayerDataPath = bind_ActorPath;
        });
    }
    public void ShowPanel()
    {
        transform_Panel.gameObject.SetActive(true);
        transform_Panel.transform.DOPunchScale(new Vector3(0.1f, -0.1f, 0), 0.1f);
        NetManager.Instance.JoinSessionLobby();
        Refresh();
    }
    public void HidePanel()
    {
        transform_Panel.gameObject.SetActive(false);
    }
    public void Refresh()
    {
        Debug.Log("更新大厅列表UI" + sessionInfos.Count);
        ClearLobby();
        DrawLobby();
    }
    private void ClearLobby()
    {
        foreach (UI_RoomCell cell in roomCells)
        {
            Destroy(cell.gameObject);
        }
        roomCells.Clear();
    }
    private void DrawLobby()
    {
        foreach (SessionInfo sessionInfo in sessionInfos)
        {
            GameObject obj = Instantiate(prefab_RoomCell, transform_Pool);
            obj.GetComponent<UI_RoomCell>().InitRoomCell(sessionInfo, (_) =>
            {
                HidePanel();
                roomShow.ShowPanel(_.bindInfo);
            });
            roomCells.Add(obj.GetComponent<UI_RoomCell>());
        }
    }
    public void Close()
    {
        if(action_Close != null)
        {
            action_Close.Invoke();
        }
    }
}
