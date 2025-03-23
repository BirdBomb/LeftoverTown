using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.SocialPlatforms;

public class Zombie_Hook_Bullet : MonoBehaviour
{
    [Header("钩子拥有者")]
    public ActorManager actorManager_Owner;
    [Header("钩子父级")]
    public Transform transform_Root;
    [Header("钩子绳索")]
    public LineRenderer lineRenderer_HookLine;
    [Header("钩子伤害")]
    public int float_Damage;
    [Header("目标层级")]
    public LayerMask layerMask_target;
    [Header("钩子绳索起点")]
    public Transform transform_RootLineStart;
    [Header("钩子绳索重点")]
    public Transform transform_RootLineEnd;
    [Header("钩子飞行形态")]
    public GameObject gameObject_Move;
    [Header("钩子停止形态")]
    public GameObject gameObject_Stop;
    private float float_Speed;
    private Vector3 vector3_Dir;
    private Vector3 vector3_LastPos;
    private Vector3 vector3_CurPos;
    private List<ActorManager> actorManagers_IgnoreList = new List<ActorManager>();

    public void FixedUpdate()
    {
        if (transform_RootLineStart)
        {
            lineRenderer_HookLine.SetPosition(0, transform_RootLineStart.position);
            lineRenderer_HookLine.SetPosition(1, transform_RootLineEnd.position);
        }
        Fly();
    }
    /// <summary>
    /// 飞行
    /// </summary>
    private void Fly()
    {
        if (float_Speed > 0)
        {
            Check(Time.fixedDeltaTime);
            SpeedDown(Time.fixedDeltaTime);
            transform.position += vector3_Dir * float_Speed * Time.fixedDeltaTime;
        }
    }
    private void Check(float dt)
    {
        vector3_LastPos = vector3_CurPos;
        vector3_CurPos = transform.position;
        RaycastHit2D[] hit2D = Physics2D.LinecastAll(vector3_LastPos, vector3_CurPos + vector3_Dir * float_Speed * dt, layerMask_target);
        for (int i = 0; i < hit2D.Length; i++)
        {
            if (hit2D[i].collider.CompareTag("Actor"))
            {
                if (hit2D[i].transform.TryGetComponent(out ActorManager actor))
                {
                    if (actorManagers_IgnoreList.Contains(actor)) { continue; }
                    else
                    {
                        actorManagers_IgnoreList.Add(actor);
                        GameObject effect = PoolManager.Instance.GetObject("Effect/Effect_Blood");
                        effect.GetComponent<EffectBase>().SetEffect(vector3_Dir);
                        effect.transform.position = actor.transform.position;
                        TryAttackByFly(actor);
                    }
                }
            }
            else
            {
                SpeedDown(20);
            }
        }
    }
    /// <summary>
    /// 尝试攻击
    /// </summary>
    private void TryAttackByFly(ActorManager actor)
    {
        if (actor.actorAuthority.isState)
        {
            actor.AllClient_Listen_TakeDamage(float_Damage, actorManager_Owner.actorNetManager);
            actor.actionManager.AddForce(vector3_Dir, 25);
        }
    }
    private void TryAttackByRetrieve(ActorManager actor,Vector3 dir)
    {
        if (actor.actorAuthority.isState)
        {
            actor.AllClient_Listen_TakeDamage(float_Damage * 2, actorManager_Owner.actorNetManager);
            actor.actionManager.AddForce(dir, 50);
        }
    }

    /// <summary>
    /// 减速
    /// </summary>
    /// <param name="dt"></param>
    public void SpeedDown(float dt)
    {
        if (float_Speed > 0)
        {
            float_Speed -= dt * 150;
            if(float_Speed < 0)
            {
                float_Speed = 0;
                HookPullIn();
            }
        }
    }
    /// <summary>
    /// 发射
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="speed"></param>
    /// <param name="parent"></param>
    public void Shot(Vector3 dir, float speed, Transform parent)
    {
        HookPullOut();
        vector3_LastPos = transform.position;
        vector3_CurPos = transform.position;
        actorManagers_IgnoreList.Clear();
        actorManagers_IgnoreList.Add(actorManager_Owner);
        transform.SetParent(parent);
        if(dir.x > 0)
        {
            transform.localScale = Vector3.one;
        }
        else
        {
            transform.localScale = new Vector3(1, -1, 1);
        }
        float_Speed = speed;
        vector3_Dir = dir;
        transform.right = dir;
    }
    /// <summary>
    /// 收回
    /// </summary>
    public void Retrieve()
    {
        HookPullOut();
        actorManagers_IgnoreList.Clear();
        actorManagers_IgnoreList.Add(actorManager_Owner);
        float_Speed = 0;
        transform.SetParent(transform_Root);
        transform.localScale = Vector3.one;
        transform.DOLocalMove(Vector3.zero, 0.2f);
        transform.DOLocalRotate(Vector3.zero, 0.2f);

        RaycastHit2D[] hit2D = Physics2D.LinecastAll(transform_Root.position, transform.position, layerMask_target);
        for (int i = 0; i < hit2D.Length; i++)
        {
            if (hit2D[i].collider.CompareTag("Actor"))
            {
                if (hit2D[i].transform.TryGetComponent(out ActorManager actor))
                {
                    if (actorManagers_IgnoreList.Contains(actor)) { continue; }
                    else
                    {
                        actorManagers_IgnoreList.Add(actor);
                        GameObject effect = PoolManager.Instance.GetObject("Effect/Effect_Blood");
                        effect.GetComponent<EffectBase>().SetEffect(vector3_Dir);
                        effect.transform.position = actor.transform.position;
                        TryAttackByRetrieve(actor, (transform_Root.position - transform.position).normalized);
                    }
                }
            }
        }
    }
    private void HookPullOut()
    {
        GameObject effect = PoolManager.Instance.GetObject("Effect/Effect_BombSmoke");
        effect.transform.position = transform.position;
        gameObject_Stop.SetActive(false);
        gameObject_Move.SetActive(true);
    }
    private void HookPullIn()
    {
        GameObject effect = PoolManager.Instance.GetObject("Effect/Effect_BombSmoke");
        effect.transform.position = transform.position;
        gameObject_Stop.SetActive(true);
        gameObject_Move.SetActive(false);
    }
}
