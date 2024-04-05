using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
public class UI_GameCreaterUI : MonoBehaviour
{
    [SerializeField, Header("��������")]
    private InputField Input_GameName;
    [SerializeField, Header("������Ϸ")]
    private Button Btn_Create;
    [SerializeField, Header("������Ϸ")]
    private Button Btn_Join;

    private void Start()
    {
        Bind();
    }
    private void Bind()
    {
        Btn_Create.onClick.AddListener(CreateGame);
        Btn_Join.onClick.AddListener(JoinGame);
    }
    private void CreateGame()
    {
        if (Input_GameName.text.Length > 0)
        {
            GameDataManager.Instance.mapFilePath = "MapData/TestSaveData";
            GameDataManager.Instance.actorFilePath = "PlayerData/Test";
            MessageBroker.Default.Publish(new NetEvent.NetEvent_JoinGame()
            {
                RoomName = Input_GameName.text
            });
        }
    }
    private void JoinGame()
    {
        if (Input_GameName.text.Length > 0)
        {
            GameDataManager.Instance.mapFilePath = "MapData/TestSaveData";
            GameDataManager.Instance.actorFilePath = "PlayerData/Test";
            MessageBroker.Default.Publish(new NetEvent.NetEvent_JoinGame()
            {
                RoomName = Input_GameName.text
            });
        }
    }
}
