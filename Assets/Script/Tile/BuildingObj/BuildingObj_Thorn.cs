using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class BuildingObj_Thorn : BuildingObj
{
    public SpriteRenderer spriteRenderer;
    private Material material;
    private Sequence sequence;
    [Header("�����˺�")]
    public short short_Damage;
    [Header("����ͼƬ")]
    public Sprite[] sprites_State0;
    [Header("����������")]
    public List<BaseLootInfo> baseLootInfos_State0 = new List<BaseLootInfo>();
    public override void Start()
    {
        material = new Material(spriteRenderer.sharedMaterial);
        spriteRenderer.sprite = sprites_State0[new System.Random().Next(0, sprites_State0.Length)];
        spriteRenderer.material = material;
        base.Start();
    }
    #region//����
    public override void All_PlayHpDown()
    {
        All_PlantShake();
        All_PlantFlash();
    }
    private void All_PlantShake()
    {
        AudioManager.Instance.Play3DEffect(3000, transform.position);
        transform.DOPunchScale(new Vector3(0.2f, -0.1f, 0), 0.2f).SetEase(Ease.InOutBack);
    }
    private void All_PlantFlash()
    {
        float light = 1;
        if (sequence != null) sequence.Kill();
        sequence = DOTween.Sequence();
        sequence.Insert(0,
            DOTween.To(() => light, x => light = x, 0, 0.2f).SetEase(Ease.InOutSine));
        sequence.OnUpdate(() =>
        { material.SetFloat("_White", light); });
    }
    public override void All_PlayBroken()
    {
        AudioManager.Instance.Play3DEffect(3000, transform.position);
        base.All_PlayBroken();
    }
    public override void All_UpdateHP(int newHp)
    {
        if (newHp <= 0) { All_Broken(); }
        else
        {
            if (newHp < hp)
            {
                All_HpDown(hp - newHp);
            }
            else
            {
                All_HpUp(newHp - hp);
            }
            hp = newHp;
        }
    }
    public override void All_Broken()
    {
        State_CreateLootItem(State_GetLootItem(baseLootInfos_State0, new List<ExtraLootInfo>()));
        base.All_Broken();
    }
    public override void All_ActorStandOn(ActorManager actor)
    {
        if (actor.actorAuthority.isLocal)
        {
            actor.AllClient_Listen_TakeAttackDamage(short_Damage, null);
        }
        base.All_ActorStandOn(actor);
    }
    #endregion
}
