using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class BuildingObj_RockBed : BuildingObj
{
    private enum RockState
    {
        Base, Open, Complete
    }
    private enum RockType
    {
        Rock,Coal,Niter,Copper,Iron,Gold
    }
    public SpriteRenderer spriteRenderer;
    public BoxCollider2D boxCollider;
    private Material material;
    [SerializeField]
    private RockState rockState_Now;
    [Header("残缺岩石贴图")]
    public Sprite[] sprites_RockBase;
    [Header("破碎岩石贴图")]
    public Sprite[] sprites_RockOpen;
    [Header("完整岩石贴图")]
    public Sprite[] sprites_RockComplete;
    [Header("岩石残缺持续时间(小时)")]
    public int int_RockBaseTime = 1;
    [Header("岩石破碎持续时间(小时)")]
    public int int_RockOpenTime = 1;
    [Header("岩石生命值")]
    public int int_RockHp = 1;
    private int gameTime_Sign = -9999;
    private int gameTime_Now;
    private int rockTypeMax;
    private int rockType;
    private int seed;
    [Header("完整岩石掉落物")]
    public List<BaseLootInfo> baseLootInfos_Complete = new List<BaseLootInfo>();
    [Header("不同类别岩石掉落物")]
    public List<BaseLootInfo> baseLootInfos_Open = new List<BaseLootInfo>();


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
        if (damageState == DamageState.AttackBludgeoningDamage)
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
        if (gameTime_Sign > gameTime_Now) { gameTime_Sign = gameTime_Now; }
        All_CompareTime();
    }
    /// <summary>
    /// 对比时间
    /// </summary>
    public void All_CompareTime()
    {
        if (rockState_Now == RockState.Base)
        {
            if (gameTime_Now - gameTime_Sign > int_RockBaseTime + int_RockOpenTime)
            {
                All_UpdateRockState(RockState.Complete);
            }
            else if (gameTime_Now - gameTime_Sign > int_RockBaseTime)
            {
                All_UpdateRockState(RockState.Open);
            }
        }
        else if (rockState_Now == RockState.Open)
        {
            if (gameTime_Now - gameTime_Sign <= int_RockBaseTime)
            {
                All_UpdateRockState(RockState.Base);
            }
            else if (gameTime_Now - gameTime_Sign > int_RockBaseTime + int_RockOpenTime)
            {
                All_UpdateRockState(RockState.Complete);
            }
        }
        else if (rockState_Now == RockState.Complete)
        {
            if (gameTime_Now - gameTime_Sign <= int_RockOpenTime)
            {
                All_UpdateRockState(RockState.Open);
            }
        }

    }
    /// <summary>
    /// 更新状态
    /// </summary>
    /// <param name="type"></param>
    private void All_UpdateRockState(RockState type)
    {
        if (rockState_Now != type)
        {
            transform.DOKill();
            transform.localScale = Vector3.one;
            transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0), 0.1f);
            rockState_Now = type;
        }
        switch (type)
        {
            case RockState.Base:
                Debug.Log("Base");
                Local_SetHp(int.MaxValue);
                spriteRenderer.sprite = sprites_RockBase[new System.Random().Next(0, sprites_RockBase.Length)];
                break;
            case RockState.Open:
                Debug.Log("Open");
                Local_SetHp(int_RockHp);
                spriteRenderer.sprite = sprites_RockOpen[rockType];
                break;
            case RockState.Complete:
                Debug.Log("Complete");
                Local_SetHp(int_RockHp);
                spriteRenderer.sprite = sprites_RockComplete[new System.Random().Next(0, sprites_RockComplete.Length)];
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
        AudioManager.Instance.Play3DEffect(3003, transform.position);
        base.All_OnBroken();
    }
    public override void All_UpdateInfo(string info)
    {
        gameTime_Sign = int.Parse(info);
        seed = HashCode.Combine(Mathf.RoundToInt(buildingTile.tileWorldPos.x * 1000),Mathf.RoundToInt(buildingTile.tileWorldPos.y * 1000),gameTime_Sign);
        if(rockTypeMax==0) rockTypeMax = Enum.GetValues(typeof(RockType)).Length;
        UnityEngine.Random.InitState(seed);
        rockType = UnityEngine.Random.Range(0, rockTypeMax);
        All_CompareTime();
        base.All_UpdateInfo(info);
    }
    public override void All_Broken()
    {
        if (rockState_Now == RockState.Complete)
        {
            if (WorldManager.Instance.gameNetManager.Object.HasStateAuthority)
            {
                Local_ChangeInfo((gameTime_Now).ToString());
                State_CreateLootItem(State_GetLootItem(baseLootInfos_Complete, null));
            }
        }
        else if (rockState_Now == RockState.Open)
        {
            if (WorldManager.Instance.gameNetManager.Object.HasStateAuthority)
            {
                Local_ChangeInfo((gameTime_Now).ToString());
                if (rockType < baseLootInfos_Open.Count)
                {
                    List<BaseLootInfo> temp = new List<BaseLootInfo>
                    {
                        baseLootInfos_Open[rockType]
                    };
                    State_CreateLootItem(State_GetLootItem(temp, null));
                }
            }
        }
    }

    #endregion

}
