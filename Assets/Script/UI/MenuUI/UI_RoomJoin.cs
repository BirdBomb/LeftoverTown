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
    public TMPro.TextMeshProUGUI text_Warning;
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
        TryToJoinSessionLobby();
    }
    public void HidePanel()
    {
        transform_Panel.gameObject.SetActive(false);
    }
    #region//大厅操作

    public async void TryToJoinSessionLobby()
    {
        text_Warning.text = "请等待";
        StartGameResult startGameResult = await NetManager.Instance.JoinSessionLobby();
        if(startGameResult.Ok) { Refresh(); text_Warning.text = "成功连接"; }
        else
        {
            switch (startGameResult.ShutdownReason)
            {
                case ShutdownReason.Ok:
                    text_Warning.text = "应请求关闭";
                    break;
                case ShutdownReason.Error:
                    text_Warning.text = "内部错误";
                    break;
                case ShutdownReason.IncompatibleConfiguration:
                    text_Warning.text = "房间类型不匹配";
                    break;
                case ShutdownReason.ServerInRoom:
                    text_Warning.text = "房间已经存在服务器";
                    break;
                case ShutdownReason.DisconnectedByPluginLogic:
                    text_Warning.text = "插件逻辑断开连接或踢出";
                    break;
                case ShutdownReason.GameClosed:
                    text_Warning.text = "游戏已经结束";
                    break;
                case ShutdownReason.GameNotFound:
                    text_Warning.text = "游戏未找到";
                    break;
                case ShutdownReason.MaxCcuReached:
                    text_Warning.text = "达到CCU上限，联系鸟弹";
                    break;
                case ShutdownReason.InvalidRegion:
                    text_Warning.text = "区域不可用";
                    break;
                case ShutdownReason.GameIdAlreadyExists:
                    text_Warning.text = "已存在同名的会话";
                    break;
                case ShutdownReason.GameIsFull:
                    text_Warning.text = "房间满员";
                    break;
                case ShutdownReason.InvalidAuthentication:
                    text_Warning.text = "身份验证无效";
                    break;
                case ShutdownReason.CustomAuthenticationFailed:
                    text_Warning.text = "身份验证失败";
                    break;
                case ShutdownReason.AuthenticationTicketExpired:
                    text_Warning.text = "身份票据过期";
                    break;
                case ShutdownReason.PhotonCloudTimeout:
                    text_Warning.text = "云端连接超时......";
                    break;
                case ShutdownReason.AlreadyRunning:
                    text_Warning.text = "已在运行";
                    break;
                case ShutdownReason.InvalidArguments:
                    text_Warning.text = "Game参数不符合要求";
                    break;
                case ShutdownReason.HostMigration:
                    text_Warning.text = "Runner正在关闭即将发生主机迁移";
                    break;
                case ShutdownReason.ConnectionTimeout:
                    text_Warning.text = "与远程服务器的连接超时";
                    break;
                case ShutdownReason.ConnectionRefused:
                    text_Warning.text = "与远程服务器的连接拒绝";
                    break;
                case ShutdownReason.OperationTimeout:
                    text_Warning.text = "当前操作已超时";
                    break;
            }
        }
    }
    #endregion

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
