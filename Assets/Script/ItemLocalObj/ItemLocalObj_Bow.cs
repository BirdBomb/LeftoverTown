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
    private SpriteAtlas spriteAtlas;
    [SerializeField]
    private SI_Sector skillSector;
    /// <summary>
    /// �������
    /// </summary>
    private int temp_shotTime = 0;
    /// <summary>
    /// �´����ʱ���(��������������)
    /// </summary>
    private float temp_nextShotPoint = 0;
    /// <summary>
    /// �´���ȫ׼��ʱ���(��������������)
    /// </summary>
    private float temp_nextReadyPoint = 0;
    /// <summary>
    /// �´���ȫ��׼ʱ���(��������������)
    /// </summary>
    private float temp_nextAimPoint = 0;
    [SerializeField, Header("UI��׼����")]
    private float config_aimDistance;
    [SerializeField, Header("���Ƕ�")]
    private int config_aimMaxRange;
    [SerializeField, Header("��С�Ƕ�")]
    private int config_aimMinRange;
    [SerializeField, Header("׼��ʱ��")]
    private float config_shotReadyTime;
    [SerializeField, Header("��ȫ��׼ʱ��")]
    private float config_shotAimTime;
    [SerializeField, Header("������")]
    private float config_shotCD;
    [SerializeField, Header("���ɢ��̶�")]
    private float config_shotRecoilTime;
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
    public void AddArrow()
    {
        if (actorManager.actorAuthority.isPlayer)
        {
            if (itemData.Item_Content.Item_Count > 0)
            {
                Sprite sprite = spriteAtlas.GetSprite("Item_" + itemData.Item_Content.Item_ID);
                spriteRenderer_Arrow.sprite = sprite;
            }
            else
            {
                spriteRenderer_Arrow.sprite = null;
            }
        }
        else
        {
            Sprite sprite = spriteAtlas.GetSprite("Item_" + 9001);
            spriteRenderer_Arrow.sprite = sprite;
        }
    }
    public void SubArrow()
    {
        spriteRenderer_Arrow.sprite = null;
    }
    public override bool PressLeftMouse(float time, ActorAuthority actorAuthority)
    {
        inputData.leftPressTimer = time;
        if (temp_shotTime != 0) { return true; }
        if (inputData.rightPressTimer > temp_nextReadyPoint)
        {
            if (inputData.leftPressTimer >= temp_nextShotPoint)
            {
                TryToShot(GetAttackRange());
                temp_nextShotPoint = inputData.leftPressTimer + config_shotCD;
                temp_nextReadyPoint = inputData.rightPressTimer + config_shotCD;
                if (temp_nextAimPoint > inputData.rightPressTimer)
                {
                    temp_nextAimPoint += config_shotRecoilTime;
                }
                else
                {
                    temp_nextAimPoint = inputData.rightPressTimer + config_shotRecoilTime;
                }
                return true;
            }
        }
        return false;
    }
    public override bool PressRightMouse(float time, ActorAuthority actorAuthority)
    {
        if (inputData.rightPressTimer == 0)
        {
            animator.SetBool("Ready", true);
            temp_nextAimPoint = config_shotReadyTime;
            AddArrow();
        }
        inputData.rightPressTimer = time;
        return GetAttackRange() == config_aimMinRange;
    }
    public override void ReleaseLeftMouse()
    {
        temp_nextShotPoint = 0;
        base.ReleaseLeftMouse();
    }
    public override void ReleaseRightMouse()
    {
        if (inputData.rightPressTimer > 0)
        {
            temp_shotTime = 0;
            temp_nextShotPoint = 0;
            temp_nextAimPoint = config_shotReadyTime + config_shotAimTime;
            temp_nextReadyPoint = config_shotReadyTime;
            inputData.rightPressTimer = 0;
            animator.SetBool("Ready", false);
        }
        base.ReleaseRightMouse();
    }
    public override void UpdateMousePos(Vector3 mouse)
    {
        inputData.mousePosition = mouse;
        UpdateSkillSector();
        FaceTo(mouse);
        base.UpdateMousePos(mouse);
    }
    /// <summary>
    /// ��������Χ
    /// </summary>
    /// <returns></returns>
    private float GetAttackRange()
    {
        return Mathf.Lerp(config_aimMinRange, config_aimMaxRange, (temp_nextAimPoint - inputData.rightPressTimer) / config_shotAimTime);
    }
    private Vector3 GetRandomDir(float offset)
    {
        UnityEngine.Random.InitState(itemData.Item_Seed);
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
            temp_shotTime++;
            Shot(bulletID, GetRandomDir(offset), actorManager);
            SubArrow();

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
        obj.GetComponent<BulletBase>().InitBullet(dir, 10, 0, actor.actorNetManager);
    }
    /// <summary>
    /// ����ӵ�
    /// </summary>
    /// <returns>�������</returns>
    private bool CheckBullet(out short bulletID)
    {
        bulletID = 9001;
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
    /// �����ӵ�
    /// </summary>
    private void UseBullet()
    {
        ItemData _oldItem = itemData;
        ItemData _newItem = _oldItem;
        _newItem.Item_Content.Item_Count--;
        _newItem.Item_Seed--;
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
    /// <summary>
    /// ���¼���ָʾ��
    /// </summary>
    private void UpdateSkillSector()
    {
        if (temp_shotTime > 0) { skillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0); }
        else
        {
            if (inputData.rightPressTimer > config_shotReadyTime)
            {
                skillSector.Update_SIsector(inputData.mousePosition, config_aimDistance, GetAttackRange(), 1);
            }
            else
            {
                if (inputData.rightPressTimer == 0)
                {
                    skillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
                }
                else
                {
                    skillSector.Update_SIsector(inputData.mousePosition, config_aimDistance, GetAttackRange(), 0.2f);
                }
            }
        }
    }

}
