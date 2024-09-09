using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image skillImage;
    public short skillID;
    public bool canDrag = true;
    public void Init(short id,Sprite sprite)
    {
        skillImage.sprite = sprite;
        skillID = id;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (canDrag)
        {
            skillImage.GetComponent<Canvas>().sortingOrder = 2;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canDrag)
        {
            skillImage.transform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (canDrag)
        {
            skillImage.GetComponent<Canvas>().sortingOrder = 1;
            skillImage.transform.localPosition = Vector3.zero;
            eventData.position = Input.mousePosition;
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raycastResults);
            if (raycastResults.Count > 0)
            {
                foreach (RaycastResult result in raycastResults)
                {
                    if (result.gameObject.name == "SkillBind")
                    {
                        GameLocalManager.Instance.localPlayer.BindUseSkill(skillID);
                        return;
                    }
                }
            }
        }
    }
}
