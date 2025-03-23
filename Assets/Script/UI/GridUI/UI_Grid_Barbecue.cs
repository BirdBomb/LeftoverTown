using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Grid_Barbecue : UI_Grid
{
    [SerializeField, Header("烧烤格子")]
    private UI_GridCell gridCell_Barbecue;
    private ItemData itemData_Barbecue;
    [SerializeField, Header("烧烤进度条")]
    private Image image_BarbecueBar;
    private short barbecueVal;
    private Action<ItemData, short> action_AddBarbecue;
    public void BindAction_AddBarbecueAction(Action<ItemData, short> action)
    {
        action_AddBarbecue = action;
    }
    private void Start()
    {
        BindAllCell();
    }
    private void BindAllCell()
    {
        gridCell_Barbecue.BindAction(PutIn, PutOut, null, null);
    }
    #region//信息更新与上传
    public void UpdateInfo(short barbecueVal, short barbecueMax, ItemData barbecue)
    {
        itemData_Barbecue = barbecue;
        if (itemData_Barbecue.Item_ID > 0) { gridCell_Barbecue.UpdateData(itemData_Barbecue); }
        else { gridCell_Barbecue.CleanData(); }
        if (barbecueMax != 0)
        {
            image_BarbecueBar.transform.DOKill();
            image_BarbecueBar.transform.DOScaleX(((float)barbecueVal / barbecueMax), 0.1f);
            if (barbecueVal == barbecueMax)
            {
                gridCell_Barbecue.transform.DOKill();
                gridCell_Barbecue.transform.localScale = Vector3.one;
                gridCell_Barbecue.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
            }
        }
        else
        {
            image_BarbecueBar.transform.localScale = new Vector3(0, 1f, 1f);
        }
    }
    public void ChangeInfo()
    {
        if (action_AddBarbecue != null)
        {
            action_AddBarbecue.Invoke(itemData_Barbecue, barbecueVal);
        };
        if (action_ChangeInfo != null)
        {
            action_ChangeInfo.Invoke("");
        }
    }
    #endregion
    public void PutIn(ItemData data)
    {
        BarbecueConfig barbecue = BarbecueConfigData.GetBarbecueConfig(data.Item_ID);
        if(barbecue.BarbecueID != 0)
        {
            itemData_Barbecue = GameToolManager.Instance.PutInItemSingle(itemData_Barbecue, data, out data);
        }
        if (data.Item_ID > 0 && data.Item_Count != 0)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
            {
                item = data,
            });
        }
        ChangeInfo();
    }
    public ItemData PutOut(ItemData data)
    {
        itemData_Barbecue = GameToolManager.Instance.PutOutItemSingle(itemData_Barbecue, data);
        ChangeInfo();
        return itemData_Barbecue;
    }
}
