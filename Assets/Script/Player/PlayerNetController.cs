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
            MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_AddItemInBag>().Subscribe(_ =>
            {
                playerController.actorManager.NetController.Data_ItemInBag.Add(_.networkItemConfig);
            }).AddTo(this);
            MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_AddItemInHand>().Subscribe(_ =>
            {
                playerController.actorManager.NetController.Data_ItemInHand = _.networkItemConfig;
            }).AddTo(this);

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
            Debug.Log("玩家信息获取成功,把信息同步给所有客户端");
            RPC_InitData(playerData.Pos);
            Debug.Log("玩家信息获取成功,根据玩家坐标生成周围地图");
            MessageBroker.Default.Publish(new MapEvent.MapEvent_RequestMapData()
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
        /*所有客户端根据网路数据模拟玩家输入*/
        #region
        playerController.All_PlayerInputMouse(Runner.DeltaTime, Net_Left_ClickTime, Net_Right_ClickTime, Net_LeftPress, Net_RightPress, Object.HasStateAuthority, Object.HasInputAuthority);
        playerController.All_PlayerInputMove(Runner.DeltaTime, Net_MoveDir, Net_PressShift, Object.HasStateAuthority, Object.HasInputAuthority);
        playerController.All_PlayerInputFace(Runner.DeltaTime, Net_Face, Object.HasStateAuthority, Object.HasInputAuthority);
        playerController.All_PlayerInputControl(Net_Q_ClickTime, Net_F_ClickTime, Net_Space_ClickTime, Object.HasStateAuthority, Object.HasInputAuthority);
        #endregion
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
            if (data.ClickQ > 0)
            {
                Net_Q_ClickTime += data.ClickQ;
            }
            if (data.ClickF > 0)
            {
                Net_F_ClickTime += data.ClickF;
            }
            if (data.ClickSpace > 0)
            {
                Net_Space_ClickTime += data.ClickSpace;
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
            Net_MoveDir = moveDir_temp;
            Net_PressShift = data.PressShift;
            Net_Face = data.facePostion;
        }
        base.FixedUpdateNetwork();
    }
    [Networked]
    public int Net_Left_ClickTime { get; set; } = 0;
    [Networked]
    public int Net_Right_ClickTime { get; set; } = 0;
    [Networked]
    public int Net_Q_ClickTime { get; set; } = 0;
    [Networked]
    public int Net_F_ClickTime { get; set; } = 0;
    [Networked]
    public int Net_Space_ClickTime { get; set; } = 0;
    [Networked]
    public bool Net_LeftPress { get; set; } = false;
    [Networked]
    public bool Net_RightPress { get; set; } = false;
    [Networked]
    public Vector2 Net_MoveDir { get; set; } = Vector2.zero;
    [Networked]
    public bool Net_PressShift { get; set; } = false;
    [Networked]
    public Vector3 Net_Face { get; set; } = Vector3.zero;

    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_InitData(Vector3Int pos)
    {
        /*初始化位置*/
        playerController.actorManager.NetController.UpdateNetworkTransform(pos);
    }
}

