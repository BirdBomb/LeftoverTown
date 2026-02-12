using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffBase 
{
    public BuffData data;
    public virtual void SetData(BuffData buffData)
    {
        data = buffData;
    }
    public virtual void GetData(out BuffData buffData)
    {
        buffData = data;
    }
    #region//游戏内监听
    /// <summary>
    /// 触发节点:初始化
    /// </summary>
    public virtual void Listen_Local_Init(ActorManager actor)
    {

    }
    /// <summary>
    /// 触发节点:添加
    /// </summary>
    public virtual void Listen_Local_AddOnActor(ActorManager actor)
    {

    }
    /// <summary>
    /// 触发节点:移除
    /// </summary>
    public virtual void Listen_Local_SubFromActor(ActorManager actor)
    {

    }
    /// <summary>
    /// 触发节点:血量更新
    /// </summary>
    public virtual void Listen_Local_UpdateHp(ActorManager actor)
    {

    }
    /// <summary>
    /// 触发节点:饥饿更新
    /// </summary>
    public virtual void Listen_Local_UpdateHungry(ActorManager actor)
    {

    }
    /// <summary>
    /// 触发节点:精神更新
    /// </summary>
    public virtual void Listen_Local_UpdateSan(ActorManager actor)
    {

    }
    /// <summary>
    /// 触发节点:秒更新
    /// </summary>
    public virtual void Listen_Local_UpdateSecond(ActorManager actor)
    {

    }
    /// <summary>
    /// 触发节点:我自己移动
    /// </summary>
    public virtual void Listen_MyselfMove(ActorManager actor, Vector3Int pos)
    {

    }

    #endregion
    #region//外置方法
    /// <summary>
    /// 播放特效
    /// </summary>
    public virtual void PlayEffect(ActorManager actor, int index)
    {

    }
    /// <summary>
    /// 更新图标
    /// </summary>
    /// <param name="buffIcon"></param>
    /// <param name="buffData"></param>
    public virtual void UpdateIcon(UI_BuffIcon buffIcon, BuffData buffData)
    {

    }
    #endregion
}
[Serializable]
public struct BuffData
{
    public short BuffID;
    public short BuffVal;
    public Vector3Int BuffPos;
    public BuffData(short id)
    {
        BuffID = id;
        BuffVal = 0;
        BuffPos = Vector3Int.zero;
    }
    public BuffData(short id,short val,Vector3Int pos)
    {
        BuffID = id;
        BuffVal = val;
        BuffPos = pos;
    }
}
