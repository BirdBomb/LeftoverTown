using System.Collections;
using System.Collections.Generic;
using System.Text;
using UniRx;
using UnityEngine;
using static Fusion.Allocator;
using static UnityEditor.Progress;

public class UI_Grid_Cabinet : MonoBehaviour
{
    [SerializeField,Header("格子列表")]
    private List<UI_GridCell> cellList = new List<UI_GridCell>();
    private List<ItemData> itemDataList = new List<ItemData>();

    private TileObj cabinet;

    public void Open(TileObj tileObj)
    {
        MessageBroker.Default.Publish(new UIEvent.UIEvent_OpenGridUI()
        {
            bindUI = this
        });
        cabinet = tileObj;
    }
    public void Close(TileObj tileObj)
    {
        MessageBroker.Default.Publish(new UIEvent.UIEvent_CloseGridUI()
        {
            bindUI = this
        });
    }


    /// <summary>
    /// 从地块获取更新
    /// </summary>
    /// <param name="info"></param>
    public void UpdateInfoFromTile(string info)
    {
        itemDataList.Clear();
        string[] strings = info.Split("/*I*/");
        for (int i = 0; i < strings.Length; i++)
        {
            if (strings[i] != "")
            {
                ItemData data = JsonUtility.FromJson<ItemData>(strings[i]);
                itemDataList.Add(data);
            }
        }
        DrawEveryCell();
    }
    /// <summary>
    /// 改变更新给地块
    /// </summary>
    public void ChangeInfoToTile()
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < itemDataList.Count; i++)
        {
            if (i == 0)
            {
                builder.Append(JsonUtility.ToJson(itemDataList[i]));
            }
            else
            {
                builder.Append("/*I*/" + JsonUtility.ToJson(itemDataList[i]));
            }
        }
        cabinet.TryToChangeInfo(builder.ToString());
    }
    /// <summary>
    /// 绘制所有格子
    /// </summary>
    private void DrawEveryCell()
    {
        for (int i = 0; i < cellList.Count; i++)
        {
            ResetCell(cellList[i]);
        }
        for (int i = 0; i < itemDataList.Count; i++)
        {
            DrawCell(itemDataList[i], cellList[i]);
        }
    }
    /// <summary>
    /// 重置一个格子
    /// </summary>
    /// <param name="cell"></param>
    private void ResetCell(UI_GridCell cell)
    {
        cell.ResetGridCell();
    }
    /// <summary>
    /// 绘制一个格子
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="config"></param>
    private void DrawCell(ItemData data,UI_GridCell cell)
    {
        cell.InitGridCell(data, ClickCellLeft, ClickCellRight);
    }
    public void ClickCellLeft(ItemData itemData)
    {
        PutOut(itemData);
    }
    public void ClickCellRight(ItemData itemData)
    {
        Debug.Log("Right");
    }

    /*取出*/
    public void PutOut(ItemData data)
    {
        itemDataList.Remove(data);
        ChangeInfoToTile();
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
        {
            item = data
        });
    }
    /*放入*/
    public void PutIn(ItemData data)
    {
        itemDataList.Add(data);
        ChangeInfoToTile();
    }
}
