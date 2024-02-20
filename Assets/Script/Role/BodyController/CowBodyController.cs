using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowBodyController : BaseBodyController
{
    public override void PlayBodyAction(BodyAction bodyAction, float speed, Action<string> action)
    {
        base.PlayBodyAction(bodyAction, speed, action);
    }
}
