using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class GameLocalManager : SingleTon<GameLocalManager>, ISingleTon
{
    public PlayerController localPlayer;
    public void Init()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_Local_BindLocalPlayer>().Subscribe(_ =>
        {
            localPlayer = _.player;
        }).AddTo(this);
    }
}
