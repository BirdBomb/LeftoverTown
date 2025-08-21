using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    /// <summary>
    /// 移动方向
    /// </summary>
    [HideInInspector]
    public Vector3 vectoe3_MoveDir;
    /// <summary>
    /// 当前位置
    /// </summary>
    [HideInInspector]
    public Vector3 vectoe3_CurPos;
    /// <summary>
    /// 之前位置
    /// </summary>
    [HideInInspector]
    public Vector3 vectoe3_LastPos;
    /// <summary>
    /// 子弹物理伤害
    /// </summary>
    [HideInInspector]
    public int float_BulletAttackDemage;
    /// <summary>
    /// 子弹魔法伤害
    /// </summary>
    [HideInInspector]
    public int float_BulletMagicDemage;
    /// <summary>
    /// 子弹速度
    /// </summary>
    [HideInInspector]
    public float float_BulletSpeed;
    /// <summary>
    /// 子弹力量
    /// </summary>
    [HideInInspector]
    public float float_BulletForce;
    /// <summary>
    /// 子弹拥有者
    /// </summary>
    [HideInInspector]
    public ActorManager actorManager_Owner;
    /// <summary>
    /// 子弹权限
    /// </summary>
    [HideInInspector]
    public ActorAuthority actorAuthority_Owner;

    [Header("生命周期")]
    public float float_Life;
    [Header("子弹本身加载路径")]
    public string str_BulletPath;
    [Header("目标层级")]
    public LayerMask layerMask_Target;
    protected bool _hide;
    /// <summary>
    /// 初始化子弹
    /// </summary>
    public virtual void InitBullet()
    {

    }
    /// <summary>
    /// 初始化子弹物理属性
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="speedOffset"></param>
    /// <param name="forceOffset"></param>
    public virtual void SetPhysics(Vector3 pos, Vector2 dir, float speedOffset, float forceOffset)
    {

    }
    /// <summary>
    /// 初始化子弹伤害
    /// </summary>
    public virtual void SetDamage(int AdOffset,int MdOffset)
    {

    }
    /// <summary>
    /// 初始化子弹伤害来源
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
