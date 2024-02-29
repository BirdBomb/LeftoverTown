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
    private void Start()
    {
        MessageBroker.Default.Receive<UIEvent.UIEvent_AddItemInHand>().Subscribe(_ =>
        {
            UpdateHandSlot(_.itemConfig);
        }).AddTo(this);
        MessageBroker.Default.Receive<UIEvent.UIEvent_UpdateItemInBag>().Subscribe(_ =>
        {
            _bagItemList.Clear();
            for(int i = 0;i < _.itemConfigs.Count; i++)
            {
                _bagItemList.Add(_.itemConfigs[i]);
            }
            BagUpdateItem();
        }).AddTo(this);
    }
    private void UpdateHandSlot(ItemConfig item)
    {
        Image_HandSlot.sprite= Resources.Load<SpriteAtlas>("Atlas/ItemIcon").GetSprite("Item_"+ item.Item_ID.ToString());
    }
    #region//����
    [SerializeField,Header("������λ")]
    private List<UI_GridCell> _bagCellList = new List<UI_GridCell>();
    private List<ItemConfig> _bagItemList = new List<ItemConfig>();
    /// <summary>
    /// �����������
    /// </summary>
    /// <param name="itemConfig"></param>
    private void BagAddItem(ItemConfig itemConfig)
    {
        _bagItemList.Add(itemConfig);
        BagUpdateItem();
    }
    /// <summary>
    /// �����Ƴ�����
    /// </summary>
    private void BagSubItem(ItemConfig itemConfig)
    {
        _bagItemList.Remove(itemConfig);
        BagUpdateItem();
    }
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
        for (int i = 0; i < _bagItemList.Count; i++)
        {
            if (_bagCellList.Count > i)
            {
                DrawCell(_bagCellList[i], _bagItemList[i]);
            }
        }
    }
    private void ResetCell(UI_GridCell cell)
    {
        cell.ResetGridCell();
    }
    private void DrawCell(UI_GridCell cell, ItemConfig config)
    {
        cell.InitGridCell(config, () => { }, () => { });
    }

    #endregion
}
