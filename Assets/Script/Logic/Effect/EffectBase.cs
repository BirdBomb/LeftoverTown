using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBase : MonoBehaviour
{
    [SerializeField, Header("生命周期")]
    protected float life;
    [SerializeField, Header("加载路径")]
    private string path;
    public void Hide()
    {
        PoolManager.Instance.ReleaseObject(path, gameObject);
    }
    public virtual void SetEffect(Vector3 dir)
    {

    }
    public virtual void OnEnable()
    {
        Invoke("Hide", life);
    }
    public virtual void OnDisable()
    {
        CancelInvoke();
    }
}
