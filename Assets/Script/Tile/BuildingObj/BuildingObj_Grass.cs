using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;

public class BuildingObj_Grass : BuildingObj
{
    public SpriteRenderer spriteRenderer_Grass;
    [Header("�Ͳ�")]
    public Sprite[] sprites_LowGrass;
    [Header("�߲�")]
    public Sprite[] sprites_HighGrass;
    [SerializeField, Header("��������(Сʱ)")]
    private int int_GrowthTime = 5;
    [SerializeField, Header("�߲�����ֵ")]
    private int int_HighGrassHp = 3;
    [SerializeField, Header("�Ͳ�����ֵ")]
    private int int_LowGrassHp = 10;

    public GrassState grassState;
    private int gameTime_Sign = -100;
    private int gameTime_Now;
    [SerializeField, Header("��̬����������")]
    private List<BaseLootInfo> baseLootInfos_HighGrass = new List<BaseLootInfo>();
    [SerializeField, Header("��̬���������")]
    private List<ExtraLootInfo> extraLootInfos_HighGrass = new List<ExtraLootInfo>();
    [SerializeField, Header("�Ͳݻ���������")]
    private List<BaseLootInfo> baseLootInfos_LowGrass = new List<BaseLootInfo>();
    [SerializeField, Header("�Ͳݶ��������")]
    private List<ExtraLootInfo> extraLootInfos_LowGrass = new List<ExtraLootInfo>();
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
    /// ����ʱ��
    /// </summary>
    /// <param name="hour"></param>
    /// <param name="date"></param>
    public void All_UpdateTime(int time)
    {
        gameTime_Now = time;
        All_CompareTime();
    }
    /// <summary>
    /// �Ա�ʱ��
    /// </summary>
    public void All_CompareTime()
    {
        if (grassState == GrassState.Low)
        {
            if (gameTime_Now - gameTime_Sign > int_GrowthTime)
            {
                All_UpdateGrassState(GrassState.High);
            }
        }
        else if (grassState == GrassState.High)
        {
            if (gameTime_Now - gameTime_Sign <= int_GrowthTime)
            {
                All_UpdateGrassState(GrassState.Low);
            }
        }
    }
    /// <summary>
    /// ������״̬
    /// </summary>
    /// <param name="type"></param>
    private void All_UpdateGrassState(GrassState type)
    {
        if (grassState != type)
        {
            transform.DOKill();
            transform.localScale = Vector3.one;
            transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0), 0.1f);
            grassState = type;
        }

        switch (type)
        {
            case GrassState.Low:
                hp = int_LowGrassHp;
                spriteRenderer_Grass.sprite = sprites_LowGrass[new System.Random().Next(0, sprites_LowGrass.Length)];
                break;
            case GrassState.High:
                hp = int_HighGrassHp;
                spriteRenderer_Grass.sprite = sprites_HighGrass[new System.Random().Next(0, sprites_HighGrass.Length)];
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
        if (grassState == GrassState.High)
        {
            if (MapManager.Instance.mapNetManager.Object.HasStateAuthority)
            {
                Local_ChangeInfo((gameTime_Now).ToString());
                State_CreateLootItem(State_GetLootItem(baseLootInfos_HighGrass, extraLootInfos_HighGrass));
            }
        }
        else if (grassState == GrassState.Low)
        {
            if (MapManager.Instance.mapNetManager.Object.HasStateAuthority)
            {
                State_CreateLootItem(State_GetLootItem(baseLootInfos_LowGrass, extraLootInfos_LowGrass));
            }
            base.All_Broken();
        }
    }

}
public enum GrassState
{
    Low,
    High
}