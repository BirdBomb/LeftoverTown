using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UniRx;
using UnityEngine.Windows;
using System.Numerics;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;

public class PlayerNetController : NetworkBehaviour
{
    [SerializeField, Header("角色控制器")]
    private PlayerController playerController;
    [SerializeField, Header("角色摄像机")]
    private Camera playerCamera;
    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            playerCamera.tag = "MainCamera";
            playerController.thisPlayerIsMe = true;
        }
        else
        {
            playerCamera.gameObject.SetActive(false);
        }
        if (Object.HasStateAuthority)
        {
            playerController.thisPlayerIsState = true;
        }
        else
        {
            playerController.thisPlayerIsState = false;
        }
        playerController.thisPlayerID = Object.InputAuthority.PlayerId;
        if (Object.HasInputAuthority) { InitData(); }
        base.Spawned();
    }
    /// <summary>
    /// 初始化玩家数据
    /// </summary>
    public void InitData()
    {
        PlayerData playerData = GameDataManager.Instance.LoadPlayerData();
        if(playerData != null)
        {
            Debug.Log("--玩家信息获取成功,初始化玩家背包");
            for (int i = 0; i < playerData.Bag.Count; i++)
            {
                playerController.actorManager.NetManager.RPC_LocalInput_AddItemInBag(playerData.Bag[i]);
            }
            Debug.Log("--玩家信息获取成功,初始化玩家数据");
            RPC_InitData(playerData.Pos, playerData.Hp, playerData.MaxHp);
            Debug.Log("--玩家信息获取成功,根据玩家坐标生成周围地图");
            MessageBroker.Default.Publish(new MapEvent.MapEvent_LocalTile_RequestMapData()
            {
                pos = playerData.Pos,
                player = Object.InputAuthority
            });
        }
    }
    private Vector2 moveDir_temp;
    private bool left_press = false;
    private bool right_press = false;
    public override void FixedUpdateNetwork()
    {
        if (playerController.actorManager.actorState == ActorState.Dead) return;
        /*服务器获取输入端输入并同步给所有客户端*/
        if (Object.HasStateAuthority && GetInput(out NetworkInputData data))
        {
            moveDir_temp = Vector2.zero;
            if (data.ClickLeftMouse > 0)
            {
                Net_Left_ClickTime += data.ClickLeftMouse;
            }
            if (data.ClickRightMouse > 0)
            {
                Net_Right_ClickTime += data.ClickRightMouse;
            }
            if (data.PressLeftMouse)
            {
                Net_LeftPress = true;
            }
            else
            {
                Net_LeftPress = false;
            }
            if (data.PressRightMouse)
            {
                Net_RightPress = true;
            }
            else
            {
                Net_RightPress = false;
            }

            if (data.goRight)
            {
                moveDir_temp += new Vector2(1, 0);
            }
            if (data.goLeft)
            {
                moveDir_temp += new Vector2(-1, 0);
            }
            if (data.goUp)
            {
                moveDir_temp += new Vector2(0, 1);
            }
            if (data.goDown)
            {
                moveDir_temp += new Vector2(0, -1);
            }
            Net_PressShift = data.PressShift;
            Net_Face = data.faceDir;
        }
        PlayerInputByFix(Runner.DeltaTime);
        base.FixedUpdateNetwork();
    }
    float lastRender;
    public override void Render()
    {
        float dt = Runner.LocalRenderTime - lastRender;
        lastRender = Runner.LocalRenderTime;
        PlayerInputByRender(dt);
        base.Render();
    }
    public void PlayerInputByRender(float dt)
    {
        /*服务器*/
        #region
        //if (Object.HasStateAuthority)
        //{
        //    playerController.State_PlayerInputMove(dt, moveDir_temp, Net_PressShift, Object.HasStateAuthority, Object.HasInputAuthority);
        //}
        #endregion
        /*客户端*/
        #region
        playerController.All_PlayerInputFace(dt, Net_Face, Object.HasStateAuthority, Object.HasInputAuthority);
        playerController.All_PlayerInputMouse(dt, Net_Left_ClickTime, Net_Right_ClickTime, Net_LeftPress, Net_RightPress, Object.HasStateAuthority, Object.HasInputAuthority);
        #endregion
        base.Render();
    }
    private void PlayerInputByFix(float dt)
    {
        if (playerController.actorManager.actorState == ActorState.Dead) return;
        /*服务器*/
        #region
        if (Object.HasStateAuthority)
        {
            playerController.State_PlayerInputMove(dt, moveDir_temp, Net_PressShift, Object.HasStateAuthority, Object.HasInputAuthority);
        }
        #endregion
        /*客户端*/
        #region
        //playerController.All_PlayerInputFace(dt, Net_Face, Object.HasStateAuthority, Object.HasInputAuthority);
        //playerController.All_PlayerInputMouse(dt, Net_Left_ClickTime, Net_Right_ClickTime, Net_LeftPress, Net_RightPress, Object.HasStateAuthority, Object.HasInputAuthority);
        #endregion
    }
    [Networked]
    public int Net_Left_ClickTime { get; set; } = 0;
    [Networked]
    public int Net_Right_ClickTime { get; set; } = 0;
    [Networked]
    public bool Net_LeftPress { get; set; } = false;
    [Networked]
    public bool Net_RightPress { get; set; } = false;
    [Networked]
    public bool Net_PressShift { get; set; } = false;
    [Networked]
    public Vector3 Net_Face { get; set; } = Vector3.zero;

    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_InitData(Vector3Int pos, int hp, int maxHp)
    {
        /*初始化位置*/
        playerController.actorManager.NetManager.UpdateNetworkTransform(pos);
        playerController.actorManager.NetManager.Data_Hp = 50;
        playerController.actorManager.NetManager.Data_MaxHp = 50;
    }

}

