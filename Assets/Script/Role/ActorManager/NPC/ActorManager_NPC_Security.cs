using Fusion;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Rendering;
using static GameEvent;

public class ActorManager_NPC_Security : ActorManager_NPC
{
    [SerializeField, Header("攻击间隔"), Range(1, 10)]
    public float float_AttackCD;
    private float float_AttackTimer;
    [SerializeField, Header("思考间隔"), Range(1, 10)]
    public float float_ThinkCD;
    private float float_ThinkTimer;
    [SerializeField, Header("搜查时长"), Range(1, 10)]
    private float float_SearchTime;

    private Vector3Int vector3_StationPos;
    private Vector3Int vector3_AttackPos;
    private GlobalTime globalTime_Now;
    /// <summary>
    /// 睡眠中
    /// </summary>
    private bool bool_Sleeping = false;
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
            if(float_AttackTimer < 0)
            {
                float_AttackTimer = float_AttackCD;
                if (State_CheckingAttackingDistance())
                {
                    State_Elude();
                    if (brainManager.allClient_actorManager_AttackTarget.actorState != ActorState.Dead)
                    {
                        actorNetManager.RPC_State_NpcChangeAttackState(true);
                    }
                    else
                    {
                        actorNetManager.RPC_State_NpcChangeAttackTarget(new NetworkId());
                    }
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
            if(float_ThinkTimer < 0)
            {
                float_ThinkTimer = float_ThinkCD;
                State_DoWhat();
            }
        }
        base.State_FixedUpdateNetwork(dt);
    }
    public override void AllClient_Listen_UpdateTime(int hour, int date, GlobalTime globalTime)
    {
        globalTime_Now = globalTime; 
        base.AllClient_Listen_UpdateTime(hour, date, globalTime);
    }
    public override void State_Listen_RoleInView(ActorManager actor)
    {
        if (actor.statusManager.statusType != StatusType.Animal_Common)
        {
            brainManager.actorManagers_Nearby.Add(actor);
        }
        base.State_Listen_RoleInView(actor);
    }
    public override void State_Listen_RoleOutView(ActorManager actor)
    {
        if (actor.statusManager.statusType != StatusType.Animal_Common)
        {
            brainManager.actorManagers_Nearby.Remove(actor);
        }
        base.State_Listen_RoleInView(actor);
    }
    public override void State_CustomUpdate()
    {
        if (!bool_Sleeping)
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
        AllClient_IdleLoop();
        base.AllClient_SecondUpdate();
    }
    /// <summary>
    /// 检查威胁
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
                vector3_AttackPos = brainManager.allClient_actorManager_AttackTarget.pathManager.vector3Int_CurPos;
            }
        }
        else
        {
            for (int i = 0; i < brainManager.actorManagers_Nearby.Count; i++)
            {
                if (actionManager.LookAt(brainManager.actorManagers_Nearby[i], config.short_View))
                {
                    if (brainManager.actorManagers_Nearby[i].actorNetManager.Local_Fine > 0)
                    {

                        State_SendText(word_FindTheCulprit, Emoji.Menace);
                        State_InAttack(brainManager.actorManagers_Nearby[i]);
                        return;
                    }
                    if (brainManager.actorManagers_Nearby[i].statusManager.statusType == StatusType.Monster_Common)
                    {
                        State_SendText(word_FindTheMonster, Emoji.Menace);
                        State_InAttack(brainManager.actorManagers_Nearby[i]);
                        return;
                    }
                }
            }
            
        }
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
                        vector3_AttackPos = actor.pathManager.vector3Int_CurPos;
                        State_SendText(word_Notice, Emoji.Puzzled); 
                        State_Search();
                        pathManager.State_MoveTo(actor.pathManager.vector3Int_CurPos);
                    }
                    break;
            }
        }
        base.State_Listen_RoleSendEmoji(actor, emoji, distance);
    }
    private void State_InAttack(ActorManager actor)
    {
        Debug.Log("开始攻击");
        actorNetManager.RPC_State_NpcChangeAttackTarget(actor.actorNetManager.Object.Id);
        State_PutOnHand((itemConfig) =>
        {
            if (itemConfig.Item_Type == ItemType.Weapon) return true;
            return false;
        });
    }
    private void State_OutAttack()
    {
        Debug.Log("停止攻击");
        actorNetManager.RPC_State_NpcChangeAttackTarget(new NetworkId());
        State_PutDownHand();
    }
    /// <summary>
    /// 绑定起始位置
    /// </summary>
    /// <param name="myTile"></param>
    public void State_BindStation(Vector3Int pos)
    {
        vector3_StationPos = pos;
    }

    #region//战斗状态
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
    private void State_Follow(Vector3Int vector3)
    {
        pathManager.State_MoveTo(vector3);
    }
    /// <summary>
    /// 搜寻
    /// </summary>
    /// <param name="vector2"></param>
    private void State_Search()
    {
        if (vector3_AttackPos != Vector3Int.zero)
        {
            float_ThinkTimer = float_SearchTime;
            pathManager.State_MoveTo(vector3_AttackPos);
            vector3_AttackPos = Vector3Int.zero;
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
    private void State_DoWhat()
    {
        if (Vector3.Distance(pathManager.vector3Int_CurPos, vector3_StationPos) > 2)
        {
            State_GoToWork();
        }
        else
        {
            if (globalTime_Now == GlobalTime.Evening)
            {

            }
            else
            {
                
            }
        }
    }
    /// <summary>
    /// 去工作
    /// </summary>
    private void State_GoToWork()
    {
        if (pathManager.State_MoveTo(vector3_StationPos))
        {
            State_SendText(word_BackToWork, Emoji.Unhappy);
        }
        else
        {
            State_SendText(word_Lost, Emoji.Puzzled);
        }
    }
    public void AllClient_IdleLoop()
    {
        if (bool_Sleeping)
        {
            if (brainManager.allClient_actorManager_AttackTarget || bodyController.speed > 0 || globalTime_Now != GlobalTime.Evening)
            {
                Sleeping(false);
            }
        }
        else
        {
            if (!brainManager.allClient_actorManager_AttackTarget && bodyController.speed == 0 && globalTime_Now == GlobalTime.Evening)
            {
                Sleeping(true);
            }
        }
    }
    public void Sleeping(bool sleep)
    {
        bool_Sleeping = sleep;
        bodyController.SetAnimatorBool(BodyPart.Head, "Sleep", sleep);
    }
    #endregion
    #region//语言
    /// <summary>
    /// 发现犯罪
    /// </summary>
    private List<string> word_FindTheCulprit = new List<string>()
    {
        "嘿,你等一下","你这罪犯","站住","别动!"
    };
    /// <summary>
    /// 回去工作
    /// </summary>
    private List<string> word_BackToWork = new List<string>()
    {
        "浪费时间","但愿别再出乱子了","别再来了","这附近不安全","回去吧"
    };
    /// <summary>
    /// 迷路
    /// </summary>
    private List<string> word_Lost = new List<string>()
    {
        "什么鬼地方?","这是哪？"
    };
    /// <summary>
    /// 
    /// </summary>
    /// <param name="strings"></param>
    /// <param name="emoji"></param>
    private void State_SendText(List<string> strings,Emoji emoji)
    {
        int random = new System.Random().Next(0,strings.Count);
        State_TryToSendText(strings[random],emoji);
    }
    #endregion
}
