using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class GameUI_Status : MonoBehaviour
{
    [Header("����ֵ")]
    public Text text_Hp;
    [Header("������")]
    public Transform bar_Hp;
    [Header("����ֵ")]
    public Text text_Food;
    [Header("������")]
    public Transform bar_Food;
    [Header("����ֵ")]
    public Text text_San;
    [Header("������")]
    public Transform bar_San;
    [Header("����")]
    public Text text_Armor;
    [Header("ħ��")]
    public Text text_Resistance;
    [Header("���")]
    public Text text_Coin;
    private int val_Coin;
    [Header("����")]
    public Text text_Fine;
    private int val_Fine;
    [Header("�ӳ�")]
    public TextMeshProUGUI text_Ping;
    [Header("BuffԤ��")]
    public GameObject pref_BuffIcon;
    [Header("Buffͼ��")]
    public SpriteAtlas spriteAtlas_Buff;
    [Header("Buff���")]
    public Transform transform_BuffPanel;
    private Dictionary<int, GameObject> dic_BuffPool = new Dictionary<int, GameObject>();

    private void Start()
    {
        MessageBroker.Default.Receive<UIEvent.UIEvent_UpdateHPData>().Subscribe(_ =>
        {
            text_Hp.text = _.HP_Cur.ToString();
            text_Hp.transform.DOShakePosition(0.1f, 5);
            bar_Hp.DOKill();
            bar_Hp.DOScaleY((float)_.HP_Cur / (float)_.HP_Max, 0.1f);
        }).AddTo(this);
        MessageBroker.Default.Receive<UIEvent.UIEvent_UpdateFoodData>().Subscribe(_ =>
        {
            text_Food.text = _.Food_Cur.ToString();
            text_Food.transform.DOShakePosition(0.1f, 5);
            bar_Food.DOKill();
            bar_Food.DOScaleY((float)_.Food_Cur / (float)_.Food_Max, 0.1f);
        }).AddTo(this);
        MessageBroker.Default.Receive<UIEvent.UIEvent_UpdateSanData>().Subscribe(_ =>
        {
            text_San.text = _.San_Cur.ToString();
            text_San.transform.DOShakePosition(0.1f, 5);
            bar_San.DOKill();
            bar_San.DOScaleY((float)_.San_Cur / (float)_.San_Max, 0.1f);
        }).AddTo(this);
        MessageBroker.Default.Receive<UIEvent.UIEvent_UpdateArmorData>().Subscribe(_ =>
        {
            text_Armor.text = _.Armor.ToString();
        }).AddTo(this);
        MessageBroker.Default.Receive<UIEvent.UIEvent_UpdateResistanceData>().Subscribe(_ =>
        {
            text_Resistance.text = _.Resistance.ToString();
        }).AddTo(this);
        MessageBroker.Default.Receive<UIEvent.UIEvent_UpdateCoinData>().Subscribe(_ =>
        {
            DOTween.To(() => val_Coin, x => val_Coin = x, _.Coin, 1).OnComplete(() =>
            {
                text_Coin.transform.localScale = Vector3.one;
                text_Coin.transform.rotation = Quaternion.identity;
                text_Coin.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0), 0.1f);
                text_Coin.transform.DOShakeRotation(0.1f, new Vector3(0, 0, 30));
            });
        }).AddTo(this);
        MessageBroker.Default.Receive<UIEvent.UIEvent_UpdateFineData>().Subscribe(_ =>
        {
            DOTween.To(() => val_Fine, x => val_Fine = x, _.Fine, 1).OnComplete(() =>
            {
                text_Fine.transform.localScale = Vector3.one;
                text_Fine.transform.rotation = Quaternion.identity;
                text_Fine.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0), 0.1f);
                text_Fine.transform.DOShakeRotation(0.1f, new Vector3(0, 0, 30));
            });
        }).AddTo(this);
        MessageBroker.Default.Receive<UIEvent.UIEvent_UpdatePing>().Subscribe(_ =>
        {
            text_Ping.text = ((int)(_.ping * 1000)).ToString();
        }).AddTo(this);
        MessageBroker.Default.Receive<UIEvent.UIEvent_AddBuff>().Subscribe(_ =>
        {
            CreateBuffIcon(_.buffConfig);
        }).AddTo(this);
        MessageBroker.Default.Receive<UIEvent.UIEvent_SubBuff>().Subscribe(_ =>
        {
            DestroyBuffIcon(_.buffConfig);
        }).AddTo(this);

    }
    private void FixedUpdate()
    {
        text_Coin.text = val_Coin.ToString();
        text_Fine.text = val_Fine.ToString();
    }
    private void CreateBuffIcon(BuffConfig buffConfig)
    {
        if (buffConfig.Buff_Icon)
        {
            GameObject gameObject = Instantiate(pref_BuffIcon);
            gameObject.GetComponent<UI_BuffIcon>().Draw(spriteAtlas_Buff.GetSprite("Buff" + buffConfig.Buff_ID), "Name", "Desc");
            dic_BuffPool.Add(buffConfig.Buff_ID, gameObject);
            gameObject.transform.SetParent(transform_BuffPanel);
            gameObject.transform.localScale = Vector3.one;
        }
    }
    private void DestroyBuffIcon(BuffConfig buffConfig)
    {
        if (dic_BuffPool.ContainsKey(buffConfig.Buff_ID))
        {
            Destroy(dic_BuffPool[buffConfig.Buff_ID]);
            dic_BuffPool.Remove(buffConfig.Buff_ID);
        }
    }

}
