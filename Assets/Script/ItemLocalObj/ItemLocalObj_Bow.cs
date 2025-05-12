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
    /// �������
    /// </summary>
    private int temp_ShotTime = 0;
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

    [SerializeField, Header("UI��׼����")]
    private float config_AimDistance;
    [SerializeField, Header("���Ƕ�")]
    private int config_AimMaxRange;
    [SerializeField, Header("��С�Ƕ�")]
    private int config_AimMinRange;
    [SerializeField, Header("׼��ʱ��")]
    private float config_ShotReadyTime;
    [SerializeField, Header("��ȫ��׼ʱ��")]
    private float config_ShotAimTime;
    [SerializeField, Header("������")]
    private float config_ShotCD;
    [SerializeField, Header("���ɢ��̶�")]
    private float config_ShotRecoilTime;
    [Header("NPC�ϵ�����")]
    public int config_NPCAddBulletCount;
    private InputData inputData = new InputData();
    public override void HoldingByHand(ActorManager owner, BodyController_Human body, ItemData data)
    {
        actorManager = owner;

        transform.SetParent(body.transform_ItemInRightHand);
        body.gameObjects_ItemInHand.Add(gameObject);
        transform.localRotation = Quaternion.identity;
        transform.localPosition = new Vector3(0.1f, 0, 0);
        transform.localScale = Vector3.one;

        transform_LeftHand.GetComponent<SpriteRenderer>().sprite = body.transform_LeftHand.GetComponent<SpriteRenderer>().sprite;
        body.transform_LeftHand.GetComponent<SpriteRenderer>().enabled = false;
        transform_RightHand.GetComponent<SpriteRenderer>().sprite = body.transform_RightHand.GetComponent<SpriteRenderer>().sprite;
        body.transform_RightHand.GetComponent<SpriteRenderer>().enabled = false;

        itemData = data;
        base.HoldingByHand(owner, body, data);
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
                temp_MaxPressLeft = true;
            }
        }
        return temp_MaxPressLeft;
    }
    public override bool PressRightMouse(float time, ActorAuthority actorAuthority)
    {
        Aim(true);
        inputData.rightPressTimer = time;
        temp_MaxPressRight = (GetAttackRange()==config_AimMinRange);
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
        temp_NextAimPoint = config_ShotReadyTime + config_ShotAimTime;
        temp_NextReadyPoint = config_ShotReadyTime;
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
        return Mathf.Lerp(config_AimMinRange, config_AimMaxRange, (temp_NextAimPoint - inputData.rightPressTimer) / config_ShotAimTime);
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
        obj.transform.position = spriteRenderer_Arrow.transform.position;
        obj.GetComponent<BulletBase>().Shot(dir, 0, 10, 0, actor.actorNetManager);
    }
    private void Aim(bool on)
    {
        if (temp_Aiming != on)
        {
            temp_Aiming = on;
            if (on)
            {
                temp_NextAimPoint = config_ShotReadyTime; 
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
        bullet.Item_Count = (short)config_NPCAddBulletCount;
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
        _newItem.Item_Info--;
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
        if (inputData.rightPressTimer > config_ShotReadyTime)
        {
            skillIndicators.Draw_SkillIndicators(inputData.mousePosition, config_AimDistance, GetAttackRange(), 1);
        }
        else
        {
            if (inputData.rightPressTimer == 0)
            {
                skillIndicators.Draw_SkillIndicators(inputData.mousePosition, 0, 0, 0);
            }
            else
            {
                skillIndicators.Draw_SkillIndicators(inputData.mousePosition, config_AimDistance, GetAttackRange(), 0.2f);
            }
        }
    }
}
