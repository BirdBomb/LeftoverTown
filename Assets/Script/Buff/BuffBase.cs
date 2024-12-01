using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffBase 
{
    /// <summary>
    /// 触发节点:人物创建添加
    /// </summary>
    /// <param name="data"></param>
    public virtual void Listen_AddOnPlayerCreation(ref PlayerData data)
    {

    }
    /// <summary>
    /// 触发节点:人物创建去除
    /// </summary>
    /// <param name="data"></param>
    public virtual void Listen_SubOnPlayerCreation(ref PlayerData data)
    {

    }
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
}
