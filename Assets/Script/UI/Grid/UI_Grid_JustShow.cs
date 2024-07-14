using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class UI_Grid_JustShow : UI_Grid
{
    [SerializeField, Header("背包槽位")]
    private List<UI_GridCell> _bagCellList = new List<UI_GridCell>();
    [SerializeField, Header("手部槽位")]
    private UI_GridCell _handCell;
    [SerializeField, Header("身体槽位")]
    private UI_GridCell _bodyCell;
    [SerializeField, Header("头部槽位")]
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
