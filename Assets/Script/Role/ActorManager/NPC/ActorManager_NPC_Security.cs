using Fusion;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Rendering;
using static GameEvent;

public class ActorManager_NPC_Security : ActorManager_NPC
{
    [SerializeField, Header("�������"), Range(1, 10)]
    public float float_AttackCD;
    private float float_AttackTimer;
    [SerializeField, Header("˼�����"), Range(1, 10)]
    public float float_ThinkCD;
    private float float_ThinkTimer;
    [SerializeField, Header("�Ѳ�ʱ��"), Range(1, 10)]
    private float float_SearchTime;

    private Vector2 vector2_StationPos;
    private Vector2 vector2_AttackPos;
    private GlobalTime globalTime_Now;
    /// <summary>
    /// ˯����
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
        MessageBroker.Default.Receive<GameEvent_AllClient_UpdateTime>().Subscribe(_ =>
        {
            AllClient_Listen_UpdateTime(_.hour, _.date, _.now);
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
    /// �����в
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
                vector2_AttackPos = brainManager.allClient_actorManager_AttackTarget.transform.position;
            }
        }
        else
        {
            for (int i = 0; i < brainManager.actorManagers_Nearby.Count; i++)
            {
                if (actionManager.LookAt(brainManager.actorManagers_Nearby[i], config.short_View))
                {
                    if (brainManager.actorManagers_Nearby[i].actorNetManager.Net_Fine > 0)
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
                    if (actorNetManager.Net_HpCur * 2 <= actorNetManager.Net_HpMax)
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
                        vector2_AttackPos = actor.transform.position;
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
        Debug.Log("��ʼ����");
        actorNetManager.RPC_State_NpcChangeAttackTarget(actor.actorNetManager.Object.Id);
        State_PutOnHand((itemConfig) =>
        {
            if (itemConfig.Item_Type == ItemType.Weapon) return true;
            return false;
        });
    }
    private void State_OutAttack()
    {
        Debug.Log("ֹͣ����");
        actorNetManager.RPC_State_NpcChangeAttackTarget(new NetworkId());
        State_PutDownHand();
    }
    /// <summary>
    /// ����ʼλ��
    /// </summary>
    /// <param name="myTile"></param>
    public void State_BindStation(Vector2 pos)
    {
        vector2_StationPos = pos;
    }

    #region//ս��״̬
    /// <summary>
    /// ��鹥������
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
    /// ���
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
    /// ׷��
    /// </summary>
    /// <param name="vector2"></param>
    private void State_Follow(Vector3Int vector3)
    {
        pathManager.State_MoveTo(vector3);
    }
    /// <summary>
    /// ��Ѱ
    /// </summary>
    /// <param name="vector2"></param>
    private void State_Search()
    {
        if (vector2_AttackPos != Vector2.zero)
        {
            float_ThinkTimer = float_SearchTime;
            pathManager.State_MoveTo(vector2_AttackPos);
            vector2_AttackPos = Vector2.zero;
        }
    }
    /// <summary>
    /// ����ѭ��
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
    }

    #endregion
    #region//����״̬
    /// <summary>
    /// ˼������ʲô
    /// </summary>
    private void State_DoWhat()
    {
        if (Vector2.Distance(transform.position, vector2_StationPos) > 2)
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
                State_SendText(word_Soliloquize, Emoji.Unhappy);
            }
        }
    }
    /// <summary>
    /// ȥ����
    /// </summary>
    private void State_GoToWork()
    {
        if (pathManager.State_MoveTo(vector2_StationPos))
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
    #region//����
    /// <summary>
    /// �ֻ�
    /// </summary>
    private List<string> word_Panic = new List<string>()
    {
        "��!����Ҫ֧Ԯ!","������Ѫ!","��������","����!"
    };
    /// <summary>
    /// ��в
    /// </summary>
    private List<string> word_Menace = new List<string>()
    {
        "���군��","�����ҵ�","ʲô?","����Ϊ�Һ�����"
    };
    /// <summary>
    /// ���ַ���
    /// </summary>
    private List<string> word_FindTheCulprit = new List<string>()
    {
        "��,���һ��","�����ﷸ","վס","��!"
    };
    /// <summary>
    /// ���ֹ���
    /// </summary>
    private List<string> word_FindTheMonster = new List<string>()
    {
        "��ô���Ҹ�������","��!���������","��ù","���˰�!"
    };
    /// <summary>
    /// ׷��
    /// </summary>
    private List<string> word_Pursue = new List<string>()
    {
        "������ץס��","����Ϊ�ܵõ���","����!","�һ�������"
    };
    /// <summary>
    /// ע��
    /// </summary>
    private List<string> word_Notice = new List<string>()
    {
        "��?","ʲô����?","˭������?","��?"
    };
    /// <summary>
    /// ��ȥ����
    /// </summary>
    private List<string> word_BackToWork = new List<string>()
    {
        "�˷�ʱ��","��Ը���ٳ�������","��������","�⸽������ȫ","��ȥ��"
    };
    /// <summary>
    /// ��������
    /// </summary>
    private List<string> word_Soliloquize = new List<string>()
    {
        "������...","��ô������...��˵","...Ŷ,����...","����һ��",
    };
    /// <summary>
    /// ��·
    /// </summary>
    private List<string> word_Lost = new List<string>()
    {
        "ʲô��ط�?","�����ģ�"
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
