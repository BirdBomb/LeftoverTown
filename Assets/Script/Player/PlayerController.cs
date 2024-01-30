using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using static Fusion.Allocator;
/// <summary>
/// 玩家控制器
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("行为控制器")]
    public BaseBehaviorController baseBehaviorController;
    [Header("物理")]
    public Rigidbody2D myRigidbody;
    /// <summary>
    /// 当前持握的物体编号
    /// </summary>
    private int holdingIndex = 0;
    private void Awake()
    {
        myRigidbody.gravityScale = 0;
    }
    private void Start()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_SomeoneMove>().Subscribe(_ =>
        {
            if (_.moveRole == baseBehaviorController)
            {
                ClearAround();
                CheckAround();
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_AddItemInBag>().Subscribe(_ =>
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_UI_AddItemInBag()
            {
                itemConfig = _.itemConfig
            });
            baseBehaviorController.AddItem(_.itemConfig);
        }).AddTo(this);
    }
    private void Update()
    {
        PlayerInput();
    }
    private void LateUpdate()
    {
        baseBehaviorController.MoveByVector(Time.deltaTime);
    }
    private void PlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            baseBehaviorController.holdingByHand.ClickLeftBtn();

        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            baseBehaviorController.holdingByHand.ClickRightBtn();
        }
        if (Input.GetKey(KeyCode.Mouse1))
        {
            baseBehaviorController.holdingByHand.PressRightBtn();
        }
        else
        {
            baseBehaviorController.holdingByHand.ReleaseRightBtn();
        }

        #region//背包操作
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (baseBehaviorController.Data.Holding_BagList.Count > 0)
            {
                holdingIndex++;
                if (holdingIndex >= baseBehaviorController.Data.Holding_BagList.Count)
                {
                    holdingIndex = 0;
                }
                baseBehaviorController.HoldItem(baseBehaviorController.Data.Holding_BagList[holdingIndex]);
                MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_AddItemInHand
                {
                    itemConfig = baseBehaviorController.Data.Holding_BagList[holdingIndex]
                });
                MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_UI_AddItemInHand()
                {
                    itemConfig = baseBehaviorController.Data.Holding_BagList[holdingIndex]
                });
            }
            //GameObject obj = Resources.Load<GameObject>("ItemObj/ItemObj");
            //GameObject item = Instantiate(obj);
            //item.GetComponent<ItemObj>().Init(ItemConfigData.itemConfigs[0]);
            //item.transform.position = transform.position;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            for (int i = 0; i < ListenTiles.Count; i++)
            {
                ListenTiles[i].Interactive();
            }

        }
        #endregion
        #region//位移操作
        if (Input.GetKey(KeyCode.A))
        {
            baseBehaviorController.TurnLeft();
            baseBehaviorController.InputMoveVector(Vector2.left, Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            baseBehaviorController.TurnRight();
            baseBehaviorController.InputMoveVector(Vector2.right, Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            baseBehaviorController.InputMoveVector(Vector2.down, Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.W))
        {
            baseBehaviorController.InputMoveVector(Vector2.up, Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            baseBehaviorController.SpeedUp(true);
        }
        else
        {
            baseBehaviorController.SpeedUp(false);
        }
        #endregion
        if (Input.GetKey(KeyCode.M))
        {
            MessageBroker.Default.Publish(new MapEvent.MapEvent_SaveMap
            {
                
            });
        }
    }
    public List<MyTile> ListenTiles = new List<MyTile>();
    private void CheckAround()
    {

        List<MyTile> tiles = baseBehaviorController.GetNearbyTile();
        for (int i = 0; i < tiles.Count; i++)
        {
            if (tiles[i] == null) continue;

            if (tiles[i].interactiveType == TileInteractiveType.Cabinet)
            {
                MyTile tile = tiles[i].ShowSignal();
                if (!ListenTiles.Contains(tile))
                {
                    ListenTiles.Add(tile);
                }
            }
        }

    }
    private void ClearAround()
    {
        for (int i = 0; i < ListenTiles.Count; i++)
        {
            ListenTiles[i].HideSignal();
        }
        ListenTiles.Clear();
    }
}
