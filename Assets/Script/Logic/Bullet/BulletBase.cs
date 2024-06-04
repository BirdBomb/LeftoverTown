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
    [SerializeField, Header("��������")]
    public float life;
    [SerializeField, Header("�ӵ��������·��")]
    public string bulletPath;
    [SerializeField, Header("������Ч����·��")]
    public string effectPath;
    [SerializeField, Header("Ŀ��·��")]
    public LayerMask target;
    protected Fusion.NetworkId _ownerId;
    protected ActorNetManager _from;
    protected bool _input;
    protected bool _state;
    protected bool _hide;
    public virtual void InitBullet(Vector3 dir, float speed, ActorNetManager from)
    {
        curPos = transform.position;
        lastPos = transform.position;
        moveDir = dir;
        moveSpeed = speed;

        _ownerId = from.Object.Id;
        _from = from;
        _input = from.Object.HasInputAuthority;
        _state = from.Object.HasStateAuthority;
        _hide = false;
    }
    public virtual void HideBullet()
    {
        _hide = true;
        PoolManager.Instance.ReleaseObject(bulletPath, gameObject);
    }
    private void OnEnable()
    {
        _hide = false;
        Invoke("HideBullet", life);
    }
    private void OnDisable()
    {
        _hide = true;
        CancelInvoke();
    }
    public void FixedUpdate()
    {
        if (!_hide)
        {
            Fly(Time.fixedDeltaTime);
            Check(Time.fixedDeltaTime);
        }
    }
    public virtual void Fly(float dt)
    {
    }
    public virtual void Check(float dt)
    {

    }
}
