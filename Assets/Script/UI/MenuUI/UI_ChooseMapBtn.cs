using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ChooseMapBtn : MonoBehaviour
{
    public Button btn_Choose;
    public Button btn_Delete;
    public TextMeshProUGUI text_Name;
    private Action<int> action_Choose;
    private Action<int> action_Delete;
    [HideInInspector]
    public string bind_Data;
    [HideInInspector]
    public string bind_Path;
    private int bind_Index;
    public void Bind(int index, Action<int> actionChoose, Action<int> actionDelete)
    {
        bind_Index = index;
        action_Choose = actionChoose;
        action_Delete = actionDelete;
        btn_Choose.onClick.AddListener(Choose);
        btn_Delete.onClick.AddListener(Delete);
    }
    public void Init(int index, Action<UI_ChooseMapBtn> choose, Action<UI_ChooseMapBtn> delete)
    {
        bind_Path = "MapData/MapInfo" + index;
        bind_Data = FileManager.Instance.ReadFile(bind_Path);

        if (bind_Data != "") Draw();
        else Hide();
    }
    private void Draw()
    {
        MapInfoData mapData = JsonConvert.DeserializeObject<MapInfoData>(bind_Data);
        text_Name.text = mapData.name;
        btn_Choose.gameObject.SetActive(true);
    }
    private void Hide()
    {
        btn_Choose.gameObject.SetActive(false);
    }
    public void Choose()
    {
        action_Choose.Invoke(bind_Index);
    }
    public void Delete()
    {
        FileManager.Instance.DeleteFile(bind_Path);
        action_Delete.Invoke(bind_Index);
    }
}
