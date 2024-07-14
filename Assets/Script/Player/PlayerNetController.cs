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
            MessageBroker.Default.Publish(new GameEvent.GameEvent_Local_BindLocalPlayer() 
            { 
                player = this.playerController
            });
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
        playerController.playerData = GameDataManager.Instance.LoadPlayerData();
        if(playerController.playerData != null)
        {
            Debug.Log("--玩家信息获取成功,初始化玩家背包");
            for (int i = 0; i < playerController.playerData.BagItems.Count; i++)
            {
                playerController.actorManager.NetManager.RPC_LocalInput_AddItemInBag(playerController.playerData.BagItems[i]);
            }
            Debug.Log("--玩家信息获取成功,初始化玩家手部");
            playerController.actorManager.NetManager.RPC_LocalInput_AddItemOnHand(playerController.playerData.HandItem);
            Debug.Log("--玩家信息获取成功,初始化玩家头部");
            playerController.actorManager.NetManager.RPC_LocalInput_AddItemOnHead(playerController.playerData.HandItem);
            Debug.Log("--玩家信息获取成功,初始化玩家身体");
            playerController.actorManager.NetManager.RPC_LocalInput_AddItemOnBody(playerController.playerData.HandItem);
            PlayerNetData playerNetData = new PlayerNetData();
            playerNetData.Speed = playerController.playerData.Speed;
            playerNetData.MaxSpeed = playerController.playerData.MaxSpeed;
            playerNetData.En = playerController.playerData.En;
            playerNetData.Pos = playerController.playerData.Pos;
            playerNetData.Hp = playerController.playerData.Hp;
            playerNetData.MaxHp = playerController.playerData.MaxHp;
            playerNetData.Food = playerController.playerData.Food;
            playerNetData.MaxFood = playerController.playerData.MaxFood;
            playerNetData.San = playerController.playerData.San;
            playerNetData.MaxSan = playerController.playerData.MaxSan;
            playerNetData.Point_Strength = playerController.playerData.Point_Strength;
            playerNetData.Point_Intelligence = playerController.playerData.Point_Intelligence;
            playerNetData.Point_Focus = playerController.playerData.Point_Focus;
            playerNetData.Point_Agility = playerController.playerData.Point_Agility;
            Debug.Log("--玩家信息获取成功,初始化玩家数据");
            RPC_Input_InitPlayerData(playerNetData);
            Debug.Log("--玩家信息获取成功,根据玩家坐标生成周围地图");
            MessageBroker.Default.Publish(new MapEvent.MapEvent_LocalTile_RequestMapData()
            {
                pos = playerController.playerData.Pos,
                player = Object.InputAuthority
            });
        }
        else
        {
            Debug.Log("--玩家信息获取失败,初始化玩家背包");
        }
    }
    private Vector2 moveDir_temp;
    private bool left_press = false;
    private bool right_press = false;
    public override void FixedUpdateNetwork()
    {
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
        if (GetInput(out NetworkInputData data))
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

            playerController.All_PlayerInputMove(dt, moveDir_temp, Net_PressShift, Object.HasStateAuthority, Object.HasInputAuthority);
        }
        /*服务器*/
        #region
        //if (Object.HasStateAuthority)
        //{
        //    playerController.State_PlayerInputMove(dt, moveDir_temp, Net_PressShift, Object.HasStateAuthority, Object.HasInputAuthority);
        //}
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
    public void RPC_Input_InitPlayerData(PlayerNetData netData)
    {
        /*初始化位置*/
        playerController.actorManager.NetManager.UpdateNetworkTransform(netData.Pos, 999);
        playerController.actorManager.NetManager.Data_Hp = netData.Hp;
        playerController.actorManager.NetManager.Data_MaxHp = netData.MaxHp;
        playerController.actorManager.NetManager.NetData.Speed = netData.Speed;
        playerController.actorManager.NetManager.NetData.MaxSpeed = netData.MaxSpeed;
        playerController.actorManager.NetManager.NetData.Endurance = netData.En;
        playerController.actorManager.NetManager.NetData.Point_Strength = netData.Point_Strength;
        playerController.actorManager.NetManager.NetData.Point_Intelligence = netData.Point_Intelligence;
        playerController.actorManager.NetManager.NetData.Point_Focus = netData.Point_Focus;
        playerController.actorManager.NetManager.NetData.Point_Agility = netData.Point_Agility;
    }

}
public struct PlayerNetData : INetworkStruct
{
    public Vector3 Pos;
    public int Hp;
    public int MaxHp;
    public int Food;
    public int MaxFood;
    public int San;
    public int MaxSan;
    public int Speed;
    public int MaxSpeed;
    public int En;
    public int Point_Strength;
    public int Point_Intelligence;
    public int Point_Focus;
    public int Point_Agility;
}

