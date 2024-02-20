using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimaEventListen : MonoBehaviour
{
    private Action<string> tempEvent;
    public void BindEvent(Action<string> action)
    {
        if (action != null)
        {
            tempEvent = action;
        }
    }
    public void InvokeEvent(string name)
    {
        if (tempEvent != null)
        {
            tempEvent.Invoke(name);
            tempEvent = null;
        }
    }
}
