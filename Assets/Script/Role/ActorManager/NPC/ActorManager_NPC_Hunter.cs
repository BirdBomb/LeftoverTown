using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using static GameEvent;

public class ActorManager_NPC_Hunter : ActorManager_NPC
{
    [SerializeField, Header("攻击间隔"), Range(1, 10)]
    public float float_AttackCD;
    private float float_AttackTimer;
    [SerializeField, Header("思考间隔"), Range(1, 10)]
    public float float_ThinkCD;
    private float float_ThinkTimer;
    [SerializeField, Header("搜查时长"), Range(1, 10)]
    private float float_SearchTime;
    /// <summary>
    /// 出生点
    /// </summary>
    private Vector3Int vector3_HomePos;
    /// <summary>
    /// 搜查点
    /// </summary>
    private Vector3Int vector3_SearchPos;
    /// <summary>
    /// 当前时间
    /// </summary>
    private GlobalTime globalTime_Now;
    /// <summary>
    /// 睡眠中
    /// </summary>
    private bool bool_Working = false;
    public override void FixedUpdate()
    {
        AllClient_AttackLoop(Time.fixedDeltaTime);
        base.FixedUpdate();
    }
    public override void AllClient_AddListener()
    {
        MessageBroker.Default.Receive<GameEvent_AllClient_SomeoneSendEmoji>().Subscribe(_ =>
        {
            AllClient_Listen_RoleSendEmoji(_.actor, _.emoji, _.distance);
            if (actorAuthority.isState) State_Listen_RoleSendEmoji(_.actor, _.emoji, _.distance);

        }).AddTo(this);
        MessageBroker.Default.Receive<GameEvent_All_UpdateHour>().Subscribe(_ =>
        {
            AllClient_Listen_UpdateTime(_.hour, _.day, _.now);
        }).AddTo(this);
        base.AllClient_AddListener();
    }
    public override void State_FixedUpdateNetwork(float dt)
    {
        if (brainManager.allClient_actorManager_AttackTarget)
        {
            float_AttackTimer -= dt;
            if (float_AttackTimer < 0)
            {
                float_AttackTimer = float_AttackCD;
                if (State_CheckingAttackingDistance())
                {
                    State_Elude();
                    actorNetManager.RPC_State_NpcChangeAttackState(true);
                }
                else
                {
                    State_Follow(brainManager.allClient_actorManager_AttackTarget.pathManager.vector3Int_CurPos);
                }
            }
        }
        else
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
        if (!bool_Working)
        {
            State_CheckNearby();
        }
        base.State_CustomUpdate();
    }
    public override void State_SecondUpdate()
    {
        base.State_SecondUpdate();
    }
    public override void AllClient_SecondUpdate()
    {
        AllClient_StateLoop();
        AllClient_CheckState();
        base.AllClient_SecondUpdate();
    }
    #region//初始化
    /// <summary>
    /// 绑定起始位置
    /// </summary>
    /// <param name="myTile"></param>
    public void State_BindChoppingBoard(Vector3Int pos)
    {
        vector3_HomePos = pos;
    }
    #endregion
    #region//对外界的反应
    public override void AllClient_Listen_UpdateTime(int hour, int date, GlobalTime globalTime)
    {
        globalTime_Now = globalTime;
        base.AllClient_Listen_UpdateTime(hour, date, globalTime);
    }
    public override void State_Listen_WorldGlobalTimeChange(int hour, int date, GlobalTime globalTime)
    {
        if (globalTime == GlobalTime.Morning)
        {
            State_ResetBag();
        }
        base.State_Listen_WorldGlobalTimeChange(hour, date, globalTime);
    }
    public override void State_Listen_MyselfHpChange(int parameter, NetworkId id)
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
                    State_SendText(word_Menace, Emoji.Menace);
                    State_InAttack(who);
                }
                else
                {
                    if (actorNetManager.Net_HpCur * 2 <= actorNetManager.Local_HpMax)
                        State_SendText(word_Panic, Emoji.Panic);
                }
            }
        }
        base.State_Listen_MyselfHpChange(parameter, id);
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
                        vector3_SearchPos = actor.pathManager.vector3Int_CurPos;
                        State_SendText(word_Notice, Emoji.Puzzled);
                        State_Search();
                        pathManager.State_MoveTo(actor.pathManager.vector3Int_CurPos);
                        State_TryToSendEmoji(0.5f, 0);
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
            if (!actionManager.LookAt(brainManager.allClient_actorManager_AttackTarget, config.short_View))
            {
                State_SendText(word_Pursue, Emoji.Menace);
                State_Search();
                State_OutAttack();
            }
            else
            {
                vector3_SearchPos = brainManager.allClient_actorManager_AttackTarget.pathManager.vector3Int_CurPos;
            }
        }
        else
        {
            if (brainManager.allClient_actorManager_AttackTargetID != new Fusion.NetworkId())
            {
                State_Search();
                State_OutAttack();
            }
            //State_CheckNearbyItem();
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
                    State_SendText(word_FindTheMonster, Emoji.Menace);
                    State_InAttack(brainManager.actorManagers_Nearby[i]);
                    return true;
                }
                if (brainManager.actorManagers_Nearby[i].statusManager.statusType == StatusType.Animal_Common)
                {
                    State_SendText(word_FindTheAnimal, Emoji.Menace);
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
            if (pathManager.State_MoveTo(targetPos))
            {
                State_SendText(word_PickUp, Emoji.Happy);
            }
            actionManager.PickUp(1.5f);
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
        State_PutOnHand((itemConfig) =>
        {
            if (itemConfig.Item_Type == ItemType.Weapon) return true;
            return false;
        });
    }
    /// <summary>
    /// 离开攻击状态
    /// </summary>
    private void State_OutAttack()
    {
        actorNetManager.RPC_State_NpcChangeAttackTarget(new NetworkId());
        State_PutDownHand();
    }
    /// <summary>
    /// 检查攻击距离
    /// </summary>
    /// <returns></returns>
    public bool State_CheckingAttackingDistance()
    {
        if (itemManager.itemBase_OnHand.itemData.Item_ID != 0)
        {
            if (Vector3.Distance(brainManager.allClient_actorManager_AttackTarget.transform.position, transform.position) < itemManager.itemBase_OnHand.itemConfig.Attack_Distance + 0.5f)
            {
                return true;
            }
        }
        else
        {
            if (Vector3.Distance(brainManager.allClient_actorManager_AttackTarget.transform.position, transform.position) < 1)
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// 躲避
    /// </summary>
    private void State_Elude()
    {
        Vector3Int vector3 = Vector3Int.zero;
        int random = new System.Random().Next(1, 4);
        if (random == 1) vector3 = Vector3Int.down;
        if (random == 2) vector3 = Vector3Int.up;
        if (random == 3) vector3 = Vector3Int.right;
        if (random == 4) vector3 = Vector3Int.left;
        Vector3Int pos_R = pathManager.vector3Int_CurPos + vector3;
        pathManager.State_MoveTo(pos_R);
    }
    /// <summary>
    /// 追击
    /// </summary>
    /// <param name="vector2"></param>
    private void State_Follow(Vector3Int to)
    {
        pathManager.State_MoveTo(to);
    }
    /// <summary>
    /// 搜寻
    /// </summary>
    /// <param name="vector2"></param>
    private void State_Search()
    {
        if (vector3_SearchPos != Vector3Int.zero)
        {
            float_ThinkTimer = float_SearchTime;
            pathManager.State_MoveTo(vector3_SearchPos);
            vector3_SearchPos = Vector3Int.zero;
        }
    }
    /// <summary>
    /// 攻击循环
    /// </summary>
    public void AllClient_AttackLoop(float dt)
    {
        if (brainManager.allClient_actorManager_AttackTarget)
        {
            inputManager.Simulate_InputMousePos(brainManager.allClient_actorManager_AttackTarget.transform.position);
            if (brainManager.bool_AttackState)
            {
                brainManager.bool_AttackState = !inputManager.Simulate_InputMousePress(dt, ActorInputManager.MouseInputType.PressRightThenPressLeft);
            }
        }
        else
        {
            actionManager.FaceTo(bodyController.turnDir);
        }
    }
    #endregion
    #region//闲置状态
    /// <summary>
    /// 思考该做什么
    /// </summary>
    private void State_Think()
    {
        if (globalTime_Now == GlobalTime.Evening)
        {
            if(vector3_HomePos != pathManager.vector3Int_CurPos) State_Think_GoToWork();
        }
        else
        {
            State_Think_Stroll();
        }
    }
    /// <summary>
    /// 工作
    /// </summary>
    private void State_Think_GoToWork()
    {
        pathManager.State_MoveTo(vector3_HomePos);
    }
    /// <summary>
    /// 闲逛
    /// </summary>
    private void State_Think_Stroll()
    {
        int x = new System.Random().Next(-1, 1);
        int y = new System.Random().Next(-1, 1);
        Vector3Int dir = new Vector3Int(x, y, 0);
        Vector3Int pos_F = pathManager.vector3Int_CurPos + dir * 3;
        Vector3Int pos_R = pathManager.vector3Int_CurPos - dir * 3;
        if (!pathManager.State_MoveTo(pos_F)) pathManager.State_MoveTo(pos_R);
    }
    #endregion
    #region//客户端状态循环
    private enum HunterState
    {
        Default, Working
    }
    private HunterState hunterState;
    /// <summary>
    /// 状态循环
    /// </summary>
    public void AllClient_StateLoop()
    {
        bool bool_InEvening = (globalTime_Now == GlobalTime.Evening);
        bool bool_InAttack = (brainManager.allClient_actorManager_AttackTarget);
        bool bool_InMove = (bodyController.speed != 0);
        switch (hunterState)
        {
            case HunterState.Default:
                {
                    if (bool_InEvening && !bool_InMove && !bool_InAttack)
                    {
                        if (MapManager.Instance.GetBuilding(pathManager.vector3Int_CurPos, out BuildingTile buildingTile))
                        {
                            if(buildingTile.tileID == 8301)
                            {
                                hunterState = HunterState.Working;
                            }
                        }
                    }
                    break;
                }
            case HunterState.Working:
                {
                    if(!bool_InEvening || bool_InMove || bool_InAttack)
                    {
                        hunterState = HunterState.Default;
                    }
                    break;
                }
        }
    }
    /// <summary>
    /// 状态检查
    /// </summary>
    private void AllClient_CheckState()
    {
        switch (hunterState)
        {
            case HunterState.Default:
                {
                    bodyController.SetAnimatorBool(BodyPart.Hand, "Work", false);
                    break;
                }
            case HunterState.Working:
                {
                    bodyController.SetAnimatorBool(BodyPart.Hand, "Work", true);
                    break;
                }
        }

    }

    #endregion
    #region//语言
    /// <summary>
    /// 发现猎物
    /// </summary>
    private List<string> word_FindTheAnimal = new List<string>()
    {
        "这可是上好的食物","狩猎时间..",
    };
    /// <summary>
    /// 说话
    /// </summary>
    /// <param name="strings"></param>
    /// <param name="emoji"></param>
    private void State_SendText(List<string> strings, Emoji emoji)
    {
        int random = new System.Random().Next(0, strings.Count);
        State_TryToSendText(strings[random], emoji);
    }
    #endregion
    #region//交流
    public override bool Local_Communication()
    {
        return true;
    }
    public override bool Local_Deal(out ActorManager dealActor, out List<ItemData> dealGoods, out Func<ItemData, int> dealOffer)
    {
        dealActor = this;
        dealGoods = null;
        dealOffer = Local_Offer;
        if (globalTime_Now != GlobalTime.Evening)
        {
            Local_TryToSendText("交易?晚上再说吧", Emoji.Unhappy);
        }
        else
        {
            if (!brainManager.bool_AttackState)
            {
                dealGoods = new List<ItemData>();
                List<ItemData> items = actorNetManager.Local_GetBagItem();
                for (int i = 0; i < items.Count; i++)
                {
                    ItemConfig itemConfig = ItemConfigData.GetItemConfig(items[i].Item_ID);
                    if (itemConfig.Item_Type != ItemType.Weapon)
                    {
                        dealGoods.Add(items[i]);
                    }
                }
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// 报价
    /// </summary>
    /// <param name="itemData"></param>
    /// <returns></returns>
    public int Local_Offer(ItemData itemData)
    {
        int offer = 0;
        ItemConfig itemConfig = ItemConfigData.GetItemConfig(itemData.Item_ID);
        if (itemConfig.Item_Type == ItemType.Food || itemConfig.Item_Type == ItemType.Ingredient)
        {
            offer = itemConfig.Average_Value * itemData.Item_Count * 2;
        }
        return offer;
    }
    #endregion
}
