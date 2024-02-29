using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Unity.VisualScripting;
using Vector3 = UnityEngine.Vector3;
using static UnityEditor.Progress;
using Fusion;
using static PlayerNetController;
/// <summary>
/// ��Ϊ������
/// </summary>
public class BaseBehaviorController : NetworkBehaviour
{
    [Header("���������")]
    public BaseBodyController bodyController;
    [Header("����ָʾ��")]
    public SI_Sector skillSector;
    [Header("�ֲ�������")]
    public ItemBase holdingByHand = new ItemBase();
    private Vector2 faceTo = Vector2.zero;
    [SerializeField]
    public RoleData Data = new RoleData();
    [HideInInspector]
    public bool isPlayer = false;
    private void Start()
    {
        Init();
    }
    public virtual void FixedUpdate()
    {
        if (tempPath != null)
        {
            MoveByPath(Time.fixedDeltaTime);
        }
        if (!Object.HasStateAuthority)
        {
            ClientMove();
        }
    }
    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
    }
    /*��ʼ*/
    #region
    /// <summary>
    /// ��ʼ��
    /// </summary>
    public virtual void Init()
    {
        navManager = GameObject.Find("Furniture").GetComponent<NavManager>();
        MessageBroker.Default.Receive<GameEvent.GameEvent_SomeoneMove>().Subscribe(_ =>
        {
            ListenRoleMove(_.moveRole, _.moveTile);
        }).AddTo(this);
    }
    #endregion
    /*�ƶ�*/
    #region
    [Header("������ڵ�")]
    public Transform Root;
    [Header("λ��ͬ�����")]
    public NetworkTransform NetTransform;

    [HideInInspector, Header("�ƶ�����")]
    public Vector2 tempVector;
    [HideInInspector, Header("��ǰλ��")]
    public MyTile curTile;
    [HideInInspector, Header("֮ǰλ��")]
    public MyTile lastTile;
    [HideInInspector, Header("����״̬")]
    public bool speedUp_State;
    [HideInInspector, Header("��������")]
    public float speedUp_Val = 2f;
    [HideInInspector, Header("��ʱ·��")]
    public List<MyTile> tempPathList = new List<MyTile>();
    [HideInInspector, Header("��һ��·����")]
    public MyTile tempPath = null;

    public virtual void InputMoveVector(UnityEngine.Vector2 vector2, float dt)
    {
        tempVector += vector2;
    }
    public virtual void InputFaceVector(Vector3 face)
    {
        faceTo = face;
        if (faceTo.x > 0)
        {
            FaceRight();
        }
        else
        {
            FaceLeft();
        }
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="up"></param>
    public virtual void SpeedUp(bool up)
    {
        speedUp_State = up;
    }
    /// <summary>
    /// ����·��
    /// </summary>
    public virtual void UpdatePath(List<MyTile> path)
    {
        tempPathList.Clear();
        for(int i = 1; i < path.Count; i++)
        {
            tempPathList.Add(path[i]);
            if (i == 1) { tempPath = tempPathList[0]; }
        }
    }
    /// <summary>
    /// ���շ����ƶ�
    /// </summary>
    /// <param name="dt"></param>
    public virtual void MoveByVector(float dt)
    {
        if (Object.HasStateAuthority)
        {
            StateMove(tempVector, Data.Data_Speed, dt);
        }
        if (tempVector != Vector2.zero)
        {
            PlayMove(Data.Data_Speed);
            PlayTurn(tempVector);
        }
        else
        {
            PlayStop(Data.Data_Speed);
        }
        CheckMyPos();        
        tempVector = Vector3.zero;
    }
    /// <summary>
    /// ����·���ƶ�
    /// </summary>
    public virtual void MoveByPath(float dt)
    {
        /*����·����*/
        if (Vector2.Distance(tempPath.pos,transform.position) <= 0.1f)
        {
            PlayStop(Data.Data_Speed);
            tempPath = null;
            if (tempPathList.Count > 0)
            {
                tempPathList.RemoveAt(0);
                if(tempPathList.Count > 0)
                {
                    tempPath = tempPathList[0];
                }
            }
            CheckMyPos();
            return;
        }
        else
        {
            Vector2 temp = Vector2.zero;
            if (transform.position.x > tempPath.x)
            {
                if((transform.position.x - tempPath.x) > 0.05f)
                {
                    temp += new Vector2(-1, 0);
                }
            }
            else
            {
                if ((transform.position.x - tempPath.x) < -0.05f)
                {
                    temp += new Vector2(1, 0);
                }
            }
            if(transform.position.y > tempPath.y)
            {
                if ((transform.position.y - tempPath.y) > 0.05f)
                {
                    temp += new Vector2(0, -1);
                }
            }
            else
            {
                if ((transform.position.y - tempPath.y) < -0.05f)
                {
                    temp += new Vector2(0, 1);
                }
            }
            if (Object.HasStateAuthority)
            {
                StateMove(temp, Data.Data_Speed, dt);
            }
            PlayTurn(temp);
            PlayMove(Data.Data_Speed);
            if (Vector2.Distance(tempPath.pos, transform.position) <= 0.1f)
            {
                PlayStop(Data.Data_Speed);
                CheckMyPos();
            }
        }
    }
    private Vector3 newPositon;
    /// <summary>
    /// �ƶ�
    /// </summary>
    private void StateMove(Vector2 dir, float speed, float dt)
    {
        dir = dir.normalized;
        if (speedUp_State) { speed *= speedUp_Val; }
        statePos = transform.position;
        transform.position =
            transform.position + new UnityEngine.Vector3(dir.x * speed * dt, dir.y * speed * dt, 0);
    }
    private void ClientMove()
    {
        Debug.Log(statePos);
        transform.position = statePos;
    }
    /// <summary>
    /// �ƶ�����
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="speed"></param>
    /// <param name="dt"></param>
    public void PlayMove(float speed)
    {
        bodyController.PlayBodyAction(BodyAction.Move, speed, null);
        bodyController.PlayHeadAction(HeadAction.Move, speed, null);
        bodyController.PlayLegAction(LegAction.Step, speed);
        bodyController.PlayHandAction(HandAction.Step, speed, null);
    }
    /// <summary>
    /// ת�򶯻�
    /// </summary>
    /// <param name="dir"></param>
    public void PlayTurn(Vector2 dir)
    {
        if (dir.x > 0)
        {
            TurnRight();
        }
        if (dir.x < 0)
        {
            TurnLeft();
        }
    }
    /// <summary>
    /// ֹͣ����
    /// </summary>
    public void PlayStop(float speed)
    {
        bodyController.PlayBodyAction(BodyAction.Idle, speed, null);
        bodyController.PlayHeadAction(HeadAction.Idle, speed, null);
        bodyController.PlayLegAction(LegAction.Idle, speed);
        bodyController.PlayHandAction(HandAction.Idle, speed, null);
    }
    /// <summary>
    /// ����ҵ�λ��
    /// </summary>
    private void CheckMyPos()
    {
        curTile = GetMyTile();
        if (!curTile) { return; }
        if (lastTile == null) { lastTile = curTile; }
        if (lastTile.pos != curTile.pos)
        {
            lastTile = curTile;
            MessageBroker.Default.Publish(new GameEvent.GameEvent_SomeoneMove
            {
                moveRole = this,
                moveTile = curTile
            });
        }
    }
    #endregion
    /*Ѱ·*/
    #region
    public List<MyTile> myLoad = new List<MyTile>();
    public NavManager navManager;
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
    List<MyTile> tempTiles = new List<MyTile>();
    public List<MyTile> GetNearbyTile()
    {
        tempTiles.Clear();
        MyTile tile = GetMyTile();

        tempTiles.Add((MyTile)navManager.tilemap.GetTile(new Vector3Int(tile.x, tile.y, 0)));
        tempTiles.Add((MyTile)navManager.tilemap.GetTile(new Vector3Int(tile.x + 1, tile.y, 0)));
        tempTiles.Add((MyTile)navManager.tilemap.GetTile(new Vector3Int(tile.x - 1, tile.y, 0)));
        tempTiles.Add((MyTile)navManager.tilemap.GetTile(new Vector3Int(tile.x, tile.y + 1, 0)));
        tempTiles.Add((MyTile)navManager.tilemap.GetTile(new Vector3Int(tile.x, tile.y - 1, 0)));
        return tempTiles;
    }
    public Vector2 GetMyPos()
    {
        int X = (int)(transform.position.x);
        int Y = (int)(transform.position.y);
        return new Vector2(X,Y);
    }
    #endregion
    /*����*/
    #region
    /// <summary>
    /// ʱ�����
    /// </summary>
    public virtual void ListenTimeUpdate()
    {

    }
    /// <summary>
    /// λ�ø���
    /// </summary>
    /// <param name="who"></param>
    /// <param name="where"></param>
    public virtual void ListenRoleMove(BaseBehaviorController who,MyTile where)
    {
        
    }
    #endregion
    /*��Ϊ*/
    #region
    public void PickUpItem_Bag(ItemObj itemObj)
    {
        bodyController.PlayHeadAction( HeadAction.LowerHead, 1, null);
        bodyController.PlayHandAction( HandAction.PickUp,1,
            (string x) => 
            {
                Debug.Log("OK"+x);
                if (x == "PickUp")
                {
                    itemObj.PickUp(this);
                }
            });
    }
    public void AddItem_Hand(ItemConfig config,bool showInUI)
    {
        Type type = Type.GetType("Item_" + config.Item_ID.ToString());
        holdingByHand = (ItemBase)Activator.CreateInstance(type);
        holdingByHand.Init(config);
        holdingByHand.BeHolding(this, bodyController.Hand_Right);
        /*����UI*/
        if (showInUI)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_AddItemInHand()
            {
                itemConfig = config
            });
        }
    }
    public void UseItem_Hand(ItemConfig config)
    {
        
    }
    public void SubItem_Hand(ItemConfig config)
    {

    }
    public void AddItem_Bag(ItemConfig config,bool showInUI)
    {
        /*����������������Ϊ��*/
        if(config.Item_CurCount <= 0) { config.Item_CurCount = 1; }
        /*��������*/
        for(int i = 0; i < Data.Holding_BagList.Count; i++)
        {
            /*��鱳������ͬ������*/
            if (Data.Holding_BagList[i].Item_ID == config.Item_ID)
            {
                /*��鱳�����ͬ�������Ƿ�ﵽ�ѵ����ֵ*/
                if (Data.Holding_BagList[i].Item_CurCount < Data.Holding_BagList[i].Item_MaxCount)
                {
                    ItemConfig tempItem = Data.Holding_BagList[i];
                    /*ʣ�����ֵ�����������ĵ���*/
                    if (Data.Holding_BagList[i].Item_MaxCount - Data.Holding_BagList[i].Item_CurCount < config.Item_CurCount)
                    {
                        tempItem.Item_CurCount = tempItem.Item_MaxCount;
                        config.Item_CurCount -= (Data.Holding_BagList[i].Item_MaxCount - Data.Holding_BagList[i].Item_CurCount);
                        Data.Holding_BagList[i] = tempItem;
                    }
                    /*ʣ�����ֵ���������ĵ���*/
                    else
                    {
                        tempItem.Item_CurCount += config.Item_CurCount;
                        config.Item_CurCount = 0; 
                        Data.Holding_BagList[i] = tempItem;
                    }
                }
                else
                {
                    continue;
                }
            }
        }
        /*����һ��֮����ʣ��*/
        if(config.Item_CurCount > 0)
        {
            Data.Holding_BagList.Add(config);
        }
        /*����UI*/
        if (showInUI)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateItemInBag()
            {
                itemConfigs = Data.Holding_BagList
            });
        }
    }
    public void SubItem_Bag(ItemConfig config)
    {
        Data.Holding_BagList.Remove(config);
    }
    public virtual void RunAway()
    {

    }
    public virtual void RunTo()
    {

    }
    public virtual void TurnLeft()
    {
        bodyController.Leg.localScale = new Vector3(-1, 1, 1);
    }
    public virtual void FaceLeft()
    {
        bodyController.Head.localScale = new Vector3(-1, 1, 1);
        bodyController.Body.localScale = new Vector3(-1, 1, 1);
        bodyController.Hand.localScale = new Vector3(-1, 1, 1);
    }
    public virtual void TurnRight()
    {
        bodyController.Leg.localScale = new Vector3(1, 1, 1);
    }
    public virtual void FaceRight()
    {
        bodyController.Head.localScale = new Vector3(1, 1, 1);
        bodyController.Body.localScale = new Vector3(1, 1, 1);
        bodyController.Hand.localScale = new Vector3(1, 1, 1);
    }

    #endregion
    /*����ͬ����Ϊ*/
    #region
    [Networked]
    public Vector2 statePos { get; set; } 

    public void TryToFindPathByRPC(Vector2 to, Vector2 from)
    {
        if (Object.HasStateAuthority)
        {
            RPC_FindPath(to, from);
        }
    }
    /// <summary>
    /// Ѱ��·��
    /// </summary>
    /// <param name="to"></param>
    /// <param name="from"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPC_FindPath(Vector2 to, Vector2 from)
    {
        if (myLoad != null)
        {
            myLoad.Clear();
        }
        myLoad = navManager.FindPath(navManager.FindTileByPos(to), navManager.FindTileByPos(from));
        UpdatePath(myLoad);
    }
    public void TryToTakeDamage(int damage)
    {
        RPC_TakeDamage(damage);
    }
    /// <summary>
    /// �ܵ��˺�
    /// </summary>
    /// <param name="damage"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_TakeDamage(int damage)
    {
        GameObject obj_num = UIManager.Instance.ShowUI("UI/UI_DamageNum", (Vector2)transform.position+new Vector2(0,1));
        obj_num.GetComponent<UI_DamageNum>().Play(damage, new Color32(255, 50, 50, 255));

        if (Data.Data_Hp >= damage)
        {
            Data.Data_Hp -= damage;
            bodyController.PlayHeadAction(HeadAction.TakeDamage,1,null);
        }
        else
        {
            Data.Data_Hp -= damage;
            bodyController.PlayHeadAction(HeadAction.TakeDamage, 1, null);
        }
    }
    #endregion
}
