using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using UniRx;

public class BasicSpawner : MonoBehaviour,INetworkRunnerCallbacks
{
    [SerializeField,Header("���Ԥ����")] private NetworkPrefabRef _playerPrefab;
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

    private int local_leftClick;
    private int local_rightClick;
    private float local_leftPressTimer;
    private float local_rightPressTimer;
    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            local_leftPressTimer += Time.deltaTime;
        }
        else
        {
            local_leftPressTimer = 0;
        }
        if (Input.GetKey(KeyCode.Mouse1))
        {
            local_rightPressTimer += Time.deltaTime;
        }
        else
        {
            local_rightPressTimer = 0;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            local_leftClick++;
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            local_rightClick++;
        }
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        
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
            data.MouseLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Camera.main.transform.position;
        }
        if (local_leftClick > 0)
        {
            data.MouseLeftClick = true;
            local_leftClick = 0;
        }
        else { data.MouseLeftClick = false; }
        if (local_rightClick > 0) 
        { 
            data.MouseRightClick = true; ;
            local_rightClick = 0;
        }
        else { data.MouseRightClick = false; }
        data.MouseLeftPressTimer = local_leftPressTimer;
        data.MouseRightPressTimer = local_rightPressTimer;
        #endregion
        #region//λ������
        data.PressA = Input.GetKey(KeyCode.A);
        data.PressD = Input.GetKey(KeyCode.D);
        data.PressW = Input.GetKey(KeyCode.W);
        data.PressS = Input.GetKey(KeyCode.S);
        data.PressLeftShift = Input.GetKey(KeyCode.LeftShift);
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
        Debug.Log("��Ҽ���" + player);
        if (runner.IsServer)
        {
            // Create a unique position for the player
            Vector3 spawnPosition = new Vector3((player.RawEncoded % runner.Config.Simulation.PlayerCount) * 3, 1, 0);
            NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
            // Keep track of the player avatars for easy access
            _spawnedCharacters.Add(player, networkPlayerObject);
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
        
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
       
    }

}
public struct NetworkInputData:INetworkInput
{
    /// <summary>
    /// ������
    /// </summary>
    public bool MouseLeftClick;
    /// <summary>
    /// �Ҽ����
    /// </summary>
    public bool MouseRightClick;
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
    /// <summary>
    /// Shift
    /// </summary>
    public bool PressLeftShift;
}