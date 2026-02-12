using Fusion;
using Fusion.Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetManager : SingleTon<NetManager>,ISingleTon
{
    private NetworkRunner networkRunner = null;
    private GameObject networkObj;
    public void Init()
    {

    }
    private void OnCloudConnectionLost(NetworkRunner runner, ShutdownReason reason, bool reconnecting)
    {
        Debug.Log($"云端连接失败: {reason} (原因: {reconnecting})");

        if (!reconnecting)
        {
            Debug.Log("不再重新连接云端");
            // Handle scenarios where reconnection is not possible
            // e.g., notify the user, attempt manual reconnection, etc.
        }
        else
        {
            // Wait for automatic reconnection
            Debug.Log("尝试重新连接云端");
            StartCoroutine(WaitForReconnection(runner));
        }
    }

    private IEnumerator WaitForReconnection(NetworkRunner runner)
    {
        yield return new WaitUntil(() => runner.IsInSession);
        Debug.Log("Reconnected to the Cloud!");
    }

    public override void Start()
    {
        NetworkRunner.CloudConnectionLost += OnCloudConnectionLost;
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
        networkObj = Instantiate(obj, transform);
        networkRunner = networkObj.GetComponent<NetworkRunner>();
        networkRunner.ProvideInput = true;
    }
    private void CheckNetCore()
    {
        if (networkRunner == null)
        {
            CreateNetCore();
        }
    }
    private async Task DestroyNetCore()
    {
        if (networkRunner != null)
        {
            await networkRunner.Shutdown();
            Destroy(networkRunner);
        }
    }
    public async void CreateRoom(string roomName,int roomType)
    {
        CheckNetCore();
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
        var startGameResult = await networkRunner.StartGame(new StartGameArgs()
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
        CheckNetCore();
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
        await DestroyNetCore();
        SceneManager.LoadScene("MenuScene");
    }
    public async void JoinSessionLobby()
    {
        CheckNetCore();
        StartGameResult startGameResult = await networkRunner.JoinSessionLobby(SessionLobby.ClientServer);
        if (startGameResult.Ok)
        {
            Debug.Log("加入公共会话大厅成功");
        }
        else
        {
            Debug.Log($"加入公共会话大厅失败:原因{startGameResult.ShutdownReason}");
            await DestroyNetCore();
        }
    }
    public void UpdateFusionSetting(FusionAppSettings settings)
    {
        PhotonAppSettings.Global.AppSettings = settings;
    }
}
