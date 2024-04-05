using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Windows;
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
        MessageBroker.Default.Receive<GameEvent.GameEvent_SomeoneMove>().Subscribe(_ =>
        {
            if (_.moveActor == actorManager)
            {
                UpdateNearByTile();
            }
        }).AddTo(this);
        actorManager.InitByPlayer();
    }

    #region//玩家输入
    private int lastLeftClickTime = 0;
    private int lastRightClickTime = 0;
    private int lastQClickTime = 0;
    private int lastFClickTime = 0;
    private int lastSpaceClickTime = 0;
    /// <summary>
    /// 鼠标输入
    /// </summary>
    /// <param name="leftClick"></param>
    /// <param name="rightClick"></param>
    /// <param name="leftPress"></param>
    /// <param name="rightPress"></param>
    public void All_PlayerInputMouse(float dt, int leftClickTime,int rightClickTime,bool leftPress,bool rightPress, bool hasStateAuthority, bool hasInputAuthority)
    {
        if (lastLeftClickTime != leftClickTime) 
        {
            lastLeftClickTime = leftClickTime;
            actorManager.holdingByHand.ClickLeftClick(dt, hasInputAuthority); 
        }
        if (lastRightClickTime != rightClickTime) 
        {
            lastRightClickTime = rightClickTime;
            actorManager.holdingByHand.ClickRightClick(dt, hasInputAuthority); 
        }
        if (leftPress) { actorManager.holdingByHand.PressLeftClick(dt, hasInputAuthority); }
        else { actorManager.holdingByHand.ReleaseLeftClick(dt, hasInputAuthority); }
        if (rightPress) { actorManager.holdingByHand.PressRightClick(dt, hasInputAuthority); }
        else { actorManager.holdingByHand.ReleaseRightClick(dt, hasInputAuthority); }
    }
    /// <summary>
    /// 位移输入
    /// </summary>
    /// <param name="dir">方向</param>
    /// <param name="deltaTime">间隔</param>
    /// <param name="speedUp">加速</param>
    public void All_PlayerInputMove(float deltaTime, Vector2 dir, bool speedUp, bool hasStateAuthority, bool hasInputAuthority)
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
        actorManager.holdingByHand.MousePosition(dir, dt);
    }
    /// <summary>
    /// 操作输入
    /// </summary>
    /// <param name="clickQ">Q</param>
    /// <param name="clickF">F</param>
    /// <param name="clickSpace">Space</param>
    public void All_PlayerInputControl(int clickQ,int clickF,int clickSpace, bool hasStateAuthority, bool hasInputAuthority)
    {
        if (hasInputAuthority)
        {
            if (lastQClickTime != clickQ)
            {
                lastQClickTime = clickQ;
                if (actorManager.NetController.Data_ItemInBag.Count > 0)
                {
                    holdingIndex++;
                    if (holdingIndex >= actorManager.NetController.Data_ItemInBag.Count)
                    {
                        holdingIndex = 0;
                    }
                    MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_AddItemInHand
                    {
                        networkItemConfig = actorManager.NetController.Data_ItemInBag[holdingIndex]
                    }) ;
                }
            }
        }
        if (lastFClickTime != clickF)
        {
            lastFClickTime = clickF;
            for (int i = 0; i < nearbyTiles.Count; i++)
            {
                MessageBroker.Default.Publish(new MapEvent.MapEvent_InvokeTile
                {
                    tilePos = new Vector3Int(nearbyTiles[i].x, nearbyTiles[i].y, 0),
                    hasInput = hasInputAuthority,
                    hasState = hasStateAuthority,
                });
                nearbyTiles[i].InvokeTile();
            }
        }
        if (lastSpaceClickTime != clickSpace)
        {
            lastSpaceClickTime = clickSpace;
            var item = Physics2D.OverlapCircleAll(transform.position, 0.5f, itemLayer);
            if (item.Length > 0)
            {
                if (item[0].gameObject.TryGetComponent(out ItemObj obj))
                {
                    actorManager.NetController.Data_ItemInBag.Add(obj.netConfig);
                }
            }
        }
    }

    #endregion
    #region//玩家周围地块更新
    public List<MyTile> nearbyTiles = new List<MyTile>();
    private List<MyTile> tempTiles = new List<MyTile>();
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
