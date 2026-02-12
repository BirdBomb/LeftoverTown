using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class BuildingObj_Plant_OneState : BuildingObj
{
    public SpriteRenderer spriteRenderer;
    private Material material;
    [Header("Sprite成熟")]
    public Sprite[] sprites_State0;
    [Header("基本掉落物")]
    public List<BaseLootInfo> baseLootInfos_State0 = new List<BaseLootInfo>();

    public override void Start()
    {
        material = new Material(spriteRenderer.sharedMaterial);
        spriteRenderer.sprite = sprites_State0[new System.Random().Next(0, sprites_State0.Length)];
        spriteRenderer.material = material;
        base.Start();
    }
    public override int Local_TakeDamage(int val, DamageState damageState, ActorNetManager from)
    {
        if (damageState == DamageState.AttackReapDamage)
        {
            return base.Local_TakeDamage(val, damageState, from);
        }
        else
        {
            Local_IneffectiveDamage(damageState, from);
            return 0;
        }

    }
    #region//方法
    public override void All_OnHpDown(int offset)
    {
        if (offset < 0)
        {
            AudioManager.Instance.Play3DEffect(3000, transform.position);
            All_PlantShake();
            All_PlantFlash();
        }
        All_PlantSwing();
    }
    private void All_PlantShake()
    {
        transform.DOKill();
        transform.localScale = Vector3.one;
        transform.DOPunchScale(new Vector3(0.2f, -0.1f, 0), 0.2f).SetEase(Ease.InOutBack);
    }
    private Sequence sequence_Light;
    private void All_PlantFlash()
    {
        float light = 1;
        if (sequence_Light != null) sequence_Light.Kill();
        sequence_Light = DOTween.Sequence();
        sequence_Light.Insert(0,
            DOTween.To(() => light, x => light = x, 0, 0.2f).SetEase(Ease.InOutSine));
        sequence_Light.OnUpdate(() =>
        { material.SetFloat("_White", light); });
    }
    private Sequence sequence_ExtraShake;
    private Sequence sequence_ExtraScale;
    private void All_PlantSwing()
    {
        float duration = 2f;

        /*额外的抖动*/
        float force_ExtraShake = 0f;
        if (sequence_ExtraShake != null) sequence_ExtraShake.Kill();
        sequence_ExtraShake = DOTween.Sequence();
        sequence_ExtraShake.Insert(0,
            DOTween.To(() => force_ExtraShake, x => force_ExtraShake = x, 3 * Mathf.PI, duration).SetEase(Ease.OutCubic));
        sequence_ExtraShake.OnUpdate(() =>
        { material.SetFloat("_ExtraShake", force_ExtraShake); });
        /*额外的幅度*/
        float force_ExtraScale = 0;
        if (sequence_ExtraScale != null) sequence_ExtraScale.Kill();
        sequence_ExtraScale = DOTween.Sequence();
        sequence_ExtraScale.Insert(0,
            DOTween.To(() => force_ExtraScale, x => force_ExtraScale = x, 0.1f, duration / 2f).SetEase(Ease.OutCubic).SetLoops(2, LoopType.Yoyo));
        sequence_ExtraScale.OnUpdate(() =>
        { material.SetFloat("_ExtraScale", force_ExtraScale); });
    }
    public override void All_OnBroken()
    {
        AudioManager.Instance.Play3DEffect(3000, transform.position);
        base.All_OnBroken();
    }
    public override void All_Broken()
    {
        State_CreateLootItem(State_GetLootItem(baseLootInfos_State0, new List<ExtraLootInfo>()));
        base.All_Broken();
    }
    public override void All_ActorStandOn(ActorManager actor)
    {
        All_PlantSwing();
        base.All_ActorStandOn(actor);
    }
    #endregion
}
