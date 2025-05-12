using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorStatusManager 
{
    public StatusType statusType = StatusType.Human_Common;
    private ActorManager actorManager;
    public void Bind(ActorManager actorManager)
    {
        this.actorManager = actorManager;
    }
    /// <summary>
    /// 通用方法(审视某人)
    /// </summary>
    /// <param name="who"></param>
    /// <param name="handItemID"></param>
    /// <param name="headItemID"></param>
    /// <param name="bodyItemID"></param>
    public void Tool_CheckOutSomeone(ActorManager who, out short handItemID, out short headItemID, out short bodyItemID, out short fine)
    {
        handItemID = who.actorNetManager.Net_ItemInHand.Item_ID;
        headItemID = who.actorNetManager.Net_ItemOnHead.Item_ID;
        bodyItemID = who.actorNetManager.Net_ItemOnBody.Item_ID;
        fine = who.actorNetManager.Local_Fine;
    }
    /// <summary>
    /// 通用方法(检查身份)
    /// </summary>
    /// <param name="clothes"></param>
    /// <param name="hat"></param>
    /// <param name="fine"></param>
    /// <param name="id"></param>
    public void Tool_CheckStatus(short clothes, short hat, short fine, out StatusType status)
    {
        if (hat == 0 && clothes == 0)
        {
            status = statusType;
        }
        else
        {
            StatusConfig statusConfig = StatusConfigData.statusConfigs.Find((x) => { return x.Status_Hat == hat || x.Status_Clothes == clothes; });
            if (statusConfig.Status_ID == 0)
            {
                status = statusType;
            }
            else
            {
                status = statusConfig.Status_Type;
            }
        }
    }

}
