using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class ItemLocalObj_ScatterGun : ItemLocalObj_Gun
{
    [SerializeField]
    private SpriteRenderer spriteRenderer_RightHand;
    [SerializeField]
    private SpriteRenderer spriteRenderer_LeftHand;
    [Header("ɢ������")]
    public int config_BulletCount;

    public override void HoldingStart(ActorManager owner, BodyController_Human body)
    {
        actorManager = owner;

        transform.SetParent(body.transform_ItemInRightHand);
        body.gameObjects_ItemInHand.Add(gameObject);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;

        temp_NextAimPoint = AimTime;

        spriteRenderer_RightHand.color = body.transform_RightHand.GetComponent<SpriteRenderer>().color;
        body.transform_RightHand.GetComponent<SpriteRenderer>().enabled = false;

        spriteRenderer_LeftHand.color = body.transform_LeftHand.GetComponent<SpriteRenderer>().color;
        body.transform_LeftHand.GetComponent<SpriteRenderer>().enabled = false;
        base.HoldingStart(owner, body);
    }

    public override void TryToShot(float offset)
    {
        if (actorManager.actorAuthority.isPlayer && actorManager.actorAuthority.isLocal)
        {
            skillIndicators.Shake_SkillIndicators(new Vector3(0.2f, 0.2f, 0), 0.1f);
        }
        TriggerReset();
        Recoil();
        if (CheckBullet(out short bulletID))
        {
            Shoot(bulletID, GetRandomDirList(offset));
        }
        else
        {
            Dull();
        }
    }
    private List<Vector3> GetRandomDirList(float offset)
    {
        List<Vector3> dirList = new List<Vector3>();
        for (int i = 0; i < config_BulletCount; i++)
        {
            float val;
            int index = i;
            if (config_BulletCount == 1)
            {
                val = 0.5f; 
            }
            else
            {
                val = 1f / (config_BulletCount - 1);
            }
            //������ƫת��
            float randomAngle = Mathf.Lerp(-offset * 0.5f, offset * 0.5f, index * val);
            // ���Ƕ�ת��ΪQuaternion
            Quaternion randomRotation = Quaternion.Euler(0f, 0f, randomAngle);
            // ����תӦ�õ�ԭʼ������
            dirList.Add(randomRotation * (inputData.mousePosition.normalized));
        }
        return dirList;
    }

    public void Shoot(short bulletID, List<Vector3> dirList)
    {
        for (int i = 0; i < dirList.Count; i++)
        {
            GameObject obj = PoolManager.Instance.GetObject("Bullet/Bullet_" + bulletID);
            if (obj.TryGetComponent(out BulletBase bulletBase))
            {
                bulletBase.InitBullet();
                bulletBase.SetPhysics(transform_Muzzle.position, dirList[i], 0, 5);
                bulletBase.SetDamage(0, 0);
                bulletBase.SetOwner(actorManager);
            }
        }
        MuzzleFire();
        KickBack();
    }
    public override void MuzzleFire()
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
        AudioManager.Instance.Play3DEffect(2002, transform_Muzzle.position);
    }
    public override void KickBack()
    {
        transform_Body.DOKill();
        transform_Body.localPosition = Vector3.zero;
        transform_Body.DOPunchPosition(vector3_KickBack, ShotCD).OnComplete(() =>
        {
            spriteRenderer_LeftHand.transform.DOLocalMoveX(-0.1f, 0.1f).SetLoops(2, LoopType.Yoyo);
            AudioManager.Instance.Play3DEffect(2003, transform_Muzzle.position);
        });
        transform_Body.localRotation = Quaternion.identity;
        transform_Body.DOPunchRotation(new Vector3(0, 0, float_KickOn), ShotCD);

    }
}
