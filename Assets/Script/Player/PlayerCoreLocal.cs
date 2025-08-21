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
    //[HideInInspector]
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
        #region//��������
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TryAddItemInBag>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actorNetManager.Local_AddItemInBag(_.index, _.itemData);
                Debug.Log(_.itemData.Item_ID);
                Local_SaveActorData();
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TryExpendItemInBag>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actorNetManager.Local_ExpendItemInBag(_.itemID, _.itemCount);
                Local_SaveActorData();
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TryChangeItemInBag>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actorNetManager.Local_ChangeItemInBag(_.index,_.itemData);
                Local_SaveActorData();
            }
        }).AddTo(this);

        #endregion
        #region//�ֲ�����
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TryAddItemOnHand>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actorNetManager.RPC_LocalInput_AddItemOnHand(_.item);
                Local_SaveActorData();
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TrySubItemOnHand>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actorNetManager.RPC_LocalInput_SubItemOnHand(_.item);
                Local_SaveActorData();
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TryChangeItemOnHand>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actorNetManager.RPC_LocalInput_ChangeItemOnHand(_.oldItem, _.newItem);
                Local_SaveActorData();
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TrySwitchItemBetweenHandAndBag>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actionManager.SwitchHandAndBag(_.index);
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TryPutAwayItemOnHand>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                ItemData itemNew = new ItemData(0);
                actorManager_Bind.actorNetManager.RPC_LocalInput_AddItemOnHand(itemNew);
            }
        }).AddTo(this);
        #endregion
        #region//ͷ������
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TryAddItemOnHead>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actorNetManager.RPC_LocalInput_AddItemOnHead(_.item);
                Local_SaveActorData();
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TrySubItemOnHead>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actorNetManager.RPC_LocalInput_SubItemOnHead(_.item);
                Local_SaveActorData();
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TryChangeItemOnHead>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actorNetManager.RPC_LocalInput_ChangeItemOnHead(_.oldItem, _.newItem);
                Local_SaveActorData();
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TrySwitchItemBetweenHeadAndBag>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actionManager.SwitchHeadAndBag(_.index);
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TryPutAwayItemOnHead>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                ItemData itemNew = new ItemData(0);
                actorManager_Bind.actorNetManager.RPC_LocalInput_AddItemOnHead(itemNew);
            }
        }).AddTo(this);
        #endregion
        #region//��������
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TryAddItemOnBody>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actorNetManager.RPC_LocalInput_AddItemOnBody(_.item);
                Local_SaveActorData();
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TrySubItemOnBody>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actorNetManager.RPC_LocalInput_SubItemOnBody(_.item);
                Local_SaveActorData();
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TryChangeItemOnBody>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actorNetManager.RPC_LocalInput_ChangeItemOnBody(_.oldItem, _.newItem);
                Local_SaveActorData();
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TrySwitchItemBetweenBodyAndBag>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                actorManager_Bind.actionManager.SwitchBodyAndBag(_.index);
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TryPutAwayItemOnBody>().Subscribe(_ =>
        {
            if (bool_Local)
            {
                ItemData itemNew = new ItemData(0);
                actorManager_Bind.actorNetManager.RPC_LocalInput_AddItemOnBody(itemNew);
            }
        }).AddTo(this);
        #endregion
        #region//Buff
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
        if (bool_Local && actorManager_Bind != null)
        {
            Local_PlayerInputKey();
            Local_FindNearestNearbyTile();
        }
    }
    #region//��ɫ��ʼ��
    public void AllClinet_InitPlayer(bool input, bool state)
    {
        bool_Local = input;
        bool_State = state;
    }
    #endregion
    #region//��ɫ��
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
        CameraManager.Instance.FollowTarget(actorManager_Bind.playerSimulation.transform);
        MessageBroker.Default.Publish(new GameEvent.GameEvent_Local_BindLocalPlayer()
        {
            playerCore = this
        });
    }
    public void State_BindActor()
    {
        
    }
    #endregion
    #region//��ɫ��Ϣ
    [HideInInspector]
    public PlayerData playerData_Local;
    public void Local_SetActorData()
    {
        GameDataManager.Instance.LoadPlayer(out playerData_Local);
        if (playerData_Local != null)
        {
            Debug.Log("--�����Ϣ��ȡ�ɹ�--" + playerCoreNet.Object.InputAuthority);
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
            actorManager_Bind.actorNetManager.Local_SetBuffList(playerData_Local.BuffList);
            //Debug.Log("--��ʼ������ֲ�");
            actorManager_Bind.actorNetManager.RPC_LocalInput_AddItemOnHand(playerData_Local.HandItem);
            //Debug.Log("--��ʼ�����ͷ��");
            actorManager_Bind.actorNetManager.RPC_LocalInput_AddItemOnHead(playerData_Local.HeadItem);
            //Debug.Log("--��ʼ���������");
            actorManager_Bind.actorNetManager.RPC_LocalInput_AddItemOnBody(playerData_Local.BodyItem);
            //Debug.Log("--��ʼ���������");
            actorManager_Bind.actorNetManager.RPC_LocalInput_InitPlayerCommonData(Local_CreatePlayerNetData(playerData_Local), playerData_Local.Name);
            Debug.Log("--��ʼ�����λ��");
            actorManager_Bind.actorNetManager.RPC_Local_SetNetworkTransform(Vector3Int.zero);
            Local_UpdateMapInView(Vector3Int.zero);
        }
        else
        {
            Debug.Log("--�����Ϣ��ȡʧ��--");
        }
    }
    /// <summary>
    /// ������������
    /// </summary>
    public void Local_SaveActorData()
    {
        playerData_Local.HandItem = actorManager_Bind.actorNetManager.Net_ItemInHand;
        playerData_Local.HeadItem = actorManager_Bind.actorNetManager.Net_ItemOnHead;
        playerData_Local.BodyItem = actorManager_Bind.actorNetManager.Net_ItemOnBody;
        playerData_Local.BagItems = actorManager_Bind.actorNetManager.Local_GetBagItem();

        playerData_Local.Hp_Cur = actorManager_Bind.actorNetManager.Net_HpCur;
        playerData_Local.Hp_Max = actorManager_Bind.actorNetManager.Local_HpMax;
        playerData_Local.Food_Cur = actorManager_Bind.actorNetManager.Net_FoodCur;
        playerData_Local.Food_Max = actorManager_Bind.actorNetManager.Local_FoodMax;
        playerData_Local.San_Cur = actorManager_Bind.actorNetManager.Net_SanCur;
        playerData_Local.San_Max = actorManager_Bind.actorNetManager.Local_SanMax;
        playerData_Local.Armor_Cur = 0;
        playerData_Local.Resistance_Cur = 0;
        playerData_Local.Coin_Cur = actorManager_Bind.actorNetManager.Local_Coin;
        playerData_Local.Fine_Cur = actorManager_Bind.actorNetManager.Local_Fine;

        playerData_Local.Hair_ID = actorManager_Bind.actorNetManager.Local_HairID;
        playerData_Local.Hair_Color = actorManager_Bind.actorNetManager.Local_HairColor;
        playerData_Local.Eye_ID = actorManager_Bind.actorNetManager.Local_EyeID;
        playerData_Local.BuffList = actorManager_Bind.actorNetManager.Local_GetBuffList();

        GameDataManager.Instance.SavePlayer(playerData_Local);
    }
    /// <summary>
    /// ������������
    /// </summary>
    public void Local_ResetActorData()
    {
        playerData_Local.Hp_Cur = (short)(playerData_Local.Hp_Max / 2);
        playerData_Local.Food_Cur = (short)(playerData_Local.Food_Max / 2);
        playerData_Local.San_Cur = (short)(playerData_Local.San_Max / 2);
        playerData_Local.Fine_Cur = 0;
        playerData_Local.BuffList.Clear();

        GameDataManager.Instance.SavePlayer(playerData_Local);
    }
    public void Test()
    {
        Debug.Log("--���������Χ��ͼ");
        Local_UpdateMapInView(Vector3Int.zero);
    }
    private PlayerNetData Local_CreatePlayerNetData(PlayerData playerData)
    {
        PlayerNetData playerNetData = new PlayerNetData();

        playerNetData.Eye_ID = playerData.Eye_ID;
        playerNetData.Hair_ID = playerData.Hair_ID;
        playerNetData.Hair_Color = playerData.Hair_Color;

        playerNetData.Speed_Common = playerData.Speed_Common;
        playerNetData.Hp_Cur = playerData.Hp_Cur;
        playerNetData.Hp_Max = playerData.Hp_Max;
        playerNetData.Food_Cur = playerData.Food_Cur;
        playerNetData.Food_Max = playerData.Food_Max;
        playerNetData.San_Cur = playerData.San_Cur;
        playerNetData.San_Max = playerData.San_Max;
        playerNetData.Armor_Cur = playerData.Armor_Cur;
        playerNetData.Resistance_Cur = playerData.Resistance_Cur;
        playerNetData.Coin_Cur = playerData.Coin_Cur;

        return playerNetData;
    }

    #endregion
    #region//�������
    private void Local_PlayerInputKey()
    {
        if (!actorManager_Bind || !Input.anyKeyDown) return;
        if (Input.GetKeyDown(KeyCode.Alpha1)) actorManager_Bind.inputManager.InputAlpha(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) actorManager_Bind.inputManager.InputAlpha(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) actorManager_Bind.inputManager.InputAlpha(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) actorManager_Bind.inputManager.InputAlpha(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) actorManager_Bind.inputManager.InputAlpha(4);
        if (Input.GetKeyDown(KeyCode.Alpha6)) actorManager_Bind.inputManager.InputAlpha(5);
        if (Input.GetKeyDown(KeyCode.Alpha7)) actorManager_Bind.inputManager.InputAlpha(6);
        if (Input.GetKeyDown(KeyCode.Alpha8)) actorManager_Bind.inputManager.InputAlpha(7);
        if (Input.GetKeyDown(KeyCode.Alpha9)) actorManager_Bind.inputManager.InputAlpha(8);
        if (Input.GetKeyDown(KeyCode.Alpha0)) actorManager_Bind.inputManager.InputAlpha(9);

        if (Input.GetKeyDown(KeyCode.F)) actorManager_Bind.inputManager.InputKeycode(KeyCode.F);
        if (Input.GetKeyDown(KeyCode.R)) actorManager_Bind.inputManager.InputKeycode(KeyCode.R);
        if (Input.GetKeyDown(KeyCode.Space)) actorManager_Bind.inputManager.InputKeycode(KeyCode.Space);
    }

    #endregion
    #region//��ͼ����
    /// <summary>
    /// �����ؿ�
    /// </summary>
    private List<BuildingTile> buildingTiles_Nearby = new List<BuildingTile>();
    /// <summary>
    /// �����ؿ�
    /// </summary>
    private BuildingTile buildingTiles_HighLight = null;
    /// <summary>
    /// ����ؿ�
    /// </summary>
    private BuildingTile buildingTiles_Nearest = null;
    /// <summary>
    /// �������
    /// </summary>
    private float distance_Nearset;
    private Vector3Int vector3Int_mapCenter = new Vector3Int(-99999, -99999);
    private const float config_MapView = 12;
    /// <summary>
    /// ���µ�ͼ����
    /// </summary>
    public void Local_UpdateMapInView(Vector3Int pos)
    {
        GameUI_MiniMap.Instance.ChangePlayerPos((Vector2Int)pos);
        if (Mathf.Abs(pos.x - vector3Int_mapCenter.x) > config_MapView || Mathf.Abs(pos.y - vector3Int_mapCenter.y) > config_MapView)
        {
            Debug.Log($"������ͼ���Ʒ�Χ,���������򡣵�ǰλ��({pos})��ͼê��({vector3Int_mapCenter})");
            vector3Int_mapCenter = new Vector3Int((int)(Math.Round(pos.x / config_MapView) * config_MapView), (int)(Math.Round(pos.y / config_MapView) * config_MapView), 0);
            MessageBroker.Default.Publish(new MapEvent.MapEvent_Local_RequestMapData()
            {
                playerPos = vector3Int_mapCenter,
                mapSize = (int)config_MapView
            });
        }
    }
    /// <summary>
    /// ���¸����ĵؿ�
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
    /// ��ȡ����ؿ�
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
        }
        if (buildingTiles_HighLight != buildingTiles_Nearest)
        {
            Local_HighLightTile(buildingTiles_Nearest);
        }
    }
    /// <summary>
    /// �����ؿ�
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
/// �����������
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
    public short Speed_Common;

    public int Coin_Cur;
    public int Fine_Cur;

    public short Hair_ID;
    public Color32 Hair_Color;
    public short Eye_ID;
}
