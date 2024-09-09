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
    #region//物体图标
    [Header("图标主要")]
    public Image image_MainIcon;
    [Header("图标次要")]
    public Image image_ChildIcon;
    [Header("图标遮罩(冰冻)")]
    public Image image_IceMask;
    [Header("信息文本")]
    public Text text_Info;
    #endregion
    #region//物体名字
    [Header("名字面板")]
    public Transform panel_Name;
    [Header("名字文本")]
    public TextMeshProUGUI text_Name;
    #endregion
    #region//功能UI
    [Header("UI按钮")]
    public Button btn_Main;
    [Header("UI子集")]
    public UI_Grid_Child grid_Child;
    #endregion
    #region//滑动条
    [Header("滑动条背景")]
    public Transform panel_Bar;
    [Header("滑动条填充")]
    public Image image_Bar;
    #endregion
    #region//状态
    private bool _sleep = false;
    #endregion
    private void Awake()
    {
        Bind();
    }

    #region//绑定
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
    public void ClickLeft()
    {
        if (_bindClickLeft != null && !_sleep)
        {
            _bindClickLeft.Invoke(this);
        }
    }
    public void ClickRight()
    {
        if (_bindClickRight != null && !_sleep)
        {

        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_bindDragBegin != null && !_sleep)
        {
            _bindDragBegin.Invoke(this, _bindItem.itemData, eventData);
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (_bindDraging != null && !_sleep)
        {
            _bindDraging.Invoke(this, _bindItem.itemData, eventData);
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        image_MainIcon.transform.localPosition = Vector3.zero;
        if (_bindDragEnd != null && !_sleep)
        {
            _bindDragEnd.Invoke(this, _bindItem.itemData, new PointerEventData(EventSystem.current));
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_sleep)
        {
            transform.DOKill();
            transform.localScale = Vector3.one;
            transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0), 0.2f).SetEase(Ease.InOutBack);
        }
        ShowNamePanel();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        HideNamePanel();
    }
    #endregion
    #region//更新清空格子
    /// <summary>
    /// 更新物品格子
    /// </summary>
    public virtual ItemBase UpdateGridCell(ItemData data)
    {
        ResetGridCell();
        BindID = data.Item_ID;
        if (data.Item_ID != 0)
        {
            _bindItem.UpdateData(data);
            _bindItem.DrawGridCell(this);
        }
        return _bindItem;
    }
    /// <summary>
    /// 清空物品格子
    /// </summary>
    public void ResetGridCell()
    {
        FreezeCell(false);
        SleepCell(false);
        image_MainIcon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_Default");
        image_ChildIcon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_Default");
        text_Info.text = "";
        text_Name.text = "";
        panel_Name.gameObject.SetActive(false);
        panel_Bar.gameObject.SetActive(false);
        image_Bar.transform.localScale = Vector3.one;
        _bindClickLeft = null;
        _bindClickRight = null;
        if (grid_Child)
        {
            grid_Child.Close();
        }
    }
    #endregion
    #region//显示隐藏名字
    private void ShowNamePanel()
    {
        if (text_Name.text != "")
        {
            panel_Name.DOKill();
            panel_Name.transform.localScale = Vector3.zero;
            panel_Name.DOScale(Vector3.one, 0.1f).SetEase(Ease.OutBack);
            panel_Name.gameObject.SetActive(true);
        }
    }
    private void HideNamePanel()
    {
        panel_Name.DOKill();
        panel_Name.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
        {
            panel_Name.gameObject.SetActive(false);
        });
    }
    #endregion
    #region//格子特殊状态
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
        _sleep = sleep;
    }
    #endregion
}
