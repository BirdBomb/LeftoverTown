using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
public class UI_GameCreaterUI : MonoBehaviour
{
    [SerializeField, Header("房间名称")]
    private InputField Input_GameName;
    [SerializeField, Header("创建游戏")]
    private Button Btn_Create;
    [SerializeField, Header("加入游戏")]
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
            GameDataManager.Instance.mapBuildingTypeFilePath = "MapData/SaveData_BuildingTileType_Test";
            GameDataManager.Instance.mapBuildingInfoFilePath = "MapData/SaveData_BuildingTileInfo_Test";
            GameDataManager.Instance.mapFloorTypeFilePath = "MapData/SaveData_FloorTileType_Test";
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
            GameDataManager.Instance.mapBuildingTypeFilePath = "MapData/SaveData_MapTileType_Test";
            GameDataManager.Instance.mapBuildingInfoFilePath = "MapData/SaveData_MapTileInfo_Test";
            GameDataManager.Instance.actorFilePath = "PlayerData/Test";
            MessageBroker.Default.Publish(new NetEvent.NetEvent_JoinGame()
            {
                RoomName = Input_GameName.text
            });
        }
    }
}
