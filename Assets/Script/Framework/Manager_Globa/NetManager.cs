using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using DG.Tweening;
using Fusion.Addons.Physics;
using Fusion.Photon.Realtime;

public class NetManager : SingleTon<NetManager>,ISingleTon
{
    private NetworkRunner networkRunner;
    public void Init()
    {

    }

    public override void Start()
    {
        CreateNetCore();
        MessageBroker.Default.Receive<NetEvent.NetEvent_JoinGame>().Subscribe(_ =>
        {
            JoinRoom(_.RoomInfo, _.RoomName);
        });
        MessageBroker.Default.Receive<NetEvent.NetEvent_CreateGame>().Subscribe(_ =>
        {
            CreateRoom(_.RoomName, _.RoomType);
        });
        MessageBroker.Default.Receive<NetEvent.NetEvent_QuitGame>().Subscribe(_ =>
        {
            QuitRoom();
        });
        base.Start();
    }
    private void CreateNetCore()
    {
        GameObject obj = Resources.Load<GameObject>("Runner/NetRunner");
        GameObject netCore = Instantiate(obj, transform);
        networkRunner = netCore.GetComponent<NetworkRunner>();
        networkRunner.ProvideInput = true;
        networkRunner.JoinSessionLobby(SessionLobby.ClientServer);
    }
    public async void CreateRoom(string roomName,int roomType)
    {
        Dictionary<string, SessionProperty> gameProperty = new Dictionary<string, SessionProperty>() { };
        gameProperty.Add("GameMode", 0);
        var scene = SceneRef.FromIndex(2);
        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid)
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }
        GameMode gameMode = GameMode.Single;
        bool isVisible = true;
        switch (roomType)
        {
            case 0:
                {
                    gameMode = GameMode.Single;
                    gameProperty["GameMode"] = (int)gameMode;
                    break;
                }
            case 1:
                {
                    gameMode = GameMode.AutoHostOrClient;
                    isVisible = true;
                    gameProperty["GameMode"] = (int)gameMode;
                    break;
                }
        }
        await networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = gameMode,
            SessionName = roomName,
            Scene = scene,
            IsVisible = isVisible,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
            SessionProperties = gameProperty
        });
    }
    public async void JoinRoom(SessionInfo sessionInfo, string roomName)
    {
        GameMode gameMode = (GameMode)((int)sessionInfo.Properties["GameMode"]);
        Debug.Log("加入房间" + roomName + "/类型/" + gameMode);
        var scene = SceneRef.FromIndex(1);
        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid)
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }
        await networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = gameMode,
            SessionName = roomName,
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });

    }
    public async void QuitRoom()
    {
        await networkRunner.Shutdown();
        Destroy(networkRunner);
        CreateNetCore();
        SceneManager.LoadScene("MenuScene");
    }
    public void UpdateFusionSetting(FusionAppSettings settings)
    {
        PhotonAppSettings.Global.AppSettings = settings;
    }
}
