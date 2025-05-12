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
    [Header("散弹数量")]
    public int config_BulletCount;

    public override void HoldingByHand(ActorManager owner, BodyController_Human body, ItemData data)
    {
        itemData = data;
        actorManager = owner;

        transform.SetParent(body.transform_ItemInRightHand);
        body.gameObjects_ItemInHand.Add(gameObject);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;

        temp_NextAimPoint = config_ShotAimTime;

        spriteRenderer_RightHand.sprite = body.transform_RightHand.GetComponent<SpriteRenderer>().sprite;
        body.transform_RightHand.GetComponent<SpriteRenderer>().enabled = false;

        spriteRenderer_LeftHand.sprite = body.transform_LeftHand.GetComponent<SpriteRenderer>().sprite;
        body.transform_LeftHand.GetComponent<SpriteRenderer>().enabled = false;
        base.HoldingByHand(owner, body, data);
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
            //获得随机偏转角
            float randomAngle = Mathf.Lerp(-offset * 0.5f, offset * 0.5f, index * val);
            // 将角度转换为Quaternion
            Quaternion randomRotation = Quaternion.Euler(0f, 0f, randomAngle);
            // 将旋转应用到原始向量上
            dirList.Add(randomRotation * (inputData.mousePosition.normalized));
        }
        return dirList;
    }

    public void Shoot(short bulletID, List<Vector3> dirList)
    {
        for (int i = 0; i < dirList.Count; i++)
        {
            GameObject obj = PoolManager.Instance.GetObject("Bullet/Bullet_" + bulletID);
            BulletBase bulletBase = obj.GetComponent<BulletBase>();
            obj.transform.position = transform_Muzzle.position;
            bulletBase.Shot(dirList[i], 0, 0, 5, actorManager.actorNetManager);
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
        AudioManager.Instance.PlayEffect(2000, transform.position);
    }
    public override void KickBack()
    {
        transform_Body.DOKill();
        transform_Body.localPosition = Vector3.zero;
        transform_Body.DOPunchPosition(vector3_KickBack, config_ShotCD).OnComplete(() =>
        {
            spriteRenderer_LeftHand.transform.DOLocalMoveX(-0.1f, 0.1f).SetLoops(2, LoopType.Yoyo);
            AudioManager.Instance.PlayEffect(1003, transform.position);
        });
        transform_Body.localRotation = Quaternion.identity;
        transform_Body.DOPunchRotation(new Vector3(0, 0, float_KickOn), config_ShotCD);

    }
}
