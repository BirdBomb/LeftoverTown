using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorUI : MonoBehaviour
{
    public Transform HpPanel;
    public Transform HPBar;

    public void UpdateHPBar(float val)
    {
        HPBar.localScale = new Vector3(val, 1, 1);
    }
}
