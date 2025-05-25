using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class UI_ChooseActorBtn : MonoBehaviour
{
    public Button btn_Choose;
    public Button btn_Delete;
    public Image image_Head;
    public Image image_Hair;
    public Image image_Eye;
    public TextMeshProUGUI text_Name;
    public SpriteAtlas spriteAtlas_Hair;
    public SpriteAtlas spriteAtlas_Eye;
    private Action<int> action_Choose;
    private Action<int> action_Delete;
    [HideInInspector]
    public string bind_Data;
    [HideInInspector]
    public string bind_Path;
    public int bind_Index;
    public void Bind(int index, Action<int> actionChoose, Action<int> actionDelete)
    {
        bind_Index = index;
        action_Choose = actionChoose;
        action_Delete = actionDelete;
        btn_Choose.onClick.AddListener(Choose);
        btn_Delete.onClick.AddListener(Delete);
    }
    public void Init(string data, string path, Action<UI_ChooseActorBtn> choose, Action<UI_ChooseActorBtn> delete)
    {
        bind_Data = data;
        bind_Path = path;
        if (data != "") Draw();
        else Hide();
    }
    private void Draw()
    {
        PlayerData playerData = JsonConvert.DeserializeObject<PlayerData>(bind_Data);
        image_Eye.sprite = spriteAtlas_Eye.GetSprite("Eye_" + playerData.Eye_ID.ToString());
        image_Hair.sprite = spriteAtlas_Hair.GetSprite("Hair_" + playerData.Hair_ID.ToString());
        image_Hair.color = playerData.Hair_Color;
        text_Name.text = playerData.Name;
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
