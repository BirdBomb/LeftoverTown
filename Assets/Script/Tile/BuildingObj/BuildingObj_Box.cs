using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class BuildingObj_Box : BuildingObj
{
    [SerializeField]
    private GameObject obj_Singal;
    [SerializeField]
    private GameObject prefab_UI;
    public List<ItemData> itemDatas_List = new List<ItemData>();
    private TileUI_Box tileUI_Bind;
    private bool bool_OpenUI = false;

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
        string[] strings = info.Split("/*I*/");
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
                builder.Append("/*I*/" + JsonUtility.ToJson(itemDatas_List[i]));
            }
        }
        Local_ChangeInfo(builder.ToString());
    }
    #endregion
    public override void All_ActorInputKeycode(ActorManager actor, KeyCode code)
    {
        if (code == KeyCode.F)
        {
            OpenOrCloseSingal(bool_OpenUI);
            OpenOrCloseCabinetUI(!bool_OpenUI);
        }
        base.All_ActorInputKeycode(actor, code);
    }
    public override bool All_PlayerHolding(PlayerCoreLocal player)
    {
        /*靠近是我自己*/
        if (player.bool_Local)
        {
            OpenOrCloseSingal(true);
            return true;
        }
        return false;
    }
    public override bool All_PlayerRelease(PlayerCoreLocal player)
    {
        /*离开是我自己*/
        if (player.bool_Local)
        {
            OpenOrCloseSingal(false);
            OpenOrCloseCabinetUI(false);
            return true;
        }
        return false;
    }
    private void OpenOrCloseSingal(bool open)
    {
        obj_Singal.transform.DOKill();
        if (open)
        {
            obj_Singal.SetActive(true);
            obj_Singal.transform.localScale = Vector3.one;
            obj_Singal.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
        }
        else
        {
            obj_Singal.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
            {
                obj_Singal.SetActive(false);
            });
        }
    }
    private void OpenOrCloseCabinetUI(bool open)
    {
        if (open)
        {
            bool_OpenUI = true;
            UIManager.Instance.ShowTileUI(prefab_UI, out TileUI tileUI);
            tileUI_Bind = tileUI.GetComponent<TileUI_Box>();
            tileUI_Bind.BindBuilding(this);
            ReadInfo(info);
        }
        else
        {
            bool_OpenUI = false;
            if (tileUI_Bind) UIManager.Instance.HideTileUI(tileUI_Bind);
            if (tileUI_Bind) tileUI_Bind = null;
        }
    }
}
