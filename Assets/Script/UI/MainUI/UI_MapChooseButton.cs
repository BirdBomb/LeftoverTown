using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;
using static UnityEngine.InputManagerEntry;

public class UI_MapChooseButton : MonoBehaviour
{
    public Button btn_Create;
    public Button btn_Choose;
    public Button btn_Delete;
    public TextMeshProUGUI text_MapName;

    [HideInInspector]
    public string bind_Data;
    [HideInInspector]
    public string bind_BuildInfoPath;
    [HideInInspector]
    public string bind_BuildTypePath;
    [HideInInspector]
    public string bind_FloorTypePath;

    private Action<UI_MapChooseButton> chooseAction;
    private Action<UI_MapChooseButton> createAction;
    private Action<UI_MapChooseButton> deleteAction;

    public void Start()
    {
        Bind();
    }
    public void Bind()
    {
        btn_Create.onClick.AddListener(Create);
        btn_Choose.onClick.AddListener(Choose);
        btn_Delete.onClick.AddListener(Delete);
    }
    public void Init(string data, string buildInfoPath, string buildTypePath,string floorTypePath, Action<UI_MapChooseButton> choose, Action<UI_MapChooseButton> create, Action<UI_MapChooseButton> delete)
    {
        bind_Data = data;
        bind_BuildInfoPath = buildInfoPath;
        bind_BuildTypePath = buildTypePath;
        bind_FloorTypePath = floorTypePath;

        chooseAction = choose;
        createAction = create;
        deleteAction = delete;

        if (data != "")
        {
            text_MapName.text = JsonConvert.DeserializeObject<MapTileInfoData>(data).name;
            btn_Choose.gameObject.SetActive(true);
            btn_Delete.gameObject.SetActive(true);
            btn_Create.gameObject.SetActive(false);
        }
        else
        {
            btn_Choose.gameObject.SetActive(false);
            btn_Delete.gameObject.SetActive(false);
            btn_Create.gameObject.SetActive(true);
        }
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
        FileManager.Instance.DeleteFile(bind_BuildInfoPath);
        FileManager.Instance.DeleteFile(bind_BuildTypePath);
        FileManager.Instance.DeleteFile(bind_FloorTypePath);
        btn_Choose.gameObject.SetActive(false);
        btn_Delete.gameObject.SetActive(false);
        btn_Create.gameObject.SetActive(true);
    }

}
