using Fusion;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UI_LobbyCreate : MonoBehaviour
{
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
    public Transform transform_Panel;
    public Button btn_Return;
    private void BindMain()
    {
        btn_Return.onClick.AddListener(HidePanel);
    }
    public void ShowPanel()
    {
        transform_Panel.gameObject.SetActive(true);
    }
    public void HidePanel()
    {
        transform_Panel.gameObject.SetActive(false);
    }
    #endregion
    #region//角色选择
    public List<UI_ActorChooseButton> uI_ActorChooseButtons = new List<UI_ActorChooseButton>();
    /// <summary>
    /// 刷新角色选择界面
    /// </summary>
    public void UpdateActorChooseUI()
    {
        for (int i = 0; i < uI_ActorChooseButtons.Count; i++)
        {
            uI_ActorChooseButtons[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < uI_ActorChooseButtons.Count; i++)
        {
            int index = i;
            string path = "PlayerData/Player" + index;
            string data =  FileManager.Instance.ReadFile(path);
            uI_ActorChooseButtons[index].gameObject.SetActive(true);
            uI_ActorChooseButtons[index].Init(data, path,
               (_) => { ShowActorShowPanel(_.bind_Data, _.bind_Path); },
               (_) => { ShowActorCreatePanel(_.bind_Path); },
               (_) => { UpdateActorChooseUI(); });
        }
    }
    #endregion
    #region//角色预览
    public UI_ActorShowPanel uI_ActorShowPanel;
    public void ShowActorShowPanel(string data, string path)
    {
        bind_ActorDataPath = path;
        uI_ActorShowPanel.Init(JsonConvert.DeserializeObject<PlayerData>(data));
        uI_ActorShowPanel.Show();
        uI_ActorCreatePanel.Hide();
    }
    #endregion
    #region//角色创建
    public UI_ActorCreatePanel uI_ActorCreatePanel;
    public void ShowActorCreatePanel(string path)
    {
        uI_ActorCreatePanel.Init(path, () =>
        {
            HideActorCreatePanel();
        });
        uI_ActorCreatePanel.Show();
        uI_ActorShowPanel.Hide();
    }
    public void HideActorCreatePanel()
    {
        UpdateActorChooseUI();
        uI_ActorCreatePanel.Hide();
    }
    #endregion
    #region//地图选择
    public List<UI_MapChooseButton> uI_MapChooseButtons = new List<UI_MapChooseButton>();
    public void UpdateMapChooseUI()
    {
        for (int i = 0; i < uI_MapChooseButtons.Count; i++)
        {
            uI_MapChooseButtons[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < uI_MapChooseButtons.Count; i++)
        {
            int index = i;
            uI_MapChooseButtons[index].gameObject.SetActive(true);
            uI_MapChooseButtons[index].Init(index,
               (_) => { ShowMapShowPanel(_.bind_MapInfoData, _.bind_MapInfoPath, _.bind_BuildingInfoPath, _.bind_BuildingTypePath, _.bind_FloorTypePath); },
               (_) => { ShowMapCreatePanel(_.bind_MapInfoPath, _.bind_BuildingInfoPath, _.bind_BuildingTypePath, _.bind_FloorTypePath); },
               (_) => { UpdateMapChooseUI(); });
        }
    }

    #endregion
    #region//地图预览
    public UI_MapShowPanel uI_MapShowPanel;
    public void ShowMapShowPanel(string mapInfoData,string mapInfoPath, string mapBuildInfoPath, string mapBuildTypePath,string mapFloorTypePath)
    {
        MapInfoData data = JsonConvert.DeserializeObject<MapInfoData>(mapInfoData);
        bind_MapInfoPath = mapInfoPath;
        bind_BuildingInfoPath = mapBuildInfoPath;
        bind_BuildingTypePath = mapBuildTypePath; 
        bind_FloorTypePath = mapFloorTypePath;
        uI_MapShowPanel.Init(data.name, data.seed);
        uI_MapShowPanel.Show();
        uI_ActorCreatePanel.Hide();
    }
    #endregion
    #region//地图创建
    public UI_MapCreatePanel uI_MapCreatePanel;
    public void ShowMapCreatePanel(string mapInfoPath,string mapBuildingInfoPath, string mapBuildingTypePath, string mapFloorTypePath)
    {
        uI_MapShowPanel.Hide();
        uI_MapCreatePanel.Show();
        uI_MapCreatePanel.Init(mapInfoPath, mapBuildingInfoPath, mapBuildingTypePath, mapFloorTypePath, () =>
        {
            HideMapCreatePanel();
        });
    }
    public void HideMapCreatePanel()
    {
        UpdateMapChooseUI();
        uI_MapCreatePanel.Hide();
    }

    #endregion
    #region//游戏房间设置

    public TMP_InputField input_RoomName;
    public TMP_Dropdown dropdown_RoomType;
    public Button btn_CreateRoom;

    private string roomName;
    private string bind_ActorDataPath = "";
    private string bind_MapInfoPath = "";
    private string bind_BuildingInfoPath = "";
    private string bind_BuildingTypePath = "";
    private string bind_FloorTypePath = "";

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
            GameDataManager.Instance.bind_MapInfoPath = bind_MapInfoPath;
            GameDataManager.Instance.bind_MapBuildingTypeFilePath = bind_BuildingTypePath;
            GameDataManager.Instance.bind_MapBuildingInfoFilePath = bind_BuildingInfoPath;
            GameDataManager.Instance.bind_MapFloorTypeFilePath = bind_FloorTypePath;
            GameDataManager.Instance.bind_PlayerDataPath = bind_ActorDataPath;

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
        if (roomName != ""&& bind_BuildingTypePath!=""&& bind_BuildingInfoPath != "" && bind_FloorTypePath != "" && bind_ActorDataPath != "")
        {
            return true;
        }
        return false;
    }
    #endregion
}
