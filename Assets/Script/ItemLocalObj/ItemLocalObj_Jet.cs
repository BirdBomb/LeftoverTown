using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
public class ItemLocalObj_Jet : ItemLocalObj
{
    [SerializeField]
    public Transform transform_Root;
    [SerializeField]
    public Transform transform_Body;
    [SerializeField]
    public Transform transform_Muzzle;
    [SerializeField]
    private SpriteRenderer spriteRenderer_RightHand;
    [SerializeField]
    private SpriteRenderer spriteRenderer_LeftHand;

    [Header("粒子发射器")]
    public ParticleSystem particleSystem_Jet;
    [Header("准星距离")]
    public float config_AimDistance;
    public SkillIndicators skillIndicators;
    protected InputData inputData = new InputData();
    protected int MagicDamage;
    protected float JetTime;
    protected float JetDistance;
    protected float JetInterval;
    protected float temp_RemainingTimer = 0;

    protected bool bool_Jet = false;

    /// <summary>
    /// 下次射击时间点(随着射击次数提高)
    /// </summary>
    protected float temp_NextShotPoint = 0;

    public void Update()
    {
        if (inputData.leftPressTimer == 0)
        {
            UpdateJetState(false);
            if (temp_RemainingTimer < JetTime) temp_RemainingTimer += Time.deltaTime * 10;
            if (temp_NextShotPoint > 0)
            {
                temp_NextShotPoint -= Time.deltaTime;
            }

        }
        else
        {
            UpdateJetState(temp_RemainingTimer > 0);
            temp_RemainingTimer -= Time.deltaTime;
        }
    }
    public void UpdateJetData(int magicDamage,float jetDistance,float jetTime,float jetInterval, ItemQuality itemQuality)
    {
        MagicDamage = magicDamage;
        JetDistance = jetDistance;
        JetInterval = jetInterval;
        JetTime = jetTime;
    }
    public override bool PressLeftMouse(float time, ActorAuthority actorAuthority)
    {
        inputData.leftPressTimer = time;
        if (inputData.leftPressTimer >= temp_NextShotPoint)
        {
            TryToShot();
        }

        return temp_RemainingTimer <=0;
    }
    public override void ReleaseLeftMouse()
    {
        inputData.leftPressTimer = 0;
        if (temp_NextShotPoint > JetInterval)
        {
            temp_NextShotPoint = JetInterval;
        }
        base.ReleaseLeftMouse();
    }
    public override void UpdateMousePos(Vector3 mouse)
    {
        inputData.mousePosition = mouse;
        if (mouse.x >= 0)
        {
            transform_Root.right = mouse;
        }
        if (mouse.x < 0)
        {
            transform_Root.right = -mouse;
        }

        if (actorManager.actorAuthority.isPlayer && actorManager.actorAuthority.isLocal)
        {
            UpdateSkillSector();
        }
        base.UpdateMousePos(mouse);
    }
    public virtual void UpdateSkillSector()
    {
        skillIndicators.Draw_SkillIndicators(Vector2.up, config_AimDistance, Mathf.Lerp(0, 60, temp_RemainingTimer / JetTime), 1);

    }
    #region//喷射器操作
    public override void HoldingStart(ActorManager owner, BodyController_Human body)
    {
        actorManager = owner;

        body.AddItemOnRightHand(gameObject, Vector3.zero, Quaternion.identity, Vector3.one);
        spriteRenderer_RightHand.color = body.ShowRightHand(false).color;
        spriteRenderer_LeftHand.color = body.ShowLeftHand(false).color;

        base.HoldingStart(owner, body);
    }

    public virtual void UpdateJetState(bool on)
    {
        if (bool_Jet == on) return;
        bool_Jet = on;
        if(bool_Jet) particleSystem_Jet.Play();
        else particleSystem_Jet.Stop();
    }
    public virtual void TryToShot()
    {
        if (actorManager.actorAuthority.isPlayer && actorManager.actorAuthority.isLocal)
        {
            skillIndicators.Shake_SkillIndicators(new Vector3(0.2f, 0.2f, 0), 0.1f);
        }
        TriggerReset();
        Shoot(inputData.mousePosition.normalized);
    }
    /// <summary>
    /// 复位
    /// </summary>
    public virtual void TriggerReset()
    {
        temp_NextShotPoint = inputData.leftPressTimer + JetInterval;
    }
    /// <summary>
    /// 射击
    /// </summary>
    /// <param name="bulletID"></param>
    /// <param name="dir"></param>
    public virtual void Shoot(Vector3 dir)
    {
        GameObject obj = PoolManager.Instance.GetObject("Bullet/Bullet_Sphere");
        if (obj.TryGetComponent(out BulletBase bulletBase))
        {
            bulletBase.InitBullet();
            bulletBase.SetPhysics(transform_Muzzle.position, dir, 0, 0);
            bulletBase.SetDamage(0, MagicDamage);
            bulletBase.SetOwner(actorManager);
        }
    }

    #endregion
}
