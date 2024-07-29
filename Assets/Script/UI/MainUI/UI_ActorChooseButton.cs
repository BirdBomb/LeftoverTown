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
    public string bind_Data;
    public string bind_Path;
    private Action<UI_ActorChooseButton> chooseAction;
    private Action<UI_ActorChooseButton> createAction;
    private Action<UI_ActorChooseButton> deleteAction;
    private SpriteAtlas atlasHair;
    private SpriteAtlas atlasEye;

    public void Start()
    {
        Bind();   
    }
    public void Bind()
    {
        atlasHair = Resources.Load<SpriteAtlas>("Atlas/HairSprite");
        atlasEye = Resources.Load<SpriteAtlas>("Atlas/EyeSprite");

        btn_Create.onClick.AddListener(Create);
        btn_Choose.onClick.AddListener(Choose);
        btn_Delete.onClick.AddListener(Delete);
    }
    public void Init(string data,string path, Action<UI_ActorChooseButton> choose, Action<UI_ActorChooseButton> create, Action<UI_ActorChooseButton> delete)
    {
        bind_Data = data;
        bind_Path = path;

        chooseAction = choose;
        createAction = create;
        deleteAction = delete;

        if (data != "")
        {
            DrawPlayerHead(data);
            btn_Choose.gameObject.SetActive(true);
            btn_Create.gameObject.SetActive(false);
        }
        else
        {
            btn_Choose.gameObject.SetActive(false);
            btn_Create.gameObject.SetActive(true);
        }
    }
    private void DrawPlayerHead(string data)
    {
        PlayerData playerData = JsonConvert.DeserializeObject<PlayerData>(data);
        if (atlasHair == null)
        {
            atlasHair = Resources.Load<SpriteAtlas>("Atlas/HairSprite");
        }
        if (atlasEye == null)
        {
            atlasEye = Resources.Load<SpriteAtlas>("Atlas/EyeSprite");
        }
        image_Hair.sprite = atlasHair.GetSprite("Hair_" + playerData.HairID.ToString());
        image_Hair.color = playerData.HairColor;
        image_Eye.sprite = atlasEye.GetSprite("Eye_" + playerData.EyeID.ToString());
        text_Name.text = playerData.Name;
    }
    public void Create()
    {
        createAction.Invoke(this);
    }
    public void Choose()
    {
        chooseAction.Invoke(this);
    }
    public void Delete()
    {
        deleteAction.Invoke(this);
        FileManager.Instance.DeleteFile(bind_Path);
        btn_Choose.gameObject.SetActive(false);
        btn_Create.gameObject.SetActive(true);
    }
}
