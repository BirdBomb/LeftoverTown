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

public class UI_GridCell : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField, Header("图标图集")]
    private SpriteAtlas spriteAtlas_ItemIcon;
    [SerializeField, Header("背景图集")]
    private SpriteAtlas spriteAtlas_ItemBG;
    [SerializeField, Header("图标背景")]
    private Image image_IconBG;
    [SerializeField, Header("图标主要")]
    private Image image_IconMain;
    [SerializeField, Header("冷冻遮罩")]
    private Image image_IconMask;
    [SerializeField, Header("角标")]
    private Text text_CornerMark;
    [SerializeField, Header("子Grid")]
    public UI_Grid_Child grid_Child;
    [SerializeField, Header("滑动条背景")]
    private Transform panel_Bar;
    [SerializeField, Header("滑动条填充")]
    private Image image_Bar;
    private bool _sleeping = false;
    private bool _pointing = false;

    private Action<ItemData> action_PutIn;
    private Func<ItemData, ItemData> action_PutOut;
    private Action<UI_GridCell> action_ClickLeft;
    private Action<UI_GridCell> action_ClickRight;
    private string str_itemName = "";
    private string str_itemDesc = "";
    private string str_itemInfo = "";

    public ItemBase _bindItemBase = null;
    public void OnDisable()
    {
        if (_pointing)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_HidenfoTextUI()
            {

            });
        }
    }

    #region//UI绑定
    public void BindAction(Action<ItemData> putIn, Func<ItemData, ItemData> putOut, Action<UI_GridCell> clickLeft, Action<UI_GridCell> clickRight)
    {
        action_PutIn = putIn;
        action_PutOut = putOut;
        action_ClickLeft = clickLeft;
        action_ClickRight = clickRight;
    }
    #endregion
    #region//更新格子
    /// <summary>
    /// 更新数据
    /// </summary>
    public virtual ItemBase UpdateData(ItemData data)
    {
        if (_bindItemBase == null || _bindItemBase.itemData.Item_ID != data.Item_ID)
        {
            ResetCell();
            BindItemBase(data);
            UpdateItemBase(data);
            ColourCell(_bindItemBase.itemConfig.Item_Name, _bindItemBase.itemConfig.Item_Desc, _bindItemBase.itemConfig.ItemRarity);
        }
        else
        {
            UpdateItemBase(data);
        }
        return _bindItemBase;
    }
    /// <summary>
    /// 清空数据
    /// </summary>
    public virtual void CleanData()
    {
        if(_bindItemBase != null)
        {
            _bindItemBase = null;
            ResetCell();
        }
    }
    /// <summary>
    /// 绑定实例
    /// </summary>
    /// <param name="data"></param>
    private void BindItemBase(ItemData data)
    {
        Type type = Type.GetType("Item_" + data.Item_ID.ToString());
        if (type != null) _bindItemBase = (ItemBase)Activator.CreateInstance(type);
        else _bindItemBase = null;
    }
    /// <summary>
    /// 更新实例
    /// </summary>
    /// <param name="data"></param>
    private void UpdateItemBase(ItemData data)
    {
        _bindItemBase.UpdateDataFromNet(data);
        _bindItemBase.DrawGridCell(this);
        if (grid_Child)
        {
            grid_Child.UpdateGrid(data);
            grid_Child.DrawCell(data);
        }
    }
    #endregion
    #region//绘制格子
    /// <summary>
    /// 绘制格子
    /// </summary>
    /// <param name="mainIcon"></param>
    /// <param name="childIcon"></param>
    /// <param name="backGround"></param>
    /// <param name="name"></param>
    /// <param name="info"></param>
    public void DrawCell(string mainIcon, string backGround, string name, string info)
    {
        image_IconBG.gameObject.SetActive(true);
        image_IconMain.sprite = spriteAtlas_ItemIcon.GetSprite(mainIcon);
        image_IconBG.sprite = spriteAtlas_ItemBG.GetSprite(backGround);
        str_itemName = name;
        text_CornerMark.text = info;
    }
    /// <summary>
    /// 重设格子
    /// </summary>
    private void ResetCell()
    {
        FreezeCell(false);
        SleepCell(false);
        CleanCell();
    }
    /// <summary>
    /// 清空格子
    /// </summary>
    public void CleanCell()
    {
        text_CornerMark.text = "";
        str_itemName = "";
        str_itemDesc = "";
        str_itemInfo = "";
        image_IconMain.sprite = spriteAtlas_ItemIcon.GetSprite("Item_Default");
        image_IconBG.gameObject.SetActive(false);
        panel_Bar.gameObject.SetActive(false);
        image_Bar.transform.localScale = Vector3.one;
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
    /// 染色格子
    /// </summary>
    /// <param name="nameStr"></param>
    /// <param name="descStr"></param>
    /// <param name="rarity"></param>
    private void ColourCell(string nameStr, string descStr, int rarity)
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
        str_itemInfo = str_itemName + "\n" + str_itemDesc;
    }
    /// <summary>
    /// 冰冻格子
    /// </summary>
    /// <param name="freeze"></param>
    public void FreezeCell(bool freeze)
    {
        if (freeze) 
        {
            image_Bar.color = new Color(0.5f, 1, 1, 1);
        }
        image_IconMask.gameObject.SetActive(freeze);
    }
    /// <summary>
    /// 休眠格子
    /// </summary>
    /// <param name="sleep"></param>
    public void SleepCell(bool sleep)
    {
        if (sleep)
        {
            image_IconMain.color = new Color(1, 0.5f, 0.5f, 0.5f);
            text_CornerMark.color = new Color(1, 0.25f, 0.25f, 1);
        }
        else
        {
            image_IconMain.color = new Color(1, 1, 1, 1);
            text_CornerMark.color = new Color(1, 1, 1, 1);
        }
        _sleeping = sleep;
    }
    #endregion
    #region//滑动条
    public void SetSliderVal(float val)
    {
        panel_Bar.gameObject.SetActive(true);
        image_Bar.transform.localScale = new Vector3(val, 1, 1);
    }
    public void SetSliderColor(Color color)
    {
        image_Bar.color = color;
    }
    #endregion
    #region//UI交互
    /// <summary>
    /// 鼠标点击
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (_sleeping)
        {
            return;
        }
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            ClickLeft();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            ClickRight();
        }
    }
    /// <summary>
    /// 鼠标移入
    /// </summary>
    /// <param name="eventData"></param>
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
                text = str_itemInfo
            });
        }
    }
    /// <summary>
    /// 鼠标移出
    /// </summary>
    /// <param name="eventData"></param>
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
    /// <summary>
    /// 鼠标左击
    /// </summary>
    private void ClickLeft()
    {
        if (action_ClickLeft != null)
        {
            action_ClickLeft.Invoke(this);
        }
        Keeping();
    }
    /// <summary>
    /// 鼠标右击
    /// </summary>
    private void ClickRight()
    {
        if (action_ClickRight != null)
        {
            action_ClickRight.Invoke(this);
        }
    }
    /// <summary>
    /// 放入
    /// </summary>
    /// <param name="data"></param>
    public void PutIn(ItemData data)
    {
        if (action_PutIn != null)
        {
            action_PutIn.Invoke(data);
        }
    }
    /// <summary>
    /// 取出
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public ItemData PutOut(ItemData data)
    {
        if(action_PutOut != null)
        {
            return action_PutOut.Invoke(data);
        }
        else
        {
            return new ItemData();
        }
    }
    /// <summary>
    /// 鼠标持有
    /// </summary>
    public void Keeping()
    {
        ItemData temp = new ItemData();
        if (_bindItemBase != null)
        {
            temp = _bindItemBase.itemData;
            temp.Item_Count = (Input.GetKey(KeyCode.LeftControl)) ? (short)(Mathf.FloorToInt(_bindItemBase.itemData.Item_Count * 0.5f)) : _bindItemBase.itemData.Item_Count;
        }
        MessageBroker.Default.Publish(new UIEvent.UIEvent_StartKeepingItem()
        {
            itemCell = this,
            itemData = temp
        });
    }
    #endregion
}
