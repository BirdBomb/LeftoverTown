using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Grid : MonoBehaviour
{
    /// <summary>
    /// ��
    /// </summary>
    /// <param name="tileObj"></param>
    public virtual void Open(TileObj tileObj)
    {
        
    }
    /// <summary>
    /// �ر�
    /// </summary>
    /// <param name="tileObj"></param>
    public virtual void Close(TileObj tileObj)
    {
        
    }
    /// <summary>
    /// ���ӿ�ʼ��ק
    /// </summary>
    /// <param name="gridCell"></param>
    /// <param name="itemData"></param>
    /// <param name="pointerEventData"></param>
    public virtual void CellDragBegin(UI_GridCell gridCell, ItemData itemData, PointerEventData pointerEventData)
    {

    }
    /// <summary>
    /// ������ק��
    /// </summary>
    /// <param name="gridCell"></param>
    /// <param name="itemData"></param>
    /// <param name="pointerEventData"></param>
    public virtual void CellDragIn(UI_GridCell gridCell, ItemData itemData, PointerEventData pointerEventData)
    {

    }
    /// <summary>
    /// ������ק����
    /// </summary>
    /// <param name="gridCell"></param>
    /// <param name="itemData"></param>
    /// <param name="pointerEventData"></param>
    public virtual void CellDragEnd(UI_GridCell gridCell, ItemData itemData, PointerEventData pointerEventData)
    {

    }
    /// <summary>
    /// ������ק
    /// </summary>
    public virtual void ListenDragOn<T>(T from, UI_GridCell cell, ItemData itemData) where T : UI_Grid
    {

    }
    /*ȡ��*/
    public virtual void PutOut(ItemData data)
    {

    }
    /*����*/
    public virtual void PutIn(ItemData itemData)
    {

    }
    /*����*/
    public virtual void Calculate(ItemData before, out ItemData after)
    {
        after = before;
    }

}
