using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEditor.Progress;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class UI_GridCell : MonoBehaviour
{
    [Header("图标")]
    public Image image_Icon;
    [Header("数量")]
    public Text text_Count;
    [Header("按钮")]
    public Button btn_Main;

    private ItemConfig _itemConfig;
    private int _itemID;
    private int _itemCount;
    private string _itemInfo; 
    private UI_Grid _parentGrid;

    private Action _clickLeft;
    private Action _clickRight;

    /// <summary>
    /// 重置物品格子
    /// </summary>
    public void ResetGridCell()
    {
        image_Icon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemIcon").GetSprite("Item_Default");
        text_Count.text = "";
        _clickLeft = null;
        _clickRight = null;
    }
    /// <summary>
    /// 初始化物品格子
    /// </summary>
    /// <param name="itemConfig">物品信息</param>
    /// <param name="clickLeft">左击事件</param>
    /// <param name="clickRight">右击事件</param>
    public void InitGridCell(ItemConfig itemConfig,Action clickLeft,Action clickRight)
    {
        _itemConfig = itemConfig;

        _itemID = itemConfig.Item_ID;
        _itemCount = itemConfig.Item_Count;
        _itemInfo = itemConfig.Item_Info;

        _clickLeft = clickLeft;
        _clickRight = clickRight;

        DrawCell();
        BindAction();
    }
    private void DrawCell()
    {
        image_Icon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemIcon").GetSprite("Item_" + _itemID.ToString());
        if (_itemCount > 0)
        {
            text_Count.text = _itemCount.ToString();
        }
    }
    private void BindAction()
    {
        btn_Main.onClick.RemoveAllListeners();
        btn_Main.onClick.AddListener(() =>
        {
            ClickLeft();
        });

    }
    public void ClickLeft()
    {
        if(_clickLeft != null)
        {
            _clickLeft.Invoke();
        }
        UnityEngine.Debug.Log("Click_L");
    }
    public void ClickRight()
    {
        if (_clickRight != null)
        {
            _clickRight.Invoke();
        }
        UnityEngine.Debug.Log("Click_R");
    }
}
public struct CellInfo
{
}