using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Grid_Buy : UI_Grid
{
    [SerializeField, Header("格子列表")]
    private List<UI_GridCell> gridCells_List = new List<UI_GridCell>();
    [SerializeField, Header("价格列表")]
    private List<TextMeshProUGUI> texts_Price = new List<TextMeshProUGUI>();
    private List<ItemData> itemDatas_List = new List<ItemData>();
    public void Start()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_AllClient_UpdateTime>().Subscribe(_ =>
        {
            DrawEveryCell();
        }).AddTo(this);
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
        action_ChangeInfo.Invoke(builder.ToString());
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
                gridCells_List[i].UpdateData(itemDatas_List[i]);
                if (itemDatas_List[i].Item_ID == 0)
                {
                    texts_Price[i].transform.parent.gameObject.SetActive(false);
                    texts_Price[i].text = "";
                }
                else
                {
                    int val = (int)ItemConfigData.GetItemConfig(itemDatas_List[i].Item_ID).Average_Value * itemDatas_List[i].Item_Count;
                    texts_Price[i].transform.parent.gameObject.SetActive(true);
                    texts_Price[i].text = val.ToString();
                    if (GameLocalManager.Instance.playerCoreLocal.actorManager_Bind.actorNetManager.Net_Coin >= val)
                    {
                        gridCells_List[i].SleepCell(false);
                    }
                    else
                    {
                        gridCells_List[i].SleepCell(true);
                    }
                }
            }
            else
            {
                gridCells_List[i].CleanData();
                texts_Price[i].transform.parent.gameObject.SetActive(false);
                texts_Price[i].text = "";
            }
        }
    }
    #endregion
    public void PutIn(ItemData data)
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
        {
            item = data,
        });
        ChangeInfo();
    }
    public ItemData PutOut(ItemData data)
    {
        int price = (int)(ItemConfigData.GetItemConfig(data.Item_ID).Average_Value * data.Item_Count);
        if (GameLocalManager.Instance.playerCoreLocal.actorManager_Bind.actionManager.PayCoin(price))
        {
            itemDatas_List = GameToolManager.Instance.PutOutItemList(itemDatas_List, data);
        }
        else
        {
            data = new ItemData();
        }
        ChangeInfo();
        return data;
    }
}
