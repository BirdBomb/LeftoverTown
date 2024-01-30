using System.Collections;
using System.Collections.Generic;
using System.Text;
using UniRx;
using UnityEngine;
using static UnityEditor.Progress;

public class UI_Grid : MonoBehaviour
{
    [SerializeField,Header("�����б�")]
    private List<UI_GridCell> cellList = new List<UI_GridCell>();
    private List<ItemConfig> itemList = new List<ItemConfig>();

    private MyTile_Cabinet cabinet;
    /// <summary>
    /// ��ʼ��
    /// </summary>
    /// <param name="json"></param>
    public void InitGrid(string json ,MyTile_Cabinet tile)
    {
        Debug.Log("�򿪺���:" + json);

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

    /*ȡ��*/
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
    /*����*/
    public void PutIn(ItemConfig config)
    {
        itemList.Add(config);
        UpdateInfo();
    }
    /*���ı���¸��ؿ�*/
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
