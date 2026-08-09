using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class ItemLocalObj_Jet_ByFuel : ItemLocalObj
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
    protected InputData inputData = new InputData();
    private const short const_FuelID = 1111;

    protected int MagicDamage;
    protected float BulletSpeed;
    protected float BulletDuration;
    protected float JetFuelPerRec;
    protected float JetInterval;

    /// <summary>
    /// 下次射击时间点(随着射击次数提高)
    /// </summary>
    protected float temp_NextShotPoint = 0;
    /// <summary>
    /// 下次燃料消耗点(随着消耗次数提高)
    /// </summary>
    protected float temp_NextExpendPoint = 0;
    /// <summary>
    /// 是否还有燃料
    /// </summary>
    protected bool temp_Fuel = false;
    protected bool temp_Jetting = false;

    public void Update()
    {
        if (inputData.leftPressTimer == 0)
        {
            UpdateJetState(false);
        }
        else
        {
            UpdateJetState(temp_Fuel);
        }
    }
    public void UpdateJetData(int magicDamage, float bulletSpeed,float bulletDuration, float jetFuelPer, float jetInterval, ItemQuality itemQuality)
    {
        MagicDamage = magicDamage;
        BulletSpeed = bulletSpeed;
        BulletDuration = bulletDuration;
        JetFuelPerRec = 1 / jetFuelPer;
        JetInterval = jetInterval;

        var main = particleSystem_Jet.main;
        main.startLifetime = BulletDuration;
        main.startSpeed = BulletSpeed * 10;
    }
    public override bool PressLeftMouse(float time, ActorAuthority actorAuthority)
    {
        if (inputData.leftPressTimer >= temp_NextExpendPoint) { temp_Fuel = ExpendFuel(); temp_NextExpendPoint += JetFuelPerRec; }
        inputData.leftPressTimer = time;
        if (inputData.leftPressTimer >= temp_NextShotPoint && temp_Fuel)
        {
            Shoot(inputData.mousePosition);
            temp_NextShotPoint = inputData.leftPressTimer + JetInterval;
        }

        return inputData.leftPressTimer >= 1;
    }
    public override void ReleaseLeftMouse()
    {
        inputData.leftPressTimer = 0;
        temp_NextShotPoint = 0;
        temp_NextExpendPoint = 0;
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
        base.UpdateMousePos(mouse);
    }

    public virtual void UpdateJetState(bool on)
    {
        if (temp_Jetting == on) return;
        temp_Jetting = on;
        if (temp_Jetting) particleSystem_Jet.Play();
        else particleSystem_Jet.Stop();

    }
    public override void HoldingStart(ActorManager owner, BodyController_Human body)
    {
        actorManager = owner;

        body.AddItemOnRightHand(gameObject, Vector3.zero, Quaternion.identity, Vector3.one);
        spriteRenderer_RightHand.color = body.ShowRightHand(false).color;
        spriteRenderer_LeftHand.color = body.ShowLeftHand(false).color;

        base.HoldingStart(owner, body);
    }

    public virtual void Shoot(Vector3 dir)
    {
        GameObject obj = PoolManager.Instance.GetObject("Bullet/Bullet_Jet_Fire");
        if (obj.TryGetComponent(out BulletBase bulletBase))
        {
            bulletBase.InitBullet();
            bulletBase.SetPhysics(transform_Muzzle.position, dir, BulletSpeed, 0);
            bulletBase.SetDamage(0, MagicDamage);
            bulletBase.SetLifeTime(BulletDuration);
            bulletBase.SetOwner(actorManager);
        }
    }

    public virtual bool ExpendFuel()
    {
        if (actorManager.actorAuthority.isPlayer)
        {
            ItemData itemData = actorManager.actorNetManager.Local_ItemConsumables;
            if (const_FuelID == itemData.I && itemData.C > 0)
            {
                ItemData _oldItemConsumables = itemData;
                ItemData _newItemConsumables = _oldItemConsumables;
                _newItemConsumables.C--;
                if (_newItemConsumables.C <= 0)
                {
                    MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemConsumables_Sub()
                    {
                        item = _oldItemConsumables,
                    });
                }
                else
                {
                    MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemConsumables_Change()
                    {
                        oldItem = _oldItemConsumables,
                        newItem = _newItemConsumables,
                    });
                }

                return true;
            }
            return false;
        }
        else
        {
            return true;
        }
    }
}
