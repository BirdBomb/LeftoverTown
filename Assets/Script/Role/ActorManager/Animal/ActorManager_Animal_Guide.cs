using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using static GameEvent;

public class ActorManager_Animal_Guide : ActorManager
{
    [Header("向导配置")]
    public ActorConfig_Guide config;
    /// <summary>
    /// 出生点
    /// </summary>
    private Vector3Int vector3_HomePos;
    [SerializeField]
    private GameObject prefab_DialogUI;
    [SerializeField]
    private GameObject prefab_DealUI;
    private TileUI_Dialog tileUI_Dialog;
    private TileUI_DealUI tileUI_Deal;
    private bool bool_Dialog = false;
    private bool bool_Deal = false;
    #region//初始化
    public override void Client_Init()
    {
        statusManager.statusType = config.status_Type;
        base.Client_Init();
    }
    public override void State_Init()
    {
        State_InitNPCData();
        base.State_Init();
    }
    /// <summary>
    /// 初始化数据
    /// </summary>
    private void State_InitNPCData()
    {
        List<ItemData> itemDatas = new List<ItemData>();
        State_SetAbilityData(config.short_Hp, config.short_Armor, config.short_Resistance, config.short_MoveSpeed);
        actorNetManager.Local_SetBagItem(itemDatas);
    }
    /// <summary>
    /// 绑定出生点
    /// </summary>
    /// <param name="pos"></param>
    public void State_BindRabbitHole(Vector3Int pos)
    {
        vector3_HomePos = pos;
    }
    #endregion
    #region//交互
    public override void Local_InPlayerView(ActorManager actor)
    {
        actorUI.ShowSingalF();
        base.Local_InPlayerView(actor);
    }
    public override void Local_OutPlayerView(ActorManager actor)
    {
        actorUI.HideSingal();
        Local_OverDialog(actor);
        base.Local_OutPlayerView(actor);
    }
    public override void Local_GetPlayerInput(KeyCode keyCode, ActorManager actor)
    {
        actorUI.ShowSingalTalk();
        Local_StartDialog(actor, 0);
        base.Local_GetPlayerInput(keyCode, actor);
    }
    /// <summary>
    /// 开始谈话
    /// </summary>
    /// <param name="actor"></param>
    /// <param name="index"></param>
    private void Local_StartDialog(ActorManager actor, int index)
    {
        bool_Dialog = true;
        if (actor != null && actor.actorNetManager.Object != null)
        {
            actorNetManager.RPC_State_NpcUseSkill(0, actor.pathManager.vector3Int_CurPos, actor.actorNetManager.Object.Id);
            UIManager.Instance.ShowTileUI(prefab_DialogUI, out TileUI tileUI);
            tileUI_Dialog = tileUI.GetComponent<TileUI_Dialog>();

            List<DialogOption> dialogOptions = new List<DialogOption>();
            DialogOption dialogOption_0 = new DialogOption();
            DialogOption dialogOption_1 = new DialogOption();
            DialogOption dialogOption_2 = new DialogOption();
            switch (index)
            {
                case 0:
                    dialogOption_0.optionTable = "Role_String";
                    dialogOption_0.optionEntry = "Guide_Dialog0_Option0";
                    dialogOption_0.optionAction = (() =>
                    {
                        Local_OverDialog(actor);
                    });
                    dialogOption_1.optionTable = "Role_String";
                    dialogOption_1.optionEntry = "Guide_Dialog0_Option1";
                    dialogOption_1.optionAction = (() =>
                    {
                        Local_StartDialog(actor, 2);
                    });
                    dialogOption_2.optionTable = "Role_String";
                    dialogOption_2.optionEntry = "Guide_Dialog0_Option2";
                    dialogOption_2.optionAction = (() =>
                    {
                        Local_StartDialog(actor, 1);
                    });

                    dialogOptions.Add(dialogOption_0);
                    dialogOptions.Add(dialogOption_1);
                    dialogOptions.Add(dialogOption_2);
                    tileUI_Dialog.InitDialog("Role_String", "Guide_Name", "Role_String", "Guide_Dialog0");
                    tileUI_Dialog.InitOption(dialogOptions);
                    break;
                case 1:
                    dialogOption_0.optionTable = "Role_String";
                    dialogOption_0.optionEntry = "Guide_Dialog1_Option0";
                    dialogOption_0.optionAction = (() =>
                    {
                        Local_OverDialog(actor);
                    });
                    dialogOption_1.optionTable = "Role_String";
                    dialogOption_1.optionEntry = "Guide_Dialog1_Option1";
                    dialogOption_1.optionAction = (() =>
                    {
                        Local_StartDialog(actor, 4);
                    });

                    dialogOption_2.optionTable = "Role_String";
                    dialogOption_2.optionEntry = "Guide_Dialog1_Option2";
                    dialogOption_2.optionAction = (() =>
                    {
                        Local_StartDialog(actor, 3);
                    });

                    dialogOptions.Add(dialogOption_0);
                    dialogOptions.Add(dialogOption_1);
                    dialogOptions.Add(dialogOption_2);
                    tileUI_Dialog.InitDialog("Role_String", "Guide_Name", "Role_String", "Guide_Dialog1");
                    tileUI_Dialog.InitOption(dialogOptions);
                    break;
                case 2:
                    dialogOption_0.optionTable = "Role_String";
                    dialogOption_0.optionEntry = "Guide_Dialog2_Option0";
                    dialogOption_0.optionAction = (() =>
                    {
                        Local_OverDialog(actor);
                    });
                    dialogOption_1.optionTable = "Role_String";
                    dialogOption_1.optionEntry = "Guide_Dialog2_Option1";
                    dialogOption_1.optionAction = (() =>
                    {
                        Local_StartDialog(actor, 4);
                    });

                    dialogOption_2.optionTable = "Role_String";
                    dialogOption_2.optionEntry = "Guide_Dialog2_Option2";
                    dialogOption_2.optionAction = (() =>
                    {
                        Local_StartDialog(actor, 3);
                    });

                    dialogOptions.Add(dialogOption_0);
                    dialogOptions.Add(dialogOption_1);
                    dialogOptions.Add(dialogOption_2);
                    tileUI_Dialog.InitDialog("Role_String", "Guide_Name", "Role_String", "Guide_Dialog2");
                    tileUI_Dialog.InitOption(dialogOptions);
                    break;
                case 3:
                    dialogOption_0.optionTable = "Role_String";
                    dialogOption_0.optionEntry = "Guide_Dialog3_Option0";
                    dialogOption_0.optionAction = (() =>
                    {
                        Local_OverDialog(actor);
                    });
                    dialogOption_1.optionTable = "Role_String";
                    dialogOption_1.optionEntry = "Guide_Dialog3_Option1";
                    dialogOption_1.optionAction = (() =>
                    {
                        Local_StartDialog(actor, 4);
                    });

                    dialogOption_2.optionTable = "Role_String";
                    dialogOption_2.optionEntry = "Guide_Dialog3_Option2";
                    dialogOption_2.optionAction = (() =>
                    {
                        Local_StartDialog(actor, 5);
                    });

                    dialogOptions.Add(dialogOption_0);
                    dialogOptions.Add(dialogOption_1);
                    dialogOptions.Add(dialogOption_2);
                    tileUI_Dialog.InitDialog("Role_String", "Guide_Name", "Role_String", "Guide_Dialog3");
                    tileUI_Dialog.InitOption(dialogOptions);
                    break;
                case 4:
                    dialogOption_0.optionTable = "Role_String";
                    dialogOption_0.optionEntry = "Guide_Dialog4_Option0";
                    dialogOption_0.optionAction = (() =>
                    {
                        Local_OverDialog(actor);
                    });
                    dialogOption_1.optionTable = "Role_String";
                    dialogOption_1.optionEntry = "Guide_Dialog4_Option1";
                    dialogOption_1.optionAction = (() =>
                    {
                        Local_StartDialog(actor, 3);
                    });

                    dialogOption_2.optionTable = "Role_String";
                    dialogOption_2.optionEntry = "Guide_Dialog4_Option2";
                    dialogOption_2.optionAction = (() =>
                    {
                        Local_StartDialog(actor, 5);
                    });

                    dialogOptions.Add(dialogOption_0);
                    dialogOptions.Add(dialogOption_1);
                    dialogOptions.Add(dialogOption_2);
                    tileUI_Dialog.InitDialog("Role_String", "Guide_Name", "Role_String", "Guide_Dialog4");
                    tileUI_Dialog.InitOption(dialogOptions);
                    break;
                case 5:
                    dialogOption_0.optionTable = "Role_String";
                    dialogOption_0.optionEntry = "Guide_Dialog5_Option0";
                    dialogOption_0.optionAction = (() =>
                    {
                        Local_OverDialog(actor);
                    });
                    dialogOptions.Add(dialogOption_0);
                    tileUI_Dialog.InitDialog("Role_String", "Guide_Name", "Role_String", "Guide_Dialog5");
                    tileUI_Dialog.InitOption(dialogOptions);
                    break;
            }
        }
    }
    /// <summary>
    /// 结束谈话
    /// </summary>
    /// <param name="actor"></param>
    private void Local_OverDialog(ActorManager actor)
    {
        if (bool_Dialog && actor != null && actor.actorNetManager.Object != null)
        {
            bool_Dialog = false;
            actorNetManager.RPC_State_NpcUseSkill(1, actor.pathManager.vector3Int_CurPos, actor.actorNetManager.Object.Id);
            UIManager.Instance.HideTileUI(tileUI_Dialog);
        }
    }

    #endregion
    #region//技能
    public override void AllClient_Listen_NpcAction(int id, Vector3Int vector3, NetworkId networkId)
    {
        if (id == 0)
        {
            AllClient_StartTalk(vector3, networkId);
        }
        else if (id == 1)
        {
            AllClient_OverTalk(vector3, networkId);
        }
        base.AllClient_Listen_NpcAction(id, vector3, networkId);
    }
    private void AllClient_StartTalk(Vector3Int vector3, NetworkId networkId)
    {
        if (actorAuthority.isState)
        {
            Vector3 dir = (actorNetManager.Runner.FindObject(networkId).transform.position - transform.position).normalized;
            if (dir.x > 0)
            {
                bodyController.TurnRight();
            }
            else
            {
                bodyController.TurnLeft();
            }
            pathManager.State_StandDown(999);
        }
    }
    private void AllClient_OverTalk(Vector3Int vector3, NetworkId networkId)
    {
        if (actorAuthority.isState)
        {
            pathManager.State_StandDown(1);
        }
    }
    #endregion

}
[Serializable]
public struct ActorConfig_Guide
{
    [Header("初始身份")]
    public StatusType status_Type;
    [Header("生命")]
    public short short_Hp;
    [Header("护甲")]
    public short short_Armor;
    [Header("魔抗")]
    public short short_Resistance;
    [Header("移动速度")]
    public short short_MoveSpeed;
    [Header("移动距离"), Range(1, 10)]
    public short short_MoveStep;
    [Header("视野距离"), Range(1, 99)]
    public float float_ViewDistance;
}
