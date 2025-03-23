using Fusion;
using Fusion.Addons.Physics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleNetManager : NetworkBehaviour
{
    [SerializeField, Header("�������")]
    public NetworkRigidbody2D networkRigidbody2D_VehicleBody;
    [SerializeField, Header("���ؿ�����")]
    public VehicleManager vehicleManager_Local;
    [HideInInspector]
    public string string_Data = "";
    /// <summary>
    /// ���ض˸���λ��
    /// </summary>
    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_UpdateNetworkTransform(Vector3 pos, float speed)
    {
        OnlyState_UpdateNetworkTransform(pos, speed);
    }
    public void OnlyState_UpdateNetworkTransform(Vector3 pos, float speed)
    {
        if (networkRigidbody2D_VehicleBody)
        {
            if (networkRigidbody2D_VehicleBody.Rigidbody.velocity.magnitude <= speed)
            {
                networkRigidbody2D_VehicleBody.Rigidbody.velocity = Vector2.zero;

                networkRigidbody2D_VehicleBody.Rigidbody.position = (pos);
            }
        }
    }
    /// <summary>
    /// ���ض��ϳ�
    /// </summary>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_LocalInput_ActorGetOn(NetworkId networkId)
    {
        NetworkObject networkObject = Runner.FindObject(networkId);
        if (networkObject.transform.TryGetComponent(out ActorManager actor))
        {
            vehicleManager_Local.FromRPC_AllClient_GetOn(actor);
        }
    }
    /// <summary>
    /// ���ض��³�
    /// </summary>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_LocalInput_ActorGetOff(NetworkId networkId)
    {
        NetworkObject networkObject = Runner.FindObject(networkId);
        if (networkObject.transform.TryGetComponent(out ActorManager actor))
        {
            vehicleManager_Local.FromRPC_AllClient_GetOff(actor);
        }
    }
    /// <summary>
    /// ���ض˸�����Ϣ
    /// </summary>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_ChangeInfo(string info)
    {
        RPC_State_ChangeInfo(info);
    }
    /// <summary>
    /// ���ض�������Ϣ
    /// </summary>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_RequestInfo()
    {
        RPC_State_ChangeInfo(string_Data);
    }
    /// <summary>
    /// ����˸�����Ϣ
    /// </summary>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPC_State_ChangeInfo(string info)
    {
        string_Data = info;
        vehicleManager_Local.FromRPC_AllClient_UpdateInfo(string_Data);
    }
    /// <summary>
    /// ���ض˸��ĳ���ת��
    /// </summary>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void Rpc_LocalInput_SetDirection(VehicleDirection vehicleDirection)
    {
        vehicleManager_Local.FromRPC_AllClient_SetDirection(vehicleDirection);
    }
    /// <summary>
    /// ���ض˸��ĳ���״̬
    /// </summary>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void Rpc_LocalInput_Engine(bool engineOn)
    {
        vehicleManager_Local.FromRPC_AllClient_Engine(engineOn);
    }
}
