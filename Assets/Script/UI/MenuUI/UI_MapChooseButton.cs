using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;

public class UI_MapChooseButton : MonoBehaviour
{
    public Button btn_Create;
    public Button btn_Choose;
    public Button btn_Delete;
    public TextMeshProUGUI text_MapName;

    private Action<UI_MapChooseButton> action_Choose;
    private Action<UI_MapChooseButton> action_Create;
    private Action<UI_MapChooseButton> action_Delete;

    [HideInInspector]
    public string bind_MapInfoData;
    [HideInInspector]
    public string bind_MapInfoPath;
    [HideInInspector]
    public string bind_BuildingInfoPath;
    [HideInInspector]
    public string bind_BuildingTypePath;
    [HideInInspector]
    public string bind_FloorTypePath;

    private bool binding;
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
    public void Init(int index, Action<UI_MapChooseButton> choose, Action<UI_MapChooseButton> create, Action<UI_MapChooseButton> delete)
    {
        bind_MapInfoPath = "MapData/MapInfo" + index;
        bind_BuildingInfoPath = "MapData/MapBuildingInfo" + index;
        bind_BuildingTypePath = "MapData/MapBuildingType" + index;
        bind_FloorTypePath = "MapData/MapFloorType" + index;
        bind_MapInfoData = FileManager.Instance.ReadFile(bind_MapInfoPath);

        action_Choose = choose;
        action_Create = create;
        action_Delete = delete;
        if (bind_MapInfoData != "") binding = true;
        else binding = false;
        Draw();
    }
    private void Draw()
    {
        if (binding)
        {
            text_MapName.text = JsonConvert.DeserializeObject<MapInfoData>(bind_MapInfoData).name;
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
        action_Create.Invoke(this);
    }
    public void Choose()
    {
        action_Choose.Invoke(this);
    }
    public void Delete()
    {
        action_Delete.Invoke(this);
        FileManager.Instance.DeleteFile(bind_MapInfoPath);
        FileManager.Instance.DeleteFile(bind_BuildingInfoPath);
        FileManager.Instance.DeleteFile(bind_BuildingTypePath);
        FileManager.Instance.DeleteFile(bind_FloorTypePath);
        btn_Choose.gameObject.SetActive(false);
        btn_Delete.gameObject.SetActive(false);
        btn_Create.gameObject.SetActive(true);
    }

}
