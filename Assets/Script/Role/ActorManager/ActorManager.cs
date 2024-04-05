using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
/// <summary>
/// ��ɫ������
/// </summary>
public class ActorManager : MonoBehaviour
{
    [SerializeField, Header("��ɫ��������")]
    public ActorConfig actorConfig;
    [SerializeField, Header("���������")]
    private ActorNetManager netController;
    public ActorNetManager NetController 
    {
        get { return netController; }
        set { netController = value; }
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
    public ActorUI ActorUI
    {
        get { return actorUI; }
        set { actorUI = value; }
    }
    [HideInInspector]
    public ItemBase holdingByHand = new ItemBase();
    [HideInInspector]
    public NavManager navManager;

    protected bool isPlayer = false;
    protected bool isState = false;
    public virtual void FixedUpdateNetwork(float dt)
    {
        UpdateAnima(dt);
        UpdatePath(dt);
    }
    public virtual void CustomUpdate()
    {
        CheckMyTile();
    }
    public void InitByNetManager(bool hasStateAuthority)
    {
        isState = hasStateAuthority;
        InvokeRepeating("CustomUpdate", 1f, 0.5f);
        navManager = GameObject.Find("Furniture").GetComponent<NavManager>();
        MessageBroker.Default.Receive<GameEvent.GameEvent_SomeoneMove>().Subscribe(_ =>
        {
            ListenRoleMove(_.moveActor, _.moveTile);
        }).AddTo(this);
    }
    public void InitByPlayer()
    {
        isPlayer = true;
    }
    /*����*/
    #region
    public virtual void ListenRoleMove(ActorManager who, MyTile where)
    {

    }
    #endregion
    /*����*/
    #region
    private float curPos_X;
    private float curPos_Y;
    private float lastPos_X;
    private float lastPos_Y;
    private bool turnRight = false;
    private bool turnLeft = false;

    public virtual void UpdateAnima(float dt)
    {
        curPos_X = transform.position.x;
        curPos_Y = transform.position.y;
        float distance = Vector2.Distance(new Vector2(curPos_X, curPos_Y), new Vector2(lastPos_X, lastPos_Y));
        float speed = distance / dt;
        if (speed > 0.1f)
        {
            PlayMove(1);
        }
        else
        {
            PlayStop(1);
        }
        if (curPos_X > lastPos_X)
        {
            if (!turnRight) { TurnRight(); }
        }
        else
        {
            if (!turnLeft) { TurnLeft(); }
        }
        lastPos_X = curPos_X;
        lastPos_Y = curPos_Y;
    }

    /// <summary>
    /// ����ĳ��
    /// </summary>
    public virtual void FaceTo(Vector3 face)
    {
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
        bodyController.PlayBodyAction(BodyAction.Move, speed, null);
        bodyController.PlayHeadAction(HeadAction.Move, speed, null);
        bodyController.PlayLegAction(LegAction.Step, speed, null);
        bodyController.PlayHandAction(HandAction.Step, speed, null);
    }
    public virtual void PlayStop(float speed)
    {
        bodyController.PlayBodyAction(BodyAction.Idle, speed, null);
        bodyController.PlayHeadAction(HeadAction.Idle, speed, null);
        bodyController.PlayLegAction(LegAction.Idle, speed, null);
        bodyController.PlayHandAction(HandAction.Idle, speed, null);
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
    public void CheckMyTile()
    {
        curTile = GetMyTile();
        if (!curTile) { return; }
        if (lastTile == null) { lastTile = curTile; }
        if (lastTile.pos != curTile.pos)
        {
            InNewTile();
        }
    }
    /// <summary>
    /// �����µؿ�
    /// </summary>
    public virtual void InNewTile()
    {
        lastTile = curTile;
        MessageBroker.Default.Publish(new GameEvent.GameEvent_SomeoneMove
        {
            moveActor = this,
            moveTile = curTile
        });
    }
    #endregion
    /*��Ʒ*/
    #region
    public void AddItem_Hand(NetworkItemConfig config, bool showInUI)
    {
        Type type = Type.GetType("Item_" + config.Item_ID.ToString());
        holdingByHand = (ItemBase)Activator.CreateInstance(type);
        holdingByHand.Init(config);
        holdingByHand.BeHolding(this, BodyController.Hand_RightItem);
        /*����UI*/
        if (showInUI)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateItemInHand()
            {
                networkItemConfig = config
            });
        }
    }
    public void PickUpItem_Bag(ItemObj itemObj)
    {
        BodyController.PlayHeadAction(HeadAction.LowerHead, 1, null);
        BodyController.PlayHandAction(HandAction.PickUp, 1,
            (string x) =>
            {
                Debug.Log("OK" + x);
                if (x == "PickUp")
                {
                    itemObj.PickUp(this);
                }
            });
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
            /*����·���㣬���*/
            if (Vector2.Distance(targetTile.pos, transform.position) <= 0.1f)
            {
                targetTile = null;
                /*���·����δ����*/
                if (tempPath.Count > 0)
                {
                    tempPath.RemoveAt(0);
                    if (tempPath.Count > 0)
                    {
                        targetTile = tempPath[0];
                    }
                }
                CheckMyTile();
                return;
            }
            /*����·���㣬ǰ��·����*/
            else
            {
                Vector2 temp = Vector2.zero;
                if (transform.position.x > targetTile.pos.x)
                {
                    if ((transform.position.x - targetTile.pos.x) > 0.05f)
                    {
                        temp += new Vector2(-1, 0);
                    }
                }
                else
                {
                    if ((transform.position.x - targetTile.pos.x) < -0.05f)
                    {
                        temp += new Vector2(1, 0);
                    }
                }
                if (transform.position.y > targetTile.pos.y)
                {
                    if ((transform.position.y - targetTile.pos.y) > 0.05f)
                    {
                        temp += new Vector2(0, -1);
                    }
                }
                else
                {
                    if ((transform.position.y - targetTile.pos.y) < -0.05f)
                    {
                        temp += new Vector2(0, 1);
                    }
                }
                temp = temp.normalized;

                netController.UpdateNetworkTransform(transform.position + new UnityEngine.Vector3(temp.x * dt * actorConfig.Config_Speed, temp.y * dt * actorConfig.Config_Speed, 0));
            }
        }
    }
    public virtual void SetTargetTile(Vector2 targetPos)
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
            if (targetTile.pos.x >= transform.position.x)
            {
                FaceRight();
            }
            else
            {
                FaceLeft();
            }
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
}
[Serializable]
public struct ActorConfig
{
    public float Config_Speed;
}