using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public class NetEvent 
{
    public class NetEvent_JoinGame
    {
        /// <summary>
        /// ������
        /// </summary>
        public bool IsState;
        /// <summary>
        /// ��������
        /// </summary>
        public string RoomName;
        /// <summary>
        /// ��ɫ·��
        /// </summary>
        public string ActorFilePath;
        /// <summary>
        /// ��ͼ·��
        /// </summary>
        public string MapFilePath;
    }
}
