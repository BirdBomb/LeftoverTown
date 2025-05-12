using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet_Arrow : BulletBase
{
    [SerializeField, Header("¹­¼ýËÙ¶ÈË¥¼õ(m/s2)")]
    private float float_ArrowSpeedDown;
    [SerializeField, Header("¼ýÊ¸ÉËº¦")]
    public short config_BaseDamage;
    [SerializeField, Header("¼ýÊ¸ËÙ¶È")]
    public short config_BaseSpeed;
    [SerializeField, Header("¼ýÊ¸Á¦Á¿")]
    public float config_BaseForce;

    [SerializeField, Header("¹­¼ýID")]
    private short int_ArrowID;
    [SerializeField, Header("¹­¼ý»ØÊÕ¼¸ÂÊ%1000")]
    private int int_ArrowRecoveryProbability;
    private List<ActorManager> actorManagers_Ignore = new List<ActorManager>();
    public override void Shot(Vector3 dir, int speed_Demage, float speed_Offset, float force_Offset, ActorNetManager from)
    {
        vectoe3_MoveDir = dir;
        float_BulletDemage = config_BaseDamage + speed_Demage;
        float_BulletSpeed = config_BaseSpeed + speed_Offset;
        float_BulletForce = config_BaseForce + force_Offset;
        vectoe3_CurPos = transform.position;
        vectoe3_LastPos = transform.position;
        actorNetManager_Owner = from;
        actorAuthority_Owner = actorNetManager_Owner.actorManager_Local.actorAuthority;

        actorManagers_Ignore.Clear();
        actorManagers_Ignore.Add(from.actorManager_Local);
        transform.DOKill();
        transform.localScale = Vector3.one;
        base.Shot(dir, speed_Demage, speed_Offset, force_Offset, from);
        transform.right = vectoe3_MoveDir;
    }
    public override void Fly(float dt)
    {
        SpeedDown(dt);
        transform.position += vectoe3_MoveDir * float_BulletSpeed * dt;
        base.Fly(dt);
    }
    public override void Check(float dt)
    {
        vectoe3_LastPos = vectoe3_CurPos;
        vectoe3_CurPos = transform.position;
        RaycastHit2D[] hit2D = Physics2D.LinecastAll(vectoe3_LastPos, vectoe3_CurPos + vectoe3_MoveDir * float_BulletSpeed * dt, layerMask_Target);
        for (int i = 0; i < hit2D.Length; i++)
        {
            if (hit2D[i].collider.CompareTag("Actor"))
            {
                if (hit2D[i].collider.isTrigger && hit2D[i].transform.TryGetComponent(out ActorManager actor))
                {
                    if (!actorManagers_Ignore.Contains(actor))
                    {
                        actorManagers_Ignore.Add(actor);
                        GameObject effect = PoolManager.Instance.GetObject("Effect/Effect_Blood");
                        effect.GetComponent<EffectBase>().SetEffect(vectoe3_MoveDir);
                        effect.transform.position = actor.transform.position;
                        Attack(actor);
                    }
                }
            }
            else
            {
                hit2D[i].transform.DOKill();
                hit2D[i].transform.localScale = Vector3.one;
                hit2D[i].transform.DOPunchScale(new Vector3(0.1f, -0.1f, 0), 0.1f);
                Boom(hit2D[i].point);
            }
        }
    }
    /// <summary>
    /// ³¢ÊÔ¹¥»÷
    /// </summary>
    private void Attack(ActorManager actor)
    {
        if (actorAuthority_Owner.isLocal)
        {
            actor.AllClient_Listen_TakeAttackDamage(float_BulletDemage, actorNetManager_Owner);
        }
    }
    /// <summary>
    /// ¼õËÙ
    /// </summary>
    /// <param name="dt"></param>
    public void SpeedDown(float dt)
    {
        if (float_BulletSpeed > 0)
        {
            float_BulletSpeed -= dt * float_ArrowSpeedDown;
        }
        else
        {
            HideBullet();
        }
    }
    /// <summary>
    /// ±¬Õ¨
    /// </summary>
    public void Boom(Vector2 pos)
    {
        GameObject effect = PoolManager.Instance.GetObject("Effect/Effect_WoodBoom");
        effect.transform.localScale = new Vector3(1 - (2 * new System.Random().Next(0, 2)), 1, 1);
        effect.transform.position = pos;
        if (float_BulletSpeed > 0)
        {
            float_BulletSpeed = 0;
            Recycle(pos);
        }
    }
    public void Recycle(Vector2 pos)
    {
        if (actorAuthority_Owner.isState)
        {
            if (new System.Random().Next(0, 1001) <= int_ArrowRecoveryProbability)
            {
                Type type = Type.GetType("Item_" + int_ArrowID.ToString());
                ((ItemBase)Activator.CreateInstance(type)).StaticAction_InitData(int_ArrowID, out ItemData initData);
                initData.Item_Count = 1;
                MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnItem()
                {
                    itemData = initData,
                    pos = (Vector3)pos - vectoe3_MoveDir,
                });
            }
        }
    }
}
