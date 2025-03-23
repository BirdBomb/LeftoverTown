using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    [HideInInspector]
    public Vector3 vectoe3_MoveDir;
    [HideInInspector]
    public Vector3 vectoe3_CurPos;
    [HideInInspector]
    public Vector3 vectoe3_LastPos;
    [HideInInspector]
    public float float_MoveSpeed;
    [HideInInspector]
    public float float_Force;
    [SerializeField, Header("��������")]
    public float float_Life;
    [SerializeField, Header("�ӵ��˺�")]
    public short config_Damage;
    [SerializeField, Header("�ӵ��ٶ�")]
    public short config_Speed;
    [SerializeField, Header("�ӵ�����")]
    public float config_Force;

    [SerializeField, Header("�ӵ��������·��")]
    public string str_BulletPath;
    [SerializeField, Header("Ŀ��㼶")]
    public LayerMask layerMask_Target;
    protected ActorNetManager actorNetManager_Owner;
    protected ActorAuthority actorAuthority_Owner;
    protected bool _hide;
    public virtual void InitBullet(Vector3 dir, float speed, float force, ActorNetManager from)
    {
        vectoe3_CurPos = transform.position;
        vectoe3_LastPos = transform.position;
        vectoe3_MoveDir = dir;
        float_MoveSpeed = config_Speed + speed;
        float_Force = config_Force + force;
        actorNetManager_Owner = from;
        actorAuthority_Owner = actorNetManager_Owner.actorManager_Local.actorAuthority;
        _hide = false;
    }
    public virtual void HideBullet()
    {
        _hide = true;
        PoolManager.Instance.ReleaseObject(str_BulletPath, gameObject);
    }
    private void OnEnable()
    {
        _hide = false;
        Invoke("HideBullet", float_Life);
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
    /// <summary>
    /// ����
    /// </summary>
    /// <param name="dt"></param>
    public virtual void Fly(float dt)
    {
    }
    /// <summary>
    /// ���
    /// </summary>
    /// <param name="dt"></param>
    public virtual void Check(float dt)
    {

    }
}
