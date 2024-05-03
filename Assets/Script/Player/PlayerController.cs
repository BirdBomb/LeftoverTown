using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Windows;
using static UnityEditor.Progress;
using Input = UnityEngine.Input;
/// <summary>
/// 玩家控制器
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("人物")]
    public ActorManager actorManager;
    [Header("物理")]
    public Rigidbody2D myRigidbody;
    [Header("物体层级")]
    public LayerMask itemLayer;
    /// <summary>
    /// 当前持握的物体编号
    /// </summary>
    private int holdingIndex = 0;
    [HideInInspector]
    public bool thisPlayerIsMe = false;
    [HideInInspector]
    public bool thisPlayerIsState = false;
    [HideInInspector]
    public int thisPlayerID = 0;
    private void Awake()
    {
        myRigidbody.gravityScale = 0;
    }
    private void Start()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_Local_SomeoneMove>().Subscribe(_ =>
        {
            if (_.moveActor == actorManager && thisPlayerIsMe)
            {
                UpdateNearByTile();
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TryRemoveItemFromBag>().Subscribe(_ =>
        {
            if (thisPlayerIsMe)
            {
                actorManager.NetController.RPC_Local_LoseItem(_.item);
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TryAddItemInBag>().Subscribe(_ =>
        {
            if (thisPlayerIsMe)
            {
                actorManager.NetController.RPC_AddItemInBag(_.item);
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TryDropItem>().Subscribe(_ =>
        {
            if (thisPlayerIsMe)
            {
                MessageBroker.Default.Publish(new GameEvent.GameEvent_Local_SpawnItem()
                {
                    itemData = _.item,
                    pos = transform.position - new Vector3(0, 0.1f, 0)
                });
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TrySpawnActor>().Subscribe(_ =>
        {
            if (thisPlayerIsMe)
            {
                MessageBroker.Default.Publish(new GameEvent.GameEvent_Local_SpawnActor()
                {
                    name = _.name,
                    pos = transform.position
                });
            }
        }).AddTo(this);
        actorManager.InitByPlayer();
        itemLayer = LayerMask.GetMask("ItemObj");
    }
    private void Update()
    {
        if (thisPlayerIsMe)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (actorManager.NetController.Data_ItemInBag.Count > 0)
                {
                    holdingIndex++;
                    if (holdingIndex >= actorManager.NetController.Data_ItemInBag.Count)
                    {
                        holdingIndex = 0;
                    }
                    actorManager.NetController.RPC_Local_HoldItem(actorManager.NetController.Data_ItemInBag[holdingIndex]);
                }
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                var item = Physics2D.OverlapCircleAll(transform.position, 0.5f, itemLayer);
                if (item.Length <= 0) { return; }
                if (item[0].gameObject.transform.parent.TryGetComponent(out ItemNetObj obj)) 
                {
                    actorManager.NetController.RPC_Local_PickItem(obj.Object.Id);
                }
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                for (int i = 0; i < nearbyTiles.Count; i++)
                {
                    nearbyTiles[i].InvokeTile();
                }
            }
        }
    }
    #region//玩家输入
    /// <summary>
    /// 左键点击次数
    /// </summary>
    private int lastLeftClickTime = 0;
    /// <summary>
    /// 左键按下
    /// </summary>
    private bool lastLeftPress = false;
    /// <summary>
    /// 右键点击次数
    /// </summary>
    private int lastRightClickTime = 0;
    /// <summary>
    /// 右键按下
    /// </summary>
    private bool lastRightPress = false;

    /// <summary>
    /// 鼠标输入
    /// </summary>
    /// <param name="leftClickTime">左键点击次数</param>
    /// <param name="rightClickTime">右键点击次数</param>
    /// <param name="leftPress">左键长按</param>
    /// <param name="rightPress">右键长按</param>
    public void All_PlayerInputMouse(float dt, int leftClickTime,int rightClickTime,bool leftPress,bool rightPress, bool hasStateAuthority, bool hasInputAuthority)
    {
        if (lastLeftClickTime != leftClickTime) 
        {
            lastLeftClickTime = leftClickTime;
            actorManager.holdingByHand.ClickLeftClick(dt, hasStateAuthority, hasInputAuthority, hasInputAuthority);
        }
        if (lastRightClickTime != rightClickTime) 
        {
            lastRightClickTime = rightClickTime;
            actorManager.holdingByHand.ClickRightClick(dt, hasStateAuthority, hasInputAuthority, hasInputAuthority);
        }
        if (leftPress) 
        {
            lastLeftPress = leftPress;
            actorManager.holdingByHand.PressLeftClick(dt, hasStateAuthority, hasInputAuthority, hasInputAuthority);
        }
        else if(lastLeftPress)
        {
            lastLeftPress = leftPress;
            actorManager.holdingByHand.ReleaseLeftClick(dt, hasStateAuthority, hasInputAuthority, hasInputAuthority);
        }
        if (rightPress) 
        {
            lastRightPress = rightPress;
            actorManager.holdingByHand.PressRightClick(dt, hasStateAuthority, hasInputAuthority, hasInputAuthority);
        }
        else if (lastRightPress)
        {
            lastRightPress = rightPress;
            actorManager.holdingByHand.ReleaseRightClick(dt, hasStateAuthority, hasInputAuthority, hasInputAuthority);
        }
    }
    /// <summary>
    /// 位移输入
    /// </summary>
    /// <param name="dir">方向</param>
    /// <param name="speedUp">加速</param>
    public void State_PlayerInputMove(float deltaTime, Vector2 dir, bool speedUp, bool hasStateAuthority, bool hasInputAuthority)
    {
        dir = dir.normalized;
        float speed;
        if (speedUp) 
        {
            speed = actorManager.actorConfig.Config_Speed * 2;
        }
        else
        {
            speed = actorManager.actorConfig.Config_Speed;
        }
        Vector3 newPos = transform.position + new UnityEngine.Vector3(dir.x * speed * deltaTime, dir.y * speed * deltaTime, 0);
        actorManager.NetController.UpdateNetworkTransform(newPos);
    }
    /// <summary>
    /// 朝向输入
    /// </summary>
    /// <param name="dir">方向</param>
    public void All_PlayerInputFace(float dt, Vector2 dir, bool hasStateAuthority, bool hasInputAuthority)
    {
        actorManager.FaceTo(dir);
        actorManager.holdingByHand.FaceTo(dir, dt);
    }

    #endregion
    #region
    public List<MyTile> nearbyTiles = new List<MyTile>();
    private List<MyTile> tempTiles = new List<MyTile>();
    /// <summary>
    /// 跟新附近的地块
    /// </summary>
    private void UpdateNearByTile()
    {
        /*周围地块*/
        tempTiles = actorManager.GetNearbyTile();
        /*剔除上次检测的地块*/
        for (int i = 0; i < nearbyTiles.Count; i++)
        {
            if (nearbyTiles[i] == null) { continue; }
            if (!tempTiles.Contains(nearbyTiles[i]))
            {
                nearbyTiles[i].FarawayTileByPlayer(this);
                nearbyTiles.RemoveAt(i);
            }
        }
        /*添加本次加入的地块*/
        for (int i = 0; i < tempTiles.Count; i++)
        {
            if (tempTiles[i] == null) { continue; }
            if (tempTiles[i].NearbyTileByPlayer(this))
            {
                if (!nearbyTiles.Contains(tempTiles[i]))
                {
                    nearbyTiles.Add(tempTiles[i]);
                }
            }
        }
    }
    #endregion
}
