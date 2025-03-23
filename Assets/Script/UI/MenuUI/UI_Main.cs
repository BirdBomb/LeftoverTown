using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UI_Main : MonoBehaviour
{
    public Button btn_LobbyCreate;
    public Button btn_LobbyJoin;
    public Button btn_Setting;
    public Button btn_Quit;

    public UI_LobbyCreate ui_LobbyCreate;
    public UI_LobbyJoin ui_LobbyJoin;
    public UI_GameSetting UI_GameSetting;
    public void Awake()
    {
        Bind();
    }
    public void Bind()
    {
        btn_LobbyCreate.onClick.AddListener(ClickLobbyCreateBtn);
        btn_LobbyJoin.onClick.AddListener(ClickLobbyJoinBtn);
        btn_Setting.onClick.AddListener(ClickSettingBtn);
        btn_Quit.onClick.AddListener(ClickQuitBtn);
    }
    #region//创建大厅
    private void ClickLobbyCreateBtn()
    {
        ui_LobbyCreate.UpdateActorChooseUI();
        ui_LobbyCreate.UpdateMapChooseUI();
        ui_LobbyCreate.ShowPanel();
    }
    #endregion
    #region//加入大厅
    private void ClickLobbyJoinBtn()
    {
        ui_LobbyJoin.UpdateLobby();
        ui_LobbyJoin.ShowPanel();
    }
    #endregion
    #region//设置
    private void ClickSettingBtn()
    {
        UI_GameSetting.ShowPanel();
    }
    #endregion
    #region//退出
    private void ClickQuitBtn()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    #endregion
}
