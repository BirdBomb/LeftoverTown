using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Grid_OnBody : UI_Grid
{
    [SerializeField, Header("�������")]
    private UI_GridCell cell;
    [SerializeField, Header("��������")]
    private Text text;
    private ItemData itemData;

    private ItemConfig onHead;
    private ItemConfig onBody;
    private ItemConfig onHand;

    private void Start()
    {
        HowDoILook();
        MessageBroker.Default.Receive<UIEvent.UIEvent_UpdateItemOnBody>().Subscribe(_ =>
        {
            onBody = ItemConfigData.GetItemConfig(_.itemData.Item_ID);
            HowDoILook();
            itemData = _.itemData;
            if (itemData.Item_ID == 0)
            {
                ResetCell();
            }
            else
            {
                DrawCell();
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<UIEvent.UIEvent_UpdateItemInHand>().Subscribe(_ =>
        {
            onHand = ItemConfigData.GetItemConfig(_.itemData.Item_ID);
            HowDoILook();
        }).AddTo(this);
        MessageBroker.Default.Receive<UIEvent.UIEvent_UpdateItemOnHead>().Subscribe(_ =>
        {
            onHead = ItemConfigData.GetItemConfig(_.itemData.Item_ID);
            HowDoILook();
        }).AddTo(this);
    }
    private void HowDoILook()
    {
        if (onBody.Item_ID == 0 || onHead.Item_ID == 0)
        {
            if (onHand.Item_ID != 0)
            {
                text.text = "�²������Ұ��,�ֳ�" + onHand.Item_Name;
            }
            else
            {
                text.text = "�²������Ұ��";
            }
            return;
        }
        else
        {
            if (onBody.Item_ID == onHead.Item_ID)
            {
                text.text = "��˵�е�" + onBody.Item_Name + "��";
                return;
            }
            if (onBody.Item_Type != ItemType.Clothes)
            {
                if (onHand.Item_ID != 0)
                {
                    text.text = "��" + onBody.Item_Name + "��������,�ֳ�" + onHand.Item_Name + ",����ȥ��̫����";
                }
                else
                {
                    text.text = "��" + onBody.Item_Name + "��������,����ȥ��̫����";
                }

                return;
            }
            if (onHead.Item_Type != ItemType.Hat)
            {
                if (onHand.Item_ID != 0)
                {
                    text.text = "��" + onHead.Item_Name + "����ͷ��,�ֳ�" + onHand.Item_Name + ",����ȥ��̫����";
                }
                else
                {
                    text.text = "��" + onHead.Item_Name + "����ͷ����,����ȥ��̫����";
                }

                return;
            }
            if (onHead.HowDoILook.CompareTo(onBody.HowDoILook) == 0)
            {
                if (onHand.Item_ID != 0)
                {
                    text.text = onBody.HowDoILook + ",�ֳ�" + onHand.Item_Name;
                }
                else
                {
                    text.text = onBody.HowDoILook;
                }

            }
            else
            {
                if (onHand.Item_ID != 0)
                {
                    text.text = onBody.HowDoILook + ",�ֳ�" + onHand.Item_Name + "(��" + onHead.Item_Name + "����ͷ��)";
                }
                else
                {
                    text.text = onBody.HowDoILook + "(��" + onHead.Item_Name + "����ͷ��)";
                }
            }
        }
    }
    private void DrawCell()
    {
        cell.UpdateGridCell(itemData);
        cell.BindClickAction(ClickCellLeft, ClickCellRight);
        cell.BindDragAction(CellDragBegin, CellDragIn, CellDragEnd);
    }
    private void ResetCell()
    {
        cell.ClearGridCell();
    }
    public void ClickCellLeft(UI_GridCell gridCell)
    {
       
    }
    public void ClickCellRight(UI_GridCell gridCell)
    {
        
    }
    public override void CellDragBegin(UI_GridCell gridCell, ItemData itemData, PointerEventData pointerEventData)
    {

    }
    public override void CellDragIn(UI_GridCell gridCell, ItemData itemData, PointerEventData pointerEventData)
    {
        gridCell.image_Icon.transform.position = Input.mousePosition;
    }
    public override void CellDragEnd(UI_GridCell gridCell, ItemData itemData, PointerEventData pointerEventData)
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryRemoveItemOnBody()
        {
            item = itemData
        });
        pointerEventData.position = Input.mousePosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);
        if (raycastResults.Count > 0)
        {
            foreach (RaycastResult result in raycastResults)
            {
                if (result.gameObject.TryGetComponent(out UI_Grid grid))
                {
                    grid.ListenDragOn(this, gridCell, itemData);
                    return;
                }
            }
        }
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryDropItem()
        {
            item = itemData
        });
    }
    public override void ListenDragOn<T>(T grid, UI_GridCell cell, ItemData itemData)
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemOnBody()
        {
            item = itemData
        });
    }
}
