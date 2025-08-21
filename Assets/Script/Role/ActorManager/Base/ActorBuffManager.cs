using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class ActorBuffManager
{
    private ActorManager actorManager;
    private Dictionary<short, BuffBase> bindBuffDic = new Dictionary<short, BuffBase>();
    private List<short> bindBuffIDList = new List<short>();
    private List<BuffBase> bindBuffEntity = new List<BuffBase>();
    public void Bind(ActorManager actorManager)
    {
        this.actorManager = actorManager;
    }
    public void Listen_UpdateSecond()
    {
        for(int i = 0; i < bindBuffEntity.Count; i++)
        {
            bindBuffEntity[i].Listen_UpdateSecond(actorManager);
        }
    }
    public void Listen_Move(Vector3Int pos)
    {
        for (int i = 0; i < bindBuffEntity.Count; i++)
        {
            bindBuffEntity[i].Listen_MyselfMove(actorManager);
        }
    }
    public void Local_InitBuffs(List<BuffData> buffDatas)
    {
        bindBuffEntity.Clear();
        bindBuffIDList.Clear();
        bindBuffDic.Clear();
        for (int i = 0; i < buffDatas.Count;i++)
        {
            Local_InitBuff(buffDatas[i]);
        }
        if(actorManager.actorAuthority.isPlayer && actorManager.actorAuthority.isLocal)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateBuffList()
            {
                buffList = buffDatas
            });
        }
    }
    public bool Local_CheckBuff(short buffID)
    {
        return bindBuffIDList.Contains(buffID);
    }
    public void Local_InitBuff(BuffData buffData)
    {
        Type type = Type.GetType("Buff" + buffData.BuffID.ToString());
        BuffBase buff = (BuffBase)Activator.CreateInstance(type);
        bindBuffEntity.Add(buff);
        bindBuffIDList.Add(buffData.BuffID);
        bindBuffDic.Add(buffData.BuffID, buff);
        buff.Init(buffData);
    }
    public void Local_AddBuff(BuffData buffData)
    {
        Debug.Log("Ìí¼ÓBuff" + buffData);
        if (!bindBuffIDList.Contains(buffData.BuffID))
        {
            Type type = Type.GetType("Buff" + buffData.BuffID.ToString());
            BuffBase buff = (BuffBase)Activator.CreateInstance(type);
            bindBuffEntity.Add(buff);
            bindBuffIDList.Add(buffData.BuffID);
            bindBuffDic.Add(buffData.BuffID, buff);
            buff.Init(buffData);
            buff.Listen_AddOnActor(actorManager);

            if (actorManager.actorAuthority.isPlayer && actorManager.actorAuthority.isLocal)
            {
                MessageBroker.Default.Publish(new UIEvent.UIEvent_AddBuff()
                {
                    buffData = buffData
                });
            }
        }
    }
    public void Local_RemoveBuff(short buffID)
    {
        if (bindBuffIDList.Contains(buffID))
        {
            BuffBase buff = bindBuffDic[buffID];
            bindBuffEntity.Remove(buff);
            bindBuffIDList.Remove(buffID);
            bindBuffDic.Remove(buffID);
            buff.Listen_SubFromActor(actorManager);

            if (actorManager.actorAuthority.isPlayer && actorManager.actorAuthority.isLocal)
            {
                MessageBroker.Default.Publish(new UIEvent.UIEvent_SubBuff()
                {
                    buffID = buffID
                });
            }
        }
    }
    public void All_PlayBuffEffect(short id,short index)
    {
        Type type = Type.GetType("Buff" + id.ToString());
        BuffBase buff = (BuffBase)Activator.CreateInstance(type);
        buff.PlayEffect(actorManager, index);
    }
}
