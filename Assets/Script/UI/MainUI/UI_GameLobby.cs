using DG.Tweening;
using Fusion;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameLobby : MonoBehaviour
{
    private void Start()
    {
        Bind();
    }
    private void Bind()
    {
        BindMain();
        BindLobbyPanel();
        BindSearchPanel();
        BindRoomPanel();
    }
    #region//主要
    public Button btn_Return;
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
    #region//房间列表
    private List<SessionInfo> sessionInfos = new List<SessionInfo>();
    private List<UI_RoomCell> roomCells = new List<UI_RoomCell>();
    public Button btn_Refresh;
    public GameObject roomCell;
    public Transform pool;
    private void BindLobbyPanel()
    {
        MessageBroker.Default.Receive<NetEvent.NetEvent_SessionListUpdated>().Subscribe(_ =>
        {
            sessionInfos.Clear();
            foreach (var sessionInfo in _.SessionList)
            {
                sessionInfos.Add(sessionInfo);
            }
        });
        btn_Refresh.onClick.AddListener(UpdateLobby);
    }
    public void UpdateLobby()
    {
        Debug.Log("更新大厅列表" + sessionInfos.Count);
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
        foreach(SessionInfo sessionInfo in sessionInfos)
        {
            GameObject obj = Instantiate(roomCell, pool);
            obj.GetComponent<UI_RoomCell>().InitRoomCell(sessionInfo, (_) =>
            {
                ShowRoomPanel(_.bindInfo);
            });
            roomCells.Add(obj.GetComponent<UI_RoomCell>());
        }
    }
    #endregion
    #region//房间查找
    public TMP_InputField input_SearchRoom;
    public TextMeshProUGUI text_SearchCallBack;
    public Button btn_SearchRoom;
    private string searchName = "";

    private void BindSearchPanel()
    {
        input_SearchRoom.onValueChanged.AddListener((str) =>
        {
            searchName = str;
        });
        btn_SearchRoom.onClick.AddListener(ClickSearchBtn);
    }
    private void ClickSearchBtn()
    {
        if(searchName == "")
        {
            
        }
        else
        {
            foreach(SessionInfo sessionInfo in sessionInfos)
            {
                if(searchName == sessionInfo.Name)
                {
                    ShowRoomPanel(sessionInfo);
                    return;
                }
            }
            input_SearchRoom.text = "";
            text_SearchCallBack.text = "未找到目标房间";
        }
    }
    #endregion
    #region//房间预览
    public Button btn_HideRoomPanel;
    public Button btn_JoinRoom;
    public Transform panel_RoomPanel;
    private SessionInfo bindSessionInfo;

    public List<UI_ActorChooseButton> ActorChooseButtonList = new List<UI_ActorChooseButton>();
    private void BindRoomPanel()
    {
        btn_HideRoomPanel.onClick.AddListener(HideRoomPanel);
        btn_JoinRoom.onClick.AddListener(JoinRoom);
    }
    public void UpdateActorChooseUI()
    {
        for (int i = 0; i < ActorChooseButtonList.Count; i++)
        {
            ActorChooseButtonList[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < ActorChooseButtonList.Count; i++)
        {
            int index = i;
            string data = FileManager.Instance.ReadFile("PlayerData/Player" + index);
            ActorChooseButtonList[index].gameObject.SetActive(true);
            ActorChooseButtonList[index].Init(data, "PlayerData/Player" + index, 
                (_) =>
            {
                ShowActorShowPanel(JsonConvert.DeserializeObject<PlayerData>(_.bind_Data));
                BindActorData(_.bind_Path);
            },  (_) =>
            {
                ShowActorCreatePanel(_.bind_Path);
            },  (_) =>
            {
                UpdateActorChooseUI();
            });
        }
    }
    public void ShowRoomPanel(SessionInfo sessionInfo)
    {
        bindSessionInfo = sessionInfo;
        panel_RoomPanel.gameObject.SetActive(true);
        UpdateActorChooseUI();
    }
    public void HideRoomPanel()
    {
        panel_RoomPanel.gameObject.SetActive(false);
    }
    public void JoinRoom()
    {
        GameDataManager.Instance.actorFilePath = actorDataPath;

        MessageBroker.Default.Publish(new NetEvent.NetEvent_JoinGame()
        {
            RoomName = bindSessionInfo.Name,
        });
    }

    #region//角色预览
    public UI_ActorShowPanel ActorShowPanel = new UI_ActorShowPanel();
    private string actorDataPath;
    public void BindActorData(string path)
    {
        actorDataPath = path;
    }
    public void ShowActorShowPanel(PlayerData playerData)
    {
        ActorShowPanel.Init(playerData);
        ActorCreatePanel.Hide();
    }
    #endregion
    #region//角色创建
    public UI_ActorCreatePanel ActorCreatePanel = new UI_ActorCreatePanel();
    public void ShowActorCreatePanel(string path)
    {
        ActorShowPanel.Hide();
        ActorCreatePanel.Show();
        ActorCreatePanel.Init(path, () => { HideActorCreatePanel(); });
    }
    public void HideActorCreatePanel()
    {
        UpdateActorChooseUI();
        ActorCreatePanel.Hide();
    }
    #endregion
    #endregion
}
