using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UI_MainMenu : MonoBehaviour
{
    public Image image_BG;
    public Button btn_Create;
    public Button btn_Join;
    public Button btn_Setting;
    public Button btn_Quit;
    public Button btn_Steam;
    public Button btn_TT;
    public string url_Steam;
    public string url_TT;
    public UI_GameCreatePanel createGame;
    public UI_GameJoinPanel joinGame;
    private void Awake()
    {
        Bind();
    }
    private void Start()
    {
        image_BG.DOFade(0.2f, 5f).SetLoops(-1, LoopType.Yoyo);
    }
    private void Bind()
    {
        btn_Create.onClick.AddListener(Create);
        btn_Join.onClick.AddListener(Join);
        btn_Quit.onClick.AddListener(Quit);
        btn_Steam.onClick.AddListener(LinkToSteam);
        btn_TT.onClick.AddListener(LinkToTT);
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
    private void LinkToSteam()
    {
        Application.OpenURL(url_Steam);
    }
    private void LinkToTT()
    {
        Application.OpenURL(url_TT);
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
