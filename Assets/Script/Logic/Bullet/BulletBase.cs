using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    /// <summary>
    /// �ƶ�����
    /// </summary>
    [HideInInspector]
    public Vector3 vectoe3_MoveDir;
    /// <summary>
    /// ��ǰλ��
    /// </summary>
    [HideInInspector]
    public Vector3 vectoe3_CurPos;
    /// <summary>
    /// ֮ǰλ��
    /// </summary>
    [HideInInspector]
    public Vector3 vectoe3_LastPos;
    /// <summary>
    /// �ӵ������˺�
    /// </summary>
    [HideInInspector]
    public int float_BulletAttackDemage;
    /// <summary>
    /// �ӵ�ħ���˺�
    /// </summary>
    [HideInInspector]
    public int float_BulletMagicDemage;
    /// <summary>
    /// �ӵ��ٶ�
    /// </summary>
    [HideInInspector]
    public float float_BulletSpeed;
    /// <summary>
    /// �ӵ�����
    /// </summary>
    [HideInInspector]
    public float float_BulletForce;
    /// <summary>
    /// �ӵ�ӵ����
    /// </summary>
    [HideInInspector]
    public ActorManager actorManager_Owner;
    /// <summary>
    /// �ӵ�Ȩ��
    /// </summary>
    [HideInInspector]
    public ActorAuthority actorAuthority_Owner;

    [Header("��������")]
    public float float_Life;
    [Header("�ӵ��������·��")]
    public string str_BulletPath;
    [Header("Ŀ��㼶")]
    public LayerMask layerMask_Target;
    protected bool _hide;
    /// <summary>
    /// ��ʼ���ӵ�
    /// </summary>
    public virtual void InitBullet()
    {

    }
    /// <summary>
    /// ��ʼ���ӵ���������
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="speedOffset"></param>
    /// <param name="forceOffset"></param>
    public virtual void SetPhysics(Vector3 pos, Vector2 dir, float speedOffset, float forceOffset)
    {

    }
    /// <summary>
    /// ��ʼ���ӵ��˺�
    /// </summary>
    public virtual void SetDamage(int AdOffset,int MdOffset)
    {

    }
    /// <summary>
    /// ��ʼ���ӵ��˺���Դ
    /// </summary>
    public virtual void SetOwner(ActorManager owner)
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
        CancelInvoke();
        Invoke("HideBullet", float_Life);
    }
    private void OnDisable()
    {
        _hide = true;
        CancelInvoke();
    }
}
