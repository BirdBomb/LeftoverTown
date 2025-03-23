using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController_Zombie_Hook : BodyController_Zombie
{
    [SerializeField, Header("右脚节点")]
    public Transform Tran_RightLeg;
    [SerializeField, Header("左脚节点")]
    public Transform Tran_LeftLeg;


}
