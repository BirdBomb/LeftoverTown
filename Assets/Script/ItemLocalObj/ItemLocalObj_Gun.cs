using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class ItemLocalObj_Gun : ItemLocalObj
{
    [SerializeField]
    public Transform transform_Root;
    [SerializeField]
    public Transform transform_Body;
    [SerializeField]
    public Transform transform_Muzzle;
    [Header("׼�Ǿ���")]
    public float config_AimDistance;
    [Header("���Ƕ�")]
    public int config_AimMaxRange;
    [Header("��С�Ƕ�")]
    public int config_AimMinRange;
    [Header("��׼ʱ��")]
    public float config_ShotAimTime;
    [Header("������")]
    public float config_ShotCD;
    [Header("���ɢ��̶�")]
    public float config_ShotRecoilTime;
    [Header("ǹ�ں���")]
    public Vector3 vector3_KickBack;
    [Header("ǹ����̧")]
    public float float_KickOn;
    [Header("NPC�ϵ�����")]
    public int config_NPCAddBulletCount;

    public SkillIndicators skillIndicators;
    protected InputData inputData = new InputData();
    /// <summary>
    /// �´����ʱ���(��������������)
    /// </summary>
    protected float temp_NextShotPoint = 0;
    /// <summary>
    /// �´���ȫ��׼ʱ���(��������������)
    /// </summary>
    protected float temp_NextAimPoint = 0;
    protected bool temp_MaxPressRight;
    protected bool temp_MaxPressLeft;
    protected bool temp_Aiming = false;
    public void Update()
    {
        if (inputData.leftPressTimer == 0 && temp_NextShotPoint > 0)
        {
            temp_NextShotPoint -= Time.deltaTime;
        }
    }
    public override bool PressLeftMouse(float time, ActorAuthority actorAuthority)
    {
        temp_MaxPressLeft = false;
        inputData.leftPressTimer = time;
        if (inputData.leftPressTimer >= temp_NextShotPoint)
        {
            TryToShot(GetAttackRange());
            if (actorAuthority.isPlayer)
            {
                temp_MaxPressLeft = false;
            }
            else
            {
                if (itemData.Item_Content.Item_Count < 1)
                {
                    AddBullet();
                    temp_MaxPressLeft = true;
                }
            }
        }
        return temp_MaxPressLeft;
    }
    public override bool PressRightMouse(float time, ActorAuthority actorAuthority)
    {
        temp_MaxPressRight = true;
        inputData.rightPressTimer = time;
        Aim(true);
        return temp_MaxPressRight;
    }
    public override void ReleaseLeftMouse()
    {
        if (temp_NextShotPoint > config_ShotCD)
        {
            temp_NextShotPoint = config_ShotCD;
        }
        inputData.leftPressTimer = 0;
        base.ReleaseLeftMouse();
    }
    public override void ReleaseRightMouse()
    {
        temp_NextAimPoint = config_ShotAimTime;
        inputData.rightPressTimer = 0;
        Aim(false);
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
        if (actorManager.actorAuthority.isPlayer && actorManager.actorAuthority.isLocal)
        {
            UpdateSkillSector();
        }
        base.UpdateMousePos(mouse);
    }
    /// <summary>
    /// ���¼���ָʾ��
    /// </summary>
    public virtual void UpdateSkillSector()
    {
        if (inputData.rightPressTimer > 0 || inputData.leftPressTimer > 0)
        {
            skillIndicators.Draw_SkillIndicators(inputData.mousePosition, config_AimDistance, GetAttackRange(), 1);
        }
        else
        {
            skillIndicators.Draw_SkillIndicators(inputData.mousePosition, 0, 0, 0);
        }
    }
    #region//�ӵ�����
    /// <summary>
    /// ����ӵ�
    /// </summary>
    /// <returns>�������</returns>
    public virtual bool CheckBullet(out short bulletID)
    {
        if (actorManager.actorAuthority.isPlayer)
        {
            if (itemData.Item_Content.Item_ID != 0 && itemData.Item_Content.Item_Count > 0)
            {
                if (itemData.Item_Content.Item_ID == 9020) 
                { 
                    bulletID = 9010;
                    UseBullet(0);
                }
                else 
                {
                    bulletID = itemData.Item_Content.Item_ID;
                    UseBullet(1); 
                }
                return true;
            }
            else
            {
                bulletID = 0;
                return false;
            }
        }
        else
        {
            if (itemData.Item_Content.Item_ID == 0 || itemData.Item_Content.Item_Count == 0)
            {
                AddBullet();
            }
            bulletID = itemData.Item_Content.Item_ID;
            UseBullet(1);
            return true;
        }
    }
    /// <summary>
    /// ����ӵ�
    /// </summary>
    public virtual void AddBullet()
    {
        ItemData bullet = new ItemData();
        bullet.Item_ID = 9010;
        bullet.Item_Count = (short)config_NPCAddBulletCount;
        itemData.Item_Content = new ContentData(bullet);
    }
    /// <summary>
    /// �����ӵ�
    /// </summary>
    public virtual void UseBullet(int count)
    {
        ItemData _oldItem = itemData;
        ItemData _newItem = _oldItem;
        _newItem.Item_Content.Item_Count = (short)(_newItem.Item_Content.Item_Count - count);
        _newItem.Item_Durability--;
        UpdateDataByLocal(_newItem);
        if (actorManager.actorAuthority.isPlayer && actorManager.actorAuthority.isLocal)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemOnHand()
            {
                oldItem = _oldItem,
                newItem = _newItem,
            });
        }
    }

    #endregion
    #region//ǹе����
    /// <summary>
    /// ��������Χ
    /// </summary>
    /// <returns></returns>
    public virtual float GetAttackRange()
    {
        return Mathf.Lerp(config_AimMinRange, config_AimMaxRange, (temp_NextAimPoint - inputData.rightPressTimer) / config_ShotAimTime);
    }

    /// <summary>
    /// ����������
    /// </summary>
    /// <param name="offset"></param>
    /// <returns></returns>
    public virtual Vector3 GetRandomDir(float offset)
    {
        UnityEngine.Random.InitState(itemData.Item_Durability + itemData.Item_Content.Item_Count);
        //������ƫת��
        float randomAngle = Mathf.Lerp(-offset * 0.5f, offset * 0.5f, UnityEngine.Random.Range(0f, 1f));
        // ���Ƕ�ת��ΪQuaternion
        Quaternion randomRotation = Quaternion.Euler(0f, 0f, randomAngle);
        // ����תӦ�õ�ԭʼ������
        Vector3 offsetVector = randomRotation * (inputData.mousePosition.normalized);
        return offsetVector;
    }

    /// <summary>
    /// �������
    /// </summary>
    /// <param name="offset"></param>
    public virtual void TryToShot(float offset)
    {
        if (actorManager.actorAuthority.isPlayer && actorManager.actorAuthority.isLocal)
        {
            skillIndicators.Shake_SkillIndicators(new Vector3(0.2f, 0.2f, 0), 0.1f);
        }
        TriggerReset();
        Recoil();
        if (CheckBullet(out short bulletID))
        {
            Shoot(bulletID, GetRandomDir(offset));
        }
        else
        {
            Dull();
        }
    }
    /// <summary>
    /// ���
    /// </summary>
    /// <param name="bulletID"></param>
    /// <param name="dir"></param>
    public virtual void Shoot(short bulletID, Vector3 dir)
    {
        GameObject obj = PoolManager.Instance.GetObject("Bullet/Bullet_" + bulletID);
        obj.transform.position = transform_Muzzle.position;
        obj.GetComponent<BulletBase>().Shot(dir, 0, 1, 0, actorManager.actorNetManager);

        MuzzleFire();
        KickBack();

    }
    /// <summary>
    /// �ջ�
    /// </summary>
    public virtual void Dull()
    {
        AudioManager.Instance.Play3DEffect(2000, transform_Muzzle.position);
    }
    /// <summary>
    /// ��׼
    /// </summary>
    /// <param name="on"></param>
    public virtual void Aim(bool on)
    {
        if (temp_Aiming != on)
        {
            temp_Aiming = on;
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
    /// <summary>
    /// ɢ��
    /// </summary>
    public virtual void Recoil()
    {
        if (temp_NextAimPoint > inputData.rightPressTimer)
        {
            temp_NextAimPoint += config_ShotRecoilTime;
        }
        else
        {
            temp_NextAimPoint = inputData.rightPressTimer + config_ShotRecoilTime;
        }
    }
    /// <summary>
    /// ǹ��
    /// </summary>
    public virtual void MuzzleFire()
    {
        GameObject muzzleFire101 = PoolManager.Instance.GetObject("Effect/Effect_MuzzleFire");
        muzzleFire101.transform.SetParent(transform_Muzzle);
        muzzleFire101.transform.localScale = new Vector3(1, 1 - (2 * new System.Random().Next(0, 2)), 1);
        muzzleFire101.transform.localPosition = new Vector3(new System.Random().Next(-1, 1) * 0.1f, new System.Random().Next(-1, 1) * 0.1f, 1);
        muzzleFire101.transform.localRotation = Quaternion.identity;

        GameObject muzzleSmoke = PoolManager.Instance.GetObject("Effect/Effect_MuzzleSmoke");
        muzzleSmoke.transform.localScale = Vector3.one;
        muzzleSmoke.transform.position = transform_Muzzle.position;
        muzzleSmoke.transform.rotation = Quaternion.identity;
        AudioManager.Instance.Play3DEffect(2001, transform_Muzzle.position);
    }
    /// <summary>
    /// ����
    /// </summary>
    public virtual void KickBack()
    {
        transform_Body.DOKill();
        transform_Body.localPosition = Vector3.zero;
        transform_Body.DOPunchPosition(vector3_KickBack, config_ShotCD);
        transform_Body.localRotation = Quaternion.identity;
        transform_Body.DOPunchRotation(new Vector3(0, 0, float_KickOn), config_ShotCD);
    }
    /// <summary>
    /// ��λ
    /// </summary>
    public virtual void TriggerReset()
    {
        temp_NextShotPoint = inputData.leftPressTimer + config_ShotCD;
    }


    #endregion
}
