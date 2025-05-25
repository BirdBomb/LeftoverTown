using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_BuffIcon : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public GameObject obj_Icon;
    public Image spriteRenderer_Icon;
    public GameObject obj_Desc;
    public TextMeshProUGUI text_Desc;
    public void Draw(Sprite sprite,string buffName,string buffDesc)
    {
        obj_Icon.transform.DOKill();
        obj_Icon.transform.localScale = Vector3.one;
        obj_Icon.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);

        spriteRenderer_Icon.sprite = sprite;
        text_Desc.text = buffName + "\n" + buffDesc;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        obj_Icon.transform.DOKill();
        obj_Icon.transform.localScale = Vector3.one;
        obj_Icon.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);

        obj_Desc.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        obj_Desc.SetActive(false);
    }
}
