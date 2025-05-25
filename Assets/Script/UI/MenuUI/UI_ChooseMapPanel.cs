using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ChooseMapPanel : MonoBehaviour
{
    public Transform transform_Panel;
    public Transform transform_Sign;
    public Button btn_Pass;
    public Button btn_Close;
    public List<UI_ChooseMapBtn> chooseMapBtns = new List<UI_ChooseMapBtn>();
    public List<Button> createMapBtns = new List<Button>();
    public Action action_Pass = null;
    public Action action_Close = null;
    public int bind_MapIndex = -1;
    public UI_CreateMap createMap;
    public void HidePanel()
    {
        transform_Panel.gameObject.SetActive(false);
    }
    public void ShowPanel()
    {
        transform_Panel.gameObject.SetActive(true);
        transform_Panel.transform.DOPunchScale(new Vector3(0.1f, -0.1f, 0), 0.1f);
        UpdatePanel();
    }
    public void Bind(Action actionPass, Action actionClose)
    {
        btn_Pass.onClick.AddListener(Pass);
        btn_Close.onClick.AddListener(Close);
        action_Pass = actionPass;
        action_Close = actionClose;
        for (int i = 0; i < createMapBtns.Count; i++)
        {
            int index = i;
            createMapBtns[index].onClick.RemoveAllListeners();
            createMapBtns[index].onClick.AddListener(() =>
            {
                Create(index);
            });
            chooseMapBtns[index].Bind(index,
                (_) => { Choose(index); },
                (_) => { UpdatePanel(); });
        }
    }
    public void UpdatePanel()
    {
        Choose(-1);
        for (int i = 0; i < chooseMapBtns.Count; i++)
        {
            int index = i;
            string path = "PlayerData/Player" + index;
            string data = FileManager.Instance.ReadFile(path);
            chooseMapBtns[index].gameObject.SetActive(true);
            chooseMapBtns[index].Init(index,
               (_) => { },
               (_) => { });
        }
    }

    private void Pass()
    {
        if (action_Pass != null)
        {
            action_Pass.Invoke();
        }
    }
    private void Close()
    {
        if (action_Close != null)
        {
            action_Close.Invoke();
        }
    }
    private void Create(int index)
    {
        HidePanel();
        createMap.Init(index,
            () =>
            {
                ShowPanel();
            },
            () =>
            {
                ShowPanel();
            });

    }
    private void Choose(int index)
    {
        if (index >= 0)
        {
            btn_Pass.interactable = true;
            transform_Sign.gameObject.SetActive(true);
            transform_Sign.transform.position = chooseMapBtns[index].transform.position;
            bind_MapIndex = index;
        }
        else
        {
            btn_Pass.interactable = false;
            transform_Sign.gameObject.SetActive(false);
            transform_Sign.transform.position = chooseMapBtns[0].transform.position;
        }
    }

}
