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
    [SerializeField, Header("��ɫ������")]
    private PlayerController playerController;
    [SerializeField, Header("��ɫ�����")]
    private Camera playerCamera;
    #region//��ʼ��
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
    /// ��Ϊ������ҳ�ʼ��
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
    /// ��Ϊ������ҳ�ʼ��
    /// </summary>
    private void InitByOtherPlayer()
    {
        playerCamera.gameObject.SetActive(false);
    }
    /// <summary>
    /// ��Ϊ������ҳ�ʼ��
    /// </summary>
    private void InitByStatePlayer()
    {

    }
    /// <summary>
    /// ��ʼ�������������
    /// </summary>
    public void InitLocalPlayerData()
    {
        GameDataManager.Instance.LoadPlayer(out playerController.localPlayerData);
        if (playerController.localPlayerData != null)
        {
            Debug.Log("--�����Ϣ��ȡ�ɹ�--");
            //Debug.Log("--��ʼ����ұ���");
            playerController.actorManager.NetManager.RPC_LocalInput_ChangeBagCapacity(10);
            for (int i = 0; i < playerController.localPlayerData.BagItems.Count; i++)
            {
                playerController.actorManager.NetManager.RPC_LocalInput_AddItemInBag(playerController.localPlayerData.BagItems[i]);
            }
            //Debug.Log("--��ʼ������ֲ�");
            playerController.actorManager.NetManager.RPC_LocalInput_AddItemOnHand(playerController.localPlayerData.HandItem);
            //Debug.Log("--��ʼ�����ͷ��");
            playerController.actorManager.NetManager.RPC_LocalInput_AddItemOnHead(playerController.localPlayerData.HeadItem);
            //Debug.Log("--��ʼ���������");
            playerController.actorManager.NetManager.RPC_LocalInput_AddItemOnBody(playerController.localPlayerData.BodyItem);
            //Debug.Log("--��ʼ���������");
            playerController.actorManager.NetManager.RPC_LocalInput_InitPlayerCommonData(CreatePlayerNetData(playerController.localPlayerData));
            //Debug.Log("--��ʼ�����Buff��Skill");
            for (int i = 0; i < playerController.localPlayerData.BuffList.Count; i++)
            {
                playerController.actorManager.NetManager.RPC_LocalInput_AddBuff(playerController.localPlayerData.BuffList[i],"");
            }
            for (int i = 0; i < playerController.localPlayerData.SkillKnowList.Count; i++)
            {
                playerController.actorManager.NetManager.RPC_LocalInput_AddSkillKnow(playerController.localPlayerData.SkillKnowList[i]);
            }
            for (int i = 0; i < playerController.localPlayerData.SkillUseList.Count; i++)
            {
                playerController.actorManager.NetManager.RPC_LocalInput_AddSkillUse(playerController.localPlayerData.SkillUseList[i]);
            }
            //Debug.Log("--��ʼ�����λ��");
            playerController.actorManager.NetManager.RPC_LocalInput_UpdateNetworkTransform(playerController.localPlayerData.Pos, 999);
            //Debug.Log("--���������Χ��ͼ");
            playerController.UpdateMapInView(playerController.localPlayerData.Pos);
        }
        else
        {
            Debug.Log("--�����Ϣ��ȡʧ��--");
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

    #endregion
    #region//���������ͬ��
    [Networked]
    public float MouseRightPressTimer { get; set; }
    [Networked]
    public float MouseLeftPressTimer { get; set; }
    [Networked]
    public Vector2 MouseLocation { get; set; }
    private Vector2 moveDir_temp;

    public override void FixedUpdateNetwork()
    {
        PlayerInput(Runner.DeltaTime);
        base.FixedUpdateNetwork();
    }
    public override void Render()
    {
        PlayerSync();
        base.Render();
    }
    /// <summary>
    /// �������
    /// </summary>
    /// <param name="dt"></param>
    private void PlayerInput(float dt)
    {
        if (playerController.actorManager.actorState == ActorState.Dead) return;
        if (GetInput(out NetworkInputData netPlayerData))
        {
            moveDir_temp = Vector2.zero;
            if (netPlayerData.PressD)
            {
                moveDir_temp += new Vector2(1, 0);
            }
            if (netPlayerData.PressA)
            {
                moveDir_temp += new Vector2(-1, 0);
            }
            if (netPlayerData.PressW)
            {
                moveDir_temp += new Vector2(0, 1);
            }
            if (netPlayerData.PressS)
            {
                moveDir_temp += new Vector2(0, -1);
            }

            playerController.AllClient_PlayerInputMove(dt, moveDir_temp, netPlayerData.PressLeftShift, Object.HasStateAuthority, Object.HasInputAuthority);
            MouseRightPressTimer = netPlayerData.MouseRightPressTimer;
            MouseLeftPressTimer = netPlayerData.MouseLeftPressTimer;
            MouseLocation = netPlayerData.MouseLocation;
        }
    }
    /// <summary>
    /// ���ͬ��
    /// </summary>
    private void PlayerSync()
    {
        playerController.AllClient_PlayerInputFace(MouseLocation, Object.HasStateAuthority, Object.HasInputAuthority);
        playerController.AllClient_PlayerInputMouse(MouseLeftPressTimer, MouseRightPressTimer, Object.HasStateAuthority, Object.HasInputAuthority);
    }

    #endregion
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

