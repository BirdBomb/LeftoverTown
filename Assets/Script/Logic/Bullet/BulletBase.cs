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
    [SerializeField, Header("��������")]
    public float life;
    [SerializeField, Header("�ӵ��������·��")]
    public string bulletPath;
    [SerializeField, Header("������Ч����·��")]
    public string effectPath;
    [SerializeField, Header("Ŀ��·��")]
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
