using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;
public class BuildingObj_Bush : BuildingObj
{
    private enum State
    {
        State0, State1
    }
    public SpriteRenderer spriteRenderer;
    private State state_Now;
    private Material material;
    [Header("Sprite未成熟")]
    public Sprite[] sprites_State0;
    [Header("Sprite成熟")]
    public Sprite[] sprites_State1;

    [Header("未成熟持续时间")]
    public int int_TimeState0 = 1;
    [Header("未成熟生命值")]
    public int int_HpState0 = 1;
    [Header("成熟生命值")]
    public int int_HpState1 = 1;

    private int gameTime_Sign = -100;
    private int gameTime_Now;

    [Header("未成熟基本掉落物")]
    public List<BaseLootInfo> baseLootInfos_State0 = new List<BaseLootInfo>();
    [Header("成熟基本掉落物")]
    public List<BaseLootInfo> baseLootInfos_State1 = new List<BaseLootInfo>();
    [Header("成熟额外掉落物")]
    public List<ExtraLootInfo> extraLootInfos_State1 = new List<ExtraLootInfo>();

    public override void Start()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_All_UpdateHour>().Subscribe(_ =>
        {
            All_UpdateTime(_.hour + _.day * 10);
        }).AddTo(this);
        WorldManager.Instance.GetTime(out int day, out int hour, out _);
        All_UpdateTime(day * 10 + hour);
        material = new Material(spriteRenderer.sharedMaterial);
        spriteRenderer.material = material;
        base.Start();
    }
    public override void All_UpdateHP(int newHp)
    {
        if (newHp <= 0) { All_Broken(); }
        else
        {
            if (newHp <= local_Hp)
            {
                All_HpDown(newHp - local_Hp);
            }
            else
            {
                All_HpUp(newHp - local_Hp);
            }
            Local_SetHp(newHp);
        }
    }
    public override int Local_TakeDamage(int val, DamageState damageState, ActorNetManager from)
    {
        if (damageState == DamageState.AttackSlashingDamage)
        {
            return base.Local_TakeDamage(val, damageState, from);
        }
        else
        {
            Local_IneffectiveDamage(damageState, from);
            return 0;
        }
    }
    #region//生长
    /// <summary>
    /// 更新时间
    /// </summary>
    /// <param name="hour"></param>
    /// <param name="date"></param>
    public void All_UpdateTime(int time)
    {
        gameTime_Now = time;
        All_CompareTime();
    }
    /// <summary>
    /// 对比时间
    /// </summary>
    public void All_CompareTime()
    {
        if (state_Now == State.State0)
        {
            if (gameTime_Now - gameTime_Sign > int_TimeState0)
            {
                All_UpdateState(State.State1);
            }
        }
        else if (state_Now == State.State1)
        {
            if (gameTime_Now - gameTime_Sign <= int_TimeState0)
            {
                All_UpdateState(State.State0);
            }
        }
    }
    /// <summary>
    /// 更新树状态
    /// </summary>
    /// <param name="type"></param>
    private void All_UpdateState(State type)
    {
        if (state_Now != type)
        {
            transform.DOKill();
            transform.localScale = Vector3.one;
            transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0), 0.1f);
            state_Now = type;
        }
        switch (type)
        {
            case State.State0:
                Local_SetHp(int_HpState0);
                AudioManager.Instance.Play3DEffect(3000, transform.position);
                spriteRenderer.sprite = sprites_State0[new System.Random().Next(0, sprites_State0.Length)];
                break;
            case State.State1:
                Local_SetHp(int_HpState1);
                spriteRenderer.sprite = sprites_State1[new System.Random().Next(0, sprites_State1.Length)];
                break;
        }
    }
    #endregion
    #region//方法
    public override void All_OnHpDown(int offset)
    {
        if (offset < 0)
        {
            AudioManager.Instance.Play3DEffect(3000, transform.position);
            All_TreeShake();
            All_TreeFlash();
        }
        All_TreeSwing();
    }
    private void All_TreeShake()
    {
        transform.DOKill();
        transform.localScale = Vector3.one;
        transform.DOPunchScale(new Vector3(0.2f, -0.1f, 0), 0.2f).SetEase(Ease.InOutBack);
    }
    private Sequence sequence_Light;
    private void All_TreeFlash()
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
    private void All_TreeSwing()
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
    public override void All_UpdateInfo(string info)
    {
        gameTime_Sign = int.Parse(info);
        All_CompareTime();
        base.All_UpdateInfo(info);
    }
    public override void All_Broken()
    {
        if (state_Now == State.State0)
        {
            if (WorldManager.Instance.gameNetManager.Object.HasStateAuthority)
            {
                State_CreateLootItem(State_GetLootItem(baseLootInfos_State0, null));
            }
            base.All_Broken();
        }
        else if (state_Now == State.State1)
        {
            if (WorldManager.Instance.gameNetManager.Object.HasStateAuthority)
            {
                Local_ChangeInfo((gameTime_Now).ToString());
                State_CreateLootItem(State_GetLootItem(baseLootInfos_State1, extraLootInfos_State1));
            }
        }
    }

    #endregion

}
