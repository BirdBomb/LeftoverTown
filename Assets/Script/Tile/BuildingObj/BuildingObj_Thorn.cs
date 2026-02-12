using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class BuildingObj_Thorn : BuildingObj
{
    public SpriteRenderer spriteRenderer;
    private Material material;
    [Header("æ£º¨…À∫¶")]
    public short short_Damage;
    [Header("æ£º¨Õº∆¨")]
    public Sprite[] sprites_State0;
    [Header("æ£º¨µÙ¬‰ŒÔ")]
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
    #region//∑Ω∑®
    public override void All_OnHpDown(int offset)
    {
        if (offset < 0)
        {
            AudioManager.Instance.Play3DEffect(3000, transform.position);
            All_Flash();
        }
        All_Shake();
    }
    private void All_Shake()
    {
        transform.DOKill();
        transform.localScale = Vector3.one;
        transform.DOPunchScale(new Vector3(0.2f, -0.1f, 0), 0.2f).SetEase(Ease.InOutBack);
    }
    private Sequence sequence;
    private void All_Flash()
    {
        float light = 1;
        if (sequence != null) sequence.Kill();
        sequence = DOTween.Sequence();
        sequence.Insert(0,
            DOTween.To(() => light, x => light = x, 0, 0.2f).SetEase(Ease.InOutSine));
        sequence.OnUpdate(() =>
        { material.SetFloat("_White", light); });
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
        if (actor.actorAuthority.isLocal)
        {
            GameObject effect = PoolManager.Instance.GetEffectObj("Effect/Effect_Impact");
            effect.GetComponent<Effect_Impact>().PlayPiercing(actor.transform.position - transform.position);
            effect.transform.position = actor.transform.position;

            actor.actorHpManager.TakeDamage(short_Damage, DamageState.AttackPiercingDamage, null);
        }
        base.All_ActorStandOn(actor);
    }
    #endregion
}
