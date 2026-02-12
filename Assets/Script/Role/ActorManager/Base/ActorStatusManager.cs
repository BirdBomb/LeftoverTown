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
    /// 设置身份
    /// </summary>
    public void SetStaus(StatusType status)
    {
        statusType = status;
    }
    /// <summary>
    /// 获取身份
    /// </summary>
    public StatusType GetStaus()
    {
        return statusType;
    }
}
