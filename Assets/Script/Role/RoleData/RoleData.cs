using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
[SerializeField]
public struct RoleData
{
    [Header("速度")]
    public float Data_Speed;
    [Header("死亡")]
    public bool Data_Dead;
    [Header("生命值")]
    public int Data_Hp;
    [Header("最大生命值")]
    public int Data_HpMax;
    [Header("饥饿值")]
    public int Data_Food;
    [Header("最大饥饿值")]
    public int Data_FoodMax;
    [Header("饥渴值")]
    public int Data_Water;
    [Header("最大饥渴值")]
    public int Data_WaterMax;

    [Header("警惕值")]
    public int Temp_Alert;
    [Header("手部持有")]
    public ItemConfig Holding_ByHand;
    [Header("背包持有")]
    public List<ItemConfig> Holding_BagList;
    [Header("掉落列表")]
    public List<LootInfo> Loot_List;
    [SerializeField, Header("掉落数量")]
    public int LootCount;

}
public enum RoleType
{
    
}
