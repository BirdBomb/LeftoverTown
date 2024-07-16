using DG.Tweening;
using Fusion;
using Fusion.Addons.Physics;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;
/// <summary>
/// ��ɫ������
/// </summary>
public class ActorManager : MonoBehaviour
{
    [Header("����")]
    public Rigidbody2D myRigidbody;
    [SerializeField, Header("��ɫ��������")]
    public ActorConfig actorConfig;
    [SerializeField, Header("���������")]
    private ActorNetManager netManager;
    public ActorNetManager NetManager
    {
        get { return netManager; }
        set { netManager = value; }
    }
    [SerializeField, Header("���������")]
    private BaseBodyController bodyController;
    public BaseBodyController BodyController
    {
        get { return bodyController; }
        set { bodyController = value; }
    }
    [SerializeField, Header("��Χָʾ��")]
    private SI_Sector skillSector;
    public SI_Sector SkillSector
    {
        get { return skillSector; }
        set { skillSector = value; }
    }
    [SerializeField, Header("UI������")]
    private ActorUI actorUI;
    [Header("������ʲô���?")]
    public ActorState actorState;
    public ActorUI ActorUI
    {
        get { return actorUI; }
        set { actorUI = value; }
    }
    [HideInInspector]
    public ItemBase holdingByHand = new ItemBase();
    [HideInInspector]
    public ItemBase wearingOnHead = new ItemBase();
    [HideInInspector]
    public ItemBase wearingOnBody = new ItemBase();
    [HideInInspector]
    public SkillBase bindSkill = new SkillBase();
    [HideInInspector]
    public NavManager navManager;

    public bool isPlayer = false;
    public bool isState = false;
    public bool isInput = false;
    public MyTile onlyState_myBed;
    public const float customUpdateTime = 0.1f;
    protected LayerMask tileLayer;
    private void Awake()
    {
        tileLayer = LayerMask.GetMask("TileObj_Wall");
        myRigidbody.gravityScale = 0;
    }
    public virtual void FixedUpdateNetwork(float dt)
    {
        UpdatePath(dt);
    }
    public virtual void CustomUpdate()
    {
        UpdateAnima(customUpdateTime);
        CheckMyTile();
    }
    public virtual void InitByNetManager(bool hasStateAuthority,bool hasInputAuthority)
    {
        isState = hasStateAuthority;
        isInput = hasInputAuthority;
        InvokeRepeating("CustomUpdate", 1f, customUpdateTime);
        navManager = GameObject.Find("Map").GetComponent<NavManager>();
        MessageBroker.Default.Receive<GameEvent.GameEvent_Local_SomeoneMove>().Subscribe(_ =>
        {
            ListenRoleMove(_.moveActor, _.moveTile);
        }).AddTo(this);
        MessageBroker.Default.Receive<GameEvent.GameEvent_Local_SomeoneSendEmoji>().Subscribe(_ =>
        {
            ListenRoleSendEmoji(_.actor, _.id);
        }).AddTo(this);
        MessageBroker.Default.Receive<GameEvent.GameEvent_Local_SomeoneDoSomething>().Subscribe(_ =>
        {
            ListenRoleDoSamething(_.actor, _.actorAction);
        }).AddTo(this);
        MessageBroker.Default.Receive<GameEvent.GameEvent_Local_TimeChange>().Subscribe(_ =>
        {
            ListenWorldGlobalTimeChange(_.now);
        }).AddTo(this);

        for (int i = 0; i < actorConfig.Config_BagCount; i++)
        {
            netManager.RPC_LocalInput_AddItemInBag(GetBagItem());
        }
    }
    public void InitByPlayer()
    {
        isPlayer = true;
    }
    /*����*/
    #region
    /// <summary>
    /// ����ʱ��ı�
    /// </summary>
    /// <param name="globalTime"></param>
    public virtual void ListenWorldGlobalTimeChange(GlobalTime globalTime)
    {
        
    }
    /// <summary>
    /// ����ĳ���ƶ�
    /// </summary>
    /// <param name="who"></param>
    /// <param name="where"></param>
    public virtual void ListenRoleMove(ActorManager who, MyTile where)
    {

    }
    /// <summary>
    /// ����ĳ������ʲô����
    /// </summary>
    /// <param name="who"></param>
    /// <param name="actorAction"></param>
    public virtual void ListenRoleDoSamething(ActorManager who, GameEvent.ActorAction actorAction)
    {

    }
    /// <summary>
    /// ����ĳ�˷���emoji
    /// </summary>
    /// <param name="actor"></param>
    /// <param name="id"></param>
    public virtual void ListenRoleSendEmoji(ActorManager actor,int id)
    {

    }
    #endregion
    /*����*/
    #region
    private float curPos_X;
    private float curPos_Y;
    private float rigSpeed;
    private float lastPos_X;
    private float lastPos_Y;
    private bool turnRight = false;
    private bool turnLeft = false;
    [HideInInspector]
    public Vector2 face;
    /*��鶯������״̬*/
    public virtual void UpdateAnima(float dt)
    {
        curPos_X = transform.position.x;
        curPos_Y = transform.position.y;
        rigSpeed = netManager.networkRigidbody.Rigidbody.velocity.magnitude;

        float distance = Vector2.Distance(new Vector2(curPos_X, curPos_Y), new Vector2(lastPos_X, lastPos_Y));
        float speed = distance / dt;
        if (rigSpeed <= 0.1f)
        {
            if (speed > 0.1f)
            {
                PlayMove(speed);
            }
            else
            {
                PlayStop(1);
            }
        }
        else
        {

        }

        if (curPos_X > lastPos_X)
        {
            if (!turnRight)
            {
                if (!isPlayer)
                {
                    FaceRight();
                }
                TurnRight();
            }
        }
        else
        {
            if (!turnLeft)
            {
                if (!isPlayer)
                {
                    FaceLeft();
                }
                TurnLeft();
            }
        }

        lastPos_X = curPos_X;
        lastPos_Y = curPos_Y;
    }
    /// <summary>
    /// ����ĳ��
    /// </summary>
    public virtual void FaceTo(Vector3 face)
    {
        this.face = new Vector2(face.x, face.y).normalized;
        if (face.x > 0)
        {
            FaceRight();
        }
        else
        {
            FaceLeft();
        }
    }
    /// <summary>
    /// �������
    /// </summary>
    public virtual void FaceLeft()
    {
        BodyController.Head.localScale = new Vector3(-1, 1, 1);
        BodyController.Body.localScale = new Vector3(-1, 1, 1);
        BodyController.Hand.localScale = new Vector3(-1, 1, 1);
    }
    /// <summary>
    /// �����ұ�
    /// </summary>
    public virtual void FaceRight()
    {
        BodyController.Head.localScale = new Vector3(1, 1, 1);
        BodyController.Body.localScale = new Vector3(1, 1, 1);
        BodyController.Hand.localScale = new Vector3(1, 1, 1);
    }
    /// <summary>
    /// ת��ĳ��
    /// </summary>
    /// <param name="turn"></param>
    public virtual void TurnTo(Vector3 turn)
    {
        if (turn.x > 0)
        {
            TurnRight();
        }
        if (turn.x < 0)
        {
            TurnLeft();
        }
    }
    /// <summary>
    /// ת�����
    /// </summary>
    public virtual void TurnLeft()
    {
        turnLeft = true;
        turnRight = false;
        BodyController.Leg.localScale = new Vector3(-1, 1, 1);
    }
    /// <summary>
    /// ת���ұ�
    /// </summary>
    public virtual void TurnRight()
    {
        turnRight = true;
        turnLeft = false;
        BodyController.Leg.localScale = new Vector3(1, 1, 1);
    }

    public virtual void PlayMove(float speed)
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
    public virtual void PlayStop(float speed)
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
    public virtual void PlayDead(float speed)
    {
        bodyController.SetHeadTrigger("Dead", 1, (str) =>
        {
            if (str == "Dead")
            {
                Dead();
            }
        });
        bodyController.SetBodyTrigger("Dead", 1, null);
        bodyController.SetHandTrigger("Dead", 1, null);
        bodyController.SetLegTrigger("Dead", 1, null);
        bodyController.locking = true;
    }
    public virtual void PlayTakeDamage(float speed)
    {
        bodyController.SetHeadTrigger("TakeDamage", 1, null);
    }
    public virtual void PlayPickUp(float speed, Action<string> action)
    {
        bodyController.SetHeadTrigger("LowerHead", 1, null);
        bodyController.SetHandTrigger("PickUp", 1, action);
    }
    #endregion
    /*λ��*/
    #region
    private List<MyTile> tempTiles = new List<MyTile>();
    private MyTile curTile;
    private MyTile lastTile;
    public MyTile GetMyTile()
    {
        if (navManager != null)
        {
            Vector3Int vector3Int = navManager.grid.WorldToCell(transform.position);
            return (MyTile)navManager.tilemap.GetTile(vector3Int);
        }
        else
        {
            return null;
        }
    }
    public MyTile GetMyTileWithOffset(Vector3Int offset)
    {
        if (navManager != null)
        {
            Vector3Int vector3Int = navManager.grid.WorldToCell(transform.position);
            return (MyTile)navManager.tilemap.GetTile(vector3Int + offset);
        }
        else
        {
            return null;
        }

    }
    public List<MyTile> GetNearbyTiles()
    {
        tempTiles.Clear();
        MyTile tile = GetMyTile();
        tempTiles.Add((MyTile)navManager.tilemap.GetTile(tile._posInCell));
        tempTiles.Add((MyTile)navManager.tilemap.GetTile(tile._posInCell + Vector3Int.right));
        tempTiles.Add((MyTile)navManager.tilemap.GetTile(tile._posInCell + Vector3Int.left));
        tempTiles.Add((MyTile)navManager.tilemap.GetTile(tile._posInCell + Vector3Int.up));
        tempTiles.Add((MyTile)navManager.tilemap.GetTile(tile._posInCell + Vector3Int.down));
        return tempTiles;
    }
    public void CheckMyTile()
    {
        curTile = GetMyTile();
        if (!curTile) { return; }
        if (lastTile == null) { lastTile = curTile; }
        if (lastTile._posInWorld != curTile._posInWorld)
        {
            ListenMyself_InNewTile();
        }
    }
    /// <summary>
    /// �����µؿ�
    /// </summary>
    public virtual void ListenMyself_InNewTile()
    {
        lastTile = curTile;
        MessageBroker.Default.Publish(new GameEvent.GameEvent_Local_SomeoneMove
        {
            moveActor = this,
            moveTile = curTile
        });
    }
    /// <summary>
    /// �����յ�
    /// </summary>
    public virtual void ListenMyself_InDestinationTile()
    {

    }
    #endregion
    /*��Ʒ*/
    #region
    /// <summary>
    /// �õ�����
    /// </summary>
    /// <param name="data"></param>
    /// <param name="showInUI"></param>
    public void AddItem_Hand(ItemData data)
    {
        Debug.Log("�õ�������" + data.Item_ID + "/" + data.Item_Val + "/" + data.Item_Count);
        /*����UI*/
        if (isPlayer && netManager.Object.HasInputAuthority)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateItemInHand()
            {
                itemData = data
            });
        }

        if (netManager.Last_ItemInHand.Item_ID != data.Item_ID)
        {
            ResetItem_Hand();
            if (data.Item_ID != 0)
            {
                Type type = Type.GetType("Item_" + data.Item_ID.ToString());
                holdingByHand = (ItemBase)Activator.CreateInstance(type);
                holdingByHand.UpdateData(data);
                holdingByHand.BeHolding(this, BodyController);
            }
        }
        else
        {
            if (holdingByHand != null)
            {
                holdingByHand.UpdateData(data);
            }
        }
        netManager.Last_ItemInHand = data;
    }
    public void ResetItem_Hand()
    {
        if (holdingByHand != null) { holdingByHand.BeRelease(this); }
        holdingByHand = new ItemBase();
        if (bodyController.Hand_LeftItem.childCount > 0)
        {
            for (int i = 0; i < bodyController.Hand_LeftItem.childCount; i++)
            {
                Destroy(bodyController.Hand_LeftItem.GetChild(i).gameObject);
            }
        }
        if (bodyController.Hand_RightItem.childCount > 0)
        {
            for (int i = 0; i < bodyController.Hand_RightItem.childCount; i++)
            {
                Destroy(bodyController.Hand_RightItem.GetChild(i).gameObject);
            }
        }

        bodyController.Hand_Left.GetComponent<SpriteRenderer>().enabled = true;
        bodyController.Hand_Right.GetComponent<SpriteRenderer>().enabled = true;

        bodyController.SetBodyBool("Idle", true, 1, null);
        bodyController.SetHeadBool("Idle", true, 1, null);
        bodyController.SetHandBool("Idle", true, 1, null);
        bodyController.SetHandBool("Idle", true, 1, null);

        bodyController.Hand_RightItem.localScale = Vector3.one;
        bodyController.Hand_LeftItem.localScale = Vector3.one;
        bodyController.Hand_RightItem.localScale = Vector3.one;
        bodyController.Hand_LeftItem.localPosition = Vector3.zero;
        bodyController.Hand_LeftItem.localRotation = Quaternion.identity;
        bodyController.Hand_RightItem.localPosition = Vector3.zero;
        bodyController.Hand_RightItem.localRotation = Quaternion.identity;
        bodyController.Hand_LeftItem.GetComponent<SpriteRenderer>().sprite
            = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_Default");
        bodyController.Hand_LeftItem.GetComponent<SpriteRenderer>().sortingOrder = 1;

        bodyController.Hand_RightItem.GetComponent<SpriteRenderer>().sprite
            = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_Default");
        bodyController.Hand_RightItem.GetComponent<SpriteRenderer>().sortingOrder = 4;
    }
    /// <summary>
    /// ����ͷ��
    /// </summary>
    /// <param name="data"></param>
    public void WearItem_Head(ItemData data)
    {
        Debug.Log("������ͷ��" + data.Item_ID + "/" + data.Item_Val + "/" + data.Item_Count);
        ResetItem_Head();
        /*����UI*/
        if (isPlayer && netManager.Object.HasInputAuthority)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateItemOnHead()
            {
                itemData = data
            });
        }
        if (data.Item_ID != 0)
        {
            Type type = Type.GetType("Item_" + data.Item_ID.ToString());
            wearingOnHead = (ItemBase)Activator.CreateInstance(type);
            wearingOnHead.UpdateData(data);
            wearingOnHead.BeWearingOnHead(this, BodyController);
        }
    }
    public void ResetItem_Head()
    {
        wearingOnHead = new ItemBase();
        if (bodyController.Head_Item.childCount > 0)
        {
            for (int i = 0; i < bodyController.Head_Item.childCount; i++)
            {
                Destroy(bodyController.Head_Item.GetChild(i).gameObject);
            }
        }
        bodyController.Head_Item.localScale = Vector3.one;
        bodyController.Head_Item.localPosition = new Vector3(0, 0.25f, 0);
        bodyController.Head_Item.localRotation = Quaternion.identity;
        bodyController.Head_Item.GetComponent<SpriteRenderer>().sprite
            = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_Default");
        bodyController.Head_Item.GetComponent<SpriteRenderer>().sortingOrder = 3;
    }
    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="data"></param>
    public void WearItem_Body(ItemData data)
    {
        Debug.Log("��������" + data.Item_ID + "/" + data.Item_Val + "/" + data.Item_Count);
        ResetItem_Body();
        /*����UI*/
        if (isPlayer && netManager.Object.HasInputAuthority)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateItemOnBody()
            {
                itemData = data
            });
        }
        if (data.Item_ID != 0)
        {
            Type type = Type.GetType("Item_" + data.Item_ID.ToString());
            wearingOnBody = (ItemBase)Activator.CreateInstance(type);
            wearingOnBody.UpdateData(data);
            wearingOnBody.BeWearingOnBody(this, BodyController);
        }
    }
    public void ResetItem_Body()
    {
        wearingOnHead = new ItemBase();
        if (bodyController.Body_Item.childCount > 0)
        {
            for (int i = 0; i < bodyController.Body_Item.childCount; i++)
            {
                Destroy(bodyController.Body_Item.GetChild(i).gameObject);
            }
        }
        bodyController.Body_Item.localScale = Vector3.one;
        bodyController.Body_Item.localPosition = Vector3.zero;
        bodyController.Body_Item.localRotation = Quaternion.identity;
        bodyController.Body_Item.GetComponent<SpriteRenderer>().sprite
            = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_Default");
        bodyController.Body_Item.GetComponent<SpriteRenderer>().sortingOrder = 2;
    }

    /// <summary>
    /// ����
    /// </summary>
    public virtual void Loot()
    {
        for (int i = 0; i < actorConfig.Config_LootCount; i++)
        {
            ItemData item = GetLootItem();
            if (item.Item_ID != 0)
            {
                item.Item_Seed = System.DateTime.Now.Second + item.Item_Seed;
                MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnItem()
                {
                    itemData = item,
                    pos = transform.position - new Vector3(0, 0.1f, 0)
                });
            }
        }
        for (int i = 0; i < netManager.Data_ItemInBag.Count; i++)
        {
            ItemData item = netManager.Data_ItemInBag[i];
            MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnItem()
            {
                itemData = item,
                pos = transform.position - new Vector3(0, 0.1f, 0)
            });
        }
        if (netManager.Data_ItemInHand.Item_ID != 0)
        {
            MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnItem()
            {
                itemData = netManager.Data_ItemInHand,
                pos = transform.position - new Vector3(0, 0.1f, 0)
            });
        }
    }
    /// <summary>
    /// ����Ȩ�ػ��һ��������id
    /// </summary>
    /// <returns></returns>
    private ItemData GetLootItem()
    {
        int weight_Main = 0;
        int weight_temp = 0;
        int random;
        for (int j = 0; j < actorConfig.Config_LootList.Count; j++)
        {
            weight_Main += actorConfig.Config_LootList[j].Weight;
        }
        random = UnityEngine.Random.Range(0, weight_Main);
        for (int j = 0; j < actorConfig.Config_LootList.Count; j++)
        {
            weight_temp += actorConfig.Config_LootList[j].Weight;
            if (weight_temp > random)
            {
                return actorConfig.Config_LootList[j].Item;
            }
        }
        return new ItemData();
    }
    private ItemData GetBagItem()
    {
        int weight_Main = 0;
        int weight_temp = 0;
        int random;
        for (int j = 0; j < actorConfig.Config_BagList.Count; j++)
        {
            weight_Main += actorConfig.Config_BagList[j].Weight;
        }
        random = UnityEngine.Random.Range(0, weight_Main);
        for (int j = 0; j < actorConfig.Config_BagList.Count; j++)
        {
            weight_temp += actorConfig.Config_BagList[j].Weight;
            if (weight_temp > random)
            {
                return actorConfig.Config_BagList[j].Item;
            }
        }
        return new ItemData();
    }

    #endregion
    /*Ѱ·*/
    #region
    public List<MyTile> tempPath = new List<MyTile>();
    public MyTile targetTile = null;
    public void UpdatePath(float dt)
    {
        if (targetTile)
        {
            if (Vector2.Distance(targetTile._posInWorld, transform.position) <= 0.1f)
            {
                /*����·���㣬���*/
                targetTile = null;
                if (tempPath.Count > 0)
                {
                    /*·����δ����*/
                    tempPath.RemoveAt(0);
                    if (tempPath.Count > 0)
                    {
                        targetTile = tempPath[0];
                    }
                }
                else
                {
                    ListenMyself_InDestinationTile();
                    /*·���Ѿ�����*/
                }
                CheckMyTile();
                return;
            }
            else
            {
                /*����·���㣬ǰ��·����*/

                Vector2 temp = Vector2.zero;
                if (transform.position.x > targetTile._posInWorld.x)
                {
                    if ((transform.position.x - targetTile._posInWorld.x) > 0.05f)
                    {
                        temp += new Vector2(-1, 0);
                    }
                }
                else
                {
                    if ((transform.position.x - targetTile._posInWorld.x) < -0.05f)
                    {
                        temp += new Vector2(1, 0);
                    }
                }
                if (transform.position.y > targetTile._posInWorld.y)
                {
                    if ((transform.position.y - targetTile._posInWorld.y) > 0.05f)
                    {
                        temp += new Vector2(0, -1);
                    }
                }
                else
                {
                    if ((transform.position.y - targetTile._posInWorld.y) < -0.05f)
                    {
                        temp += new Vector2(0, 1);
                    }
                }
                temp = temp.normalized;

                Vector2 velocity = new Vector2(temp.x * actorConfig.Config_Speed, temp.y * actorConfig.Config_Speed);
                Vector3 newPos = transform.position + new UnityEngine.Vector3(velocity.x * dt, velocity.y * dt, 0);
                netManager.UpdateNetworkTransform(newPos, velocity.magnitude);
            }
        }
    }
    /// <summary>
    /// �ҵ�һ��·��
    /// </summary>
    /// <param name="targetPos"></param>
    public virtual void FindWayToTarget(Vector2 targetPos)
    {
        tempPath = navManager.FindPath(navManager.FindTileByPos(targetPos), GetMyTile());
        if (tempPath != null)
        {
            /*����·���ƶ�ʱ��ֱ��������һ��·���㣬��ֹ����*/
            if (tempPath.Count > 1)
            {
                tempPath.RemoveAt(0);
            }
            targetTile = tempPath[0];
        }
    }
    public virtual void CalculatePath(Vector2 from, Vector2 to)
    {
        if (tempPath != null)
        {
            tempPath.Clear();
        }
        tempPath = navManager.FindPath(navManager.FindTileByPos(to), navManager.FindTileByPos(from));
    }
    #endregion
    /*�������Ա仯*/
    #region
    /// <summary>
    /// ����
    /// </summary>
    /// <param name="val"></param>
    /// <param name="id"></param>
    public void TakeDamage(int val,ActorNetManager from)
    {
        if (actorState != ActorState.Dead && from.HasInputAuthority)
        {
            netManager.RPC_HpChange(-val, from.Object.Id);
        }
    }
    public void TryToDead()
    {
        if (actorState != ActorState.Dead)
        {
            actorState = ActorState.Dead;
            PlayDead(1);
        }
    }
    /// <summary>
    /// ����
    /// </summary>
    public void Dead()
    {
        if (isState)
        {
            Loot();
            if (!isPlayer)
            {
                netManager.Runner.Despawn(netManager.Object);
            }
        }
    }

    #endregion
    /*RPC*/
    public bool attackState = false;
    public virtual void FromRPC_ChangeAttackState(bool state)
    {
        attackState = state;
    }
    protected ActorManager attackTarget;
    /// <summary>
    /// ���Ĺ���Ŀ��
    /// </summary>
    /// <param name="id"></param>
    public virtual void FromRPC_ChangeAttackTarget(Fusion.NetworkId id)
    {
        if (id != new NetworkId())
        {
            attackTarget = netManager.Runner.FindObject(id).GetComponent<ActorManager>();
        }
        else
        {
            attackTarget = null;
        }
    }
    public virtual void FromRPC_BindSkill(int skillID)
    {
        Type type = Type.GetType("Skill_" + skillID.ToString());
        bindSkill = (SkillBase)Activator.CreateInstance(type);
        bindSkill.Init(this);
    }
    public virtual void FromRPC_InvokeSkill(int parameter, Fusion.NetworkId id)
    {
        bindSkill.ClickSpace();
    }
    public virtual void Listen_HpChange(int parameter, Fusion.NetworkId id)
    {
        if (parameter < 0)
        {
            GameObject obj_num = UIManager.Instance.ShowUI("UI/UI_DamageNum", (Vector2)transform.position + new Vector2(0, 1));
            obj_num.GetComponent<UI_DamageNum>().Play(parameter, new Color32(255, 50, 50, 255));
            PlayTakeDamage(1);
        }
    }
    public virtual void FromRPC_SendEmoji(int emojiID)
    {
        MessageBroker.Default.Publish(new GameEvent.GameEvent_Local_SomeoneSendEmoji
        {
            actor = this,
            id = emojiID
        });
        ActorUI.SendEmoji(EmojiConfigData.GetItemConfig(emojiID));

    }
}
[Serializable]
public struct ActorConfig
{
    public float Config_Hp;
    public float Config_MaxHp;
    public float Config_Speed;
    public float Config_MaxSpeed;
    public float Config_Endurance;
    public int Config_LootCount;
    public List<LootInfo> Config_LootList;
    public int Config_BagCount;
    public List<LootInfo> Config_BagList;
}
public enum ActorState
{
    Default,
    Dead
}