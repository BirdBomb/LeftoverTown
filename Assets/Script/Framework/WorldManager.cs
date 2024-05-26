using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.U2D;
using DG.Tweening;
using static UnityEngine.Rendering.DebugUI;

public class WorldManager : SingleTon<FileManager>, ISingleTon
{
    [SerializeField]
    private Light2D globalLight;
    private GlobalTime globalTimeNow;
    private GlobalTime GlobalTimeNow
    {
        get { return globalTimeNow; }
        set { globalTimeNow = value; }
    }
    public void Init()
    {
        
    }
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            UpdateGlobalTime(GlobalTimeNow);
        }
    }
    private void UpdateGlobalTime(GlobalTime globalTime)
    {
        if (globalTime == GlobalTime.Morning)
        {
            DOTween.To(() => globalLight.color, x => globalLight.color = x, new Color(1, 1, 0, 1), 10f);
            DOTween.To(() => globalLight.intensity, x => globalLight.intensity = x, 1f, 10f);
        }
        if (globalTime == GlobalTime.HighNoon)
        {
            DOTween.To(() => globalLight.color, x => globalLight.color = x, new Color(1, 1, 0, 1), 10f);
            DOTween.To(() => globalLight.intensity, x => globalLight.intensity = x, 1f, 10f);
        }
        if (globalTime == GlobalTime.Dusk)
        {
            DOTween.To(() => globalLight.color, x => globalLight.color = x, new Color(1, 0.75f, 0, 1), 10f);
            DOTween.To(() => globalLight.intensity, x => globalLight.intensity = x, 0.75f, 10f);
        }
        if (globalTime == GlobalTime.Evening)
        {
            DOTween.To(() => globalLight.color, x => globalLight.color = x, new Color(0.25f, 0, 1, 1), 10f);
            DOTween.To(() => globalLight.intensity, x => globalLight.intensity = x, 0.5f, 20f);
        }
        GlobalTimeNow = GlobalTimeNow + 1;
    }
}
public enum GlobalTime
{
    Default,
    Morning,
    HighNoon,
    Dusk,
    Evening,
}