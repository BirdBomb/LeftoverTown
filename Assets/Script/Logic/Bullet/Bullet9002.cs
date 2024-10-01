using DG.Tweening;
using Fusion;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ¹­¼ý
/// </summary>
public class Bullet9002 : BulletBase
{
    [SerializeField, Header("¹­¼ýÉÏ°ë²¿·Ö")]
    private SpriteRenderer bullet_Up;
    [SerializeField, Header("¹­¼ýÏÂ°ë²¿·Ö")]
    private SpriteRenderer bullet_Down;
    [SerializeField, Header("¹­¼ýËÙ¶ÈË¥¼õ(m/s2)")]
    private float speedDown;
    /// <summary>
    /// ºöÂÔ
    /// </summary>
    private List<ActorManager> ignoreList = new List<ActorManager>();
    public override void InitBullet(Vector3 dir, float speed, ActorNetManager from)
    {
        transform.DOKill();
        transform.localScale = Vector3.one;

        bullet_Up.sortingOrder = 0;
        bullet_Up.gameObject.SetActive(true);
        bullet_Down.sortingOrder = 0;
        bullet_Down.gameObject.SetActive(true);

        ignoreList.Clear();
        ignoreList.Add(from.LocalManager);

        base.InitBullet(dir, speed, from);

        transform.right = moveDir;
    }
    public override void Fly(float dt)
    {
        SpeedDown(dt);
        transform.position += moveDir * moveSpeed * dt;
        base.Fly(dt);
    }
    public override void Check(float dt)
    {
        lastPos = curPos;
        curPos = transform.position;

        RaycastHit2D[] hit2D = Physics2D.LinecastAll(lastPos, curPos, target);
        for (int i = 0; i < hit2D.Length; i++)
        {
            if (hit2D[i].collider.CompareTag("Actor"))
            {
                if (hit2D[i].transform.TryGetComponent(out ActorManager actor))
                {
                    if (ignoreList.Contains(actor)) { continue; }
                    else
                    {
                        ignoreList.Add(actor);
                        TryAttack(actor);
                        Boom();
                    }
                }
            }
            else
            {
                StayIn(hit2D[i].transform);
            }
        }
        base.Check(dt);
    }
    /// <summary>
    /// ³¢ÊÔ¹¥»÷
    /// </summary>
    private void TryAttack(ActorManager actor)
    {
        if (_input)
        {
            actor.AllClient_TakeDamage(configDamage, _from);
        }
    }
    /// <summary>
    /// ¼õËÙ
    /// </summary>
    /// <param name="dt"></param>
    public void SpeedDown(float dt)
    {
        if (moveSpeed > 0)
        {
            moveSpeed -= dt * speedDown;
        }
        if (moveSpeed < 0)
        {
            HideBullet();
        }
    }
    /// <summary>
    /// Í£Áô
    /// </summary>
    /// <param name="parent"></param>
    public void StayIn(Transform parent)
    {
        if (parent.TryGetComponent(out SpriteRenderer sprite))
        {
            bullet_Up.sortingOrder = sprite.sortingOrder;
        }
        else
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                if (parent.GetChild(i).TryGetComponent(out SpriteRenderer spriteRenderer))
                {
                    bullet_Up.sortingOrder = spriteRenderer.sortingOrder;
                    break;
                }
            }
        }
        transform.SetParent(parent);
        bullet_Down.gameObject.SetActive(false);
        _hide = true;
        moveSpeed = 0;
        CancelInvoke();
        Invoke("HideBullet", 20);
    }
    /// <summary>
    /// ±¬Õ¨
    /// </summary>
    public void Boom()
    {
        bullet_Down.gameObject.SetActive(false);
        _hide = true;
        moveSpeed = 0;
        CancelInvoke();
        Invoke("HideBullet", 1);
    }
}
