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
    /// ��ʼ���������
    /// </summary>
    public void InitData()
    {
        PlayerData playerData = GameDataManager.Instance.LoadPlayerData();
        if(playerData != null)
        {
            Debug.Log("�����Ϣ��ȡ�ɹ�,����Ϣͬ�������пͻ���");
            RPC_InitData(playerData.Pos);
            Debug.Log("�����Ϣ��ȡ�ɹ�,�����������������Χ��ͼ");
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
        /*���пͻ��˸�����·����ģ���������*/
        #region
        playerController.All_PlayerInputMouse(Runner.DeltaTime, Net_Left_ClickTime, Net_Right_ClickTime, Net_LeftPress, Net_RightPress, Object.HasStateAuthority, Object.HasInputAuthority);
        playerController.All_PlayerInputMove(Runner.DeltaTime, Net_MoveDir, Net_PressShift, Object.HasStateAuthority, Object.HasInputAuthority);
        playerController.All_PlayerInputFace(Runner.DeltaTime, Net_Face, Object.HasStateAuthority, Object.HasInputAuthority);
        playerController.All_PlayerInputControl(Net_Q_ClickTime, Net_F_ClickTime, Net_Space_ClickTime, Object.HasStateAuthority, Object.HasInputAuthority);
        #endregion
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
        /*��ʼ��λ��*/
        playerController.actorManager.NetController.UpdateNetworkTransform(pos);
    }
}

