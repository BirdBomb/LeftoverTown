using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_MapShowPanel : MonoBehaviour
{
    public Transform panel_Main;
    public TextMeshProUGUI text_MapName;
    public TMP_InputField text_MapSeed;
    public Button btn_Return;
    public void Awake()
    {
        Bind();
    }
    private void Bind()
    {
        btn_Return.onClick.AddListener(Hide);
    }
    public void Init(string mapName,string mapSeed)
    {
        text_MapName.text = mapName;
        text_MapSeed.text = mapSeed;
    }
    public void Show()
    {
        panel_Main.gameObject.SetActive(true);
    }
    public void Hide()
    {
        panel_Main.gameObject.SetActive(false);
    }
}
