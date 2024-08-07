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
    [Header("物体层级")]
    public LayerMask itemLayer;
    [HideInInspector]
    public PlayerData localPlayerData;
    [HideInInspector]
    public bool thisPlayerIsMe = false;
    [HideInInspector]
    public bool thisPlayerIsState = false;
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
            if (thisPlayerIsMe && actorManager.NetManager.Object.HasInputAuthority)
            {
                actorManager.NetManager.RPC_LocalInput_LoseItem(_.item);
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TryAddItemInBag>().Subscribe(_ =>
        {
            if (thisPlayerIsMe)
            {
                actorManager.NetManager.CalculateBag(_.item, out ItemData itemResidue);
                actorManager.NetManager.RPC_LocalInput_AddItemInBag(_.item);
                if (_.itemResidueBack != null)
                {
                    _.itemResidueBack.Invoke(itemResidue);
                }
                if (itemResidue.Item_Count != 0)//背包溢出
                {
                    MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryDropItem()
                    {
                        item = itemResidue
                    });
                }
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TryChangeItemInBag>().Subscribe(_ =>
        {
            if (thisPlayerIsMe)
            {
                if(_.oldItem.Item_ID == actorManager.NetManager.Data_ItemInHand.Item_ID&& _.oldItem.Item_Seed == actorManager.NetManager.Data_ItemInHand.Item_Seed)
                {
                    actorManager.NetManager.RPC_LocalInput_ChangeItemInHand(_.oldItem, _.newItem);
                }
                else
                {
                    actorManager.NetManager.CalculateBag(_.newItem, out ItemData itemResidue);
                    actorManager.NetManager.RPC_LocalInput_ChangeItemInBag(_.oldItem, _.newItem);
                    if (_.itemResidueBack != null)
                    {
                        _.itemResidueBack.Invoke(itemResidue);
                    }
                    if (itemResidue.Item_Count != 0)//背包溢出
                    {
                        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryDropItem()
                        {
                            item = itemResidue
                        });
                    }
                }
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TryAddItemOnHand>().Subscribe(_ =>
        {
            if (thisPlayerIsMe)
            {
                if (actorManager.NetManager.Data_ItemInHand.Item_ID != 0)
                {
                    /*手上已经有东西*/
                    actorManager.NetManager.RPC_LocalInput_AddItemInBag(actorManager.NetManager.Data_ItemInHand);
                }
                actorManager.NetManager.RPC_LocalInput_AddItemOnHand(_.item);
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TryAddItemOnHead>().Subscribe(_ =>
        {
            if (thisPlayerIsMe)
            {
                if (actorManager.NetManager.Data_ItemOnHead.Item_ID != 0)
                {
                    /*头上已经有东西*/
                    actorManager.NetManager.RPC_LocalInput_AddItemInBag(actorManager.NetManager.Data_ItemOnHead);
                }
                actorManager.NetManager.RPC_LocalInput_AddItemOnHead(_.item);
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TryAddItemOnBody>().Subscribe(_ =>
        {
            if (thisPlayerIsMe)
            {
                if (actorManager.NetManager.Data_ItemOnBody.Item_ID != 0)
                {
                    /*头上已经有东西*/
                    actorManager.NetManager.RPC_LocalInput_AddItemInBag(actorManager.NetManager.Data_ItemOnBody);
                }
                actorManager.NetManager.RPC_LocalInput_AddItemOnBody(_.item);
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TryRemoveItemOnHand>().Subscribe(_ =>
        {
            if (thisPlayerIsMe)
            {
                actorManager.NetManager.RPC_LocalInput_RemoveItemOnHand();
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TryRemoveItemOnHead>().Subscribe(_ =>
        {
            if (thisPlayerIsMe)
            {
                actorManager.NetManager.RPC_LocalInput_RemoveItemOnHead();
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TryRemoveItemOnBody>().Subscribe(_ =>
        {
            if (thisPlayerIsMe)
            {
                actorManager.NetManager.RPC_LocalInput_RemoveItemOnBody();
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
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TryBuildBuilding>().Subscribe(_ =>
        {
            if (thisPlayerIsMe)
            {
                MessageBroker.Default.Publish(new MapEvent.MapEvent_LocalTile_ChangeBuilding()
                {
                    buildingName = _.name,
                    buildingPos = actorManager.GetMyTile()._posInCell
                });
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TryBuildFloor>().Subscribe(_ =>
        {
            if (thisPlayerIsMe)
            {
                MessageBroker.Default.Publish(new MapEvent.MapEvent_LocalTile_ChangeFloor()
                {
                    floorName = _.name,
                    floorPos = actorManager.GetMyTile()._posInCell
                });
            }
        }).AddTo(this);

        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_Emoji>().Subscribe(_ =>
        {
            if (thisPlayerIsMe)
            {
                actorManager.NetManager.RPC_LocalInput_SendEmoji(_.id);
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
                if (actorManager.NetManager.Data_ItemInBag.Count > 0)
                {
                    ItemData itemData = actorManager.NetManager.Data_ItemInBag[0];
                    MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryRemoveItemFromBag()
                    {
                        item = itemData
                    });
                    if (actorManager.NetManager.Data_ItemInHand.Item_ID != 0)
                    {
                        /*手上已经有东西*/
                        actorManager.NetManager.RPC_LocalInput_AddItemInBag(actorManager.NetManager.Data_ItemInHand);
                    }
                    actorManager.NetManager.RPC_LocalInput_AddItemOnHand(itemData);
                }
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                for (int i = 0; i < nearbyTiles.Count; i++)
                {
                    nearbyTiles[i].InvokeTile(this);
                }
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                var item = Physics2D.OverlapCircleAll(transform.position, 0.5f, itemLayer);
                if (item.Length <= 0) { return; }
                if (item[0].gameObject.transform.parent.TryGetComponent(out ItemNetObj obj))
                {
                    actorManager.NetManager.RPC_LocalInput_PickItem(obj.Object.Id);
                }
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                if (actorManager.GetMyTile().name == "Default")
                {
                    MessageBroker.Default.Publish(new MapEvent.MapEvent_LocalTile_ChangeBuilding()
                    {
                        buildingName = "Base",
                        buildingPos = actorManager.GetMyTile()._posInCell
                    });
                }
                else if (actorManager.GetMyTile().name == "Base")
                {
                    MessageBroker.Default.Publish(new MapEvent.MapEvent_LocalTile_ChangeBuilding()
                    {
                        buildingName = "Default",
                        buildingPos = actorManager.GetMyTile()._posInCell
                    });
                }
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                if (actorManager.GetMyTile().name == "Default")
                {
                    MessageBroker.Default.Publish(new MapEvent.MapEvent_LocalTile_ChangeBuilding()
                    {
                        buildingName = "FloorBuilder",
                        buildingPos = actorManager.GetMyTile()._posInCell
                    });
                }
                else if (actorManager.GetMyTile().name == "FloorBuilder")
                {
                    MessageBroker.Default.Publish(new MapEvent.MapEvent_LocalTile_ChangeBuilding()
                    {
                        buildingName = "Default",
                        buildingPos = actorManager.GetMyTile()._posInCell
                    });
                }
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (localPlayerData.SkillUseList.Count > 0)
                {
                    InvokeSkill(localPlayerData.SkillUseList[0]);
                }
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                ChangeUsingSkill(true);
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                ChangeUsingSkill(false);
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
    private float shiftPressTimer = 0;
    private float shiftReleaseTimer = 0;
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
    public void All_PlayerInputMove(float deltaTime, Vector2 dir, bool speedUp, bool hasStateAuthority, bool hasInputAuthority)
    {
        dir = dir.normalized;
        float speed = actorManager.NetManager.Data_CommonSpeed;
        if (speedUp)
        {
            shiftReleaseTimer = 0;
            actorManager.NetManager.Data_EnRelease = 0;
            if (shiftPressTimer < actorManager.NetManager.Data_En * 0.001f)
            {
                shiftPressTimer += deltaTime;
                actorManager.NetManager.Data_En = (int)((shiftPressTimer * 1000) / (0.001f * actorManager.NetManager.Data_En));
                if (shiftPressTimer > 1)
                {
                    speed = Mathf.Lerp(actorManager.NetManager.Data_CommonSpeed, actorManager.NetManager.Data_MaxSpeed, 1);
                }
                else
                {
                    speed = Mathf.Lerp(actorManager.NetManager.Data_CommonSpeed, actorManager.NetManager.Data_MaxSpeed, shiftPressTimer);
                }
            }
        }
        else
        {
            if (shiftPressTimer != 0)
            {
                shiftReleaseTimer += deltaTime;
                actorManager.NetManager.Data_EnRelease = (int)((shiftReleaseTimer * 1000) / (0.001f * actorManager.NetManager.Data_En));
                if (shiftReleaseTimer > actorManager.NetManager.Data_En * 0.001f)
                {
                    shiftPressTimer = 0;
                    actorManager.NetManager.Data_En = (int)((shiftPressTimer * 1000) / (0.001f * actorManager.NetManager.Data_En));
                }
            }
        }
        Vector2 velocity = new Vector2(dir.x * speed, dir.y * speed);
        Vector3 newPos = transform.position + new UnityEngine.Vector3(velocity.x * deltaTime, velocity.y * deltaTime, 0);
        actorManager.NetManager.UpdateNetworkTransform(newPos, velocity.magnitude);
    }
    /// <summary>
    /// 朝向输入
    /// </summary>
    /// <param name="dir">方向</param>
    public void All_PlayerInputFace(float dt, Vector2 dir, bool hasStateAuthority, bool hasInputAuthority)
    {
        actorManager.FaceTo(dir);
        if (actorManager.holdingByHand != null)
        {
            actorManager.holdingByHand.FaceTo(dir, dt);
        }
    }

    #endregion
    #region//技能系统
    public void ChangeUsingSkill(bool next)
    {
        if (localPlayerData.SkillUseList.Count > 0)
        {
            int firstSkill = localPlayerData.SkillUseList[0];
            int lastSkill = localPlayerData.SkillUseList[localPlayerData.SkillUseList.Count - 1];

            if (next)
            {
                for(int i = 0; i < localPlayerData.SkillUseList.Count; i++)
                {
                    if (localPlayerData.SkillUseList.Count > i + 1)
                    {
                        /*有下位*/
                        localPlayerData.SkillUseList[i] = localPlayerData.SkillUseList[i + 1];
                    }
                    else
                    {
                        localPlayerData.SkillUseList[i] = firstSkill;
                    }
                }
            }
            else
            {
                for (int i = localPlayerData.SkillUseList.Count - 1; i >= 0; i--)
                {
                    if (i > 0)
                    {
                        /*有上位*/
                        localPlayerData.SkillUseList[i] = localPlayerData.SkillUseList[i - 1];
                    }
                    else
                    {
                        localPlayerData.SkillUseList[i] = lastSkill;
                    }
                }
            }
            actorManager.NetManager.RPC_LocalInput_BindSkill(localPlayerData.SkillUseList[0]);
        }
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_BindSkill()
        {
            skillIDs = localPlayerData.SkillUseList
        });
    }
    public void BindUseSkill(int id)
    {
        if (localPlayerData.SkillUseList.Count < 3)
        {
            localPlayerData.SkillUseList.Add(id);
        }
        else
        {
            localPlayerData.SkillUseList[0] = id;
        }
        List<int> copySkill = new List<int>();
        for (int i = 0; i < localPlayerData.SkillUseList.Count; i++)
        {
            copySkill.Add(localPlayerData.SkillUseList[i]);
        }
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_BindSkill()
        {
            skillIDs = copySkill
        });
    }
    public void InvokeSkill(int id)
    {
        actorManager.NetManager.RPC_LocalInput_InvokeSkill(id, new Fusion.NetworkId());
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
        tempTiles = actorManager.GetNearbyTiles();
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
