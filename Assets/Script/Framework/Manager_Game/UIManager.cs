using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class UIManager : SingleTon<UIManager>, ISingleTon
{
    [SerializeField,Header("UIÃæ°å")]
    private Transform _Panel;
    [SerializeField, Header("TileUI")]
    private Transform transform_TileUIPanel;
    [SerializeField, Header("BagUI")]
    private GameUI_BagPanel gameUI_BagPanel;
    private Canvas _Canvas;
    private Dictionary<string, UIGroup> _Pool = new Dictionary<string, UIGroup>();
    public void Init()
    {
        
    }
    public GameObject ShowUI(string name,Vector2 pos)
    {
        if (!_Pool.ContainsKey(name))
        {
            _Pool.Add(name, new UIGroup(name));
        }
        GameObject obj = _Pool[name].Pop();
        obj.SetActive(true);
        obj.transform.SetParent(_Panel,false);
        obj.transform.localScale = Vector3.one;
        obj.transform.position = Camera.main.WorldToScreenPoint(pos);
        return obj;
    }
    public GameObject ShowUI(string name)
    {
        if (!_Pool.ContainsKey(name))
        {
            _Pool.Add(name, new UIGroup(name));
        }
        GameObject obj = _Pool[name].Pop();
        obj.SetActive(true);
        obj.transform.SetParent(_Panel);
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = Vector3.zero;
        return obj;
    }
    public void HideUI(string name, GameObject ui)
    {
        ui.SetActive(false);
        _Pool[name].Push(ui);
    }

    private TileUI tileUI_Bind;
    public void ShowTileUI(GameObject obj, out TileUI tileUI)
    {
        tileUI = Instantiate(obj, transform_TileUIPanel).GetComponent<TileUI>();
        if (tileUI_Bind)
        {
            HideTileUI(tileUI_Bind);
        }
        tileUI_Bind = tileUI;
        tileUI_Bind.Show();
        gameUI_BagPanel.ShowPanel();
    }
    public void HideTileUI(TileUI tileUI)
    {
        if (tileUI == this.tileUI_Bind)
        {
            if (tileUI_Bind != null) tileUI_Bind.Hide();
            if (tileUI_Bind) tileUI_Bind = null;
        }
        gameUI_BagPanel.HidePanel();
    }
}
public class UIGroup
{
    private string _name;
    public UIGroup(string name)
    {
        _name = name;
    }
    List<GameObject> group = new List<GameObject>();
    public GameObject Pop()
    {
        if (group.Count > 0)
        {
            GameObject gameObject = group[0];
            group.RemoveAt(0);
            return gameObject;
        }
        else
        {
            GameObject obj = Resources.Load<GameObject>(_name);
            GameObject ui = Object.Instantiate(obj);
            return ui;
        }
    }
    public void Push(GameObject obj)
    {
        group.Add(obj);
    }
}