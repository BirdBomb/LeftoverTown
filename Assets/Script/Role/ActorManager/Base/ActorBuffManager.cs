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
    #region//监听
    /// <summary>
    /// 监听秒更新
    /// </summary>
    public void Listen_UpdateSecond()
    {
        if (actorManager.actorAuthority.isLocal)
        {
            for (int i = 0; i < bindBuffEntity.Count; i++)
            {
                bindBuffEntity[i].Listen_Local_UpdateSecond(actorManager);
            }
        }
    }
    /// <summary>
    /// 监听移动
    /// </summary>
    /// <param name="pos"></param>
    public void Listen_Move(Vector3Int pos)
    {
        if (actorManager.actorAuthority.isLocal)
        {
            for (int i = 0; i < bindBuffEntity.Count; i++)
            {
                bindBuffEntity[i].Listen_MyselfMove(actorManager, pos);
            }
        }
    }
    /// <summary>
    /// 监听生命值改变
    /// </summary>
    public void Listen_UpdateHp()
    {
        if (actorManager.actorAuthority.isLocal)
        {
            for (int i = 0; i < bindBuffEntity.Count; i++)
            {
                bindBuffEntity[i].Listen_Local_UpdateHp(actorManager);
            }
        }
    }
    /// <summary>
    /// 监听饥饿值改变
    /// </summary>
    public void Listen_UpdateHungry()
    {
        if (actorManager.actorAuthority.isLocal)
        {
            for (int i = 0; i < bindBuffEntity.Count; i++)
            {
                bindBuffEntity[i].Listen_Local_UpdateHungry(actorManager);
            }
        }
    }
    public void Listen_UpdateSan()
    {
        if (actorManager.actorAuthority.isLocal)
        {
            for (int i = 0; i < bindBuffEntity.Count; i++)
            {
                bindBuffEntity[i].Listen_Local_UpdateSan(actorManager);
            }
        }
    }

    #endregion
    #region//基本方法
    /// <summary>
    /// 初始Buff
    /// </summary>
    /// <param name="buffDatas"></param>
    public void Local_InitBuffs(List<BuffData> buffDatas)
    {
        bindBuffEntity.Clear();
        bindBuffIDList.Clear();
        bindBuffDic.Clear();
        MessageBroker.Default.Publish(new UIEvent.UIEvent_ClearBuff(){ });
        for (int i = 0; i < buffDatas.Count; i++)
        {
            BuffBase buff = Local_CreateBuff(buffDatas[i]);
            bindBuffEntity.Add(buff);
            bindBuffIDList.Add(buffDatas[i].BuffID);
            bindBuffDic.Add(buffDatas[i].BuffID, buff);
        }
        Local_EssentialBuffs();
    }
    /// <summary>
    /// 必要Buff
    /// </summary>
    private void Local_EssentialBuffs()
    {
        Local_AddBuff(new BuffData(100));
        Local_AddBuff(new BuffData(101));
        Local_AddBuff(new BuffData(102));
    }
    /// <summary>
    /// 创建Buff
    /// </summary>
    /// <param name="buffData"></param>
    /// <returns></returns>
    public BuffBase Local_CreateBuff(BuffData buffData)
    {
        Type type = Type.GetType("Buff" + buffData.BuffID.ToString());
        BuffBase buff = (BuffBase)Activator.CreateInstance(type);
        buff.SetData(buffData);
        buff.Listen_Local_Init(actorManager);
        return buff;
    }
    /// <summary>
    /// 检查Buff
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool Local_CheckBuff<T>(short id,out T buffBase) where T : BuffBase 
    {
        if (bindBuffDic.TryGetValue(id, out var buff))
        {
            buffBase = buff as T;
            return buffBase != null;
        }
        else
        {
            buffBase = null;
            return false;
        }
    }
    /// <summary>
    /// 添加Buff
    /// </summary>
    /// <param name="buffData"></param>
    public void Local_AddBuff(BuffData buffData)
    {
        if (!bindBuffIDList.Contains(buffData.BuffID))
        {
            BuffBase buff = Local_CreateBuff(buffData);
            buff.Listen_Local_AddOnActor(actorManager);
            bindBuffEntity.Add(buff);
            bindBuffIDList.Add(buffData.BuffID);
            bindBuffDic.Add(buffData.BuffID, buff);
        }
    }
    /// <summary>
    /// 获得Buff
    /// </summary>
    /// <param name="buffDatas"></param>
    public void Local_GetBuff(out List<BuffData> buffDatas)
    {
        List<BuffData> temp = new List<BuffData>();
        for (int i = 0; i < bindBuffEntity.Count; i++)
        {
            bindBuffEntity[i].GetData(out BuffData buffData);
            temp.Add(buffData);
        }
        buffDatas = temp;
    }
    /// <summary>
    /// 移除Buff
    /// </summary>
    /// <param name="buffID"></param>
    public void Local_RemoveBuff(short buffID)
    {
        if (bindBuffIDList.Contains(buffID))
        {
            BuffBase buff = bindBuffDic[buffID];
            bindBuffEntity.Remove(buff);
            bindBuffIDList.Remove(buffID);
            bindBuffDic.Remove(buffID);
            buff.Listen_Local_SubFromActor(actorManager);

            if (actorManager.actorAuthority.isPlayer && actorManager.actorAuthority.isLocal)
            {
                MessageBroker.Default.Publish(new UIEvent.UIEvent_SubBuff()
                {
                    buffID = buffID
                });
            }
        }
    }
    /// <summary>
    /// 播放Buff特效
    /// </summary>
    /// <param name="id"></param>
    /// <param name="index"></param>
    public void All_PlayBuffEffect(short id, short index)
    {
        Type type = Type.GetType("Buff" + id.ToString());
        BuffBase buff = (BuffBase)Activator.CreateInstance(type);
        buff.PlayEffect(actorManager, index);
    }

    #endregion
}
