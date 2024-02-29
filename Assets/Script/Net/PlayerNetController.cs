using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UniRx;

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
                RPC_AddItemInBag(ItemConfigLocalToNet(_.itemConfig), Object.InputAuthority);
            }).AddTo(this);
            MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_AddItemInHand>().Subscribe(_ =>
            {
                RPC_AddItemInHand(ItemConfigLocalToNet(_.itemConfig), Object.InputAuthority);
            }).AddTo(this);

            playerCamera.tag = "MainCamera";
            playerController.thisPlayerIsMe = true;
        }
        else
        {
            playerCamera.gameObject.SetActive(false);
            transform.position = RealPosition;
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


        base.Spawned();
    }
    private Vector2 moveDir_temp;
    private bool left_press = false;
    private bool right_press = false;
    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority && GetInput(out NetworkInputData data))
        {
            moveDir_temp = Vector2.zero;
            if (data.ClickLeftMouse > 0)
            {
                LeftClickTime += data.ClickLeftMouse;
            }
            if (data.ClickRightMouse > 0)
            {
                RightClickTime += data.ClickRightMouse;
            }
            if (data.ClickQ > 0)
            {
                QClickTime += data.ClickQ;
            }
            if (data.ClickF > 0)
            {
                FClickTime += data.ClickF;
            }
            if (data.ClickSpace > 0)
            {
                SpaceClickTime += data.ClickSpace;
            }
            if (data.PressLeftMouse)
            {
                left_press = true;
            }
            else
            {
                left_press = false;
            }
            if (data.PressRightMouse)
            {
                right_press = true;
            }
            else
            {
                right_press = false;
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
            LeftPress = left_press;
            RightPress = right_press;
            MoveDir = moveDir_temp;
            MoveSpeedUp = data.goFaster;
            Face = data.facePostion;
            RealPosition = transform.position;
        }
        base.FixedUpdateNetwork();
    }
    [Networked]
    public int LeftClickTime { get; set; } = 0;
    [Networked]
    public int RightClickTime { get; set; } = 0;
    [Networked]
    public int QClickTime { get; set; } = 0;
    [Networked]
    public int FClickTime { get; set; } = 0;
    [Networked]
    public int SpaceClickTime { get; set; } = 0;
    [Networked]
    public bool LeftPress { get; set; } = false;
    [Networked]
    public bool RightPress { get; set; } = false;
    [Networked]
    public Vector2 MoveDir { get; set; } = Vector2.zero;
    [Networked]
    public bool MoveSpeedUp { get; set; } = false;
    [Networked]
    public Vector3 Face { get; set; } = Vector3.zero;
    [Networked]
    public Vector3 RealPosition { get; set; }
    public void FixedUpdate()
    {
        playerController.InputMouse(LeftClickTime, RightClickTime, LeftPress, RightPress, Time.fixedDeltaTime,Object.HasInputAuthority);
        playerController.InputMoveDir(MoveDir, Time.fixedDeltaTime, MoveSpeedUp);
        playerController.InputFaceDir(Face, Time.fixedDeltaTime);
        playerController.InputControl(QClickTime, FClickTime, SpaceClickTime);
    }
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_AddItemInHand(NetworkItemConfig itemConfig,PlayerRef player)
    {
        Debug.Log("监听到玩家" + player);
        if(player == Object.InputAuthority)
        {
            Debug.Log("玩家" + Object.InputAuthority + "添加物品" + itemConfig.Item_Name + "到持握");
            playerController.baseBehaviorController.AddItem_Hand(ItemConfigNetToLocal(itemConfig), Object.HasInputAuthority);
        }
    }
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_AddItemInBag(NetworkItemConfig itemConfig, PlayerRef player)
    {
        Debug.Log("监听到玩家" + player);
        if (player == Object.InputAuthority)
        {
            Debug.Log("玩家" + Object.InputAuthority + "添加物品" + itemConfig.Item_Name + "到背包");
            playerController.baseBehaviorController.AddItem_Bag(ItemConfigNetToLocal(itemConfig), Object.HasInputAuthority);
        }
    }
    private ItemConfig ItemConfigNetToLocal(NetworkItemConfig config)
    {
        ItemConfig itemConfig = new ItemConfig();
        itemConfig.Item_ID = config.Item_ID;
        itemConfig.Item_Name = config.Item_Name.Value;
        itemConfig.Item_Desc = config.Item_Desc.Value;
        itemConfig.Item_CurCount = config.Item_CurCount;
        itemConfig.Item_MaxCount = config.Item_MaxCount;
        itemConfig.Item_Type = config.Item_Type;
        itemConfig.Average_Weight = config.Average_Weight;
        itemConfig.Average_Value = config.Average_Value;
        itemConfig.Item_Info = config.Item_Info.Value;
        return itemConfig;
    }
    private NetworkItemConfig ItemConfigLocalToNet(ItemConfig config)
    {
        NetworkItemConfig itemConfig = new NetworkItemConfig();
        itemConfig.Item_ID = config.Item_ID;
        itemConfig.Item_Name = config.Item_Name;
        itemConfig.Item_Desc = config.Item_Desc;
        itemConfig.Item_CurCount = config.Item_CurCount;
        itemConfig.Item_MaxCount = config.Item_MaxCount;
        itemConfig.Item_Type = config.Item_Type;
        itemConfig.Average_Weight = config.Average_Weight;
        itemConfig.Average_Value = config.Average_Value;
        itemConfig.Item_Info = config.Item_Info;
        return itemConfig;
    }
    public struct NetworkItemConfig : INetworkStruct
    {
        public int Item_ID;
        public NetworkString<_16> Item_Name;
        public NetworkString<_16> Item_Desc;
        public int Item_CurCount;
        public int Item_MaxCount;
        public ItemType Item_Type;
        public float Average_Weight;
        public float Average_Value;
        public NetworkString<_256> Item_Info;
    }
}

