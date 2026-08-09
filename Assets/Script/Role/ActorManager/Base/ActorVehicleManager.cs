using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActorVehicleManager
{
    public VehicleState vehicleState;
    private ActorManager actorManager;
    public ActorManager actorManager_Vehicle;
    public ActorManager actorManager_Rider;
    public void Bind(ActorManager actor)
    {
        actorManager = actor;
    }
    public void AllClient_SetVehicle(ActorManager actor)
    {
        if (vehicleState == VehicleState.Default)
        {
            vehicleState = VehicleState.AsRider;
            actor.vehicleManager.AllClient_SetRider(actorManager);
            actorManager.actorNetManager.Local_ChangeCollider(false);
            actorManager.bodyController.ShowAsRider(true, actor);

            actorManager.actorNetManager.RPC_Local_SetBodyAction((short)BodyActionType.RideOn, 1, actorManager.transform.position);

            actorManager_Vehicle = actor;
        }
    }
    public void AllClient_CleanVehicle()
    {
        if(vehicleState == VehicleState.AsRider)
        {
            vehicleState = VehicleState.Default;
            actorManager_Vehicle.vehicleManager.AllClient_CleanRider();
            actorManager.actorNetManager.Local_ChangeCollider(true);
            actorManager.bodyController.ShowAsRider(false, actorManager_Vehicle);

            actorManager.actorNetManager.RPC_Local_SetBodyAction((short)BodyActionType.RideOff, 1, actorManager.transform.position);
            actorManager_Vehicle = null;
        }
    }
    public void AllClient_SetRider(ActorManager actor)
    {
        if (vehicleState == VehicleState.Default)
        {
            vehicleState = VehicleState.AsVehicle;
            actor.vehicleManager.AllClient_SetVehicle(actorManager);

            actorManager.bodyController.ShowAsVehicle(true, actor);
            actorManager_Rider = actor;
        }
    }
    public void AllClient_CleanRider()
    {
        if (vehicleState == VehicleState.AsVehicle)
        {
            vehicleState = VehicleState.Default;
            actorManager_Rider.vehicleManager.AllClient_CleanVehicle();

            actorManager.bodyController.ShowAsVehicle(false, actorManager_Rider);
            actorManager_Rider = null;
        }
    }
}
public enum VehicleState
{
    Default, AsRider, AsVehicle
}
