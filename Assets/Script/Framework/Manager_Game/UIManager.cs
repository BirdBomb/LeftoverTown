using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingleTon<UIManager>, ISingleTon
{
    [SerializeField]
    private Transform transform_Panel;
    [SerializeField]
    private Button btn_Quit;
    [SerializeField,Header("UIÃæ°å")]
    private Transform trans_Panel;
    [SerializeField, Header("TileUI")]
    private Transform trans_TileUIPanel;
    public GameUI_BagPanel gameUI_BagPanel;
    public GameUI_Build gameUI_Build;
    public GameUI_Skill gameUI_Skill;
    private Dictionary<string, UIGroup> dic_UiPool = new Dictionary<string, UIGroup>();
    public void Init()
    {
        btn_Quit.onClick.AddListener(Quit);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool openEscPanel = true;
            openEscPanel = CheckEscAction() & CheckBagPanel() & CheckBuildPanel() & CheckSkillPanel() & CheckBindTileUI();
            if (openEscPanel)
            {
                ShowEscPanel();
            }
        }
    }
    #region//EscInput
    private Stack<Action> stack_EscAction = new Stack<Action>();
    public void PushEscAction(Action action)
    {
        stack_EscAction.Push(action);
    }
    public void PopEscAction()
    {
        Action action = stack_EscAction.Pop();
        if (action != null) action.Invoke();
    }

    private bool CheckEscAction()
    {
        bool var = stack_EscAction.Count <= 0;
        while (stack_EscAction.Count > 0)
        {
            PopEscAction();
        }
        return var;
    }
    private bool CheckBagPanel()
    {
        bool var = !gameUI_BagPanel.bool_Show;
        gameUI_BagPanel.HidePanel();
        return var;
    }
    private bool CheckBuildPanel()
    {
        bool var = !gameUI_Build.bool_Show;
        gameUI_Build.HidePanel();
        return var;
    }
    private bool CheckSkillPanel()
    {
        bool var = !gameUI_Skill.bool_Show;
        gameUI_Skill.HidePanel();
        return var;
    }
    private bool CheckBindTileUI()
    {
        bool var = tileUI_Bind == null;
        if (!var) { HideTileUI(tileUI_Bind); }
        return var;
    }
    #endregion
    #region//EscPanel
    private void ShowEscPanel()
    {
        transform_Panel.gameObject.SetActive(!transform_Panel.gameObject.activeSelf);
    }
    private void Quit()
    {
        NetManager.Instance.QuitRoom();
    }

    #endregion
    public GameObject ShowUI(string name,Vector2 pos)
    {
        if (!dic_UiPool.ContainsKey(name))
        {
            dic_UiPool.Add(name, new UIGroup(name));
        }
        GameObject obj = dic_UiPool[name].Pop();
        obj.SetActive(true);
        obj.transform.SetParent(trans_Panel,false);
        obj.transform.localScale = Vector3.one;
        obj.transform.position = Camera.main.WorldToScreenPoint(pos);
        return obj;
    }
    public GameObject ShowUI(string name)
    {
        if (!dic_UiPool.ContainsKey(name))
        {
            dic_UiPool.Add(name, new UIGroup(name));
        }
        GameObject obj = dic_UiPool[name].Pop();
        obj.SetActive(true);
        obj.transform.SetParent(trans_Panel);
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = Vector3.zero;
        return obj;
    }
    public void HideUI(string name, GameObject ui)
    {
        ui.SetActive(false);
        dic_UiPool[name].Push(ui);
    }

    private TileUI tileUI_Bind;
    public void ShowTileUI(GameObject obj, out TileUI tileUI)
    {
        HideTileUI(tileUI_Bind);
        tileUI = Instantiate(obj, trans_TileUIPanel).GetComponent<TileUI>();
        tileUI_Bind = tileUI;
        tileUI_Bind.Show();
        if(tileUI.NeedToOpenBagPanel()) gameUI_BagPanel.ShowPanel();
    }
    public void HideTileUI(TileUI tileUI)
    {
        if (tileUI != null && tileUI == tileUI_Bind)
        {
            tileUI_Bind.Hide();
            tileUI_Bind = null;
        }
        gameUI_BagPanel.HidePanel();
    }

    
}
public class UIGroup
{
    private string str_Name;
    public UIGroup(string name)
    {
        str_Name = name;
    }
    List<GameObject> list_ObjGroup = new List<GameObject>();
    public GameObject Pop()
    {
        if (list_ObjGroup.Count > 0)
        {
            GameObject gameObject = list_ObjGroup[0];
            list_ObjGroup.RemoveAt(0);
            return gameObject;
        }
        else
        {
            GameObject obj = Resources.Load<GameObject>(str_Name);
            GameObject ui = UnityEngine.Object.Instantiate(obj);
            return ui;
        }
    }
    public void Push(GameObject obj)
    {
        list_ObjGroup.Add(obj);
    }
}