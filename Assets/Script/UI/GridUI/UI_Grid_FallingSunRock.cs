using System.Collections;
using System.Collections.Generic;
using System.Text;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.Rendering.CameraUI;

public class UI_Grid_FallingSunRock : UI_Grid
{
    [SerializeField, Header("距离")]
    private TextMeshProUGUI text_Distance;
    [SerializeField, Header("格子列表")]
    private List<UI_GridCell> gridCells_List = new List<UI_GridCell>();
    private List<ItemData> itemDatas_List = new List<ItemData>();
    public void Start()
    {
        BindAllCell();
    }
    private void BindAllCell()
    {
        for (int i = 0; i < gridCells_List.Count; i++)
        {
            gridCells_List[i].BindAction(PutIn, PutOut, null, null);
        }
    }

    #region//信息更新与上传
    /// <summary>
    /// 从地块获取更新
    /// </summary>
    /// <param name="info"></param>
    public void UpdateInfo(string info)
    {
        itemDatas_List.Clear();
        string[] strings = info.Split("/*I*/");
        for (int i = 0; i < strings.Length; i++)
        {
            if (strings[i] != "")
            {
                ItemData data = JsonUtility.FromJson<ItemData>(strings[i]);
                itemDatas_List.Add(data);
            }
        }
        DrawEveryCell();
        UpdateDistance();
    }
    /// <summary>
    /// 改变更新给地块
    /// </summary>
    public void ChangeInfo()
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < itemDatas_List.Count; i++)
        {
            if (i == 0)
            {
                builder.Append(JsonUtility.ToJson(itemDatas_List[i]));
            }
            else
            {
                builder.Append("/*I*/" + JsonUtility.ToJson(itemDatas_List[i]));
            }
        }
        if (action_ChangeInfo != null)
        {
            action_ChangeInfo.Invoke(builder.ToString());
        }
    }
    #endregion
    #region//UI绘制
    /// <summary>
    /// 绘制所有格子
    /// </summary>
    private void DrawEveryCell()
    {
        for (int i = 0; i < gridCells_List.Count; i++)
        {
            if (i < itemDatas_List.Count)
            {
                if (itemDatas_List[i].Item_ID != 0)
                {
                    gridCells_List[i].UpdateData(itemDatas_List[i]);
                }
                else
                {
                    gridCells_List[i].CleanData();
                }
            }
            else
            {
                gridCells_List[i].CleanData();
            }
        }
    }
    #endregion
    public void UpdateDistance()
    {
        int distance = 10 + itemDatas_List.Count*10;
        text_Distance.text = distance.ToString();
        MessageBroker.Default.Publish(new MapEvent.MapEvent_LocalTile_ChangeSunLight()
        {
            distance = distance
        });
    }
    public void PutIn(ItemData addData)
    {
        itemDatas_List = GameToolManager.Instance.PutInItemList(itemDatas_List, gridCells_List.Count, addData, out ItemData resData);
        if (resData.Item_ID > 0 && resData.Item_Count != 0)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
            {
                item = resData,
            });
        }
        ChangeInfo();
    }
    public ItemData PutOut(ItemData data)
    {
        itemDatas_List = GameToolManager.Instance.PutOutItemList(itemDatas_List, data);
        ChangeInfo();
        return data;
    }

}
