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
        switch (time)
        {
            case GlobalTime.Evening:
                {
                    if (pathManager.vector3Int_CurPos == brainManager.state_homePostion.position)
                    {
                        actionManager.Despawn();
                    }
                    else
                    {
                        if (!State_Think_GoToHome()) State_Think_GoToStroll_Long(4, 5);
                    }
                    return;
                }
        }
        State_Think_GoToStroll_Long(4, 5);
    }
    public override void State_ThinkByTimeChange(int date, int hour, GlobalTime time)
    {
        base.State_ThinkByTimeUpdate(date, hour, time);
    }
}
