using Fusion;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class AcrorManager_Zombie_Runner : ActorManager
{
    [Header("��ʬ����")]
    public ActorConfig_ZombieRunner config;
    [Header("�������"), Range(1, 10)]
    public float float_AttackCD;
    private float float_AttackTimer;
    private float float_FollowCD = 1f;
    private float float_FollowTimer;
    private float float_ThinkCD = 1f;
    private float float_ThinkTimer;
    private float float_SearchTime = 4f;
    [Header("��ʬǹ��")]
    public Transform trans_Muzzle;
    #region//��ʼ��
    public override void AllClient_Init()
    {
        statusManager.statusType = config.status_Type;
        base.AllClient_Init();
    }
    public override void State_Init()
    {
        float_ThinkCD += new System.Random().Next(0, 100) * 0.01f;
        float_ThinkTimer = float_ThinkCD;
        float_FollowCD += new System.Random().Next(0, 100) * 0.01f;
        float_FollowTimer = float_FollowCD;
        State_InitNPCData();
        base.State_Init();
    }
    /// <summary>
    /// ��ʼ������
    /// </summary>
    private void State_InitNPCData()
    {
        List<ItemData> itemDatas = new List<ItemData>();
        for (int i = 0; i < config.lootInfos_Base.Count; i++)
        {
            int count = new System.Random().Next(config.lootInfos_Base[i].CountMin, config.lootInfos_Base[i].CountMax + 1);
            ItemData item = itemManager.CreateItemData(config.lootInfos_Base[i].ID, (short)count);
            itemDatas.Add(item);
        }
        for (int i = 0; i < config.lootInfos_Extra.Count; i++)
        {
            int random = new System.Random().Next(0, 1000);
            if (random <= config.lootInfos_Extra[i].Weight)
            {
                ItemData item = itemManager.CreateItemData(config.lootInfos_Extra[i].ID, (short)config.lootInfos_Extra[i].Count);
                itemDatas.Add(item);
            }
        }
        State_SetAbilityData(config.short_Hp, config.short_Armor, config.short_Resistance, config.short_MoveSpeed);
        actorNetManager.Local_SetLootItems(itemDatas);
    }
    public override void AllClient_AddListener()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_All_UpdateHour>().Subscribe(_ =>
        {
            AllClient_Listen_UpdateTime(_.hour, _.day, _.now);
        }).AddTo(this);
        base.AllClient_AddListener();
    }
    #endregion
    #region//ʱ������
    public override void State_FixedUpdateNetwork(float dt)
    {
        if (brainManager.allClient_actorManager_AttackTarget)
        {
            float_AttackTimer -= dt;
            float_FollowTimer -= dt;
            if (float_AttackTimer < 0)
            {
                if (State_CheckingHackDistance())
                {
                    /*ִ������*/
                    pathManager.State_StandDown(1);
                    float_AttackTimer = float_AttackCD;
                    actorNetManager.RPC_State_NpcUseSkill((int)Skill.Hack, brainManager.allClient_actorManager_AttackTarget.pathManager.vector3Int_CurPos, brainManager.allClient_actorManager_AttackTarget.actorNetManager.Object.Id);
                }
                else if (State_CheckingJumpDistance())
                {
                    /*����֮һ����ִ����Ծ*/
                    if (new System.Random().Next(0, 10) > 4)
                    {
                        pathManager.State_StandDown(2f);
                        actorNetManager.RPC_State_NpcUseSkill((int)Skill.Jump, brainManager.allClient_actorManager_AttackTarget.pathManager.vector3Int_CurPos, brainManager.allClient_actorManager_AttackTarget.actorNetManager.Object.Id);
                        float_AttackTimer = float_AttackCD;
                    }
                    else
                    {
                        float_AttackTimer = float_AttackCD;
                    }
                }
            }
            if (float_FollowTimer < 0)
            {
                float_FollowTimer = float_FollowCD;
                if (!State_CheckingHackDistance())
                {
                    pathManager.State_MovePostion(brainManager.allClient_actorManager_AttackTarget.pathManager.vector3Int_CurPos);
                }
            }
        }
        else
        {
            float_ThinkTimer -= dt;
            if (float_ThinkTimer < 0)
            {
                float_ThinkTimer = float_ThinkCD;
                State_Think();
            }
        }

        base.State_FixedUpdateNetwork(dt);
    }
    public override void State_CustomUpdate()
    {
        State_CheckNearby();
        base.State_CustomUpdate();
    }
    #endregion
    #region//�����ķ�Ӧ
    public override void State_Listen_MyselfHpChange(int parameter, HpChangeReason reason, NetworkId id)
    {
        NetworkObject networkObject = actorNetManager.Runner.FindObject(id);
        if (networkObject != null)
        {
            ActorManager who = networkObject.GetComponent<ActorManager>();
            if (who.actorAuthority.isPlayer && actionManager.LookAt(who, config.float_ViewDistance))
            {
                if (brainManager.allClient_actorManager_AttackTarget == null)
                {
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
    public override void AllClient_Listen_UpdateTime(int hour, int date, GlobalTime globalTime)
    {
        brainManager.SetTime(globalTime);
        base.AllClient_Listen_UpdateTime(hour, date, globalTime);
    }
    public override void State_Listen_RoleInView(ActorManager actor)
    {
        if (actor.statusManager.statusType != StatusType.Monster_Common)
        {
            brainManager.actorManagers_Nearby.Add(actor);
        }
        base.State_Listen_RoleInView(actor);
    }
    public override void State_Listen_RoleOutView(ActorManager actor)
    {
        if (actor.statusManager.statusType != StatusType.Monster_Common)
        {
            brainManager.actorManagers_Nearby.Remove(actor);
        }
        base.State_Listen_RoleInView(actor);
    }
    /// <summary>
    /// ��鸽��
    /// </summary>
    public void State_CheckNearby()
    {
        if (brainManager.allClient_actorManager_AttackTarget != null)
        {
            if (!actionManager.LookAt(brainManager.allClient_actorManager_AttackTarget, config.float_ViewDistance))
            {
                State_Search();
                State_OutAttack();
            }
            else
            {
                brainManager.SetSearch(brainManager.allClient_actorManager_AttackTarget.pathManager.vector3Int_CurPos);
            }
        }
        else
        {
            if (brainManager.allClient_actorManager_AttackTargetID != new Fusion.NetworkId())
            {
                State_Search();
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
        float distance_Min = float.MaxValue;
        float distance_New = 0;
        ActorManager actorManager_Attack = null;
        brainManager.actorManagers_Nearby.RemoveAll((x) => { return x == null; });
        for (int i = 0; i < brainManager.actorManagers_Nearby.Count; i++)
        {
            if (actionManager.LookAt(brainManager.actorManagers_Nearby[i], config.float_ViewDistance))
            {
                distance_New = Vector3.Distance(transform.position, (brainManager.actorManagers_Nearby[i].transform.position));
                if (distance_Min > distance_New)
                {
                    distance_Min = distance_New;
                    actorManager_Attack = brainManager.actorManagers_Nearby[i];
                }
            }
        }
        if (actorManager_Attack != null)
        {
            State_InAttack(actorManager_Attack);
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion
    #region//����״̬
    /// <summary>
    /// ˼��
    /// </summary>
    public void State_Think()
    {
        if (brainManager.globalTime_Now == GlobalTime.Evening)
        {
            State_Think_Stroll(config.short_MoveStep);
        }
        else
        {
            if (brainManager.homePostion.isValue)
            {
                State_Think_GoHome();
            }
            else
            {
                State_Think_Stroll(config.short_MoveStep);
            }
        }
    }
    /// <summary>
    /// �й�
    /// </summary>
    private void State_Think_Stroll(int distance, int tryTime = 0)
    {
        if (distance < 1 || tryTime > 3)
        {
            /*�й�ʧ��*/
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
    /// <summary>
    /// �ؼ�
    /// </summary>
    public void State_Think_GoHome()
    {
        if (Vector3.Distance(pathManager.vector3Int_CurPos, brainManager.homePostion.postion) < 0.5f)
        {
            actionManager.Despawn();
        }
        else
        {
            pathManager.State_MovePostion(brainManager.homePostion.postion);
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
    /// ��鹥������
    /// </summary>
    /// <returns></returns>
    public bool State_CheckingHackDistance()
    {
        if (Vector3.Distance(brainManager.allClient_actorManager_AttackTarget.transform.position, transform.position) < config.float_HackDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// ��鼼�ܾ���
    /// </summary>
    /// <returns></returns>
    public bool State_CheckingJumpDistance()
    {
        float distance = Vector3.Distance(brainManager.allClient_actorManager_AttackTarget.transform.position, transform.position);
        if (distance < config.float_MaxJumpDistance && distance > config.float_MinJumpDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
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
    #region//����
    private enum Skill
    {
        Hack,
        Jump
    }
    public override void AllClient_Listen_NpcAction(int id, Vector3Int vector3, NetworkId networkId)
    {
        if (id == (int)Skill.Hack)
        {
            AllClient_Hack(vector3, networkId);
        }
        if (id == (int)Skill.Jump)
        {
            AllClient_Jump(vector3, networkId);
        }
        base.AllClient_Listen_NpcAction(id, vector3, networkId);
    }
    private void AllClient_Hack(Vector3Int vector3, NetworkId networkId)
    {
        actionManager.TurnTo(actorNetManager.Runner.FindObject(networkId).transform.position - transform.position);
        bodyController.SetAnimatorTrigger(BodyPart.Body, "Hack");
        if (actorAuthority.isState)
        {
            bodyController.SetAnimatorFunc(BodyPart.Body, (str) =>
            {
                if (str.Equals("Hack_0"))
                {
                    RaycastHit2D[] raycastHit2Ds = Physics2D.CircleCastAll(transform.position, 1.5f, Vector2.zero);
                    foreach (RaycastHit2D hit2D in raycastHit2Ds)
                    {
                        if (hit2D.collider.isTrigger && hit2D.collider.gameObject.TryGetComponent(out ActorManager actorManager))
                        {
                            if (actorManager.statusManager.statusType != StatusType.Monster_Common)
                            {
                                actorManager.AllClient_Listen_TakeDamage(config.int_HackDamage, DamageState.AttackSlashingDamage, actorNetManager);
                            }
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            });
            bodyController.SetAnimatorFunc(BodyPart.Body, (str) =>
            {
                if (str.Equals("Hack_1"))
                {
                    RaycastHit2D[] raycastHit2Ds = Physics2D.CircleCastAll(transform.position, 1.5f, Vector2.zero);
                    foreach (RaycastHit2D hit2D in raycastHit2Ds)
                    {
                        if (hit2D.collider.isTrigger && hit2D.collider.gameObject.TryGetComponent(out ActorManager actorManager))
                        {
                            if (actorManager.statusManager.statusType != StatusType.Monster_Common)
                            {
                                actorManager.AllClient_Listen_TakeDamage(config.int_HackDamage, DamageState.AttackSlashingDamage, actorNetManager);
                            }
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            });
        }
    }
    private void AllClient_Jump(Vector3Int vector3, NetworkId networkId)
    {
        if (actorNetManager.Runner.FindObject(networkId) != null)
        {
            Vector2 dir = actorNetManager.Runner.FindObject(networkId).transform.position - transform.position;
            actionManager.TurnTo(dir);
            bodyController.SetAnimatorTrigger(BodyPart.Body, "Jump");
            if (actorAuthority.isState)
            {
                bodyController.SetAnimatorFunc(BodyPart.Body, (str) =>
                {
                    if (str.Equals("StartJump"))
                    {
                        actionManager.AddForce(dir.normalized, 40);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                });
                bodyController.SetAnimatorFunc(BodyPart.Body, (str) =>
                {
                    if (str.Equals("OverJump"))
                    {
                        RaycastHit2D[] raycastHit2Ds = Physics2D.CircleCastAll(transform.position, 1.5f, Vector2.zero);
                        foreach (RaycastHit2D hit2D in raycastHit2Ds)
                        {
                            if (hit2D.collider.isTrigger && hit2D.collider.gameObject.TryGetComponent(out ActorManager actorManager))
                            {
                                if (actorManager.statusManager.statusType != StatusType.Monster_Common)
                                {
                                    actorManager.AllClient_Listen_TakeDamage(config.int_JumpDamage, DamageState.AttackSlashingDamage, actorNetManager);
                                }
                            }
                        }

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                });
            }
        }
    }
    #endregion
}
[Serializable]
public struct ActorConfig_ZombieRunner
{
    [Header("��ʼ���")]
    public StatusType status_Type;
    [Header("����")]
    public short short_Hp;
    [Header("����")]
    public short short_Armor;
    [Header("ħ��")]
    public short short_Resistance;
    [Header("�ƶ��ٶ�")]
    public short short_MoveSpeed;
    [Header("�ƶ�����"), Range(1, 10)]
    public short short_MoveStep;
    [Header("������������"), Range(1, 5)]
    public float float_HackDistance;
    [Header("׼����Ծ����С����"), Range(0, 25)]
    public float float_MinJumpDistance;
    [Header("׼����Ծ��������"), Range(0, 25)]
    public float float_MaxJumpDistance;
    [Header("��Ծ�˺�")]
    public int int_JumpDamage;
    [Header("�����˺�")]
    public int int_HackDamage;
    [Header("��Ұ����"), Range(1, 99)]
    public float float_ViewDistance;
    [Header("���������б�")]
    public List<BaseLootInfo> lootInfos_Base;
    [Header("��������б�")]
    public List<ExtraLootInfo> lootInfos_Extra;
}
