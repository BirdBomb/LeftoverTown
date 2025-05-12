using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UniRx;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class BuildingObj_Tree : BuildingObj
{
    public SpriteRenderer spriteRenderer_Tree;
    [Header("Sprite树桩")]
    public Sprite[] sprites_Stump;
    [Header("Sprite成年树")]
    public Sprite[] sprites_Tree;
    [SerializeField, Header("生长周期(小时)")]
    private int int_GrowthTime = 5;
    [SerializeField, Header("成年树生命值")]
    private int int_TreeHp = 10;
    [SerializeField, Header("树桩生命值")]
    private int int_StumpHp = 20;

    private TreeState treeState;
    private int gameTime_Sign = -100;
    private int gameTime_Now;
    [SerializeField, Header("常态基本掉落物")]
    private List<BaseLootInfo> baseLootInfos_Tree = new List<BaseLootInfo>();
    [SerializeField, Header("常态额外掉落物")]
    private List<ExtraLootInfo> extraLootInfos_Tree = new List<ExtraLootInfo>();
    [SerializeField, Header("树桩基本掉落物")]
    private List<BaseLootInfo> baseLootInfos_Stump = new List<BaseLootInfo>();
    [SerializeField, Header("树桩额外掉落物")]
    private List<ExtraLootInfo> extraLootInfos_Stump = new List<ExtraLootInfo>();
    public override void Start()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_All_UpdateHour>().Subscribe(_ =>
        {
            All_UpdateTime(_.hour + _.day * 10);
        }).AddTo(this);
        All_UpdateTime(MapManager.Instance.mapNetManager.Day * 10 + MapManager.Instance.mapNetManager.Hour);
        base.Start();
    }
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
        if (treeState == TreeState.Stump)
        {
            if (gameTime_Now - gameTime_Sign > int_GrowthTime)
            {
                All_UpdateTreeState(TreeState.Tree);
            }
        }
        else if (treeState == TreeState.Tree)
        {
            if (gameTime_Now - gameTime_Sign <= int_GrowthTime)
            {
                All_UpdateTreeState(TreeState.Stump);
            }
        }
    }
    /// <summary>
    /// 更新树状态
    /// </summary>
    /// <param name="type"></param>
    private void All_UpdateTreeState(TreeState type)
    {
        if (treeState != type)
        {
            transform.DOKill();
            transform.localScale = Vector3.one;
            transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0), 0.1f);
            treeState = type;
        }
        switch (type) 
        {
            case TreeState.Stump:
                hp = int_StumpHp;
                spriteRenderer_Tree.sprite = sprites_Stump[new System.Random().Next(0, sprites_Stump.Length)];
                break;
            case TreeState.Tree:
                hp = int_TreeHp;
                spriteRenderer_Tree.sprite = sprites_Tree[new System.Random().Next(0, sprites_Tree.Length)];
                break;
        }
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
        if (treeState == TreeState.Tree)
        {
            if (MapManager.Instance.mapNetManager.Object.HasStateAuthority)
            {
                Local_ChangeInfo((gameTime_Now).ToString());
                State_CreateLootItem(State_GetLootItem(baseLootInfos_Tree, extraLootInfos_Tree));
            }
        }
        else if (treeState == TreeState.Stump)
        {
            if (MapManager.Instance.mapNetManager.Object.HasStateAuthority)
            {
                State_CreateLootItem(State_GetLootItem(baseLootInfos_Stump, extraLootInfos_Stump));
            }
            base.All_Broken();
        }
    }
}
public enum TreeState
{
    Tree,
    Fruiter,
    Stump,
}