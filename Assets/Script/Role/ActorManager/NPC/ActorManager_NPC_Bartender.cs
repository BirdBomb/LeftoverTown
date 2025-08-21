using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using static GameEvent;

public class ActorManager_NPC_Bartender : ActorManager_NPC
{
    /// <summary>
    /// 思考周期
    /// </summary>
    private float float_ThinkCD = 1;
    /// <summary>
    /// 搜查时长
    /// </summary>
    private float float_SearchTime = 4;
    private float float_ThinkTimer;
    #region//初始化
    public override void State_Init()
    {
        float_ThinkCD += new System.Random().Next(0, 100) * 0.01f;
        float_ThinkTimer = float_ThinkCD;
        base.State_Init();
    }
    #endregion
    #region//时间周期
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
    #region//对外界的反应
    public override void AllClient_Listen_UpdateTime(int hour, int date, GlobalTime globalTime)
    {
        brainManager.SetTime(globalTime);
        base.AllClient_Listen_UpdateTime(hour, date, globalTime);
    }
    public override void State_Listen_UpdateTime(int hour, int date, GlobalTime globalTime)
    {
        if (globalTime == GlobalTime.Morning)
        {
            State_Think_FindCounter();
        }
        else if (globalTime == GlobalTime.Highnoon)
        {
            State_Think_FindCounter();
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
    /// 检查附近
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
    /// 检查附近角色
    /// </summary>
    /// <returns>终止思考</returns>
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
    #region//拾取状态
    /// <summary>
    /// 检查拾取
    /// </summary>
    /// <returns>发现目标</returns>
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
            /*前往目标*/
            brainManager.ItemNetObj_Target = target;
            return true;
        }
        else
        {
            /*没有目标*/
            brainManager.ItemNetObj_Target = null;
            return false;
        }
    }
    /// <summary>
    /// 前往拾取
    /// </summary>
    /// <returns>是否可以去拾取</returns>
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
    #region//战斗状态
    /// <summary>
    /// 进入攻击状态
    /// </summary>
    /// <param name="actor"></param>
    private void State_InAttack(ActorManager actor)
    {
        actorNetManager.RPC_State_NpcChangeAttackTarget(actor.actorNetManager.Object.Id);
    }
    /// <summary>
    /// 离开攻击状态
    /// </summary>
    private void State_OutAttack()
    {
        actorNetManager.RPC_State_NpcChangeAttackTarget(new NetworkId());
    }
    /// <summary>
    /// 逃走
    /// </summary>
    private void State_RunAway(ActorManager from)
    {
        int randomX = new System.Random().Next(0, 2);
        int randomY = new System.Random().Next(0, 2);
        Vector3Int dirX = Vector3Int.zero;
        Vector3Int dirY = Vector3Int.zero;

        #region//首先尝试对角逃跑
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
        #region//然后尝试四向逃跑
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
    /// 搜寻
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
    #region//闲置状态
    /// <summary>
    /// 思考该做什么
    /// </summary>
    private void State_Think()
    {
        if (brainManager.globalTime_Now == GlobalTime.Evening)
        {
            /*晚上回家*/
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
            /*上午出摊*/
            if (brainManager.workPostion.isValue)
            {
                if (brainManager.workPostion.postion != pathManager.vector3Int_CurPos)
                {
                    /*没在工作地块上*/
                    State_Think_GoToWork();
                    return;
                }
                else
                {
                    /*在工作地块上*/
                    return;
                }
            }
        }
        else if (brainManager.globalTime_Now == GlobalTime.Afternoon)
        {
            /*下午出摊*/
            if (brainManager.workPostion.isValue)
            {
                if (brainManager.workPostion.postion != pathManager.vector3Int_CurPos)
                {
                    /*没在工作地块上*/
                    State_Think_GoToWork();
                    return;
                }
                else
                {
                    /*在工作地块上*/
                    return;
                }
            }
        }
        State_Think_Stroll(3);
    }
    /// <summary>
    /// 回家
    /// </summary>
    private void State_Think_GoToHome()
    {
        pathManager.State_MovePostion(brainManager.homePostion.postion + Vector3Int.down);
    }
    private List<Vector3Int> ploughList = new List<Vector3Int>();
    /// <summary>
    /// 在家周围寻找柜台
    /// </summary>
    private void State_Think_FindCounter()
    {
        brainManager.ResetWork();
        for (int i = -10; i < 10; i++)
        {
            for (int j = -10; j < 10; j++)
            {
                if (MapManager.Instance.GetBuilding(brainManager.homePostion.postion + new Vector3Int(i, j, 0), out BuildingTile buildingTile))
                {
                    if (buildingTile.tileID == 2102)
                    {
                        brainManager.SetWork(brainManager.homePostion.postion + new Vector3Int(i, j, 0));
                    }
                }
            }
        }
    }
    /// <summary>
    /// 在家周围寻找床铺
    /// </summary>
    private void State_Think_FindBed()
    {
        for (int i = -10; i < 10; i++)
        {
            for (int j = -10; j < 10; j++)
            {
                if (MapManager.Instance.GetBuilding(brainManager.homePostion.postion + new Vector3Int(i, j, 0), out BuildingTile buildingTile))
                {
                    if (buildingTile.tileID == 3008)
                    {
                        brainManager.SetWork(brainManager.homePostion.postion + new Vector3Int(i, j, 0));
                    }
                }
            }
        }
    }
    /// <summary>
    /// 去工作
    /// </summary>
    private void State_Think_GoToWork()
    {
        pathManager.State_MovePostion(brainManager.workPostion.postion);
    }
    /// <summary>
    /// 闲逛
    /// </summary>
    private void State_Think_Stroll(int distance, int tryTime = 0)
    {
        if (distance < 1 || tryTime > 3)
        {
            /*闲逛失败*/
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
                /*无法抵达*/
                State_Think_Stroll(distance - 1, tryTime + 1);
            }
        }
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
                dialogOption_0.optionEntry = "Bartender_Dialog0_Option0";
                dialogOption_0.optionAction = (() =>
                {
                    Local_OverDialog(player);
                });
                dialogOption_1.optionTable = "Role_String";
                dialogOption_1.optionEntry = "Bartender_Dialog0_Option1";
                dialogOption_1.optionAction = (() =>
                {
                    Local_StartDeal(player);
                });
                dialogOptions.Add(dialogOption_0);
                dialogOptions.Add(dialogOption_1);
                tileUI_Dialog.InitDialog("Role_String", "Bartender_Name", "Role_String", "Bartender_Dialog0");
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
        ItemConfig itemConfig = ItemConfigData.GetItemConfig(itemData.Item_ID);
        int offer = itemConfig.Item_Value * itemData.Item_Count / 2 + 1;
        return offer;
    }
    #endregion
}
