using DG.Tweening;
using Fusion;
using UnityEngine;

public class Bullet9002 : BulletBase
{
    [SerializeField]
    private Transform shadow;
    [SerializeField]
    private float speedOffset;
    [SerializeField]
    private int damageOffset;
    public override void InitBullet(Vector3 dir, float speed, NetworkId id)
    {
        transform.DOKill();
        transform.localScale = Vector3.one;
        base.InitBullet(dir, speed + speedOffset, id);
    }
    public override void Fly(float dt)
    {
        SpeedDown(dt);
        transform.position += moveDir * moveSpeed * dt;
        transform.right = moveDir;
        shadow.position = transform.position - Vector3.up * 0.5f; 
        base.Fly(dt);
    }
    public override void Check(float dt)
    {
        lastPos = curPos;
        curPos = transform.position;
        RaycastHit2D hit2D = Physics2D.Linecast(lastPos, curPos, target);
        if (hit2D)
        {
            if (hit2D.collider.TryGetComponent(out NetworkObject netObj))
            {
                if (netObj.Id == ownerId)
                {
                    return;
                }
            }
            HideBullet();
            if (hit2D.transform.TryGetComponent(out ActorManager actor))
            {
                actor.TakeDamage(1 + damageOffset, ownerId);
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
    public override void HideBullet()
    {
        moveSpeed = 0;

        transform.DOKill();
        Vector3 point = UnityEngine.Random.insideUnitCircle.normalized * UnityEngine.Random.Range(0.2f, 0.5f);
        transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
        transform.DOLocalJump(transform.position + point, 1, 1, 0.5f);

        transform.DOLocalRotate(new Vector3(0, 0, 720), 1, RotateMode.FastBeyond360);
        transform.DOScale(Vector3.zero, 1).OnComplete(() =>
        {
            base.HideBullet();
        });
    }
}
