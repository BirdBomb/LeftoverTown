using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBase : MonoBehaviour
{
    [SerializeField, Header("��������")]
    private float life;
    [SerializeField, Header("����·��")]
    private string path;
    public void Hide()
    {
        PoolManager.Instance.ReleaseObject(path, gameObject);
    }
    private void OnEnable()
    {
        Invoke("Hide", life);
    }
    private void OnDisable()
    {
        CancelInvoke();
    }
}
