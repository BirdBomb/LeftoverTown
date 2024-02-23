using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using UniRx;

public class BasicSpawner : MonoBehaviour,INetworkRunnerCallbacks
{
    [SerializeField,Header("玩家预制体")] private NetworkPrefabRef _playerPrefab;
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

    private int leftClick;
    private int rightClick;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            leftClick++;
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            rightClick++;
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
        if (Camera.main != null)
        {
            data.mousePostion = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (leftClick > 0)
        {
            data.ClickLeftMouse = 1;
            leftClick = 0;
        }
        else { data.ClickLeftMouse = 0; }
        if (rightClick > 0) 
        { 
            data.ClickRightMouse = 1;
            rightClick = 0;
        }
        else { data.ClickRightMouse = 0; }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            data.PressLeftMouse = true;
        }
        else
        {
            data.PressLeftMouse = false;
        }
        if (Input.GetKey(KeyCode.Mouse1))
        {
            data.PressRightMouse = true;
        }
        else
        {
            data.PressRightMouse = false;
        }
        #region//位移
        if (Input.GetKey(KeyCode.A))
        {
            data.goLeft = true;
        }
        else
        {
            data.goLeft = false;
        }
        if (Input.GetKey(KeyCode.D))
        {
            data.goRight = true;
        }
        else
        {
            data.goRight = false;
        }
        if (Input.GetKey(KeyCode.W))
        {
            data.goUp = true;
        }
        else
        {
            data.goUp = false;
        }
        if (Input.GetKey(KeyCode.S))
        {
            data.goDown = true;
        }
        else
        {
            data.goDown = false;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            data.goFaster = true;
        }
        else
        {
            data.goFaster = false;
        }
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
        Debug.Log("玩家加入");
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
    public int ClickE;
    public int ClickQ;
    public int ClickM;

    public Vector3 mousePostion;
    public int ClickLeftMouse;
    public int ClickRightMouse;
    public bool PressLeftMouse;
    public bool PressRightMouse;

    public bool goUp;
    public bool goDown;
    public bool goLeft;
    public bool goRight;
    public bool goFaster;
}