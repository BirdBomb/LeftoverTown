using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class GameLocalManager : SingleTon<GameLocalManager>, ISingleTon
{
    [HideInInspector]
    public PlayerCoreLocal playerCoreLocal;
    public void Init()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_Local_BindLocalPlayer>().Subscribe(_ =>
        {
            playerCoreLocal = _.playerCore;
        }).AddTo(this);
    }
}
