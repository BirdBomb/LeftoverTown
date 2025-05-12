using DG.Tweening;
using Fusion;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UI_LobbyJoin : MonoBehaviour
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
    public Transform transform_Panel;

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
    #region//房间列表
    private List<SessionInfo> sessionInfos = new List<SessionInfo>();
    private List<UI_RoomCell> uI_RoomCells = new List<UI_RoomCell>();
    public Transform transform_Pool;
    public Button btn_Refresh;
    public GameObject prefab_RoomCell;
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
        foreach (UI_RoomCell cell in uI_RoomCells)
        {
            Destroy(cell.gameObject);
        }
        uI_RoomCells.Clear();
    }
    private void DrawLobby()
    {
        foreach(SessionInfo sessionInfo in sessionInfos)
        {
            GameObject obj = Instantiate(prefab_RoomCell, transform_Pool);
            obj.GetComponent<UI_RoomCell>().InitRoomCell(sessionInfo, (_) =>
            {
                ShowRoomPanel(_.bindInfo);
            });
            uI_RoomCells.Add(obj.GetComponent<UI_RoomCell>());
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
    private SessionInfo sessionInfo_Bind;

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
            string path = "PlayerData/Player" + index;
            string data = FileManager.Instance.ReadFile(path);
            ActorChooseButtonList[index].gameObject.SetActive(true);
            ActorChooseButtonList[index].Init(data, path,
               (_) => { ShowActorShowPanel(_.bind_Data, _.bind_Path); },
               (_) => { ShowActorCreatePanel(_.bind_Path); },
               (_) => { UpdateActorChooseUI(); });
        }
    }
    public void ShowRoomPanel(SessionInfo sessionInfo)
    {
        sessionInfo_Bind = sessionInfo;
        panel_RoomPanel.gameObject.SetActive(true);
        UpdateActorChooseUI();
    }
    public void HideRoomPanel()
    {
        panel_RoomPanel.gameObject.SetActive(false);
    }
    public void JoinRoom()
    {
        GameDataManager.Instance.bind_PlayerDataPath = bind_ActorDataPath;
        MessageBroker.Default.Publish(new NetEvent.NetEvent_JoinGame()
        {
            RoomInfo = sessionInfo_Bind,
            RoomName = sessionInfo_Bind.Name,
        });
    }

    #region//角色预览
    public UI_ActorShowPanel uI_ActorShowPanel;
    private string bind_ActorDataPath;
    public void ShowActorShowPanel(string data, string path)
    {
        bind_ActorDataPath = path;
        uI_ActorShowPanel.Init(JsonConvert.DeserializeObject<PlayerData>(data));
        uI_ActorShowPanel.Show();
        uI_ActorCreatePanel.Hide();
    }
    #endregion
    #region//角色创建
    public UI_ActorCreatePanel uI_ActorCreatePanel = new UI_ActorCreatePanel();
    public void ShowActorCreatePanel(string path)
    {
        uI_ActorShowPanel.Hide();
        uI_ActorCreatePanel.Show();
        uI_ActorCreatePanel.Init(path, () => { HideActorCreatePanel(); });
    }
    public void HideActorCreatePanel()
    {
        UpdateActorChooseUI();
        uI_ActorCreatePanel.Hide();
    }
    #endregion
    #endregion
}
