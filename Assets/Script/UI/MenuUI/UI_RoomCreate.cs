using DG.Tweening;
using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UI_RoomCreate : MonoBehaviour
{
    public Transform transform_CreateRoom;
    public TMP_InputField input_RoomName;
    public TMP_Dropdown dropdown_RoomType;
    public Button btn_Create;
    public Button btn_Close;
    public TextMeshProUGUI text_Warning;
    private string roomName = "";
    private int roomType;
    private int bind_MapIndex;
    private string bind_ActorPath;
    private Action action_Close;
    public void Init(string actorPath, int mapIndex)
    {
        bind_MapIndex = mapIndex;
        bind_ActorPath = actorPath;
    }
    public void Bind(Action actionClose)
    {
        input_RoomName.onValueChanged.AddListener(ChangeRoomName);
        dropdown_RoomType.onValueChanged.AddListener(ChangeRoomType);
        action_Close = actionClose;
        btn_Create.onClick.AddListener(Create);
        btn_Close.onClick.AddListener(Close);
    }
    public void ChangeRoomType(int val)
    {
        roomType = val;
    }
    public void ChangeRoomName(string val)
    {
        roomName = val;
        CheckRoomSetting();
    }
    public void ShowPanel()
    {
        transform_CreateRoom.gameObject.SetActive(true);
        transform_CreateRoom.transform.DOPunchScale(new Vector3(0.1f, -0.1f, 0), 0.1f);
        CheckRoomSetting();
    }
    public void HidePanel()
    {
        transform_CreateRoom.gameObject.SetActive(false);
    }
    private bool CheckRoomSetting()
    {
        if (!roomName.Equals("") && !bind_ActorPath.Equals("") && bind_MapIndex != -1)
        {
            btn_Create.interactable = true;
            return true;
        }
        else
        {
            btn_Create.interactable = false;
            return false;
        }
    }
    private async void Create()
    {
        if (CheckRoomSetting())
        {
            GameDataManager.Instance.bind_MapInfoPath = "MapData/MapInfo" + bind_MapIndex;
            GameDataManager.Instance.bind_MapBuildingTypeFilePath = "MapData/MapBuildingType" + bind_MapIndex;
            GameDataManager.Instance.bind_MapBuildingInfoFilePath = "MapData/MapBuildingInfo" + bind_MapIndex;
            GameDataManager.Instance.bind_MapFloorTypeFilePath = "MapData/MapFloorType" + bind_MapIndex;
            GameDataManager.Instance.bind_PlayerDataPath = bind_ActorPath;


            text_Warning.text = "请等待";
            StartGameResult startGameResult = await NetManager.Instance.CreateRoom(roomName, roomType, 4);
            if (!startGameResult.Ok)
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
        else
        {
            Debug.Log("房间设置未完成");
        }
    }
    private void Close()
    {
        action_Close.Invoke();
    }

}
