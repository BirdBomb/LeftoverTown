using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Random = UnityEngine.Random;
using UnityEditor;
using DG.Tweening;
using System.Linq;
/// <summary>
/// ������Ʒ����
/// </summary>
public class TileObj : MonoBehaviour
{
    [HideInInspector]
    public MyTile bindTile;
    [SerializeField, Header("�������ֵ")]
    public int CurHp;
    [SerializeField, Header("��ǰ����ֵ")]
    public int MaxHp;
    [SerializeField, Header("�ؿ���Ϣ")]
    public string info;
    #region//��Ƭ��������
    public virtual void Start()
    {
        Init();
    }
    public virtual void OnDestroy()
    {

    }
    /// <summary>
    /// ��
    /// </summary>
    /// <param name="json"></param>
    public void Bind(MyTile tile)
    {
        bindTile = tile;
    }
    /// <summary>
    /// ��ʼ��
    /// </summary>
    public virtual void Init()
    {

    }
    /// <summary>
    /// ����
    /// </summary>
    public virtual void Draw(int seed)
    {

    }

    #endregion
    #region//��Ƭ�߼�
    /// <summary>
    /// ���ԶԵؿ�����˺�
    /// </summary>
    /// <param name="damage"></param>
    public void TryToTakeDamage(int damage)
    {
        if (damage > BuildingConfigData.GetBuildingConfig(bindTile.config_tileID).Building_Armor)
        {
            damage -= BuildingConfigData.GetBuildingConfig(bindTile.config_tileID).Building_Armor;
            Vector2 offset = 0.01f * new Vector2(new System.Random().Next(-10, 10), new System.Random().Next(-10, 10));
            GameObject obj_num = UIManager.Instance.ShowUI("UI/UI_DamageNum", (Vector2)transform.position + Vector2.up + offset);
            obj_num.GetComponent<UI_DamageNum>().Play((-damage).ToString(), Color.white, 48);

            MessageBroker.Default.Publish(new MapEvent.MapEvent_LocalTile_TakeDamage()
            {
                damage = damage
            });
        }
        else
        {
            Vector2 offset = 0.01f * new Vector2(new System.Random().Next(-10, 10), new System.Random().Next(-10, 10));
            GameObject obj_num = UIManager.Instance.ShowUI("UI/UI_DamageNum", (Vector2)transform.position + Vector2.up + offset);
            obj_num.GetComponent<UI_DamageNum>().Play("�˺�����", Color.gray, 24);
        }
    }
    /// <summary>
    /// ���Ը�������ֵ
    /// </summary>
    /// <param name="newHp"></param>
    public void TryToUpdateHp(int newHp)
    {
        if (newHp <= 0) { Broken(); }
        if (newHp < CurHp)
        {
            HpDown(CurHp - newHp);
        }
        else
        {
            HpUp(newHp - CurHp);
        }
        CurHp = newHp;
    }
    /// <summary>
    /// ����ֵ����
    /// </summary>
    /// <param name="val"></param>
    public virtual void HpUp(int val)
    {

    }
    /// <summary>
    /// ����ֵ�½�
    /// </summary>
    /// <param name="val"></param>
    public virtual void HpDown(int val)
    {
        PlayDamaged();
    }
    /// <summary>
    /// ��
    /// </summary>
    public virtual void Broken()
    {
        Loot();
        PlayBroken();
        MessageBroker.Default.Publish(new MapEvent.MapEvent_LocalTile_ChangeBuildingArea()
        {
            buildingID = 0,
            buildingPos = bindTile._posInCell,
            areaSize = BuildingConfigData.GetBuildingConfig(bindTile.config_tileID).Building_Size
        });
    }
    /// <summary>
    /// ���Ըı�ؿ���Ϣ
    /// </summary>
    public virtual void TryToChangeInfo(string info)
    {
    }
    /// <summary>
    /// ���Ը��µؿ���Ϣ
    /// </summary>
    public virtual void TryToUpdateInfo(string info)
    {
        this.info = info;
    }
    #endregion
    #region//��Ƭ����
    /// <summary>
    /// �������
    /// </summary>
    public virtual void ActorInputKeycode(ActorManager actor, KeyCode code)
    {

    }
    /// <summary>
    /// ��ɫ����
    /// </summary>
    /// <returns></returns>
    public virtual bool ActorNearby(ActorManager actor)
    {
        return false;
    }
    /// <summary>
    /// ��ɫվ��
    /// </summary>
    public virtual void ActorStandOn(ActorManager actor)
    {

    }
    /// <summary>
    /// ��ɫԶ��
    /// </summary>
    /// <returns></returns>
    public virtual bool ActorFaraway(ActorManager actor)
    {
        return false;
    }
    /// <summary>
    /// ��ҳ���
    /// </summary>
    /// <param name="player"></param>
    /// <returns>������</returns>
    public virtual bool PlayerHolding(PlayerCoreLocal player)
    {
        return false;
    }
    /// <summary>
    /// ����ͷ�
    /// </summary>
    /// <param name="player"></param>
    /// <returns>���ͷ�</returns>
    public virtual bool PlayerRelease(PlayerCoreLocal player)
    {
        return false;
    }

    #endregion
    #region//��Ƭ����
    [HideInInspector]
    public bool linkAlready = false;

    public virtual void CheckAroundBuilding(int id)
    {

    }
    /// <summary>
    /// ������Χ
    /// </summary>
    /// <param name="aroundState">��Χ״̬</param>
    public virtual void LinkAround(AroundState_EightSide aroundState)
    {

    }
    /// <summary>
    /// 
    /// </summary>
    public virtual void UpdateAround(int code)
    {

    }
    /// <summary>
    /// ������Χ
    /// </summary>
    /// <param name="aroundState">��Χ״̬</param>
    public virtual void LinkAround(AroundState_FourSide aroundState)
    {

    }
    #endregion
    #region//��Ƭ����
    /// <summary>
    /// ���˶���
    /// </summary>
    public virtual void PlayDamaged()
    {
        transform.DOPunchScale(new Vector3(0.2f, -0.1f, 0), 0.2f).SetEase(Ease.InOutBack);
    }
    /// <summary>
    /// ���ٶ���
    /// </summary>
    public virtual void PlayBroken()
    {
        GameObject effect = PoolManager.Instance.GetObject("Effect/Effect_BombSmoke");
        effect.transform.position = transform.position;
    }
    #endregion
    #region//��Ƭ����
    [SerializeField, Header("���������б�")]
    public List<BaseLootInfo> baseLoots = new List<BaseLootInfo>();
    [SerializeField, Header("��������б�")]
    public List<ExtraLootInfo> extraLoots = new List<ExtraLootInfo>();

    /// <summary>
    /// ����
    /// </summary>
    public virtual void Loot()
    {
        for (int i = 0; i < baseLoots.Count; i++)
        {
            Random.InitState(new System.Random().Next(0, 500));
            Vector3 offset = Random.insideUnitCircle.normalized * 0.25f;
            int count = new System.Random().Next(baseLoots[i].CountMin, baseLoots[i].CountMax + 1);
            MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnItem()
            {
                itemData = GetItemData(baseLoots[i].ID, (short)count),
                pos = transform.position - new Vector3(0, 0.1f, 0) + offset
            });
        }
        for (int i = 0; i < extraLoots.Count; i++)
        {
            Random.InitState(new System.Random().Next(0, 500));
            Vector3 offset = Random.insideUnitCircle.normalized * 0.25f;
            int random = new System.Random().Next(0, 1000);
            if (random <= extraLoots[i].Weight)
            {
                MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnItem()
                {
                    itemData = GetItemData(extraLoots[i].ID, extraLoots[i].Count),
                    pos = transform.position - new Vector3(0, 0.1f, 0)+ offset
                });
            }
        }
    }
    private ItemData GetItemData(short ID,short Count)
    {
        Type type = Type.GetType("Item_" + ID.ToString());
        ((ItemBase)Activator.CreateInstance(type)).StaticAction_InitData(ID, out ItemData initData);
        initData.Item_Count = Count;
        return initData;
    }
    #endregion
}
[Serializable]
public struct BaseLootInfo
{
    [SerializeField, Header("������������")]
    public short ID;
    [SerializeField, Header("��С����������")]
    public short CountMin;
    [SerializeField, Header("������������")]
    public short CountMax;
}
[Serializable]
public struct ExtraLootInfo
{
    [SerializeField, Header("�����������")]
    public short ID;
    [SerializeField, Header("�������������")]
    public short Count;
    [SerializeField, Header("���������Ȩ��(1/1000)")]
    public short Weight;
}
public enum LinkState
{
    FourSide,
    ThreeSide_RightMissing,
    ThreeSide_LeftMissing,
    TwoSide_UpDown,
    ThreeSide_DownMissing,
    TwoSide_UpLeft,
    TwoSide_UpRight,
    OneSide_Up,
    ThreeSide_UpMissing,
    TwoSide_DownLeft,
    TwoSide_DownRight,
    OneSide_Down,
    TwoSide_LeftRight,
    OneSide_Left,
    OneSide_Right,
    NoneSide,
}
public struct AroundState_EightSide
{
    public bool Left;
    public bool Right;
    public bool Up;
    public bool Down;
    public bool UpLeft;
    public bool UpRight;
    public bool DownLeft;
    public bool DownRight;
}
public struct AroundState_FourSide
{
    public bool Left;
    public bool Right;
    public bool Up;
    public bool Down;
}