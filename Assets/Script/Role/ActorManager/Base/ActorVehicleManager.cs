using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorVehicleManager
{
    private ActorManager actorManager;
    public void Bind(ActorManager actorManager)
    {
        this.actorManager = actorManager;
    }
    private VehicleManager vehicleManager_Bind;
    public IEnumerator AllClient_GetOnVehicle(VehicleManager vehicle)
    {
        vehicleManager_Bind = vehicle;
        yield return new WaitForSeconds(0.2f);
        actorManager.inputManager.AllClient_AddInputMove(vehicle.AllClient_ActorInputMove);
    }
    public IEnumerator AllClient_GetOffVehicle(VehicleManager vehicle)
    {
        yield return new WaitForSeconds(0.2f);
        vehicleManager_Bind = null;
        actorManager.inputManager.AllClient_RemoveInputMove(vehicle.AllClient_ActorInputMove);
    }

}
