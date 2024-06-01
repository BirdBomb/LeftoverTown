using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorUI : MonoBehaviour
{
    public Transform HpPanel;
    public Transform HPBar;
    public Transform EnPanel;
    public Transform EnBar;
    public Transform EnReleaseBar;

    public void UpdateHPBar(float val)
    {
        HPBar.localScale = new Vector3(val, 1, 1);
    }
    public void HighLightENBar()
    {
        EnBar.DOPunchScale(Vector3.one * 0.1f, 0.1f);
    }
    public void UpdateENBar(float val)
    {
        EnBar.localScale = new Vector3(val, 1, 1);
    }
    public void UpdateENReleaseBar(float val)
    {
        EnReleaseBar.localScale = new Vector3(val, 1, 1);
    }
}
