using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_BuffIcon : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public GameObject obj_Icon;
    public Image spriteRenderer_Icon;
    private string string_Desc;
    private bool bool_Pointing = false;
    public void InitIcon(Sprite sprite,string desc)
    {
        obj_Icon.transform.DOKill();
        obj_Icon.transform.localScale = Vector3.one;
        obj_Icon.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);

        spriteRenderer_Icon.sprite = sprite;
        string_Desc = desc;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        bool_Pointing = true;
        MessageBroker.Default.Publish(new UIEvent.UIEvent_ShowInfoTextUI()
        {
            anchor = eventData.position,
            text = string_Desc
        });
        obj_Icon.transform.DOKill();
        obj_Icon.transform.localScale = Vector3.one;
        obj_Icon.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        bool_Pointing = false;
        MessageBroker.Default.Publish(new UIEvent.UIEvent_HidenfoTextUI()
        {
            
        });
    }
    private void OnDisable()
    {
        if (bool_Pointing)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_HidenfoTextUI()
            {

            });
        }
    }
}
