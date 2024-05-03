using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    [HideInInspector]
    public Vector3 moveDir;
    [HideInInspector]
    public float moveSpeed;
    [HideInInspector]
    public Vector3 curPos;
    [HideInInspector]
    public Vector3 lastPos;
    [HideInInspector]
    public Fusion.NetworkId ownerId;
    [SerializeField, Header("生命周期")]
    public float life;
    [SerializeField, Header("子弹本身加载路径")]
    public string bulletPath;
    [SerializeField, Header("击中特效加载路径")]
    public string effectPath;
    [SerializeField, Header("目标路径")]
    public LayerMask target;

    public void InitBullet(Vector3 dir, float speed, Fusion.NetworkId id)
    {
        curPos = transform.position;
        lastPos = transform.position;
        moveDir = dir;
        moveSpeed = speed;
        ownerId = id;
    }
    public virtual void HideBullet()
    {
        PoolManager.Instance.ReleaseObject(bulletPath, gameObject);
    }
    private void OnEnable()
    {
        Invoke("HideBullet", life);
    }
    private void OnDisable()
    {
        CancelInvoke();
    }
    public void FixedUpdate()
    {
        Fly(Time.fixedDeltaTime);
    }
    public virtual void Fly(float dt)
    {
    }
}
