using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using UnityEngine.U2D;
using UnityEngine.XR;

public class UI_GameSenceUI : MonoBehaviour
{
    [Header("�ֲ���λ")]
    public Image Image_HandSlot;
    [Header("����ֵ")]
    public Text Text_Hp;
    [Header("����ֵ")]
    public Text Text_Food;
    [Header("ȱˮֵ")]
    public Text Text_Water;
    [Header("��UI")]
    public UI_Grid_Cabinet Bind_Grid = null;
    private void Start()
    {
        MessageBroker.Default.Receive<UIEvent.UIEvent_UpdateItemInHand>().Subscribe(_ =>
        {
            UpdateHandSlot(_.itemData);
        }).AddTo(this);
        MessageBroker.Default.Receive<UIEvent.UIEvent_UpdateItemInBag>().Subscribe(_ =>
        {
            _bagItemDataList.Clear();
            for (int i = 0; i < _.itemDatas.Count; i++)
            {
                _bagItemDataList.Add(_.itemDatas[i]);
            }
            BagUpdateItem();

        }).AddTo(this);
        MessageBroker.Default.Receive<UIEvent.UIEvent_UpdateData>().Subscribe(_ =>
        {
            Text_Hp.text = _.HP.ToString();
            Text_Food.text = _.Food.ToString();
            Text_Water.text = _.Water.ToString();
        }).AddTo(this);
        MessageBroker.Default.Receive<UIEvent.UIEvent_OpenGridUI>().Subscribe(_ =>
        {
            Bind_Grid = _.bindUI;
        }).AddTo(this);
        MessageBroker.Default.Receive<UIEvent.UIEvent_CloseGridUI>().Subscribe(_ =>
        {
            if (Bind_Grid == _.bindUI)
            {
                Bind_Grid = null;
            }
        }).AddTo(this);
    }
    private void UpdateHandSlot(ItemData item)
    {
        Image_HandSlot.enabled = true;
        Image_HandSlot.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemIcon").GetSprite("Item_" + item.Item_ID.ToString());
    }
    #region//����
    [SerializeField,Header("������λ")]
    private List<UI_GridCell> _bagCellList = new List<UI_GridCell>();
    private List<ItemData> _bagItemDataList = new List<ItemData>();
    /// <summary>
    /// ���±�������
    /// </summary>
    private void BagUpdateItem()
    {
        BagDrawEveryCell();
    }
    private void BagDrawEveryCell()
    {
        for (int i = 0; i < _bagCellList.Count; i++)
        {
            ResetCell(_bagCellList[i]);
        }
        for (int i = 0; i < _bagItemDataList.Count; i++)
        {
            if (_bagCellList.Count > i)
            {
                DrawCell(_bagCellList[i], _bagItemDataList[i]);
            }
        }
    }
    private void ResetCell(UI_GridCell cell)
    {
        cell.ResetGridCell();
    }
    /// <summary>
    /// ����һ������
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="data"></param>
    private void DrawCell(UI_GridCell cell, ItemData data)
    {
        cell.InitGridCell(data, ClickCellLeft, ClickCellRight);
    }
    /// <summary>
    /// ����������
    /// </summary>
    /// <param name="itemData"></param>
    public void ClickCellLeft(ItemData itemData)
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryRemoveItemFromBag()
        {
            item = itemData
        });
        if (Bind_Grid != null)
        {
            /*������������ʱ������*/
            Bind_Grid.PutIn(itemData);
        }
        else
        {
            /*û������������ʱ������*/
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryDropItem()
            {
                item = itemData
            });
        }
        Debug.Log("Left");
    }
    public void ClickCellRight(ItemData itemData)
    {
        Debug.Log("Right");
    }
    #endregion
}
