using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using UnityEngine.U2D;
using UnityEngine.XR;
using static Unity.Collections.Unicode;
using UnityEngine.EventSystems;
using System;

public class UI_GameSenceUI : UI_Grid
{
    [Header("手部槽位")]
    public UI_GridCell _handCellList;
    [Header("生命值")]
    public Text Text_Hp;
    [Header("饥饿值")]
    public Text Text_Food;
    [Header("缺水值")]
    public Text Text_Water;

    private int _hp;
    private int _food;
    private int _water;
    private void Start()
    {
        MessageBroker.Default.Receive<UIEvent.UIEvent_UpdateItemInHand>().Subscribe(_ =>
        {
            UpdateHandSlot(_.itemData);
        }).AddTo(this);
        MessageBroker.Default.Receive<UIEvent.UIEvent_UpdateData>().Subscribe(_ =>
        {
            Text_Hp.text = _.HP.ToString();
        }).AddTo(this);
    }
    private void UpdateHandSlot(ItemData item)
    {
        _handCellList.UpdateGridCell(item);
    }
}
