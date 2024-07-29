using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Grid : MonoBehaviour
{
    /// <summary>
    /// 打开
    /// </summary>
    /// <param name="tileObj"></param>
    public virtual void Open(TileObj tileObj)
    {
        
    }
    /// <summary>
    /// 关闭
    /// </summary>
    /// <param name="tileObj"></param>
    public virtual void Close(TileObj tileObj)
    {
        
    }
    /// <summary>
    /// 格子开始拖拽
    /// </summary>
    /// <param name="gridCell"></param>
    /// <param name="itemData"></param>
    /// <param name="pointerEventData"></param>
    public virtual void CellDragBegin(UI_GridCell gridCell, ItemData itemData, PointerEventData pointerEventData)
    {

    }
    /// <summary>
    /// 格子拖拽中
    /// </summary>
    /// <param name="gridCell"></param>
    /// <param name="itemData"></param>
    /// <param name="pointerEventData"></param>
    public virtual void CellDragIn(UI_GridCell gridCell, ItemData itemData, PointerEventData pointerEventData)
    {

    }
    /// <summary>
    /// 格子拖拽结束
    /// </summary>
    /// <param name="gridCell"></param>
    /// <param name="itemData"></param>
    /// <param name="pointerEventData"></param>
    public virtual void CellDragEnd(UI_GridCell gridCell, ItemData itemData, PointerEventData pointerEventData)
    {

    }
    /// <summary>
    /// 监听拖拽
    /// </summary>
    public virtual void ListenDragOn<T>(T from, UI_GridCell cell, ItemData itemData) where T : UI_Grid
    {

    }
    /*取出*/
    public virtual void PutOut(ItemData data)
    {

    }
    /*放入*/
    public virtual void PutIn(ItemData itemData)
    {

    }
    /*计算*/
    public virtual void Calculate(ItemData before, out ItemData after)
    {
        after = before;
    }

}
