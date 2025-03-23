using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class ItemLocalObj_Rifle : ItemLocalObj
{
    [SerializeField]
    private SpriteRenderer spriteRenderer_RightHand;
    [SerializeField]
    private SpriteRenderer spriteRenderer_LeftHand;
    [SerializeField]
    private Transform transform_Root;
    [SerializeField]
    private Transform transform_Muzzle;
    [SerializeField]
    private Transform transform_Rifle;

    [SerializeField]
    private SI_Sector sector;
    private InputData inputData = new InputData();
    /// <summary>
    /// 射击次数
    /// </summary>
    private int temp_ShotTime = 0;
    /// <summary>
    /// 下次射击时间点(随着射击次数提高)
    /// </summary>
    private float temp_NextShotPoint = 0;
    /// <summary>
    /// 下次完全准备时间点(随着射击次数提高)
    /// </summary>
    private float temp_NextReadyPoint = 0;
    /// <summary>
    /// 下次完全瞄准时间点(随着射击次数提高)
    /// </summary>
    private float temp_NextAimPoint = 0;
    [SerializeField, Header("准星距离")]
    private float config_AimDistance;
    [SerializeField, Header("最大角度")]
    private int config_AimMaxRange;
    [SerializeField, Header("最小角度")]
    private int config_AimMinRange;
    [SerializeField, Header("预备时间")]
    private float config_ShotReadyTime;
    [SerializeField, Header("瞄准时间")]
    private float config_ShotAimTime;
    [SerializeField, Header("射击间隔")]
    private float config_ShotCD;
    [SerializeField, Header("射击散射程度")]
    private float config_ShotRecoilTime;
    [SerializeField, Header("NPC连续射击次数")]
    private int config_ShotMaxTime;
    [SerializeField, Header("枪口后座")]
    private Vector3 vector3_KickBack;
    [SerializeField, Header("枪口上抬")]
    private float float_KickOn;
    public override void HoldingByHand(ActorManager owner, BodyController_Human body, ItemData data)
    {
        itemData = data;
        actorManager = owner;

        transform.SetParent(body.transform_ItemInRightHand);
        body.gameObjects_ItemInHand.Add(gameObject);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;

        spriteRenderer_RightHand.sprite = body.transform_RightHand.GetComponent<SpriteRenderer>().sprite;
        body.transform_RightHand.GetComponent<SpriteRenderer>().enabled = false;

        spriteRenderer_LeftHand.sprite = body.transform_LeftHand.GetComponent<SpriteRenderer>().sprite;
        body.transform_LeftHand.GetComponent<SpriteRenderer>().enabled = false;
        base.HoldingByHand(owner, body, data);
    }

    public override bool PressLeftMouse(float time, ActorAuthority actorAuthority)
    {
        inputData.leftPressTimer = time;
        if (inputData.rightPressTimer > temp_NextReadyPoint)
        {
            if (inputData.leftPressTimer >= temp_NextShotPoint)
            {
                TryToShot(GetAttackRange());
                temp_NextShotPoint = inputData.leftPressTimer + config_ShotCD;
                temp_NextReadyPoint = inputData.rightPressTimer + config_ShotCD;
                if (temp_NextAimPoint > inputData.rightPressTimer)
                {
                    temp_NextAimPoint += config_ShotRecoilTime;
                }
                else
                {
                    temp_NextAimPoint = inputData.rightPressTimer + config_ShotRecoilTime;
                }
                return temp_ShotTime > config_ShotMaxTime;
            }
        }
        return false;
    }
    public override bool PressRightMouse(float time, ActorAuthority actorAuthority)
    {
        if (inputData.rightPressTimer == 0)
        {
            Aim(true);
        }
        inputData.rightPressTimer = time;
        if (actorAuthority.isPlayer && actorAuthority.isLocal)
        {
            UpdateSkillSector();
        }
        return true;
    }
    public override void ReleaseLeftMouse()
    {
        temp_NextShotPoint = 0;
        base.ReleaseLeftMouse();
    }
    public override void ReleaseRightMouse()
    {
        if (inputData.rightPressTimer > 0)
        {
            temp_ShotTime = 0;
            temp_NextShotPoint = 0;
            temp_NextAimPoint = config_ShotReadyTime + config_ShotAimTime;
            temp_NextReadyPoint = config_ShotReadyTime;
            inputData.rightPressTimer = 0;
            if (actorManager.actorAuthority.isPlayer && actorManager.actorAuthority.isLocal)
            {
                UpdateSkillSector();
            }
            Aim(false);
        }
        base.ReleaseRightMouse();
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

    /// <summary>
    /// 更新技能指示器
    /// </summary>
    private void UpdateSkillSector()
    {
        if (inputData.rightPressTimer > config_ShotReadyTime)
        {
            sector.Update_SIsector(inputData.mousePosition, config_AimDistance, GetAttackRange(), 1);
        }
        else
        {
            if (inputData.rightPressTimer == 0)
            {
                sector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
            }
            else
            {
                sector.Update_SIsector(inputData.mousePosition, config_AimDistance, GetAttackRange(), 0.2f);
            }
        }
    }
    /// <summary>
    /// 获得射击范围
    /// </summary>
    /// <returns></returns>
    private float GetAttackRange()
    {
        return Mathf.Lerp(config_AimMinRange, config_AimMaxRange, (temp_NextAimPoint - inputData.rightPressTimer) / config_ShotAimTime);
    }
    /// <summary>
    /// 获得随机方向
    /// </summary>
    /// <param name="offset"></param>
    /// <returns></returns>
    private Vector3 GetRandomDir(float offset, int bullet)
    {
        UnityEngine.Random.InitState(bullet + itemData.Item_Seed);
        //获得随机偏转角
        float randomAngle = Mathf.Lerp(-offset * 0.5f, offset * 0.5f, UnityEngine.Random.Range(0f, 1f));
        //Debug.Log(randomAngle);
        //Debug.Log(offset + "Left:" + inputData.leftPressTimer + "/Right:" + inputData.rightPressTimer + "/Face:" + inputData.mousePosition);
        // 将角度转换为Quaternion
        Quaternion randomRotation = Quaternion.Euler(0f, 0f, randomAngle);
        // 将旋转应用到原始向量上
        Vector3 offsetVector = randomRotation * (inputData.mousePosition.normalized);
        return offsetVector;
    }

    /// <summary>
    /// 尝试射击
    /// </summary>
    /// <param name="offset"></param>
    /// <param name="inputState"></param>
    private void TryToShot(float offset)
    {
        if (CheckBullet(out short bulletID))
        {
            temp_ShotTime++;
            Shoot(bulletID, GetRandomDir(offset, temp_ShotTime));
        }
        else
        {
            Dull();
        }
    }
    /// <summary>
    /// 检查子弹
    /// </summary>
    /// <returns>可以射击</returns>
    private bool CheckBullet(out short bulletID)
    {
        bulletID = 9010;
        if (actorManager.actorAuthority.isPlayer)
        {
            if (itemData.Item_Content.Item_ID != 0 && itemData.Item_Content.Item_Count > 0)
            {
                bulletID = itemData.Item_Content.Item_ID;
                UseBullet();
                return true;
            }
            else
            {
                return false;
            }
        }
        return true;
    }
    /// <summary>
    /// 消耗子弹
    /// </summary>
    private void UseBullet()
    {
        ItemData _oldItem = itemData;
        ItemData _newItem = _oldItem;
        _newItem.Item_Content.Item_Count--;
        UpdateDataByLocal(_newItem);
        if (actorManager.actorAuthority.isPlayer && actorManager.actorAuthority.isLocal)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemInBag()
            {
                oldItem = _oldItem,
                newItem = _newItem,
            });
        }
    }
    public void Shoot(short bulletID, Vector3 dir)
    {
        GameObject obj = PoolManager.Instance.GetObject("Bullet/Bullet_" + bulletID);
        obj.transform.position = transform_Muzzle.position;
        obj.GetComponent<BulletBase>().InitBullet(dir, 1, 0, actorManager.actorNetManager);

        GameObject muzzleFire101 = PoolManager.Instance.GetObject("Effect/Effect_MuzzleFire");
        muzzleFire101.transform.SetParent(transform_Muzzle);
        muzzleFire101.transform.localScale = Vector3.one;
        muzzleFire101.transform.localPosition = Vector3.zero;
        muzzleFire101.transform.localRotation = Quaternion.identity;

        GameObject muzzleSmoke = PoolManager.Instance.GetObject("Effect/Effect_MuzzleSmoke");
        muzzleSmoke.transform.localScale = Vector3.one;
        muzzleSmoke.transform.position = transform_Muzzle.position;
        muzzleSmoke.transform.rotation = Quaternion.identity;
        AudioManager.Instance.PlayEffect(2001, transform.position);

        transform_Rifle.DOKill();
        transform_Rifle.localPosition = Vector3.zero;
        transform_Rifle.DOPunchPosition(vector3_KickBack, config_ShotCD);
        transform_Rifle.localRotation = Quaternion.identity;
        transform_Rifle.DOPunchRotation(new Vector3(0, 0, float_KickOn), config_ShotCD);
    }
    public void Dull()
    {
        AudioManager.Instance.PlayEffect(1002, transform.position);
    }
    public void Aim(bool on)
    {
        if (on)
        {
            transform_Root.DOLocalMove(new Vector3(0.25f, 0.125f, 0), 0.2f);
        }
        else
        {
            transform_Root.DOLocalMove(Vector3.zero, 0.2f);
        }
    }
}
