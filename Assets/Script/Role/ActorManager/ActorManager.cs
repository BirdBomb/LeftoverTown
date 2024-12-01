using DG.Tweening;
using Fusion;
using Fusion.Addons.Physics;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;
using static GameEvent;
using static Unity.Collections.Unicode;
/// <summary>
/// ��ɫ������
/// </summary>
public class ActorManager : MonoBehaviour
{
    [SerializeField, Header("�������")]
    private Rigidbody2D myRigidbody;
    public Rigidbody2D MyRigidbody
    {
        get { return myRigidbody; }
    }
    [SerializeField, Header("���������")]
    private ActorNetManager netManager;
    public ActorNetManager NetManager
    {
        get { return netManager; }
    }
    [SerializeField, Header("���������")]
    private BaseBodyController bodyController;
    public BaseBodyController BodyController
    {
        get { return bodyController; }
    }
    [SerializeField, Header("��Χָʾ��")]
    private SI_Sector skillSector;
    public SI_Sector SkillSector
    {
        get { return skillSector; }
    }
    [SerializeField, Header("UI������")]
    private ActorUI actorUI;
    public ActorUI ActorUI
    {
        get { return actorUI; }
    }
    [HideInInspector]
    public ActorState actorState;
    [HideInInspector]
    public bool isPlayer = false;
    [HideInInspector]
    public bool isState = false;
    [HideInInspector]
    public bool isInput = false;
    /// <summary>
    /// �㼶(ǽ)
    /// </summary>
    protected LayerMask layerMask_Wall;
    /*��ʼ��*/
    #region
    private void Awake()
    {
        layerMask_Wall = LayerMask.GetMask("TileObj_Wall");
        myRigidbody.gravityScale = 0;
    }
    /// <summary>
    /// ��Ϊ��������ʼ��
    /// </summary>
    /// <param name="hasStateAuthority"></param>
    /// <param name="hasInputAuthority"></param>
    public virtual void AllClient_InitByNetManager(bool hasStateAuthority, bool hasInputAuthority)
    {
        navManager = MapManager.Instance.navManager;
        isState = hasStateAuthority;
        isInput = hasInputAuthority;
        AllClient_StatrLoop();
        AllClient_AddListener();
    }
    /// <summary>
    /// ��Ϊ��ҳ�ʼ��
    /// </summary>
    public virtual void AllClient_InitByPlayer()
    {
        isPlayer = true;
    }
    #endregion
    /*����*/
    #region
    /// <summary>
    /// ��ʼ����
    /// </summary>
    private void AllClient_AddListener()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_Local_SomeoneMove>().Subscribe(_ =>
        {
            AllClientListen_RoleMove(_.moveActor, _.moveTile);
        }).AddTo(this);
        MessageBroker.Default.Receive<GameEvent.GameEvent_Local_SomeoneSendEmoji>().Subscribe(_ =>
        {
            AllClientListen_RoleSendEmoji(_.actor, _.id, _.distance);
        }).AddTo(this);
        MessageBroker.Default.Receive<GameEvent.GameEvent_Local_SomeoneDoSomething>().Subscribe(_ =>
        {
            AllClientListen_RoleDoSomething(_.actor, _.action);
        }).AddTo(this);
        MessageBroker.Default.Receive<GameEvent.GameEvent_Local_SomeoneCommit>().Subscribe(_ =>
        {
            AllClientListen_RoleCommit(_.actor, _.fine);
        }).AddTo(this);
        MessageBroker.Default.Receive<GameEvent.GameEvent_Local_TimeChange>().Subscribe(_ =>
        {
            AllClientListen_WorldGlobalTimeChange(_.hour, _.date, _.now);
        }).AddTo(this);
    }
    /// <summary>
    /// ����ʱ��ı�(�ͻ���)
    /// </summary>
    /// <param name="globalTime"></param>
    public virtual void AllClientListen_WorldGlobalTimeChange(int hour, int date, GlobalTime globalTime)
    {
        if (isState)
        {
            OnlyStateListen_WorldGlobalTimeChange(hour, date, globalTime);
        }
    }
    /// <summary>
    /// ����ʱ��ı�(������)
    /// </summary>
    /// <param name="hour"></param>
    /// <param name="date"></param>
    /// <param name="globalTime"></param>
    public virtual void OnlyStateListen_WorldGlobalTimeChange(int hour, int date, GlobalTime globalTime)
    {

    }
    /// <summary>
    /// ����ĳ���ƶ�(�ͻ���)
    /// </summary>
    /// <param name="who"></param>
    /// <param name="where"></param>
    public virtual void AllClientListen_RoleMove(ActorManager who, MyTile where)
    {
        if (who == this)
        {
            AllClientListen_MoveMyself(who, where);
        }
        else
        {
            AllClientListen_MoveOther(who, where);
        }
    }
    /// <summary>
    /// �������Լ��ƶ�(�ͻ���)
    /// </summary>
    /// <param name="who"></param>
    /// <param name="where"></param>
    public virtual void AllClientListen_MoveMyself(ActorManager who, MyTile where)
    {
        for (int i = 0; i < bindBuffEntity.Count; i++)
        {
            bindBuffEntity[i].Listen_MyselfMove(this);
        }
        if (isState)
        {
            OnlyStateListen_MoveMyself(who, where);
        }
        AllClient_UpdateNearByTile();
    }
    /// <summary>
    /// �������Լ��ƶ�(������)
    /// </summary>
    /// <param name="who"></param>
    /// <param name="where"></param>
    public virtual void OnlyStateListen_MoveMyself(ActorManager who, MyTile where)
    {

    }
    /// <summary>
    /// �����������ƶ�(�ͻ���)
    /// </summary>
    /// <param name="who"></param>
    /// <param name="where"></param>
    public virtual void AllClientListen_MoveOther(ActorManager who, MyTile where)
    {
        if (isState)
        {
            OnlyStateListen_MoveOther(who, where);
        }
    }
    /// <summary>
    /// �����������ƶ�(������)
    /// </summary>
    /// <param name="who"></param>
    /// <param name="where"></param>
    public virtual void OnlyStateListen_MoveOther(ActorManager who, MyTile where)
    {

    }
    /// <summary>
    /// ����ĳ������ʲô����(�ͻ���)
    /// </summary>
    /// <param name="who"></param>
    /// <param name="actorAction"></param>
    public virtual void AllClientListen_RoleDoSomething(ActorManager who, GameEvent.ActorAction actorAction)
    {
        if (isState)
        {
            OnlyStateListen_RoleDoSomething(who, actorAction);
        }
    }
    /// <summary>
    /// ����ĳ������ʲô����(������)
    /// </summary>
    /// <param name="who"></param>
    /// <param name="actorAction"></param>
    public virtual void OnlyStateListen_RoleDoSomething(ActorManager who, GameEvent.ActorAction actorAction)
    {

    }
    /// <summary>
    /// ����ĳ�˷���(�ͻ���)
    /// </summary>
    /// <param name="who"></param>
    /// <param name="actorAction"></param>
    public virtual void AllClientListen_RoleCommit(ActorManager who, short val)
    {
        if (isState)
        {
            OnlyStateListen_RoleCommit(who, val);
        }
    }
    /// <summary>
    /// ����ĳ�˷���(������)
    /// </summary>
    /// <param name="who"></param>
    /// <param name="val"></param>
    public virtual void OnlyStateListen_RoleCommit(ActorManager who, short val)
    {

    }
    /// <summary>
    /// ����ĳ�˷���emoji(�ͻ���)
    /// </summary>
    /// <param name="actor">˭</param>
    /// <param name="id">ʲô</param>
    /// <param name="distance">����</param>
    public virtual void AllClientListen_RoleSendEmoji(ActorManager actor, int id, float distance)
    {
        if (isState)
        {
            OnlyStateListen_RoleSendEmoji(actor, id, distance);
        }
    }
    /// <summary>
    /// ����ĳ�˷���emoji(������)
    /// </summary>
    /// <param name="actor"></param>
    /// <param name="id"></param>
    /// <param name="distance"></param>
    public virtual void OnlyStateListen_RoleSendEmoji(ActorManager actor, int id, float distance)
    {

    }
    /// <summary>
    /// ��������ֵ�ı�(�ͻ���)
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="id"></param>
    public virtual void AllClientListen_MyselfHpChange(int parameter, Fusion.NetworkId id)
    {
        if (parameter <= 0)
        {
            ShowText(parameter.ToString(), new Color32(255, 50, 50, 255), 64, Vector2.up);
            AllClient_PlayTakeDamage(1);
        }
        if (isState)
        {
            OnlyStateListen_MyselfHpChange(parameter, id);
        }
    }
    /// <summary>
    ///  ��������ֵ�ı�(������)
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="id"></param>
    public virtual void OnlyStateListen_MyselfHpChange(int parameter, Fusion.NetworkId id)
    {

    }
    /// <summary>
    /// ��������״̬�仯(�ͻ���)
    /// </summary>
    /// <param name="state"></param>
    public virtual void AllClientListen_ChangeAttackState(bool state)
    {
        allClient_AttackState = state;
        if (isState)
        {
            OnlyStateListen_ChangeAttackState(state);
        }
    }
    /// <summary>
    /// ��������״̬�仯(������)
    /// </summary>
    /// <param name="state"></param>
    public virtual void OnlyStateListen_ChangeAttackState(bool state)
    {
       
    }
    /// <summary>
    /// ��������Ŀ��仯(�ͻ���)
    /// </summary>
    /// <param name="id"></param>
    public virtual void AllClientListen_ChangeAttackTarget(NetworkId id)
    {
        if (id != new NetworkId())
        {
            allClient_AttackTarget = netManager.Runner.FindObject(id).GetComponent<ActorManager>();
        }
        else
        {
            allClient_AttackTarget = null;
        }
        if (isState)
        {
            OnlyStateListen_ChangeAttackTarget(id);
        }
    }
    /// <summary>
    /// ��������Ŀ��仯(������)
    /// </summary>
    /// <param name="id"></param>
    public virtual void OnlyStateListen_ChangeAttackTarget(NetworkId id)
    {

    }
    #endregion
    /*��ʱ*/
    #region
    [HideInInspector]
    public const float customUpdateTime = 0.1f;
    /// <summary>
    /// ��ʼ�Զ���ѭ��(�ͻ���)
    /// </summary>
    private void AllClient_StatrLoop()
    {
        InvokeRepeating("AllClient_CustomUpdate", 1f, customUpdateTime);
        InvokeRepeating("AllClient_AddOneSecond", 1, 1);
    }
    /// <summary>
    /// �������(�ͻ���)
    /// </summary>
    public virtual void FixedUpdate()
    {
        
    }
    /// <summary>
    /// �������(������)
    /// </summary>
    /// <param name="dt"></param>
    public virtual void OnlyState_FixedUpdateNetwork(float dt)
    {
        OnlyState_RunningPath(dt);
    }
    /// <summary>
    /// �������(�ͻ���)
    /// </summary>
    /// <param name="dt"></param>
    public virtual void AllClient_FixedUpdateNetwork(float dt)
    {
    }
    /// <summary>
    /// �������(������)
    /// </summary>
    /// <param name="dt"></param>
    public virtual void OnlyState_Render(float dt)
    {

    }
    /// <summary>
    /// �������(�ͻ���)
    /// </summary>
    /// <param name="dt"></param>
    public virtual void AllClient_Render(float dt)
    {

    }
    /// <summary>
    /// �Զ������(�ͻ���)
    /// </summary>
    public virtual void AllClient_CustomUpdate()
    {
        AllClient_UpdateBody(customUpdateTime);
        AllClient_CheckMyTile();
    }
    /// <summary>
    /// �����(�ͻ���)
    /// </summary>
    public virtual void AllClient_AddOneSecond()
    {
        AllClient_GetHungry(1);
        AllClient_UpdateItemTime(1);
    }
    #endregion
    /*����״̬*/
    #region
    private Vector2 temp_lastPos;
    private Vector2 temp_curPos;
    private float temp_rigSpeed;
    /// <summary>
    /// ��������״̬(�ͻ���)
    /// </summary>
    /// <param name="dt"></param>
    public virtual void AllClient_UpdateBody(float dt)
    {
        temp_curPos = transform.position;
        temp_rigSpeed = netManager.networkRigidbody.Rigidbody.velocity.magnitude;

        float distance = Vector2.Distance(temp_lastPos, temp_curPos);
        float speed = distance * 10;
        if (temp_rigSpeed <= 0.1f)
        {
            if (speed > 0.1f)
            {
                AllClient_PlayMove(speed);
                AllClient_TurnTo(temp_curPos - temp_lastPos);
            }
            else
            {
                AllClient_PlayStop(1);
            }
        }
        temp_lastPos = temp_curPos;
    }
    /// <summary>
    /// ����ĳ��(�ͻ���)
    /// </summary>
    public virtual void AllClient_FaceTo(Vector2 dir)
    {
        bodyController.faceDir = dir.normalized;
        if (dir.x > 0)
        {
            bodyController.FaceRight();
        }
        else
        {
            bodyController.FaceLeft();
        }
    }
    /// <summary>
    /// ת��ĳ��(�ͻ���)
    /// </summary>
    /// <param name="turn"></param>
    public virtual void AllClient_TurnTo(Vector3 turn)
    {
        if (turn.x > 0)
        {
            bodyController.TurnRight();
        }
        if (turn.x < 0)
        {
            bodyController.TurnLeft();
        }
    }
    /// <summary>
    /// ��������(�ͻ���)
    /// </summary>
    /// <param name="speed"></param>
    public virtual void AllClient_PlayMove(float speed)
    {
        bodyController.SetHeadFloat("MoveSpeed", speed);
        bodyController.SetHeadBool("Step", true, 1, null);
        bodyController.SetHeadBool("Idle", false, 1, null);

        bodyController.SetBodyFloat("MoveSpeed", speed);
        bodyController.SetBodyBool("Step", true, 1, null);
        bodyController.SetBodyBool("Idle", false, 1, null);

        bodyController.SetHandFloat("MoveSpeed", speed);
        bodyController.SetHandBool("Step", true, 1, null);
        bodyController.SetHandBool("Idle", false, 1, null);

        bodyController.SetLegFloat("MoveSpeed", speed);
        bodyController.SetLegBool("Step", true, 1, null);
        bodyController.SetLegBool("Idle", false, 1, null);
    }
    /// <summary>
    /// ����ͣ��(�ͻ���)
    /// </summary>
    /// <param name="speed"></param>
    public virtual void AllClient_PlayStop(float speed)
    {
        bodyController.SetHeadFloat("MoveSpeed", speed);
        bodyController.SetHeadBool("Step", false, 1, null);
        bodyController.SetHeadBool("Idle", true, 1, null);

        bodyController.SetBodyFloat("MoveSpeed", speed);
        bodyController.SetBodyBool("Step", false, 1, null);
        bodyController.SetBodyBool("Idle", true, 1, null);

        bodyController.SetHandFloat("MoveSpeed", speed);
        bodyController.SetHandBool("Step", false, 1, null);
        bodyController.SetHandBool("Idle", true, 1, null);

        bodyController.SetLegFloat("MoveSpeed", speed);
        bodyController.SetLegBool("Step", false, 1, null);
        bodyController.SetLegBool("Idle", true, 1, null);
    }
    /// <summary>
    /// ��������(�ͻ���)
    /// </summary>
    /// <param name="speed"></param>
    /// <param name="action"></param>
    public virtual void AllClient_PlayDead(float speed,Action<string> action)
    {
        bodyController.SetHeadTrigger("Dead", 1, action);
        bodyController.SetBodyTrigger("Dead", 1, null);
        bodyController.SetHandTrigger("Dead", 1, null);
        bodyController.SetLegTrigger("Dead", 1, null);
        bodyController.locking = true;
    }
    /// <summary>
    /// ��������(�ͻ���)
    /// </summary>
    /// <param name="speed"></param>
    public virtual void AllClient_PlayTakeDamage(float speed)
    {
        bodyController.SetHeadTrigger("TakeDamage", 1, null);
    }
    /// <summary>
    /// ���ż���(�ͻ���)
    /// </summary>
    /// <param name="speed"></param>
    /// <param name="action"></param>
    public virtual void AllClient_PlayPickUp(float speed, Action<string> action)
    {
        bodyController.SetHeadTrigger("LowerHead", 1, null);
        bodyController.SetHandTrigger("PickUp", 1, action);
    }
    #endregion
    /*λ��*/
    #region
    /// <summary>
    /// �ϸ��ؿ�
    /// </summary>
    public MyTile record_lastTile = null;
    /// <summary>
    /// ����ؿ�
    /// </summary>
    public MyTile record_curTile = null;

    /// <summary>
    /// ��鵱ǰ�ؿ�(�ͻ���)
    /// </summary>
    public void AllClient_CheckMyTile()
    {
        record_curTile = Tool_GetMyTileWithOffset(Vector3Int.zero);
        if (record_lastTile != record_curTile)
        {
            record_lastTile = record_curTile;
            MessageBroker.Default.Publish(new GameEvent.GameEvent_Local_SomeoneMove
            {
                moveActor = this,
                moveTile = record_curTile
            });
        }
    }
    /// <summary>
    /// ��Χ�ؿ�
    /// </summary>
    private List<MyTile> record_NearbyTiles = new List<MyTile>();
    private List<MyTile> temp_NearbyTiles = new List<MyTile>();
    /// <summary>
    /// ���¸����ĵؿ�
    /// </summary>
    private void AllClient_UpdateNearByTile()
    {
        /*��Χ�ؿ�*/
        temp_NearbyTiles = Tool_GetNearbyTiles(NearByMean.EightSide);
        temp_NearbyTiles[0].StandOnTileByActor(this);
        /*�޳��ϴμ��ĵؿ�*/
        for (int i = 0; i < record_NearbyTiles.Count; i++)
        {
            if (record_NearbyTiles[i] == null) { continue; }
            if (!temp_NearbyTiles.Contains(record_NearbyTiles[i]))
            {
                record_NearbyTiles[i].FarawayTileByActor(this);
                record_NearbyTiles.RemoveAt(i);
            }
        }
        /*��ӱ��μ���ĵؿ�*/
        for (int i = 0; i < temp_NearbyTiles.Count; i++)
        {
            if (temp_NearbyTiles[i] == null) { continue; }
            if (temp_NearbyTiles[i].NearbyTileByActor(this))
            {
                if (!record_NearbyTiles.Contains(temp_NearbyTiles[i]))
                {
                    record_NearbyTiles.Add(temp_NearbyTiles[i]);
                }
            }
        }
    }
    /// <summary>
    /// ͨ�÷���(������һ�׼��һ���ؿ�)
    /// </summary>
    /// <param name="offset">ƫ��</param>
    /// <returns>�ؿ�</returns>
    public MyTile Tool_GetMyTileWithOffset(Vector3Int offset)
    {
        if (navManager != null)
        {
            Vector3Int vector3Int = navManager.grid.WorldToCell(transform.position);
            return (MyTile)navManager.tilemap_Building.GetTile(vector3Int + offset);
        }
        else
        {
            return null;
        }
    }
    private List<MyTile> temp_TilesList = new List<MyTile>();
    /// <summary>
    /// �����Χ�ؿ�
    /// </summary>
    /// <param name="radiu">�뾶</param>
    /// <returns></returns>
    public List<MyTile> Tool_GetNearbyTiles(NearByMean type)
    {
        temp_TilesList.Clear();
        MyTile tile = Tool_GetMyTileWithOffset(Vector3Int.zero);
        temp_TilesList.Add((MyTile)navManager.tilemap_Building.GetTile(tile._posInCell));
        temp_TilesList.Add((MyTile)navManager.tilemap_Building.GetTile(tile._posInCell + Vector3Int.up));
        temp_TilesList.Add((MyTile)navManager.tilemap_Building.GetTile(tile._posInCell + Vector3Int.up + Vector3Int.right));
        temp_TilesList.Add((MyTile)navManager.tilemap_Building.GetTile(tile._posInCell + Vector3Int.right));
        temp_TilesList.Add((MyTile)navManager.tilemap_Building.GetTile(tile._posInCell + Vector3Int.right + Vector3Int.down));
        temp_TilesList.Add((MyTile)navManager.tilemap_Building.GetTile(tile._posInCell + Vector3Int.down));
        temp_TilesList.Add((MyTile)navManager.tilemap_Building.GetTile(tile._posInCell + Vector3Int.down + Vector3Int.left));
        temp_TilesList.Add((MyTile)navManager.tilemap_Building.GetTile(tile._posInCell + Vector3Int.left));
        temp_TilesList.Add((MyTile)navManager.tilemap_Building.GetTile(tile._posInCell + Vector3Int.left + Vector3Int.up));
        return temp_TilesList;
    }
    public enum NearByMean
    {
        /// <summary>
        /// �ķ���
        /// </summary>
        FourSide,
        /// <summary>
        /// �˷���
        /// </summary>
        EightSide,
    }
    #endregion
    /*��Ʒ*/
    #region
    [HideInInspector]
    public ItemBase itemOnHand = new ItemBase();
    [HideInInspector]
    public ItemBase itemOnHead = new ItemBase();
    [HideInInspector]
    public ItemBase itemOnBody = new ItemBase();
    [HideInInspector]
    public List<ItemBase> itemInBag = new List<ItemBase>();
    /// <summary>
    /// �õ�����(�ͻ���)
    /// </summary>
    /// <param name="data"></param>
    /// <param name="showInUI"></param>
    public void AllClient_HoldItemInHand(ItemData data)
    {
        /*����UI*/
        if (isPlayer && netManager.Object.HasInputAuthority)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateItemInHand()
            {
                itemData = data
            });
        }
        if (data.Item_ID != 0)
        {
            if (netManager.Last_ItemInHand.Item_ID != data.Item_ID)
            {
                AllClient_ResetItemInHand();
                Type type = Type.GetType("Item_" + data.Item_ID.ToString());
                itemOnHand = (ItemBase)Activator.CreateInstance(type);
                itemOnHand.UpdateDataFromNet(data);
                itemOnHand.Holding_Start(this, BodyController);
                itemOnHand.Holding_UpdateLook();
            }
            else
            {
                if (itemOnHand != null)
                {
                    itemOnHand.UpdateDataFromNet(data);
                    itemOnHand.Holding_UpdateLook();
                }
            }
        }
        else
        {
            AllClient_ResetItemInHand();
        }
        netManager.Last_ItemInHand = data;
    }
    /// <summary>
    /// ������������(�ͻ���)
    /// </summary>
    public void AllClient_ResetItemInHand()
    {
        if (itemOnHand != null) { itemOnHand.Holding_Over(this); }
        itemOnHand = new ItemBase();
        if (bodyController.Tran_LeftItemInHand.childCount > 0)
        {
            for (int i = 0; i < bodyController.Tran_LeftItemInHand.childCount; i++)
            {
                Destroy(bodyController.Tran_LeftItemInHand.GetChild(i).gameObject);
            }
        }
        if (bodyController.Tran_RightItemInHand.childCount > 0)
        {
            for (int i = 0; i < bodyController.Tran_RightItemInHand.childCount; i++)
            {
                Destroy(bodyController.Tran_RightItemInHand.GetChild(i).gameObject);
            }
        }

        bodyController.Tran_LeftHand.GetComponent<SpriteRenderer>().enabled = true;
        bodyController.Tran_RightHand.GetComponent<SpriteRenderer>().enabled = true;

        bodyController.SetBodyBool("Idle", true, 1, null);
        bodyController.SetHeadBool("Idle", true, 1, null);
        bodyController.SetHandBool("Idle", true, 1, null);
        bodyController.SetHandBool("Idle", true, 1, null);

        bodyController.Tran_RightItemInHand.localScale = Vector3.one;
        bodyController.Tran_LeftItemInHand.localScale = Vector3.one;
        bodyController.Tran_RightItemInHand.localScale = Vector3.one;
        bodyController.Tran_LeftItemInHand.localPosition = Vector3.zero;
        bodyController.Tran_LeftItemInHand.localRotation = Quaternion.identity;
        bodyController.Tran_RightItemInHand.localPosition = Vector3.zero;
        bodyController.Tran_RightItemInHand.localRotation = Quaternion.identity;
        bodyController.Tran_LeftItemInHand.GetComponent<SpriteRenderer>().sprite
            = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_Default");
        bodyController.Tran_LeftItemInHand.GetComponent<SpriteRenderer>().sortingOrder = 1;

        bodyController.Tran_RightItemInHand.GetComponent<SpriteRenderer>().sprite
            = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_Default");
        bodyController.Tran_RightItemInHand.GetComponent<SpriteRenderer>().sortingOrder = 4;
    }
    /// <summary>
    /// ����ͷ��(�ͻ���)
    /// </summary>
    /// <param name="data"></param>
    public void AllClient_WearItemOnHead(ItemData data)
    {
        AllClient_ResetItemOnHead();
        /*����UI*/
        if (isPlayer && netManager.Object.HasInputAuthority)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateStatus()
            {
                statusId = NetManager.LocalData_Status
            });
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateItemOnHead()
            {
                itemData = data
            });
        }
        if (data.Item_ID != 0)
        {
            Type type = Type.GetType("Item_" + data.Item_ID.ToString());
            itemOnHead = (ItemBase)Activator.CreateInstance(type);
            itemOnHead.UpdateDataFromNet(data);
            itemOnHead.BeWearingOnHead(this, BodyController);
        }
    }
    /// <summary>
    /// ����ͷ������(�ͻ���)
    /// </summary>
    public void AllClient_ResetItemOnHead()
    {
        itemOnHead = new ItemBase();
        if (bodyController.Tran_ItemOnHead.childCount > 0)
        {
            for (int i = 0; i < bodyController.Tran_ItemOnHead.childCount; i++)
            {
                Destroy(bodyController.Tran_ItemOnHead.GetChild(i).gameObject);
            }
        }
        bodyController.Tran_ItemOnHead.localScale = Vector3.one;
        bodyController.Tran_ItemOnHead.localPosition = new Vector3(0, 0.25f, 0);
        bodyController.Tran_ItemOnHead.localRotation = Quaternion.identity;
        bodyController.Tran_ItemOnHead.GetComponent<SpriteRenderer>().sprite
            = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_Default");
        bodyController.Tran_ItemOnHead.GetComponent<SpriteRenderer>().sortingOrder = 3;
    }
    /// <summary>
    /// ��������(�ͻ���)
    /// </summary>
    /// <param name="data"></param>
    public void AllClient_WearItemOnBody(ItemData data)
    {
        AllClient_ResetItemOnBody();
        /*����UI*/
        if (isPlayer && netManager.Object.HasInputAuthority)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateStatus()
            {
                statusId = NetManager.LocalData_Status
            });
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateItemOnBody()
            {
                itemData = data
            });
        }
        if (data.Item_ID != 0)
        {
            Type type = Type.GetType("Item_" + data.Item_ID.ToString());
            itemOnBody = (ItemBase)Activator.CreateInstance(type);
            itemOnBody.UpdateDataFromNet(data);
            itemOnBody.BeWearingOnBody(this, BodyController);
        }
    }
    /// <summary>
    /// ������������(�ͻ���)
    /// </summary>
    public void AllClient_ResetItemOnBody()
    {
        itemOnHead = new ItemBase();
        if (bodyController.Tran_ItemOnBody.childCount > 0)
        {
            for (int i = 0; i < bodyController.Tran_ItemOnBody.childCount; i++)
            {
                Destroy(bodyController.Tran_ItemOnBody.GetChild(i).gameObject);
            }
        }
        bodyController.Tran_ItemOnBody.localScale = Vector3.one;
        bodyController.Tran_ItemOnBody.localPosition = Vector3.zero;
        bodyController.Tran_ItemOnBody.localRotation = Quaternion.identity;
        bodyController.Tran_ItemOnBody.GetComponent<SpriteRenderer>().sprite
            = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_Default");
        bodyController.Tran_ItemOnBody.GetComponent<SpriteRenderer>().sortingOrder = 2;
    }
    /// <summary>
    /// ������Ʒʱ���(�ͻ���)
    /// </summary>
    /// <param name="val"></param>
    public void AllClient_UpdateItemTime(int val)
    {
        if (itemOnHand != null)
        {
            itemOnHand.UpdateTime(val);
        }
        if (itemOnHead != null)
        {
            itemOnHead.UpdateTime(val);
        }
        if (itemOnBody != null)
        {
            itemOnBody.UpdateTime(val);
        }
    }
    #endregion
    /*���*/
    #region
    /// <summary>
    /// ͨ�÷���(����ĳ��)
    /// </summary>
    /// <param name="who"></param>
    /// <param name="handItemID"></param>
    /// <param name="headItemID"></param>
    /// <param name="bodyItemID"></param>
    public void Tool_CheckOutSomeone(ActorManager who, out short handItemID, out short headItemID, out short bodyItemID, out short fine)
    {
        handItemID = who.NetManager.Data_ItemInHand.Item_ID;
        headItemID = who.NetManager.Data_ItemOnHead.Item_ID;
        bodyItemID = who.NetManager.Data_ItemOnBody.Item_ID;
        fine = who.NetManager.Data_Fine;
    }
    /// <summary>
    /// ͨ�÷���(������)
    /// </summary>
    /// <param name="clothes"></param>
    /// <param name="hat"></param>
    /// <param name="fine"></param>
    /// <param name="id"></param>
    public void Tool_CheckStatus(short clothes,short hat,short fine,out short id)
    {
        id = 0;
        StatusConfig status;
        status = StatusConfigData.statusConfigs.Find((x) => { return x.Status_Clothes == clothes; });
        if (status.Status_ID != 0)
        {
            id = status.Status_ID;
            return;
        }
        status = StatusConfigData.statusConfigs.Find((x) => { return x.Status_Hat == hat; });
        if (status.Status_ID != 0)
        {
            id = status.Status_ID;
            return;
        }
        if (clothes / 1000 != 5 && hat / 1000 != 5)
        {
            id = 1002;
        }
        else
        {
            id = 1003;
        }
    }
    #endregion
    /*Ѱ·*/
    #region
    private NavManager navManager;
    /// <summary>
    /// Ŀ��·��(������)
    /// </summary>
    private List<MyTile> onlyState_TargetPath = new List<MyTile>();
    /// <summary>
    /// Ŀ��ؿ�(������)
    /// </summary>
    private MyTile onlyState_TargetTile = null;
    /// <summary>
    /// ִ��·��(������)
    /// </summary>
    /// <param name="dt"></param>
    private void OnlyState_RunningPath(float dt)
    {
        if (onlyState_TargetTile)
        {
            if (Vector2.Distance(onlyState_TargetTile._posInWorld, transform.position) <= 0.1f)
            {
                /*����·���㣬���*/
                if (onlyState_TargetPath.Count > 0)
                {
                    /*·����δ����*/
                    OnlyState_UpdateTargetTile(onlyState_TargetPath[0]);
                    onlyState_TargetPath.RemoveAt(0);
                }
                else
                {
                    /*·���Ѿ�����*/
                    OnlyState_UpdateTargetTile(null);
                }
                return;
            }
            else
            {
                /*��δ����·���㣬ǰ��·����*/
                Vector2 temp = Vector2.zero;
                if (transform.position.x > onlyState_TargetTile._posInWorld.x)
                {
                    if ((transform.position.x - onlyState_TargetTile._posInWorld.x) > 0.05f)
                    {
                        temp += new Vector2(-1, 0);
                    }
                }
                else
                {
                    if ((transform.position.x - onlyState_TargetTile._posInWorld.x) < -0.05f)
                    {
                        temp += new Vector2(1, 0);
                    }
                }
                if (transform.position.y > onlyState_TargetTile._posInWorld.y)
                {
                    if ((transform.position.y - onlyState_TargetTile._posInWorld.y) > 0.05f)
                    {
                        temp += new Vector2(0, -1);
                    }
                }
                else
                {
                    if ((transform.position.y - onlyState_TargetTile._posInWorld.y) < -0.05f)
                    {
                        temp += new Vector2(0, 1);
                    }
                }
                temp = temp.normalized;

                float commonSpeed = NetManager.Data_CommonSpeed / 10f;
                Vector2 velocity = new Vector2(temp.x * commonSpeed, temp.y * commonSpeed);
                Vector3 newPos = transform.position + new UnityEngine.Vector3(velocity.x * dt, velocity.y * dt, 0);
                netManager.OnlyState_UpdateNetworkTransform(newPos, velocity.magnitude);
            }
        }
    }
    /// <summary>
    /// ����Ŀ��ؿ�
    /// </summary>
    public virtual void OnlyState_UpdateTargetTile(MyTile myTile)
    {
        onlyState_TargetTile = myTile;
    }
    /// <summary>
    /// �ҵ�һ��·��(������)
    /// </summary>
    /// <param name="targetPos"></param>
    public void OnlyState_FindWayToTarget(Vector2 targetPos)
    {
        onlyState_TargetPath.Clear();
        onlyState_TargetPath = navManager.FindPath(navManager.FindTileByPos(targetPos), Tool_GetMyTileWithOffset(Vector3Int.zero));
        if (onlyState_TargetPath.Count > 1)
        {
            OnlyState_UpdateTargetTile(onlyState_TargetPath[0]);
            onlyState_TargetPath.RemoveAt(0);
        }
    }
    #endregion
    /*�������Ա仯*/
    #region
    #region//����ϵͳ
    /// <summary>
    /// ������ʱ��(������ר��)
    /// </summary>
    private int onlyState_hungryTimer;
    /// <summary>
    /// �е�����(�ͻ���)
    /// </summary>
    private void AllClient_GetHungry(int val)
    {
        onlyState_hungryTimer += val;
        if (onlyState_hungryTimer >= netManager.Data_Water + 5)
        {
            onlyState_hungryTimer = 0;
            if (AllClient_SubFood(-1) <= 0)
            {

            }
        }
    }
    /// <summary>
    /// ����ʳ��ֵ(�ͻ���)
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public int AllClient_SubFood(int val)
    {
        if (actorState != ActorState.Dead && isInput)
        {
            if (netManager.Data_CurFood + val > 0)
            {
                netManager.RPC_LocalInput_FoodChange((short)(netManager.Data_CurFood + val));
            }
            else
            {
                netManager.RPC_LocalInput_FoodChange(0);
            }
        }
        return netManager.Data_CurFood;
    }
    /// <summary>
    /// ����ʳ��ֵ(�ͻ���)
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public int AllClient_AddFood(int val)
    {
        if (actorState != ActorState.Dead && isInput)
        {
            if (netManager.Data_CurFood + val <= netManager.Data_MaxFood)
            {
                netManager.RPC_LocalInput_FoodChange((short)(netManager.Data_CurFood + val));
            }
            else
            {
                netManager.RPC_LocalInput_FoodChange(netManager.Data_MaxFood);
            }
        }
        return netManager.Data_CurFood;
    }
    #endregion
    #region//����ϵͳ
    /// <summary>
    /// �ܵ��˺�(�ͻ���)
    /// </summary>
    /// <param name="val"></param>
    /// <param name="id"></param>
    public void AllClient_TakeDamage(int val, ActorNetManager from)
    {
        if (actorState != ActorState.Dead)
        {
            NetworkId networkId = new NetworkId();
            if (from) { networkId = from.Object.Id; }
            val -= netManager.Data_Armor;
            if (val > 0)
            {
                netManager.RPC_AllClient_HpChange(-val, networkId);
            }
            else
            {
                netManager.RPC_AllClient_HpChange(0, networkId);
            }
        }
    }
    #endregion
    #region//����ϵͳ
    /// <summary>
    /// ��������(�ͻ���)
    /// </summary>
    public void AllClient_TryToDead()
    {
        if (actorState != ActorState.Dead)
        {
            actorState = ActorState.Dead;
            AllClient_PlayDead(1, (string str) =>
            {
                if (str.Equals("Dead"))
                {
                    AllClient_AlreadyDead();
                    if (isState) { OnlyState_AlreadyDead(); }
                }
            });
        }
    }
    /// <summary>
    /// �Ѿ�����(�ͻ���)
    /// </summary>
    public virtual void AllClient_AlreadyDead()
    {

    }
    /// <summary>
    /// �Ѿ�����(������)
    /// </summary>
    public virtual void OnlyState_AlreadyDead()
    {
        if (isPlayer)
        {

        }
        else
        {
            netManager.Runner.Despawn(netManager.Object);
        }
    }
    #endregion
    #region//����ϵͳ
    /// <summary>
    /// ����֧��(�ͻ���)
    /// </summary>
    /// <returns></returns>
    public bool AllClient_PayCoin(int coin)
    {
        if (netManager.Data_Coin >= coin && isInput)
        {
            netManager.RPC_LocalInput_PayCoin(coin);
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// ����׬Ǯ(�ͻ���)
    /// </summary>
    /// <returns></returns>
    public int AllClient_EarnCoin(int coin)
    {
        if (isInput)
        {
            netManager.RPC_LocalInput_EarnCoin(coin);
        }
        return netManager.Data_Coin;
    }
    #endregion
    #region//�ͽ�ϵͳ
    /// <summary>
    /// ���÷���(�ͻ���)
    /// </summary>
    public void AllClient_SetFine(short val)
    {
        if (NetManager.Data_Fine < val && isInput)
        {
            NetManager.RPC_LocalInput_ChangeFine(val);
        }
    }
    /// <summary>
    /// ��շ���(�ͻ���)
    /// </summary>
    public void AllClient_ClearFine()
    {
        if (isInput)
        {
            NetManager.RPC_LocalInput_ChangeFine(0);
        }
    }
    #endregion
    #endregion
    /*Buff*/
    #region
    [HideInInspector]
    Dictionary<short, BuffBase> bindBuffDic = new Dictionary<short, BuffBase>();
    [HideInInspector]
    public List<short> bindBuffID = new List<short>();
    [HideInInspector]
    public List<BuffBase> bindBuffEntity = new List<BuffBase>();
    public virtual void AllClient_UpdateBuff(NetworkLinkedList<short> buffs)
    {
        foreach(short buff in buffs)
        {
            AllClient_TryToBindBuff(buff);
        }
    }
    public virtual void AllClient_TryToBindBuff(short id)
    {
        if (!bindBuffID.Contains(id))
        {
            Type type = Type.GetType("Buff" + id.ToString());
            BuffBase buff = (BuffBase)Activator.CreateInstance(type);
            bindBuffDic.Add(id, buff);
            bindBuffEntity.Add(buff);
            bindBuffID.Add(id);
            buff.Listen_AddOnActor(this);

            if (isPlayer && isInput)
            {
                MessageBroker.Default.Publish(new UIEvent.UIEvent_AddBuff()
                {
                    buffConfig = BuffConfigData.GetBuffConfig(id),
                });
            }
        }
    }
    public virtual void AllClient_RemoveBuff(short id)
    {
        if (bindBuffID.Contains(id))
        {
            BuffBase buff = bindBuffDic[id];
            bindBuffEntity.Remove(buff);
            bindBuffID.Remove(id);
            bindBuffDic.Remove(id);
            buff.Listen_SubFromActor(this);

            if (isPlayer && isInput)
            {
                MessageBroker.Default.Publish(new UIEvent.UIEvent_SubBuff()
                {
                    buffConfig = BuffConfigData.GetBuffConfig(id),
                });
            }
        }
    }
    #endregion
    /*����*/
    #region
    [HideInInspector]
    public SkillBase bindSkill = new SkillBase();
    /// <summary>
    /// ��һ������(�ͻ���)
    /// </summary>
    /// <param name="skillID"></param>
    public virtual void AllClient_BindSkill(int skillID)
    {
        Type type = Type.GetType("Skill_" + skillID.ToString());
        bindSkill = (SkillBase)Activator.CreateInstance(type);
        bindSkill.Init(skillID, this);
    }
    /// <summary>
    /// �ͷ�һ������(�ͻ���)
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="id"></param>
    public virtual void AllClient_UseSkill(int parameter, Fusion.NetworkId id)
    {
        bindSkill.ClickSpace();
    }
    #endregion
    /*����*/
    #region
    /// <summary>
    /// ����һ������(�ͻ���)
    /// </summary>
    /// <param name="emojiID"></param>
    public virtual void AllClient_SendEmoji(int emojiID)
    {
        Debug.Log("Emoji" + emojiID);
        MessageBroker.Default.Publish(new GameEvent.GameEvent_Local_SomeoneSendEmoji
        {
            actor = this,
            id = emojiID,
            distance = EmojiConfigData.GetEmojiConfig(emojiID).Emoji_Distance
        });
        ActorUI.SendEmoji(EmojiConfigData.GetEmojiConfig(emojiID));
    }
    #endregion
    /*AI����*/
    #region
    /// <summary>
    /// ע��Ŀ��(������)
    /// </summary>
    protected List<ActorManager> onlyState_LookTarget = new List<ActorManager>();
    /// <summary>
    /// ����Ŀ��(������)
    /// </summary>
    protected List<ActorManager> onlyState_RememberTarget = new List<ActorManager>();
    /// <summary>
    /// ����Ŀ��(�ͻ���)
    /// </summary>
    protected ActorManager allClient_AttackTarget;
    /// <summary>
    /// ����״̬(�ͻ���)
    /// </summary>
    protected bool allClient_AttackState { get; set; } = false;
    protected float onlyState_AttackCD;
    protected float onlyState_AttackCDTimer;

    /// <summary>
    /// ����Ƿ�ʼ��һ�ι���(������)
    /// </summary>
    /// <param name="dt"></param>
    /// <returns>������һ�ι���</returns>
    public bool OnlyState_Update_CheckingAttackingTimer(float dt)
    {
        if (onlyState_AttackCDTimer > 0)
        {
            onlyState_AttackCDTimer -= dt;
            return false;
        }
        else
        {
            onlyState_AttackCDTimer = onlyState_AttackCD;
            return true;
        }

    }
    /// <summary>
    /// ����Ƿ�ﵽ��������(������)
    /// </summary>
    /// <param name="dt"></param>
    /// <returns>�ﵽ��������</returns>
    public bool OnlyState_Update_CheckingAttackingDistance(float dt)
    {
        if (Vector3.Distance(allClient_AttackTarget.transform.position, transform.position) < itemOnHand.itemConfig.Attack_Distance + 0.5f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion
    /*UI*/
    #region
    public void ShowText(string val,Color32 color,int size,Vector2 offset)
    {
        GameObject obj_num = UIManager.Instance.ShowUI("UI/UI_DamageNum", (Vector2)transform.position + offset);
        obj_num.GetComponent<UI_DamageNum>().Play(val, color, size);
    }
    #endregion
    /*ģ���������*/
    #region
    /// <summary>
    /// ģ���Ҽ���ѹʱ��
    /// </summary>
    private float mouseRightPressTimer;
    /// <summary>
    /// ģ�������ѹʱ��
    /// </summary>
    private float mouseLeftPressTimer;
    /// <summary>
    /// ģ�����λ��
    /// </summary>
    private Vector3 mouseLocation;
    /// <summary>
    /// ģ����갴��
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="inputType"></param>
    /// <returns>ģ�����</returns>
    public bool AllClient_Simulate_InputMousePress(float dt, MouseInputType inputType)
    {
        if (inputType == MouseInputType.PressRightThenPressLeft)
        {
            mouseRightPressTimer += dt;
            if (itemOnHand.UpdateRightPress(mouseRightPressTimer, isState, isInput, false))
            {
                mouseLeftPressTimer += dt;
                if (itemOnHand.UpdateLeftPress(mouseLeftPressTimer, isState, isInput, false))
                {
                    mouseRightPressTimer = 0;
                    mouseLeftPressTimer = 0;
                    itemOnHand.ReleaseLeftPress(isState, isInput, false);
                    itemOnHand.ReleaseRightPress(isState, isInput, false);
                    return true;
                }
            }
        }
        return false;
    }
    /// <summary>
    /// ģ�����λ��
    /// </summary>
    /// <param name="dt"></param>
    public void AllClient_Simulate_InputMousePos(Vector3 pos)
    {
        mouseLocation = pos;
        AllClient_FaceTo(mouseLocation - transform.position);
        itemOnHand.UpdateMousePos(mouseLocation - transform.position);
    }
    /// <summary>
    /// ��갴�����·���
    /// </summary>
    public enum MouseInputType
    {
        /// <summary>
        /// �Ȱ�ѹ�Ҽ��ٰ�ѹ���
        /// </summary>
        PressRightThenPressLeft
    }
    #endregion

}
public enum ActorState
{
    Default,
    Dead
}