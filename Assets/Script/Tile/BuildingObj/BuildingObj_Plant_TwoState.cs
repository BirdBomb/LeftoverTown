using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class BuildingObj_Plant_TwoState : BuildingObj
{
    [HideInInspector]
    public State state_Now;
    public enum State
    {
        State0, State1
    }
    public SpriteRenderer spriteRenderer;
    private Material material;
    private Sequence sequence;
    [Header("Sprite幼苗")]
    public Sprite[] sprites_State0;
    [Header("Sprite成熟")]
    public Sprite[] sprites_State1;

    [Header("持续时间_幼苗")]
    public int int_TimeState0 = 1;
    [Header("持续时间_成熟")]
    public int int_TimeState1 = 1;
    [Header("生命值_幼苗")]
    public int int_HpState0 = 1;
    [Header("生命值_成熟")]
    public int int_HpState1 = 1;

    private int gameTime_Sign = int.MaxValue;
    private int gameTime_Now;

    [Header("基本掉落物_幼苗")]
    public List<BaseLootInfo> baseLootInfos_State0 = new List<BaseLootInfo>();
    [Header("基本掉落物_成熟")]
    public List<BaseLootInfo> baseLootInfos_State1 = new List<BaseLootInfo>();
    [Header("额外掉落物_成熟")]
    public List<ExtraLootInfo> extraLootInfos_State1 = new List<ExtraLootInfo>();

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
            else if (gameTime_Now - gameTime_Sign > int_TimeState0 + int_HpState1)
            {
                All_UpdateState(State.State1);
            }
        }
    }
    /// <summary>
    /// 更新状态
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
                hp = int_HpState0;
                AudioManager.Instance.Play3DEffect(3000, transform.position);
                spriteRenderer.sprite = sprites_State0[new System.Random().Next(0, sprites_State0.Length)];
                break;
            case State.State1:
                hp = int_HpState1;
                spriteRenderer.sprite = sprites_State1[new System.Random().Next(0, sprites_State1.Length)];
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
        if (state_Now == State.State0)
        {
            if (MapManager.Instance.mapNetManager.Object.HasStateAuthority)
            {
                State_CreateLootItem(State_GetLootItem(baseLootInfos_State0, null));
            }
            base.All_Broken();
        }
        else if (state_Now == State.State1)
        {
            if (MapManager.Instance.mapNetManager.Object.HasStateAuthority)
            {
                State_CreateLootItem(State_GetLootItem(baseLootInfos_State1, extraLootInfos_State1));
            }
            base.All_Broken();
        }
    }
    #endregion

}
