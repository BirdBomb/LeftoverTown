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
    public virtual void UpdateInfo(string json)
    {
        info = json;
    }
    /// <summary>
    /// ����
    /// </summary>
    public virtual void Invoke()
    {

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
    /// Զ��
    /// </summary>
    /// <returns></returns>
    public virtual bool PlayerFaraway(PlayerController player)
    {
        return false;
    }
    /// <summary>
    /// ��
    /// </summary>
    /// <param name="val"></param>
    public virtual void Damaged(int val)
    {

    }
    /// <summary>
    /// ����
    /// </summary>
    public virtual void Break()
    {

    }
    /// <summary>
    /// ����
    /// </summary>
    public virtual void Loot()
    {
        for (int i = 0; i < LootCount; i++)
        {
            int id = GetNextLootID();
            if (id != 0)
            {
                GameObject obj = Resources.Load<GameObject>("ItemObj/ItemObj");
                GameObject item = Instantiate(obj);
                item.transform.position = transform.position;
                item.GetComponent<ItemObj>().Init(ItemConfigData.GetItemConfig(id));
                item.GetComponent<ItemObj>().PlayDropAnim(transform.position);
            }
        }
    }
    /// <summary>
    /// ����Ȩ�ػ��һ��������id
    /// </summary>
    /// <returns></returns>
    private int GetNextLootID()
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
                return LootList[j].ID;
            }
        }
        return 0;
    }
    /// <summary>
    /// �Ƴ�
    /// </summary>
    public virtual void Remove()
    {
        MessageBroker.Default.Publish(new MapEvent.MapEvent_ChangeTile() 
        {
            tilePos = new Vector3Int( bindTile.x, bindTile.y, 0),
            tileName = "Default"
        });
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
    public int ID;
    [SerializeField,Header("Ȩ��")]
    public int Weight;
}