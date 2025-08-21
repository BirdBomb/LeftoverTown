using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffBase 
{
    public virtual void Init(BuffData buffData)
    {

    }
    #region//游戏内监听
    /// <summary>
    /// 触发节点:游戏开始
    /// </summary>
    public virtual void Listen_GameStart()
    {

    }
    /// <summary>
    /// 触发节点:添加
    /// </summary>
    public virtual void Listen_AddOnActor(ActorManager actor)
    {

    }
    /// <summary>
    /// 触发节点:移除
    /// </summary>
    public virtual void Listen_SubFromActor(ActorManager actor)
    {

    }
    /// <summary>
    /// 触发节点:我自己移动
    /// </summary>
    public virtual void Listen_MyselfMove(ActorManager actor)
    {

    }
    /// <summary>
    /// 触发节点:秒更新
    /// </summary>
    public virtual void Listen_UpdateSecond(ActorManager actor)
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
    public BuffData(short id)
    {
        BuffID = id;
        BuffVal = 0;
    }
}
