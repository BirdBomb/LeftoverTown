using System.Collections;
using System.Collections.Generic;
using System.Text;
using UniRx;
using UnityEngine;
using static Fusion.Allocator;
using static UnityEditor.Progress;

public class UI_Grid_Cabinet : MonoBehaviour
{
    [SerializeField,Header("�����б�")]
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
    /// �ӵؿ��ȡ����
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
    /// �ı���¸��ؿ�
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
    /// �������и���
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
    /// ����һ������
    /// </summary>
    /// <param name="cell"></param>
    private void ResetCell(UI_GridCell cell)
    {
        cell.ResetGridCell();
    }
    /// <summary>
    /// ����һ������
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

    /*ȡ��*/
    public void PutOut(ItemData data)
    {
        itemDataList.Remove(data);
        ChangeInfoToTile();
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
        {
            item = data
        });
    }
    /*����*/
    public void PutIn(ItemData data)
    {
        itemDataList.Add(data);
        ChangeInfoToTile();
    }
}
