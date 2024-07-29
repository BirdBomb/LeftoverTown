using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UI_Main : MonoBehaviour
{
    public Button btn_GameCreate;
    public Button btn_GameLobby;
    public Button btn_Setting;
    public Button btn_Quit;

    public UI_GameCreate ui_GameCreate;
    public UI_GameLobby ui_GameLobby;
    public void Start()
    {
        Bind();
    }
    public void Bind()
    {
        btn_GameCreate.onClick.AddListener(ClickGameCreateBtn);
        btn_GameLobby.onClick.AddListener(ClickGameLobbyBtn);
        btn_Setting.onClick.AddListener(ClickSettingBtn);
        btn_Quit.onClick.AddListener(ClickQuitBtn);
    }
    private void ClickGameCreateBtn()
    {
        ui_GameCreate.UpdateActorChooseUI();
        ui_GameCreate.ShowPanel();
    }
    private void ClickGameLobbyBtn()
    {
        ui_GameLobby.UpdateLobby();
        ui_GameLobby.ShowPanel();
    }
    private void ClickSettingBtn()
    {

    }
    private void ClickQuitBtn()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
