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
    private Dictionary<short, BuffBase> dictionary_bindBuffDic = new Dictionary<short, BuffBase>();
    private List<short> shorts_bindBuffIDList = new List<short>();
    private List<BuffBase> buffBases_bindBuffEntity = new List<BuffBase>();
    public void Bind(ActorManager actorManager)
    {
        this.actorManager = actorManager;
    }
    public void Listen_UpdateSecond()
    {
        foreach (BuffBase buff in buffBases_bindBuffEntity)
        {
            buff.Listen_UpdateSecond(actorManager);
        }
    }
    public void Listen_Move(Vector3Int pos)
    {
        for (int i = 0; i < buffBases_bindBuffEntity.Count; i++)
        {
            buffBases_bindBuffEntity[i].Listen_MyselfMove(actorManager);
        }
    }
    public void UpdateBuff(NetworkLinkedList<short> buffs)
    {
        foreach (short buff in buffs)
        {
            AddBuff(buff);
        }
        for (int i = 0; i < shorts_bindBuffIDList.Count; i++)
        {
            if (!buffs.Contains(shorts_bindBuffIDList[i]))
            {
                RemoveBuff(shorts_bindBuffIDList[i]);
            }
        }
    }
    public bool CheckBuff(short id)
    {
        return shorts_bindBuffIDList.Contains(id);
    }
    private void AddBuff(short id)
    {
        if (!shorts_bindBuffIDList.Contains(id))
        {
            Type type = Type.GetType("Buff" + id.ToString());
            BuffBase buff = (BuffBase)Activator.CreateInstance(type);
            dictionary_bindBuffDic.Add(id, buff);
            buffBases_bindBuffEntity.Add(buff);
            shorts_bindBuffIDList.Add(id);
            buff.Listen_AddOnActor(actorManager);

            if (actorManager.actorAuthority.isPlayer && actorManager.actorAuthority.isLocal)
            {
                MessageBroker.Default.Publish(new UIEvent.UIEvent_AddBuff()
                {
                    buffConfig = BuffConfigData.GetBuffConfig(id)
                });
            }
        }
    }
    private void RemoveBuff(short id)
    {
        if (shorts_bindBuffIDList.Contains(id))
        {
            BuffBase buff = dictionary_bindBuffDic[id];
            buffBases_bindBuffEntity.Remove(buff);
            shorts_bindBuffIDList.Remove(id);
            dictionary_bindBuffDic.Remove(id);
            buff.Listen_SubFromActor(actorManager);

            if (actorManager.actorAuthority.isPlayer && actorManager.actorAuthority.isLocal)
            {
                MessageBroker.Default.Publish(new UIEvent.UIEvent_SubBuff()
                {
                    buffConfig = BuffConfigData.GetBuffConfig(id)
                });
            }
        }
    }
}
