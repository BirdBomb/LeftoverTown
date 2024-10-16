using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimaEventListen : MonoBehaviour
{
    private Action<string> tempEvent;
    private Action<string> commonEvent;
    public void BindTempEvent(Action<string> action)
    {
        if (action != null)
        {
            tempEvent = action;
        }
    }
    public void BindCommonEvent(Action<string> action)
    {
        if (action != null)
        {
            commonEvent = action;
        }
    }
    public void InvokeEvent(string name)
    {
        if (tempEvent != null)
        {
            tempEvent.Invoke(name);
            tempEvent = null;
        }
        if (commonEvent != null)
        {
            commonEvent.Invoke(name);
        }
    }
}
