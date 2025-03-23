using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Common : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOKill();
        transform.localScale = Vector3.one;
        transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.2f);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOKill();
        transform.localScale = Vector3.one;
    }
}
