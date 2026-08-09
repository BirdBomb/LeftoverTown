using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SignalUI_Unlock : MonoBehaviour
{
    public GameObject transform_Unlock;
    public GameObject transform_Lock;
    public Transform transform_Bar;
    public SpriteRenderer spriteRenderer_Bar;
    public TextMesh text_Pro;
    public void UpdateState(bool _Unlock, int _pro)
    {
        transform_Bar.DOKill();
        transform_Bar.localScale = new Vector3(0, 1, 1);
        transform_Unlock.SetActive(_Unlock);
        transform_Lock.SetActive(!_Unlock);
        text_Pro.text = Mathf.Min(_pro, 99).ToString() + "%";
    }
    public void PlayUnlock(float time)
    {
        spriteRenderer_Bar.color = Color.green;
        transform_Bar.DOKill();
        transform_Bar.localScale = new Vector3(0, 1, 1);
        transform_Bar.DOScaleX(1, time);
    }
    public void PlayUnlockResult(bool success)
    {
        transform_Unlock.SetActive(success);
        transform_Lock.SetActive(!success);
        spriteRenderer_Bar.color = success ? Color.green : Color.red;
        if (!success)
        {
            transform_Lock.transform.localPosition = Vector3.zero;
            transform_Lock.transform.DOKill();
            transform_Lock.transform.DOShakePosition(0.1f, new Vector3(0.1f, 0.1f, 0));
        }
    }
}
