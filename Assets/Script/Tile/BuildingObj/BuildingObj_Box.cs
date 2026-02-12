using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using UnityEngine;

public class BuildingObj_Box : BuildingObj_Manmade
{
    public GameObject obj_SingalUI; 
    public GameObject obj_SingalAwakeUI;
    public GameObject obj_HightlightUI;
    [SerializeField]
    private GameObject prefab_UI;
    public List<ItemData> itemDatas_List = new List<ItemData>();
    private TileUI_Box tileUI_Bind;

    #region//信息更新与上传
    public override void All_UpdateInfo(string info)
    {
        if (tileUI_Bind)
        {
            ReadInfo(info);
        }
        base.All_UpdateInfo(info);
    }
    public void ReadInfo(string info)
    {
        itemDatas_List.Clear();
        string[] strings = info.Split("/");
        for (int i = 0; i < strings.Length; i++)
        {
            if (strings[i] != "")
            {
                ItemData data = JsonUtility.FromJson<ItemData>(strings[i]);
                itemDatas_List.Add(data);
            }
        }
        if (tileUI_Bind)
        {
            tileUI_Bind.DrawEveryCell();
        }
    }
    public void WriteInfo()
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < itemDatas_List.Count; i++)
        {
            if (i == 0)
            {
                builder.Append(JsonUtility.ToJson(itemDatas_List[i]));
            }
            else
            {
                builder.Append("/" + JsonUtility.ToJson(itemDatas_List[i]));
            }
        }
        Debug.Log(builder.ToString());
        Local_ChangeInfo(builder.ToString());
    }
    #endregion
    #region//瓦片交互
    public override void Local_ActorInputKeycode(ActorManager actor, KeyCode code)
    {
        if (code == KeyCode.F)
        {
            OpenOrCloseUI(tileUI_Bind == null);
        }
        base.Local_ActorInputKeycode(actor, code);
    }
    public override void Local_PlayerHighlight(bool on)
    {
        OpenOrCloseHighlightUI(on);
        base.Local_PlayerHighlight(on);
    }
    public override void Local_PlayerFaraway()
    {
        OpenOrCloseUI(false);
        base.Local_PlayerFaraway();
    }
    public override void OpenOrCloseHighlightUI(bool open)
    {
        obj_SingalUI.transform.DOKill();
        if (open)
        {
            obj_SingalUI.SetActive(true);
            obj_SingalUI.transform.localScale = Vector3.one;
            obj_SingalUI.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
        }
        else
        {
            obj_SingalUI.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
            {
                obj_SingalUI.SetActive(false);
            });
        }
        obj_HightlightUI.transform.DOKill();
        if (open)
        {
            obj_HightlightUI.SetActive(true);
            obj_HightlightUI.transform.localScale = Vector3.one;
            obj_HightlightUI.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
        }
        else
        {
            obj_HightlightUI.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
            {
                obj_HightlightUI.SetActive(false);
            });
        }
    }
    public override void OpenOrCloseAwakeUI(bool open)
    {
        obj_SingalAwakeUI.transform.DOKill();
        if (open)
        {
            obj_SingalAwakeUI.SetActive(true);
            obj_SingalAwakeUI.transform.localScale = Vector3.one;
            obj_SingalAwakeUI.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
        }
        else
        {
            obj_SingalAwakeUI.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
            {
                obj_SingalAwakeUI.SetActive(false);
            });
        }
    }
    public override void OpenOrCloseUI(bool open)
    {
        if (open)
        {
            UIManager.Instance.ShowTileUI(prefab_UI, out TileUI tileUI);
            tileUI_Bind = tileUI.GetComponent<TileUI_Box>();
            tileUI_Bind.BindBuilding(this);
            ReadInfo(info);
        }
        else
        {
            if (tileUI_Bind) UIManager.Instance.HideTileUI(tileUI_Bind);
            if (tileUI_Bind) tileUI_Bind = null;
        }
    }
    public override bool CanHighlight()
    {
        return true;
    }
    #endregion
}
