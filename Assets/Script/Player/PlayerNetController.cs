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
    /// ��ʼ���������
    /// </summary>
    public void InitData()
    {
        PlayerData playerData = GameDataManager.Instance.LoadPlayerData();
        if(playerData != null)
        {
            Debug.Log("--�����Ϣ��ȡ�ɹ�,��ʼ����ұ���");
            for (int i = 0; i < playerData.Bag.Count; i++)
            {
                playerController.actorManager.NetManager.RPC_LocalInput_AddItemInBag(playerData.Bag[i]);
            }
            Debug.Log("--�����Ϣ��ȡ�ɹ�,��ʼ���������");
            RPC_InitData(playerData.Pos, playerData.Hp, playerData.MaxHp);
            Debug.Log("--�����Ϣ��ȡ�ɹ�,�����������������Χ��ͼ");
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
        /*��������ȡ��������벢ͬ�������пͻ���*/
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
        /*������*/
        #region
        //if (Object.HasStateAuthority)
        //{
        //    playerController.State_PlayerInputMove(dt, moveDir_temp, Net_PressShift, Object.HasStateAuthority, Object.HasInputAuthority);
        //}
        #endregion
        /*�ͻ���*/
        #region
        playerController.All_PlayerInputFace(dt, Net_Face, Object.HasStateAuthority, Object.HasInputAuthority);
        playerController.All_PlayerInputMouse(dt, Net_Left_ClickTime, Net_Right_ClickTime, Net_LeftPress, Net_RightPress, Object.HasStateAuthority, Object.HasInputAuthority);
        #endregion
        base.Render();
    }
    private void PlayerInputByFix(float dt)
    {
        if (playerController.actorManager.actorState == ActorState.Dead) return;
        /*������*/
        #region
        if (Object.HasStateAuthority)
        {
            playerController.State_PlayerInputMove(dt, moveDir_temp, Net_PressShift, Object.HasStateAuthority, Object.HasInputAuthority);
        }
        #endregion
        /*�ͻ���*/
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
        /*��ʼ��λ��*/
        playerController.actorManager.NetManager.UpdateNetworkTransform(pos);
        playerController.actorManager.NetManager.Data_Hp = 50;
        playerController.actorManager.NetManager.Data_MaxHp = 50;
    }

}

