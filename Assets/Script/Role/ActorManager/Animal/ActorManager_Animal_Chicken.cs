using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using static GameEvent;

public class ActorManager_Animal_Chicken : ActorManager_Animal
{
    public override void State_ThinkByTimeUpdate(int date, int hour, GlobalTime time)
    {
        if (time == GlobalTime.Evening)
        {
            if (pathManager.vector3Int_CurPos == brainManager.state_homePostion.position)
            {
                actionManager.Despawn();
                return;
            }
        }
        State_Think_GoToStroll_Long(2, 5);
    }
    public override void State_ThinkByTimeChange(int date, int hour, GlobalTime time)
    {
        if (time == GlobalTime.Evening)
        {
            State_Think_GoToHome();
        }
        base.State_ThinkByTimeUpdate(date, hour, time);
    }

}
