using DG.Tweening;
using Fusion;
using UnityEngine;

public class Bullet9002 : BulletBase
{
    [SerializeField]
    private SpriteRenderer bullet_Up;
    [SerializeField]
    private SpriteRenderer bullet_Down;
    [SerializeField]
    private float speedOffset;
    [SerializeField]
    private int damageOffset;
    public override void InitBullet(Vector3 dir, float speed, ActorNetManager from)
    {
        transform.DOKill();
        transform.localScale = Vector3.one;

        bullet_Up.sortingOrder = 4;
        bullet_Up.gameObject.SetActive(true);
        bullet_Down.sortingOrder = 3;
        bullet_Down.gameObject.SetActive(true);
        base.InitBullet(dir, speed + speedOffset, from);

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
                    if (actor == _from.LocalManager) { continue; }
                    else
                    {
                        actor.TakeDamage(1 + damageOffset, _from);
                        StayIn(hit2D[i].transform);
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
    public void SpeedDown(float dt)
    {
        if (moveSpeed > 0)
        {
            moveSpeed -= dt;
        }
        if (moveSpeed < 0)
        {
            HideBullet();
        }
    }
    public void StayIn(Transform parent)
    {
        if (parent.TryGetComponent(out SpriteRenderer sprite))
        {
            bullet_Up.sortingOrder = sprite.sortingOrder + 1;
        }
        else
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                if (parent.GetChild(i).TryGetComponent(out SpriteRenderer spriteRenderer))
                {
                    bullet_Up.sortingOrder = spriteRenderer.sortingOrder + 1;
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
}
