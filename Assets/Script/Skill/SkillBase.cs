using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase 
{
    public ActorManager bindActor;
    public virtual void Init(ActorManager actor)
    {
        bindActor = actor;
    }
    public virtual void ClickSpace()
    {

    } 
    public virtual void HoldingSpace()
    {

    }
}
