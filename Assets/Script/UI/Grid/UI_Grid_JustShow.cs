using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class UI_Grid_JustShow : UI_Grid
{
    [SerializeField, Header("������λ")]
    private List<UI_GridCell> _bagCellList = new List<UI_GridCell>();
    [SerializeField, Header("�ֲ���λ")]
    private UI_GridCell _handCell;
    [SerializeField, Header("�����λ")]
    private UI_GridCell _bodyCell;
    [SerializeField, Header("ͷ����λ")]
    private UI_GridCell _headCell;

    private void Start()
    {
        
    }
    public void Open()
    {
        UpdateCell();
    }
    public void UpdateCell()
    {
        NetworkLinkedList<ItemData> bagItem = GameLocalManager.Instance.localPlayer.actorManager.NetManager.Data_ItemInBag;
        ItemData handItem = GameLocalManager.Instance.localPlayer.actorManager.NetManager.Data_ItemInHand;
        ItemData headItem = GameLocalManager.Instance.localPlayer.actorManager.NetManager.Data_ItemOnHead;
        ItemData bodyItem = GameLocalManager.Instance.localPlayer.actorManager.NetManager.Data_ItemOnBody;
        for (int i = 0; i < bagItem.Count; i++)
        {
            int index = i;
            if (_bagCellList.Count > index)
            {
                _bagCellList[index].UpdateGridCell(bagItem[index]);
            }
        }
        _handCell.UpdateGridCell(handItem);
        _headCell.UpdateGridCell(headItem);
        _bodyCell.UpdateGridCell(bodyItem);
    }
}
