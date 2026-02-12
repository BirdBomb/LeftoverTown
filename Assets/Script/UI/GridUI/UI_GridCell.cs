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
    [SerializeField, Header("滑动条背景")]
    private Transform panel_Bar;
    [SerializeField, Header("滑动条填充")]
    private Image image_Bar;
    private bool bool_Pointing = false;
    private bool bool_Showing = false;

    private Action<ItemData,ItemPath> action_PutIn;
    private Func<ItemData, ItemData, ItemPath, ItemData> action_PutOut;
    private Action<UI_GridCell> action_ClickLeft;
    private Action<UI_GridCell> action_ClickRight;
    private string str_itemInfo = "";

    public ItemBase _bindItemBase = null;
    public ItemPath itemPath_Bind= new ItemPath();
    public void OnDisable()
    {
        if (bool_Pointing)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_HidenfoTextUI()
            {

            });
        }
    }

    #region//UI绑定
    /// <summary>
    /// 绑定
    /// </summary>
    /// <param name="itemPath"></param>
    /// <param name="putIn"></param>
    /// <param name="putOut"></param>
    /// <param name="clickLeft"></param>
    /// <param name="clickRight"></param>
    public void BindGrid(ItemPath itemPath, Action<ItemData, ItemPath> putIn, Func<ItemData, ItemData, ItemPath, ItemData> putOut, Action<UI_GridCell> clickLeft, Action<UI_GridCell> clickRight)
    {
        itemPath_Bind = itemPath;
        action_PutIn = putIn;
        action_PutOut = putOut;
        action_ClickLeft = clickLeft;
        action_ClickRight = clickRight;
    }
    public void BindGrid(ItemPath itemPath, Action<ItemData, ItemPath> putIn, Func<ItemData, ItemData, ItemPath, ItemData> putOut)
    {
        itemPath_Bind = itemPath;
        action_PutIn = putIn;
        action_PutOut = putOut;
    }
    #endregion
    #region//更新格子
    /// <summary>
    /// 更新数据
    /// </summary>
    public virtual ItemBase UpdateData(ItemData data)
    {
        if (_bindItemBase == null || _bindItemBase.itemData.I != data.I)
        {
            CreateItemBase(data, itemPath_Bind);
            UpdateItemBase(data);
        }
        else
        {
            UpdateItemBase(data);
        }
        return _bindItemBase;
    }
    /// <summary>
    /// 创建实例
    /// </summary>
    /// <param name="data"></param>
    private void CreateItemBase(ItemData data,ItemPath path)
    {
        ResetCell();
        Type type = Type.GetType("Item_" + data.I.ToString());
        if (type != null)
        {
            _bindItemBase = (ItemBase)Activator.CreateInstance(type);
            _bindItemBase.BindPath(path);
        }
        else _bindItemBase = null;
    }
    /// <summary>
    /// 更新实例
    /// </summary>
    /// <param name="data"></param>
    private void UpdateItemBase(ItemData data)
    {
        _bindItemBase.UpdateDataFromNet(data);
        _bindItemBase.GridCell_Draw(this);
    }
    /// <summary>
    /// 清空数据
    /// </summary>
    public void CleanItemBase()
    {
        if(_bindItemBase != null)
        {
            _bindItemBase = null;
            ResetCell();
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
    public void DrawCell(string mainIcon, string backGround, string info)
    {
        bool_Showing = true;
        image_IconBG.gameObject.SetActive(true);
        image_IconMain.sprite = spriteAtlas_ItemIcon.GetSprite(mainIcon);
        image_IconBG.sprite = spriteAtlas_ItemBG.GetSprite(backGround);
        text_CornerMark.text = info;
    }
    /// <summary>
    /// 设置格子
    /// </summary>
    /// <param name="name"></param>
    /// <param name="quality"></param>
    /// <param name="desc"></param>
    public void SetCell(string info)
    {
        str_itemInfo = info;
    }
    /// <summary>
    /// 重设格子
    /// </summary>
    private void ResetCell()
    {
        FreezeCell(false);
        CleanCell();
    }
    /// <summary>
    /// 清空格子
    /// </summary>
    public void CleanCell()
    {
        bool_Showing = false;
        text_CornerMark.text = "";
        str_itemInfo = "";
        image_IconMain.sprite = spriteAtlas_ItemIcon.GetSprite("Item_Default");
        image_IconBG.gameObject.SetActive(false);
        panel_Bar.gameObject.SetActive(false);
        image_Bar.transform.localScale = Vector3.one;
        if (bool_Pointing)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_HidenfoTextUI()
            {

            });
        }
    }
    /// <summary>
    /// 冰冻格子
    /// </summary>
    /// <param name="freeze"></param>
    public void FreezeCell(bool freeze)
    {
        image_IconMask.gameObject.SetActive(freeze);
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
        if (bool_Showing)
        {
            bool_Pointing = true;
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
        if (bool_Showing)
        {
            bool_Pointing = false;
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
        MouseHolding();
    }
    /// <summary>
    /// 鼠标右击
    /// </summary>
    public void ClickRight()
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
            action_PutIn.Invoke(data, itemPath_Bind);
        }
    }
    /// <summary>
    /// 取出
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public ItemData PutOut(ItemData itemData_From, ItemData itemData_Out)
    {
        if(action_PutOut != null)
        {
            return action_PutOut.Invoke(itemData_From, itemData_Out, itemPath_Bind);
        }
        else
        {
            return new ItemData();
        }
    }
    /// <summary>
    /// 鼠标持有
    /// </summary>
    public void MouseHolding()
    {
        ItemData itemData_Out = new ItemData();
        ItemData itemData_From = new ItemData();
        if (_bindItemBase != null)
        {
            itemData_From = _bindItemBase.itemData; 
            itemData_Out = itemData_From;
            if (Input.GetKey(KeyCode.LeftControl))
            {
                itemData_Out.C = (short)(Mathf.CeilToInt(itemData_From.C * 0.5f));
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                itemData_Out.C = 1;
            }
            else
            {
                itemData_Out.C = itemData_From.C;
            }
        }
        MessageBroker.Default.Publish(new UIEvent.UIEvent_StartHoldingItem()
        {
            itemCell = this,
            itemData_From = itemData_From,
            itemData_Out = itemData_Out
        });
    }
    #endregion
}
