using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using UniRx;
using UnityEngine.EventSystems;

public class BasicSpawner : MonoBehaviour,INetworkRunnerCallbacks
{
    [SerializeField, Header("���Ԥ����")]
    private NetworkPrefabRef _playerPrefab;
    [SerializeField, Header("���ƫ��")]
    private Vector3 local_MouseOffset;
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();
    private float local_leftPressTimer;
    private float local_rightPressTimer;
    private void Start()
    {
        //NetworkRunner.CloudConnectionLost += OnCloudConnectionLost;
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject())
        {
            local_leftPressTimer += Time.deltaTime;
        }
        else
        {
            local_leftPressTimer = 0;
        }
        if (Input.GetKey(KeyCode.Mouse1) && !EventSystem.current.IsPointerOverGameObject())
        {
            local_rightPressTimer += Time.deltaTime;
        }
        else
        {
            local_rightPressTimer = 0;
        }
    }
    private void OnCloudConnectionLost(NetworkRunner runner, ShutdownReason reason, bool reconnecting)
    {
        Debug.Log($"�ƶ�����ʧ��: {reason} (ԭ��: {reconnecting})");

        if (!reconnecting)
        {
            Debug.Log("�������������ƶ�");
            // Handle scenarios where reconnection is not possible
            // e.g., notify the user, attempt manual reconnection, etc.
        }
        else
        {
            // Wait for automatic reconnection
            Debug.Log("�������������ƶ�");
            StartCoroutine(WaitForReconnection(runner));
        }
    }

    private IEnumerator WaitForReconnection(NetworkRunner runner)
    {
        yield return new WaitUntil(() => runner.SessionInfo.IsValid);
        Debug.Log("Reconnected to the Cloud!");
    }
    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log("���ӵ�������");
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        Debug.Log($"����ʧ��{reason}");
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        Debug.Log($"��������{request.RemoteAddress}");
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        Debug.Log($"����������{reason}");
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var data = new NetworkInputData();
        #region//�������
        if (Camera.main != null)
        {
            data.MouseLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Camera.main.transform.position - local_MouseOffset;
        }
        data.MouseLeftPressTimer = local_leftPressTimer;
        data.MouseRightPressTimer = local_rightPressTimer;
        #endregion
        #region//λ������
        data.PressA = Input.GetKey(KeyCode.A);
        data.PressD = Input.GetKey(KeyCode.D);
        data.PressW = Input.GetKey(KeyCode.W);
        data.PressS = Input.GetKey(KeyCode.S);
        #endregion
        input.Set(data);
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("��������" + runner.Config.Simulation.PlayerCount);
        Debug.Log("��Ҽ���" + player + "���ID" + player.RawEncoded);
        if (runner.GameMode == GameMode.Single || runner.GameMode == GameMode.Host || runner.GameMode == GameMode.Client || runner.GameMode == GameMode.AutoHostOrClient)
        {
            if (runner.IsServer)
            {
                NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, new Vector3(999, 0, 0), Quaternion.identity, player);
                _spawnedCharacters.Add(player, networkPlayerObject);
            }
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);
        }
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        Debug.Log("���´����б�" + sessionList.Count);
        MessageBroker.Default.Publish(new NetEvent.NetEvent_SessionListUpdated()
        {
             SessionList = sessionList
        });
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        Debug.Log($"�������ر�{shutdownReason}");
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
       
    }

}
public struct NetworkInputData:INetworkInput
{
    /// <summary>
    /// ���ʱ��
    /// </summary>
    public float MouseLeftPressTimer;
    /// <summary>
    /// �Ҽ�ʱ��
    /// </summary>
    public float MouseRightPressTimer;
    /// <summary>
    /// ������λ��
    /// </summary>
    public Vector2 MouseLocation;
    /// <summary>
    /// W
    /// </summary>
    public bool PressW;
    /// <summary>
    /// S
    /// </summary>
    public bool PressS;
    /// <summary>
    /// A
    /// </summary>
    public bool PressA;
    /// <summary>
    /// D
    /// </summary>
    public bool PressD;
}