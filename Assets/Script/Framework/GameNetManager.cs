using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UniRx;
using UnityEngine.UIElements;

public class GameNetManager : NetworkBehaviour
{
    public void Start()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_Local_SpawnActor>().Subscribe(_ =>
        {
            RPC_Local_SpawnActor(_.name, _.pos);
        }).AddTo(this);
        MessageBroker.Default.Receive<GameEvent.GameEvent_Local_SpawnItem>().Subscribe(_ =>
        {
            RPC_Local_SpawnItem(_.itemData, _.pos);
        }).AddTo(this);
        MessageBroker.Default.Receive<GameEvent.GameEvent_State_SpawnItem>().Subscribe(_ =>
        {
            SpawnItem(_.itemData, _.pos);
        }).AddTo(this);
        MessageBroker.Default.Receive<GameEvent.GameEvent_State_SpawnActor>().Subscribe(_ =>
        {
            SpawnActor(_.name, _.pos, _.callBack);
        }).AddTo(this);
    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_Local_SpawnActor(string name, Vector3 postion)
    {
        SpawnActor(name, postion, null);
    }
    private void SpawnActor(string name, Vector3 postion, System.Action<ActorManager> action)
    {
        if (Object.HasStateAuthority)
        {
            GameObject obj = Resources.Load<GameObject>(name);
            NetworkObject networkObject = Runner.Spawn(obj, postion, Quaternion.identity);
            networkObject.AssignInputAuthority(Runner.LocalPlayer);
            if (action != null)
            {
                action.Invoke(networkObject.GetComponent<ActorManager>());
            }
        }
    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_Local_SpawnItem(ItemData data, Vector3 postion)
    {
        SpawnItem(data, postion);
    }
    private void SpawnItem(ItemData data, Vector3 postion)
    {
        if (Object.HasStateAuthority)
        {
            GameObject obj = Resources.Load<GameObject>("ItemObj/ItemNetObj");
            NetworkObject networkPlayerObject = Runner.Spawn(obj, postion, Quaternion.identity, Object.StateAuthority);
            networkPlayerObject.GetComponent<ItemNetObj>().data = data;
        }
    }
}
