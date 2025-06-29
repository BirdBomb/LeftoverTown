using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class BuildingObj_RockBed : BuildingObj
{
    private enum RockState
    {
        Base, Rock
    }
    public SpriteRenderer spriteRenderer;
    public BoxCollider2D boxCollider;
    [SerializeField]
    private RockState rockState_Now;
    private Material material;
    private Sequence sequence;
    [Header("岩石贴图")]
    public Sprite[] sprites_Rock;
    [Header("岩石恢复周期(小时)")]
    public int int_RockRsetTime = 1;
    [Header("岩石生命值")]
    public int int_RockHp = 1;
    private int gameTime_Sign = -9999;
    private int gameTime_Now;

    [Header("岩石基本掉落物")]
    public List<BaseLootInfo> baseLootInfos_State = new List<BaseLootInfo>();
    [Header("岩石额外掉落物")]
    public List<ExtraLootInfo> extraLootInfos_State = new List<ExtraLootInfo>();

    public override void Start()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_All_UpdateHour>().Subscribe(_ =>
        {
            All_UpdateTime(_.hour + _.day * 10);
        }).AddTo(this);
        All_UpdateTime(MapManager.Instance.mapNetManager.Day * 10 + MapManager.Instance.mapNetManager.Hour);
        material = new Material(spriteRenderer.sharedMaterial);
        spriteRenderer.material = material;
        base.Start();
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
            if (gameTime_Now - gameTime_Sign > int_RockRsetTime)
            {
                All_UpdateRockState(RockState.Rock);
            }
        }
        else if (rockState_Now == RockState.Rock)
        {
            if (gameTime_Now - gameTime_Sign <= int_RockRsetTime)
            {
                All_UpdateRockState(RockState.Base);
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
                hp = int.MaxValue;
                spriteRenderer.enabled = false;
                boxCollider.enabled = false;
                break;
            case RockState.Rock:
                hp = int_RockHp;
                spriteRenderer.enabled = true;
                boxCollider.enabled = true;
                spriteRenderer.sprite = sprites_Rock[new System.Random().Next(0, sprites_Rock.Length)];
                break;
        }
    }
    #endregion
    #region//方法
    public override void All_PlayHpDown()
    {
        All_PlantShake();
        All_PlantFlash();
    }
    private void All_PlantShake()
    {
        AudioManager.Instance.Play3DEffect(3002, transform.position);
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
        AudioManager.Instance.Play3DEffect(3003, transform.position);
        base.All_PlayBroken();
    }
    public override void All_UpdateInfo(string info)
    {
        gameTime_Sign = int.Parse(info);
        All_CompareTime();
        base.All_UpdateInfo(info);
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
        if (rockState_Now == RockState.Rock)
        {
            if (MapManager.Instance.mapNetManager.Object.HasStateAuthority)
            {
                Local_ChangeInfo((gameTime_Now).ToString()); 
                State_CreateLootItem(State_GetLootItem(baseLootInfos_State, extraLootInfos_State));
            }
        }
    }

    #endregion

}
