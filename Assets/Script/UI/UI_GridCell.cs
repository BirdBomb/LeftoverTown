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
    [Header("ͼ��")]
    public Image image_Icon;
    [Header("����")]
    public Text text_Count;
    [Header("��ť")]
    public Button btn_Main;

    private ItemConfig _itemConfig;
    private int _itemID;
    private int _itemCount;
    private string _itemInfo; 
    private UI_Grid _parentGrid;

    private Action _clickLeft;
    private Action _clickRight;

    /// <summary>
    /// ������Ʒ����
    /// </summary>
    public void ResetGridCell()
    {
        image_Icon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemIcon").GetSprite("Item_Default");
        text_Count.text = "";
        _clickLeft = null;
        _clickRight = null;
    }
    /// <summary>
    /// ��ʼ����Ʒ����
    /// </summary>
    /// <param name="itemConfig">��Ʒ��Ϣ</param>
    /// <param name="clickLeft">����¼�</param>
    /// <param name="clickRight">�һ��¼�</param>
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