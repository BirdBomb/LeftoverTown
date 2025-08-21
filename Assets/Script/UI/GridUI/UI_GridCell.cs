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
    [SerializeField, Header("ͼ��ͼ��")]
    private SpriteAtlas spriteAtlas_ItemIcon;
    [SerializeField, Header("����ͼ��")]
    private SpriteAtlas spriteAtlas_ItemBG;
    [SerializeField, Header("ͼ�걳��")]
    private Image image_IconBG;
    [SerializeField, Header("ͼ����Ҫ")]
    private Image image_IconMain;
    [SerializeField, Header("�䶳����")]
    private Image image_IconMask;
    [SerializeField, Header("�Ǳ�")]
    private Text text_CornerMark;
    [SerializeField, Header("��Grid")]
    public UI_Grid_Child grid_Child;
    [SerializeField, Header("����������")]
    private Transform panel_Bar;
    [SerializeField, Header("���������")]
    private Image image_Bar;
    private bool bool_Pointing = false;
    private bool bool_Showing = false;

    private Action<ItemData,ItemPath> action_PutIn;
    private Func<ItemData, ItemData, ItemPath, ItemData> action_PutOut;
    private Action<UI_GridCell> action_ClickLeft;
    private Action<UI_GridCell> action_ClickRight;
    private string str_itemName = "";
    private string str_itemQuality = "";
    private string str_itemDesc = "";
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

    #region//UI��
    /// <summary>
    /// ��
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
        if (grid_Child) { grid_Child.BindCell(itemPath); }
    }
    public void BindGrid(ItemPath itemPath, Action<ItemData, ItemPath> putIn, Func<ItemData, ItemData, ItemPath, ItemData> putOut)
    {
        itemPath_Bind = itemPath;
        action_PutIn = putIn;
        action_PutOut = putOut;
    }
    #endregion
    #region//���¸���
    /// <summary>
    /// ��������
    /// </summary>
    public virtual ItemBase UpdateData(ItemData data)
    {
        if (_bindItemBase == null || _bindItemBase.itemData.Item_ID != data.Item_ID)
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
    /// ����ʵ��
    /// </summary>
    /// <param name="data"></param>
    private void CreateItemBase(ItemData data,ItemPath path)
    {
        ResetCell();
        Type type = Type.GetType("Item_" + data.Item_ID.ToString());
        if (type != null)
        {
            _bindItemBase = (ItemBase)Activator.CreateInstance(type);
            _bindItemBase.BindPath(path);
        }
        else _bindItemBase = null;
    }
    /// <summary>
    /// ����ʵ��
    /// </summary>
    /// <param name="data"></param>
    private void UpdateItemBase(ItemData data)
    {
        _bindItemBase.UpdateDataFromNet(data);
        _bindItemBase.GridCell_Draw(this);
        if (grid_Child)
        {
            grid_Child.UpdateGrid(data);
            grid_Child.DrawCell(data);
        }
    }
    /// <summary>
    /// �������
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
    #region//���Ƹ���
    /// <summary>
    /// ���Ƹ���
    /// </summary>
    /// <param name="mainIcon"></param>
    /// <param name="childIcon"></param>
    /// <param name="backGround"></param>
    /// <param name="name"></param>
    /// <param name="info"></param>
    public void DrawCell(string mainIcon, string backGround, string name, string info)
    {
        bool_Showing = true;
        image_IconBG.gameObject.SetActive(true);
        image_IconMain.sprite = spriteAtlas_ItemIcon.GetSprite(mainIcon);
        image_IconBG.sprite = spriteAtlas_ItemBG.GetSprite(backGround);
        str_itemName = name;
        text_CornerMark.text = info;
    }
    /// <summary>
    /// ���ø���
    /// </summary>
    /// <param name="name"></param>
    /// <param name="quality"></param>
    /// <param name="desc"></param>
    public void SetCell(string info)
    {
        str_itemInfo = info;
    }
    /// <summary>
    /// �������
    /// </summary>
    private void ResetCell()
    {
        FreezeCell(false);
        CleanCell();
    }
    /// <summary>
    /// ��ո���
    /// </summary>
    public void CleanCell()
    {
        bool_Showing = false;
        text_CornerMark.text = "";
        str_itemName = "";
        str_itemDesc = "";
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
        if (grid_Child)
        {
            grid_Child.CloseGrid();
        }
    }
    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="freeze"></param>
    public void FreezeCell(bool freeze)
    {
        image_IconMask.gameObject.SetActive(freeze);
    }
    #endregion
    #region//������
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
    #region//UI����
    /// <summary>
    /// �����
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
    /// �������
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
    /// ����Ƴ�
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
    /// ������
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
    /// ����һ�
    /// </summary>
    public void ClickRight()
    {
        if (action_ClickRight != null)
        {
            action_ClickRight.Invoke(this);
        }
    }
    /// <summary>
    /// ����
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
    /// ȡ��
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
    /// ������
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
                itemData_Out.Item_Count = (short)(Mathf.CeilToInt(itemData_From.Item_Count * 0.5f));
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                itemData_Out.Item_Count = 1;
            }
            else
            {
                itemData_Out.Item_Count = itemData_From.Item_Count;
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
