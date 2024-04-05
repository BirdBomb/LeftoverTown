using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public class NetEvent 
{
    public class NetEvent_JoinGame
    {
        /// <summary>
        /// 服务器
        /// </summary>
        public bool IsState;
        /// <summary>
        /// 房间名称
        /// </summary>
        public string RoomName;
        /// <summary>
        /// 角色路径
        /// </summary>
        public string ActorFilePath;
        /// <summary>
        /// 地图路径
        /// </summary>
        public string MapFilePath;
    }
}
