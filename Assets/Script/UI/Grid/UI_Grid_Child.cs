using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// 子集格子，用于物品嵌套
/// </summary>
public class UI_Grid_Child : UI_Grid
{
    public UI_GridCell cell;
    /// <summary>
    /// 旧容器
    /// </summary>
    private ItemData oldContainerData;
    /// <summary>
    /// 新容器
    /// </summary>
    private ItemData newContainerData;
    private Func<ItemData, ItemData> calculate;
    public void Open(ItemData itemData, Func<ItemData,ItemData> cal)
    {
        gameObject.SetActive(true);
        oldContainerData = itemData;
        calculate = cal;
        DrawCell();
    }
    public void Close()
    {
        gameObject.SetActive(false);
        cell.ClearGridCell();
        calculate = null;
    }
    public override void Calculate(ItemData before, out ItemData after)
    {
        if (calculate != null)
        {
            after = calculate(before);
        }
        else
        {
            after = before;
        }
    }

    public void DrawCell()
    {
        ItemData item = new ItemData();
        item.Item_ID = oldContainerData.Item_Val;
        item.Item_Count = oldContainerData.Item_Count;
        if (item.Item_ID != 0)
        {
            cell.UpdateGridCell(item);
            cell.BindDragAction(CellDragBegin, CellDragIn, CellDragEnd);
        }
        else
        {
            cell.ClearGridCell();
        }
    }
    public override void CellDragBegin(UI_GridCell gridCell, ItemData itemData, PointerEventData pointerEventData)
    {
        base.CellDragBegin(gridCell, itemData, pointerEventData);
    }
    public override void CellDragIn(UI_GridCell gridCell, ItemData itemData, PointerEventData pointerEventData)
    {
        gridCell.image_Icon.transform.position = Input.mousePosition;
        base.CellDragIn(gridCell, itemData, pointerEventData);
    }
    public override void CellDragEnd(UI_GridCell gridCell, ItemData itemData, PointerEventData pointerEventData)
    {
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
        else
        {
            newContainerData = oldContainerData;
            newContainerData.Item_Val = 0;
            newContainerData.Item_Count = 1;
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryDropItem()
            {
                item = itemData
            });
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemInBag()
            {
                oldItem = oldContainerData,
                newItem = newContainerData,
                itemResidueBack = ((residueItem) =>
                {
                    if (residueItem.Item_Count != 0)//背包溢出
                    {
                        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryDropItem()
                        {
                            item = residueItem
                        });
                    }
                })
            });
        }
    }
    public override void ListenDragOn<T>(T grid, UI_GridCell cell, ItemData putIn)
    {
        if (grid.GetType() == typeof(UI_GameSenceUI))
        {
            Calculate(putIn, out ItemData back);
            if (back.Item_Count == 0)//可以全部放入
            {
                newContainerData = oldContainerData;
                newContainerData.Item_Val = putIn.Item_ID;
                if (oldContainerData.Item_Val == 0)
                {
                    newContainerData.Item_Count = oldContainerData.Item_Count - 1 + putIn.Item_Count;
                }
                else
                {
                    newContainerData.Item_Count = oldContainerData.Item_Count - 1 + putIn.Item_Count;
                }
                MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemInBag()
                {
                    oldItem = oldContainerData,
                    newItem = newContainerData,
                    itemResidueBack = ((residueItem) =>
                    {
                        if (residueItem.Item_Count != 0)//背包溢出
                        {
                            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryDropItem()
                            {
                                item = residueItem
                            });
                        }
                    })
                });
                MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryRemoveItemFromBag()
                {
                    item = putIn
                });
            }
            else if (back.Item_Count < putIn.Item_Count)//可以放入部分
            {
                newContainerData = oldContainerData;
                newContainerData.Item_Val = putIn.Item_ID;
                if (oldContainerData.Item_Val == 0)
                {
                    newContainerData.Item_Count = oldContainerData.Item_Count - 1 + putIn.Item_Count;
                }
                else
                {
                    newContainerData.Item_Count = oldContainerData.Item_Count - 1 + putIn.Item_Count;
                }

                MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryRemoveItemFromBag()
                {
                    item = putIn
                });
                MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
                {
                    item = back,
                    itemResidueBack = ((residueItem) =>
                    {
                        if (residueItem.Item_Count != 0)//背包溢出
                        {
                            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryDropItem()
                            {
                                item = residueItem
                            });
                        }
                    })

                });
                MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemInBag()
                {
                    oldItem = oldContainerData,
                    newItem = newContainerData,
                    itemResidueBack = ((residueItem) =>
                    {
                        if (residueItem.Item_Count != 0)//背包溢出
                        {
                            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryDropItem()
                            {
                                item = residueItem
                            });
                        }
                    })

                });
            }
            else if (back.Item_Count == putIn.Item_Count)//一个都没放入
            {

            }
        }
        base.ListenDragOn<T>(grid, cell, putIn);
    }
}
