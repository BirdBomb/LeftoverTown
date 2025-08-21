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
            if (Object.InputAuthority == _.playerRef)
            {
                Debug.Log("玩家死亡,正在复活玩家" + _.playerRef.RawEncoded);
                State_ReviveActor(_.reviveTime);
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<GameEvent.GameEvent_State_KillPlayer>().Subscribe(_ =>
        {
            if (Object.InputAuthority == _.playerRef)
            {
                State_KillActor();
            }
        }).AddTo(this);

        InvokeRepeating("UpdatePing", 2, 1);
    }
    public override void Spawned()
    {
        playerCoreLocal.AllClinet_InitPlayer(Object.HasInputAuthority, Object.HasStateAuthority);
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
    #region//角色销毁
    public void State_KillActor()
    {
        RPC_State_KillActor();
        State_ReviveActor(10);
        if (playerCoreLocal.actorManager_Bind)
        {
            playerCoreLocal.actorManager_Bind.actionManager.Despawn();
        }
    }
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPC_State_KillActor()
    {
        if (playerCoreLocal.bool_Local)
        {
            playerCoreLocal.Local_ResetActorData();
        }
    }

    #endregion
    #region//角色复活
    public void State_ReviveActor(float time)
    {
        RPC_State_PlayReviveCountdown(time);
        StartCoroutine(State_ReviveActorLater(time));
    }
    private IEnumerator State_ReviveActorLater(float time)
    {
        yield return new WaitForSeconds(time);
        State_CreateActor();
    }
    /// <summary>
    /// 通知本地播放复活动画
    /// </summary>
    /// <param name="time"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPC_State_PlayReviveCountdown(float time)
    {
        if (Object.HasInputAuthority)
        {
            StartCoroutine(Local_PlayReviveCountdown(time));
        }
    }
    /// <summary>
    /// 本地播放复活动画
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    private IEnumerator Local_PlayReviveCountdown(float time)
    {
        MessageBroker.Default.Publish(new UIEvent.UIEvent_OpenReviveCountdown() { time = time });
        WorldManager.Instance.ChangeSaturability(-100);
        yield return new WaitForSeconds(0.5f);
        if (WorldManager.Instance.FindPlayer(out ActorManager player))
        {
            CameraManager.Instance.FollowTarget(player.transform);
        }
        else
        {
            if (WorldManager.Instance.FindActor(out ActorManager actor))
            {
                CameraManager.Instance.FollowTarget(actor.transform);
            }
        }
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
                if (playerCoreLocal.bool_Local)
                {
                    MessageBroker.Default.Publish(new UIEvent.UIEvent_CloseReviveCountdown() { });
                    WorldManager.Instance.ChangeSaturability(0);
                }
                playerCoreLocal.AllClinet_BindActor(networkObject.GetComponent<ActorManager>());
            }
            else
            {
                Debug.Log("未找到Net_BindActorID:" + Net_BindActorID);
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
        if (playerCoreLocal.actorManager_Bind && playerCoreLocal.actorManager_Bind.actorNetManager.Object)
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
