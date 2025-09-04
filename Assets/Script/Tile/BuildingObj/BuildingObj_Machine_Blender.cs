using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UniRx;
using UnityEngine;

public class BuildingObj_Machine_Blender : BuildingObj_Manmade
{
    public GameObject obj_SingalFUI;
    public GameObject obj_SingalAwakeUI;
    public GameObject obj_HightlightUI;
    [SerializeField]
    private GameObject prefab_UI;
    private TileUI_Blender tileUI_Bind = null;
    public ItemData itemData_From;
    public ItemData itemData_To;
    #region//��Ϣ�������ϴ�
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
        itemData_From = new ItemData();
        itemData_To = new ItemData();
        string[] strings = info.Split("/*I*/");
        for (int i = 0; i < strings.Length; i++)
        {
            if (strings[i] != "")
            {
                if (i == 0)
                {
                    itemData_From = JsonUtility.FromJson<ItemData>(strings[i]);
                }
                else if (i == 1)
                {
                    itemData_To = JsonUtility.FromJson<ItemData>(strings[i]);
                }
            }
        }
        if (tileUI_Bind)
        {
            tileUI_Bind.DrawAllCell();
        }
    }
    public void WriteInfo()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(JsonUtility.ToJson(itemData_From));
        builder.Append("/*I*/" + JsonUtility.ToJson(itemData_To));
        Local_ChangeInfo(builder.ToString());
    }
    #endregion
    #region//����
    public void Local_Blender()
    {
        if (itemData_From.Item_Count > 0 && itemData_From.Item_ID > 0)
        {
            BlenderConfig blenderConfig = BlenderConfigData.GetBlenderConfig(itemData_From.Item_ID);
            int toID = blenderConfig.blender_ToID;
            int toCount = blenderConfig.blender_ToCount;
            if (itemData_To.Item_ID == 0 || itemData_To.Item_ID == toID)
            {
                ItemData itemData_Expend = itemData_From;
                itemData_Expend.Item_Count = 1;
                itemData_From = GameToolManager.Instance.SplitItem(itemData_From, itemData_Expend);

                Type type = Type.GetType("Item_" + toID.ToString());
                ((ItemBase)Activator.CreateInstance(type)).StaticAction_InitData((short)toID, out ItemData initData);
                initData.Item_Count = (short)toCount;
                if (itemData_To.Item_ID == 0)
                {
                    itemData_To = initData;
                }
                else if (itemData_To.Item_ID == toID)
                {
                    itemData_To = GameToolManager.Instance.CombineItem(itemData_To, initData, out ItemData itemData_Res);
                    if (itemData_Res.Item_Count > 0 && itemData_Res.Item_ID > 0)
                    {
                        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Add()
                        {
                            index = 0,
                            itemData = itemData_Res,
                        });
                    }
                }
            }
            WriteInfo();
        }
    }
    #endregion
    #region//��Ƭ����
    public override void All_ActorInputKeycode(ActorManager actor, KeyCode code)
    {
        if (code == KeyCode.F)
        {
            OpenOrCloseUI(tileUI_Bind == null);
        }
        base.All_ActorInputKeycode(actor, code);
    }
    public override void All_PlayerHighlight(bool on)
    {
        OpenOrCloseHighlightUI(on);
        base.All_PlayerHighlight(on);
    }
    public override void All_PlayerFaraway()
    {
        OpenOrCloseUI(false);
        base.All_PlayerFaraway();
    }
    public override void OpenOrCloseHighlightUI(bool open)
    {
        obj_SingalFUI.transform.DOKill();
        if (open)
        {
            obj_SingalFUI.SetActive(true);
            obj_SingalFUI.transform.localScale = Vector3.one;
            obj_SingalFUI.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
        }
        else
        {
            obj_SingalFUI.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
            {
                obj_SingalFUI.SetActive(false);
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
            tileUI_Bind = tileUI.GetComponent<TileUI_Blender>();
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
