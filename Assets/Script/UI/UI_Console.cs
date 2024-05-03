using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UniRx;
using static UnityEngine.Rendering.DebugUI;

public class UI_Console : MonoBehaviour
{
    [SerializeField]
    private InputField inputField;
    private void Start()
    {
        // 添加输入框值改变的监听器
        inputField.onValueChanged.AddListener(OnInputFieldValueChanged);
        // 添加输入框点击的监听器
        inputField.onEndEdit.AddListener(OnInputFieldEndEdit);
    }

    // 当输入框值改变时调用的方法
    private void OnInputFieldValueChanged(string value)
    {
        Debug.Log("输入框的值发生了改变：" + value);
    }

    // 当输入框结束编辑时调用的方法
    private void OnInputFieldEndEdit(string value)
    {
        string[] strs = value.Split('.');
        if (strs.Length >= 2)
        {
            if (strs[0] == "giveitem")
            {
                ItemData itemData = new ItemData();
                itemData.Item_ID = int.Parse(strs[1]);
                itemData.Item_Seed = System.DateTime.Now.Second;
                MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryDropItem()
                {
                    item = itemData
                }); ;
            }
            else if (strs[0] == "spawn")
            {
                MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TrySpawnActor()
                {
                    name = "Actor/" + strs[1]
                }); ;
            }
        }
        Debug.Log("输入框结束编辑：" + value);
    }
}
