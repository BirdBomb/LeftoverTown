using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public class GameEvent 
{
    /// <summary>
    /// Ä³ÈËÒÆ¶¯
    /// </summary>
    public class GameEvent_SomeoneMove
    {
        public BaseBehaviorController moveRole;
        public MyTile moveTile;
    }
}
