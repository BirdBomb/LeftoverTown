using DG.Tweening;
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
    private void Create()
    {
        if (CheckRoomSetting())
        {
            GameDataManager.Instance.bind_MapInfoPath = "MapData/MapInfo" + bind_MapIndex;
            GameDataManager.Instance.bind_MapBuildingTypeFilePath = "MapData/MapBuildingType" + bind_MapIndex;
            GameDataManager.Instance.bind_MapBuildingInfoFilePath = "MapData/MapBuildingInfo" + bind_MapIndex;
            GameDataManager.Instance.bind_MapFloorTypeFilePath = "MapData/MapFloorType" + bind_MapIndex;
            GameDataManager.Instance.bind_PlayerDataPath = bind_ActorPath;

            MessageBroker.Default.Publish(new NetEvent.NetEvent_CreateGame()
            {
                RoomName = roomName,
                RoomType = roomType,
            });
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
