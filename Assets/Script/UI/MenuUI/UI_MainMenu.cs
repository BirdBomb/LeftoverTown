using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainMenu : MonoBehaviour
{
    public Button btn_Create;
    public Button btn_Join;
    public Button btn_Setting;
    public Button btn_Quit;
    public UI_GameCreatePanel createGame;
    public UI_GameJoinPanel joinGame;
    private void Awake()
    {
        Bind();
    }
    private void Bind()
    {
        btn_Create.onClick.AddListener(Create);
        btn_Join.onClick.AddListener(Join);
        btn_Quit.onClick.AddListener(Quit);
    }
    private void Create()
    {
        createGame.ShowGameCreatePanel();
    }
    private void Join()
    {
        joinGame.ShowGameJoinPanel();
    }
    private void Setting()
    {
        
    }
    private void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
