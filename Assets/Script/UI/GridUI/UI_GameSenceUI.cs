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
using DG.Tweening;
using TMPro;

public class UI_GameSenceUI : UI_Grid
{
    [Header("手部槽位")]
    public UI_GridCell _handCellList;
    [Header("生命值")]
    public Text Text_Hp;
    [Header("最大生命值")]
    public Text Text_HpMax;
    [Header("饥饿值")]
    public Text Text_Food;
    [Header("最大饥饿值")]
    public Text Text_FoodMax;
    [Header("精神值")]
    public Text Text_San;
    [Header("最大精神值")]
    public Text Text_SanMax;
    [Header("护甲")]
    public TextMeshProUGUI Text_Armor;
    [Header("缺水")]
    public TextMeshProUGUI Text_Water;
    [Header("心情")]
    public TextMeshProUGUI Text_Happy;
    [Header("金币")]
    public TextMeshProUGUI Text_Coin;
    [Header("身份")]
    public TextMeshProUGUI Text_Status;
    [Header("悬赏")]
    public TextMeshProUGUI Text_Fine;

    private int _hp;
    private int _food;
    private int _water;
    private void Start()
    {
        MessageBroker.Default.Receive<UIEvent.UIEvent_UpdateItemInHand>().Subscribe(_ =>
        {
            UpdateHandSlot(_.itemData);
        }).AddTo(this);
        MessageBroker.Default.Receive<UIEvent.UIEvent_UpdateHPData>().Subscribe(_ =>
        {
            Text_Hp.text = _.HP.ToString();
            Text_Hp.transform.DOShakePosition(0.1f,5);
            Text_HpMax.text = _.MaxHP.ToString();
        }).AddTo(this);
        MessageBroker.Default.Receive<UIEvent.UIEvent_UpdateFoodData>().Subscribe(_ =>
        {
            Text_Food.text = _.Food.ToString();
            Text_Food.transform.DOShakePosition(0.1f, 5);
            Text_FoodMax.text = _.MaxFood.ToString();
        }).AddTo(this);
        MessageBroker.Default.Receive<UIEvent.UIEvent_UpdateSanData>().Subscribe(_ =>
        {
            Text_San.text = _.San.ToString();
            Text_San.transform.DOShakePosition(0.1f, 5);
            Text_SanMax.text = _.MaxSan.ToString();
        }).AddTo(this);
        MessageBroker.Default.Receive<UIEvent.UIEvent_UpdateArmorData>().Subscribe(_ =>
        {
            Text_Armor.text = _.Armor.ToString();
        }).AddTo(this);
        MessageBroker.Default.Receive<UIEvent.UIEvent_UpdateWaterData>().Subscribe(_ =>
        {
            Text_Water.text = _.Water.ToString();
        }).AddTo(this);
        MessageBroker.Default.Receive<UIEvent.UIEvent_UpdateHappyData>().Subscribe(_ =>
        {
            Text_Happy.text = _.Happy.ToString();
        }).AddTo(this);
        MessageBroker.Default.Receive<UIEvent.UIEvent_UpdateCoinData>().Subscribe(_ =>
        {
            Text_Coin.text = _.Coin.ToString();
        }).AddTo(this);
        MessageBroker.Default.Receive<UIEvent.UIEvent_UpdateStatus>().Subscribe(_ =>
        {
            Text_Status.text = StatusConfigData.GetStatusConfig(_.statusId).Status_Name.ToString();
        }).AddTo(this);
        MessageBroker.Default.Receive<UIEvent.UIEvent_UpdateFineData>().Subscribe(_ =>
        {
            Text_Fine.text = _.Fine.ToString();
        }).AddTo(this);

    }
    private void UpdateHandSlot(ItemData item)
    {
        _handCellList.UpdateGridCell(item);
    }
}
