using DG.Tweening;
using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Debug = UnityEngine.Debug;
using Image = UnityEngine.UI.Image;

public class UI_GridCell : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler,IPointerExitHandler
{
    [Header("图标图集")]
    public SpriteAtlas atlas_ItemIcon;
    [Header("背景图集")]
    public SpriteAtlas atlas_ItemBG;
    [Header("图标背景")]
    public Image image_BackGround;
    [Header("图标主要")]
    public Image image_MainIcon;
    [Header("图标次要")]
    public Image image_ChildIcon;
    [Header("图标遮罩(冰冻)")]
    public Image image_IceMask;
    [Header("信息文本")]
    public Text text_Info;
    [Header("UI按钮")]
    public Button btn_Main;
    [Header("UI子集")]
    public UI_Grid_Child grid_Child;
    [Header("滑动条背景")]
    public Transform panel_Bar;
    [Header("滑动条填充")]
    public Image image_Bar;
    private bool _sleeping = false;
    private bool _pointing = false;
    private void Awake()
    {
        Bind();
    }

    public ItemBase _bindItem;
    public int BindID
    {
        get { return bindID; }
        set
        {
            if (bindID != value)
            {
                bindID = value;
                if (bindID != 0)
                {
                    Type type = Type.GetType("Item_" + bindID.ToString());
                    _bindItem = (ItemBase)Activator.CreateInstance(type);
                }
                else
                {
                    _bindItem = null;
                }
            }
        }
    }
    private int bindID;
    private Action<UI_GridCell> _bindClickLeft;
    private Action<UI_GridCell> _bindClickRight;
    private Action<UI_GridCell, ItemData, PointerEventData> _bindDragBegin;
    private Action<UI_GridCell, ItemData, PointerEventData> _bindDraging;
    private Action<UI_GridCell, ItemData, PointerEventData> _bindDragEnd;
    private string str_itemName = "";
    private string str_itemDesc = "";
    private string str_itemInfoDesc = "";
    #region//UI绑定
    private void Bind()
    {
        btn_Main.onClick.AddListener(ClickLeft);
    }
    public virtual void BindClickAction(Action<UI_GridCell> clickLeft, Action<UI_GridCell> clickRight)
    {
        _bindClickLeft = clickLeft;
        _bindClickRight = clickRight;
    }
    public virtual void BindDragAction(Action<UI_GridCell, ItemData, PointerEventData> dragBegin, Action<UI_GridCell, ItemData, PointerEventData> dragIn, Action<UI_GridCell, ItemData, PointerEventData> dragEnd)
    {
        _bindDragBegin = dragBegin;
        _bindDraging = dragIn;
        _bindDragEnd = dragEnd;
    }
    #endregion
    #region//UI交互
    public void OnDisable()
    {
        if (_pointing)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_HidenfoTextUI()
            {

            });
        }
    }
    public void ClickLeft()
    {
        if (_bindClickLeft != null && !_sleeping)
        {
            _bindClickLeft.Invoke(this);
        }
    }
    public void ClickRight()
    {
        if (_bindClickRight != null && !_sleeping)
        {

        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_bindDragBegin != null && !_sleeping)
        {
            _bindDragBegin.Invoke(this, _bindItem.itemData, eventData);
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (_bindDraging != null && !_sleeping)
        {
            _bindDraging.Invoke(this, _bindItem.itemData, eventData);
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        image_MainIcon.transform.localPosition = Vector3.zero;
        if (_bindDragEnd != null && !_sleeping)
        {
            _bindDragEnd.Invoke(this, _bindItem.itemData, new PointerEventData(EventSystem.current));
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _pointing = true;
        if (!_sleeping)
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
        _pointing = false;
        if (!_sleeping)
        {
            transform.DOKill();
            transform.localScale = Vector3.one;
            MessageBroker.Default.Publish(new UIEvent.UIEvent_HidenfoTextUI()
            {

            });
        }
    }
    #endregion
    #region//更新格子
    /// <summary>
    /// 更新物品格子
    /// </summary>
    public virtual ItemBase UpdateGridCell(ItemData data)
    {
        if (data.Item_ID == 0)
        {
            BindID = data.Item_ID;
            ResetGridCell();
        }
        else
        {
            if (BindID != data.Item_ID)
            {
                ResetGridCell();
                BindID = data.Item_ID;
                _bindItem.UpdateDataFromNet(data);
                _bindItem.DrawGridCell(this);
                Colour(_bindItem.itemConfig.Item_Name, _bindItem.itemConfig.Item_Desc, _bindItem.itemConfig.ItemRarity);
                str_itemInfoDesc = str_itemName + "\n" + str_itemDesc;
            }
            else
            {
                BindID = data.Item_ID;
                _bindItem.UpdateDataFromNet(data);
                _bindItem.DrawGridCell(this);
            }
        }
        return _bindItem;
    }
    /// <summary>
    /// 重设物品格子
    /// </summary>
    public void ResetGridCell()
    {
        FreezeCell(false);
        SleepCell(false);
        CleanCell();
    }
    private void Colour(string nameStr, string descStr, int rarity)
    {
        str_itemName = nameStr;
        str_itemDesc = descStr;

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
            str_itemName = "<color=#d507c6>" + str_itemName + "</color>";
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
    #endregion
    #region//格子状态
    /// <summary>
    /// 冰冻
    /// </summary>
    /// <param name="freeze"></param>
    public void FreezeCell(bool freeze)
    {
        if (freeze) 
        {
            image_Bar.color = new Color(0.5f, 1, 1, 1);
        }
        image_IceMask.gameObject.SetActive(freeze);
    }
    /// <summary>
    /// 休眠
    /// </summary>
    /// <param name="sleep"></param>
    public void SleepCell(bool sleep)
    {
        if (sleep)
        {
            image_MainIcon.color = new Color(1, 0.5f, 0.5f, 0.5f);
            image_ChildIcon.color = new Color(1, 0.5f, 0.5f, 0.5f);
            text_Info.color = new Color(1, 0.25f, 0.25f, 1);
        }
        else
        {
            image_MainIcon.color = new Color(1, 1, 1, 1);
            image_ChildIcon.color = new Color(1, 1, 1, 1);
            text_Info.color = new Color(1, 1, 1, 1);
        }
        _sleeping = sleep;
    }
    /// <summary>
    /// 清空
    /// </summary>
    public void CleanCell()
    {
        text_Info.text = "";
        str_itemName = "";
        str_itemDesc = "";
        str_itemInfoDesc = "";
        image_MainIcon.sprite = atlas_ItemIcon.GetSprite("Item_Default");
        image_ChildIcon.sprite = atlas_ItemIcon.GetSprite("Item_Default");
        image_BackGround.gameObject.SetActive(false);
        panel_Bar.gameObject.SetActive(false);
        image_Bar.transform.localScale = Vector3.one;
        _bindClickLeft = null;
        _bindClickRight = null;
        _bindDragBegin = null;
        _bindDraging = null;
        _bindDragEnd = null;
        if (_pointing)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_HidenfoTextUI()
            {

            });
        }
        if (grid_Child)
        {
            grid_Child.CloseGrid();
        }
    }
    /// <summary>
    /// 绘制
    /// </summary>
    /// <param name="mainIcon"></param>
    /// <param name="childIcon"></param>
    /// <param name="backGround"></param>
    /// <param name="name"></param>
    /// <param name="info"></param>
    public void DrawCell(string mainIcon, string childIcon, string backGround, string name, string info)
    {
        image_BackGround.gameObject.SetActive(true);
        image_MainIcon.sprite = atlas_ItemIcon.GetSprite(mainIcon);
        image_ChildIcon.sprite = atlas_ItemIcon.GetSprite(childIcon);
        image_BackGround.sprite = atlas_ItemBG.GetSprite(backGround);
        text_Info.text = info;
    }
    #endregion
}
