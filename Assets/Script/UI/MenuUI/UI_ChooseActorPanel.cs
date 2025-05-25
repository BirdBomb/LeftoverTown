using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ChooseActorPanel : MonoBehaviour
{
    public Transform transform_Panel;
    public Transform transform_Sign;
    public Button btn_Pass;
    public Button btn_Close; 
    public List<UI_ChooseActorBtn> chooseActorBtns = new List<UI_ChooseActorBtn>();
    public List<Button> createActorBtns = new List<Button>();
    public Action action_Pass = null;
    public Action action_Close = null;
    public string bind_ActorPath = "";
    public UI_CreateActor createActor;
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
        for (int i = 0; i < createActorBtns.Count; i++)
        {
            int index = i;
            createActorBtns[index].onClick.RemoveAllListeners();
            createActorBtns[index].onClick.AddListener(() =>
            {
                Create(index);
            });
            chooseActorBtns[index].Bind(index,
                (_) =>
                {
                    Choose(index);
                },
                (_) => { UpdatePanel(); });
        }
    }
    public void UpdatePanel() 
    {
        Choose(-1);
        for (int i = 0; i < chooseActorBtns.Count; i++)
        {
            int index = i;
            string path = "PlayerData/Player" + index;
            string data = FileManager.Instance.ReadFile(path);
            chooseActorBtns[index].gameObject.SetActive(true);
            chooseActorBtns[index].Init(data, path,
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
        createActor.Init(index,
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
            transform_Sign.transform.position = chooseActorBtns[index].transform.position;
            bind_ActorPath = "PlayerData/Player" + index;
        }
        else
        {
            btn_Pass.interactable = false;
            transform_Sign.gameObject.SetActive(false);
            transform_Sign.transform.position = chooseActorBtns[0].transform.position;
        }
    }
}
