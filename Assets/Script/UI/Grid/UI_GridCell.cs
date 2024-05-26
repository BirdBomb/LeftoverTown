using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static Fusion.Allocator;
using Button = UnityEngine.UI.Button;
using Debug = UnityEngine.Debug;
using Image = UnityEngine.UI.Image;

public class UI_GridCell : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("图标")]
    public Image image_Icon;
    [Header("文本")]
    public Text text_Info;
    [Header("按钮")]
    public Button btn_Main;
    [Header("子集")]
    public UI_Grid_Child grid;

    public ItemBase _bindItem;
    private Action<UI_GridCell> _bindClickLeft;
    private Action<UI_GridCell> _bindClickRight;
    private Action<UI_GridCell, ItemData, PointerEventData> _bindDragBegin;
    private Action<UI_GridCell, ItemData, PointerEventData> _bindDraging;
    private Action<UI_GridCell, ItemData, PointerEventData> _bindDragEnd;
    private void Start()
    {
        btn_Main.onClick.AddListener(ClickLeft);
    }

    /// <summary>
    /// 更新物品格子
    /// </summary>
    public virtual ItemBase UpdateGridCell(ItemData data)
    {
        if (data.Item_ID == 0)
        {
           image_Icon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_Default");
            return null;
        }

        if (_bindItem == null)
        {
            Type type = Type.GetType("Item_" + data.Item_ID.ToString());
            _bindItem = (ItemBase)Activator.CreateInstance(type);
        }
        else
        {
            if (_bindItem.data.Item_ID != data.Item_ID)
            {
                Type type = Type.GetType("Item_" + data.Item_ID.ToString());
                _bindItem = (ItemBase)Activator.CreateInstance(type);
            }
        }
        if (grid)
        {
            grid.Close();
        }
        _bindItem.UpdateData(data);
        _bindItem.DrawGridCell(this);
        return _bindItem;
    }
    /// <summary>
    /// 清空物品格子
    /// </summary>
    public void ClearGridCell()
    {
        image_Icon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_Default");
        text_Info.text = "";
        _bindClickLeft = null;
        _bindClickRight = null;
        if (grid)
        {
            grid.Close();
        }
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
    public void ClickLeft()
    {
        if(_bindClickLeft != null)
        {
            _bindClickLeft.Invoke(this);
        }
    }
    public void ClickRight()
    {
        if (_bindClickRight != null)
        {
            
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_bindDragBegin != null)
        {
            _bindDragBegin.Invoke(this, _bindItem.data, eventData);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_bindDraging != null)
        {
            _bindDraging.Invoke(this, _bindItem.data, eventData);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image_Icon.transform.localPosition = Vector3.zero;
        if (_bindDragEnd != null)
        {
            _bindDragEnd.Invoke(this, _bindItem.data, new PointerEventData(EventSystem.current));
        }
    }
}
public class GridCellData
{
    private ItemConfig _itemConfig;
    private ItemData _itemData;
}