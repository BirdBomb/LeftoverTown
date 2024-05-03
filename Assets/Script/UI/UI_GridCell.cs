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
    [Header("ͼ��")]
    public Image image_Icon;
    [Header("����")]
    public Text text_Count;
    [Header("��ť")]
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
    /// <param name="data">��Ʒ��Ϣ</param>
    /// <param name="clickLeft">����¼�</param>
    /// <param name="clickRight">�һ��¼�</param>
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