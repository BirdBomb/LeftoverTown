using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class UIManager : SingleTon<UIManager>, ISingleTon
{
    [SerializeField,Header("UIÃæ°å")]
    private Transform _Panel;
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
        obj.transform.position = Camera.main.WorldToScreenPoint(pos);
        return obj;
    }
    public void HideUI(string name, GameObject ui)
    {
        ui.SetActive(false);
        _Pool[name].Push(ui);
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
            return group[0];
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