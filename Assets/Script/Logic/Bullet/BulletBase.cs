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
    public int float_BulletDemage;
    [HideInInspector]
    public float float_BulletSpeed;
    [HideInInspector]
    public float float_BulletForce;
    [SerializeField, Header("��������")]
    public float float_Life;

    [SerializeField, Header("�ӵ��������·��")]
    public string str_BulletPath;
    [SerializeField, Header("Ŀ��㼶")]
    public LayerMask layerMask_Target;
    protected ActorNetManager actorNetManager_Owner;
    protected ActorAuthority actorAuthority_Owner;
    protected bool _hide;
    public virtual void Shot(Vector3 dir, int demage_Offset, float speed_Offset, float force_Offset, ActorNetManager from)
    {

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
