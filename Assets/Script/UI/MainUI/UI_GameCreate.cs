using Fusion;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameCreate : MonoBehaviour
{
    public Button btn_Return;
    private void Start()
    {
        Bind();
    }
    private void Bind()
    {
        BindMain();
        BindRoomSettingPanel();
    }
    #region//主要
    public Transform Panel;
    private void BindMain()
    {
        btn_Return.onClick.AddListener(HidePanel);
    }
    public void ShowPanel()
    {
        Panel.gameObject.SetActive(true);
    }
    public void HidePanel()
    {
        Panel.gameObject.SetActive(false);
    }
    #endregion
    #region//角色选择
    public List<UI_ActorChooseButton> ActorChooseButtonList = new List<UI_ActorChooseButton>();
    /// <summary>
    /// 刷新角色选择界面
    /// </summary>
    public void UpdateActorChooseUI()
    {
        for (int i = 0; i < ActorChooseButtonList.Count; i++)
        {
            ActorChooseButtonList[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < ActorChooseButtonList.Count; i++)
        {
            int index = i;
            string data =  FileManager.Instance.ReadFile("PlayerData/Player" + index);
            ActorChooseButtonList[index].gameObject.SetActive(true);
            ActorChooseButtonList[index].Init(data, "PlayerData/Player" + index, 
               (_) =>
            {
                ShowActorShowPanel(_.bind_Data, _.bind_Path);
            }, (_) =>
            {
                ShowActorCreatePanel(_.bind_Path);
            }, (_) =>
            {
                UpdateActorChooseUI();
            });
        }
    }
    #endregion
    #region//角色预览
    public UI_ActorShowPanel ActorShowPanel = new UI_ActorShowPanel();
    public void ShowActorShowPanel(string playerData,string playerPath)
    {
        ActorCreatePanel.Hide();
        ActorShowPanel.Init(JsonConvert.DeserializeObject<PlayerData>(playerData));
        actorDataPath = playerPath;
    }
    #endregion
    #region//角色创建
    public UI_ActorCreatePanel ActorCreatePanel = new UI_ActorCreatePanel();
    public void ShowActorCreatePanel(string path)
    {
        ActorShowPanel.Hide();
        ActorCreatePanel.Show();
        ActorCreatePanel.Init(path, () => 
        {
            HideActorCreatePanel();
        });
    }
    public void HideActorCreatePanel()
    {
        UpdateActorChooseUI();
        ActorCreatePanel.Hide();
    }
    #endregion
    #region//地图选择
    //public List<UI_ActorChooseButton> ActorChooseButtonList = new List<UI_ActorChooseButton>();
    #endregion
    #region//地图预览
    #endregion
    #region//游戏房间设置

    public TMP_InputField input_RoomName;
    public TMP_Dropdown dropdown_RoomType;
    public Button btn_CreateRoom;

    private string roomName;
    private string actorDataPath;
    private string buildInfoPath = "MapData/SaveData_BuildingTileInfo_Test";
    private string buildTypePath = "MapData/SaveData_BuildingTileType_Test";
    private string floorTypePath = "MapData/SaveData_FloorTileType_Test";

    private int roomType;
    private void BindRoomSettingPanel()
    {
        btn_CreateRoom.onClick.AddListener(CreateRoom);
        dropdown_RoomType.onValueChanged.AddListener(ChangeRoomType);
        input_RoomName.onValueChanged.AddListener(ChangeRoomName);
    }
    private void ChangeRoomType(int val)
    {
        roomType = val;
    }
    private void ChangeRoomName(string val)
    {
        roomName = val;
    }
    private void CreateRoom()
    {
        if (CheckRoomSetting())
        {
            GameDataManager.Instance.mapBuildingTypeFilePath = buildTypePath;
            GameDataManager.Instance.mapBuildingInfoFilePath = buildInfoPath;
            GameDataManager.Instance.mapFloorTypeFilePath = floorTypePath;
            GameDataManager.Instance.actorFilePath = actorDataPath;

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
    private bool CheckRoomSetting()
    {
        if (roomName != "")
        {
            return true;
        }
        return false;
    }
    #endregion

}
