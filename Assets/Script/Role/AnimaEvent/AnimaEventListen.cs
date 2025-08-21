using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimaEventListen : MonoBehaviour
{
    private List<Func<string, bool>> FuncList = new List<Func<string, bool>>();
    private List<Action<string>> ActionList = new List<Action<string>>();

    public void BindTempFunc(Func<string, bool> func)
    {
        if (!FuncList.Contains(func))
        {
            FuncList.Add(func);
        }
    }
    public void BindCommonEvent(Action<string> action)
    {
        if (action != null)
        {
            ActionList.Add(action);
        }
    }
    public void InvokeEvent(string name)
    {
        for (int i = 0; i < FuncList.Count; i++)
        {
            if (FuncList[i].Invoke(name))
            {
                FuncList.Remove(FuncList[i]);
            }
        }
        for (int i = 0; i < ActionList.Count; i++)
        {
            ActionList[i].Invoke(name);
        }
    }
}
