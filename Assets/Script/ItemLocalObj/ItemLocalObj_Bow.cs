using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.U2D;
using static UnityEngine.UI.GridLayoutGroup;
using UnityEngine.Windows;
using UniRx;
using System.Security.Policy;

public class ItemLocalObj_Bow : ItemLocalObj
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Transform transform_Root;
    [SerializeField, Header("��ͷ")]
    private SpriteRenderer spriteRenderer_Arrow;
    [SerializeField, Header("����")]
    public Transform transform_LeftHand;
    [SerializeField, Header("����")]
    public Transform transform_RightHand;
    [SerializeField]
    private SpriteAtlas spriteAtlas_Item;
    [SerializeField]
    private SkillIndicators skillIndicators;

    /// <summary>
    /// �´����ʱ���(��������������)
    /// </summary>
    private float temp_NextShotPoint = 0;
    /// <summary>
    /// �´���ȫ׼��ʱ���(��������������)
    /// </summary>
    private float temp_NextReadyPoint = 0;
    /// <summary>
    /// �´���ȫ��׼ʱ���(��������������)
    /// </summary>
    private float temp_NextAimPoint = 0;
    private bool temp_Aiming;
    protected bool temp_MaxPressRight;
    protected bool temp_MaxPressLeft;

    /// <summary>
    /// ���������˺�
    /// </summary>
    private int AttackDamage;
    /// <summary>
    /// ����ħ���˺�
    /// </summary>
    private int MagicDamage;
    /// <summary>
    /// ��С����Ƕ�
    /// </summary>
    private int AimRangeMin;
    /// <summary>
    /// �������Ƕ�
    /// </summary>
    private int AimRangeMax;
    /// <summary>
    /// ɢ��̶�
    /// </summary>
    private float ShotRecoil;
    /// <summary>
    /// ������
    /// </summary>
    private float ShotCD;
    /// <summary>
    /// ׼��ʱ��
    /// </summary>
    private float ReadyTime;
    /// <summary>
    /// ��׼ʱ��
    /// </summary>
    private float AimTime;
    /// <summary>
    /// ������
    /// </summary>
    private int BulletCapacity;
    private InputData inputData = new InputData();
    public override void HoldingStart(ActorManager owner, BodyController_Human body)
    {
        actorManager = owner;

        transform.SetParent(body.transform_ItemInRightHand);
        body.gameObjects_ItemInHand.Add(gameObject);
        transform.localRotation = Quaternion.identity;
        transform.localPosition = new Vector3(0.1f, 0, 0);
        transform.localScale = Vector3.one;

        transform_LeftHand.GetComponent<SpriteRenderer>().color = body.transform_LeftHand.GetComponent<SpriteRenderer>().color;
        body.transform_LeftHand.GetComponent<SpriteRenderer>().enabled = false;
        transform_RightHand.GetComponent<SpriteRenderer>().color = body.transform_RightHand.GetComponent<SpriteRenderer>().color;
        body.transform_RightHand.GetComponent<SpriteRenderer>().enabled = false;

        base.HoldingStart(owner, body);
    }
    public void UpdateBowData(int attackDamage,int magicDamage, int bulletCapacity, float shotSpeed, float readySpeed, float aimSpeed, int aimRangeMin, int aimRangeMax, float shotRecoil, ItemQuality itemQuality)
    {
        AttackDamage = attackDamage;
        MagicDamage = magicDamage;
        BulletCapacity = bulletCapacity;
        ShotCD = 60f / shotSpeed;
        ReadyTime = 1f / readySpeed;
        AimTime = 1f / aimSpeed;
        AimRangeMin = aimRangeMin;
        AimRangeMax = aimRangeMax;
        ShotRecoil = shotRecoil;
    }
    public void FaceTo(Vector3 dir)
    {
        if (dir.x >= 0)
        {
            transform_Root.right = dir;
        }
        if (dir.x < 0)
        {
            transform_Root.right = -dir;
        }

    }
    /// <summary>
    /// ���ϼ�ͷ
    /// </summary>
    public void PutArrowOn()
    {
        if (actorManager.actorAuthority.isPlayer)
        {
            if (itemData.Item_Content.Item_Count > 0)
            {
                Sprite sprite = spriteAtlas_Item.GetSprite("Item_" + itemData.Item_Content.Item_ID);
                spriteRenderer_Arrow.DOKill();
                spriteRenderer_Arrow.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0), 0.1f);
                spriteRenderer_Arrow.sprite = sprite;
            }
            else
            {
                spriteRenderer_Arrow.sprite = null;
            }
        }
        else
        {
            Sprite sprite = spriteAtlas_Item.GetSprite("Item_" + 9001);
            spriteRenderer_Arrow.DOKill();
            spriteRenderer_Arrow.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0), 0.1f);
            spriteRenderer_Arrow.sprite = sprite;
        }
    }
    /// <summary>
    /// ���¼�ͷ
    /// </summary>
    public void PutArrowDown()
    {
        spriteRenderer_Arrow.sprite = null;
    }
    public override bool PressLeftMouse(float time, ActorAuthority actorAuthority)
    {
        temp_MaxPressLeft = false;
        inputData.leftPressTimer = time;
        if (inputData.rightPressTimer > temp_NextReadyPoint)
        {
            if (inputData.leftPressTimer >= temp_NextShotPoint)
            {
                TryToShot(GetAttackRange());
                temp_NextShotPoint = inputData.leftPressTimer + ShotCD;
                temp_NextReadyPoint = inputData.rightPressTimer + ShotCD;
                if (temp_NextAimPoint > inputData.rightPressTimer)
                {
                    temp_NextAimPoint += (ShotCD + ShotCD * ShotRecoil);
                }
                else
                {
                    temp_NextAimPoint = inputData.rightPressTimer + (ShotCD + ShotCD * ShotRecoil);
                }
                temp_MaxPressLeft = true;
            }
        }
        return temp_MaxPressLeft;
    }
    public override bool PressRightMouse(float time, ActorAuthority actorAuthority)
    {
        Aim(true);
        inputData.rightPressTimer = time;
        temp_MaxPressRight = (GetAttackRange()==AimRangeMin);
        return temp_MaxPressRight;
    }
    public override void ReleaseLeftMouse()
    {
        temp_NextShotPoint = 0;
        base.ReleaseLeftMouse();
    }
    public override void ReleaseRightMouse()
    {
        Aim(false);
        inputData.rightPressTimer = 0;
        temp_NextShotPoint = 0;
        temp_NextAimPoint = ReadyTime + AimTime;
        temp_NextReadyPoint = ReadyTime;
        base.ReleaseRightMouse();
    }
    public override void UpdateMousePos(Vector3 mouse)
    {
        inputData.mousePosition = mouse;
        if (actorManager.actorAuthority.isLocal && actorManager.actorAuthority.isPlayer)
        {
            UpdateSkillSector();
        }
        FaceTo(mouse);
        base.UpdateMousePos(mouse);
    }
    /// <summary>
    /// ��������Χ
    /// </summary>
    /// <returns></returns>
    private float GetAttackRange()
    {
        return Mathf.Lerp(AimRangeMin, AimRangeMax, (temp_NextAimPoint - inputData.rightPressTimer) / AimTime);
    }
    private Vector3 GetRandomDir(float offset)
    {
        UnityEngine.Random.InitState(itemData.Item_Info);
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
    /// <param name="inputState"></param>
    private void TryToShot(float offset)
    {
        if (CheckBullet(out short bulletID))
        {
            Shot(bulletID, GetRandomDir(offset), actorManager);
            PutArrowOn();
            animator.SetTrigger("Shoot");
        }
    }
    /// <summary>
    /// ���
    /// </summary>
    private void Shot(short bulletID, Vector3 dir, ActorManager actor)
    {
        GameObject obj = PoolManager.Instance.GetObject("Bullet/Bullet_" + bulletID);
        if (obj.TryGetComponent(out BulletBase bulletBase))
        {
            bulletBase.InitBullet();
            bulletBase.SetPhysics(spriteRenderer_Arrow.transform.position, dir, 0, 0);
            bulletBase.SetDamage(AttackDamage, MagicDamage);
            bulletBase.SetOwner(actorManager);
        }
    }
    private void Aim(bool on)
    {
        if (temp_Aiming != on)
        {
            temp_Aiming = on;
            if (on)
            {
                temp_NextAimPoint = ReadyTime; 
                animator.SetBool("Ready", on);
                PutArrowOn();
            }
            else
            {
                animator.SetBool("Ready", false);
                PutArrowDown();
            }
        }

    }
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
                bulletID = itemData.Item_Content.Item_ID;
                UseBullet();
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
            UseBullet();
            return true;
        }
    }
    /// <summary>
    /// ����ӵ�
    /// </summary>
    public virtual void AddBullet()
    {
        ItemData bullet = new ItemData();
        bullet.Item_ID = 9001;
        bullet.Item_Count = (short)(BulletCapacity / 2 + 1);
        itemData.Item_Content = new ContentData(bullet);
    }
    /// <summary>
    /// �����ӵ�
    /// </summary>
    private void UseBullet()
    {
        ItemData _oldItem = itemData;
        ItemData _newItem = _oldItem;
        _newItem.Item_Content.Item_Count--;
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
    /// <summary>
    /// ���¼���ָʾ��
    /// </summary>
    private void UpdateSkillSector()
    {
        if (inputData.rightPressTimer > ReadyTime)
        {
            skillIndicators.Draw_SkillIndicators(inputData.mousePosition, 1.5f, GetAttackRange(), 1);
        }
        else
        {
            if (inputData.rightPressTimer == 0)
            {
                skillIndicators.Draw_SkillIndicators(inputData.mousePosition, 0, 0, 0);
            }
            else
            {
                skillIndicators.Draw_SkillIndicators(inputData.mousePosition, 1.5f, GetAttackRange(), 0.2f);
            }
        }
    }
}
