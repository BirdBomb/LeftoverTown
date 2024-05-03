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
using Debug = UnityEngine.Debug;
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
    private ItemData _itemData;
    private int _itemID;
    private int _itemCurCount;
    private int _itemMaxCount;
    private string _itemInfo; 
    private UI_Grid_Cabinet _parentGrid;

    private Action<ItemData> _clickLeft;
    private Action<ItemData> _clickRight;

    private void Start()
    {
        btn_Main.onClick.AddListener(() => { Debug.Log("yes"); });
    }
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
    /// <param name="data">物品信息</param>
    /// <param name="clickLeft">左击事件</param>
    /// <param name="clickRight">右击事件</param>
    public void InitGridCell(ItemData data, Action<ItemData> clickLeft, Action<ItemData> clickRight)
    {
        UnityEngine.Debug.Log("OK"+data.Item_ID);

        _itemData = data;
        _itemConfig = ItemConfigData.GetItemConfig(data.Item_ID);

        _itemID = _itemConfig.Item_ID;
        _itemCurCount = _itemConfig.Item_CurCount;
        _itemMaxCount = _itemConfig.Item_MaxCount;
        _itemInfo = _itemConfig.Item_Info;

        _clickLeft = clickLeft;
        _clickRight = clickRight;

        DrawCell();
        BindAction();
    }

    private void DrawCell()
    {
        image_Icon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemIcon").GetSprite("Item_" + _itemID.ToString());
        if (_itemCurCount > 0)
        {
            text_Count.text = _itemCurCount.ToString();
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
            _clickLeft.Invoke(_itemData);
        }
        UnityEngine.Debug.Log("Click_L");
    }
    public void ClickRight()
    {
        if (_clickRight != null)
        {
            _clickRight.Invoke(_itemData);
        }
        UnityEngine.Debug.Log("Click_R");
    }
}
public struct CellInfo
{
}