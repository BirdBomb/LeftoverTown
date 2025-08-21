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
    [Header("生命值")]
    public Text text_Hp;
    [Header("生命条")]
    public Transform bar_Hp;
    [Header("饥饿值")]
    public Text text_Food;
    [Header("饥饿条")]
    public Transform bar_Food;
    [Header("精神值")]
    public Text text_San;
    [Header("精神条")]
    public Transform bar_San;
    [Header("护甲")]
    public Text text_Armor;
    [Header("魔抗")]
    public Text text_Resistance;
    [Header("金币")]
    public Text text_Coin;
    private int val_Coin;
    [Header("悬赏")]
    public Text text_Fine;
    private int val_Fine;
    [Header("延迟")]
    public TextMeshProUGUI text_Ping;
    [Header("Buff预制")]
    public GameObject pref_BuffIcon;
    [Header("Buff图集")]
    public SpriteAtlas spriteAtlas_Buff;
    [Header("Buff面板")]
    public Transform transform_BuffPanel;
    private Dictionary<int, GameObject> BuffIconDic = new Dictionary<int, GameObject>();
    private List<int> BuffIconList = new List<int>();
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
        MessageBroker.Default.Receive<UIEvent.UIEvent_UpdateBuffList>().Subscribe(_ =>
        {
            ResetBuffIcon(_.buffList);
        }).AddTo(this);
        MessageBroker.Default.Receive<UIEvent.UIEvent_AddBuff>().Subscribe(_ =>
        {
            CreateBuffIcon(_.buffData);
        }).AddTo(this);
        MessageBroker.Default.Receive<UIEvent.UIEvent_SubBuff>().Subscribe(_ =>
        {
            DestroyBuffIcon(_.buffID);
        }).AddTo(this);

    }
    private void FixedUpdate()
    {
        text_Coin.text = val_Coin.ToString();
        text_Fine.text = val_Fine.ToString();
    }
    #region//Buff
    private void ResetBuffIcon(List<BuffData> buffDatas)
    {
        for(int i = 0; i< BuffIconList.Count; i++)
        {
            if (BuffIconDic.ContainsKey(BuffIconList[i]))
            {
                Destroy(BuffIconDic[BuffIconList[i]]);
            }
        }
        BuffIconDic.Clear();
        BuffIconList.Clear();
        for (int i = 0; i < buffDatas.Count; i++)
        {
            CreateBuffIcon(buffDatas[i]);
        }
    }
    private void CreateBuffIcon(BuffData buffData)
    {
        BuffConfig buffConfig = BuffConfigData.GetBuffConfig(buffData.BuffID);
        if (buffConfig.Buff_Icon && !BuffIconDic.ContainsKey(buffData.BuffID))
        {
            GameObject gameObject = Instantiate(pref_BuffIcon);
            UI_BuffIcon buffIcon = gameObject.GetComponent<UI_BuffIcon>();
            buffIcon.DrawIcon(spriteAtlas_Buff.GetSprite("Buff" + buffData.BuffID));
            BuffIconDic.Add(buffData.BuffID, gameObject);
            BuffIconList.Add(buffData.BuffID);
            gameObject.transform.SetParent(transform_BuffPanel);
            gameObject.transform.localScale = Vector3.one;
        }
    }
    private void DestroyBuffIcon(short buffID)
    {
        BuffConfig buffConfig = BuffConfigData.GetBuffConfig(buffID);
        if (buffConfig.Buff_Icon && BuffIconDic.ContainsKey(buffID))
        {
            Destroy(BuffIconDic[buffID]);
            BuffIconDic.Remove(buffID);
            BuffIconList.Remove(buffID);
        }
    }
    #endregion

}
