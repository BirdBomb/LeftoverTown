using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using static GameEvent;

public class ActorManager_NPC_Farmer : ActorManager_NPC
{
    /// <summary>
    /// ˼������
    /// </summary>
    private float float_ThinkCD = 1;
    /// <summary>
    /// �Ѳ�ʱ��
    /// </summary>
    private float float_SearchTime = 4;
    private float float_ThinkTimer;
    [HideInInspector]
    public short int_PlantID = 1005;
    #region//��ʼ��
    public override void State_Init()
    {
        float_ThinkCD += new System.Random().Next(0, 100) * 0.01f;
        float_ThinkTimer = float_ThinkCD;
        base.State_Init();
    }
    #endregion
    #region//ʱ������
    public override void State_FixedUpdateNetwork(float dt)
    {
        if (!brainManager.allClient_actorManager_AttackTarget)
        {
            float_ThinkTimer -= dt;
            if (float_ThinkTimer < 0)
            {
                float_ThinkTimer = float_ThinkCD;
                if (State_CheckToPickUp()) { State_MoveToPickUp(); }
                else { State_Think(); }
            }
        }
        base.State_FixedUpdateNetwork(dt);
    }
    public override void State_CustomUpdate()
    {
        State_CheckNearby();
        base.State_CustomUpdate();
    }
    public override void Local_CustomUpdate()
    {
        actionManager.FaceTo(bodyController.turnDir);
        base.Local_CustomUpdate();
    }
    public override void Local_SecondUpdate()
    {
        base.Local_SecondUpdate();
    }
    #endregion
    #region//�����ķ�Ӧ
    public override void AllClient_Listen_UpdateTime(int hour, int date, GlobalTime globalTime)
    {
        brainManager.SetTime(globalTime);
        base.AllClient_Listen_UpdateTime(hour, date, globalTime);
    }
    public override void State_Listen_UpdateTime(int hour, int date, GlobalTime globalTime)
    {
        if (globalTime == GlobalTime.Morning)
        {
            State_Think_FindPloughAroundHome();
        }
        if (globalTime == GlobalTime.Highnoon)
        {
            State_Think_FindPloughAroundHome();
        }
        base.State_Listen_UpdateTime(hour, date, globalTime);
    }
    public override void State_Listen_MyselfHpChange(int parameter, HpChangeReason reason, NetworkId id)
    {
        NetworkObject networkObject = actorNetManager.Runner.FindObject(id);
        if (networkObject != null)
        {
            ActorManager who = networkObject.GetComponent<ActorManager>();
            if (who.actorAuthority.isPlayer && actionManager.LookAt(who, config.short_View))
            {
                who.actionManager.SetFine(500);
                if (brainManager.allClient_actorManager_AttackTarget == null)
                {
                    State_TryToSendEmoji(0, Emoji.Menace);
                    State_InAttack(who);
                }
                else
                {
                    if (!brainManager.allClient_actorManager_AttackTarget.actorAuthority.isPlayer)
                    {
                        State_InAttack(who);
                    }
                }
            }
        }
        base.State_Listen_MyselfHpChange(parameter, reason, id);
    }
    public override void State_Listen_RoleCommit(ActorManager who, short val)
    {
        if (who.actorAuthority.isPlayer && actionManager.LookAt(who, config.short_View))
        {
            who.actionManager.SetFine(val);
            State_TryToSendEmoji(0, Emoji.Yell);
        }
        base.State_Listen_RoleCommit(who, val);
    }
    public override void State_Listen_RoleSendEmoji(ActorManager actor, Emoji emoji, float distance)
    {
        if (actionManager.HearTo(actor, distance))
        {
            switch (emoji)
            {
                case Emoji.Yell:
                    if (brainManager.allClient_actorManager_AttackTarget == null)
                    {
                        brainManager.SetSearch(actor.pathManager.vector3Int_CurPos);
                        State_TryToSendEmoji(0, Emoji.Puzzled);
                        State_Search();
                        pathManager.State_MovePostion(actor.pathManager.vector3Int_CurPos);
                    }
                    break;
            }
        }
        base.State_Listen_RoleSendEmoji(actor, emoji, distance);
    }
    public override void State_Listen_RoleInView(ActorManager actor)
    {
        brainManager.actorManagers_Nearby.Add(actor);
        base.State_Listen_RoleInView(actor);
    }
    public override void State_Listen_RoleOutView(ActorManager actor)
    {
        brainManager.actorManagers_Nearby.Remove(actor);
        base.State_Listen_RoleInView(actor);
    }
    public override void State_Listen_ItemInView(ItemNetObj obj)
    {
        brainManager.ItemNetObj_Nearby.Add(obj);
        base.State_Listen_ItemInView(obj);
    }
    public override void State_Listen_ItemOutView(ItemNetObj obj)
    {
        brainManager.ItemNetObj_Nearby.Remove(obj);
        base.State_Listen_ItemOutView(obj);
    }
    /// <summary>
    /// ��鸽��
    /// </summary>
    public void State_CheckNearby()
    {
        if (brainManager.allClient_actorManager_AttackTarget != null)
        {
            if (actionManager.LookAt(brainManager.allClient_actorManager_AttackTarget, 4))
            {
                State_RunAway(brainManager.allClient_actorManager_AttackTarget);
            }
            if (!actionManager.HearTo(brainManager.allClient_actorManager_AttackTarget, config.short_View))
            {
                State_OutAttack();
            }
        }
        else
        {
            if (brainManager.allClient_actorManager_AttackTargetID != new Fusion.NetworkId())
            {
                State_OutAttack();
            }
            State_CheckNearbyActor();
        }
    }
    /// <summary>
    /// ��鸽����ɫ
    /// </summary>
    /// <returns>��ֹ˼��</returns>
    private bool State_CheckNearbyActor()
    {
        for (int i = 0; i < brainManager.actorManagers_Nearby.Count; i++)
        {
            if (actionManager.LookAt(brainManager.actorManagers_Nearby[i], config.short_View))
            {
                if (brainManager.actorManagers_Nearby[i].statusManager.statusType == StatusType.Monster_Common)
                {
                    State_InAttack(brainManager.actorManagers_Nearby[i]);
                    return true;
                }
                if (brainManager.actorManagers_Nearby[i].statusManager.statusType == StatusType.Human_Offender)
                {
                    State_InAttack(brainManager.actorManagers_Nearby[i]);
                    return true;
                }
            }
        }
        return false;
    }
    #endregion
    #region//ʰȡ״̬
    /// <summary>
    /// ���ʰȡ
    /// </summary>
    /// <returns>����Ŀ��</returns>
    private bool State_CheckToPickUp()
    {
        ItemNetObj target = null;
        float distance = float.MaxValue;
        for (int i = 0; i < brainManager.ItemNetObj_Nearby.Count; i++)
        {
            float temp = Vector2.Distance(transform.position, brainManager.ItemNetObj_Nearby[i].transform.position);
            if (temp < distance)
            {
                distance = temp;
                target = brainManager.ItemNetObj_Nearby[i];
            }
        }
        if (target != null)
        {
            /*ǰ��Ŀ��*/
            brainManager.ItemNetObj_Target = target;
            return true;
        }
        else
        {
            /*û��Ŀ��*/
            brainManager.ItemNetObj_Target = null;
            return false;
        }
    }
    /// <summary>
    /// ǰ��ʰȡ
    /// </summary>
    /// <returns>�Ƿ����ȥʰȡ</returns>
    private void State_MoveToPickUp()
    {
        if (brainManager.ItemNetObj_Target != null)
        {
            Vector3Int targetPos = MapManager.Instance.grid_Ground.WorldToCell(brainManager.ItemNetObj_Target.transform.position);
            if (pathManager.State_MovePostion(targetPos))
            {
                State_TryToSendEmoji(0, Emoji.Happy);
            }
            actionManager.PickUp(1f);
        }
    }
    #endregion
    #region//ս��״̬
    /// <summary>
    /// ���빥��״̬
    /// </summary>
    /// <param name="actor"></param>
    private void State_InAttack(ActorManager actor)
    {
        actorNetManager.RPC_State_NpcChangeAttackTarget(actor.actorNetManager.Object.Id);
    }
    /// <summary>
    /// �뿪����״̬
    /// </summary>
    private void State_OutAttack()
    {
        actorNetManager.RPC_State_NpcChangeAttackTarget(new NetworkId());
    }
    /// <summary>
    /// ����
    /// </summary>
    private void State_RunAway(ActorManager from)
    {
        int randomX = new System.Random().Next(0, 2);
        int randomY = new System.Random().Next(0, 2);
        Vector3Int dirX = Vector3Int.zero;
        Vector3Int dirY = Vector3Int.zero;

        #region//���ȳ��ԶԽ�����
        if (from.pathManager.vector3Int_CurPos.x > pathManager.vector3Int_CurPos.x)
        {
            dirX += Vector3Int.left;
        }
        else if (from.pathManager.vector3Int_CurPos.x < pathManager.vector3Int_CurPos.x)
        {
            dirX += Vector3Int.right;
        }
        if (from.pathManager.vector3Int_CurPos.y > pathManager.vector3Int_CurPos.y)
        {
            dirY += Vector3Int.down;
        }
        else if (from.pathManager.vector3Int_CurPos.y < pathManager.vector3Int_CurPos.y)
        {
            dirY += Vector3Int.up;
        }
        if (pathManager.State_MovePostion(pathManager.vector3Int_CurPos + dirX + dirY)) return;
        #endregion
        #region//Ȼ������������
        int distanceX = Math.Abs(from.pathManager.vector3Int_CurPos.x - pathManager.vector3Int_CurPos.x);
        int distanceY = Math.Abs(from.pathManager.vector3Int_CurPos.y - pathManager.vector3Int_CurPos.y);
        if (distanceX >= distanceY)
        {
            if (pathManager.State_MovePostion(pathManager.vector3Int_CurPos + dirX)) return;
            if (randomY == 0)
            {
                dirY = Vector3Int.up;
            }
            else
            {
                dirY = Vector3Int.down;
            }
            if (pathManager.State_MovePostion(pathManager.vector3Int_CurPos + dirY)) return;
            dirY = -dirY;
            if (pathManager.State_MovePostion(pathManager.vector3Int_CurPos + dirY)) return;
        }
        else
        {
            if (pathManager.State_MovePostion(pathManager.vector3Int_CurPos + dirY)) return;
            if (randomX == 0)
            {
                dirX = Vector3Int.right;
            }
            else
            {
                dirX = Vector3Int.left;
            }
            if (pathManager.State_MovePostion(pathManager.vector3Int_CurPos + dirX)) return;
            dirX = -dirX;
            if (pathManager.State_MovePostion(pathManager.vector3Int_CurPos + dirX)) return;
        }
        #endregion
    }
    /// <summary>
    /// ��Ѱ
    /// </summary>
    /// <param name="vector2"></param>
    private void State_Search()
    {
        if (brainManager.searchPostion.isValue)
        {
            float_ThinkTimer = float_SearchTime;
            pathManager.State_MovePostion(brainManager.searchPostion.postion);
            brainManager.ResetSearch();
        }
    }
    #endregion
    #region//����״̬
    /// <summary>
    /// ˼������ʲô
    /// </summary>
    private void State_Think()
    {
        if (brainManager.globalTime_Now == GlobalTime.Evening)
        {
            /*���ϻؼ�*/
            if (brainManager.homePostion.isValue)
            {
                if (brainManager.homePostion.postion != pathManager.vector3Int_CurPos)
                {
                    State_Think_GoToHome();
                    return;
                }
                else
                {
                    
                    return;
                }
            }
        }
        else if (brainManager.globalTime_Now == GlobalTime.Forenoon)
        {
            /*���粥��*/
            if (brainManager.workPostion.isValue)
            {
                if (brainManager.workPostion.postion != pathManager.vector3Int_CurPos)
                {
                    /*û�ڹ����ؿ���*/
                    State_Think_GoToWork();
                    return;
                }
                else
                {
                    /*�ڹ����ؿ���*/
                    State_Plant();
                    return;
                }
            }
        }
        else if (brainManager.globalTime_Now == GlobalTime.Afternoon)
        {
            /*�����ջ�*/
            if (brainManager.workPostion.isValue)
            {
                if (brainManager.workPostion.postion != pathManager.vector3Int_CurPos)
                {
                    /*û�ڹ����ؿ���*/
                    State_Think_GoToWork();
                    return;
                }
                else
                {
                    /*�ڹ����ؿ���*/
                    State_Harvest();
                    return;
                }
            }
        }
        State_Think_Stroll(3);
    }
    /// <summary>
    /// �ؼ�
    /// </summary>
    private void State_Think_GoToHome()
    {
        pathManager.State_MovePostion(brainManager.homePostion.postion + Vector3Int.down);
    }
    private List<Vector3Int> ploughList = new List<Vector3Int>();
    /// <summary>
    /// �ڼ���ΧѰ�Ҹ���
    /// </summary>
    private void State_Think_FindPloughAroundHome()
    {
        ploughList.Clear();
        for (int i = -10; i < 10; i++)
        {
            for (int j = -10; j < 10; j++)
            {
                if (MapManager.Instance.GetGround(brainManager.homePostion.postion + new Vector3Int(i, j, 0), out GroundTile groundTile))
                {
                    if (groundTile.tileID == 2002)
                    {
                        ploughList.Add(brainManager.homePostion.postion + new Vector3Int(i, j, 0));
                    }
                }
            }
        }
        if (ploughList.Count > 0)
        {

            brainManager.SetWork(ploughList[0]);
        }
        else
        {
            Debug.Log("????????????????????????????????");
        }
    }
    /// <summary>
    /// ȥ����
    /// </summary>
    private void State_Think_GoToWork()
    {
        pathManager.State_MovePostion(brainManager.workPostion.postion);
    }
    /// <summary>
    /// ����
    /// </summary>
    private void State_Plant()
    {
        if (!MapManager.Instance.GetBuilding(pathManager.vector3Int_CurPos, out BuildingTile buildingTile))
        {
            actorNetManager.RPC_State_NpcUseSkill((int)Skill.Plant, pathManager.vector3Int_CurPos, actorNetManager.Object.Id);
            MessageBroker.Default.Publish(new MapEvent.MapEvent_State_ChangeBuildingArea()
            {
                buildingID = int_PlantID,
                buildingPos = pathManager.vector3Int_CurPos,
                areaSize = AreaSize._1X1
            });
        }
        ploughList.Remove(pathManager.vector3Int_CurPos);
        brainManager.ResetWork();
        if (ploughList.Count > 0) { brainManager.SetWork(ploughList[0]); }
    }
    /// <summary>
    /// �ջ�
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
        brainManager.ResetWork();
        if (ploughList.Count > 0) { brainManager.SetWork(ploughList[0]); }
    }
    /// <summary>
    /// �й�
    /// </summary>
    private void State_Think_Stroll(int distance, int tryTime = 0)
    {
        if (distance < 1 || tryTime > 3)
        {
            /*�й�ʧ��*/
            State_TryToSendEmoji(0.1f, Emoji.Search);
        }
        else
        {
            Vector3Int random = new Vector3Int(new System.Random().Next(-distance, distance + 1), new System.Random().Next(-distance, distance + 1));
            Vector3Int offset = Vector3Int.zero;
            if (brainManager.activityPostion.isValue)
            {
                if (brainManager.activityPostion.postion.x - pathManager.vector3Int_CurPos.x > 5)
                {
                    offset += Vector3Int.right;
                }
                if (brainManager.activityPostion.postion.x - pathManager.vector3Int_CurPos.x < -5)
                {
                    offset += Vector3Int.left;
                }
                if (brainManager.activityPostion.postion.y - pathManager.vector3Int_CurPos.y > 5)
                {
                    offset += Vector3Int.up;
                }
                if (brainManager.activityPostion.postion.y - pathManager.vector3Int_CurPos.y < -5)
                {
                    offset += Vector3Int.down;
                }
            }
            if (!pathManager.State_MovePostion(pathManager.vector3Int_CurPos + random + offset))
            {
                /*�޷��ִ�*/
                State_Think_Stroll(distance - 1, tryTime + 1);
            }
        }
    }
    #endregion
    #region//����
    /// <summary>
    /// ����̸��
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
    /// �չ�
    /// </summary>
    /// <param name="itemData"></param>
    /// <returns></returns>
    public override int Local_Offer(ItemData itemData)
    {
        ItemConfig itemConfig = ItemConfigData.GetItemConfig(itemData.Item_ID);
        int offer = itemConfig.Item_Value * itemData.Item_Count / 2 + 1;
        return offer;
    }
    #endregion
    #region//����

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
    private void AllClient_StartTalk(Vector3Int vector3, NetworkId networkId)
    {
        actionManager.TurnTo(actorNetManager.Runner.FindObject(networkId).transform.position - transform.position);
        if (actorAuthority.isState)
        {
            pathManager.State_StandDown(999);
        }
    }
    private void AllClient_OverTalk(Vector3Int vector3, NetworkId networkId)
    {
        if (actorAuthority.isState)
        {
            pathManager.State_StandDown(1);
        }
    }
    private void AllClient_Plant()
    {
        bodyController.SetAnimatorTrigger(BodyPart.Hand, "Pick");
        bodyController.SetAnimatorTrigger(BodyPart.Head, "Pick");
    }
    #endregion
}
