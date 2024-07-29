using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
public class UI_MainBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOKill();
        transform.GetComponent<RectTransform>().DOAnchorPosX(-150,0.2f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOKill();
        transform.GetComponent<RectTransform>().DOAnchorPosX(-100, 0.2f);
    }
}
