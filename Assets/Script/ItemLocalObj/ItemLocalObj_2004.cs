using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLocalObj_2004 : ItemLocalObj
{
    public Transform fire;
    private void FixedUpdate()
    {
        fire.rotation = Quaternion.identity;
    }
}
