using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    public MapInfoData mapInfoData;
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
        bind_Path = $"MapData/MapInfo{index}";
        mapInfoData = FileManager.Instance.ReadMapInfoData(bind_Path);
        Draw();
    }
    public void Draw()
    {
        if (mapInfoData != null)
        {
            text_Name.text = mapInfoData.name;
            btn_Choose.gameObject.SetActive(true);
        }
        else
        {
            btn_Choose.gameObject.SetActive(false);
        }
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
