using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using static GameEvent;
/// <summary>
/// 农民
/// </summary>
public class ActorManager_NPC_Farmer : ActorManager_NPC
{
    [HideInInspector]
    public short int_PlantID = 1005;
    private List<Vector3Int> ploughList = new List<Vector3Int>();
    #region//行为逻辑
    public override void State_ThinkByTimeUpdate(int date, int hour, GlobalTime time)
    {
        if (time == GlobalTime.Forenoon)
        {
            if (!State_Think_GoToWork())
            {
                State_Think_GoToStroll_Long(10, 5);
            }
            if (brainManager.state_workPostion.position == pathManager.vector3Int_CurPos)
            {
                /*在工作地块上*/
                State_Plant();
            }
            return;
        }
        if (time == GlobalTime.Highnoon)
        {
            if (!State_Think_GoForFood())
            {
                State_Think_GoToStroll_Long(10, 5);
            }
            return;
        }
        if (time == GlobalTime.Afternoon)
        {
            if (!State_Think_GoToWork())
            {
                State_Think_GoToStroll_Long(10, 5);
            }
            if (brainManager.state_workPostion.position == pathManager.vector3Int_CurPos)
            {
                /*在工作地块上*/
                State_Harvest();
            }
            return;
        }
        if (time == GlobalTime.Dusk)
        {
            if (!State_Think_GoForFood())
            {
                State_Think_GoToStroll_Long(10, 5);
            }
            return;
        }
        if (time == GlobalTime.Evening)
        {
            if (!State_Think_GoToSleep())
            {
                State_Think_GoToStroll_Long(10, 5);
            }
            return;
        }
        State_Think_GoToStroll_Long(10, 5);
    }
    public override void State_ThinkByTimeChange(int date, int hour, GlobalTime globalTime)
    {
        switch (globalTime)
        {
            case GlobalTime.Forenoon:
                State_Think_FindPloughAroundHome();
                break;
            case GlobalTime.Highnoon:
                State_Think_FindFood();
                break;
            case GlobalTime.Afternoon:
                State_Think_FindPloughAroundHome();
                break;
            case GlobalTime.Dusk:
                State_Think_FindFood();
                break;
            case GlobalTime.Evening:
                State_Think_FindBed();
                break;
        }
    }
    /// <summary>
    /// 在家周围寻找耕地
    /// </summary>
    private void State_Think_FindPloughAroundHome()
    {
        ploughList.Clear();
        for (int i = -10; i < 10; i++)
        {
            for (int j = -10; j < 10; j++)
            {
                if (MapManager.Instance.GetGround(brainManager.state_homePostion.position + new Vector3Int(i, j, 0), out GroundTile groundTile))
                {
                    if (groundTile.tileID == 2002)
                    {
                        ploughList.Add(brainManager.state_homePostion.position + new Vector3Int(i, j, 0));
                    }
                }
            }
        }
        if (ploughList.Count > 0)
        {
            brainManager.State_SetWorkPos(ploughList[0]);
        }
        else
        {
            Debug.Log("????????????????????????????????");
        }
    }
    /// <summary>
    /// 播种
    /// </summary>
    private void State_Plant()
    {
        if (!MapManager.Instance.GetBuilding(pathManager.vector3Int_CurPos, out BuildingTile buildingTile))
        {
            actorNetManager.RPC_State_NpcUseSkill((int)Skill.Plant, pathManager.vector3Int_CurPos, actorNetManager.Object.Id);
            MessageBroker.Default.Publish(new MapEvent.MapEvent_State_CreateBuildingArea()
            {
                buildingID = int_PlantID,
                buildingPos = pathManager.vector3Int_CurPos,
                areaSize = AreaSize._1X1
            });
        }
        ploughList.Remove(pathManager.vector3Int_CurPos);
        brainManager.State_ResetWorkPos();
        if (ploughList.Count > 0) { brainManager.State_SetWorkPos(ploughList[0]); }
    }
    /// <summary>
    /// 收获
    /// </summary>
    private void State_Harvest()
    {
        if (MapManager.Instance.GetBuilding(pathManager.vector3Int_CurPos, out BuildingTile buildingTile))
        {
            GameObject buildingObj = MapManager.Instance.GetBuildingObj(pathManager.vector3Int_CurPos);
            if (buildingObj.TryGetComponent(out BuildingObj_Plant_ThreeState buildingObj_Plant_ThreeState))
            {
                if (buildingObj_Plant_ThreeState.state_Now == BuildingObj_Plant_ThreeState.State.State2)
                {
                    actorNetManager.RPC_State_NpcUseSkill((int)Skill.Plant, pathManager.vector3Int_CurPos, actorNetManager.Object.Id);
                    buildingObj_Plant_ThreeState.All_Broken();
                    return;
                }
            }
            if (buildingObj.TryGetComponent(out BuildingObj_Plant_TwoState buildingObj_Plant_TwoState))
            {
                if (buildingObj_Plant_TwoState.state_Now == BuildingObj_Plant_TwoState.State.State1)
                {
                    actorNetManager.RPC_State_NpcUseSkill((int)Skill.Plant, pathManager.vector3Int_CurPos, actorNetManager.Object.Id);
                    buildingObj_Plant_TwoState.All_Broken();
                    return;
                }
            }

        }
        ploughList.Remove(pathManager.vector3Int_CurPos);
        brainManager.State_ResetWorkPos();
        if (ploughList.Count > 0) { brainManager.State_SetWorkPos(ploughList[0]); }
    }
    #endregion
    #region//交互
    /// <summary>
    /// 更新谈话
    /// </summary>
    public override void Local_SetDialog(ActorManager player, int index)
    {
        List<DialogOption> dialogOptions = new List<DialogOption>();
        DialogOption dialogOption_0 = new DialogOption();
        DialogOption dialogOption_1 = new DialogOption();
        switch (index)
        {
            case 0:
                dialogOption_0.optionTable = "Role_String";
                dialogOption_0.optionEntry = "Farmer_Dialog0_Option0";
                dialogOption_0.optionAction = (() =>
                {
                    Local_OverDialog(player);
                });
                dialogOption_1.optionTable = "Role_String";
                dialogOption_1.optionEntry = "Farmer_Dialog0_Option1";
                dialogOption_1.optionAction = (() =>
                {
                    Local_StartDeal(player);
                });
                dialogOptions.Add(dialogOption_0);
                dialogOptions.Add(dialogOption_1);
                tileUI_Dialog.InitDialog("Role_String", "Farmer_Name", "Role_String", "Farmer_Dialog0");
                tileUI_Dialog.InitOption(dialogOptions);
                break;
        }
    }
    /// <summary>
    /// 收购
    /// </summary>
    /// <param name="itemData"></param>
    /// <returns></returns>
    public override int Local_Offer(ItemData itemData)
    {
        ItemConfig itemConfig = ItemConfigData.GetItemConfig(itemData.I);
        int offer = itemConfig.Item_Value * itemData.C / 2 + 1;
        return offer;
    }
    #endregion
    #region//技能

    private enum Skill
    {
        Plant, Harvest
    }
    public override void AllClient_Listen_NpcAction(int id, Vector3Int vector3, NetworkId networkId)
    {
        if (id == (int)Skill.Plant)
        {
            AllClient_Plant();
        }
        base.AllClient_Listen_NpcAction(id, vector3, networkId);
    }
    private void AllClient_Plant()
    {
        bodyController.SetAnimatorTrigger(BodyPart.Hand, "Pick");
        bodyController.SetAnimatorTrigger(BodyPart.Head, "Pick");
    }
    #endregion
}
