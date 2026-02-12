using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundObj : MonoBehaviour
{
    [HideInInspector]
    public GroundTile groundTile;
    /// <summary>
    /// 绑定
    /// </summary>
    public virtual void Bind(GroundTile tile,out GroundObj obj)
    {
        groundTile = tile;
        obj = this;
    }
    /// <summary>
    /// 绘制地块
    /// </summary>
    public virtual void Draw()
    {

    }
    /// <summary>
    /// 角色靠近
    /// </summary>
    /// <returns></returns>
    public virtual bool All_ActorNearby(ActorManager actor)
    {
        return false;
    }
    /// <summary>
    /// 角色站在
    /// </summary>
    public virtual void All_ActorStandOn(ActorManager actor)
    {

    }
    /// <summary>
    /// 角色远离
    /// </summary>
    /// <returns></returns>
    public virtual bool All_ActorFaraway(ActorManager actor)
    {
        return false;
    }
}
