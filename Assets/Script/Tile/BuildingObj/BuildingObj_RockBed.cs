using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UniRx;
using UnityEngine;

public class BuildingObj_RockBed : BuildingObj
{
    private enum RockState
    {
        Default, Base, Open, Complete
    }
    private enum RockType
    {
        Rock, Coal, Niter, Copper, Iron, Gold
    }
    public override void Start()
    {
        InitializeMaterial();
        SubscribeToEvents();
        LoadInitialState();
        base.Start();
    }

    #region 序列化字段
    [Header("碰撞器")]
    public BoxCollider2D boxCollider;
    [Header("Sprite渲染器")]
    public SpriteRenderer spriteRenderer;
    [Header("状态精灵")]
    public Sprite[] sprites_RockBase;
    public Sprite[] sprites_RockOpen;
    public Sprite[] sprites_RockComplete;

    [Header("岩石残缺持续时间(小时)")]
    public int duration_RockBase = 1;
    [Header("岩石破碎持续时间(小时)")]
    public int duration_RockOpen = 1;
    [Header("状态生命值")]
    public int int_RockHp = 1;
    #endregion
    #region 私有字段
    [SerializeField]
    private RockState rockState_Now;
    public BuildingData_RockBed buildingData_RockBed = new BuildingData_RockBed();
    private Sequence sequence;
    private Material material;
    #endregion

    #region 初始化
    private void InitializeMaterial()
    {
        if (spriteRenderer != null)
        {
            material = new Material(spriteRenderer.sharedMaterial);
            spriteRenderer.material = material;
        }
    }
    public virtual void SubscribeToEvents()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_All_UpdateHour>()
            .Subscribe(All_OnHourUpdated)
            .AddTo(this);
    }

    public virtual void LoadInitialState()
    {
        All_UpdateState(); // 根据时间初始化状态
    }

    #endregion
    #region 信息上传与同步
    public override void All_OnRawDataUpdate()
    {
        buildingData_RockBed.Deserialize(local_ByteData);
        All_UpdateState();
        base.All_OnRawDataUpdate();
    }
    public void TryToPush()
    {
        All_PushData(buildingData_RockBed.Serialize());
    }
    #endregion
    #region 基类方法
    public override int Local_TakeDamage(int val, DamageState damageState, ActorNetManager from)
    {
        if (rockState_Now == RockState.Base)
        {
            if (damageState == DamageState.AttackStructureDamage) { return base.Local_TakeDamage(val, damageState, from); }
            Local_IneffectiveDamage(damageState, from);
            return 0;
        }
        else
        {
            if (damageState == DamageState.AttackBludgeoningDamage) return base.Local_TakeDamage(val, damageState, from);
            Local_IneffectiveDamage(damageState, from);
            return 0;
        }
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
    public override void All_Broken()
    {
        PlayBrokenEffect();
        if (WorldManager.Instance?.gameNetManager?.Object?.HasStateAuthority == true)
        {
            LootRandomInfo[] lootRandomInfos = new LootRandomInfo[] { };
            LootFixedInfo[] lootFixedInfos = new LootFixedInfo[] { };
            WorldManager.Instance.GetTime_NowHour(out int signTime);
            switch (rockState_Now)
            {
                case RockState.Complete:
                    lootRandomInfos = LootItemConfigData.GetLootRandomConfig(buildingTile.tileID * 10).Loot_List;
                    lootFixedInfos = LootItemConfigData.GetLootFixedConfig(buildingTile.tileID * 10).Loot_List;
                    signTime -= duration_RockOpen;
                    break;
                case RockState.Open:
                    lootRandomInfos = LootItemConfigData.GetLootRandomConfig(buildingTile.tileID * 10 + 1).Loot_List;
                    lootFixedInfos = LootItemConfigData.GetLootFixedConfig(buildingTile.tileID * 10 + 1).Loot_List;
                    break;
            }
            State_CreateLootItem(Tool_GetFixedItemList(lootFixedInfos));
            State_CreateLootItem(Tool_GetRandomItemList(lootRandomInfos, 1));
            buildingData_RockBed.WriteSignTime(signTime);
            TryToPush();
        }
    }
    public override void All_OnHpDown(int offset)
    {
        if (offset < 0)
        {
            PlayHurtEffect();
        }
    }
    #endregion
    #region 生长计算
    public void All_OnHourUpdated(GameEvent.GameEvent_All_UpdateHour eventData)
    {
        All_UpdateState();
    }
    public void All_UpdateState()
    {
        WorldManager.Instance.GetTime_NowHour(out int now);
        int sign =  buildingData_RockBed.ReadSignTime();
        int elapsedTime = now - sign;
        RockState targetState = CalculateTargetState(elapsedTime);
        if (targetState != rockState_Now)
        {
            TransitionToState(targetState);
        }
    }
    /// <summary>
    /// 计算目标状态
    /// </summary>
    private RockState CalculateTargetState(int elapsedTime)
    {
        if (elapsedTime < duration_RockBase)
            return RockState.Base;
        else if (elapsedTime < duration_RockBase + duration_RockOpen)
            return RockState.Open;
        else
            return RockState.Complete;
    }
    /// <summary>
    /// 状态转换
    /// </summary>
    private void TransitionToState(RockState newState)
    {
        rockState_Now = newState;
        All_ApplyStateConfig();
    }
    #endregion
    #region 状态切换
    private void All_ApplyStateConfig()
    {
        switch (rockState_Now)
        {
            case RockState.Base:
                All_ConfigureBaseState();
                break;
            case RockState.Open:
                All_ConfigureOpenState();
                break;
            case RockState.Complete:
                All_ConfigureCompleteState();
                break;
        }
    }
    public virtual void All_ConfigureBaseState()
    {
        Local_SetHp(int.MaxValue);
        PlayStateChangeAnimation();
        spriteRenderer.sprite = sprites_RockBase[new System.Random().Next(0, sprites_RockBase.Length)];
    }
    public virtual void All_ConfigureOpenState()
    {
        Local_SetHp(int_RockHp);
        PlayStateChangeAnimation();
        spriteRenderer.sprite = sprites_RockOpen[new System.Random().Next(0, sprites_RockOpen.Length)];
    }
    public virtual void All_ConfigureCompleteState()
    {
        Local_SetHp(int_RockHp);
        PlayStateChangeAnimation();
        spriteRenderer.sprite = sprites_RockComplete[new System.Random().Next(0, sprites_RockComplete.Length)];
    }

    #endregion
    #region 特效
    private void PlayStateChangeAnimation()
    {
        transform.DOKill();
        transform.localScale = Vector3.one;
        transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0), 0.1f);
    }
    private void PlayHurtEffect()
    {
        transform.DOKill();
        transform.localScale = Vector3.one;
        transform.DOPunchScale(new Vector3(0.2f, -0.1f, 0), 0.2f).SetEase(Ease.InOutBack);

        AudioManager.Instance.Play3DEffect(3002, transform.position);
        float light = 1;
        if (sequence != null) sequence.Kill();
        sequence = DOTween.Sequence();
        sequence.Insert(0,
            DOTween.To(() => light, x => light = x, 0, 0.2f).SetEase(Ease.InOutSine));
        sequence.OnUpdate(() =>
        { material.SetFloat("_White", light); });
    }
    private void PlayBrokenEffect()
    {
        AudioManager.Instance.Play3DEffect(3003, transform.position);
    }
    #endregion
}
public class BuildingData_RockBed
{
    public int int_SignTime = - 9999;
    public int ReadSignTime() { return int_SignTime; }
    public void WriteSignTime(int val) { int_SignTime = val; }
    public byte[] Serialize()
    {
        using (var ms = new MemoryStream())
        using (var writer = new BinaryWriter(ms))
        {
            writer.Write(int_SignTime);
            return ms.ToArray();
        }
    }
    public void Deserialize(byte[] data)
    {
        if (data == null || data.Length == 0)
        {
            return;
        }
        using (var ms = new MemoryStream(data))
        using (var reader = new BinaryReader(ms))
        {
            int_SignTime = reader.ReadInt32();
        }
    }

}