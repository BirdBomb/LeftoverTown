using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorConfigData : MonoBehaviour
{

    public struct ActorConfig
    {
        [SerializeField]/*编号*/
        public int Actor_ID;
        [SerializeField]/*名字*/
        public string Actor_Name;
        [SerializeField]/*资源名*/
        public string Actor_FileName;
    }
}

