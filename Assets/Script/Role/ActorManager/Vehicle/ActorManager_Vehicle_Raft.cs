using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorManager_Vehicle_Raft : ActorManager_Vehicle
{
    #region//½»»¥
    public override bool Local_IsInteractable()
    {
        return vehicleManager.vehicleState == VehicleState.Default;
    }
    public override void Local_PlayerClose(ActorManager player)
    {
        if (Local_IsInteractable())
        {
            actorUI.ShowSingal_R(true);
        }
        base.Local_PlayerClose(player);
    }
    public override void Local_PlayerFaraway(ActorManager player)
    {
        actorUI.HideAllSingal();
        base.Local_PlayerFaraway(player);
    }
    public override void Local_GetPlayerInput(ActorManager actor, KeyCode keyCode)
    {
        switch (keyCode)
        {
            case KeyCode.R:
                {
                    if (vehicleManager.vehicleState == VehicleState.Default)
                    {
                        Local_TryToGetOn(actor);
                        actorUI.HideAllSingal();
                    }
                    else if (vehicleManager.vehicleState == VehicleState.AsVehicle && vehicleManager.actorManager_Rider == actor)
                    {
                        Local_TryToGetOff(actor);
                        actorUI.ShowSingal_R(true);
                    }
                }
                break;
        }
        base.Local_GetPlayerInput(actor, keyCode);
    }
    #endregion
    #region//Æï³Ë

    public virtual void Local_TryToGetOn(ActorManager actor)
    {
        actorNetManager.RPC_Local_NpcUseSkill((int)Skill.CarryStart, pathManager.vector3Int_CurPos, actor.actorNetManager.Object.Id);
    }
    public virtual void Local_TryToGetOff(ActorManager actor)
    {
        actorNetManager.RPC_Local_NpcUseSkill((int)Skill.CarryOver, pathManager.vector3Int_CurPos, actor.actorNetManager.Object.Id);
    }
    #endregion
    #region//¼¼ÄÜ
    private enum Skill
    {
        CarryStart, CarryOver
    }
    public override void ForAll_Listen_NpcAction(int id, Vector3Int vector3, NetworkId networkId)
    {
        if (id == (int)Skill.CarryStart)
        {
            AllClinet_CarryStart(vector3, networkId);
        }
        if (id == (int)Skill.CarryOver)
        {
            AllClinet_CarryOver(vector3, networkId);
        }
        base.ForAll_Listen_NpcAction(id, vector3, networkId);
    }
    private void AllClinet_CarryStart(Vector3Int vector3, NetworkId networkId)
    {
        actionManager.TurnTo(((Vector3)(vector3 - pathManager.vector3Int_CurPos)));
        if (actorNetManager.Runner.FindObject(networkId).TryGetComponent(out ActorManager actor))
        {
            actor.actorNetManager.Net_Vehicle = actorNetManager.Object.Id;
        }
    }
    private void AllClinet_CarryOver(Vector3Int vector3, NetworkId networkId)
    {
        actionManager.TurnTo(((Vector3)(vector3 - pathManager.vector3Int_CurPos)));
        if (actorNetManager.Runner.FindObject(networkId).TryGetComponent(out ActorManager actor))
        {
            actor.actorNetManager.Net_Vehicle = new NetworkId();
        }
    }
    #endregion
}
