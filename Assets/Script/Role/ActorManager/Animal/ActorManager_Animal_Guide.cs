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
    public GameObject prefab_DialogUI;
    public GameObject prefab_DealUI;
    public GameObject prefab_DictionayUI;
    private TileUI_Dialog tileUI_Dialog;
    private TileUI_Deal tileUI_Deal;
    private TileUI_Dictionary tileUI_Dictionary;
    private bool bool_Dialog = false;
    private bool bool_Deal = false;
    private bool bool_Dictionay = false;
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
        Local_OverDeal(actor);
        Local_OverDictionary(actor);
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
                    tileUI_Dialog.InitDialog("Role_String", "Guide_Name", "Role_String", "Guide_Dialog0");
                    dialogOption_0.optionTable = "Role_String";
                    dialogOption_0.optionEntry = "Guide_Dialog0_Option0";
                    dialogOption_0.optionAction = (() =>
                    {
                        Local_StartDialog(actor, 1);
                    });
                    dialogOption_1.optionTable = "Role_String";
                    dialogOption_1.optionEntry = "Guide_Dialog0_Option1";
                    dialogOption_1.optionAction = (() =>
                    {
                        Local_StartDeal(actor);
                    });
                    dialogOption_2.optionTable = "Role_String";
                    dialogOption_2.optionEntry = "Guide_Dialog0_Option2";
                    dialogOption_2.optionAction = (() =>
                    {
                        Local_StartDictionary(actor);
                    });

                    dialogOptions.Add(dialogOption_0);
                    dialogOptions.Add(dialogOption_1);
                    dialogOptions.Add(dialogOption_2);
                    tileUI_Dialog.InitOption(dialogOptions);
                    break;
                case 1:
                    tileUI_Dialog.InitDialog("Role_String", "Guide_Name", "Role_String", "Guide_Dialog1");
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
                        Local_StartDialog(actor, 2);
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
                    tileUI_Dialog.InitOption(dialogOptions);
                    break;
                case 2:
                    tileUI_Dialog.InitDialog("Role_String", "Guide_Name", "Role_String", "Guide_Dialog2");
                    dialogOption_0.optionTable = "Role_String";
                    dialogOption_0.optionEntry = "Guide_Dialog2_Option0";
                    dialogOption_0.optionAction = (() =>
                    {
                        Local_OverDialog(actor);
                    });

                    dialogOptions.Add(dialogOption_0);
                    tileUI_Dialog.InitOption(dialogOptions);
                    break;
                case 3:
                    tileUI_Dialog.InitDialog("Role_String", "Guide_Name", "Role_String", "Guide_Dialog3");
                    dialogOption_0.optionTable = "Role_String";
                    dialogOption_0.optionEntry = "Guide_Dialog3_Option0";
                    dialogOption_0.optionAction = (() =>
                    {
                        Local_OverDialog(actor);
                    });

                    dialogOptions.Add(dialogOption_0);
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
    /// <summary>
    /// 开始交易
    /// </summary>
    /// <param name="actor"></param>
    private void Local_StartDeal(ActorManager actor)
    {
        bool_Deal = true;
        if (actor != null && actor.actorNetManager.Object != null)
        {
            UIManager.Instance.ShowTileUI(prefab_DealUI, out TileUI tileUI);
            tileUI_Deal = tileUI.GetComponent<TileUI_Deal>();
            List<ItemData> itemsDatas_Bag = actorNetManager.Local_GetBagItem();
            List<ItemData> itemDatas_Goods = new List<ItemData>();
            for (int i = 0; i < itemsDatas_Bag.Count; i++)
            {
                ItemConfig itemConfig = ItemConfigData.GetItemConfig(itemsDatas_Bag[i].Item_ID);
                if (itemConfig.Item_Type != ItemType.Weapon)
                {
                    itemDatas_Goods.Add(itemsDatas_Bag[i]);
                }
            }
            tileUI_Deal.Init(this, itemDatas_Goods, Local_Offer);
        }
    }
    /// <summary>
    /// 结束交易
    /// </summary>
    /// <param name="actor"></param>
    private void Local_OverDeal(ActorManager actor)
    {
        if (bool_Deal && actor != null && actor.actorNetManager.Object != null)
        {
            bool_Deal = false;
            UIManager.Instance.HideTileUI(tileUI_Deal);
        }
    }
    /// <summary>
    /// 开始字典
    /// </summary>
    /// <param name="actor"></param>
    private void Local_StartDictionary(ActorManager actor)
    {
        bool_Dictionay = true;
        if (actor != null && actor.actorNetManager.Object != null)
        {
            UIManager.Instance.ShowTileUI(prefab_DictionayUI, out TileUI tileUI);
            tileUI_Dictionary = tileUI.GetComponent<TileUI_Dictionary>();
            tileUI_Dictionary.Init();
        }
    }
    /// <summary>
    /// 结束字典
    /// </summary>
    /// <param name="actor"></param>
    private void Local_OverDictionary(ActorManager actor)
    {
        if (bool_Dictionay && actor != null && actor.actorNetManager.Object != null)
        {
            bool_Dictionay = false;
            UIManager.Instance.HideTileUI(tileUI_Dictionary);
        }
    }
    /// <summary>
    /// 收购
    /// </summary>
    /// <param name="itemData"></param>
    /// <returns></returns>
    public int Local_Offer(ItemData itemData)
    {
        ItemConfig itemConfig = ItemConfigData.GetItemConfig(itemData.Item_ID);
        int offer = itemConfig.Item_Value * itemData.Item_Count;
        return offer;
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
