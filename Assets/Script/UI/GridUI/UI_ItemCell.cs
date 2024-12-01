using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class UI_ItemCell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private string str_itemName;
    private string str_itemDesc;
    private string str_itemInfoDesc;
    public Button btn_ItemCell;
    public Image image_ItemIcon;
    public Image image_ItemBG;
    public Text text_Info;
    private bool pointing = false;
    private bool hiding = true;
    public void Draw(Sprite iconSprite,Sprite bgSprite,int rarity, string infoStr,string nameStr,string descStr)
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
    public void Clean()
    {
        image_ItemIcon.enabled = false;
        image_ItemBG.enabled = false;
        text_Info.text = "";
        str_itemName = "";
        str_itemDesc = "";
        hiding = true;
        str_itemInfoDesc = "";
        if (pointing)
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
    private void OnDisable()
    {
        if (pointing)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_HidenfoTextUI()
            {

            });
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        pointing = true;
        if (!hiding)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_ShowInfoTextUI()
            {
                anchor = eventData.position,
                text = str_itemInfoDesc
            });
            transform.DOKill();
            transform.localScale = Vector3.one;
            transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.2f);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        pointing = false;
        if (!hiding)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_HidenfoTextUI()
            {

            });
            transform.DOKill();
            transform.localScale = Vector3.one;
        }
    }
}
