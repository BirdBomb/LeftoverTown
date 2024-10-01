using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Windows;
using Input = UnityEngine.Input;
/// <summary>
/// ��ҿ�����
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("����")]
    public ActorManager actorManager;
    [Header("����㼶")]
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
                    buildingPos = actorManager.Tool_GetMyTileWithOffset(Vector3Int.zero)._posInCell
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
                    floorPos = actorManager.Tool_GetMyTileWithOffset(Vector3Int.zero)._posInCell
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
        actorManager.AllClient_InitByPlayer();
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
                if (actorManager.Tool_GetMyTileWithOffset(Vector3Int.zero).name == "Default")
                {
                    MessageBroker.Default.Publish(new MapEvent.MapEvent_LocalTile_ChangeBuilding()
                    {
                        buildingID = 1,
                        buildingPos = actorManager.Tool_GetMyTileWithOffset(Vector3Int.zero)._posInCell
                    });
                }
                else if (actorManager.Tool_GetMyTileWithOffset(Vector3Int.zero).name == "BuildingBuilder")
                {
                    MessageBroker.Default.Publish(new MapEvent.MapEvent_LocalTile_ChangeBuilding()
                    {
                        buildingID = 0,
                        buildingPos = actorManager.Tool_GetMyTileWithOffset(Vector3Int.zero)._posInCell
                    });
                }
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                if (actorManager.Tool_GetMyTileWithOffset(Vector3Int.zero).name == "Default")
                {
                    MessageBroker.Default.Publish(new MapEvent.MapEvent_LocalTile_ChangeBuilding()
                    {
                        buildingID = 2,
                        buildingPos = actorManager.Tool_GetMyTileWithOffset(Vector3Int.zero)._posInCell
                    });
                }
                else if (actorManager.Tool_GetMyTileWithOffset(Vector3Int.zero).name == "FloorBuilder")
                {
                    MessageBroker.Default.Publish(new MapEvent.MapEvent_LocalTile_ChangeBuilding()
                    {
                        buildingID = 0,
                        buildingPos = actorManager.Tool_GetMyTileWithOffset(Vector3Int.zero)._posInCell
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
    #region//�������
    private float shiftPressTimer = 0;
    /// <summary>
    /// λ������
    /// </summary>
    /// <param name="deltaTime">����</param>
    /// <param name="dir">����</param>
    /// <param name="speedUp">����</param>
    /// <param name="hasStateAuthority"></param>
    /// <param name="hasInputAuthority"></param>
    public void AllClient_PlayerInputMove(float deltaTime, Vector2 dir, bool speedUp, bool hasStateAuthority, bool hasInputAuthority)
    {
        dir = dir.normalized;
        float commonSpeed = actorManager.NetManager.Data_CommonSpeed * 0.1f;
        float maxSpeed = actorManager.NetManager.Data_MaxSpeed * 0.1f;
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
        actorManager.NetManager.OnlyState_UpdateNetworkTransform(newPos, velocity.magnitude);
    }
    /// <summary>
    /// �������
    /// </summary>
    /// <param name="leftClickTime"></param>
    /// <param name="rightClickTime"></param>
    /// <param name="leftPressTime"></param>
    /// <param name="rightPressTime"></param>
    /// <param name="hasStateAuthority"></param>
    /// <param name="hasInputAuthority"></param>
    public void AllClient_PlayerInputMouse(float leftPressTime, float rightPressTime, bool hasStateAuthority, bool hasInputAuthority)
    {
        if (leftPressTime > 0)
        {
            actorManager.itemOnHand.UpdateLeftPress(leftPressTime, hasStateAuthority, hasInputAuthority, true);
        }
        else
        {
            actorManager.itemOnHand.ReleaseLeftPress(hasStateAuthority, hasInputAuthority, true);
        }
        if (rightPressTime > 0)
        {
            actorManager.itemOnHand.UpdateRightPress(rightPressTime, hasStateAuthority, hasInputAuthority, true);
        }
        else
        {
            actorManager.itemOnHand.ReleaseRightPress(hasStateAuthority, hasInputAuthority, true);
        }
    }
    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="dir">����</param>
    public void AllClient_PlayerInputFace(Vector2 dir, bool hasStateAuthority, bool hasInputAuthority)
    {
        actorManager.AllClient_FaceTo(dir);
        if (actorManager.itemOnHand != null)
        {
            actorManager.itemOnHand.UpdateMousePos(dir);
        }
    }

    #endregion
    #region//����ϵͳ
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
    #region//����ϵͳ
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
                        /*����λ*/
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
                        /*����λ*/
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
    #region//��ͼ����
    private List<MyTile> nearbyTiles = new List<MyTile>();
    private MyTile holdingTile = null;
    private List<MyTile> tempTiles = new List<MyTile>();
    private Vector3Int mapCenter = new Vector3Int(-99999,-99999);
    private const int config_MapView = 15;
    /// <summary>
    /// ���µ�ͼ����
    /// </summary>
    public void UpdateMapInView(Vector3Int pos)
    {
        if (Mathf.Abs(pos.x - mapCenter.x) > config_MapView || Mathf.Abs(pos.y - mapCenter.y) > config_MapView)
        {
            Debug.Log("������ͼ���Ʒ�Χ,����������" + "��ǰλ��(" + pos + ")" + "��ͼê��(" + mapCenter + ")");
            mapCenter = new Vector3Int((int)Math.Round(pos.x / 20f) * 20, (int)Math.Round(pos.y / 20f) * 20, 0);
            MessageBroker.Default.Publish(new MapEvent.MapEvent_LocalTile_RequestMapData()
            {
                playerPos = mapCenter,
                playerRef = localPlayerRef
            });
        }
    }
    /// <summary>
    /// ���¸����ĵؿ�
    /// </summary>
    private void UpdateNearByTile()
    {
        /*��Χ�ؿ�*/
        tempTiles = actorManager.Tool_GetNearbyTiles();
        /*�޳��ϴμ��ĵؿ�*/
        for (int i = 0; i < nearbyTiles.Count; i++)
        {
            if (nearbyTiles[i] == null) { continue; }
            if (!tempTiles.Contains(nearbyTiles[i]))
            {
                nearbyTiles[i].FarawayTileByPlayer(this);
                nearbyTiles.RemoveAt(i);
            }
        }
        /*��ӱ��μ���ĵؿ�*/
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
        /*���³��еؿ�*/
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
    #region//��ͷ����
    #endregion
}
