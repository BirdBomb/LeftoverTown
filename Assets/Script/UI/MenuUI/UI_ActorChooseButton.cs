using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
public class UI_ActorChooseButton : MonoBehaviour
{
    public Button btn_Create;
    public Button btn_Choose;
    public Button btn_Delete;
    public Image image_Head;
    public Image image_Hair;
    public Image image_Eye;
    public TextMeshProUGUI text_Name;
    public SpriteAtlas spriteAtlas_Hair;
    public SpriteAtlas spriteAtlas_Eye;
    private Action<UI_ActorChooseButton> action_Choose;
    private Action<UI_ActorChooseButton> action_Create;
    private Action<UI_ActorChooseButton> action_Delete;
    [HideInInspector]
    public string bind_Data;
    [HideInInspector]
    public string bind_Path;
    private bool binding;

    public void Awake()
    {
        Bind();   
    }
    public void Bind()
    {
        btn_Create.onClick.AddListener(Create);
        btn_Choose.onClick.AddListener(Choose);
        btn_Delete.onClick.AddListener(Delete);
    }
    public void Init(string data,string path, Action<UI_ActorChooseButton> choose, Action<UI_ActorChooseButton> create, Action<UI_ActorChooseButton> delete)
    {
        bind_Data = data;
        bind_Path = path;

        action_Choose = choose;
        action_Create = create;
        action_Delete = delete;

        if (data != "") binding = true;
        else binding = false;
        DrawPlayerHead();
    }
    private void DrawPlayerHead()
    {
        if (binding)
        {
            PlayerData playerData = JsonConvert.DeserializeObject<PlayerData>(bind_Data);
            if (!spriteAtlas_Eye) Debug.Log("aaqq");
            image_Eye.sprite = spriteAtlas_Eye.GetSprite("Eye_" + playerData.Eye_ID.ToString());
            image_Hair.sprite = spriteAtlas_Hair.GetSprite("Hair_" + playerData.Hair_ID.ToString());
            image_Hair.color = playerData.Hair_Color;
            text_Name.text = playerData.Name;
            btn_Choose.gameObject.SetActive(true);
            btn_Create.gameObject.SetActive(false);
        }
        else
        {
            btn_Choose.gameObject.SetActive(false);
            btn_Create.gameObject.SetActive(true);
        }
    }
    public void Create()
    {
        action_Create.Invoke(this);
    }
    public void Choose()
    {
        action_Choose.Invoke(this);
    }
    public void Delete()
    {
        action_Delete.Invoke(this);
        FileManager.Instance.DeleteFile(bind_Path);
    }
}
