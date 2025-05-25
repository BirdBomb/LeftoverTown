using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UniRx;
using Fusion.Sockets;
/// <summary>
/// 玩家核心
/// </summary>
public class PlayerCoreNet : NetworkBehaviour
{
    [SerializeField]
    private PlayerCoreLocal playerCoreLocal;
    private void Start()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_State_RevivePlayer>().Subscribe(_ =>
        {
            Debug.Log(_.playerRef);
            Debug.Log(Object.InputAuthority);
            if (Object.InputAuthority == _.playerRef)
            {
                Debug.Log("55555");
                State_CreateActor();
            }
        }).AddTo(this);
        InvokeRepeating("UpdatePing", 2, 1);
    }
    public override void Spawned()
    {
        playerCoreLocal.InitPlayer(Object.HasInputAuthority, Object.HasStateAuthority);
        StartCoroutine(AllClient_Init()); 
        if (Object.HasInputAuthority) { StartCoroutine(Local_Init()); }
        if (Object.HasStateAuthority) { StartCoroutine(State_Init()); }
        base.Spawned();
    }
    #region//玩家初始化
    private IEnumerator AllClient_Init()
    {
        yield return new WaitForSeconds(1);
        OnBindActorChange();
    }
    private IEnumerator Local_Init()
    {
        yield return new WaitForSeconds(1);
    }
    private IEnumerator State_Init()
    {
        yield return new WaitForSeconds(1);
        State_CreateActor();
    }
    #endregion
    #region//角色创建
    [SerializeField, Header("玩家预制体")]
    private NetworkPrefabRef networkPrefabRef_Actor;
    private void State_CreateActor()
    {
        NetworkObject networkObject = Runner.Spawn(networkPrefabRef_Actor, new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
        Net_BindActorID = networkObject.Id;
    }
    #endregion
    #region//角色绑定
    [Networked, HideInInspector, OnChangedRender(nameof(OnBindActorChange))]
    public NetworkId Net_BindActorID { get; set; } = new NetworkId();
    private void OnBindActorChange()
    {
        if (Net_BindActorID != null)
        {
            NetworkObject networkObject = Runner.FindObject(Net_BindActorID);
            if (networkObject != null)
            {
                playerCoreLocal.AllClinet_BindActor(networkObject.GetComponent<ActorManager>());
            }
        }
        else
        {
            Debug.Log("Net_BindActorID为空");
        }
    }
    #endregion
    #region//玩家输入
    [Networked, HideInInspector]
    public float Net_MouseRightPressTimer { get; set; }
    [Networked, HideInInspector]
    public float Net_MouseLeftPressTimer { get; set; }
    [Networked, HideInInspector]
    public Vector2 Net_MouseLocation { get; set; }
    private Vector2 vector2_MoveDir;

    public override void FixedUpdateNetwork()
    {
        if (playerCoreLocal.actorManager_Bind)
        {
            AllClient_PlayerInput(Runner.DeltaTime);
        }
        base.FixedUpdateNetwork();
    }
    public override void Render()
    {
        if (playerCoreLocal.actorManager_Bind)
        {
            AllClient_PlayerSync();
        }
        base.Render();
    }
    /// <summary>
    /// 玩家输入
    /// </summary>
    /// <param name="dt"></param>
    private void AllClient_PlayerInput(float dt)
    {
        if (playerCoreLocal.actorManager_Bind.actorState == ActorState.Dead) return;
        if (GetInput(out NetworkInputData netPlayerData))
        {
            if (Object.HasStateAuthority)
            {
                vector2_MoveDir = Vector2.zero;
                if (netPlayerData.PressD)
                {
                    vector2_MoveDir += new Vector2(1, 0);
                }
                if (netPlayerData.PressA)
                {
                    vector2_MoveDir += new Vector2(-1, 0);
                }
                if (netPlayerData.PressW)
                {
                    vector2_MoveDir += new Vector2(0, 1);
                }
                if (netPlayerData.PressS)
                {
                    vector2_MoveDir += new Vector2(0, -1);
                }
                playerCoreLocal.actorManager_Bind.inputManager.InputMove(dt, vector2_MoveDir);
            }
            else if (Object.HasInputAuthority)
            {
                vector2_MoveDir = Vector2.zero;
                if (netPlayerData.PressD)
                {
                    vector2_MoveDir += new Vector2(1, 0);
                }
                if (netPlayerData.PressA)
                {
                    vector2_MoveDir += new Vector2(-1, 0);
                }
                if (netPlayerData.PressW)
                {
                    vector2_MoveDir += new Vector2(0, 1);
                }
                if (netPlayerData.PressS)
                {
                    vector2_MoveDir += new Vector2(0, -1);
                }
                playerCoreLocal.actorManager_Bind.inputManager.SimulationMove(dt, vector2_MoveDir);
            }
            Net_MouseRightPressTimer = netPlayerData.MouseRightPressTimer;
            Net_MouseLeftPressTimer = netPlayerData.MouseLeftPressTimer;
            Net_MouseLocation = netPlayerData.MouseLocation;
        }
    }
    /// <summary>
    /// 玩家同步
    /// </summary>
    private void AllClient_PlayerSync()
    {
        playerCoreLocal.actorManager_Bind.inputManager.InputFace(Net_MouseLocation);
        playerCoreLocal.actorManager_Bind.inputManager.InputMouse(Net_MouseLeftPressTimer, Net_MouseRightPressTimer, Object.HasStateAuthority, Object.HasInputAuthority);
    }
    #endregion
    #region//检查延迟
    private void UpdatePing()
    {
        if (Object.HasInputAuthority)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdatePing()
            {
                ping = Runner.GetPlayerRtt(Object.InputAuthority)
            });
        }
    }
    #endregion
}
