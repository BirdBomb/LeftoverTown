using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Fusion;

public class NetEvent 
{
    public class NetEvent_CreateGame
    {
        public string RoomName;
        public int RoomType;
    }
    public class NetEvent_JoinGame
    {
        public SessionInfo RoomInfo;
        public string RoomName;
    }
    public class NetEvent_QuitGame
    {
        
    }
    public class NetEvent_JoinSessionLobby
    {
       
    }

    public class NetEvent_SessionListUpdated
    {
        public List<SessionInfo> SessionList = new List<SessionInfo>();
    }
}
