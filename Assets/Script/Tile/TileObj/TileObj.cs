using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Random = UnityEngine.Random;
/// <summary>
/// ������Ʒ����
/// </summary>
public class TileObj : MonoBehaviour
{
    public MyTile bindTile;
    [SerializeField, Header("�������ֵ")]
    public int CurHp;
    [SerializeField, Header("��ǰ����ֵ")]
    public int MaxHp;
    [SerializeField, Header("����")]
    public int Armor;
    [SerializeField, Header("�����б�")]
    public List<LootInfo> LootList = new List<LootInfo>();
    [SerializeField, Header("��������")]
    public int LootCount;
    [SerializeField, Header("�ؿ���Ϣ")]
    public string info;
    private bool breaking = false;
    #region//�����߼�
    /// <summary>
    /// ��ʼ��
    /// </summary>
    /// <param name="json"></param>
    public virtual void Init(MyTile tile)
    {
        bindTile = tile;
    }
    /// <summary>
    /// ��ȡ
    /// </summary>
    /// <param name="json"></param>
    public virtual void Load(string json)
    {
        Debug.Log(json);
        info = json;
    }
    /// <summary>
    /// ����
    /// </summary>
    /// <param name="json"></param>
    public virtual void Save(out string json)
    {
        json = info;
    }

    /// <summary>
    /// ����
    /// </summary>
    public virtual void Invoke(PlayerController player)
    {

    }
    /// <summary>
    /// ���Ըı�����ֵ
    /// </summary>
    /// <param name="newHp"></param>
    public virtual void TryToChangeHp(int val)
    {
        MessageBroker.Default.Publish(new MapEvent.MapEvent_LocalTile_TakeDamage()
        {
            tileObj = this,
            damage = val
        });
    }
    /// <summary>
    /// ���Ը�������ֵ
    /// </summary>
    /// <param name="newHp"></param>
    public virtual void TryToUpdateHp(int newHp)
    {
        CurHp = newHp;
    }
    /// <summary>
    /// ���Ըı�ؿ���Ϣ
    /// </summary>
    public virtual void TryToChangeInfo(string info)
    {
        MessageBroker.Default.Publish(new MapEvent.MapEvent_LocalTile_UpdateBuildingInfo
        {
            tileObj = this,
            tileInfo = info
        });
    }
    /// <summary>
    /// ���Ը��µؿ���Ϣ
    /// </summary>
    public virtual void TryToUpdateInfo(string info)
    {
        this.info = info;
    }

    /// <summary>
    /// ��ҿ���
    /// </summary>
    /// <returns></returns>
    public virtual bool PlayerNearby(PlayerController player)
    {
        return false;
    }
    /// <summary>
    /// ���Զ��
    /// </summary>
    /// <returns></returns>
    public virtual bool PlayerFaraway(PlayerController player)
    {
        return false;
    }

    /// <summary>
    /// �����ƻ���������
    /// </summary>
    public virtual void ListenDestroyMyObj()
    {
        DestroyMyObj();
    }
    /// <summary>
    /// ����
    /// </summary>
    public virtual void DestroyMyObj()
    {
        Destroy(gameObject);
    }
    /// <summary>
    /// ����
    /// </summary>
    public virtual void Loot()
    {
        for (int i = 0; i < LootCount; i++)
        {
            ItemData item = GetNextLootItem();
            if (item.Item_ID != 0)
            {
                item.Item_Seed = System.DateTime.Now.Second + item.Item_Seed;

                MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnItem()
                {
                    itemData = item,
                    pos = transform.position - new Vector3(0, 0.1f, 0)
                });
            }
        }
    }
    /// <summary>
    /// ����Ȩ�ػ��һ��������id
    /// </summary>
    /// <returns></returns>
    private ItemData GetNextLootItem()
    {
        int weight_Main = 0;
        int weight_temp = 0;
        int random = 0;
        for (int j = 0; j < LootList.Count; j++)
        {
            weight_Main += LootList[j].Weight;
        }
        random = Random.Range(0, weight_Main);
        for (int j = 0; j < LootList.Count; j++)
        {
            weight_temp += LootList[j].Weight;
            if (weight_temp > random)
            {
                return LootList[j].Item;
            }
        }
        return new ItemData();
    }
    /// <summary>
    /// ʱ�����
    /// </summary>
    public virtual void ListenTimeUpdate()
    {

    }
    /// <summary>
    /// λ�ø���
    /// </summary>
    /// <param name="who"></param>
    /// <param name="where"></param>
    public virtual void ListenRoleMove(ActorManager who, MyTile where)
    {

    }
    #endregion
    #region//���Ŷ���
    /// <summary>
    /// ���˶���
    /// </summary>
    public virtual void PlayDamagedAnim()
    {

    }
    /// <summary>
    /// ���ٶ���
    /// </summary>
    public virtual void PlayBreakAnim()
    {

    }
    #endregion
}
[Serializable]
public struct LootInfo
{
    [SerializeField,Header("���")]
    public ItemData Item;
    [SerializeField,Header("Ȩ��")]
    public int Weight;
}