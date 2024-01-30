using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DG.Tweening;

public class MyTile_Cabinet : MyTile
{
    GameObject signalObj;
    GameObject cabinetObj;

    private string itemInfo;
    public override void InitTile(int x,int y,Vector2 pos)
    {
        signalObj = null;
        cabinetObj = null;
        base.InitTile(x,y,pos);
    }
    public override void LoadTile(string json)
    {
        Debug.Log(json);
        itemInfo = json;
        base.LoadTile(json);
    }
    public override string SaveTile()
    {
        return itemInfo;
    }
    public override void UpdateTile(string json)
    {
        Debug.Log("地块更新" + json);
        itemInfo = json;
        base.UpdateTile(json);
    }
    public override MyTile ShowSignal()
    {
        if (!signalObj)
        {
            signalObj = UIManager.Instance.ShowUI("UI/UI_Signal", pos + new Vector2(0.5625f, 2));
            signalObj.transform.localScale = Vector3.zero;
            signalObj.transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.OutBack);
        }
        return this;
    }
    public override void HideSignal()
    {
        if (signalObj)
        {
            UIManager.Instance.HideUI("UI/UI_Signal", signalObj);
            signalObj = null;
        }
        if (cabinetObj)
        {
            UIManager.Instance.HideUI("UI/UI_Grid_0", cabinetObj);
            cabinetObj = null;
        }
        base.HideSignal();
    }
    public override void Interactive()
    {
        Debug.Log("打开盒子");
        if (!cabinetObj)
        {
            cabinetObj = UIManager.Instance.ShowUI("UI/UI_Grid_0", pos + new Vector2(0.5625f, 2));
            cabinetObj.transform.localScale = Vector3.zero;
            cabinetObj.transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.OutBack);

            cabinetObj.GetComponent<UI_Grid>().InitGrid(itemInfo,this);
        }
        if (signalObj)
        {
            UIManager.Instance.HideUI("UI/UI_Signal", signalObj);
            signalObj = null;
        }
        base.Interactive();
    }
#if UNITY_EDITOR
    // 下面是添加菜单项以创建 RoadTile 资源的 helper 函数
    [MenuItem("Assets/Create/MyTile/MyTile_Cabinet")]
    public static void CreateTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Road Tile", "New Road Tile", "Asset", "Save Road Tile", "Assets");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<MyTile_Cabinet>(), path);
    }
# endif
}
