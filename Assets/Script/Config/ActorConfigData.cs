using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorConfigData : MonoBehaviour
{

    public struct ActorConfig
    {
        [SerializeField]/*���*/
        public int Actor_ID;
        [SerializeField]/*����*/
        public string Actor_Name;
        [SerializeField]/*��Դ��*/
        public string Actor_FileName;
    }
}

