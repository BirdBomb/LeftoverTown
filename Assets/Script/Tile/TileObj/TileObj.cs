using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Random = UnityEngine.Random;
/// <summary>
/// 场景物品基类
/// </summary>
public class TileObj : MonoBehaviour
{
    public MyTile bindTile;
    [SerializeField, Header("最大生命值")]
    public int CurHp;
    [SerializeField, Header("当前生命值")]
    public int MaxHp;
    [SerializeField, Header("护甲")]
    public int Armor;
    [SerializeField, Header("掉落列表")]
    public List<LootInfo> LootList = new List<LootInfo>();
    [SerializeField, Header("掉落数量")]
    public int LootCount;
    [SerializeField, Header("地块信息")]
    public string info;
    #region//基本逻辑
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="json"></param>
    public virtual void Init(MyTile tile)
    {
        bindTile = tile;
    }
    /// <summary>
    /// 读取
    /// </summary>
    /// <param name="json"></param>
    public virtual void Load(string json)
    {
        Debug.Log(json);
        info = json;
    }
    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="json"></param>
    public virtual void Save(out string json)
    {
        json = info;
    }
    /// <summary>
    /// 更新
    /// </summary>
    public virtual void UpdateInfo(string json)
    {
        info = json;
    }
    /// <summary>
    /// 交互
    /// </summary>
    public virtual void Invoke()
    {

    }
    /// <summary>
    /// 玩家靠近
    /// </summary>
    /// <returns></returns>
    public virtual bool PlayerNearby(PlayerController player)
    {
        return false;
    }
    /// <summary>
    /// 远离
    /// </summary>
    /// <returns></returns>
    public virtual bool PlayerFaraway(PlayerController player)
    {
        return false;
    }
    /// <summary>
    /// 损坏
    /// </summary>
    /// <param name="val"></param>
    public virtual void Damaged(int val)
    {

    }
    /// <summary>
    /// 销毁
    /// </summary>
    public virtual void Break()
    {

    }
    /// <summary>
    /// 掉落
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
    /// 根据权重获得一个掉落物id
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
    /// 移除
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
    /// 时间更新
    /// </summary>
    public virtual void ListenTimeUpdate()
    {

    }
    /// <summary>
    /// 位置更新
    /// </summary>
    /// <param name="who"></param>
    /// <param name="where"></param>
    public virtual void ListenRoleMove(ActorManager who, MyTile where)
    {

    }
    #endregion
    #region//播放动画
    /// <summary>
    /// 受伤动画
    /// </summary>
    public virtual void PlayDamagedAnim()
    {

    }
    /// <summary>
    /// 销毁动画
    /// </summary>
    public virtual void PlayBreakAnim()
    {

    }
    #endregion
}
[Serializable]
public struct LootInfo
{
    [SerializeField,Header("编号")]
    public int ID;
    [SerializeField,Header("权重")]
    public int Weight;
}