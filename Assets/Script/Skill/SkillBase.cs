using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase 
{
    public int skillID;
    public ActorManager bindActor;
    /// <summary>
    /// ≥ı ºªØ
    /// </summary>
    /// <param name="id"></param>
    /// <param name="actor"></param>
    public virtual void Init(int id,ActorManager actor)
    {
        skillID = id;
        bindActor = actor;
    }
    public virtual void Listen_ActorMove()
    {

    }
    public virtual void ClickSpace()
    {

    } 
    public virtual void HoldingSpace()
    {

    }
}
