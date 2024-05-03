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
        // ��������ֵ�ı�ļ�����
        inputField.onValueChanged.AddListener(OnInputFieldValueChanged);
        // �����������ļ�����
        inputField.onEndEdit.AddListener(OnInputFieldEndEdit);
    }

    // �������ֵ�ı�ʱ���õķ���
    private void OnInputFieldValueChanged(string value)
    {
        Debug.Log("������ֵ�����˸ı䣺" + value);
    }

    // �����������༭ʱ���õķ���
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
        Debug.Log("���������༭��" + value);
    }
}
