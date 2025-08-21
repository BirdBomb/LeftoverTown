using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;
using DG.Tweening;

public class GameUI_Mask : MonoBehaviour
{
    public GameObject gameObject_Mask;
    private float count;
    void Start()
    {
        MessageBroker.Default.Receive<UIEvent.UIEvent_OpenReviveCountdown>().Subscribe(_ =>
        {
            OpenReviveCountDown(_.time);

        }).AddTo(this);
        MessageBroker.Default.Receive<UIEvent.UIEvent_CloseReviveCountdown>().Subscribe(_ =>
        {
            CloseReviveCountDown();

        }).AddTo(this);
    }
    #region//¸´»îµ¹¼ÆÊ±
    public Transform trans_RevivePanel;
    public TextMeshProUGUI text_ReviveCount;

    public void OpenReviveCountDown(float time)
    {
        gameObject_Mask.gameObject.SetActive(true);
        trans_RevivePanel.DOKill();
        trans_RevivePanel.localScale = Vector3.one;
        trans_RevivePanel.DOPunchScale(new Vector3(0, 0.2f, 0), 0.2f);
        count = time;
        if (IsInvoking("CountDown"))
        {
            CancelInvoke("CountDown");
        }
        InvokeRepeating("CountDown", 0f, 1);
    }
    public void CloseReviveCountDown()
    {
        gameObject_Mask.gameObject.SetActive(false);
        trans_RevivePanel.DOKill();
        trans_RevivePanel.localScale = Vector3.zero;
        if (IsInvoking("CountDown"))
        {
            CancelInvoke("CountDown");
        }
    }
    public void CountDown()
    {
        if (count > 0)
        {
            text_ReviveCount.text = count.ToString();
            text_ReviveCount.transform.DOKill();
            text_ReviveCount.transform.localScale = Vector3.one;
            text_ReviveCount.transform.DOPunchScale(new Vector3(0, 0.2f, 0), 0.2f);
            count--;
        }
        else
        {
            CloseReviveCountDown();
        }
    }

    #endregion
}
