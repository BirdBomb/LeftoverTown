using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Fusion;

public class NetEvent 
{
    public class NetEvent_SessionListUpdated
    {
        public List<SessionInfo> SessionList = new List<SessionInfo>();
    }
}
