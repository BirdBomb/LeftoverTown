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
    private List<short> list_PlantID = new List<short>() { 1300  };
    private Queue<Vector3Int> queue_PloughPos = new Queue<Vector3Int>();
    private System.Random rand = new System.Random();
    #region//行为逻辑
    public override void State_ThinkByTimeUpdate(int date, int hour, GlobalTime time)
    {
        switch (time)
        {
            case GlobalTime.Morning:
                {
                    if (State_Think_GoForFood()) return;
                    break;
                }
            case GlobalTime.Forenoon:
                {
                    if (brainManager.state_workPostion.isValue && brainManager.state_workPostion.position == pathManager.vector3Int_CurPos)
                    {
                        State_Plant();
                        return;
                    }
                    else if (State_Think_GoToWork())
                    {
                        return;
                    }
                    break;
                }
            case GlobalTime.Afternoon:
                {
                    if (brainManager.state_workPostion.isValue && brainManager.state_workPostion.position == pathManager.vector3Int_CurPos)
                    {
                        State_Harvest();
                        return;
                    }
                    else if(State_Think_GoToWork())
                    {
                        return;
                    }
                    break;
                }
            case GlobalTime.Dusk:
                {
                    if (State_Think_GoForFood()) return;
                    break;
                }
            case GlobalTime.Evening:
                {
                    if (State_Think_GoToSleep()) return;
                    break;
                }
        }
        if (State_Think_GoToWork()) return;
        base.State_ThinkByTimeUpdate(date,hour,time);
    }
    public override void State_ThinkByTimeChange(int date, int hour, GlobalTime globalTime)
    {
        switch (globalTime)
        {
            case GlobalTime.Forenoon:
                StartCoroutine(State_Think_FindPloughAroundHome());
                break;
            case GlobalTime.Highnoon:
                StartCoroutine(State_Think_FindFoodPos());
                break;
            case GlobalTime.Afternoon:
                StartCoroutine(State_Think_FindPloughAroundHome());
                break;
            case GlobalTime.Dusk:
                StartCoroutine(State_Think_FindFoodPos());
                break;
            case GlobalTime.Evening:
                StartCoroutine(State_Think_FindSleepPos());
                break;
        }
    }
    public IEnumerator State_Think_FindPloughAroundHome(int maxDistance = 15)
    {
        queue_PloughPos.Clear();
        for (int i = -maxDistance; i < maxDistance; i++)
        {
            for (int j = -maxDistance; j < maxDistance; j++)
            {
                if (MapManager.Instance.GetGround(brainManager.state_homePostion.position + new Vector3Int(i, j, 0), out GroundTile groundTile))
                {
                    if (groundTile.tileID == 2002)
                    {
                        queue_PloughPos.Enqueue(brainManager.state_homePostion.position + new Vector3Int(i, j, 0));
                    }
                }
            }
            yield return true;
        }
        if (queue_PloughPos.Count > 0) { brainManager.ForState_SetWorkPos(queue_PloughPos.Dequeue()); }
    }
    /// <summary>
    /// 播种
    /// </summary>
    private void State_Plant()
    {
        if (!MapManager.Instance.GetBuilding(pathManager.vector3Int_CurPos, out BuildingTile buildingTile))
        {
            actorNetManager.RPC_State_NpcUseSkill((int)ActorSkill_NPC.Pick, pathManager.vector3Int_CurPos, actorNetManager.Object.Id);
            MessageBroker.Default.Publish(new MapEvent.MapEvent_State_CreateBuildingArea()
            {
                buildingID = list_PlantID[rand.Next(0,list_PlantID.Count)],
                buildingPos = pathManager.vector3Int_CurPos,
                areaSize = AreaSize._1X1
            });
        }
        brainManager.ForState_ResetWorkPos();
        if (queue_PloughPos.Count > 0) { brainManager.ForState_SetWorkPos(queue_PloughPos.Dequeue()); }
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
                if (buildingObj_Plant_ThreeState.All_GetCurState() == BuildingObj_Growth_ThreeState.State.Mature)
                {
                    actorNetManager.RPC_State_NpcUseSkill((int)ActorSkill_NPC.Pick, pathManager.vector3Int_CurPos, actorNetManager.Object.Id);
                    buildingObj_Plant_ThreeState.All_Broken();
                    return;
                }
            }
            if (buildingObj.TryGetComponent(out BuildingObj_Plant_TwoState buildingObj_Plant_TwoState))
            {
                if (buildingObj_Plant_TwoState.All_GetCurState() == BuildingObj_Growth_TwoState.State.Mature)
                {
                    actorNetManager.RPC_State_NpcUseSkill((int)ActorSkill_NPC.Pick, pathManager.vector3Int_CurPos, actorNetManager.Object.Id);
                    buildingObj_Plant_TwoState.All_Broken();
                    return;
                }
            }

        }
        brainManager.ForState_ResetWorkPos();
        if (queue_PloughPos.Count > 0) { brainManager.ForState_SetWorkPos(queue_PloughPos.Dequeue()); }
    }
    #endregion
    #region//交互
    public override void ForAll_InitDialog()
    {
        dialogMap = new Dictionary<int, Action>();
        dialogMap[0] = Dialog_Start;
    }
    public override void Local_StartDialog()
    {
        base.Local_StartDialog();
        ChooseDialog(0);
    }
    public void Dialog_Start()
    {
        List<DialogOption> options = new List<DialogOption>
        {
            new DialogOption("Farmer_Dialog0_Option0", Local_StartDeal),
            new DialogOption("Farmer_Dialog0_Option1", Local_OverDialog)
        };
        ShowDialog("Farmer_Dialog0", options);
    }
    #endregion
    #region//交易
    public override int Local_Offer(ItemData itemData)
    {
        ItemConfig itemConfig = ItemConfigData.GetItemConfig(itemData.I);
        int offer = itemConfig.Item_Value * itemData.C / 2 + 1;
        return offer;
    }
    #endregion

}
