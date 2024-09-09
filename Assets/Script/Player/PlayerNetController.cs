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
            playerController.localPlayerRef = Object.InputAuthority;
            InitByLocalPlayer();
        }
        else
        {
            playerController.localPlayerRef = Object.InputAuthority;
            InitByOtherPlayer();
        }
        if (Object.HasStateAuthority)
        {
            playerController.thisPlayerIsState = true;
            InitByStatePlayer();
        }
        else
        {
            playerController.thisPlayerIsState = false;
        }
        base.Spawned();
    }
    /// <summary>
    /// 作为本地玩家初始化
    /// </summary>
    private void InitByLocalPlayer()
    {
        playerCamera.gameObject.SetActive(true);
        playerCamera.tag = "MainCamera";
        playerController.thisPlayerIsMe = true;
        MessageBroker.Default.Publish(new GameEvent.GameEvent_Local_BindLocalPlayer()
        {
            player = playerController
        });
        InitLocalPlayerData();
    }
    /// <summary>
    /// 作为其他玩家初始化
    /// </summary>
    private void InitByOtherPlayer()
    {
        playerCamera.gameObject.SetActive(false);
    }
    /// <summary>
    /// 作为主机玩家初始化
    /// </summary>
    private void InitByStatePlayer()
    {
        playerController.actorManager.NetManager.StartLoop();
    }
    /// <summary>
    /// 初始化本地玩家数据
    /// </summary>
    public void InitLocalPlayerData()
    {
        GameDataManager.Instance.LoadPlayer(out playerController.localPlayerData);
        if(playerController.localPlayerData != null)
        {
            Debug.Log("--玩家信息获取成功--");
            Debug.Log("--初始化玩家背包");
            playerController.actorManager.NetManager.RPC_LocalInput_ChangeBagCapacity(5);
            for (int i = 0; i < playerController.localPlayerData.BagItems.Count; i++)
            {
                playerController.actorManager.NetManager.RPC_LocalInput_AddItemInBag(playerController.localPlayerData.BagItems[i]);
            }
            Debug.Log("--初始化玩家手部");
            playerController.actorManager.NetManager.RPC_LocalInput_AddItemOnHand(playerController.localPlayerData.HandItem);
            Debug.Log("--初始化玩家头部");
            playerController.actorManager.NetManager.RPC_LocalInput_AddItemOnHead(playerController.localPlayerData.HeadItem);
            Debug.Log("--初始化玩家身体");
            playerController.actorManager.NetManager.RPC_LocalInput_AddItemOnBody(playerController.localPlayerData.BodyItem);
            Debug.Log("--初始化玩家数据");
            playerController.actorManager.NetManager.RPC_LocalInput_InitPlayerCommonData(CreatePlayerNetData(playerController.localPlayerData));
            Debug.Log("--初始化玩家Buff与Skill");
            for (int i = 0; i < playerController.localPlayerData.BuffList.Count; i++)
            {
                playerController.actorManager.NetManager.RPC_LocalInput_AddBuff(playerController.localPlayerData.BuffList[i]);
            }
            for (int i = 0; i < playerController.localPlayerData.SkillKnowList.Count; i++)
            {
                playerController.actorManager.NetManager.RPC_LocalInput_AddSkillKnow(playerController.localPlayerData.SkillKnowList[i]);
            }
            for (int i = 0; i < playerController.localPlayerData.SkillUseList.Count; i++)
            {
                playerController.actorManager.NetManager.RPC_LocalInput_AddSkillUse(playerController.localPlayerData.SkillUseList[i]);
            }

            Debug.Log("--初始化玩家位置");
            playerController.actorManager.NetManager.UpdateNetworkTransform(playerController.localPlayerData.Pos, 999);
            Debug.Log("--绘制玩家周围地图");
            playerController.UpdateMapInView(playerController.localPlayerData.Pos);
        }
        else
        {
            Debug.Log("--玩家信息获取失败,初始化玩家背包");
        }
    }
    private PlayerNetData CreatePlayerNetData(PlayerData playerData)
    {
        PlayerNetData playerNetData = new PlayerNetData();

        playerNetData.HairID = playerData.HairID;
        playerNetData.HairColor = playerData.HairColor;
        playerNetData.EyeID = playerData.EyeID;

        playerNetData.Pos = playerData.Pos;
        playerNetData.CommonSpeed = playerData.CommonSpeed;
        playerNetData.MaxSpeed = playerData.MaxSpeed;
        playerNetData.CurHp = playerData.CurHp;
        playerNetData.MaxHp = playerData.MaxHp;
        playerNetData.Armor = playerData.Armor;
        playerNetData.CurFood = playerData.CurFood;
        playerNetData.MaxFood = playerData.MaxFood;
        playerNetData.Water = playerData.Water;
        playerNetData.CurSan = playerData.CurSan;
        playerNetData.MaxSan = playerData.MaxSan;
        playerNetData.Happy = playerData.Happy;
        playerNetData.Coin = playerData.Coin;
        playerNetData.En = playerData.En;

        playerNetData.Point_Strength = playerData.Point_Strength;
        playerNetData.Point_Intelligence = playerData.Point_Intelligence;
        playerNetData.Point_Agility = playerData.Point_Agility;
        playerNetData.Point_Focus = playerData.Point_Focus;
        playerNetData.Point_SPower = playerData.Point_SPower;
        playerNetData.Point_Make = playerData.Point_Make;
        playerNetData.Point_Build = playerData.Point_Build;
        playerNetData.Point_Cook = playerData.Point_Cook;

        return playerNetData;
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
}
public struct PlayerNetData : INetworkStruct
{
    public Vector3 Pos;
    public int CurHp;
    public int MaxHp;
    public short Armor;
    public short CurFood;
    public short MaxFood;
    public short Water;
    public short CurSan;
    public short MaxSan;
    public short Happy;
    public short Coin;
    public short CommonSpeed;
    public short MaxSpeed;
    public int En;
    public short Point_Strength;
    public short Point_Intelligence;
    public short Point_Focus;
    public short Point_Agility;
    public short Point_SPower;
    public short Point_Make;
    public short Point_Build;
    public short Point_Cook;

    public int HairID;
    public int EyeID;
    public Color32 HairColor;
}

