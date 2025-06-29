using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCoreLocal : MonoBehaviour
{
    [SerializeField]
    private PlayerCoreNet playerCoreNet;
    [HideInInspector]
    public bool bool_Local, bool_State = false;

    private void Start()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_AllClient_SomeoneMove>().Subscribe(_ =>
        {
            if (bool_Local && _.moveActor == actorManager_Bind)
            {
                Local_UpdateMapInView(_.movePos);
                Local_UpdateNearbyTile(_.movePos);
            }
        }).AddTo(this);
        #region//背包物体
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TryAddItemInBag>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actorNetManager.Local_AddItemInBag(_.index, _.itemData);
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TryExpendItemInBag>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actorNetManager.Local_ExpendItemInBag(_.itemID, _.itemCount);
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TryChangeItemInBag>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actorNetManager.Local_ChangeItemInBag(_.index,_.itemData);
            }
        }).AddTo(this);
        #endregion
        #region//手部物体
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TryAddItemOnHand>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actorNetManager.RPC_LocalInput_AddItemOnHand(_.item);
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TrySubItemOnHand>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actorNetManager.RPC_LocalInput_SubItemOnHand(_.item);
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TryChangeItemOnHand>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actorNetManager.RPC_LocalInput_ChangeItemOnHand(_.oldItem, _.newItem);
            }
        }).AddTo(this);
        #endregion
        #region//头部物体
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TryAddItemOnHead>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actorNetManager.RPC_LocalInput_AddItemOnHead(_.item);
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TrySubItemOnHead>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actorNetManager.RPC_LocalInput_SubItemOnHead(_.item);
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TryChangeItemOnHead>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actorNetManager.RPC_LocalInput_ChangeItemOnHead(_.oldItem, _.newItem);
            }
        }).AddTo(this);
        #endregion
        #region//身体物体
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TryAddItemOnBody>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actorNetManager.RPC_LocalInput_AddItemOnBody(_.item);
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TrySubItemOnBody>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actorNetManager.RPC_LocalInput_SubItemOnBody(_.item);
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TryChangeItemOnBody>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actorNetManager.RPC_LocalInput_ChangeItemOnBody(_.oldItem, _.newItem);
            }
        }).AddTo(this);
        #endregion
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TryDropItem>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                MessageBroker.Default.Publish(new GameEvent.GameEvent_Local_SpawnItem()
                {
                    itemData = _.item,
                    pos = actorManager_Bind.transform.position - new Vector3(0, 0.1f, 0),
                });
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TrySpawnActor>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                MessageBroker.Default.Publish(new GameEvent.GameEvent_Local_SpawnActor()
                {
                    name = _.name,
                    pos = actorManager_Bind.transform.position
                });
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_SendEmoji>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actorNetManager.RPC_LocalInput_SendEmoji((int)_.emoji);
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_SendText>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actorNetManager.RPC_LocalInput_SendText(_.text,(int)Emoji.Yell);
            }
        }).AddTo(this);
    }
    private void Update()
    {
        if (bool_Local)
        {
            Local_PlayerInputKey();
            Local_FindNearestNearbyTile();
        }
    }
    #region//角色初始化
    public void InitPlayer(bool input, bool state)
    {
        bool_Local = input;
        bool_State = state;
    }
    #endregion
    #region//角色绑定
    [HideInInspector]
    public ActorManager actorManager_Bind = null;
    public void AllClinet_BindActor(ActorManager actor)
    {
        actorManager_Bind = actor;
        actorManager_Bind.AllClient_BindPlayer(bool_State, bool_Local, playerCoreNet.Object.InputAuthority);
        if (bool_Local)
        {
            Local_BindActor();
            Local_SetActorData();
        }
        if (bool_State)
        {
            State_BindActor();
        }
    }
    public void Local_BindActor()
    {
        CameraManager.Instance.tran_target = actorManager_Bind.playerSimulation.transform;
        MessageBroker.Default.Publish(new GameEvent.GameEvent_Local_BindLocalPlayer()
        {
            playerCore = this
        });
    }
    public void State_BindActor()
    {
        //actor.actorNetManager.Object.AssignInputAuthority(playerCoreNet.Object.InputAuthority);
    }
    #endregion
    #region//角色信息
    [HideInInspector]
    public PlayerData playerData_Local;
    public void Local_SetActorData()
    {
        GameDataManager.Instance.LoadPlayer(out playerData_Local);
        if (playerData_Local != null)
        {
            Debug.Log("--玩家信息获取成功--");
            for (int i = 0; i < 15; i++)
            {
                if(i < playerData_Local.BagItems.Count)
                {
                    actorManager_Bind.actorNetManager.Local_ChangeItemInBag(i, playerData_Local.BagItems[i]);
                }
                else
                {
                    actorManager_Bind.actorNetManager.Local_ChangeItemInBag(i, new ItemData());
                }
            }
            //Debug.Log("--初始化玩家手部");
            actorManager_Bind.actorNetManager.RPC_LocalInput_AddItemOnHand(playerData_Local.HandItem);
            //Debug.Log("--初始化玩家头部");
            actorManager_Bind.actorNetManager.RPC_LocalInput_AddItemOnHead(playerData_Local.HeadItem);
            //Debug.Log("--初始化玩家身体");
            actorManager_Bind.actorNetManager.RPC_LocalInput_AddItemOnBody(playerData_Local.BodyItem);
            //Debug.Log("--初始化玩家数据");
            actorManager_Bind.actorNetManager.RPC_LocalInput_InitPlayerCommonData(Local_CreatePlayerNetData(playerData_Local), playerData_Local.Name);
            Debug.Log("--初始化玩家位置");
            actorManager_Bind.actorNetManager.RPC_Local_SetNetworkTransform(Vector3Int.zero);
            Local_UpdateMapInView(Vector3Int.zero);
        }
        else
        {
            Debug.Log("--玩家信息获取失败--");
        }
    }
    public void Test()
    {
        Debug.Log("--绘制玩家周围地图");
        Local_UpdateMapInView(Vector3Int.zero);
    }
    private PlayerNetData Local_CreatePlayerNetData(PlayerData playerData)
    {
        PlayerNetData playerNetData = new PlayerNetData();

        playerNetData.Hair_ID = playerData.Hair_ID;
        playerNetData.Hair_Color = playerData.Hair_Color;
        playerNetData.Eye_ID = playerData.Eye_ID;

        playerNetData.Speed_Common = playerData.Speed_Common;
        playerNetData.Hp_Cur = playerData.Hp_Cur;
        playerNetData.Hp_Max = playerData.Hp_Max;
        playerNetData.Armor_Cur = playerData.Armor_Cur;
        playerNetData.Food_Cur = playerData.Food_Cur;
        playerNetData.Food_Max = playerData.Food_Max;
        playerNetData.San_Cur = playerData.San_Cur;
        playerNetData.San_Max = playerData.San_Max;
        playerNetData.Coin_Cur = playerData.Coin_Cur;

        return playerNetData;
    }

    #endregion
    #region//玩家输入
    private void Local_PlayerInputKey()
    {
        if (!actorManager_Bind) return;
        if (Input.GetKeyDown(KeyCode.F))
        {
            actorManager_Bind.inputManager.InputKeycode(KeyCode.F);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            actorManager_Bind.inputManager.InputKeycode(KeyCode.E);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            actorManager_Bind.inputManager.InputKeycode(KeyCode.Q);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            actorManager_Bind.inputManager.InputKeycode(KeyCode.R);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            actorManager_Bind.inputManager.InputKeycode(KeyCode.Space);
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            actorManager_Bind.inputManager.InputKeycode(KeyCode.Tab);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            actorManager_Bind.inputManager.InputKeycode(KeyCode.C);
        }
    }

    #endregion

    #region//地图绘制
    /// <summary>
    /// 附近地块
    /// </summary>
    private List<BuildingTile> buildingTiles_Nearby = new List<BuildingTile>();
    /// <summary>
    /// 高亮地块
    /// </summary>
    private BuildingTile buildingTiles_HighLight = null;
    /// <summary>
    /// 最近地块
    /// </summary>
    private BuildingTile buildingTiles_Nearest = null;
    /// <summary>
    /// 最近距离
    /// </summary>
    private float distance_Nearset;
    private Vector3Int vector3Int_mapCenter = new Vector3Int(-99999, -99999);
    private const float config_MapView = 12;
    /// <summary>
    /// 更新地图绘制
    /// </summary>
    public void Local_UpdateMapInView(Vector3Int pos)
    {
        GameUI_MiniMap.Instance.ChangePlayerPos((Vector2Int)pos);
        if (Mathf.Abs(pos.x - vector3Int_mapCenter.x) > config_MapView || Mathf.Abs(pos.y - vector3Int_mapCenter.y) > config_MapView)
        {
            Debug.Log($"超出地图绘制范围,绘制新区域。当前位置({pos})地图锚点({vector3Int_mapCenter})");
            vector3Int_mapCenter = new Vector3Int((int)(Math.Round(pos.x / config_MapView) * config_MapView), (int)(Math.Round(pos.y / config_MapView) * config_MapView), 0);
            MessageBroker.Default.Publish(new MapEvent.MapEvent_Local_RequestMapData()
            {
                playerPos = vector3Int_mapCenter,
                mapSize = (int)config_MapView
            });
        }
    }
    /// <summary>
    /// 更新附近的地块
    /// </summary>
    private void Local_UpdateNearbyTile(Vector3Int pos)
    {
        List<BuildingTile> temp = MapManager.Instance.GetNearbyBuildings_EightSide(pos, Vector3Int.zero);
        for (int i = 0; i < buildingTiles_Nearby.Count; i++)
        {
            if (buildingTiles_Nearby[i] != null)
            {
                buildingTiles_Nearby[i].FarawayTileByPlayer(this);
            }
        }
        for (int i = 0; i < temp.Count; i++)
        {
            if(temp[i] != null)
            {
                temp[i].NearbyTileByPlayer(this);
            }
        }
        buildingTiles_Nearby.Clear();
        for (int i = 0; i < temp.Count; i++)
        {
            if (temp[i] != null && temp[i].CanHighlight())
            {
                buildingTiles_Nearby.Add(temp[i]);
            }
        }
    }
    /// <summary>
    /// 获取最近地块
    /// </summary>
    private void Local_FindNearestNearbyTile()
    {
        distance_Nearset = float.MaxValue;
        buildingTiles_Nearest = null;
        for (int i = 0; i < buildingTiles_Nearby.Count; i++)
        {
            Vector3 myPos = actorManager_Bind.transform.position + (Vector3)playerCoreNet.Net_MouseLocation * 0.75f;
            Vector3 tilePos = buildingTiles_Nearby[i].tilePos + new Vector3(0.5f, 0.5f, 0);
            float val = Vector3.Distance(myPos, tilePos);
            if (val < distance_Nearset)
            {
                distance_Nearset = val;
                buildingTiles_Nearest = buildingTiles_Nearby[i];
            }
            Debug.Log(actorManager_Bind.transform.position + "/" + actorManager_Bind.pathManager.vector3Int_CurPos);
        }
        if (buildingTiles_HighLight != buildingTiles_Nearest)
        {
            Local_HighLightTile(buildingTiles_Nearest);
        }
    }
    /// <summary>
    /// 高亮地块
    /// </summary>
    /// <param name="tile"></param>
    private void Local_HighLightTile(BuildingTile tile)
    {
        if (buildingTiles_HighLight != tile)
        {
            if (buildingTiles_HighLight != null)
            {
                buildingTiles_HighLight.HighlightTileByPlayer(false);
                actorManager_Bind.inputManager.Local_RemoveInputKeycodeAction(buildingTiles_HighLight.ActorInputKeycode);
            }

            buildingTiles_HighLight = tile;
            if (buildingTiles_HighLight != null)
            {
                buildingTiles_HighLight.HighlightTileByPlayer(true);
                actorManager_Bind.inputManager.Local_AddInputKeycodeAction(buildingTiles_HighLight.ActorInputKeycode);
            }
        }
    }
    #endregion
}
/// <summary>
/// 玩家网络数据
/// </summary>
public struct PlayerNetData : INetworkStruct
{
    public short Hp_Cur;
    public short Hp_Max;
    public short Food_Cur;
    public short Food_Max;
    public short San_Cur;
    public short San_Max;
    public short Armor_Cur;
    public short Resistance_Cur;
    public short Coin_Cur;
    public short Speed_Common;

    public short Hair_ID;
    public Color32 Hair_Color;
    public short Eye_ID;
}
