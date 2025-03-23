using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Grid : MonoBehaviour
{
    public Action<string> action_ChangeInfo;
    /// <summary>
    /// 绑定数据回调
    /// </summary>
    public virtual void BindAction_ChangeInfo(Action<string> callBack)
    {
        action_ChangeInfo = callBack;
    }
    public virtual void Open()
    {

    }

    public virtual void Close()
    {

    }
}
