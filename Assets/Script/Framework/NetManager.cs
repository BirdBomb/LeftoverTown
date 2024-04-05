using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;

public class NetManager : SingleTon<NetManager>,ISingleTon
{
    private NetworkRunner networkRunner;
    public void Init()
    {

    }

    private void Start()
    {
        MessageBroker.Default.Receive<NetEvent.NetEvent_JoinGame>().Subscribe(_ =>
        {
            JoinRoomTest(_.RoomName);
        });
    }
    public async void JoinRoomTest(string roomName)
    {
        networkRunner = gameObject.AddComponent<NetworkRunner>();
        networkRunner.ProvideInput = true;

        //var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        var scene = SceneRef.FromIndex(1);
        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid)
        {
            sceneInfo.AddSceneRef(scene,LoadSceneMode.Additive);
        }
        await networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.AutoHostOrClient,
            SessionName = roomName,
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }

}
