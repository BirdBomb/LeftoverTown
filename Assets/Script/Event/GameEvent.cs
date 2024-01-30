using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public class GameEvent 
{
    public class GameEvent_SomeoneMove
    {
        public BaseBehaviorController moveRole;
        public MyTile moveTile;
    }
}
