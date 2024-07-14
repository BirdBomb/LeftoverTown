using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingleTon<T>: MonoBehaviour where T : SingleTon<T>, ISingleTon
{
    private void Start()
    {
        instance = this as T;
        instance.Init();
    }
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Lazy<T>(true).Value;
                instance.Init();
            }
            return instance;
        }
    }
}
public interface ISingleTon
{
    void Init();
}

