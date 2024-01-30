using System.Collections;
using System.Collections.Generic;
using System.Text;
using UniRx;
using UnityEngine;
using static UnityEditor.Progress;

public class UI_Grid : MonoBehaviour
{
    [SerializeField,Header("格子列表")]
    private List<UI_GridCell> cellList = new List<UI_GridCell>();
    private List<ItemConfig> itemList = new List<ItemConfig>();

    private MyTile_Cabinet cabinet;
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="json"></param>
    public void InitGrid(string json ,MyTile_Cabinet tile)
    {
        Debug.Log("打开盒子:" + json);

        itemList.Clear();
        string[] strings = json.Split("/*I*/");
        for (int i = 0; i < strings.Length; i++)
        {
            if (strings[i] != "")
            {
                ItemConfig config = JsonUtility.FromJson<ItemConfig>(strings[i]);
                itemList.Add(config);
            }
        }
        cabinet = tile;
        DrawEveryCell();
        UpdateInfo();
    }
    private void DrawEveryCell()
    {
        for (int i = 0; i < cellList.Count; i++)
        {
            ResetCell(cellList[i]);
        }
        for (int i = 0; i < itemList.Count; i++)
        {
            if(cellList.Count > i)
            {
                DrawCell(cellList[i], itemList[i]);
            }
        }
    }
    private void ResetCell(UI_GridCell cell)
    {
        cell.ResetGridCell();
    }
    private void DrawCell(UI_GridCell cell, ItemConfig config)
    {
        cell.InitGridCell(config, () => { PutOut(config); }, () => {  });
    }

    /*取出*/
    public void PutOut(ItemConfig config)
    {
        itemList.Remove(config);
        UpdateInfo();
        DrawEveryCell();
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_AddItemInBag
        {
            itemConfig = config
        });
    }
    /*放入*/
    public void PutIn(ItemConfig config)
    {
        itemList.Add(config);
        UpdateInfo();
    }
    /*将改变更新给地块*/
    private void UpdateInfo()
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < itemList.Count; i++)
        {
            if (i == 0)
            {
                builder.Append(JsonUtility.ToJson(itemList[i]));
            }
            else
            {
                builder.Append("/*I*/" + JsonUtility.ToJson(itemList[i]));
            }

        }
        cabinet.UpdateTile(builder.ToString());
    }
}
