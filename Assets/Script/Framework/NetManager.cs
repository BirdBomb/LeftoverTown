using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using DG.Tweening;
using Fusion.Addons.Physics;

public class NetManager : SingleTon<NetManager>,ISingleTon
{
    private GameObject netCore;
    private NetworkRunner networkRunner;
    public void Init()
    {

    }

    private void Start()
    {
        CreateNetCore();
        MessageBroker.Default.Receive<NetEvent.NetEvent_JoinGame>().Subscribe(_ =>
        {
            JoinRoom(_.RoomName);
        });
        MessageBroker.Default.Receive<NetEvent.NetEvent_CreateGame>().Subscribe(_ =>
        {
            CreateRoom(_.RoomName,_.RoomType);
        });
    }
    private void CreateNetCore()
    {
        GameObject obj = Resources.Load<GameObject>("Runner/NetRunner");
        netCore = Instantiate(obj, transform);
        networkRunner = netCore.GetComponent<NetworkRunner>();
        networkRunner.ProvideInput = true;
        networkRunner.JoinSessionLobby(SessionLobby.ClientServer);
    }
    public async void CreateRoom(string roomName,int roomType)
    {
        var scene = SceneRef.FromIndex(1);
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
                    break;
                }
            case 1:
                {
                    gameMode = GameMode.AutoHostOrClient;
                    isVisible = false;
                    break;
                }
            case 2:
                {
                    gameMode = GameMode.AutoHostOrClient;
                    isVisible = true;
                    break;
                }
        }

        await networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = gameMode,
            SessionName = roomName,
            Scene = scene,
            IsVisible = isVisible,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }
    public async void JoinRoom(string roomName)
    {
        var scene = SceneRef.FromIndex(1);
        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid)
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }
        await networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Client,
            SessionName = roomName,
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });

    }
}
