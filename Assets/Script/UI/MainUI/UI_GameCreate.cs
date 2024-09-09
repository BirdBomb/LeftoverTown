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
    #region//��Ҫ
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
    #region//��ɫѡ��
    public List<UI_ActorChooseButton> ActorChooseButtonList = new List<UI_ActorChooseButton>();
    /// <summary>
    /// ˢ�½�ɫѡ�����
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
    #region//��ɫԤ��
    public UI_ActorShowPanel ActorShowPanel = new UI_ActorShowPanel();
    public void ShowActorShowPanel(string playerData,string playerPath)
    {
        ActorCreatePanel.Hide();
        ActorShowPanel.Init(JsonConvert.DeserializeObject<PlayerData>(playerData));
        actorDataPath = playerPath;
    }
    #endregion
    #region//��ɫ����
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
    #region//��ͼѡ��
    public List<UI_MapChooseButton> MapChooseButtonList = new List<UI_MapChooseButton>();
    public void UpdateMapChooseUI()
    {
        for (int i = 0; i < MapChooseButtonList.Count; i++)
        {
            MapChooseButtonList[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < MapChooseButtonList.Count; i++)
        {
            int index = i;
            string data = FileManager.Instance.ReadFile("MapData/MapBuildInfo" + index);
            MapChooseButtonList[index].gameObject.SetActive(true);
            MapChooseButtonList[index].Init(data,
                "MapData/MapBuildInfo" + index,
                "MapData/MapBuildType" + index,
                "MapData/MapFloorType" + index,
               (_) =>
               {
                   ShowMapShowPanel(_.bind_Data, _.bind_BuildInfoPath,_.bind_BuildTypePath,_.bind_FloorTypePath);
               }, (_) =>
               {
                   ShowMapCreatePanel(_.bind_BuildInfoPath, _.bind_BuildTypePath, _.bind_FloorTypePath);
               }, (_) =>
               {
                   UpdateMapChooseUI();
               });
        }
    }

    #endregion
    #region//��ͼԤ��
    public UI_MapShowPanel MapShowPanel = new UI_MapShowPanel();
    public void ShowMapShowPanel(string mapBuildInfoData, string mapBuildInfoPath, string mapBuildTypePath,string mapFloorTypePath)
    {
        ActorCreatePanel.Hide();
        MapTileInfoData data = JsonConvert.DeserializeObject<MapTileInfoData>(mapBuildInfoData);
        MapShowPanel.Init(data.name, data.seed);
        buildInfoPath = mapBuildInfoPath;
        buildTypePath = mapBuildTypePath; 
        floorTypePath = mapFloorTypePath;
    }
    #endregion
    #region//��ͼ����
    public UI_MapCreatePanel MapCreatePanel = new UI_MapCreatePanel();
    public void ShowMapCreatePanel(string mapBuildInfoPath, string mapBuildTypePath, string mapFloorTypePath)
    {
        MapShowPanel.Hide();
        MapCreatePanel.Show();
        MapCreatePanel.Init(mapBuildInfoPath, mapBuildTypePath, mapFloorTypePath, () =>
        {
            HideMapCreatePanel();
        });
    }
    public void HideMapCreatePanel()
    {
        UpdateMapChooseUI();
        MapCreatePanel.Hide();
    }

    #endregion
    #region//��Ϸ��������

    public TMP_InputField input_RoomName;
    public TMP_Dropdown dropdown_RoomType;
    public Button btn_CreateRoom;

    private string roomName;
    private string actorDataPath = "";
    private string buildInfoPath = "";
    private string buildTypePath = "";
    private string floorTypePath = "";

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
            Debug.Log("��������δ���");
        }
    }
    private bool CheckRoomSetting()
    {
        if (roomName != ""&& buildTypePath!=""&& buildInfoPath != "" && floorTypePath != "" && actorDataPath != "")
        {
            return true;
        }
        return false;
    }
    #endregion

}
