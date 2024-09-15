using Fusion;
using System;
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
    [HideInInspector]
    public PlayerRef localPlayerRef;
    private void Start()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_Local_SomeoneMove>().Subscribe(_ =>
        {
            if (_.moveActor == actorManager && thisPlayerIsMe)
            {
                UpdateMapInView(_.moveTile._posInCell);
                UpdateNearByTile();
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TryRemoveItemFromBag>().Subscribe(_ =>
        {
            if (thisPlayerIsMe)
            {
                actorManager.NetManager.RPC_LocalInput_LoseItem(_.item);
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TryAddItemInBag>().Subscribe(_ =>
        {
            if (thisPlayerIsMe)
            {
                actorManager.NetManager.RPC_LocalInput_AddItemInBag(_.item);
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TryChangeItemInBag>().Subscribe(_ =>
        {
            if (thisPlayerIsMe)
            {
                if (actorManager.NetManager.Data_ItemInHand.Equals(_.oldItem))
                {
                    actorManager.NetManager.RPC_LocalInput_ChangeItemOnHand(_.newItem);
                }
                else if (actorManager.NetManager.Data_ItemOnHead.Equals(_.oldItem))
                {
                    actorManager.NetManager.RPC_LocalInput_ChangeItemOnHead(_.newItem);
                }
                else if (actorManager.NetManager.Data_ItemOnBody.Equals(_.oldItem))
                {
                    actorManager.NetManager.RPC_LocalInput_ChangeItemOnBody(_.newItem);
                }
                else
                {
                    actorManager.NetManager.RPC_LocalInput_ChangeItemInBag(_.oldItem, _.newItem);
                }
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TryAddItemOnHand>().Subscribe(_ =>
        {
            if (thisPlayerIsMe)
            {
                actorManager.NetManager.RPC_LocalInput_AddItemOnHand(_.item);
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TryAddItemOnHead>().Subscribe(_ =>
        {
            if (thisPlayerIsMe)
            {
                actorManager.NetManager.RPC_LocalInput_AddItemOnHead(_.item);
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TryAddItemOnBody>().Subscribe(_ =>
        {
            if (thisPlayerIsMe)
            {
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
                    pos = transform.position - new Vector3(0, 0.1f, 0),
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
                    buildingID = _.id,
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
                    floorID = _.id,
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
                    actorManager.NetManager.RPC_LocalInput_AddItemOnHand(itemData);
                }
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (holdingTile)
                {
                    holdingTile.InvokeTile(this, KeyCode.F);
                }
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (holdingTile)
                {
                    holdingTile.InvokeTile(this, KeyCode.E);
                }
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                var items = Physics2D.OverlapCircleAll(transform.position, 0.5f, itemLayer);
                Debug.Log(items.Length + "/" + transform.position + "/" );
                foreach (Collider2D item in items)
                {
                    if (item.gameObject.transform.parent.TryGetComponent(out ItemNetObj obj))
                    {
                        actorManager.NetManager.RPC_LocalInput_PickItem(obj.Object.Id);
                        break;
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                if (actorManager.GetMyTile().name == "Default")
                {
                    MessageBroker.Default.Publish(new MapEvent.MapEvent_LocalTile_ChangeBuilding()
                    {
                        buildingID = 1,
                        buildingPos = actorManager.GetMyTile()._posInCell
                    });
                }
                else if (actorManager.GetMyTile().name == "BuildingBuilder")
                {
                    MessageBroker.Default.Publish(new MapEvent.MapEvent_LocalTile_ChangeBuilding()
                    {
                        buildingID = 0,
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
                        buildingID = 2,
                        buildingPos = actorManager.GetMyTile()._posInCell
                    });
                }
                else if (actorManager.GetMyTile().name == "FloorBuilder")
                {
                    MessageBroker.Default.Publish(new MapEvent.MapEvent_LocalTile_ChangeBuilding()
                    {
                        buildingID = 0,
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
        float commonSpeed = actorManager.NetManager.Data_CommonSpeed / 10f;
        float maxSpeed = actorManager.NetManager.Data_MaxSpeed / 10f;
        float speed;
        if (speedUp)
        {
            if (EnSub((int)(deltaTime * 1000)))
            {
                if (shiftPressTimer < 1)
                {
                    shiftPressTimer += deltaTime;
                }
                else
                {
                    shiftPressTimer = 1;
                }
            }
            else
            {
                if (shiftPressTimer > deltaTime)
                {
                    shiftPressTimer -= deltaTime;
                }
                else
                {
                    shiftPressTimer = 0;
                }
            }
        }
        else
        {
            if (shiftPressTimer > deltaTime)
            {
                shiftPressTimer -= deltaTime;
            }
            else
            {
                shiftPressTimer = 0;
            }
            EnAdd((int)(deltaTime * actorManager.NetManager.Data_EnRelease));
        }
        if (shiftPressTimer > 1)
        {
            speed = Mathf.Lerp(commonSpeed, maxSpeed, 1);
        }
        else
        {
            speed = Mathf.Lerp(commonSpeed, maxSpeed, shiftPressTimer);
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
            actorManager.holdingByHand.InputMousePos(dir, dt);
        }
    }

    #endregion
    #region//耐力系统
    private bool EnSub(int offset)
    {
        if(actorManager.NetManager.Data_CurEn > 50)
        {
            actorManager.NetManager.Data_CurEn -= offset;
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool EnAdd(int offset)
    {
        if (actorManager.NetManager.Data_CurEn < actorManager.NetManager.Data_MaxEn)
        {
            actorManager.NetManager.Data_CurEn += offset;
            return false;
        }
        else
        {
            return true;
        }
    }
    #endregion
    #region//技能系统
    public void ChangeUsingSkill(bool next)
    {
        if (localPlayerData.SkillUseList.Count > 0)
        {
            short firstSkill = localPlayerData.SkillUseList[0];
            short lastSkill = localPlayerData.SkillUseList[localPlayerData.SkillUseList.Count - 1];

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
    public void BindUseSkill(short id)
    {
        if (localPlayerData.SkillUseList.Count < 3)
        {
            localPlayerData.SkillUseList.Add(id);
        }
        else
        {
            localPlayerData.SkillUseList[0] = id;
        }
        List<short> copySkill = new List<short>();
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
    #region//地图绘制
    private List<MyTile> nearbyTiles = new List<MyTile>();
    private MyTile holdingTile = null;
    private List<MyTile> tempTiles = new List<MyTile>();
    private Vector3Int mapCenter = new Vector3Int(-99999,-99999);
    private int mapView = 15;
    /// <summary>
    /// 更新地图绘制
    /// </summary>
    public void UpdateMapInView(Vector3Int pos)
    {
        if (Mathf.Abs(pos.x - mapCenter.x) > mapView || Mathf.Abs(pos.y - mapCenter.y) > mapView)
        {
            Debug.Log("超出地图绘制范围,绘制新区域" + "当前位置(" + pos + ")" + "地图锚点(" + mapCenter + ")");
            mapCenter = new Vector3Int((int)Math.Round(pos.x / 20f) * 20, (int)Math.Round(pos.y / 20f) * 20, 0);
            MessageBroker.Default.Publish(new MapEvent.MapEvent_LocalTile_RequestMapData()
            {
                playerPos = mapCenter,
                playerRef = localPlayerRef
            });
        }
    }
    /// <summary>
    /// 更新附近的地块
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
        /*更新持有地块*/
        for (int i = 0; i < tempTiles.Count; i++)
        {
            if (tempTiles[i] == null) { continue; }
            if (holdingTile != tempTiles[i])
            {
                if (tempTiles[i].HoldingTileByPlayer(this))
                {
                    UpdateHoldingTile(tempTiles[i]);
                    return;
                }
            }
            else
            {
                return;
            }
        }
        UpdateHoldingTile(null);
    }
    private void UpdateHoldingTile(MyTile tile)
    {
        holdingTile?.ReleaseTileByPlayer(this);
        holdingTile = tile;
    }
    #endregion
}
