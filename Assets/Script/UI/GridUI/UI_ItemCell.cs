using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class UI_ItemCell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    private string str_itemName;
    private string str_itemDesc;
    private string str_itemInfoDesc;
    public Transform transform_Mask;
    public Image image_ItemIcon;
    public Image image_ItemBG;
    public Text text_Info;
    private bool hiding = true;

    public void DrawCell(Sprite iconSprite,Sprite bgSprite,int rarity, string infoStr,string nameStr,string descStr)
    {
        image_ItemIcon.enabled = true;
        image_ItemBG.enabled = true;
        image_ItemIcon.sprite = iconSprite;
        image_ItemBG.sprite = bgSprite;
        text_Info.text = infoStr;
        str_itemName = nameStr;
        str_itemDesc = descStr;
        hiding = false;
        Colour(rarity);
        str_itemInfoDesc = str_itemName + "\n" + str_itemDesc;
    }
    public void DrawCellIcon(Sprite iconSprite, Color iconColor, Sprite bgSprite, Color bgColor)
    {
        image_ItemIcon.enabled = true;
        image_ItemBG.enabled = true;
        image_ItemIcon.sprite = iconSprite;
        image_ItemIcon.color = iconColor;
        image_ItemBG.sprite = bgSprite;
        image_ItemBG.color = bgColor;
    }
    public void DrawCellInfo(string infoStr, string nameStr, string descStr, int rarity)
    {
        text_Info.text = infoStr;
        str_itemName = nameStr;
        str_itemDesc = descStr;
        Colour(rarity);
        str_itemInfoDesc = str_itemName + "\n" + str_itemDesc;
        hiding = false;
    }
    public void CleanCell()
    {
        image_ItemIcon.enabled = false;
        image_ItemBG.enabled = false;
        text_Info.text = "";
        str_itemName = "";
        str_itemDesc = "";
        hiding = true;
        str_itemInfoDesc = "";
        if (bool_Pointing)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_HidenfoTextUI()
            {

            });
        }
    }
    private void Colour(int rarity)
    {
        if (rarity == 0)
        {
            str_itemName = "<color=#9A9A9A>" + str_itemName + "</color>";
        }
        else if (rarity == 1)
        {
            str_itemName = "<color=#43C743>" + str_itemName + "</color>";
        }
        else if (rarity == 2)
        {
            str_itemName = "<color=#4487C7>" + str_itemName + "</color>";
        }
        else if (rarity == 3)
        {
            str_itemName = "<color=#4487C7>" + str_itemName + "</color>";
        }
        else if (rarity == 4)
        {
            str_itemName = "<color=#FF9D09>" + str_itemName + "</color>";
        }
        else if (rarity == 5)
        {
            str_itemName = "<color=#FF090E>" + str_itemName + "</color>";
        }
        else if (rarity == 6)
        {
            str_itemName = "<color=#D59DD6>" + str_itemName + "</color>";
        }

    }
    #region//½»»¥
    private Action action_Click;
    private Action action_Press;
    private bool bool_Pointing = false;
    private bool bool_Pressing = false;
    private float float_PressingTime = 0;
    public void BindAction(Action clickAct, Action pressAct)
    {
        action_Click = clickAct; action_Press = pressAct;
    }
    public void CleanAction()
    {
        action_Click = null; action_Press = null;
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
    private void FixedUpdate()
    {
        if (bool_Pressing )
        {
            if (float_PressingTime > 1)
            {
                if (action_Press != null) action_Press.Invoke();
                float_PressingTime = 0;
            }
            else
            {
                float_PressingTime += Time.fixedDeltaTime;
            }
        }
        else
        {
            if (float_PressingTime > 0)
            {
                float_PressingTime -= Time.fixedDeltaTime * 0.5f;
                if (float_PressingTime < 0) float_PressingTime = 0;
            }
        }
        if (action_Press != null) { transform_Mask.localScale = new Vector3(1, float_PressingTime, 1); }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        bool_Pointing = true;
        if (!hiding && !str_itemInfoDesc.Equals(""))
        {
            transform.DOKill();
            transform.localScale = Vector3.one;
            transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.2f);
            MessageBroker.Default.Publish(new UIEvent.UIEvent_ShowInfoTextUI()
            {
                anchor = eventData.position,
                text = str_itemInfoDesc
            });
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        bool_Pointing = false;
        if (!hiding)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_HidenfoTextUI()
            {

            });
            transform.DOKill();
            transform.localScale = Vector3.one;
        }
    }
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (!bool_Pressing) { image_ItemIcon.transform.DOShakePosition(50, 5); }
        bool_Pressing = true;
    }
    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        if (bool_Pressing) { image_ItemIcon.transform.DOKill(); image_ItemIcon.transform.localPosition = Vector3.zero; }
        bool_Pressing = false;
    }
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (action_Click != null)
        {
            action_Click.Invoke();
        }
    }
    #endregion
}
