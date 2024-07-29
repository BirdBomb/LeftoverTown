using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UI_MapChooseButton : MonoBehaviour
{
    public Button btn_Create;
    public Button btn_Choose;
    public Button btn_Del;
    public Image image_Head;
    private string _data;
    private string _path;
    private UI_ActorShowPanel _actorShow;
    public void Start()
    {
        Bind();
    }
    public void Bind()
    {
        btn_Create.onClick.AddListener(Create);
        btn_Choose.onClick.AddListener(Choose);
        btn_Del.onClick.AddListener(Del);
    }
    public void InitByPlayer(string data, string path, UI_ActorShowPanel actorShow)
    {
        btn_Choose.gameObject.SetActive(true);
        btn_Create.gameObject.SetActive(false);
        _data = data;
        _path = path;
        _actorShow = actorShow;
    }
    public void InitByEmpty()
    {
        btn_Choose.gameObject.SetActive(false);
        btn_Create.gameObject.SetActive(true);
        _data = "";
        _path = "";
        _actorShow = null;
    }
    public void Create()
    {

    }
    public void Choose()
    {
        if (_actorShow)
        {
            _actorShow.Init(JsonConvert.DeserializeObject<PlayerData>(_data));
        }
    }
    public void Del()
    {

    }

}
